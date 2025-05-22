using CoreWar;

namespace CoreWarTests {
    [TestClass]
    public sealed class PlayerTests {
        [TestInitialize]
        public void TestInitialize() {
            VM.ResetInstance();
        }

        [TestMethod]
        public void Execute_OpDat_StopsProcess() {
            VM vm = VM.GetInstance(memorySize: 1);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsFalse(p.Execute());
        }

        [TestMethod]
        public void Execute_OpMov_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
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
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpAdd_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.ADD,
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
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSub_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.SUB,
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
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpMul_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MUL,
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
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpDivPosQuotient_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpDivNegQuotient_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpDivZeroDenominator_StopsProcess() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DIV,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsFalse(p.Execute());
        }

        [TestMethod]
        public void Execute_OpModPosQuotient_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpModNegQuotient_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, -1),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpModZeroDenominator_StopsProcess() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.MOD,
                OpModifier.A,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.DIRECT, 1)
            ));
            vm.InstructionAt(2, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.INDIRECT, 1),
                new Operation(AddressingMode.INDIRECT, 1)
            ));
            Player p = new("test", 1);
            Assert.IsFalse(p.Execute());
        }

        [TestMethod]
        public void Execute_OpJmp_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpJmzJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpJmzNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMZ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpJmnJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpJmnNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpDjnJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 2),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpDjnNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.JMN,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSpl_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SPL,
                OpModifier.B,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpCmpJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpCmpNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.CMP,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSeqJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSeqNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SEQ,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSneJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSneNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SNE,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSltJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpSltNoJump_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 3);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.SLT,
                OpModifier.A,
                new Operation(AddressingMode.DIRECT, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            vm.InstructionAt(1, false).Copy(new Instruction(
                OpCode.DAT,
                OpModifier.F,
                new Operation(AddressingMode.IMMEDIATE, 1),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }

        [TestMethod]
        public void Execute_OpNop_KeepsProcessAlive() {
            VM vm = VM.GetInstance(memorySize: 1);
            vm.InstructionAt(0, false).Copy(new Instruction(
                OpCode.NOP,
                OpModifier.B,
                new Operation(AddressingMode.IMMEDIATE, 0),
                new Operation(AddressingMode.IMMEDIATE, 0)
            ));
            Player p = new("test", 0);
            Assert.IsTrue(p.Execute());
        }
    }
}
