namespace CoreWar.models {
    public class IncompleteInstruction {

        public Instruction? Instruction {
            get; private set;
        }

        public char WrongOperand {
            get; private set;
        }
        public string Label {
            get; private set;
        }

        public int? LineNumber {
            get; set;
        }

        public IncompleteInstruction(Instruction? instruction, char wrongOperand, string label, int? lineNumber) {
            Instruction = instruction;
            WrongOperand = wrongOperand;
            Label = label;
            LineNumber = lineNumber;
        }

        public override bool Equals(object? obj) {
            if (obj is Instruction instruction) {
                if (WrongOperand == 'A') {
                    return Instruction?.OpCode == instruction.OpCode &&
                           Instruction.Modifier == instruction.Modifier &&
                           Instruction.OpA.Mode == instruction.OpA.Mode &&
                           instruction.OpA.Value == 0;
                }
                if (WrongOperand == 'B') {
                    return Instruction?.OpCode == instruction.OpCode &&
                           Instruction.Modifier == instruction.Modifier &&
                           Instruction.OpB.Mode == instruction.OpB.Mode &&
                           instruction.OpB.Value == 0;
                }
            }
            return obj is IncompleteInstruction incompleteInstruction &&
                Instruction != null &&
                Instruction.Equals(incompleteInstruction.Instruction) &&
                WrongOperand == incompleteInstruction.WrongOperand &&
                Label == incompleteInstruction.Label &&
                LineNumber == incompleteInstruction.LineNumber;
        }
    }
}
