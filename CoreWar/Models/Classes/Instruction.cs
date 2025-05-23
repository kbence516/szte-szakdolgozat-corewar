namespace CoreWar {
    /// <summary>
    /// Teljes utasítások reprezentálására szolgáló osztály
    /// </summary>
    public class Instruction {
        /// <summary>
        /// Az utasítás mûvelete
        /// </summary>
        public OpCode OpCode {
            get; private set;
        }

        /// <summary>
        /// A mûvelet módosítója
        /// </summary>
        public OpModifier Modifier {
            get; private set;
        }
        /// <summary>
        /// A operandus
        /// </summary>
        public Operation OpA {
            get; private set;
        }

        /// <summary>
        /// A operandus
        /// </summary>
        public Operation OpB {
            get; private set;
        }

        public Instruction(OpCode opcode, OpModifier modifier, Operation opA, Operation opB) {
            OpCode = opcode;
            Modifier = modifier;
            OpA = opA;
            OpB = opB;
        }

        /// <summary>
        /// DAT.F #0, #0 utasítást létrehozó konstruktor
        /// </summary>
        public Instruction() {
            OpCode = OpCode.DAT;
            Modifier = OpModifier.F;
            OpA = new Operation(AddressingMode.IMMEDIATE, 0);
            OpB = new Operation(AddressingMode.IMMEDIATE, 0);
        }

        /// <summary>
        /// A operandus értéke
        /// </summary>
        public int GetA() {
            VM vm = VM.GetInstance();
            return OpA.Mode switch {
                AddressingMode.DIRECT => OpA.Value,
                AddressingMode.IMMEDIATE => 0,
                AddressingMode.INDIRECT => vm.InstructionAt(OpA.Value, true).OpA.Value,
                AddressingMode.PREDECREMENT_INDIRECT => vm.InstructionAt(--OpA.Value, true).OpA.Value,
                AddressingMode.POSTINCREMENT_INDIRECT => vm.InstructionAt(OpA.Value++, true).OpA.Value,
                _ => throw new Exception("Helytelen címzési mód")
            };
        }

        /// <summary>
        /// B operandus értéke
        /// </summary>
        public int GetB() {
            VM vm = VM.GetInstance();
            return OpB.Mode switch {
                AddressingMode.DIRECT => OpB.Value,
                AddressingMode.IMMEDIATE => 0,
                AddressingMode.INDIRECT => vm.InstructionAt(OpB.Value, true).OpB.Value,
                AddressingMode.PREDECREMENT_INDIRECT => vm.InstructionAt(--OpB.Value, true).OpB.Value,
                AddressingMode.POSTINCREMENT_INDIRECT => vm.InstructionAt(OpB.Value++, true).OpB.Value,
                _ => throw new Exception("Helytelen címzési mód")
            };
        }

        /// <summary>
        /// Operandus értékeinek átmásolása egy forrás operandusból
        /// </summary>
        /// <param name="source">A forrás operandus</param>
        public void Copy(Instruction source) {
            OpCode = source.OpCode;
            Modifier = source.Modifier;
            OpA.Copy(source.OpA);
            OpB.Copy(source.OpB);
        }

        public override string ToString() {
            return $"{OpCode}.{Modifier}  {OpA}  {OpB}";
        }

        public override bool Equals(object? obj) {
            if (this == null || obj == null) {
                return this == obj;
            }
            return obj is Instruction instruction &&
                OpCode == instruction.OpCode &&
                Modifier == instruction.Modifier &&
                OpA.Equals(instruction.OpA) &&
                OpB.Equals(instruction.OpB);
        }
    }
}