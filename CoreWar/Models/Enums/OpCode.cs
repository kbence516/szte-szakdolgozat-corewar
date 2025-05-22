namespace CoreWar {
    public enum OpCode {
        DAT,            // processzus megállítása
        MOV,            // adatmásolás
        ADD,            // hozzáadás
        SUB,            // kivonás
        MUL,            // szorzás
        DIV,            // egészosztás
        MOD,            // modulo
        JMP,            // feltétel nélküli ugrás
        JMZ,            // ugrás, ha az eredmény 0
        JMN,            // ugrás, ha az eredmény nem 0
        DJN,            // csökkentés 1-gyel, majd ugrás, ha az eredmény nem 0
        CMP,            // ugyanaz, mint az SEQ, csak ezt nem érdemes használni
        SLT,            // következő ugrása, ha kisebb
        SPL,            // új processzus indítása
        SEQ,            // következő ugrása, ha egyenlő
        SNE,            // következő ugrása, ha nem egyenlő
        NOP,             // nem csinál semmit
        ORG,            // processzus kezdőcíme
        END             // program vége
    }
}