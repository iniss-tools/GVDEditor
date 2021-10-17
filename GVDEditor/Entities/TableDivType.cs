using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Definuje sposob zadavania informacii do sekcie katalogovej tabule.
    /// </summary>
    public sealed class TableDivType : Enumeration<TableDivType>
    {
        private TableDivType(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        ///     Skonvertuje ID DivType na objekt
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TableDivType Parse(int s)
        {
            return s switch
            {
                0 => Free,
                1 => Table,
                2 => TableTime,
                3 => Translate,
                4 => Char,
                _ => null
            };
        }

        #region VALUES

        /// <summary>
        ///     Zadanie ľub. textu (bez TAB1 a TAB2)
        /// </summary>
        public static readonly TableDivType Free = new(0, "0: Zadanie ľub. textu (bez TAB1 a TAB2)");

        /// <summary>
        ///     Výber z TabTab (TAB1 povinné)
        /// </summary>
        public static readonly TableDivType Table = new(1, "1: Výber z TabTab (TAB1 povinné)");

        /// <summary>
        ///     ? (TAB1 a TAB2 povinné)
        /// </summary>
        public static readonly TableDivType TableTime = new(2, "2: ? (TAB1 a TAB2 povinné)");

        /// <summary>
        ///     Zadanie ľub. textu (TAB1 povinné)
        /// </summary>
        public static readonly TableDivType Translate = new(3, "3: Zadanie ľub. textu (TAB1 povinné)");

        /// <summary>
        ///     ? (TAB1 povinné)
        /// </summary>
        public static readonly TableDivType Char = new(4, "4: ? (TAB1 povinné)");

        #endregion
    }
}