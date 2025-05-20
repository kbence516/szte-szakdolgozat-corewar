namespace CoreWar {
    public class Program {
        private static int memorySize = 8000;
        private static int maxCycles = 80000;
        private static int maxProcesses = 8000;
        private static int warriors = 2;
        private static VM vm;

        public static void Main(string[] args) {
            Console.WriteLine(Utils.GetLogo());

            if (args.Length == 0) {
                GetShortHelp();
                return;
            }
            if (args.Contains("--help") || args.Contains("-h")) {
                GetLongHelp();
                return;
            }
            if (args.Contains("--credits") || args.Contains("-c")) {
                Console.WriteLine(Utils.GetCredits());
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
            Console.WriteLine("\nA memória inicializálva, kezdődhet a játék!\n");
            PlayGame();
        }

        private static void PlayGame() {
            bool isPlaying = true;
            while (true) {
                string? userInput = Console.ReadLine();
                string[] userInputSplit = userInput?.Split(' ') ?? [];
                if (userInputSplit.Length > 0) {
                    switch (userInputSplit[0]) {
                        case "h":
                        case "help":
                            GetGameHelp();
                            break;
                        case "r":
                        case "round":
                            Console.WriteLine($"Ez a(z) {vm.Cycle}. kör, összesen {vm.MaxCycles} van.");
                            break;
                        case "s":
                        case "show":
                            if (userInputSplit.Length == 1) {
                                Console.WriteLine(vm);
                            } else if (userInputSplit.Length == 2) {
                                try {
                                    Console.WriteLine("\n" + vm.MemoryCellAt(int.Parse(userInputSplit[1]), false) + "\n");
                                } catch (Exception) {
                                    Console.WriteLine("Helytelen memóriacím! Biztosan helyes számot adtál meg?");
                                }
                            } else if (userInputSplit.Length == 3) {
                                try {
                                    int min = int.Parse(userInputSplit[1]);
                                    int max = int.Parse(userInputSplit[2]);
                                    while (min <= max) {
                                        Console.WriteLine("\n" + vm.MemoryCellAt(min++, false) + "\n");
                                    }
                                } catch (Exception) {
                                    Console.WriteLine("Helytelen memóriacím! Biztosan helyes számot adtál meg?");
                                }
                            }
                            break;
                        case "n":
                        case "next":
                            if (isPlaying) {
                                string nextLoser = vm.Play();
                                if (!nextLoser.Equals("")) {
                                    Console.WriteLine($"{nextLoser} vesztett!");
                                    if (vm.Players.Count > 1) {
                                        Console.WriteLine($"{vm.Players.Count} játékos maradt.");
                                    } else {
                                        Console.WriteLine($"Vége a játéknak, {vm.Players.Peek().Name} nyert!");
                                        isPlaying = false;
                                    }
                                    break;
                                }
                                if (vm.Cycle == vm.MaxCycles && vm.Players.Count > 1) {
                                    Console.WriteLine("A lejátszható körök száma elérte a maximumot, a játék döntetlen.");
                                    isPlaying = false;
                                }
                            }
                            break;
                        case "j":
                        case "jump":
                            if (isPlaying) {
                                try {
                                    for (int i = 0; i < int.Parse(userInputSplit[1]); i++) {
                                        string nextLoser = vm.Play();
                                        if (!nextLoser.Equals("")) {
                                            Console.WriteLine($"{nextLoser} vesztett!");
                                            if (vm.Players.Count > 1) {
                                                Console.WriteLine($"{vm.Players.Count} játékos maradt.");
                                            } else {
                                                Console.WriteLine($"Vége a játéknak, {vm.Players.Peek().Name} nyert!");
                                                isPlaying = false;
                                            }
                                            break;
                                        }
                                        if (vm.Cycle == vm.MaxCycles && vm.Players.Count > 1) {
                                            Console.WriteLine("A lejátszható körök száma elérte a maximumot, a játék döntetlen.");
                                            isPlaying = false;
                                            break;
                                        }
                                    }
                                } catch (Exception) {
                                    Console.WriteLine("Egész számot adj meg az ugráshoz!");
                                }
                            }
                            break;
                        case "c":
                        case "continue":
                            if (isPlaying) {
                                while (true) {
                                    string nextLoser = vm.Play();
                                    if (!nextLoser.Equals("")) {
                                        Console.WriteLine($"{nextLoser} vesztett!");
                                        if (vm.Players.Count > 1) {
                                            Console.WriteLine($"{vm.Players.Count} játékos maradt.");
                                        } else {
                                            Console.WriteLine($"Vége a játéknak, {vm.Players.Peek().Name} nyert!");
                                            isPlaying = false;
                                        }
                                        break;
                                    }
                                    if (vm.Cycle == vm.MaxCycles && vm.Players.Count > 1) {
                                        Console.WriteLine("A lejátszható körök száma elérte a maximumot, a játék döntetlen.");
                                        isPlaying = false;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "e":
                        case "exit":
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }

        private static void GetGameHelp() {
            Console.WriteLine("\nJáték közben elérhető utasítások:");
            Console.WriteLine("\t n(ext)\t\t\t\tKövetkező lépés");
            Console.WriteLine("\t j(ump) <n>\t\t\tUgorj n lépést");
            Console.WriteLine("\t c(ontinue)\t\t\tFolytatás, amíg valaki veszít, vagy vége a játéknak");
            Console.WriteLine("\t s(how)\t\t\t\tMutasd az összes nemüres mezőt");
            Console.WriteLine("\t s(how) [n]\t\t\tMutasd az n. mezőt");
            Console.WriteLine("\t s(how) [first] [last]\t\tMutasd az összes nemüres mezőt [first, last] tartományban");
            Console.WriteLine("\t r(ound)\t\t\tHányadik körnél tartunk?");
            Console.WriteLine("\t h(elp)\t\t\t\tEnnek az üzenetnek a megjelenítése");
            Console.WriteLine("\t e(xit)\t\t\t\tKilépés a játékból");
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