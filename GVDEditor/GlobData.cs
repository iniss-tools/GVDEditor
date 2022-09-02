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
    public static string INISSDir { get; private set; }
    public static string DataDir { get; private set; }
    public static string RawBankDir { get; private set; }

    public static List<string> INISSExeFiles { get; private set; }

    public static List<DirList> GVDDirs { get; set; }
    public static ExBindingList<Audio> Audios { get; private set; }

    public static List<FyzSound> Sounds { get; private set; }
    public static ExBindingList<FyzLanguage> Languages { get; private set; }
    public static List<FyzLanguage> LocalLanguages { get; set; }
    public static List<Station> Stations { get; set; }
    public static ExBindingList<Station> CustomStations { get; set; }

    public static ExBindingList<Operator> Operators { get; set; }

    public static ExBindingList<Train> Trains { get; set; } = new();
    public static ExBindingList<Track> Tracks { get; set; }
    public static ExBindingList<Platform> Platforms { get; set; }

    public static ExBindingList<int> Delays { get; private set; }

    public static ExBindingList<TrainType> TrainsTypes { get; private set; }

    public static List<string> TrainNames { get; private set; }

    public static ExBindingList<TableTabTab> TabTabs { get; set; }
    public static ExBindingList<TableCatalog> TableCatalogs { get; set; }
    public static ExBindingList<TablePhysical> TablePhysicals { get; set; }
    public static ExBindingList<TableLogical> TableLogicals { get; set; }
    public static ExBindingList<TableText> TableTexts { get; set; }
    public static ExBindingList<TableFont> TableFonts { get; set; }
    public static string TableFontDir { get; set; }

    public static List<ReportVariant> ReportVariants { get; set; }
    public static List<ReportType> ReportTypes { get; set; }

    public static List<ReportType> ReportTypesV { get; private set; }
    public static List<ReportType> ReportTypesP { get; private set; }
    public static List<ReportType> ReportTypesK { get; private set; }

    public static List<Radenie> Radenia { get; set; }

    public static GVDEditorConfig Config;
    public static Styles<GVDEditorStyle> Styles;
    public static GVDEditorStyle UsingStyle;

    public static bool PrivateFeatures { get; set; }

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

        ReportVariants = new List<ReportVariant>();
        ReportTypes = new List<ReportType>();

        ReportTypesV = new List<ReportType>();
        ReportTypesP = new List<ReportType>();
        ReportTypesK = new List<ReportType>();

        CustomStations = new ExBindingList<Station>();

        Radenia = new List<Radenie>();

        INISSDir = pathtoiniss;
        DataDir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_DATA);
        RawBankDir = Utils.CombinePath(pathtoiniss, FileConsts.DIR_RAWBANK);
        GVDDirs = TxtParser.ReadDirList();

        INISSExeFiles = new List<string>();
        var di = new DirectoryInfo(INISSDir);
        var subFiles = di.GetFiles("*.exe");
        foreach (var file in subFiles) INISSExeFiles.Add(file.Name);

        var langs = RawBankParser.ReadFyzBankFile(RawBankDir, out var maxLangs);
        Languages = new ExBindingList<FyzLanguage>(TxtParser.ReadGlobalCategori(DataDir, langs, maxLangs));

        LocalLanguages = new List<FyzLanguage>();
        Sounds = RawBankParser.ReadFyzZvukFile(RawBankDir, FyzLanguage.GetBasicLanguage(Languages));
        try
        {
            TrainsTypes = new ExBindingList<TrainType>(TxtParser.ReadTrainTypes());
        }
        catch (FileNotFoundException)
        {
        }

        TrainNames = Train.GetTrainNames();
        Stations = Station.GetStations();
        Delays = new ExBindingList<int>(TxtParser.ReadZpozdeni());

        try
        {
            Audios = new ExBindingList<Audio>(TxtParser.ReadAudio());
        }
        catch (FileNotFoundException)
        {
        }
    }
}