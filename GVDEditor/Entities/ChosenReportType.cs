namespace GVDEditor.Entities;

/// <summary>
///     Definuje vybrane typy reportov
/// </summary>
public sealed class ChosenReportType
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    public ChosenReportType()
    {
        Variants = new List<ReportVariant>();
    }

    /// <summary>
    ///     Typ reportu
    /// </summary>
    public ReportType Type { get; set; } = null!;

    /// <summary>
    ///     Varianty reportu
    /// </summary>
    public List<ReportVariant> Variants { get; set; }
}