using System.Collections;

namespace GVDEditor.Entities;

/// <summary>
/// </summary>
public sealed class TableTypeModeItem : IEnumerable
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    public TableTypeModeItem()
    {
        ItemsKeys = new List<string>();
    }

    /// <summary>
    ///     Mod zobrazenia polozky.
    /// </summary>
    public TableViewMode ViewMode { get; set; }

    /// <summary>
    /// </summary>
    public List<string> ItemsKeys { get; set; }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => ItemsKeys.GetEnumerator();
}