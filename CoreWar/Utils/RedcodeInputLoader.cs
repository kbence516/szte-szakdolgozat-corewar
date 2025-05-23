using Antlr4.Runtime;

namespace CoreWar {

    /// <summary>
    /// A Redcode bet�lt�s��rt felel�s oszt�ly
    /// </summary>
    public class RedcodeInputLoader {

        /// <summary>
        /// Redcode bet�lt�se f�jlb�l
        /// </summary>
        /// <param name="path">A f�jl el�r�si �tvonala</param>
        /// <param name="playerName">A v�grehajt� harcos neve</param>
        /// <returns>Az els�, j�t�kos �ltal v�grehajthat� (offsetelt) utas�t�s mem�riac�me</returns>
        public static int LoadFromFile(string path, string playerName = "player") {
            AntlrInputStream inputStream = new(File.ReadAllText(path));
            return Load(inputStream, playerName);

        }

        /// <summary>
        /// Redcode bet�lt�se input stringb�l
        /// </summary>
        /// <param name="input">A bemeneti Redcode string</param>
        /// <param name="playerName">A v�grehajt� harcos neve</param>
        /// <returns>Az els�, j�t�kos �ltal v�grehajthat� (offsetelt) utas�t�s mem�riac�me</returns>
        public static int LoadFromInput(string input, string playerName = "player") {
            AntlrInputStream inputStream = new(input);
            return Load(inputStream, playerName);
        }

        /// <summary>
        /// Redcode bet�lt�se �talak�tott AntlrInputStreamb�l
        /// </summary>
        /// <param name="inputStream">Az �talak�tott Redcode input</param>
        /// <param name="playerName">A v�grehajt� harcos neve</param>
        /// <returns>Az els�, j�t�kos �ltal v�grehajthat� (offsetelt) utas�t�s mem�riac�me</returns>
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