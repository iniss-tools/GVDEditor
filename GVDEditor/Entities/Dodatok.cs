using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Trieda obsahujuca informacie o dodatkovom hláseni.
/// </summary>
public sealed class Dodatok
{
    /// <summary>
    ///     Vytvori novu instanciu triedy <see cref="Dodatok"/>.
    /// </summary>
    public Dodatok() => ChosenReports = new List<ChosenReportType>();

    /// <summary>
    ///     Vrati alebo nastavi fyzický zvuk dodatku.
    /// </summary>
    public FyzSound Sound { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi názov dodatkového hlásenia (zvyčajne Dxxxx, kde x - 0-9).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Vrati alebo nastavi v akých reportoch sa má dodatok hlásiť.
    /// </summary>
    public List<ChosenReportType> ChosenReports { get; set; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    ///     Konvertuje binárne pole na dodatkové hlásenie zo všetkými dátami.
    /// </summary>
    /// <param name="sound">Fyzický zvuk.</param>
    /// <param name="nums">Binárne pole vo forme reťazca.</param>
    /// <param name="reportTypes">Dostupné typy reportov.</param>
    /// <param name="reportVariants">Dostupné varianty reportov.</param>
    /// <param name="smerovanie">Smerovanie vlaku.</param>
    /// <returns>dodatkové hlásenie.</returns>
    /// <exception cref="FormatException"></exception>
    public static Dodatok NumsToDodatok(FyzSound sound, string nums, List<ReportType> reportTypes, List<ReportVariant> reportVariants,
        Routing smerovanie)
    {
        var dodatok = new Dodatok { Sound = sound, Name = sound.Name.Replace("D", "") };

        if (!Utils.IsInt(nums)) 
            throw new FormatException("Pole dodatku neobsahuje iba čísla.");

        List<ReportType> rightTypes;
        if (smerovanie == Routing.Vychadzajuci)
            rightTypes = reportTypes.Where(t => t.BaseTrain).ToList();
        else if (smerovanie == Routing.Prechadzajuci)
            rightTypes = reportTypes.Where(t => t.PassThrough).ToList();
        else
            rightTypes = reportTypes.Where(t => t.TerminateTrain).ToList();

        var rightMapLength = rightTypes.Count * reportVariants.Count;

        if (rightMapLength != nums.Length)
            throw new FormatException($"Mapa dodatku má nesprávnu dĺžku ({nums.Length}). Mala by mať dĺžku {rightMapLength}.");

        var ch = 0;
        for (var i = 0; i < rightTypes.Count; i++)
        {
            var vars = new List<ReportVariant>();
            foreach (var reportVariant in reportVariants)
            {
                switch (nums[ch])
                {
                    case '1':
                        vars.Add(reportVariant);
                        break;
                    case '0':
                        break;
                    default:
                        throw new FormatException("Pole dodatku musí obsahovať iba číslice 0 a 1.");
                }
                ch++;
            }

            dodatok.ChosenReports.Add(new ChosenReportType { Type = reportTypes[i], Variants = vars });
        }

        return dodatok;
    }

    /// <summary>
    ///     Konvertuje dodatkové hlásenie na binárne pole.
    /// </summary>
    /// <param name="dodatok">Dodatkové hlásenie.</param>
    /// <param name="reportTypes">Dostupné typy reportov.</param>
    /// <param name="reportVariants">Dostupné varianty reportov.</param>
    /// <returns>binárne pole vo forme reťazca</returns>
    public static string DodatokToNums(Dodatok dodatok, IList<ReportType> reportTypes, IList<ReportVariant> reportVariants)
    {
        var nums = new StringBuilder();

        foreach (var reportType in reportTypes)
        {
            var chosenReportType = dodatok.ChosenReports.FirstOrDefault(x => x.Type == reportType);
            if (chosenReportType != null)
                foreach (var variant in reportVariants)
                    nums.Append(chosenReportType.Variants.Contains(variant) ? '1' : '0');
            else
                for (var j = 0; j < reportVariants.Count; j++)
                    nums.Append('0');
        }

        return nums.ToString();
    }
}