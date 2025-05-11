using Antlr4.Runtime;

namespace CoreWar {
    public class Player {
        Random random = new Random();
        VM vm = VM.GetInstance();

        public Queue<int> Processes {       // Kezdetben a kezdőutasítások sorszámai
            get; private set;
        }

        public string Name {
            get; private set;
        }

        public Player(string programPath) {
            Processes = new();
            Name = programPath[(programPath.LastIndexOf('/') + 1)..programPath.LastIndexOf('.')];       // programPath jelenleg valahogy úgy néz ki, hogy ../../../path.txt

            var inputStream = new AntlrInputStream(File.ReadAllText(programPath));
            var lexer = new RedcodeLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RedcodeParser(tokenStream);
            var context = parser.program();
            var visitor = new RedcodeVisitor();

            var (process, firstInstructionOffset) = ((List<Instruction>, int))visitor.VisitProgram(context);
            int firstInstructionStart = random.Next(vm.Memory.Length) + firstInstructionOffset;          // TODO: leellenőrizni, hogy van-e már ott valami a memóriában
            vm.LoadIntoMemory(process, firstInstructionStart);
            Processes.Enqueue(firstInstructionStart);
            Console.WriteLine($"{Name} betöltve a memóriába, kezdőcím: {firstInstructionStart}\n");
            vm.Players.Add(this);
        }

