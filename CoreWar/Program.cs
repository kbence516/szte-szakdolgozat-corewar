namespace CoreWar {
    public class Program {
        private static int memorySize = 8000;
        private static int maxCycles = 80000;
        private static int maxProcesses = 8000;
        private static int warriors = 2;
        private static VM vm;

        public static void Main(string[] args) {

            if (args.Length == 0) {
                GetShortHelp();
                return;
            }
            if (args.Contains("--help") || args.Contains("-h")) {
                GetLongHelp();
                return;
            }
            if (args.Contains("--memorySize")) {
                int memSizeIndex = Array.IndexOf(args, "--memorySize");
                try {
                    int newMemorySize = int.Parse(args[memSizeIndex + 1]);
                    if (newMemorySize < 100 || newMemorySize > 20000) {
                        GetErrorMessage();
                        return;
                    }
                    memorySize = newMemorySize;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            } else if (args.Contains("-m")) {
                int memSizeIndex = Array.IndexOf(args, "-m");
                try {
                    int newMemorySize = int.Parse(args[memSizeIndex + 1]);
                    if (newMemorySize < 100 || newMemorySize > 20000) {
                        GetErrorMessage();
                        return;
                    }
                    memorySize = newMemorySize;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            }
            if (args.Contains("--maxCycles")) {
                int maxCyclesIndex = Array.IndexOf(args, "--maxCycles");
                try {
                    int newMaxCycles = int.Parse(args[maxCyclesIndex + 1]);
                    if (newMaxCycles < 1000 || newMaxCycles > 200000) {
                        GetErrorMessage();
                        return;
                    }
                    maxCycles = newMaxCycles;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            } else if (args.Contains("-c")) {
                int maxCyclesIndex = Array.IndexOf(args, "-c");
                try {
                    int newMaxCycles = int.Parse(args[maxCyclesIndex + 1]);
                    if (newMaxCycles < 1000 || newMaxCycles > 200000) {
                        GetErrorMessage();
                        return;
                    }
                    maxCycles = newMaxCycles;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            }
            if (args.Contains("--maxProcesses")) {
                int maxProcessesIndex = Array.IndexOf(args, "--maxProcesses");
                try {
                    int newMaxProcesses = int.Parse(args[maxProcessesIndex + 1]);
                    if (newMaxProcesses < 100 || newMaxProcesses > 100000) {
                        GetErrorMessage();
                        return;
                    }
                    maxProcesses = newMaxProcesses;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            } else if (args.Contains("-p")) {
                int maxProcessesIndex = Array.IndexOf(args, "-p");
                try {
                    int newMaxProcesses = int.Parse(args[maxProcessesIndex + 1]);
                    if (newMaxProcesses < 100 || newMaxProcesses > 100000) {
                        GetErrorMessage();
                        return;
                    }
                    maxProcesses = newMaxProcesses;
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                }
            }
            if (args.Contains("--warriors")) {
                int warriorsIndex = Array.IndexOf(args, "--warriors");
                try {
                    int newWarriors = int.Parse(args[warriorsIndex + 1]);
                    if (newWarriors < 2 || newWarriors > 4) {
                        GetErrorMessage();
                        return;
                    }
                    warriors = newWarriors;
                    warriorsIndex += 2;         // már az első elérési útvonalra mutat
                    vm = VM.GetInstance(memorySize, maxCycles, warriors, maxProcesses);
                    for (int i = 0; i < warriors; i++) {
                        string name = Path.GetFileNameWithoutExtension(args[warriorsIndex]);
                        int firstProcessStart = RedcodeInputLoader.LoadFromFile(args[warriorsIndex++], name);
                        Player p = new(name, firstProcessStart, vm.MaxProcesses);
                    }
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                } catch (Exception) {
                    Console.WriteLine("Hiba a fájlok beolvasásakor! Biztosan érvényes formátumú Redcode-ot adtál meg?");
                    return;
                }
            } else if (args.Contains("-w")) {
                int warriorsIndex = Array.IndexOf(args, "-w");
                try {
                    int newWarriors = int.Parse(args[warriorsIndex + 1]);
                    if (newWarriors < 2 || newWarriors > 4) {
                        GetErrorMessage();
                        return;
                    }
                    warriors = newWarriors;
                    warriorsIndex += 2;         // már az első elérési útvonalra mutat
                    vm = VM.GetInstance(memorySize, maxCycles, warriors, maxProcesses);
                    for (int i = 0; i < warriors; i++) {
                        string name = Path.GetFileNameWithoutExtension(args[warriorsIndex]);
                        int firstProcessStart = RedcodeInputLoader.LoadFromFile(args[warriorsIndex++], name);
                        Player p = new(name, firstProcessStart, vm.MaxProcesses);
                    }
                } catch (IndexOutOfRangeException) {
                    GetErrorMessage();
                    return;
                } catch (Exception) {
                    Console.WriteLine("Hiba a fájlok beolvasásakor! Biztosan érvényes formátumú Redcode-ot adtál meg?");
                    return;
                }
            } else {
                try {
                    vm = VM.GetInstance(memorySize, maxCycles, warriors, maxProcesses);
                    string name1 = Path.GetFileNameWithoutExtension(args[^2]);
                    string name2 = Path.GetFileNameWithoutExtension(args[^1]);
                    int firstProcessStart1 = RedcodeInputLoader.LoadFromFile(args[^2], name1);
                    int firstProcessStart2 = RedcodeInputLoader.LoadFromFile(args[^1], name2);
                    Player p1 = new(name1, firstProcessStart1, vm.MaxProcesses);
                    Player p2 = new(name2, firstProcessStart2, vm.MaxProcesses);
                } catch (Exception) {
                    Console.WriteLine("Hiba a fájlok beolvasásakor! Biztosan érvényes formátumú Redcode-ot adtál meg?");
                    return;
                }
            }
            Console.WriteLine("\nA memória inicializálva, kezdődhet a játék!");
        }

        private static void GetErrorMessage() {
            Console.WriteLine("Helytelen használat!");
            GetLongHelp();
        }
        private static void GetShortHelp() {
            Console.WriteLine("Helyes használat:");
            Console.WriteLine($"\t{System.AppDomain.CurrentDomain.FriendlyName} <redcode_1> <redcode_2>");
            Console.WriteLine("További paraméterek és segítség:");
            Console.WriteLine($"\t{System.AppDomain.CurrentDomain.FriendlyName} --help");
            Console.ReadLine();
        }

        private static void GetLongHelp() {
            Console.WriteLine("Helyes használat:");
            Console.WriteLine($"\t{System.AppDomain.CurrentDomain.FriendlyName} [--memorySize <n>] [--maxCycles <n>] [--maxProcesses <n>] [--warriors <wn>] <redcode_1> <redcode_2> [... <redcode_wn>]\n");
            Console.WriteLine("Redcode:");
            Console.WriteLine("\t<redcode>\t\t.red kiterjesztésű fájlok (2-4 között, játékosok számától függően)\n");
            Console.WriteLine("Egyéb paraméterek:");
            Console.WriteLine("\t-m, --memorySize\tA játéktér (memória) mérete - [100-20000], alapértelmezetten 8000");
            Console.WriteLine("\t-c, --maxCycles\t\tMaximális körök száma - [1000-200000], alapértelmezetten 80000");
            Console.WriteLine("\t-p, --maxProcesses\tMaximális processzuszok száma játékosonként - [100-100000], alapértelmezetten 8000");
            Console.WriteLine("\t-w, --warriors\t\tJátékosok száma - [2-4], alapértelmezetten 2");
            Console.WriteLine("\t-h, --help\t\tEnnek az üzenetnek a megjelenítése\n");
            Console.WriteLine("A játékon belüli vezérlés megismeréséhez indítás után használd a h(elp) utasítást!");
            Console.ReadLine();
        }
    }
}