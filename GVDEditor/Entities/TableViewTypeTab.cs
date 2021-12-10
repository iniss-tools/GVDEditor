using System.Collections;

namespace GVDEditor.Entities;

/// <summary>
///     Definuje typ, mod zobrazenia a pocet riadkov na zaznam katalogovej tabuli.
/// </summary>
public sealed class TableViewTypeTab : IEnumerable
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    public TableViewTypeTab()
    {
        TypeModeItems = new List<TableTypeModeItem>();
    }

    /// <summary>
    ///     Typ zobrazenia.
    /// </summary>
    public TableViewType ViewType { get; set; }

    /// <summary>
    ///     Pocet riadkov, kolko ma 1 zaznam na tabuli.
    /// </summary>
    public string CountLinesRecord { get; set; }

    /// <summary>
    ///     Mody zobrazenia zaznamu.
    /// </summary>
    public List<TableTypeModeItem> TypeModeItems { get; }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => TypeModeItems.GetEnumerator();
}