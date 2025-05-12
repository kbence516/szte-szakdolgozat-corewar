namespace CoreWar {
    public sealed class VM {
        public Queue<Player> Players { get; private set; }
        public Instruction[] Memory { get; private set; }
        public int MemorySize { get; private set; }
        public int Cycle { get; private set; }
        public int MaxCycles { get; private set; }
        public int MaxProcesses { get; private set; }
        public int Warriors { get; private set; }
        public int CurrentInstructionAddress { get; private set; }

        private static VM instance;
        public static VM GetInstance(int memorySize = 8000, int maxCycles = 80000, int warriors = 2, int maxProcesses = 8000) {
            if (instance == null) {
                instance = new(memorySize, maxCycles, warriors, maxProcesses);
            }
            return instance;
        }

        private VM(int memorySize, int maxCycles, int warriors, int maxProcesses) {
            Players = new(warriors);
            MemorySize = memorySize;
            Memory = Enumerable.Range(0, MemorySize).Select(_ => new Instruction()).ToArray();
            MaxProcesses = maxProcesses;
            MaxCycles = maxCycles;
            Warriors = warriors;
            Cycle = 0;
            CurrentInstructionAddress = -1;
        }

        public void Play() {
            while (Cycle < MaxCycles) {
                Console.WriteLine($"{Cycle}. kör\n{ToString()}");
                Console.ReadLine();

                Player currentPlayer = Players.Dequeue();
                if (currentPlayer.Execute()) {
                    Players.Enqueue(currentPlayer);
                } else {
                    Console.WriteLine($"{currentPlayer.Name} vesztett!");
                }

                if (Players.Count == 1) {
                    GameOver();
                }
                Cycle++;
            }
            if (Cycle == MaxCycles) {
                Tie();
            }
        }

        public void LoadIntoMemory(List<Instruction> program, int start) {
            for (int i = 0; i < program.Count; ++i) {
                Memory[(start + i) % Memory.Length] = program[i];
            }
        }

        public Instruction InstructionAt(int address, bool relative) {
            if (relative) {
                if (address + CurrentInstructionAddress < 0) {
                    address += MemorySize;
                }
                return Memory[(address + CurrentInstructionAddress) % MemorySize];
            } else {
                return Memory[address % MemorySize];
            }
        }

        /// <summary>
        /// Executes the instruction at the specified memory address.
        /// </summary>
        /// <returns>
        /// The array of integers representing the next memory addresses to execute. 
        /// If the instruction results in termination, the array contains -1.
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int[] ExecuteInstruction(int memoryAddress) {
            CurrentInstructionAddress = memoryAddress;
            Instruction currentInstruction = InstructionAt(memoryAddress, false);
            switch (currentInstruction.OpCode) {
                case OpCode.DAT:
                    return [-1];
                case OpCode.MOV:
                    Instruction targetInstruction = InstructionAt(currentInstruction.GetA(), true);
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = targetInstruction.OpA.Value;
                            break;
                        case OpModifier.B:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = targetInstruction.OpB.Value;
                            break;
                        case OpModifier.AB:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = targetInstruction.OpA.Value;
                            break;
                        case OpModifier.BA:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = targetInstruction.OpB.Value;
                            break;
                        case OpModifier.F:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = targetInstruction.OpA.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = targetInstruction.OpB.Value;
                            break;
                        case OpModifier.X:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = targetInstruction.OpB.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = targetInstruction.OpA.Value;
                            break;
                        case OpModifier.I:
                            InstructionAt(currentInstruction.GetB(), true).Copy(targetInstruction);
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.ADD:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value += InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value += InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value += InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.BA:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value += InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value += InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value += InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value += InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value += InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.SUB:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value - InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value - InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value - InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.BA:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value - InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value - InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value - InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value - InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value - InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.MUL:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value *= InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.B:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value *= InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.AB:
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value *= InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        case OpModifier.BA:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value *= InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value *= InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value *= InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            break;
                        case OpModifier.X:
                            InstructionAt(currentInstruction.GetB(), true).OpA.Value *= InstructionAt(currentInstruction.GetA(), true).OpB.Value;
                            InstructionAt(currentInstruction.GetB(), true).OpB.Value *= InstructionAt(currentInstruction.GetA(), true).OpA.Value;
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.DIV:
                    try {
                        switch (currentInstruction.Modifier) {
                            case OpModifier.A:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value / InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.B:
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value / InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.AB:
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value / InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.BA:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value / InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value / InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value / InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.X:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value / InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value / InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            default:
                                throw new Exception("Helytelen módosító");
                        }
                        return [++memoryAddress];
                    } catch (DivideByZeroException) {
                        return [-1];
                    }
                case OpCode.MOD:
                    try {
                        switch (currentInstruction.Modifier) {
                            case OpModifier.A:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value % InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.B:
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value % InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.AB:
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value % InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.BA:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value % InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value % InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value % InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            case OpModifier.X:
                                InstructionAt(currentInstruction.GetB(), true).OpA.Value = InstructionAt(currentInstruction.GetA(), true).OpB.Value % InstructionAt(currentInstruction.GetB(), true).OpA.Value;
                                InstructionAt(currentInstruction.GetB(), true).OpB.Value = InstructionAt(currentInstruction.GetA(), true).OpA.Value % InstructionAt(currentInstruction.GetB(), true).OpB.Value;
                                break;
                            default:
                                throw new Exception("Helytelen módosító");
                        }
                        return [++memoryAddress];
                    } catch (DivideByZeroException) {
                        return [-1];
                    }
                case OpCode.JMP:
                    return [memoryAddress + currentInstruction.GetA()];
                case OpCode.JMZ:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (InstructionAt(currentInstruction.GetB(), true).OpA.Value == 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (InstructionAt(currentInstruction.GetB(), true).OpB.Value == 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (InstructionAt(currentInstruction.GetB(), true).OpA.Value == 0 && InstructionAt(currentInstruction.GetB(), true).OpB.Value == 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.JMN:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (InstructionAt(currentInstruction.GetB(), true).OpA.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (InstructionAt(currentInstruction.GetB(), true).OpB.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (InstructionAt(currentInstruction.GetB(), true).OpA.Value != 0 && InstructionAt(currentInstruction.GetB(), true).OpB.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.DJN:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (--InstructionAt(currentInstruction.GetB(), true).OpA.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (--InstructionAt(currentInstruction.GetB(), true).OpB.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (--InstructionAt(currentInstruction.GetB(), true).OpA.Value != 0 && --InstructionAt(currentInstruction.GetB(), true).OpB.Value != 0) {
                                return [memoryAddress + currentInstruction.GetA()];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.SPL:
                    return [memoryAddress + currentInstruction.GetA(), memoryAddress + 1];
                case OpCode.CMP:
                case OpCode.SEQ:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value == InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.B:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value == InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.AB:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value == InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.BA:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value == InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            if (InstructionAt(currentInstruction.GetA(), true).Equals(InstructionAt(currentInstruction.GetB(), true).OpA.Value)) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.X:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value == InstructionAt(currentInstruction.GetB(), true).OpB.Value && InstructionAt(currentInstruction.GetA(), true).OpB.Value == InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.SNE:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value != InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.B:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value != InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.AB:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value != InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.BA:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value != InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            if (!InstructionAt(currentInstruction.GetA(), true).Equals(InstructionAt(currentInstruction.GetB(), true).OpA.Value)) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.X:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value != InstructionAt(currentInstruction.GetB(), true).OpB.Value && InstructionAt(currentInstruction.GetA(), true).OpB.Value != InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.SLT:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value < InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.B:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value < InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.AB:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value < InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.BA:
                            if (InstructionAt(currentInstruction.GetA(), true).OpB.Value < InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.F:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value < InstructionAt(currentInstruction.GetB(), true).OpA.Value && InstructionAt(currentInstruction.GetA(), true).OpB.Value < InstructionAt(currentInstruction.GetB(), true).OpB.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        case OpModifier.X:
                            if (InstructionAt(currentInstruction.GetA(), true).OpA.Value < InstructionAt(currentInstruction.GetB(), true).OpB.Value && InstructionAt(currentInstruction.GetA(), true).OpB.Value < InstructionAt(currentInstruction.GetB(), true).OpA.Value) {
                                return [memoryAddress + 2];
                            }
                            break;
                        default:
                            throw new Exception("Helytelen módosító");
                    }
                    return [++memoryAddress];
                case OpCode.NOP:
                    return [++memoryAddress];
                default:
                    throw new InvalidOperationException();
            }
            throw new InvalidOperationException();
        }

        public void GameOver() {
            Console.WriteLine($"Vége a játéknak, {Players.Dequeue().Name} nyert!");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public void Tie() {
            Console.WriteLine("Döntetlen!");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public override string ToString() {
            string output = "A memória jelenlegi állapota:\n";
            for (int i = 0; i < Memory.Length; ++i) {
                if (!Memory[i].Equals(new Instruction())) {
                    output += $"#{i}:\t{Memory[i]}{(i == CurrentInstructionAddress ? "\t\t<< lefutott utasítás" : "")}\n";
                }
            }
            return output;
        }
    }
}