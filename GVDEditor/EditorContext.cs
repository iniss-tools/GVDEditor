using GVDEditor.Config;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Integration;
using ToolsCore;

namespace GVDEditor;

/// <summary>
/// Stav editora s tromi zivotnostami: <see cref="Session" /> (cely beh programu), <see cref="Workspace" />
/// (zvolena instalacia INISS) a <see cref="Document" /> (otvoreny grafikon). Vytvara ho <see cref="Program" />
/// a okna ho dostavaju explicitne - v kode nie je ziadny staticky pristup k datam.
/// </summary>
internal sealed class EditorContext
{
    /// <summary>
    /// Vytvori kontext bez zvolenej instalacie a bez otvoreneho grafikonu.
    /// </summary>
    public EditorContext(AppSession<GVDEditorConfig, GVDEditorStyle> session)
    {
        Session = session;
        Document.Trains.FireEventOnSort = true;
    }

    /// <summary>
    /// Nastavenia programu (konfiguracia a styly).
    /// </summary>
    public AppSession<GVDEditorConfig, GVDEditorStyle> Session { get; }

    /// <summary>
    /// Konfiguracia programu.
    /// </summary>
    public GVDEditorConfig Config => Session.Config;

    /// <summary>
    /// Pouzivany styl.
    /// </summary>
    public GVDEditorStyle UsingStyle => Session.UsingStyle;

    /// <summary>
    /// Zvolena instalacia INISS.
    /// </summary>
    public InissWorkspace Workspace { get; private set; } = new();

    /// <summary>
    /// Konfiguracie spustania INISSu zvolenej instalacie (nacita ich hlavne okno po otvoreni instalacie).
    /// </summary>
    public RunConfigurationSet RunConfigurations { get; set; } = new("", [], null);

    /// <summary>
    /// Otvoreny grafikon (bez otvoreneho grafikonu prazdny dokument).
    /// </summary>
    public GrafikonDocument Document { get; private set; } = new();

    /// <summary>
    /// Kontext pre citanie a zapis suborov otvoreneho grafikonu.
    /// </summary>
    public GrafikonContext Grafikon => new(Workspace, Document, Config.Language);

    /// <summary>
    /// Zvoli instalaciu INISS a zavrie otvoreny grafikon.
    /// </summary>
    public void OpenWorkspace(InissWorkspace workspace)
    {
        CloseDocument();
        Workspace = workspace;
        if (Document.Trains is TrainBindingList trains)
            trains.TrainNames = workspace.TrainNames;
    }

    /// <summary>
    /// Otvori grafikon - vsetky jeho data sa vymenia naraz.
    /// </summary>
    public void OpenDocument(GrafikonDocument document)
    {
        if (document.Trains is TrainBindingList { TrainNames: null } trains)
            trains.TrainNames = Workspace.TrainNames;
        Document = document;
    }

    /// <summary>
    /// Zavrie grafikon - rovnaky stav ako bez otvoreneho grafikonu po spusteni programu. Zoznam vlakov ostava
    /// ten isty (len sa vyprazdni), aby tabulka vlakov v hlavnom okne ostala naviazana.
    /// </summary>
    public void CloseDocument()
    {
        var trains = Document.Trains;
        trains.Clear();
        Document = new GrafikonDocument { Trains = trains };
    }

    /// <summary>
    /// Stanice zvukovej banky a otvoreneho grafikonu.
    /// </summary>
    public StationDirectory Stations => new(Workspace.Stations ?? [], Document.CustomStations);
}
