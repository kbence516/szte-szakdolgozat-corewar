using Antlr4.Runtime;

namespace CoreWar {

    /// <summary>
    /// A Redcode betöltéséért felelõs osztály
    /// </summary>
    public class RedcodeInputLoader {

        /// <summary>
        /// Redcode betöltése fájlból
        /// </summary>
        /// <param name="path">A fájl elérési útvonala</param>
        /// <param name="playerName">A végrehajtó harcos neve</param>
        /// <returns>Az elsõ, játékos által végrehajtható (offsetelt) utasítás memóriacíme</returns>
        public static int LoadFromFile(string path, string playerName = "player") {
            AntlrInputStream inputStream = new(File.ReadAllText(path));
            return Load(inputStream, playerName);

        }

        /// <summary>
        /// Redcode betöltése input stringbõl
        /// </summary>
        /// <param name="input">A bemeneti Redcode string</param>
        /// <param name="playerName">A végrehajtó harcos neve</param>
        /// <returns>Az elsõ, játékos által végrehajtható (offsetelt) utasítás memóriacíme</returns>
        public static int LoadFromInput(string input, string playerName = "player") {
            AntlrInputStream inputStream = new(input);
            return Load(inputStream, playerName);
        }

        /// <summary>
        /// Redcode betöltése átalakított AntlrInputStreambõl
        /// </summary>
        /// <param name="inputStream">Az átalakított Redcode input</param>
        /// <param name="playerName">A végrehajtó harcos neve</param>
        /// <returns>Az elsõ, játékos által végrehajtható (offsetelt) utasítás memóriacíme</returns>
        private static int Load(AntlrInputStream inputStream, string playerName) {
            Random random = new();
            VM vm = VM.GetInstance();

            RedcodeLexer lexer = new(inputStream);
            CommonTokenStream tokenStream = new(lexer);
            RedcodeParser parser = new(tokenStream);
            RedcodeParser.ProgramContext context = parser.program();
            RedcodeVisitor visitor = new();

            var (process, firstInstructionOffset) = ((List<Instruction>, int))visitor.VisitProgram(context);
            process.ForEach((p) => {
                p.OpA.Value = vm.ModMemorySize(p.OpA.Value);
                p.OpB.Value = vm.ModMemorySize(p.OpB.Value);
            });
            int firstInstructionStart = random.Next(vm.MemorySize);
            vm.LoadIntoMemory(process, firstInstructionStart, playerName);

            return vm.ModMemorySize(firstInstructionStart + firstInstructionOffset);
        }
    }
}