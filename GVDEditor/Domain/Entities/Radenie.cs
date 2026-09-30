using ToolsCore.Iniss.Entities;

namespace GVDEditor.Domain.Entities;

/// <summary>
/// Radenie vlaku
/// </summary>
public sealed class Radenie
{
    /// <summary>
    /// Konstruktor
    /// </summary>
    public Radenie()
    {
        Sounds = [];
        ChosenReports = [];
    }

    /// <summary>
    /// Obdobie platnosti radenia vlaku; <see langword="null" /> = plati bez obmedzenia. Zaznam s prazdnymi datumami
    /// v Razeni1.txt INISS pri datume vobec nekontroluje.
    /// </summary>
    public ValidityPeriod? Validity { get; set; }

    /// <summary>
    /// Ci ma radenie zadane obdobie platnosti.
    /// </summary>
    public bool HasValidity => Validity is not null;

    /// <summary>
    /// Dátumové obmedzenie ako text
    /// </summary>
    public string DatObm { get; set; } = null!;

    /// <summary>
    /// Fyzické zvuky, ktoré tvoria hlásenie radenia vlaku
    /// </summary>
    public List<FyzSound> Sounds { get; set; }

    /// <summary>
    /// V ktorých variantach hlásení sa má radenie vlaku vyhlasovať
    /// </summary>
    public List<ChosenReportType> ChosenReports { get; set; }

    /// <summary>
    /// Text hlásenia vlaku
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Cieľová stanica vlaku s radením
    /// </summary>
    public Station DestStation { get; set; } = null!;

    /// <summary>
    /// Číslo vlaku, ktorému patrí toto radenie
    /// </summary>
    public string CisloVlaku { get; set; } = null!;

    /// <summary>
    /// Konveruje list fyzických zvukov radenia do reťazca  - text hlasenia
    /// </summary>
    /// <param name="sounds"></param>
    /// <returns>text hlasenia radenia</returns>
    public static string SoundsToString(IEnumerable<FyzSound> sounds)
    {
        var sb = new StringBuilder();
        foreach (var fyzZvuk in sounds) sb.Append(fyzZvuk.Text + " ");

        return sb.ToString().Trim();
    }
}