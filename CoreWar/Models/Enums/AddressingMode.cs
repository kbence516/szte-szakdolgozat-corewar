namespace CoreWar {

    /// <summary>
    /// Az operandus címzési módjait reprezentáló felsorolás
    /// </summary>
    public enum AddressingMode {
        /// <summary>
        /// Közvetlen címzés
        /// </summary>
        IMMEDIATE = '#',

        /// <summary>
        /// Direkt címzés, ez az alapértelmezett
        /// </summary>
        DIRECT = '$',

        /// <summary>
        /// Indirekt címzés
        /// </summary>
        INDIRECT = '@',

        /// <summary>
        /// Előcsökkentő indirekt címzés
        /// </summary>
        PREDECREMENT_INDIRECT = '<',

        /// <summary>
        /// Utónövelő indirekt címzés
        /// </summary>
        POSTINCREMENT_INDIRECT = '>'
    }
}