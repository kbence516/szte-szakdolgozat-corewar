using Antlr4.Runtime.Misc;

namespace CoreWar {
    public class RedcodeVisitor : RedcodeBaseVisitor<object> {
        Dictionary<string, int> labels = [];                         // címkék neve és sorsz�ma
        List<IncompleteInstruction> incompleteInstructions = [];     // hibás utasítások
        IncompleteInstruction? orgInstruction = null;
        int processStartOffset = 0, programLineNumber = 0;
        bool reachedEnd = false, lookingForFirstInstr = false;
        public override object VisitProgram([NotNull] RedcodeParser.ProgramContext context) {
            List<Instruction> process = [];                         // visszatérési érték első paramétere
            for (programLineNumber = 0; programLineNumber < context.line().Length && !reachedEnd; ++programLineNumber) {
                if (context.line()[programLineNumber].GetText() == "\r\n" || context.line()[programLineNumber].comment() != null) {               // üres sor vagy komment a sor elején
                    continue;
                }
                Instruction? instruction = VisitInstruction(context.line()[programLineNumber].instruction()) as Instruction;
                if (instruction == null) {
                    continue;
                }
                process.Add(instruction);
            }

            if (orgInstruction != null) {
                if (labels.ContainsKey(orgInstruction.Label)) {
                    processStartOffset = labels[orgInstruction.Label] - (int)orgInstruction.LineNumber;
                } else {
                    throw new Exception($"Hiba: nincs {orgInstruction.Label} címke");
                }
            }

            foreach (IncompleteInstruction incInstr in incompleteInstructions) {
                for (int i = 0; i < process.Count; ++i) {
                    if (incInstr.Equals(process[i])) {
                        if (!labels.ContainsKey(incInstr.Label)) {
                            throw new Exception($"Hiba: nincs {incInstr.Label} címke");
                        }
                        if (incInstr.WrongOperand == 'A') {
                            process[i].OpA.Value = labels[incInstr.Label] - (int)incInstr.LineNumber;
                            break;
                        } else if (incInstr.WrongOperand == 'B') {
                            process[i].OpB.Value = labels[incInstr.Label] - (int)incInstr.LineNumber;
                            break;
                        }
                    }
                }
            }

            return (process, processStartOffset);
        }

        public override object VisitInstruction([NotNull] RedcodeParser.InstructionContext context) {
            if (context.label() != null) {
                labels.Add(context.label().GetText().TrimEnd(':'), programLineNumber);
            }

            OpCode opcode = (OpCode)Enum.Parse(typeof(OpCode), context.operation().opcode().GetText().ToUpper());

            if (opcode == OpCode.ORG) {
                // nincs szükség a konkrét Instruction-re, a lineNumbert meg az első valid utasításnál adjuk meg
                orgInstruction = new IncompleteInstruction(null, 'A', context.exprA().GetText(), null);         
                lookingForFirstInstr = true;                                                                    
                return null;
            }
            if (opcode == OpCode.END) {
                reachedEnd = true;
                return null;
            }
            if (opcode == OpCode.EQU) {
                labels.Add(context.label().GetText(), int.Parse(context.exprA().GetText()));
                return null;
            }
            if (lookingForFirstInstr) {
                orgInstruction.LineNumber = programLineNumber;
                lookingForFirstInstr = false;
            }
            OpModifier? modifier = null;
            var modifierContext = context.operation().modifier();
            if (modifierContext != null) {
                modifier = (OpModifier)Enum.Parse(typeof(OpModifier), context.operation().modifier().GetText().ToUpper());
            }

            AddressingMode? adA = null;
            var adAContext = context.adA();
            if (adAContext != null) {
                adA = (AddressingMode)adAContext.GetText()[0];
            }
            int valueA = 0;
            if (adA == null) {
                adA = AddressingMode.DIRECT;
            }
            AddressingMode? adB = null;
            var adBContext = context.adB();
            if (adBContext != null) {
                adB = (AddressingMode)adBContext.GetText()[0];
            }

            if (modifier == null) {
                switch (opcode) {
                    case OpCode.DAT:
                    case OpCode.NOP:
                        modifier = OpModifier.F;
                        break;
                    case OpCode.MOV:
                    case OpCode.CMP:
                    case OpCode.SEQ:
                    case OpCode.SNE:
                        if (adA == AddressingMode.IMMEDIATE) {
                            modifier = OpModifier.AB;
                        } else if (adB == AddressingMode.IMMEDIATE) {
                            modifier = OpModifier.B;
                        } else {
                            modifier = OpModifier.I;
                        }
                        break;
                    case OpCode.ADD:
                    case OpCode.SUB:
                    case OpCode.MUL:
                    case OpCode.DIV:
                    case OpCode.MOD:
                        if (adA == AddressingMode.IMMEDIATE) {
                            modifier = OpModifier.AB;
                        } else if (adB == AddressingMode.IMMEDIATE) {
                            modifier = OpModifier.B;
                        } else {
                            modifier = OpModifier.F;
                        }
                        break;
                    case OpCode.JMP:
                    case OpCode.JMZ:
                    case OpCode.JMN:
                    case OpCode.DJN:
                    case OpCode.SPL:
                        modifier = OpModifier.B;
                        break;
                    case OpCode.SLT:
                        if (adA == AddressingMode.IMMEDIATE) {
                            modifier = OpModifier.AB;
                        } else {
                            modifier = OpModifier.B;
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            try {
                valueA = int.Parse(context.exprA().GetText());
            } catch (FormatException) {
                valueA = 0;
                IncompleteInstruction incInstr = new IncompleteInstruction(
                    new Instruction(opcode, (OpModifier)modifier, new Operation((AddressingMode)adA, valueA), null),
                    'A',
                    context.exprA().GetText(),
                    programLineNumber
                );
                incompleteInstructions.Add(incInstr);
            }

            int? valueB = null;
            var valueBContext = context.exprB();
            if (valueBContext != null) {
                try {
                    valueB = int.Parse(valueBContext.GetText());
                } catch (FormatException) {
                    valueB = 0;
                    IncompleteInstruction incInstr = new IncompleteInstruction(
                        new Instruction(opcode, (OpModifier)modifier, new Operation((AddressingMode)adA, valueA), new Operation(adB ?? AddressingMode.DIRECT, (int)valueB)),
                        'B',
                        context.exprB().GetText(),
                        programLineNumber
                    );
                    incompleteInstructions.Add(incInstr);
                }
            }

            if (valueB != null && adB == null) {
                adB = AddressingMode.DIRECT;
            }

            Instruction instruction = new Instruction(
                opcode,
                (OpModifier)modifier,
                new Operation((AddressingMode)adA, valueA),
                adB != null ? new Operation((AddressingMode)adB, valueB ?? 0) : new Operation(AddressingMode.DIRECT, 0)
                );

            return instruction;
        }
    }
}