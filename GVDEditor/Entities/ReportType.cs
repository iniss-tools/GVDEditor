namespace GVDEditor.Entities;

/// <summary>
///     Typ reportu
/// </summary>
public sealed class ReportType
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    /// <param name="key"></param>
    /// <param name="name"></param>
    /// <param name="char"></param>
    /// <param name="baseTrain"></param>
    /// <param name="passThrough"></param>
    /// <param name="terminateTrain"></param>
    /// <param name="complement"></param>
    public ReportType(string key, string name, string @char, bool baseTrain = true, bool passThrough = true, bool terminateTrain = true,
        bool complement = true)
    {
        Key = key;
        Name = name;
        Char = @char;
        BaseTrain = baseTrain;
        PassThrough = passThrough;
        TerminateTrain = terminateTrain;
        Complement = complement;
    }

    /// <summary>
    ///     Kluc reportu
    /// </summary>
    public string Key { get; }

    /// <summary>
    ///     Nazov reportu
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Znak reportu
    /// </summary>
    public string Char { get; }

    /// <summary>
    /// </summary>
    public bool BaseTrain { get; }

    /// <summary>
    /// </summary>
    public bool PassThrough { get; }

    /// <summary>
    ///     Nastavi alebo zisti, ci sa da v danom reporte ukončiť vlak
    /// </summary>
    public bool TerminateTrain { get; }

    /// <summary>
    /// </summary>
    public bool Complement { get; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    ///     Vrati predvolene typy reportov pre SK ako list
    /// </summary>
    /// <returns>list reportov pre Slovensko</returns>
    public static List<ReportType> GetDefaultValuesSK()
    {
        var types = new List<ReportType>
        {
            Prichadza,
            Vchadza,
            Zastavil,
            Stoji,
            Odchadza
        };
        return types;
    }

    /// <summary>
    ///     Vyberie zo zoznamu reportov (alltypes) vybrane reporty podla znaku reportu (toparse)
    /// </summary>
    /// <param name="allTypes"></param>
    /// <param name="toparse"></param>
    /// <returns></returns>
    public static List<ChosenReportType> Parse(IEnumerable<ReportType> allTypes, string toparse)
    {
        var reports = new List<ChosenReportType>();

        foreach (var reportType in allTypes)
        {
            if (toparse.Contains(reportType.Char.ToUpper()))
            {
                var found = false;
                foreach (var chosenReportType in reports.Where(chosenReportType => reportType.Equals(chosenReportType.Type)))
                {
                    chosenReportType.Variants.Add(GlobData.ReportVariants.ElementAtOrDefault(0));
                    found = true;
                }

                if (!found)
                    reports.Add(new ChosenReportType
                    {
                        Type = reportType,
                        Variants = new List<ReportVariant> { GlobData.ReportVariants.ElementAtOrDefault(0) }
                    });
            }

            if (toparse.Contains(reportType.Char.ToLower()))
            {
                var found = false;
                foreach (var chosenReportType in reports.Where(chosenReportType => reportType.Equals(chosenReportType.Type)))
                {
                    chosenReportType.Variants.Add(GlobData.ReportVariants.ElementAtOrDefault(1));
                    found = true;
                }

                if (!found)
                    reports.Add(new ChosenReportType
                    {
                        Type = reportType,
                        Variants = new List<ReportVariant> { GlobData.ReportVariants.ElementAtOrDefault(1) }
                    });
            }
        }

        return reports;
    }

    private bool Equals(ReportType other)
    {
        return Key == other.Key && Name == other.Name && Char == other.Char && BaseTrain == other.BaseTrain &&
               PassThrough == other.PassThrough && TerminateTrain == other.TerminateTrain &&
               Complement == other.Complement;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ReportType)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Key != null ? Key.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Char != null ? Char.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ BaseTrain.GetHashCode();
            hashCode = (hashCode * 397) ^ PassThrough.GetHashCode();
            hashCode = (hashCode * 397) ^ TerminateTrain.GetHashCode();
            hashCode = (hashCode * 397) ^ Complement.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    ///     Returns a value that indicates whether the values of two <see cref="T:GVDEditor.Entities.ReportType" />
    ///     objects are equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
    ///     false.
    /// </returns>
    public static bool operator ==(ReportType left, ReportType right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Returns a value that indicates whether two <see cref="T:GVDEditor.Entities.ReportType" /> objects have
    ///     different values.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(ReportType left, ReportType right)
    {
        return !Equals(left, right);
    }
#pragma warning disable 1591
    public static readonly ReportType Prichadza = new("Přijíždí", "Přijíždí", "P");
    public static readonly ReportType Vchadza = new("Vjíždí", "Vjíždí", "I");
    public static readonly ReportType Zastavil = new("Zastavil", "Zastavil", "L");
    public static readonly ReportType Stoji = new("Pobytové", "Pobytové", "N", terminateTrain: false);
    public static readonly ReportType Odchadza = new("Ukončit nástup", "Odjede", "O", terminateTrain: false);
#pragma warning restore 1591
}