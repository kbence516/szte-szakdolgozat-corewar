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
            process.ForEach((p) => {
                p.OpA.Value = vm.ModMemorySize(p.OpA.Value);
                p.OpB.Value = vm.ModMemorySize(p.OpB.Value);
            });
            int firstInstructionStart = vm.ModMemorySize(random.Next(vm.Memory.Length) + firstInstructionOffset);          // TODO: leellen�rizni, hogy van-e m�r ott valami a mem�ri�ban
            vm.LoadIntoMemory(process, firstInstructionStart);

            return firstInstructionStart;
        }
    }
}