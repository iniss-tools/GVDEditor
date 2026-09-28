using System.Globalization;

namespace GVDEditor.Domain.Entities;

/// <summary>
///     Cislo a varianta vlaku - zobrazuje sa v jednom stlpci zoznamu vlakov a triedi sa podla cisla, potom varianty.
/// </summary>
/// <param name="Number">Cislo vlaku.</param>
/// <param name="Variant">Varianta vlaku; -1 = vlak nema varianty.</param>
public readonly record struct NumberVariant(string Number, int Variant) : IComparable, IComparable<NumberVariant>
{
    /// <inheritdoc />
    public override string ToString() => Variant == -1 ? $"{Number}" : $"{Number} v{Variant}";

    /// <inheritdoc />
    public int CompareTo(object? obj) => obj is NumberVariant other ? CompareTo(other) : 1;

    /// <inheritdoc />
    public int CompareTo(NumberVariant other)
    {
        var compared = decimal.TryParse(Number, NumberStyles.Number, CultureInfo.InvariantCulture, out var ln) &&
                       decimal.TryParse(other.Number, NumberStyles.Number, CultureInfo.InvariantCulture, out var rn)
            ? ln.CompareTo(rn)
            : string.Compare(Number, other.Number, StringComparison.Ordinal);

        return compared == 0 ? Variant.CompareTo(other.Variant) : compared;
    }

#pragma warning disable 1591
    public static bool operator <(NumberVariant left, NumberVariant right) => left.CompareTo(right) < 0;
    public static bool operator <=(NumberVariant left, NumberVariant right) => left.CompareTo(right) <= 0;
    public static bool operator >(NumberVariant left, NumberVariant right) => left.CompareTo(right) > 0;
    public static bool operator >=(NumberVariant left, NumberVariant right) => left.CompareTo(right) >= 0;
#pragma warning restore 1591
}
