
public class Instruction {
    public OpCode OpCode {
        get; private set;
    }
    public OpModifier Modifier {
        get; private set;
    }
    public Operation OpA {
        get; private set;
    }
    public Operation OpB {
        get; private set;
    }

    public Instruction(OpCode opcode, OpModifier modifier, Operation opA, Operation opB) {
        OpCode = opcode;
        Modifier = modifier;
        OpA = opA;
        OpB = opB;
    }

    public Instruction() {
        OpCode = OpCode.DAT;
        Modifier = OpModifier.F;
        OpA = new Operation(AddressingMode.IMMEDIATE, 0);
        OpB = new Operation(AddressingMode.IMMEDIATE, 0);
    }

    public int GetA() {
        VM vm = VM.GetInstance();
        return OpA.Mode switch {
            AddressingMode.DIRECT => OpA.Value,
            AddressingMode.IMMEDIATE => 0,
            AddressingMode.INDIRECT => vm.InstructionAt(OpA.Value, true).GetA(),
            AddressingMode.PREDECREMENT_INDIRECT => vm.InstructionAt(--OpA.Value, true).GetA(),
            AddressingMode.POSTINCREMENT_INDIRECT => vm.InstructionAt(OpA.Value++, true).GetA(),
            _ => throw new Exception("Helytelen címzési mód")
        };
    }

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

    public void Copy(Instruction other) {
        OpCode = other.OpCode;
        Modifier = other.Modifier;
        OpA.Copy(other.OpA);
        OpB.Copy(other.OpB);
    }

    public override string ToString() {
        return $"{OpCode}.{Modifier}\t{OpA}\t{OpB}";
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