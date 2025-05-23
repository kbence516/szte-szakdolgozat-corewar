namespace CoreWar {

    /// <summary>
    /// Egy utasítás operandusát reprezentáló osztály
    /// </summary>
    public class Operation {
        /// <summary>
        /// Az operandus címzési módja
        /// </summary>
        public AddressingMode Mode {
            get; private set;
        }

        /// <summary>
        /// A operandus értéke
        /// </summary>
        public int Value {
            get; set;
        }

        public Operation(AddressingMode mode, int value) {
            Mode = mode;
            Value = value;
        }

        public void Copy(Operation source) {
            Mode = source.Mode;
            Value = source.Value;
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
}