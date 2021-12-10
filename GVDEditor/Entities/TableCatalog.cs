namespace GVDEditor.Entities;

/// <summary>
///     Definuje katalogovu tabulu
/// </summary>
public sealed class TableCatalog : ITable
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    public TableCatalog()
    {
        Segments = new List<TableSegment>();
        Items = new List<TableItem>();
        ViewTypeTabs = new List<TableViewTypeTab>();
    }

    /// <summary>
    ///     Vyrobca a typ tabule
    /// </summary>
    public TableManufacturer Manufacturer { get; set; }

    /// <summary>
    ///     Segmenty katalogovej tabule
    /// </summary>
    public List<TableSegment> Segments { get; set; }

    /// <summary>
    ///     Minimalna vyska segmentu
    /// </summary>
    public int MinHeight { get; set; }

    /// <summary>
    ///     Pocet segmentov na tabuli
    /// </summary>
    public int NumSegments { get; set; }

    /// <summary>
    ///     Max pocet zaznamov, ktore mozu byt na tabuli v jednom case
    /// </summary>
    public int MaxRecCount { get; set; }

    /// <summary>
    ///     Itemy (polozky/stlpce) katalogovej tabule
    /// </summary>
    public List<TableItem> Items { get; set; }

    /// <summary>
    ///     Definuje typ, mod zobrazenia a pocet riadkov na zaznam katalogovej tabuli
    /// </summary>
    public List<TableViewTypeTab> ViewTypeTabs { get; set; }

    /// <summary>
    ///     Kluc katalogovej tabule
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Nazov katalogovej tabule
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Textovy komentar ku tabuli
    /// </summary>
    public string Comment { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}