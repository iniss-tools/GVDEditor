using System.Reflection;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML;

/// <summary>
///     Obsahuje zoznam všetkých možných klávesových skratiek pre program.
/// </summary>
public record AppShortcuts()
{
    private static readonly Type classType = typeof(AppShortcuts);

    [XmlIgnore] private static readonly Dictionary<string, (Shortcut shortcut, string name)> props = new()
    {
        [nameof(New)] = (Shortcut.None, "Nový"),
        [nameof(Open)] = (Shortcut.CtrlO, "Otvoriť"),
        [nameof(ImportGvd)] = (Shortcut.CtrlI, "Importovať grafikon"),
        [nameof(ImportData)] = (Shortcut.None, "Importovať dáta"),
        [nameof(Save)] = (Shortcut.CtrlS, "Uložiť"),
        [nameof(Analyze)] = (Shortcut.CtrlShiftA, "Analyzovať"),

        [nameof(AddTrain)] = (Shortcut.Ins, "Pridať vlak"),
        [nameof(EditTrain)] = (Shortcut.ShiftIns, "Upraviť vlak"),
        [nameof(DeleteTrains)] = (Shortcut.Del, "Odstrániť vlaky"),
        [nameof(DuplicateTrain)] = (Shortcut.CtrlD, "Duplikovať vlak"),

        [nameof(LocalSettings)] = (Shortcut.CtrlL, "Lokálne nastavenia"),
        [nameof(GlobalSettings)] = (Shortcut.CtrlG, "Globálne nastavenia"),
        [nameof(AppSettings)] = (Shortcut.CtrlP, "Nastavenia programu"),

        [nameof(GSGvds)] = (Shortcut.Ctrl0, "Globálne nastavenia - Grafikony"),
        [nameof(GSLanguages)] = (Shortcut.Ctrl1, "Globálne nastavenia - Jazyky"),
        [nameof(GSDelays)] = (Shortcut.Ctrl2, "Globálne nastavenia - Meškania"),
        [nameof(GSTrainTypes)] = (Shortcut.Ctrl3, "Globálne nastavenia - Typy vlakov"),
        [nameof(GSAudio)] = (Shortcut.Ctrl4, "Globálne nastavenia - Audio"),

        [nameof(LSGvd)] = (Shortcut.CtrlShiftG, "Lokálne nastavenia - Grafikon"),
        [nameof(LSStations)] = (Shortcut.CtrlShiftS, "Lokálne nastavenia - Jazyky"),
        [nameof(LSOperators)] = (Shortcut.CtrlShiftO, "Lokálne nastavenia - Dopravcovia"),
        [nameof(LSPlatforms)] = (Shortcut.CtrlShiftN, "Lokálne nastavenia - Nástupištia"),
        [nameof(LSTracks)] = (Shortcut.CtrlShiftK, "Lokálne nastavenia - Koľaje"),
        [nameof(LSPhysicalTables)] = (Shortcut.CtrlShiftF, "Lokálne nastavenia - Fyzické tabule"),
        [nameof(LSLogicalsTables)] = (Shortcut.CtrlShiftL, "Lokálne nastavenia - Logické tabule"),
        [nameof(LSCatalogTables)] = (Shortcut.CtrlShiftC, "Lokálne nastavenia - Katalógové tabule"),
        [nameof(LSTabTab)] = (Shortcut.CtrlShiftT, "Lokálne nastavenia - TabTab"),
        [nameof(LSTTexts)] = (Shortcut.CtrlShiftE, "Lokálne nastavenia - Texty do tabúľ"),
        [nameof(LSTFonts)] = (Shortcut.CtrlShiftP, "Lokálne nastavenia - Písma"),
        [nameof(LSTabTabEditor)] = (Shortcut.CtrlT, "Lokálne nastavenia - TabTab editor"),

        [nameof(RunINISS)] = (Shortcut.F5, "Spustiť INISS"),
        [nameof(ShutdownINISS)] = (Shortcut.ShiftF5, "Ukončiť INISS"),
        [nameof(KillINISS)] = (Shortcut.F10, "Nútene vypnúť INISS"),
        [nameof(RestartINISS)] = (Shortcut.CtrlShiftF5, "Reštartovať INISS"),

        [nameof(InfoApp)] = (Shortcut.F6, "Informácie o programe"),
        [nameof(UpdateNotes)] = (Shortcut.None, "Poznámky k aktualizácií"),
        [nameof(DateLimit)] = (Shortcut.F7, "Dátumové obmedzenie")
    };

