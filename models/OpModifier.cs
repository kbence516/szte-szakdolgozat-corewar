public enum OpModifier {
    A,          // Csak opA-ra vonatkozik
    B,          // Csak opB-re vonatkozik
    AB,         // opA -> opB
    BA,         // opB -> opA
    F,          // Csak a mezőre vonatkozik (az utasításra és a címzésmódra nem)
    X,          // opA -> opB, opB -> opA
    I           // Az egész cellára vonatkozik
}