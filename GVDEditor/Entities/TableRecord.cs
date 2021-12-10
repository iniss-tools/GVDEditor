using System.Collections;

namespace GVDEditor.Entities;

/// <summary>
///     Reprezentuje zaznam tabule.
/// </summary>
public sealed class TableRecord : IEnumerable
{
    /// <summary>
    ///     Pozicie zaznamu tabule.
    /// </summary>
    public List<TablePosition> Positions { get; set; } = new();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => Positions.GetEnumerator();
}