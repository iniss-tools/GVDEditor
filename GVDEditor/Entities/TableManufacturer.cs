using ToolsCore.Tools;

namespace GVDEditor.Entities
{
    /// <summary>
    ///     Vyrobca tabule, ktory sa definuje v katalogovej tabuli
    /// </summary>
    public sealed class TableManufacturer : Enumeration<TableManufacturer>
    {
        private TableManufacturer(int id, string name) : base(id, name)
        {
        }

        /// <summary>
        ///     Odkaz na seba, pouzivane pre DataSource.
        /// </summary>
        public TableManufacturer This => this;

        /// <summary>
        ///     Konvertuje textove vyjadrenie nazvu typu tabule na objekt. V pripade, ak sa to nepodari, metoda vrati
        ///     <see langword="null" />.
        /// </summary>
        /// <param name="s">Vstupny retazec.</param>
        /// <returns></returns>
        public new static TableManufacturer Parse(string s)
        {
            return s switch
            {
                "ELEN" => ELEN,
                "ELEN10" => ELEN10,
                "ELENOLD" => ELENOLD,
                "ELEN16" => ELEN16,
                "ELEN16Kam" => ELEN16Kam,
                "Elektrocas" => ELEKTROCAS,
                "LCD1" => LCD1,
                "Pragotron" => Pragotron,
                "ERS" => ERS,
                "FERS" => FERS,
                _ => null
            };
        }

        #region VALUES

#pragma warning disable 1591
        public static readonly TableManufacturer ELEN = new(0, "ELEN");
        public static readonly TableManufacturer ELEN10 = new(1, "ELEN10");
        public static readonly TableManufacturer ELENOLD = new(2, "ELENOLD");
        public static readonly TableManufacturer ELEN16 = new(3, "ELEN16");
        public static readonly TableManufacturer ELEN16Kam = new(4, "ELENKam16");
        public static readonly TableManufacturer LCD1 = new(5, "LCD1");
        public static readonly TableManufacturer ELEKTROCAS = new(6, "Elektrocas");
        public static readonly TableManufacturer Pragotron = new(7, "Pragotron");
        public static readonly TableManufacturer ERS = new(8, "ERS");
        public static readonly TableManufacturer FERS = new(9, "FERS");
#pragma warning restore 1591

        #endregion
    }
}