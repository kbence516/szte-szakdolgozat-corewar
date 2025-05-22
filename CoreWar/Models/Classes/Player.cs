namespace CoreWar {
    public class Player {
        private readonly VM vm = VM.GetInstance();

        public Queue<int> Processes {       // Kezdetben a kezdőutasítások sorszámai
            get; private set;
        }

        public string Name {
            get; private set;
        }

        public Player(string name, int firstProcessStart) {
            Name = name;
            Processes = new(vm.MaxProcesses);
            Processes.Enqueue(firstProcessStart);
            Console.WriteLine($"{Name} betöltve a memóriába, kezdőcím: {firstProcessStart}");
            vm.Players.Enqueue(this);
        }

        /// <summary>
        /// Végrehajtja a játékos soron következő utasítását.
        /// </summary>
        /// <returns>
        /// Ha a játékosnak még van élő processzusa, akkor igaz, különben hamis
        /// </returns>
        public bool Execute() {
            int[] nextAddresses = vm.ExecuteInstruction(Processes.Dequeue(), Name);
            foreach (int address in nextAddresses) {
                if (address >= 0 && Processes.Count <= vm.MaxProcesses) {
                    Processes.Enqueue(address);
                }
            }
            return Processes.Count != 0;
        }
    }
}