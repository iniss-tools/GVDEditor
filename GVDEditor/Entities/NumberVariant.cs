namespace GVDEditor.Entities;

/// <summary>
///     Cislo a varianta vlaku.
/// </summary>
public struct NumberVariant : IComparable
{
    /// <summary>
    ///     Cislo vlaku.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///     Varianta vlaku.
    /// </summary>
    public int Variant { get; set; }

    /// <inheritdoc />
    public override string ToString() => Variant == -1 ? $"{Number}" : $"{Number} v{Variant}";

    /// <inheritdoc />
    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        var numberVariant = (NumberVariant)obj;
        int compared;
        if (decimal.TryParse(Number, out var ln) && decimal.TryParse(numberVariant.Number, out var rn))
            compared = ln.CompareTo(rn);
        else
            compared = string.Compare(Number, numberVariant.Number, StringComparison.Ordinal);
        return compared == 0 ? Variant.CompareTo(numberVariant.Variant) : compared;
    }
}