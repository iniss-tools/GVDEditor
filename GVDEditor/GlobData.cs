using ExControls;
using GVDEditor.Config;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore;
using ToolsCore.Entities;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor;

/// <summary>
/// Fasada nad tromi objektmi s roznou zivotnostou: <see cref="Session" /> (cely beh programu),
/// <see cref="Workspace" /> (zvolena instalacia INISS) a <see cref="Document" /> (otvoreny grafikon).
/// </summary>
/// <remarks>
/// Vlastnosti fasady len deleguju - existuju, kym sa pristupy postupne nenahradia explicitnymi parametrami.
/// Nove kody maju pracovat priamo s dokumentom alebo instalaciou.
/// </remarks>
internal static class GlobData
{
    private static GrafikonDocument _document = new();

    /// <summary>
    /// Nastavenia programu (konfiguracia a styly), vytvorene pri starte.
    /// </summary>
    public static AppSession<GVDEditorConfig, GVDEditorStyle> Session { get; set; } = null!;

    /// <summary>
    /// Zvolena instalacia INISS.
    /// </summary>
    public static InissWorkspace Workspace { get; private set; } = new();

    /// <summary>
    /// Otvoreny grafikon. Nacitava ho <see cref="Formats.GrafikonRepository.Load" /> do noveho dokumentu, ktory sa
    /// potom otvori cez <see cref="OpenDocument" />.
    /// </summary>
    public static GrafikonDocument Document => _document;

    /// <summary>
    /// Otvori grafikon - vsetky jeho data sa vymenia naraz.
    /// </summary>
    public static void OpenDocument(GrafikonDocument document) => _document = document;

    #region Session

    public static GVDEditorConfig Config { get => Session.Config; set => Session.Config = value; }
    public static Styles<GVDEditorStyle> Styles { get => Session.Styles; set => Session.Styles = value; }
    public static GVDEditorStyle UsingStyle { get => Session.UsingStyle; set => Session.UsingStyle = value; }

    #endregion

    #region Workspace

    public static string INISSDir { get => Workspace.INISSDir; private set => Workspace.INISSDir = value; }
    public static string DataDir { get => Workspace.DataDir; private set => Workspace.DataDir = value; }
    public static string RawBankDir { get => Workspace.RawBankDir; private set => Workspace.RawBankDir = value; }
    public static List<string> INISSExeFiles { get => Workspace.INISSExeFiles; private set => Workspace.INISSExeFiles = value; }
    public static List<DirList> GVDDirs { get => Workspace.GVDDirs; set => Workspace.GVDDirs = value; }
    public static ExBindingList<Audio> Audios { get => Workspace.Audios; internal set => Workspace.Audios = value; }
    public static List<FyzSound> Sounds { get => Workspace.Sounds; private set => Workspace.Sounds = value; }
    public static List<LogZvukText> LogZvukTexts { get => Workspace.LogZvukTexts; private set => Workspace.LogZvukTexts = value; }
    public static ExBindingList<FyzLanguage> Languages { get => Workspace.Languages; internal set => Workspace.Languages = value; }
    public static List<Station> Stations { get => Workspace.Stations; set => Workspace.Stations = value; }
    public static ExBindingList<string> Delays { get => Workspace.Delays; internal set => Workspace.Delays = value; }
    public static ExBindingList<TrainType> TrainsTypes { get => Workspace.TrainsTypes; internal set => Workspace.TrainsTypes = value; }
    public static List<TrainName> TrainNames { get => Workspace.TrainNames; private set => Workspace.TrainNames = value; }

    #endregion

    #region Document

