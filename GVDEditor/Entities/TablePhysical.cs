namespace GVDEditor.Entities;

/// <summary>
///     Definuje fyzicku tabulu.
/// </summary>
public sealed class TablePhysical : ITable
{
    /// <summary>
    ///     Identifikator tabule.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Cislo komunikacneho portu ku fyzickej tabuli.
    /// </summary>
    public int CommunicationPort { get; set; }

    /// <summary>
    ///     Pocet zaznamou, ktore dokaze tato tabula obsahovat.
    /// </summary>
    public int RecCount { get; set; }

    /// <summary>
    ///     ?
    /// </summary>
    public string ReverseArrows { get; set; }

    /// <summary>
    ///     ?
    /// </summary>
    public string Rem { get; set; }

    /// <summary>
    ///     Nazov XML suboru, ktory bude generovat INISS. Tento subor obsahuje aktualny obsah tejto tabule.
    /// </summary>
    public string SaveXML { get; set; }

    /// <summary>
    ///     Katalogova tabula definujuca obsah tejto fyzickej tabule.
    /// </summary>
    public TableCatalog TableCatalog { get; set; }

    /// <summary>
    ///     Nazov fyzickej tatule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Kluc fyzickej tatule.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Textovy komentar ku tabuli.
    /// </summary>
    public string Comment { get; set; }

    /// <inheritdoc/>
    public string TypeName => "Fyzická tabuľa";

    /// <inheritdoc />
    public override string ToString() => Name;
}