using Antlr4.Runtime;

namespace CoreWar {
    public class RedcodeInputLoader {

        public static int LoadFromFile(string path) {
            AntlrInputStream inputStream = new(File.ReadAllText(path));
            return Load(inputStream);

        }

        public static int LoadFromInput(string input) {
            AntlrInputStream inputStream = new(input);
            return Load(inputStream);
        }

        private static int Load(AntlrInputStream inputStream) {
            Random random = new();
            VM vm = VM.GetInstance();

            RedcodeLexer lexer = new(inputStream);
            CommonTokenStream tokenStream = new(lexer);
            RedcodeParser parser = new(tokenStream);
            RedcodeParser.ProgramContext context = parser.program();
            RedcodeVisitor visitor = new();

            var (process, firstInstructionOffset) = ((List<Instruction>, int))visitor.VisitProgram(context);
            int firstInstructionStart = (random.Next(vm.Memory.Length) + firstInstructionOffset) % vm.Memory.Length;          // TODO: leellenõrizni, hogy van-e már ott valami a memóriában
            vm.LoadIntoMemory(process, firstInstructionStart);

            return firstInstructionStart;
        }
    }
}