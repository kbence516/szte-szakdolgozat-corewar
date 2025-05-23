using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace CoreWar {
    /// <summary>
    /// A virtuális memóriát reprezentáló singleton osztály
    /// </summary>
    public sealed class VM : INotifyPropertyChanged {
        private List<string> _originalPlayers;
        /// <summary>
        /// Az eredeti játékosok listája (a GUI-n látható színhozzárendelés miatt kell inicializálni)
        /// </summary>
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
        /// <summary>
        /// A játékban lévő harcosok sora
        /// </summary>
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
        /// <summary>
        /// A memória cellái
        /// </summary>
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
        /// <summary>
        /// A virtuális memória mérete
        /// </summary>
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
        /// <summary>
        /// Az aktuálisan futó kör száma
        /// </summary>
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
        /// <summary>
        /// A játék maximális köreinek száma
        /// </summary>
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
        /// <summary>
        /// A harcosonként maximálisan engedélyezett processzusok száma
        /// </summary>
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
        /// <summary>
        /// A harcosok maximális száma
        /// </summary>
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
        /// <summary>
        /// A soron következő utasítás címe
        /// </summary>
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
        /// <summary>
        /// A GUI frissítéséért felelős metódus
        /// </summary>
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        private static VM instance;
        /// <summary>
        /// A singleton memóriapéldány konstruktora és settere (már létező példány esetén a megadott paraméterek figyelmen kívül maradnak)
        /// </summary>
        /// <param name="memorySize">A memória maximális mérete</param>
        /// <param name="memorySize">A memória maximális mérete</param>
        /// <param name="warriors">A harcosok kezdeti száma</param>
        /// <param name="maxProcesses">A harcosonként engedélyezett maximális processzusok száma</param>
        public static VM GetInstance(int memorySize = 8000, int maxCycles = 80000, int warriors = 2, int maxProcesses = 8000) {
            if (instance == null) {
                instance = new(memorySize, maxCycles, warriors, maxProcesses);
            }
            return instance;
        }

        /// <summary>
        /// A singleton memóriapéldány törlése
        /// </summary>
        public static void ResetInstance() {
            instance = null;
        }

        /// <summary>
        /// A memória címének normalizálása [0, MemorySize-1] tartományba
        /// </summary>
        public int ModMemorySize(int memoryAddress) {
            while (memoryAddress < 0) {
                memoryAddress += instance.MemorySize;
            }
            return memoryAddress % instance.MemorySize;
        }

        /// <summary>
        /// Privát konstruktor a singleton memóriapéldányhoz
        /// </summary>
        /// <param name="memorySize">A memória maximális mérete</param>
        /// <param name="memorySize">A memória maximális mérete</param>
        /// <param name="warriors">A harcosok kezdeti száma</param>
        /// <param name="maxProcesses">A harcosonként engedélyezett maximális processzusok száma</param>
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

        /// <summary>
        /// Végrehajtja a játék egy körét
        /// </summary>
        /// <returns>
        /// A kieső játékos neve; ha ilyen nincs, akkor üres string
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string Play() {
            if (Cycle < MaxCycles) {
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
        }
        /// <summary>
        /// Utasítássorozat betöltése a memóriába
        /// </summary>
        /// <param name="program">A betöltendő utasítások listája</param>
        /// <param name="start">Az első betöltendő utasítás kezdőcíme</param>
        /// <param name="playerName">Az utasítássorozathoz tartozó játékos neve</param>
        public void LoadIntoMemory(List<Instruction> program, int start, string playerName) {
            for (int i = 0; i < program.Count; ++i) {
                Memory[(start + i) % Memory.Count].Instruction = program[i];
                Memory[(start + i) % Memory.Count].LastModifiedBy = playerName;
            }
        }

        /// <summary>
        /// Megadott címen található memóriacella
        /// </summary>
        /// <param name="address">Az érintett memóriacím</param>
        /// <param name="relative">Relatív (true) vagy abszolút (false) a memóriacím</param>
        /// <returns>
        /// A memóriacella
        /// </returns>
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

        /// <summary>
        /// Megadott címen található utasítás
        /// </summary>
        /// <param name="address">Az érintett utasításcím</param>
        /// <param name="relative">Relatív (true) vagy abszolút (false) az utasításcím</param>
        /// <returns>
        /// Az utasítás
        /// </returns>
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
        /// Végrehajtja megadott memóriacímen található utasítást a megadott játékos nevében
        /// </summary>
        /// <param name="memoryAddress">A soron következő utasításcím</param>
        /// <param name="playerName">A végrehajtó harcos neve</param>
        /// <returns>
        /// A végrehajtás után soron következő utasítások címei; érvénytelenül végrehajtott utasítás esetén [-1]
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public int[] ExecuteInstruction(int memoryAddress, string playerName = "player") {
            CurrentInstructionAddress = memoryAddress;
            Instruction currentInstruction = InstructionAt(memoryAddress, false);
            Instruction source = InstructionAt(currentInstruction.GetA(), true);
            Instruction target = InstructionAt(currentInstruction.GetB(), true);
            MemoryCell targetMemCell = MemoryCellAt(currentInstruction.GetB(), true);

            // az utasítás műveletének függvényében folytatódik a végrehajtás
            switch (currentInstruction.OpCode) {
                case OpCode.DAT:
                    // adatszekciót nem hajtunk végre, így az érvénytelen
                    return [-1];
                case OpCode.MOV:
                    targetMemCell.LastModifiedBy = playerName;

                    // a módosítók határozzák meg, a forrás- és célcímek mely operandusára érvényesek a műveletek
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
                        // a 0-val való osztás érvénytelen
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
                        // modulo 0 érvénytelen
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
                    throw new ArgumentException();
            }
        }

        public override string ToString() {
            string output = "A memória jelenlegi állapota:\n";
            for (int i = 0; i < Memory.Count; ++i) {
                if (!Memory[i].Instruction.Equals(new Instruction())) {
                    output += $"{Memory[i]}{(i == CurrentInstructionAddress ? "\t\t<< lefutott utasítás" : "")}\n\n";
                }
            }
            return output;
        }
    }
}