using ToolsCore.Entities;

namespace GVDEditor.Entities;

/// <summary>
///     Radenie vlaku
/// </summary>
public sealed class Radenie
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    public Radenie()
    {
        Sounds = new List<FyzSound>();
        ChosenReports = new List<ChosenReportType>();
    }

    /// <summary>
    ///     Začiatok platnosti radenia vlaku
    /// </summary>
    public DateTime ZacPlatnosti { get; set; }

    /// <summary>
    ///     Koniec platnosti radenia vlaku
    /// </summary>
    public DateTime KonPlatnosti { get; set; }

    /// <summary>
    ///     Dátumové obmedzenie ako text
    /// </summary>
    public string DatObm { get; set; }

    /// <summary>
    ///     Fyzické zvuky, ktoré tvoria hlásenie radenia vlaku
    /// </summary>
    public List<FyzSound> Sounds { get; set; }

    /// <summary>
    ///     V ktorých variantach hlásení sa má radenie vlaku vyhlasovať
    /// </summary>
    public List<ChosenReportType> ChosenReports { get; set; }

    /// <summary>
    ///     Text hlásenia vlaku
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Cieľová stanica vlaku s radením
    /// </summary>
    public Station DestStation { get; set; }

    /// <summary>
    ///     Číslo vlaku, ktorému patrí toto radenie
    /// </summary>
    public string CisloVlaku { get; set; }

    /// <summary>
    ///     Konveruje list fyzických zvukov radenia do reťazca  - text hlasenia
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