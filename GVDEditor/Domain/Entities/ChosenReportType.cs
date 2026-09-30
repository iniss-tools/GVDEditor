namespace GVDEditor.Domain.Entities;

/// <summary>
/// Definuje vybrane typy reportov
/// </summary>
public sealed class ChosenReportType
{
    /// <summary>
    /// Konstruktor
    /// </summary>
    public ChosenReportType()
    {
        Variants = [];
    }

    /// <summary>
    /// Typ reportu
    /// </summary>
    public ReportType Type { get; set; } = null!;

    /// <summary>
    /// Varianty reportu
    /// </summary>
    public List<ReportVariant> Variants { get; set; }
}