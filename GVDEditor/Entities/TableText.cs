namespace GVDEditor.Entities;

/// <summary>
///     Trieda zastrešujúca texty na tabuliach.
/// </summary>
public sealed class TableText : ITable
{
    /// <summary>
    ///     Vytvori novu instanciu triedy <see cref="TableText"/>.
    /// </summary>
    public TableText()
    {
        Realizations = new List<TableTextRealization>();
        Trains = new List<TableTrain>();
    }

    /// <summary>
    ///     Vrati alebo nastavi kluc typu textu na tabuliach.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi nazov typu textu na tabuliach.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi komentar ku danemu typu textu na tabuliach.
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi realizacie tohto typu textu na tabuliach.
    /// </summary>
    public List<TableTextRealization> Realizations { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi vlaky (<see cref="TableTrain" />), ktore maju tento typ textu na tabuliach.
    /// </summary>
    public List<TableTrain> Trains { get; set; }

    /// <inheritdoc/>
    public string TypeName => "Text tabule";

    /// <inheritdoc />
    public override string ToString() => Name;
}