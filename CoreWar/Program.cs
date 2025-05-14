namespace CoreWar {
    public class Program {
        public static void Main(string[] args) {
            VM vm = VM.GetInstance();
            for (int i = 0; i < vm.Warriors; i++) {
                Console.Write($"Kérem {i + 1}. játékos programjának fájlnevét! ");
                string? path = Console.ReadLine();
                if (path != null) {
                    int firstProcessStart = RedcodeInputLoader.LoadFromFile(Path.Combine("../../../", path));
                    Player p = new($"{i + 1}. játékos", firstProcessStart, vm.MaxProcesses);
                }
            }
            vm.Play();
        }
    }
}