    #region Fields

    private CmdShortcut _new = InitShortcut(nameof(New));
    private CmdShortcut _open = InitShortcut(nameof(Open));
    private CmdShortcut _importGvd = InitShortcut(nameof(ImportGvd));
    private CmdShortcut _importData = InitShortcut(nameof(ImportData));
    private CmdShortcut _save = InitShortcut(nameof(Save));
    private CmdShortcut _analyze = InitShortcut(nameof(Analyze));

    private CmdShortcut _addTrain = InitShortcut(nameof(AddTrain));
    private CmdShortcut _editTrain = InitShortcut(nameof(EditTrain));
    private CmdShortcut _deleteTrains = InitShortcut(nameof(DeleteTrains));
    private CmdShortcut _duplicateTrain = InitShortcut(nameof(DuplicateTrain));

    private CmdShortcut _localSettings = InitShortcut(nameof(LocalSettings));
    private CmdShortcut _globalSettings = InitShortcut(nameof(GlobalSettings));
    private CmdShortcut _appSettings = InitShortcut(nameof(AppSettings));

    private CmdShortcut _gsGvds = InitShortcut(nameof(GSGvds));
    private CmdShortcut _gsLanguages = InitShortcut(nameof(GSLanguages));
    private CmdShortcut _gsDelays = InitShortcut(nameof(GSDelays));
    private CmdShortcut _gsTrainTypes = InitShortcut(nameof(GSTrainTypes));
    private CmdShortcut _gsAudio = InitShortcut(nameof(GSAudio));

    private CmdShortcut _lsGvd = InitShortcut(nameof(LSGvd));
    private CmdShortcut _lsStations = InitShortcut(nameof(LSStations));
    private CmdShortcut _lsOperators = InitShortcut(nameof(LSOperators));
    private CmdShortcut _lsPlatforms = InitShortcut(nameof(LSPlatforms));
    private CmdShortcut _lsTracks = InitShortcut(nameof(LSTracks));
    private CmdShortcut _lsPhysicals = InitShortcut(nameof(LSPhysicalTables));
    private CmdShortcut _lsLogicals = InitShortcut(nameof(LSLogicalsTables));
    private CmdShortcut _lsCatalogs = InitShortcut(nameof(LSCatalogTables));
    private CmdShortcut _lsTabTab = InitShortcut(nameof(LSTabTab));
    private CmdShortcut _lsTexts = InitShortcut(nameof(LSTTexts));
    private CmdShortcut _lsFonts = InitShortcut(nameof(LSTFonts));
    private CmdShortcut _lsTabTabEditor = InitShortcut(nameof(LSTabTabEditor));

    private CmdShortcut _runINISS = InitShortcut(nameof(RunINISS));
    private CmdShortcut _shutdownINISS = InitShortcut(nameof(ShutdownINISS));
    private CmdShortcut _killINISS = InitShortcut(nameof(KillINISS));
    private CmdShortcut _restartINISS = InitShortcut(nameof(RestartINISS));

    private CmdShortcut _infoApp = InitShortcut(nameof(InfoApp));
    private CmdShortcut _updateNotes = InitShortcut(nameof(UpdateNotes));
    private CmdShortcut _dateLimit = InitShortcut(nameof(DateLimit));

    #endregion

    #region Properties

