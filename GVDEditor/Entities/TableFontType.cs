using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Typ písma pre tabule
    /// </summary>
    public sealed class TableFontType : Enumeration<TableFontType>
    {
        private TableFontType(string key, string name) : base(key, name)
        {
        }

        /// <summary>
        ///     This
        /// </summary>
        public TableFontType This => this;

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public new static TableFontType Parse(string type)
        {
            return type switch
            {
                "" => None,
                "Tučný" => Bold,
                "Šikmý" => Italics,
                "Šikmý a tučný" => BoldItalics,
                _ => null
            };
        }

        #region VALUES

        /// <summary>
        ///     Obyčajné písmo
        /// </summary>
        public static readonly TableFontType None = new("", "Normálne");

        /// <summary>
        ///     Tučné písmo
        /// </summary>
        public static readonly TableFontType Bold = new("Tučný", "Tučné písmo");

        /// <summary>
        ///     Šikmé písmo (kurzíva)
        /// </summary>
        public static readonly TableFontType Italics = new("Šikmý", "Šikmé písmo");

        /// <summary>
        ///     Tučné a šikmé písmo
        /// </summary>
        public static readonly TableFontType BoldItalics = new("Šikmý a tučný", "Šikmé a tučné písmo");

        #endregion
    }
}