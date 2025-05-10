class Program {

    public static void Main(string[] args) {
        VM vm = VM.GetInstance();
        for (int i = 0; i < vm.Warriors; i++) {
            Console.Write($"Kérem {i + 1}. játékos programjának fájlnevét! ");
            string? path = Console.ReadLine();
            if (path != null) {
                Player p = new Player(Path.Combine("../../../", path));
            }
        }
        vm.Play();
    }
}