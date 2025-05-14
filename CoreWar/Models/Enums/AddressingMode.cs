namespace CoreWar {
    public enum AddressingMode {
        IMMEDIATE = '#',
        DIRECT = '$',                         // alapértelmezett
        INDIRECT = '@',
        PREDECREMENT_INDIRECT = '<',
        POSTINCREMENT_INDIRECT = '>'
    }
}