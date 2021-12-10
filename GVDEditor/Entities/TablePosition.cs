namespace GVDEditor.Entities;

/// <summary>
///     Reprezentuje poziciu zaznamu tabule.
/// </summary>
public sealed class TablePosition
{
    /// <summary>
    ///     Pozicia tabule podla cisla.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    ///     Druh zaznamu.
    /// </summary>
    public TableViewType TypeView { get; set; }

    /// <summary>
    ///     Fyzicka tabula, na ktorej sa zaznam zobrazuje.
    /// </summary>
    public TablePhysical Table { get; set; }
}