        public void Execute() {
            vm.CurrentInstruction = Processes.Dequeue();
            Instruction instruction = vm.InstructionAt(vm.CurrentInstruction, false);
            bool processDies = false;
            switch (instruction.OpCode) {
                case OpCode.DAT:
                    processDies = true;
                    break;
                case OpCode.MOV:
                    Instruction instr = vm.InstructionAt(instruction.GetA(), true);
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = instr.OpA.Value;
                            break;
                        case OpModifier.B:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = instr.OpB.Value;
                            break;
                        case OpModifier.AB:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = instr.OpA.Value;
                            break;
                        case OpModifier.BA:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = instr.OpB.Value;
                            break;
                        case OpModifier.F:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = instr.OpA.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = instr.OpB.Value;
                            break;
                        case OpModifier.X:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = instr.OpB.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = instr.OpA.Value;
                            break;
                        case OpModifier.I:
                            vm.InstructionAt(instruction.GetB(), true).Copy(instr);
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    vm.CurrentInstruction++;
                    break;
                case OpCode.ADD:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value += vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value += vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value += vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.BA:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value += vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value += vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value += vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value += vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value += vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    vm.CurrentInstruction++;
                    break;
                case OpCode.SUB:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value - vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value - vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value - vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.BA:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value - vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value - vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value - vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value - vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value - vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    vm.CurrentInstruction++;
                    break;
                case OpCode.MUL:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value *= vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value *= vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value *= vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.BA:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value *= vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value *= vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value *= vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            vm.InstructionAt(instruction.GetB(), true).OpA.Value *= vm.InstructionAt(instruction.GetA(), true).OpB.Value;
                            vm.InstructionAt(instruction.GetB(), true).OpB.Value *= vm.InstructionAt(instruction.GetA(), true).OpA.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    vm.CurrentInstruction++;
                    break;
                case OpCode.DIV:
                    try {
                        switch (instruction.Modifier) {
                            case OpModifier.A:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value / vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.B:
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value / vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.AB:
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value / vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.BA:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value / vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value / vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value / vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.X:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value / vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value / vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            default:
                                throw new Exception("Helytelen módosító");
                        }
                        vm.CurrentInstruction++;
                    } catch (DivideByZeroException) {
                        processDies = true;
                    }
                    break;
                case OpCode.MOD:
                    try {
                        switch (instruction.Modifier) {
                            case OpModifier.A:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value % vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.B:
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value % vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.AB:
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value % vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.BA:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value % vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value % vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value % vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.X:
                                vm.InstructionAt(instruction.GetB(), true).OpA.Value = vm.InstructionAt(instruction.GetA(), true).OpB.Value % vm.InstructionAt(instruction.GetB(), true).OpA.Value;
                                vm.InstructionAt(instruction.GetB(), true).OpB.Value = vm.InstructionAt(instruction.GetA(), true).OpA.Value % vm.InstructionAt(instruction.GetB(), true).OpB.Value;
                                break;
                            default:
                                throw new Exception("Helytelen módosító");
                        }
                        vm.CurrentInstruction++;
                    } catch (DivideByZeroException) {
                        processDies = true;
                    }
                    break;
                case OpCode.JMP:
                    vm.CurrentInstruction += instruction.GetA();
                    break;
                case OpCode.JMZ:
                    switch (instruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (vm.InstructionAt(instruction.GetB(), true).OpA.Value == 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (vm.InstructionAt(instruction.GetB(), true).OpB.Value == 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (vm.InstructionAt(instruction.GetB(), true).OpA.Value == 0 && vm.InstructionAt(instruction.GetB(), true).OpB.Value == 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.JMN:
                    switch (instruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (vm.InstructionAt(instruction.GetB(), true).OpA.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (vm.InstructionAt(instruction.GetB(), true).OpB.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (vm.InstructionAt(instruction.GetB(), true).OpA.Value != 0 && vm.InstructionAt(instruction.GetB(), true).OpB.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.DJN:
                    switch (instruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (--vm.InstructionAt(instruction.GetB(), true).OpA.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (--vm.InstructionAt(instruction.GetB(), true).OpB.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (--vm.InstructionAt(instruction.GetB(), true).OpA.Value != 0 && --vm.InstructionAt(instruction.GetB(), true).OpB.Value != 0) {
                                vm.CurrentInstruction += instruction.GetA();
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.SPL:
                    Processes.Enqueue(vm.CurrentInstruction + instruction.GetA());
                    vm.CurrentInstruction += instruction.GetA();
                    break;
                case OpCode.CMP:
                case OpCode.SEQ:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value == vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.B:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value == vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.AB:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value == vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.BA:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value == vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            if (vm.InstructionAt(instruction.GetA(), true).Equals(vm.InstructionAt(instruction.GetB(), true).OpA.Value)) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.X:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value == vm.InstructionAt(instruction.GetB(), true).OpB.Value &&
                                vm.InstructionAt(instruction.GetA(), true).OpB.Value == vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.SNE:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value != vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.B:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value != vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.AB:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value != vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.BA:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value != vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            if (!vm.InstructionAt(instruction.GetA(), true).Equals(vm.InstructionAt(instruction.GetB(), true).OpA.Value)) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.X:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value != vm.InstructionAt(instruction.GetB(), true).OpB.Value &&
                                vm.InstructionAt(instruction.GetA(), true).OpB.Value != vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.SLT:
                    switch (instruction.Modifier) {
                        case OpModifier.A:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value < vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.B:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value < vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.AB:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value < vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.BA:
                            if (vm.InstructionAt(instruction.GetA(), true).OpB.Value < vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.F:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value < vm.InstructionAt(instruction.GetB(), true).OpA.Value &&
                                vm.InstructionAt(instruction.GetA(), true).OpB.Value < vm.InstructionAt(instruction.GetB(), true).OpB.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        case OpModifier.X:
                            if (vm.InstructionAt(instruction.GetA(), true).OpA.Value < vm.InstructionAt(instruction.GetB(), true).OpB.Value &&
                                vm.InstructionAt(instruction.GetA(), true).OpB.Value < vm.InstructionAt(instruction.GetB(), true).OpA.Value) {
                                vm.CurrentInstruction += 2;
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    break;
                case OpCode.NOP:
                    vm.CurrentInstruction++;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (!processDies) {                                             // Törlünk a listából, tehát nem kell léptetni, ha elveszítünk egy processzust
                Processes.Enqueue(vm.CurrentInstruction++);
            }
            if (Processes.Count == 0) {
                Lose();
            }

        }

        public void Lose() {
            Console.WriteLine($"{Name} vesztett!");
            vm.Players.Remove(this);
        }
    }
}