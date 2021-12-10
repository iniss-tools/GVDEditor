namespace GVDEditor.Entities;

/// <summary>
///     Definuje jeden stlpec katalogovej tabule a jeho vlastnosti.
/// </summary>
public sealed class TableItem : ITable
{
    /// <summary>
    ///     Kluc polozky.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Nazov stlpca.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Typ obsahu stlpca (aky typ dat ma stlpec obsahovat).
    /// </summary>
    public TableFillSection FillSection { get; set; }

    /// <summary>
    ///     Riadok zaznamu, na ktorom sa tento stlpec zobrazuje (zero-based).
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    ///     Pociatocny pixel (odkial stlpec zacina) (prvy pixel je 0).
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    ///     Posledny pixel (kde stlpec konci).
    /// </summary>
    public int End { get; set; }

    /// <summary>
    ///     Identifikator pisma.
    /// </summary>
    public int FontIDX { get; set; }

    /// <summary>
    ///     Zarovnanie pisma.
    /// </summary>
    public TableAlign Align { get; set; }

    /// <summary>
    ///     Typ zadavania udajov do polozky.
    /// </summary>
    public TableDivType DivType { get; set; }

    /// <summary>
    ///     Referencia na TabTab1.
    /// </summary>
    public TableTabTab Tab1 { get; set; }

    /// <summary>
    ///     Referencia na TabTab2.
    /// </summary>
    public TableTabTab Tab2 { get; set; }

    /// <inheritdoc />
    public override string ToString() => Name;
}