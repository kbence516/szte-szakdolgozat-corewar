using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace CoreWar {
    public sealed class VM : INotifyPropertyChanged {
        private List<string> _originalPlayers;
        public List<string> OriginalPlayers {
            get => _originalPlayers;
            set {
                if (_originalPlayers != value) {
                    _originalPlayers = value;
                    OnPropertyChanged(nameof(OriginalPlayers));
                }
            }
        }

        private Queue<Player> _players;
        public Queue<Player> Players {
            get => _players;
            private set {
                if (_players != value) {
                    _players = value;
                    OnPropertyChanged(nameof(Players));
                }
            }
        }
        private ObservableCollection<MemoryCell> _memory;
        public ObservableCollection<MemoryCell> Memory {
            get => _memory;
            private set {
                if (_memory != value) {
                    _memory = value;
                    OnPropertyChanged(nameof(Memory));
                }
            }
        }
        private int _memorySize;
        public int MemorySize {
            get => _memorySize;
            private set {
                if (_memorySize != value) {
                    _memorySize = value;
                    OnPropertyChanged(nameof(MemorySize));
                }
            }
        }
        private int _cycle;
        public int Cycle {
            get => _cycle;
            private set {
                if (_cycle != value) {
                    _cycle = value;
                    OnPropertyChanged(nameof(Cycle));
                }
            }
        }
        private int _maxCycles;
        public int MaxCycles {
            get => _maxCycles;
            private set {
                if (_maxCycles != value) {
                    _maxCycles = value;
                    OnPropertyChanged(nameof(MaxCycles));
                }
            }
        }
        private int _maxProcesses;
        public int MaxProcesses {
            get => _maxProcesses;
            private set {
                if (_maxProcesses != value) {
                    _maxProcesses = value;
                    OnPropertyChanged(nameof(MaxProcesses));
                }
            }
        }
        private int _warriors;
        public int Warriors {
            get => _warriors;
            private set {
                if (_warriors != value) {
                    _warriors = value;
                    OnPropertyChanged(nameof(Warriors));
                }
            }
        }
        private int _currentInstructionAddress;
        public int CurrentInstructionAddress {
            get => _currentInstructionAddress;
            private set {
                if (_currentInstructionAddress != value) {
                    _currentInstructionAddress = value;
                    OnPropertyChanged(nameof(CurrentInstructionAddress));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        private static VM instance;
        public static VM GetInstance(int memorySize = 8000, int maxCycles = 80000, int warriors = 2, int maxProcesses = 8000) {
            if (instance == null) {
                instance = new(memorySize, maxCycles, warriors, maxProcesses);
            }
            return instance;
        }

        public static void ResetInstance() {
            instance = null;
        }

        public int ModMemorySize(int value) {
            while (value < 0) {
                value += instance.MemorySize;
            }
            return value % instance.MemorySize;
        }

        private VM(int memorySize, int maxCycles, int warriors, int maxProcesses) {
            Players = new(warriors);
            if (memorySize <= 0) {
                throw new ArgumentException("Adj meg pozitív memóriaméretet!");
            }
            MemorySize = memorySize;
            Memory = new ObservableCollection<MemoryCell>(Enumerable.Range(0, MemorySize).Select(index => new MemoryCell(index, new Instruction())));
            MaxProcesses = maxProcesses;
            MaxCycles = maxCycles;
            Warriors = warriors;
            Cycle = 0;
            CurrentInstructionAddress = -1;
        }

        // visszatér az aktuális körben kiesett játékos nevével
        public string Play() {
            if (Cycle < MaxCycles) {
                Debug.WriteLine($"{Cycle}. kör");
                Player currentPlayer = Players.Dequeue();
                if (currentPlayer.Execute()) {
                    Players.Enqueue(currentPlayer);
                    Cycle++;
                    return "";
                } else {
                    Cycle++;
                    return currentPlayer.Name;
                }
            } else {
                throw new InvalidOperationException("Vége a játéknak!");
            }

                //    if (Players.Count == 1) {
                //        GameOver();
                //    }
                //    Cycle++;
                //}
                //if (Cycle == MaxCycles) {
                //    Tie();
                //}
            }

        public void LoadIntoMemory(List<Instruction> program, int start, string playerName) {
            for (int i = 0; i < program.Count; ++i) {
                Memory[(start + i) % Memory.Count].Instruction = program[i];
                Memory[(start + i) % Memory.Count].LastModifiedBy = playerName;
            }
        }

        public MemoryCell MemoryCellAt(int address, bool relative) {
            if (relative) {
                if (address + CurrentInstructionAddress < 0) {
                    address += MemorySize;
                }
                return Memory[ModMemorySize(address + CurrentInstructionAddress)];
            } else {
                return Memory[ModMemorySize(address)];
            }
        }

        public Instruction InstructionAt(int address, bool relative) {
            if (relative) {
                if (address + CurrentInstructionAddress < 0) {
                    address += MemorySize;
                }
                return Memory[ModMemorySize(address + CurrentInstructionAddress)].Instruction;
            } else {
                return Memory[ModMemorySize(address)].Instruction;
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
        public int[] ExecuteInstruction(int memoryAddress, string playerName = "player") {
            CurrentInstructionAddress = memoryAddress;
            Instruction currentInstruction = InstructionAt(memoryAddress, false);
            Instruction source = InstructionAt(currentInstruction.GetA(), true);
            Instruction target = InstructionAt(currentInstruction.GetB(), true);
            MemoryCell targetMemCell = MemoryCellAt(currentInstruction.GetB(), true);
            switch (currentInstruction.OpCode) {
                case OpCode.DAT:
                    return [-1];
                case OpCode.MOV:
                    targetMemCell.LastModifiedBy = playerName;
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            target.OpA.Copy(source.OpA);
                            break;
                        case OpModifier.B:
                            target.OpB.Copy(source.OpB);
                            break;
                        case OpModifier.AB:
                            target.OpB.Copy(source.OpA);
                            break;
                        case OpModifier.BA:
                            target.OpA.Copy(source.OpB);
                            break;
                        case OpModifier.F:
                            target.OpA.Value = source.OpA.Value;
                            target.OpB.Value = source.OpB.Value;
                            break;
                        case OpModifier.X:
                            target.OpB.Copy(source.OpA);
                            target.OpA.Copy(source.OpB);
                            break;
                        case OpModifier.I:
                            target.Copy(source);
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.ADD:
                    targetMemCell.LastModifiedBy = playerName;
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            target.OpA.Value = ModMemorySize(target.OpA.Value + source.OpA.Value);
                            break;
                        case OpModifier.B:
                            target.OpB.Value = ModMemorySize(target.OpB.Value + source.OpB.Value);
                            break;
                        case OpModifier.AB:
                            target.OpB.Value = ModMemorySize(target.OpB.Value + source.OpA.Value);
                            break;
                        case OpModifier.BA:
                            target.OpA.Value = ModMemorySize(target.OpA.Value + source.OpB.Value);
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            target.OpA.Value = ModMemorySize(target.OpA.Value + source.OpA.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value + source.OpB.Value);
                            break;
                        case OpModifier.X:
                            target.OpA.Value = ModMemorySize(target.OpA.Value + source.OpB.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value + source.OpA.Value);
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.SUB:
                    targetMemCell.LastModifiedBy = playerName;
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            target.OpA.Value = ModMemorySize(target.OpA.Value - source.OpA.Value);
                            break;
                        case OpModifier.B:
                            target.OpB.Value = ModMemorySize(target.OpB.Value - source.OpB.Value);
                            break;
                        case OpModifier.AB:
                            target.OpB.Value = ModMemorySize(target.OpB.Value - source.OpA.Value);
                            break;
                        case OpModifier.BA:
                            target.OpA.Value = ModMemorySize(target.OpA.Value - source.OpB.Value);
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            target.OpA.Value = ModMemorySize(target.OpA.Value - source.OpA.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value - source.OpB.Value);
                            break;
                        case OpModifier.X:
                            target.OpA.Value = ModMemorySize(target.OpA.Value - source.OpB.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value - source.OpA.Value);
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.MUL:
                    targetMemCell.LastModifiedBy = playerName;
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            target.OpA.Value = ModMemorySize(target.OpA.Value * source.OpA.Value);
                            break;
                        case OpModifier.B:
                            target.OpB.Value = ModMemorySize(target.OpB.Value * source.OpB.Value);
                            break;
                        case OpModifier.AB:
                            target.OpB.Value = ModMemorySize(target.OpB.Value * source.OpA.Value);
                            break;
                        case OpModifier.BA:
                            target.OpA.Value = ModMemorySize(target.OpA.Value * source.OpB.Value);
                            break;
                        case OpModifier.F:
                        case OpModifier.I:
                            target.OpA.Value = ModMemorySize(target.OpA.Value * source.OpA.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value * source.OpB.Value);
                            break;
                        case OpModifier.X:
                            target.OpA.Value = ModMemorySize(target.OpA.Value * source.OpB.Value);
                            target.OpB.Value = ModMemorySize(target.OpB.Value * source.OpA.Value);
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.DIV:
                    try {
                        targetMemCell.LastModifiedBy = playerName;
                        switch (currentInstruction.Modifier) {
                            case OpModifier.A:
                                target.OpA.Value = ModMemorySize(target.OpA.Value / source.OpA.Value);
                                break;
                            case OpModifier.B:
                                target.OpB.Value = ModMemorySize(target.OpB.Value / source.OpB.Value);
                                break;
                            case OpModifier.AB:
                                target.OpB.Value = ModMemorySize(target.OpB.Value / source.OpA.Value);
                                break;
                            case OpModifier.BA:
                                target.OpA.Value = ModMemorySize(target.OpA.Value / source.OpB.Value);
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                target.OpA.Value = ModMemorySize(target.OpA.Value / source.OpA.Value);
                                target.OpB.Value = ModMemorySize(target.OpB.Value / source.OpB.Value);
                                break;
                            case OpModifier.X:
                                target.OpA.Value = ModMemorySize(target.OpA.Value / source.OpB.Value);
                                target.OpB.Value = ModMemorySize(target.OpB.Value / source.OpA.Value);
                                break;
                            default:
                                throw new ArgumentException("Helytelen módosító");
                        }
                        return [ModMemorySize(++memoryAddress)];
                    } catch (DivideByZeroException) {
                        return [-1];
                    }
                case OpCode.MOD:
                    try {
                        targetMemCell.LastModifiedBy = playerName;
                        switch (currentInstruction.Modifier) {
                            case OpModifier.A:
                                target.OpA.Value = ModMemorySize(target.OpA.Value % source.OpA.Value);
                                break;
                            case OpModifier.B:
                                target.OpB.Value = ModMemorySize(target.OpB.Value % source.OpB.Value);
                                break;
                            case OpModifier.AB:
                                target.OpB.Value = ModMemorySize(target.OpB.Value % source.OpA.Value);
                                break;
                            case OpModifier.BA:
                                target.OpA.Value = ModMemorySize(target.OpA.Value % source.OpB.Value);
                                break;
                            case OpModifier.F:
                            case OpModifier.I:
                                target.OpA.Value = ModMemorySize(target.OpA.Value % source.OpA.Value);
                                target.OpB.Value = ModMemorySize(target.OpB.Value % source.OpB.Value);
                                break;
                            case OpModifier.X:
                                target.OpA.Value = ModMemorySize(target.OpA.Value % source.OpB.Value);
                                target.OpB.Value = ModMemorySize(target.OpB.Value % source.OpA.Value);
                                break;
                            default:
                                throw new ArgumentException("Helytelen módosító");
                        }
                        return [ModMemorySize(++memoryAddress)];
                    } catch (DivideByZeroException) {
                        return [-1];
                    }
                case OpCode.JMP:
                    return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                case OpCode.JMZ:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (target.OpA.Value == 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (target.OpB.Value == 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (target.OpA.Value == 0 && target.OpB.Value == 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.JMN:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (target.OpA.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (target.OpB.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            if (target.OpA.Value != 0 || target.OpB.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.DJN:
                    targetMemCell.LastModifiedBy = playerName;
                    switch (currentInstruction.Modifier) {
                        case OpModifier.BA:
                        case OpModifier.A:
                            if (--target.OpA.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.AB:
                        case OpModifier.B:
                            if (--target.OpB.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.X:
                        case OpModifier.F:
                            // a lusta kiértékelést elkerülendő, így biztosan csökken mindkét érték
                            --target.OpA.Value;
                            --target.OpB.Value;
                            if (target.OpA.Value != 0 || target.OpB.Value != 0) {
                                return [ModMemorySize(memoryAddress + currentInstruction.GetA())];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.SPL:
                    return [ModMemorySize(memoryAddress + 1), ModMemorySize(memoryAddress + currentInstruction.GetA())];
                case OpCode.CMP:
                case OpCode.SEQ:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (source.OpA.Value == target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.B:
                            if (source.OpB.Value == target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.AB:
                            if (source.OpA.Value == target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.BA:
                            if (source.OpB.Value == target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.F:
                            if (source.OpA.Value == target.OpA.Value && source.OpB.Value == target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.I:
                            if (source.Equals(target)) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.X:
                            if (source.OpA.Value == target.OpB.Value && source.OpB.Value == target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.SNE:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (source.OpA.Value != target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.B:
                            if (source.OpB.Value != target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.AB:
                            if (source.OpA.Value != target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.BA:
                            if (source.OpB.Value != target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.F:
                            if (source.OpA.Value != target.OpA.Value || source.OpB.Value != target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.I:
                            if (!source.Equals(target)) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.X:
                            if (source.OpA.Value != target.OpB.Value || source.OpB.Value != target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.SLT:
                    switch (currentInstruction.Modifier) {
                        case OpModifier.A:
                            if (source.OpA.Value < target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.B:
                            if (source.OpB.Value < target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.AB:
                            if (source.OpA.Value < target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.BA:
                            if (source.OpB.Value < target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.I:
                        case OpModifier.F:
                            if (source.OpA.Value < target.OpA.Value && source.OpB.Value < target.OpB.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        case OpModifier.X:
                            if (source.OpA.Value < target.OpB.Value && source.OpB.Value < target.OpA.Value) {
                                return [ModMemorySize(memoryAddress + 2)];
                            }
                            break;
                        default:
                            throw new ArgumentException("Helytelen módosító");
                    }
                    return [ModMemorySize(++memoryAddress)];
                case OpCode.NOP:
                    return [ModMemorySize(++memoryAddress)];
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
            Debug.WriteLine("Döntetlen!");
            //Console.ReadLine();
            Environment.Exit(0);
        }

        public override string ToString() {
            string output = "A memória jelenlegi állapota:\n";
            for (int i = 0; i < Memory.Count; ++i) {
                if (!Memory[i].Equals(new Instruction())) {
                    output += $"#{i}:\t{Memory[i]}{(i == CurrentInstructionAddress ? "\t\t<< lefutott utasítás" : "")}\n";
                }
            }
            return output;
        }
    }
}