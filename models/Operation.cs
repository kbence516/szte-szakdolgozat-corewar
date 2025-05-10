public class Operation {
    public AddressingMode Mode {
        get; private set;
    }
    public int Value {
        get; set;
    }

    public Operation(AddressingMode mode, int value) {
        Mode = mode;
        Value = value;
    }

    public void Copy(Operation other) {
        Mode = other.Mode;
        Value = other.Value;
    }

    public override bool Equals(object? obj) {
        return obj is Operation operation &&
               Mode == operation.Mode &&
               Value == operation.Value;
    }

    public override string ToString() {
        return $"{(char)Mode}{Value}";
    }
}