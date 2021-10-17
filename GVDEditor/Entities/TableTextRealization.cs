namespace GVDEditor.Entities
{
    /// <summary>
    ///     Realizacia typu textu na tabuliach.
    /// </summary>
    public class TableTextRealization
    {
        /// <summary>
        ///     Katalogova tabula, na ktorej sa dany typ textu zobrazuje.
        /// </summary>
        public TableCatalog Table { get; set; }

        /// <summary>
        ///     Konkretny item (polozka) na katalogovej tabuli, na ktorej sa dany typ textu zobrazuje.
        /// </summary>
        public TableItem Item { get; set; }
    }
}