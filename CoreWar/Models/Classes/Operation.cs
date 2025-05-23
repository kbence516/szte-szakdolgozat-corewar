namespace CoreWar {

    /// <summary>
    /// Egy utas�t�s operandus�t reprezent�l� oszt�ly
    /// </summary>
    public class Operation {
        /// <summary>
        /// Az operandus c�mz�si m�dja
        /// </summary>
        public AddressingMode Mode {
            get; private set;
        }

        /// <summary>
        /// A operandus �rt�ke
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