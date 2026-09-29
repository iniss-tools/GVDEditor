namespace GVDEditor.Domain.Entities;

/// <summary>
/// Typ reportu
/// </summary>
public sealed record ReportType
{
    /// <summary>
    /// Konstruktor
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
    /// Kluc reportu
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Nazov reportu
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Znak reportu
    /// </summary>
    public string Char { get; }

    /// <summary>
    /// </summary>
    public bool BaseTrain { get; }

    /// <summary>
    /// </summary>
    public bool PassThrough { get; }

    /// <summary>
    /// Nastavi alebo zisti, ci sa da v danom reporte ukončiť vlak
    /// </summary>
    public bool TerminateTrain { get; }

    /// <summary>
    /// </summary>
    public bool Complement { get; }

    /// <summary>
    /// Priznak LOCKOUT_BASE z Categori.TXT. INISS ho nacita, ale nikde nepouzije; zachovava sa.
    /// </summary>
    public bool LockoutBase { get; init; }

    /// <summary>
    /// Priznak LOCKOUT_THROUGH z Categori.TXT. INISS ho nacita, ale nikde nepouzije; zachovava sa.
    /// </summary>
    public bool LockoutThrough { get; init; }

    /// <summary>
    /// Priznak LOCKOUT_TERMINATE z Categori.TXT. INISS ho nacita, ale nikde nepouzije; zachovava sa.
    /// </summary>
    public bool LockoutTerminate { get; init; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    /// Vrati predvolene typy reportov pre SK ako list
    /// </summary>
    /// <returns>list reportov pre Slovensko</returns>
    public static List<ReportType> GetDefaultValuesSk()
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
    /// Vyberie zo zoznamu reportov (alltypes) vybrane reporty podla znaku reportu (toparse)
    /// </summary>
    /// <param name="allTypes"></param>
    /// <param name="toparse"></param>
    /// <returns></returns>
    /// <param name="variants">varianty hlaseni grafikonu (prvy = velke pismeno, druhy = male)</param>
    public static List<ChosenReportType> Parse(IEnumerable<ReportType> allTypes, string toparse, IList<ReportVariant> variants)
    {
        var reports = new List<ChosenReportType>();

        foreach (var reportType in allTypes)
        {
            if (toparse.Contains(reportType.Char.ToUpperInvariant(), StringComparison.Ordinal))
            {
                var found = false;
                foreach (var chosenReportType in reports.Where(chosenReportType => reportType.Equals(chosenReportType.Type)))
                {
                    chosenReportType.Variants.Add(variants.ElementAtOrDefault(0)!);
                    found = true;
                }

                if (!found)
                    reports.Add(new ChosenReportType
                    {
                        Type = reportType,
                        Variants = [variants.ElementAtOrDefault(0)!]
                    });
            }

            if (toparse.Contains(reportType.Char.ToLowerInvariant(), StringComparison.Ordinal))
            {
                var found = false;
                foreach (var chosenReportType in reports.Where(chosenReportType => reportType.Equals(chosenReportType.Type)))
                {
                    chosenReportType.Variants.Add(variants.ElementAtOrDefault(1)!);
                    found = true;
                }

                if (!found)
                    reports.Add(new ChosenReportType
                    {
                        Type = reportType,
                        Variants = [variants.ElementAtOrDefault(1)!]
                    });
            }
        }

        return reports;
    }

    public static readonly ReportType Prichadza = new("Přijíždí", "Přijíždí", "P");
    public static readonly ReportType Vchadza = new("Vjíždí", "Vjíždí", "I");
    public static readonly ReportType Zastavil = new("Zastavil", "Zastavil", "L");
    public static readonly ReportType Stoji = new("Pobytové", "Pobytové", "N", terminateTrain: false);
    public static readonly ReportType Odchadza = new("Odjede", "Ukončit nástup", "O", terminateTrain: false);
}