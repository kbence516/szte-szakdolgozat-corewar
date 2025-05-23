namespace CoreWar {

    /// <summary>
    /// Az utasítás műveleteit reprezentáló felsorolás
    /// </summary>
    public enum OpCode {
        /// <summary>Processzus megállítása</summary>
        DAT,

        /// <summary>Adatmásolás</summary>
        MOV,

        /// <summary>Hozzáadás</summary>
        ADD,

        /// <summary>Kivonás</summary>
        SUB,

        /// <summary>Szorzás</summary>
        MUL,

        /// <summary>Egészosztás</summary>
        DIV,

        /// <summary>Modulo</summary>
        MOD,

        /// <summary>Feltétel nélküli ugrás</summary>
        JMP,

        /// <summary>Ugrás, ha az eredmény 0</summary>
        JMZ,

        /// <summary>Ugrás, ha az eredmény nem 0</summary>
        JMN,

        /// <summary>Csökkentés 1-gyel, majd ugrás, ha az eredmény nem 0</summary>
        DJN,

        /// <summary>Következő ugrása, ha egyenlő</summary>
        CMP,

        /// <summary>Következő ugrása, ha kisebb</summary>
        SLT,

        /// <summary>Új processzus indítása</summary>
        SPL,

        /// <summary>Következő ugrása, ha egyenlő</summary>
        SEQ,

        /// <summary>Következő ugrása, ha nem egyenlő</summary>
        SNE,

        /// <summary>Nem csinál semmit</summary>
        NOP,

        /// <summary>Program kezdőcíme</summary>
        ORG,

        /// <summary>Program vége</summary>
        END
    }
}