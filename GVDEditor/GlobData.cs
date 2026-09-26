using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;
using GVDEditor.XML;
using ToolsCore.Entities;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor;

internal static class GlobData
{
    public static string INISSDir { get; private set; } = null!;
    public static string DataDir { get; private set; } = null!;
    public static string RawBankDir { get; private set; } = null!;

    public static List<string> INISSExeFiles { get; private set; } = null!;

    public static List<DirList> GVDDirs { get; set; } = null!;
    public static ExBindingList<Audio> Audios { get; internal set; } = null!;

    public static List<FyzSound> Sounds { get; private set; } = null!;

    /// <summary>
    ///     Texty vyluk, odklonov a dodatkov zalozene obsluhou v INISSe (RAWBANK\LogZvuk.usr).
    /// </summary>
    public static List<LogZvukText> LogZvukTexts { get; private set; } = new();
    public static ExBindingList<FyzLanguage> Languages { get; internal set; } = null!;
    public static List<FyzLanguage> LocalLanguages { get; set; } = null!;
    public static List<Station> Stations { get; set; } = null!;
    public static ExBindingList<Station> CustomStations { get; set; } = null!;

    public static ExBindingList<Operator> Operators { get; set; } = null!;

    public static ExBindingList<Train> Trains { get; set; } = new TrainBindingList();
    public static ExBindingList<Track> Tracks { get; set; } = null!;
    public static ExBindingList<Platform> Platforms { get; set; } = null!;

    public static ExBindingList<string> Delays { get; internal set; } = null!;

    public static ExBindingList<TrainType> TrainsTypes { get; internal set; } = null!;

    public static List<TrainName> TrainNames { get; private set; } = null!;

    public static ExBindingList<TableTabTab> TabTabs { get; set; } = null!;
    public static ExBindingList<TableCatalog> TableCatalogs { get; set; } = null!;
    public static ExBindingList<TablePhysical> TablePhysicals { get; set; } = null!;
    public static ExBindingList<TableLogical> TableLogicals { get; set; } = null!;
    public static ExBindingList<TableText> TableTexts { get; set; } = null!;
    public static ExBindingList<TableFont> TableFonts { get; set; } = null!;
    public static string TableFontDir { get; set; } = null!;

    /// <summary>
    ///     Ciselniky z ModeTabs.TXT mimo sekcii MAIN a FONT ([VIEW_MODE], [VIEW_TYPE], [FILL_SECTION], [MANUFACTURER],
    ///     [ALIGN]...), tak ako boli v subore. Pri ulozeni sa zapisu spat nezmenene, aby sa nestratili polozky,
    ///     ktore GVDEditor nepozna (napr. FILL_SECTION 30-33 alebo vyrobcovia tabul).
    /// </summary>
    public static Dictionary<string, Dictionary<string, string>> ModeTabsSections { get; set; } = new();

    public static List<ReportVariant> ReportVariants { get; set; } = null!;
    public static List<ReportType> ReportTypes { get; set; } = null!;

    public static List<ReportType> ReportTypesV { get; private set; } = null!;
    public static List<ReportType> ReportTypesP { get; private set; } = null!;
    public static List<ReportType> ReportTypesK { get; private set; } = null!;

    public static List<Radenie> Radenia { get; set; } = null!;

    public static GVDEditorConfig Config = null!;
    public static Styles<GVDEditorStyle> Styles = null!;
    public static GVDEditorStyle UsingStyle = null!;

    public static void PrepareGlobalData(string pathtoiniss)
    {
        Trains.FireEventOnSort = true;

        Audios = new ExBindingList<Audio>();

        Tracks = new ExBindingList<Track>();
        Platforms = new ExBindingList<Platform>();

        TrainsTypes = new ExBindingList<TrainType>();

        Operators = new ExBindingList<Operator>();

        TabTabs = new ExBindingList<TableTabTab>();
        TableCatalogs = new ExBindingList<TableCatalog>();
        TablePhysicals = new ExBindingList<TablePhysical>();
        TableLogicals = new ExBindingList<TableLogical>();
        TableTexts = new ExBindingList<TableText>();
        TableFonts = new ExBindingList<TableFont>();
        ModeTabsSections = new Dictionary<string, Dictionary<string, string>>();

        ReportVariants = new List<ReportVariant>();
        ReportTypes = new List<ReportType>();

        ReportTypesV = new List<ReportType>();
        ReportTypesP = new List<ReportType>();
        ReportTypesK = new List<ReportType>();

        CustomStations = new ExBindingList<Station>();

        Radenia = new List<Radenie>();

        INISSDir = pathtoiniss;
        DataDir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_DATA)!;
        RawBankDir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_RAWBANK)!;
        GVDDirs = TxtParser.ReadDirList();

        INISSExeFiles = new List<string>();
        var di = new DirectoryInfo(INISSDir);
        var subFiles = di.GetFiles("*.exe");
        foreach (var file in subFiles) INISSExeFiles.Add(file.Name);

        var langs = RawBankParser.ReadFyzBankFile(RawBankDir, out var maxLangs);
        Languages = new ExBindingList<FyzLanguage>(TxtParser.ReadGlobalCategori(DataDir, langs, maxLangs));

        LocalLanguages = new List<FyzLanguage>();
        Sounds = RawBankParser.ReadFyzZvukFile(RawBankDir, FyzLanguage.GetBasicLanguage(Languages)!);
        LogZvukTexts = LogZvukParser.ReadLogZvukUsr(RawBankDir);
        try
        {
            TrainsTypes = new ExBindingList<TrainType>(TxtParser.ReadTrainTypes());
        }
        catch (FileNotFoundException)
        {
        }

        TrainNames = Train.GetTrainNames();
        Stations = Station.GetStations();
        Delays = new ExBindingList<string>(TxtParser.ReadZpozdeni());

        try
        {
            Audios = new ExBindingList<Audio>(TxtParser.ReadAudio());
        }
        catch (FileNotFoundException)
        {
        }
    }
}