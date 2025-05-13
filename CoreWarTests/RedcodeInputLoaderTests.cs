using CoreWar;

namespace CoreWarTests {

    [TestClass]
    public sealed class RedcodeInputLoaderTests {
        private VM vm;

        [TestInitialize]
        public void TestInitialize() {
            VM.ResetInstance();
            vm = VM.GetInstance(memorySize: 1);
        }

        [TestMethod]
        public void LoadFromInput_InstrDat_ReturnsInstrDat() {
            RedcodeInputLoader.LoadFromInput("DAT.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDatNoOpModifier_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("DAT #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.F,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMov_ReturnsInstrMov() {
            RedcodeInputLoader.LoadFromInput("MOV.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOV,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMovNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("MOV #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOV,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMovNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("MOV $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOV,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMovNoOpModifierNeitherImmediate_ReturnsI() {
            RedcodeInputLoader.LoadFromInput("MOV $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOV,
                    OpModifier.I,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrAdd_ReturnsInstrAdd() {
            RedcodeInputLoader.LoadFromInput("ADD.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.ADD,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrAddNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("ADD #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.ADD,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrAddNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("ADD $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.ADD,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrAddNoOpModifierNeitherImmediate_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("ADD $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.ADD,
                    OpModifier.F,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSub_ReturnsInstrSub() {
            RedcodeInputLoader.LoadFromInput("SUB.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SUB,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }
        [TestMethod]
        public void LoadFromInput_InstrSubNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("SUB #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SUB,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSubNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("SUB $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SUB,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSubNoOpModifierNeitherImmediate_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("SUB $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SUB,
                    OpModifier.F,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMul_ReturnsInstrMul() {
            RedcodeInputLoader.LoadFromInput("MUL.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MUL,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMulNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("MUL #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MUL,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMulNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("MUL $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MUL,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMulNoOpModifierNeitherImmediate_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("MUL $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MUL,
                    OpModifier.F,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDiv_ReturnsInstrDiv() {
            RedcodeInputLoader.LoadFromInput("DIV.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DIV,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDivNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("DIV #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DIV,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDivNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("DIV $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DIV,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDivNoOpModifierNeitherImmediate_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("DIV $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DIV,
                    OpModifier.F,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrMod_ReturnsInstrMod() {
            RedcodeInputLoader.LoadFromInput("MOD.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOD,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrModNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("MOD #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOD,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrModNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("MOD $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOD,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrModNoOpModifierNeitherImmediate_ReturnsF() {
            RedcodeInputLoader.LoadFromInput("MOD $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.MOD,
                    OpModifier.F,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrJmp_ReturnsInstrJmp() {
            RedcodeInputLoader.LoadFromInput("JMP.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMP,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }
        
        [TestMethod]
        public void LoadFromInput_InstrJmpNoOpModifier_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("JMP #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMP,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrJmz_ReturnsInstrJmz() {
            RedcodeInputLoader.LoadFromInput("JMZ.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMZ,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrJmzNoOpModifier_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("JMZ #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMZ,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrJmn_ReturnsInstrJmn() {
            RedcodeInputLoader.LoadFromInput("JMN.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMN,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrJmnNoOpModifier_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("JMN #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.JMN,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDjn_ReturnsInstrDjn() {
            RedcodeInputLoader.LoadFromInput("DJN.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DJN,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrDjnNoOpModifier_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("DJN #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DJN,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrCmp_ReturnsInstrCmp() {
            RedcodeInputLoader.LoadFromInput("CMP.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.CMP,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrCmpNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("CMP #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.CMP,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrCmpNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("CMP $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.CMP,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrCmpNoOpModifierNeitherImmediate_ReturnsI() {
            RedcodeInputLoader.LoadFromInput("CMP $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.CMP,
                    OpModifier.I,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSlt_ReturnsInstrSlt() {
            RedcodeInputLoader.LoadFromInput("SLT.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SLT,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSltNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("SLT #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SLT,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSltNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("SLT $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SLT,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSpl_ReturnsInstrSpl() {
            RedcodeInputLoader.LoadFromInput("SPL.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SPL,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSplNoOpModifier_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("SPL #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SPL,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSeq_ReturnsInstrSeq() {
            RedcodeInputLoader.LoadFromInput("SEQ.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SEQ,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSeqNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("SEQ #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SEQ,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSeqNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("SEQ $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SEQ,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSeqNoOpModifierNeitherImmediate_ReturnsI() {
            RedcodeInputLoader.LoadFromInput("SEQ $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SEQ,
                    OpModifier.I,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSne_ReturnsInstrSne() {
            RedcodeInputLoader.LoadFromInput("SNE.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SNE,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSneNoOpModifierImmediateA_ReturnsAB() {
            RedcodeInputLoader.LoadFromInput("SNE #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SNE,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSneNoOpModifierImmediateB_ReturnsB() {
            RedcodeInputLoader.LoadFromInput("SNE $0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SNE,
                    OpModifier.B,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrSneNoOpModifierNeitherImmediate_ReturnsI() {
            RedcodeInputLoader.LoadFromInput("SNE $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.SNE,
                    OpModifier.I,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_InstrNop_ReturnsInstrNop() {
            RedcodeInputLoader.LoadFromInput("NOP.A #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.NOP,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }


        [TestMethod]
        public void LoadFromInput_OpB_ReturnsOpB() {
            RedcodeInputLoader.LoadFromInput("DAT.B #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.B,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_OpAB_ReturnsOpAB() {
            RedcodeInputLoader.LoadFromInput("DAT.AB #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.AB,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_OpBA_ReturnsOpBA() {
            RedcodeInputLoader.LoadFromInput("DAT.BA #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.BA,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_OpF_ReturnsOpF() {
            RedcodeInputLoader.LoadFromInput("DAT.F #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.F,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_OpX_ReturnsOpX() {
            RedcodeInputLoader.LoadFromInput("DAT.X #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.X,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_OpI_ReturnsOpI() {
            RedcodeInputLoader.LoadFromInput("DAT.I #0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.I,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrNone_ReturnsAddrDirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A 0, 0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrDirect_ReturnsAddrDirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A $0, $0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrIndirect_ReturnsAddrIndirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A @0, @0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.INDIRECT, 0),
                    new Operation(AddressingMode.INDIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrPredecIndirect_ReturnsAddrPredecIndirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A <0, <0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.PREDECREMENT_INDIRECT, 0),
                    new Operation(AddressingMode.PREDECREMENT_INDIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrPostincIndirect_ReturnsAddrPostincIndirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A >0, >0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.POSTINCREMENT_INDIRECT, 0),
                    new Operation(AddressingMode.POSTINCREMENT_INDIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrMixed_ReturnsAddrDirectImmediate() {
            RedcodeInputLoader.LoadFromInput("DAT.A 0, #0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.DIRECT, 0),
                    new Operation(AddressingMode.IMMEDIATE, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrMixed_ReturnsAddrImmediateDirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A #0, 0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.IMMEDIATE, 0),
                    new Operation(AddressingMode.DIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }

        [TestMethod]
        public void LoadFromInput_AddrMixed_ReturnsAddrIndirectPostincIndirect() {
            RedcodeInputLoader.LoadFromInput("DAT.A @0, >0\n");
            Assert.AreEqual(
                new Instruction(
                    OpCode.DAT,
                    OpModifier.A,
                    new Operation(AddressingMode.INDIRECT, 0),
                    new Operation(AddressingMode.POSTINCREMENT_INDIRECT, 0)
                ),
                vm.InstructionAt(0, false));
        }
    }
}
