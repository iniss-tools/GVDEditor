namespace GVDEditor.Entities
{
    /// <summary>
    ///     Písmo pre tabule.
    /// </summary>
    public sealed class TableFont
    {
        /// <summary>
        ///     Názov písma.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Identifikator písma.
        /// </summary>
        public int FontID { get; set; }

        /// <summary>
        ///     Typ písma.
        /// </summary>
        public TableFontType Type { get; set; }

        /// <summary>
        ///     Veľkosť písma.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     Širka písma.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Či je písmo proporcionálne.
        /// </summary>
        public bool IsProportional { get; set; }

        /// <summary>
        ///     Či písmo diakritické znamienka.
        /// </summary>
        public bool IsDia { get; set; }

        /// <summary>
        ///     Či písmo obsahuje malé písmená.
        /// </summary>
        public bool IsLower { get; set; }

        /// <summary>
        ///     Či písmo obsahuje veľké písmená.
        /// </summary>
        public bool IsUpper { get; set; }

        /// <summary>
        ///     Či písmo obsahuje čísla.
        /// </summary>
        public bool IsNumber { get; set; }

        /// <summary>
        ///     Či písmo obsahuje špeciálne znaky.
        /// </summary>
        public bool IsSpecChars { get; set; }

        /// <summary>
        ///     Či má písmo špeciálny účel.
        /// </summary>
        public bool IsSpecAssigment { get; set; }

        /// <summary>
        ///     Názov súboru s týmto písmom.
        /// </summary>
        public string FileName { get; set; }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}