using CoreWar;

namespace CoreWarTests {
    [TestClass]
    public sealed class InstructionTests {
        [TestInitialize]
        public void TestInitialize() {
            VM.ResetInstance();
        }

        [TestMethod]
        public void Copy_CopyToEmptyAddress_ValuesChange() {
            VM vm = VM.GetInstance(memorySize: 1);
            vm.InstructionAt(0, false).Copy(
                new Instruction(
                    OpCode.MOV,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 1),
                    new Operation(AddressingMode.IMMEDIATE, 2)
                )
            );
            Assert.AreEqual(new Instruction(
                OpCode.MOV,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
                ), vm.InstructionAt(0, false));
        }
    }
}
