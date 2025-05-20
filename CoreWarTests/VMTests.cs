using CoreWar;

namespace CoreWarTests {
    [TestClass]
    public sealed class VMTests {
        [TestInitialize]
        public void TestInitialize() {
            VM.ResetInstance();
        }


        [TestMethod]
        public void GetInstance_DefaultMemorySize_Returns8000Memory() {
            VM vm = VM.GetInstance();
            Assert.AreEqual(8000, vm.Memory.Count);
        }

        [TestMethod]
        public void GetInstance_PositiveMemorySize_Returns10Memory() {
            VM vm = VM.GetInstance(memorySize: 10);
            Assert.AreEqual(10, vm.Memory.Count);
        }

        [TestMethod]
        public void GetInstance_ZeroMemorySize_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => VM.GetInstance(memorySize: 0));
        }

        [TestMethod]
        public void GetInstance_NegativeMemorySize_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => VM.GetInstance(memorySize: -1));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDat_ReturnsNegativeAddress() {
            VM vm = VM.GetInstance(memorySize: 1);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(-1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovAPosAPosB_CopiesAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.INDIRECT, 0)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovAPosANegB_CopiesAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, -1)
            ));
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.INDIRECT, 0)
            ), vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovAPosANegB_UnderflowsAndCopiesAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, -9)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.INDIRECT, 0)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovANegAPosB_CopiesAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.INDIRECT, 0)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovBPosAPosB_CopiesBToB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovABPosAPosB_CopiesAToB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovBAPosAPosB_CopiesBToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(1);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 0)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovFPosAPosB_CopiesABtoABWithoutAddrMode() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 2)
            ), vm.InstructionAt(2, false));
        }


        [TestMethod]
        public void ExecuteInstruction_OpMovXPosAPosB_CopiesABtoBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMovIPosAPosB_CopiesWholeInstr() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOV,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 0)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.MOV,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddAPosAPosB_AddsAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddANegAPosB_AddsAToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddBPosAPosB_AddsBToB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 5)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddABPosAPosB_AddsAToB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddBAPosAPosB_AddsBToA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 5),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddFPosAPosB_AddsABToAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 5)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddIPosAPosB_AddsABToAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 5)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpAddXPosAPosB_AddsABToBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.ADD,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 5),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubAPosAPosB_SubtractsAFromA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubANegAPosB_SubtractsAFromA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubBPosAPosB_SubtractsBFromB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubABPosAPosB_SubtractsAfromB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 2)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubBAPosAPosB_SubtractsBFromA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubFPosAPosB_SubtractsABFromAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubIPosAPosB_SubtractsABFromAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpSubXPosAPosB_SubtractsABFromBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SUB,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 5),
                new Operation(AddressingMode.INDIRECT, 5)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulAPosAPosB_MultipliesAByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulANegAPosB_MultipliesAByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulBPosAPosB_MultipliesBByB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 6)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulABPosAPosB_MultipliesAByB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 9)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulBAPosAPosB_MultipliesBByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulFPosAPosB_MultipliesABByAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 6)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulIPosAPosB_MultipliesABByAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 6)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpMulXPosAPosB_MultipliesABByBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MUL,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 9)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivAPosAPosBDivisible_DividesAByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivAPosAPosBNotDivisible_DividesAByAAndRoundsDown() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivANegAPosBDivisible_DividesAByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivANegAPosBNotDivisible_DividesAByAAndRoundsUp() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivAPosAZeroB_ReturnsNegativeAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(-1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivBPosAPosBDivisible_DividesBByB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 8)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivABPosAPosBDivisible_DividesAByB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 9)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivBAPosAPosBDivisible_DividesBByA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 8)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 8)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivFPosAPosBDivisible_DividesABByAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 8)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivIPosAPosBDivisible_DividesABByAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 8)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 3),
                new Operation(AddressingMode.INDIRECT, 4)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpDivXPosAPosBDivisible_DividesABByBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 8),
                new Operation(AddressingMode.INDIRECT, 9)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 4),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModAPosAPosBDivisible_ReturnsZeroModulo() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModAPosAPosBNotDivisible_TakesModuloAOfA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModAPosSrcANegTargetAPosBDivisible_ReturnsZeroModulo() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, -6),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModANegSrcAPosTargetAPosBDivisible_ReturnsZeroModulo() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 6),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 0),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModANegSrcAPosTargetAPosBNotDivisible_TakesModuloAOfA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModAPosSrcANegTargetAPosBNotDivisible_TakesModuloAOfA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, -7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 3)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModAPosAZeroB_ReturnsNegativeAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 3)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(-1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpModBPosAPosBNotDivisible_TakesModuloBOfB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 5),
                new Operation(AddressingMode.INDIRECT, 5)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 5),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModABPosAPosBNotDivisible_TakesModuloAOfB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 9)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 9),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModBAPosAPosBNotDivisible_TakesModuloBOfA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 3),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 7)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 7)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModFPosAPosBNotDivisible_TakesModuloABOfAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 5),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 7)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModIPosAPosBNotDivisible_TakesModuloABOfAB() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 5),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 7)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 2),
                new Operation(AddressingMode.INDIRECT, 1)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpModXPosAPosBNotDivisible_TakesModuloABOfBA() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 5),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 7),
                new Operation(AddressingMode.INDIRECT, 7)
            ));
            vm.ExecuteInstruction(0);
            Assert.AreEqual(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 2)
            ), vm.InstructionAt(2, false));
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmpPosA_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 4),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(4, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmpPosA_OverflowsAndReturnsSmallerMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 7),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmpNegA_ReturnsSmallerMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, -1),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(2);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmpNegA_UnderflowsAndReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, -7),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(2);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(0, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmpZeroA_ReturnsSameMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(2);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzAPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzAPosANonZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzBAPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzBAPosANonZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzBPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzBPosANonZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzABPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzABPosANonZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzIPosAZeroBValues_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzIPosANonZeroBValues_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzFPosAZeroBValues_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzFPosANonZeroBValues_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzXPosAZeroBValues_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmzXPosANonZeroBValues_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnAPosAZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnAPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnBAPosAZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnBAPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnBPosAZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnBPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnABPosAZeroBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnABPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnIPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnIPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnFPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnFPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnXPosAZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpJmnXPosANonZeroBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnAPosAOneBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnAPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnBAPosAOneBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnBAPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.BA,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnBPosAOneBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnBPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnABPosAOneBValue_ReturnsNextMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnABPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.AB,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnIPosAOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnIPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.I,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnFPosAOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnFPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnXPosAOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpDjnXPosANonOneBValue_ReturnsGreaterMemoryAddress() {
            VM vm = VM.GetInstance(memorySize: 5);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DJN,
                OpModifier.X,
                new Operation(AddressingMode.DIRECT, 3),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(3, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSpl_ReturnsTwoAddresses() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SPL,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(2, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
            Assert.AreEqual(2, nextInstruction[1]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpAEqualASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpAEqualADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpANotEqualASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpANotEqualADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBEqualBSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBEqualBDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBNotEqualBSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBNotEqualBDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpABEqualABSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpABEqualABDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpABNotEqualABSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpABNotEqualABDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBAEqualBASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBAEqualBADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBANotEqualBASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpBANotEqualBADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpFEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpFEqualValuesDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpFNotEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpFNotEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpXEqualValuesSameAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpXEqualValuesDiffAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpXNotEqualValuesSameAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpXNotEqualValuesDiffAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpIEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpIEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpINotEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpINotEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpCmpIEqualValuesSameAddrModeDiffOpCode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqAEqualASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqAEqualADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqANotEqualASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqANotEqualADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBEqualBSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBEqualBDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBNotEqualBSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBNotEqualBDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqABEqualABSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqABEqualABDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqABNotEqualABSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqABNotEqualABDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBAEqualBASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBAEqualBADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBANotEqualBASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqBANotEqualBADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqFEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqFEqualValuesDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqFNotEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqFNotEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqXEqualValuesSameAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqXEqualValuesDiffAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqXNotEqualValuesSameAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqXNotEqualValuesDiffAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqIEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqIEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqINotEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqINotEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSeqIEqualValuesSameAddrModeDiffOpCode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneAEqualASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneAEqualADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneANotEqualASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.DIRECT, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneANotEqualADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBEqualBSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBEqualBDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBNotEqualBSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBNotEqualBDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneABEqualABSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneABEqualABDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneABNotEqualABSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneABNotEqualABDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBAEqualBASameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBAEqualBADiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBANotEqualBASameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.DIRECT, 0),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneBANotEqualBADiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneFEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneFEqualValuesDiffAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneFNotEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneFNotEqualValuesDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneXEqualValuesSameAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneXEqualValuesDiffAddrModeCrossed_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneXNotEqualValuesSameAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneXNotEqualValuesDiffAddrModeCrossed_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneIEqualValuesSameAddrMode_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneIEqualValuesDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneINotEqualValuesSameAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneINotEqualValuesDiffAddrMode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSneIEqualValuesSameAddrModeDiffOpCode_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltASmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltAEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltAGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBSmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltABSmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltABEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltABGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.AB,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBASmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBAEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltBAGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.BA,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltISmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltIEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltIGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.I,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltFSmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltFEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltFGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltXSmallerSrc_SkipsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 2)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(2, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltXEqualSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 1)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpSltXGreaterSrc_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 10);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.X,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }

        [TestMethod]
        public void ExecuteInstruction_OpNop_ReturnsNextAddress() {
            VM vm = VM.GetInstance(memorySize: 2);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.NOP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            var nextInstruction = vm.ExecuteInstruction(0);
            Assert.AreEqual(1, nextInstruction.Length);
            Assert.AreEqual(1, nextInstruction[0]);
        }
    }
}
