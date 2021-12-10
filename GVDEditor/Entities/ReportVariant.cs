namespace GVDEditor.Entities;

/// <summary>
///     Definuje varianty reportu
/// </summary>
public sealed class ReportVariant
{
    /// <summary>
    ///     Kluc varianty reportu
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    ///     Nazov varianty reportu
    /// </summary>
    public string Name { get; set; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    ///     Vrati predvolene hodnoty variant reportov
    /// </summary>
    /// <returns></returns>
    public static List<ReportVariant> GetDefaultValues()
    {
        var variants = new List<ReportVariant> { KratkeHlasenie, DlheHlasenie };
        return variants;
    }

    private bool Equals(ReportVariant other) => Key == other.Key && Name == other.Name;

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object. </param>
    /// <returns>
    ///     <see langword="true" /> if the specified object  is equal to the current object; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is ReportVariant other && Equals(other);
    }

    /// <summary>Serves as the default hash function. </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return (Key * 397) ^ (Name != null ? Name.GetHashCode() : 0);
        }
    }

    /// <summary>
    ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.ReportVariant" />
    ///     objects are equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
    ///     false.
    /// </returns>
    public static bool operator ==(ReportVariant left, ReportVariant right) => Equals(left, right);

    /// <summary>
    ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.ReportVariant" /> objects have
    ///     different values.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ReportVariant left, ReportVariant right) => !Equals(left, right);
#pragma warning disable 1591
    public static readonly ReportVariant KratkeHlasenie = new() { Key = 0, Name = "Krátke hlásenie" };
    public static readonly ReportVariant DlheHlasenie = new() { Key = 1, Name = "Dlhé hlásenie" };
#pragma warning restore 1591
}