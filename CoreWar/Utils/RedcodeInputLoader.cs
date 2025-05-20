using Antlr4.Runtime;
using Microsoft.UI.Xaml;

namespace CoreWar {
    public class RedcodeInputLoader {

        public static int LoadFromFile(string path, string playerName = "player") {
            AntlrInputStream inputStream = new(File.ReadAllText(path));
            return Load(inputStream, playerName);

        }

        public static int LoadFromInput(string input, string playerName = "player") {
            AntlrInputStream inputStream = new(input);
            return Load(inputStream, playerName);
        }

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
            int firstInstructionStart = vm.ModMemorySize(random.Next(vm.Memory.Count));
            vm.LoadIntoMemory(process, firstInstructionStart, playerName);

            return vm.ModMemorySize(firstInstructionStart + firstInstructionOffset);
        }
    }
}