    public static ExBindingList<Train> Trains { get => Document.Trains; set => Document.Trains = value; }
    public static ExBindingList<Track> Tracks { get => Document.Tracks; set => Document.Tracks = value; }
    public static ExBindingList<Platform> Platforms { get => Document.Platforms; set => Document.Platforms = value; }
    public static ExBindingList<Operator> Operators { get => Document.Operators; set => Document.Operators = value; }
    public static ExBindingList<Station> CustomStations { get => Document.CustomStations; set => Document.CustomStations = value; }
    public static ExBindingList<TableTabTab> TabTabs { get => Document.TabTabs; set => Document.TabTabs = value; }
    public static ExBindingList<TableCatalog> TableCatalogs { get => Document.TableCatalogs; set => Document.TableCatalogs = value; }
    public static ExBindingList<TablePhysical> TablePhysicals { get => Document.TablePhysicals; set => Document.TablePhysicals = value; }
    public static ExBindingList<TableLogical> TableLogicals { get => Document.TableLogicals; set => Document.TableLogicals = value; }
    public static ExBindingList<TableText> TableTexts { get => Document.TableTexts; set => Document.TableTexts = value; }
    public static ExBindingList<TableFont> TableFonts { get => Document.TableFonts; set => Document.TableFonts = value; }
    public static string TableFontDir { get => Document.TableFontDir; set => Document.TableFontDir = value; }

    public static Dictionary<string, Dictionary<string, string>> ModeTabsSections
    {
        get => Document.ModeTabsSections;
        set => Document.ModeTabsSections = value;
    }

    public static List<ReportVariant> ReportVariants { get => Document.ReportVariants; set => Document.ReportVariants = value; }
    public static List<ReportType> ReportTypes { get => Document.ReportTypes; set => Document.ReportTypes = value; }
    public static List<ReportType> ReportTypesV => Document.ReportTypesV;
    public static List<ReportType> ReportTypesP => Document.ReportTypesP;
    public static List<ReportType> ReportTypesK => Document.ReportTypesK;
    public static List<FyzLanguage> LocalLanguages { get => Document.LocalLanguages; set => Document.LocalLanguages = value; }
    public static List<Radenie> Radenia { get => Document.Radenia; set => Document.Radenia = value; }

    #endregion

    /// <summary>
    /// Vyprazdni data otvoreneho grafikonu - rovnaky stav ako bez otvoreneho grafikonu po spusteni programu.
    /// Zoznam vlakov ostava ten isty (len sa vyprazdni), aby tabulka vlakov v hlavnom okne ostala naviazana.
    /// </summary>
    public static void ClearGrafikonData()
    {
        var trains = _document.Trains;
        trains.Clear();
        _document = new GrafikonDocument { Trains = trains };
    }

    /// <summary>
    /// Nacita zvolenu instalaciu INISS (zoznam grafikonov, banku, globalne nastavenia) a zavrie otvoreny grafikon.
    /// </summary>
    public static void PrepareGlobalData(string pathtoiniss)
    {
        Trains.FireEventOnSort = true;
        ClearGrafikonData();

        Workspace = new InissWorkspace
        {
            Audios = [],
            TrainsTypes = []
        };

        INISSDir = pathtoiniss;
        DataDir = Utils.CombinePath(pathtoiniss, GvdFileConsts.DIR_DATA)!;
        RawBankDir = Utils.CombinePath(pathtoiniss, GvdFileConsts.DIR_RAWBANK)!;
        GVDDirs = DirListFile.Read(DataDir);

        INISSExeFiles = new DirectoryInfo(INISSDir).GetFiles("*.exe").Select(file => file.Name).ToList();

        var langs = RawBankParser.ReadFyzBankFile(RawBankDir, out var maxLangs);
        Languages = new ExBindingList<FyzLanguage>(CategoriFile.ReadGlobal(DataDir, langs, maxLangs));

        Sounds = RawBankParser.ReadFyzZvukFile(RawBankDir, FyzLanguage.GetBasicLanguage(Languages)!);
        LogZvukTexts = LogZvukParser.ReadLogZvukUsr(RawBankDir);
        try
        {
            TrainsTypes = new ExBindingList<TrainType>(TrTypesFile.Read(DataDir));
        }
        catch (FileNotFoundException)
        {
        }

        TrainNames = Train.GetTrainNames();
        Stations = Station.GetStations();
        Delays = new ExBindingList<string>(ZpozdeniFile.Read(DataDir));

        try
        {
            Audios = new ExBindingList<Audio>(AudioFile.Read(DataDir));
        }
        catch (FileNotFoundException)
        {
        }
    }
}
