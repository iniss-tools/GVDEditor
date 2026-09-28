using ExControls;
using GVDEditor.Domain.Entities;
using ToolsCore.Entities;

namespace GVDEditor.Domain.Documents;

/// <summary>
/// Data otvoreneho grafikonu - od jeho otvorenia po otvorenie ineho. Nacitanie grafikonu naplni novy dokument
/// a hlavne okno ho vymeni naraz, takze sa nikdy nezmiesaju data stareho a noveho grafikonu.
/// </summary>
internal sealed class GrafikonDocument
{
    /// <summary>
    /// Vlaky grafikonu (Export3A.txt a suvisiace subory).
    /// </summary>
    public ExBindingList<Train> Trains { get; set; } = new TrainBindingList();

    public ExBindingList<Track> Tracks { get; set; } = [];
    public ExBindingList<Platform> Platforms { get; set; } = [];
    public ExBindingList<Operator> Operators { get; set; } = [];

    /// <summary>
    /// Stanice definovane v grafikone (Stanice.txt), ktore nie su vo zvukovej banke.
    /// </summary>
    public ExBindingList<Station> CustomStations { get; set; } = [];

    public ExBindingList<TableTabTab> TabTabs { get; set; } = [];
    public ExBindingList<TableCatalog> TableCatalogs { get; set; } = [];
    public ExBindingList<TablePhysical> TablePhysicals { get; set; } = [];
    public ExBindingList<TableLogical> TableLogicals { get; set; } = [];
    public ExBindingList<TableText> TableTexts { get; set; } = [];
    public ExBindingList<TableFont> TableFonts { get; set; } = [];

    /// <summary>
    /// Priecinok s pismami tabul (ModeTabs.txt, sekcia FONT, PATH); prazdny = predvoleny.
    /// </summary>
    public string TableFontDir { get; set; } = "";

    /// <summary>
    /// Ciselniky z ModeTabs.TXT mimo sekcii MAIN a FONT ([VIEW_MODE], [VIEW_TYPE], [FILL_SECTION], [MANUFACTURER],
    /// [ALIGN]...), tak ako boli v subore. Pri ulozeni sa zapisu spat nezmenene, aby sa nestratili polozky,
    /// ktore GVDEditor nepozna (napr. FILL_SECTION 30-33 alebo vyrobcovia tabul).
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> ModeTabsSections { get; set; } = [];

    public List<ReportVariant> ReportVariants { get; set; } = [];
    public List<ReportType> ReportTypes { get; set; } = [];

    /// <summary>
    /// Typy hlaseni pre vychadzajuci vlak (v poradi <see cref="ReportTypes" />).
    /// </summary>
    public List<ReportType> ReportTypesV => ReportTypes.Where(type => type.BaseTrain).ToList();

    /// <summary>
    /// Typy hlaseni pre prechadzajuci vlak (v poradi <see cref="ReportTypes" />).
    /// </summary>
    public List<ReportType> ReportTypesP => ReportTypes.Where(type => type.PassThrough).ToList();

    /// <summary>
    /// Typy hlaseni pre konciaci vlak (v poradi <see cref="ReportTypes" />).
    /// </summary>
    public List<ReportType> ReportTypesK => ReportTypes.Where(type => type.TerminateTrain).ToList();

    /// <summary>
    /// Jazyky stanice, ktore grafikon pouziva (lokalny Categori.txt).
    /// </summary>
    public List<FyzLanguage> LocalLanguages { get; set; } = [];

    public List<Radenie> Radenia { get; set; } = [];

    /// <summary>
    /// Novy prazdny grafikon: len predvolene kolaj, nastupiste a dopravca, predvolene typy a varianty hlaseni
    /// a vsetky jazyky stanice.
    /// </summary>
    /// <param name="languages">jazyky stanice (globalne nastavenia)</param>
    public static GrafikonDocument CreateNew(IEnumerable<FyzLanguage> languages) => new()
    {
        Tracks = [Track.None],
        Platforms = [Platform.None],
        Operators = [Operator.None],
        ReportTypes = ReportType.GetDefaultValuesSK(),
        ReportVariants = ReportVariant.GetDefaultValues(),
        LocalLanguages = languages.ToList()
    };
}
