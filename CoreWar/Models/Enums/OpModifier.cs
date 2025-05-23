namespace CoreWar {
    /// <summary>
    /// Az utasítás módosítóit reprezentáló felsorolás
    /// </summary>
    public enum OpModifier {
        /// <summary>Csak opA-ra vonatkozik</summary>
        A,

        /// <summary>Csak opB-re vonatkozik</summary>
        B,

        /// <summary>opA -> opB</summary>
        AB,

        /// <summary>opB -> opA</summary>
        BA,

        /// <summary>Csak a mezőre vonatkozik (az utasításra és a címzésmódra nem)</summary>
        F,

        /// <summary>opA -> opB, opB -> opA</summary>
        X,

        /// <summary>Az egész utasításra vonatkozik</summary>
        I
    }
}