    /// <summary>
    ///     Skratka pre otvorenie dialógu Nový grafikon.
    /// </summary>
    [XmlElement("NewGVD")]
    public CmdShortcut New
    {
        get => _new ??= InitShortcut(nameof(New));
        set
        {
            _new = value;
            AssignShortcutProps(ref _new, nameof(New));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie grafikonu.
    /// </summary>
    [XmlElement("OpenGVD")]
    public CmdShortcut Open
    {
        get => _open ??= InitShortcut(nameof(Open));
        set
        {
            _open = value;
            AssignShortcutProps(ref _open, nameof(Open));
        }
    }

    /// <summary>
    ///     Skratka pre import grafikonu.
    /// </summary>
    [XmlElement("ImportGVD")]
    public CmdShortcut ImportGvd
    {
        get => _importGvd ??= InitShortcut(nameof(ImportGvd));
        set
        {
            _importGvd = value;
            AssignShortcutProps(ref _importGvd, nameof(ImportGvd));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie dialógu Importovanie dát.
    /// </summary>
    [XmlElement("ImportData")]
    public CmdShortcut ImportData
    {
        get => _importData ??= InitShortcut(nameof(ImportData));
        set
        {
            _importData = value;
            AssignShortcutProps(ref _importData, nameof(ImportData));
        }
    }

    /// <summary>
    ///     Skratka pre uloženie grafikonu.
    /// </summary>
    [XmlElement("Save")]
    public CmdShortcut Save
    {
        get => _save ??= InitShortcut(nameof(Save));
        set
        {
            _save = value;
            AssignShortcutProps(ref _save, nameof(Save));
        }
    }

    /// <summary>
    ///     Skratka pre analyzu grafikonu.
    /// </summary>
    [XmlElement("Analyze")]
    public CmdShortcut Analyze
    {
        get => _analyze ??= InitShortcut(nameof(Analyze));
        set
        {
            _analyze = value;
            AssignShortcutProps(ref _analyze, nameof(Analyze));
        }
    }

    /// <summary>
    ///     Skratka pre import grafikonu.
    /// </summary>
    [XmlElement("AddTrain")]
    public CmdShortcut AddTrain
    {
        get => _addTrain ??= InitShortcut(nameof(AddTrain));
        set
        {
            _addTrain = value;
            AssignShortcutProps(ref _addTrain, nameof(AddTrain));
        }
    }

    /// <summary>
    ///     Skratka pre úpravu vlaku.
    /// </summary>
    [XmlElement("EditTrain")]
    public CmdShortcut EditTrain
    {
        get => _editTrain ??= InitShortcut(nameof(EditTrain));
        set
        {
            _editTrain = value;
            AssignShortcutProps(ref _editTrain, nameof(EditTrain));
        }
    }

    /// <summary>
    ///     Skratka pre vymazanie vlaku.
    /// </summary>
    [XmlElement("DeleteTrains")]
    public CmdShortcut DeleteTrains
    {
        get => _deleteTrains ??= InitShortcut(nameof(DeleteTrains));
        set
        {
            _deleteTrains = value;
            AssignShortcutProps(ref _deleteTrains, nameof(DeleteTrains));
        }
    }

    /// <summary>
    ///     Skratka pre duplikovanie vlaku.
    /// </summary>
    [XmlElement("DuplicateTrain")]
    public CmdShortcut DuplicateTrain
    {
        get => _duplicateTrain ??= InitShortcut(nameof(DuplicateTrain));
        set
        {
            _duplicateTrain = value;
            AssignShortcutProps(ref _duplicateTrain, nameof(DuplicateTrain));
        }
    }

    /// <summary>
    ///     Skratka pre uloženie grafikonu.
    /// </summary>
    [XmlElement("LSettings")]
    public CmdShortcut LocalSettings
    {
        get => _localSettings ??= InitShortcut(nameof(LocalSettings));
        set
        {
            _localSettings = value;
            AssignShortcutProps(ref _localSettings, nameof(LocalSettings));
        }
    }

    /// <summary>
    ///     Skratka pre pridanie vlaku.
    /// </summary>
    [XmlElement("GSettings")]
    public CmdShortcut GlobalSettings
    {
        get => _globalSettings ??= InitShortcut(nameof(GlobalSettings));
        set
        {
            _globalSettings = value;
            AssignShortcutProps(ref _globalSettings, nameof(GlobalSettings));
        }
    }

    /// <summary>
    ///     Skratka pre úpravu vlaku.
    /// </summary>
    [XmlElement("AppSettings")]
    public CmdShortcut AppSettings
    {
        get => _appSettings ??= InitShortcut(nameof(AppSettings));
        set
        {
            _appSettings = value;
            AssignShortcutProps(ref _appSettings, nameof(AppSettings));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
    /// </summary>
    [XmlElement("GSGrafikony")]
    public CmdShortcut GSGvds
    {
        get => _gsGvds ??= InitShortcut(nameof(GSGvds));
        set
        {
            _gsGvds = value;
            AssignShortcutProps(ref _gsGvds, nameof(GSGvds));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
    /// </summary>
    [XmlElement("GSLangs")]
    public CmdShortcut GSLanguages
    {
        get => _gsLanguages ??= InitShortcut(nameof(GSLanguages));
        set
        {
            _gsLanguages = value;
            AssignShortcutProps(ref _gsLanguages, nameof(GSLanguages));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
    /// </summary>
    [XmlElement("GSMeskania")]
    public CmdShortcut GSDelays
    {
        get => _gsDelays ??= InitShortcut(nameof(GSDelays));
        set
        {
            _gsDelays = value;
            AssignShortcutProps(ref _gsDelays, nameof(GSDelays));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
    /// </summary>
    [XmlElement("GSTrainTypes")]
    public CmdShortcut GSTrainTypes
    {
        get => _gsTrainTypes ??= InitShortcut(nameof(GSTrainTypes));
        set
        {
            _gsTrainTypes = value;
            AssignShortcutProps(ref _gsTrainTypes, nameof(GSTrainTypes));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
    /// </summary>
    [XmlElement("GSAudio")]
    public CmdShortcut GSAudio
    {
        get => _gsAudio ??= InitShortcut(nameof(GSAudio));
        set
        {
            _gsAudio = value;
            AssignShortcutProps(ref _gsAudio, nameof(GSAudio));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSGrafikon")] 
    public CmdShortcut LSGvd
    {
        get => _lsGvd ??= InitShortcut(nameof(LSGvd));
        set
        {
            _lsGvd = value;
            AssignShortcutProps(ref _lsGvd, nameof(LSGvd));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSStanice")] 
    public CmdShortcut LSStations
    {
        get => _lsStations ??= InitShortcut(nameof(LSStations));
        set
        {
            _lsStations = value;
            AssignShortcutProps(ref _lsStations, nameof(LSStations));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSDopravcovia")] 
    public CmdShortcut LSOperators
    {
        get => _lsOperators ??= InitShortcut(nameof(LSOperators));
        set
        {
            _lsOperators = value;
            AssignShortcutProps(ref _lsOperators, nameof(LSOperators));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSPlatforms")] 
    public CmdShortcut LSPlatforms
    {
        get => _lsPlatforms ??= InitShortcut(nameof(LSPlatforms));
        set
        {
            _lsPlatforms = value;
            AssignShortcutProps(ref _lsPlatforms, nameof(LSPlatforms));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSKolaje")] 
    public CmdShortcut LSTracks
    {
        get => _lsTracks ??= InitShortcut(nameof(LSTracks));
        set
        {
            _lsTracks = value;
            AssignShortcutProps(ref _lsTracks, nameof(LSTracks));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTPhysicals")] 
    public CmdShortcut LSPhysicalTables
    {
        get => _lsPhysicals ??= InitShortcut(nameof(LSPhysicalTables));
        set
        {
            _lsPhysicals = value;
            AssignShortcutProps(ref _lsPhysicals, nameof(LSPhysicalTables));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTLogicals")] 
    public CmdShortcut LSLogicalsTables
    {
        get => _lsLogicals ??= InitShortcut(nameof(LSLogicalsTables));
        set
        {
            _lsLogicals = value;
            AssignShortcutProps(ref _lsLogicals, nameof(LSLogicalsTables));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTCatalogs")]
    public CmdShortcut LSCatalogTables
    {
        get => _lsCatalogs ??= InitShortcut(nameof(LSCatalogTables));
        set
        {
            _lsCatalogs = value;
            AssignShortcutProps(ref _lsCatalogs, nameof(LSCatalogTables));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTabTab")] 
    public CmdShortcut LSTabTab
    {
        get => _lsTabTab ??= InitShortcut(nameof(LSTabTab));
        set
        {
            _lsTabTab = value;
            AssignShortcutProps(ref _lsTabTab, nameof(LSTabTab));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTTexts")]
    public CmdShortcut LSTTexts
    {
        get => _lsTexts ??= InitShortcut(nameof(LSTTexts));
        set
        {
            _lsTexts = value;
            AssignShortcutProps(ref _lsTexts, nameof(LSTTexts));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
    /// </summary>
    [XmlElement("LSTFonts")] 
    public CmdShortcut LSTFonts
    {
        get => _lsFonts ??= InitShortcut(nameof(LSTFonts));
        set
        {
            _lsFonts = value;
            AssignShortcutProps(ref _lsFonts, nameof(LSTFonts));
        }
    }

    /// <summary>
    ///     Skratka pre otvorenie editoru TabTab.
    /// </summary>
    [XmlElement("LSTabTabEditor")] 
    public CmdShortcut LSTabTabEditor
    {
        get => _lsTabTabEditor ??= InitShortcut(nameof(LSTabTabEditor));
        set
        {
            _lsTabTabEditor = value;
            AssignShortcutProps(ref _lsTabTabEditor, nameof(LSTabTabEditor));
        }
    }

    /// <summary>
    ///     Skratka pre spustenie programu INISS.
    /// </summary>
    [XmlElement("RunINISS")] 
    public CmdShortcut RunINISS
    {
        get => _runINISS ??= InitShortcut(nameof(RunINISS));
        set
        {
            _runINISS = value;
            AssignShortcutProps(ref _runINISS, nameof(RunINISS));
        }
    }

    /// <summary>
    ///     Skratka pre ukoncenie programu INISS.
    /// </summary>
    [XmlElement("ShutdownINISS")]
    public CmdShortcut ShutdownINISS
    {
        get => _shutdownINISS ??= InitShortcut(nameof(ShutdownINISS));
        set
        {
            _shutdownINISS = value;
            AssignShortcutProps(ref _shutdownINISS, nameof(ShutdownINISS));
        }
    }

    /// <summary>
    ///     Skratka pre nutene ukoncenie programu INISS.
    /// </summary>
    [XmlElement("KillINISS")] 
    public CmdShortcut KillINISS
    {
        get => _killINISS ??= InitShortcut(nameof(KillINISS));
        set
        {
            _killINISS = value;
            AssignShortcutProps(ref _killINISS, nameof(KillINISS));
        }
    }

    /// <summary>
    ///     Skratka pre restartovanie programu INISS.
    /// </summary>
    [XmlElement("RestartINISS")] 
    public CmdShortcut RestartINISS
    {
        get => _restartINISS ??= InitShortcut(nameof(RestartINISS));
        set
        {
            _restartINISS = value;
            AssignShortcutProps(ref _restartINISS, nameof(RestartINISS));
        }
    }

    /// <summary>
    ///     Skratka pre vymazanie vlaku.
    /// </summary>
    [XmlElement("InfoApp")] 
    public CmdShortcut InfoApp
    {
        get => _infoApp ??= InitShortcut(nameof(InfoApp));
        set
        {
            _infoApp = value;
            AssignShortcutProps(ref _infoApp, nameof(InfoApp));
        }
    }

    /// <summary>
    ///     Skratka pre duplikovanie vlaku.
    /// </summary>
    [XmlElement("UpdateNotes")] 
    public CmdShortcut UpdateNotes
    {
        get => _updateNotes ??= InitShortcut(nameof(UpdateNotes));
        set
        {
            _updateNotes = value;
            AssignShortcutProps(ref _updateNotes, nameof(UpdateNotes));
        }
    }

    /// <summary>
    ///     Skratka pre okno s generovaniim datumoveho obmedzenia.
    /// </summary>
    [XmlElement("DatObm")] 
    public CmdShortcut DateLimit
    {
        get => _dateLimit ??= InitShortcut(nameof(DateLimit));
        set
        {
            _dateLimit = value;
            AssignShortcutProps(ref _dateLimit, nameof(DateLimit));
        }
    }

    #endregion

    /// <summary>
    ///     Vráti zoznam všetkých možných klávesových skratiek pre program.
    /// </summary>
    /// <returns></returns>
    public IList<CmdShortcut> GetValues()
    {
        var properties = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        return properties.Select(prop => prop.GetValue(this) as CmdShortcut).ToList();
    }

    public void SetValues(IEnumerable<CmdShortcut> shortcuts)
    {
        foreach (var shortcut in shortcuts) 
            classType.GetProperty(shortcut.PropertyName)?.SetValue(this, shortcut);
    }

    private static CmdShortcut InitShortcut(string propname) => new(props[propname].shortcut, props[propname].name, propname);

    private static void AssignShortcutProps(ref CmdShortcut obj, string propname)
    {
        if (obj is null)
        {
            InitShortcut(propname);
        }
        else
        {
            obj.Name = props[propname].name;
            obj.PropertyName = propname;
        }
    }

    protected AppShortcuts(AppShortcuts original)
    {
        New = original.New with { };
        Open = original.Open with { };
        ImportGvd = original.ImportGvd with { };
        ImportData = original.ImportData with { };
        Save = original.Save with { };
        Analyze = original.Analyze with { };

        AddTrain = original.AddTrain with { };
        EditTrain = original.EditTrain with { };
        DeleteTrains = original.DeleteTrains with { };
        DuplicateTrain = original.DuplicateTrain with { };

        LocalSettings = original.LocalSettings with { };
        GlobalSettings = original.GlobalSettings with { };
        AppSettings = original.AppSettings with { };

        GSGvds = original.GSGvds with { };
        GSLanguages = original.GSLanguages with { };
        GSDelays = original.GSDelays with { };
        GSTrainTypes = original.GSTrainTypes with { };
        GSAudio = original.GSAudio with { };

        LSGvd = original.LSGvd with { };
        LSStations = original.LSStations with { };
        LSOperators = original.LSOperators with { };
        LSPlatforms = original.LSPlatforms with { };
        LSTracks = original.LSTracks with { };
        LSPhysicalTables = original.LSPhysicalTables with { };
        LSLogicalsTables = original.LSLogicalsTables with { };
        LSCatalogTables = original.LSCatalogTables with { };
        LSTabTab = original.LSTabTab with { };
        LSTTexts = original.LSTTexts with { };
        LSTFonts = original.LSTFonts with { };
        LSTabTabEditor = original.LSTabTabEditor with { };

        RunINISS = original.RunINISS with { };
        ShutdownINISS = original.ShutdownINISS with { };
        KillINISS = original.KillINISS with { };
        RestartINISS = original.RestartINISS with { };

        InfoApp = original.InfoApp with { };
        UpdateNotes = original.UpdateNotes with { };
        DateLimit = original.DateLimit with { };
    }
    
}