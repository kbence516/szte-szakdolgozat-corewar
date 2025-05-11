namespace CoreWar {
    public sealed class VM {

        public List<Player> Players {
            get; private set;
        }
        public Instruction[] Memory {
            get; private set;
        }
        public int MemorySize {
            get; private set;
        }

        public int ActivePlayer {
            get; private set;
        }

        public int Cycle {
            get; private set;
        }

        public int MaxCycles {
            get; private set;
        }

        public int MaxProcesses {
            get; private set;
        }

        public int CurrentInstruction {
            get; set;
        }

        public int Warriors {
            get; private set;
        }

        private static VM instance;

        public static VM GetInstance(int memorySize = 8000, int maxCycles = 80000, int warriors = 2, int maxProcesses = 8000/*, int maxLength = 100, int minDistance = 100*/) {
            if (instance == null) {
                instance = new VM(memorySize, maxCycles, warriors, maxProcesses);
            }
            return instance;
        }

        private VM(int memorySize, int maxCycles, int warriors, int maxProcesses) {
            Players = new();
            MemorySize = memorySize;
            Memory = Enumerable.Range(0, MemorySize).Select(_ => new Instruction()).ToArray();
            MaxProcesses = maxProcesses;
            Warriors = warriors;

            MaxCycles = maxCycles;
            Cycle = 0;
            ActivePlayer = 0;
            CurrentInstruction = -1;
        }

        public void Play() {
            while (Cycle < MaxCycles) {
                Console.WriteLine($"{Cycle}. kör\n{ToString()}");
                Console.ReadLine();

                ActivePlayer = (ActivePlayer + 1) % Players.Count;
                Players[ActivePlayer].Execute();
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
                if (address + CurrentInstruction < 0) {
                    address += MemorySize;
                }
                return Memory[(address + CurrentInstruction) % MemorySize];
            } else {
                return Memory[address % MemorySize];
            }
        }

        public void GameOver() {
            Console.WriteLine($"Vége a játéknak, {Players[0].Name} nyert!");
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
                    output += $"#{i}:\t{Memory[i]}{(i == CurrentInstruction ? "\t\t<< lefutott utasítás" : "")}\n";
                }
            }
            return output;
        }
    }
}