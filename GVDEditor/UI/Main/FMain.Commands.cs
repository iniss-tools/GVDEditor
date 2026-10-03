using GVDEditor.UI.Settings;
using ToolsCore.Commands;
using ToolsCore.Tools;

namespace GVDEditor.UI.Main;

internal partial class FMain
{
    // instalacia INISS je otvorena (zoznam grafikonov sa nacital)
    private bool _installationOpen;

    private bool HasInstallation => _installationOpen;

    private bool HasGrafikons => _installationOpen && _gvdDirs.Count > 0;

    // otvoreny grafikon sa nacital (alebo je novy) - pri chybe nacitania sa v globalnych nastaveniach berie z disku
    private bool HasGrafikon => _installationOpen && _grafikonLoaded;

    /// <summary>
    /// Vytvori prikazy hlavneho okna a naviaze ich na tlacidla panela nastrojov aj polozky ponuky.
    /// </summary>
    private void CreateCommands()
    {
        void Add(CommandInfo info, Action execute, Func<bool>? canExecute, params ToolStripItem[] items) =>
            _commands.Add(info, execute, canExecute).Bind(items);

        Func<bool> installation = () => HasInstallation;
        Func<bool> grafikons = () => HasGrafikons;
        Func<bool> grafikon = () => HasGrafikon;

        Add(GvdCommands.New, ShowNewGVD, installation, tsmiNew, tsbAddGVD);
        Add(GvdCommands.Open, ShowOpenDir, null, tsmiOpen, tsbOpen);
        Add(GvdCommands.ImportGvd, ShowImportGVD, installation, tsmiImportGVD, tsmimImportGVD);
        Add(GvdCommands.ImportData, ShowImportData, grafikon, tsmiImportData, tsmimImportData);
        Add(GvdCommands.ImportElis, ShowImportElis, grafikon, tsmiImportELIS, tsmimImportELIS);
        Add(GvdCommands.Save, () => DoSave(), grafikon, tsmiSave, tsbSave);
        Add(GvdCommands.Analyze, ShowAnalyzeGVD, grafikon, tsmiAnalyze, tsbAnalyze);

        Add(GvdCommands.AddTrain, () => ShowEditTrain(null, _ctx.Document.Trains.Count), grafikon, tsmimAddTrain, tsbAddTrain);
        Add(GvdCommands.EditTrain, EditSelectedTrain, grafikon, tsmimEditTrain, tsbEditTrain);
        Add(GvdCommands.DeleteTrains, DoDeleteTrains, grafikon, tsmiDeleteTrain, tsbDeleteTrain);
        Add(GvdCommands.DuplicateTrain, DuplicateSelectedTrain, grafikon, tsmiDuplikovat, tsbCopyTrain);

        Add(GvdCommands.LocalSettings, () => ShowLocalSettings(), grafikon, tsmiVlastnostiStanice, tsbStanica);
        Add(GvdCommands.GlobalSettings, () => ShowGlobalSettings(), grafikons, tsmiGlobalSettings, tsbGlobalSettings);
        Add(GvdCommands.AppSettings, () => ShowAppSettings(), null, tsmiAppSettings, tsbAppSettings);

        Add(GvdCommands.GsGvds, () => ShowGlobalSettings(GlobalSettingsPage.Grafikony), grafikons, tsmiGrafikony);
        Add(GvdCommands.GsLanguages, () => ShowGlobalSettings(GlobalSettingsPage.Jazyky), grafikons, tsmiLanguages);
        Add(GvdCommands.GsDelays, () => ShowGlobalSettings(GlobalSettingsPage.Meskania), grafikons, tsmiMeskania);
        Add(GvdCommands.GsTrainTypes, () => ShowGlobalSettings(GlobalSettingsPage.TypyVlakov), grafikons, tsmiTypyVlakov);
        Add(GvdCommands.GsAudio, () => ShowGlobalSettings(GlobalSettingsPage.Audio), grafikons, tsmiAudio);

        Add(GvdCommands.LsGvd, () => ShowLocalSettings(LocalSettingsPage.Grafikon), grafikon, tsmiGrafikon);
        Add(GvdCommands.LsLanguages, () => ShowLocalSettings(LocalSettingsPage.JazykyHlaseni), grafikon, tsmiJazykyHlaseni);
        Add(GvdCommands.LsStations, () => ShowLocalSettings(LocalSettingsPage.VlastneStanice), grafikon, tsmiStanice);
        Add(GvdCommands.LsOperators, () => ShowLocalSettings(LocalSettingsPage.Dopravcovia), grafikon, tsmiDopravcovia);
        Add(GvdCommands.LsPlatforms, () => ShowLocalSettings(LocalSettingsPage.Nastupistia), grafikon, tsmiPlatforms);
        Add(GvdCommands.LsTracks, () => ShowLocalSettings(LocalSettingsPage.Kolaje), grafikon, tsmiKolaje);
        Add(GvdCommands.LsPhysicalTables, () => ShowLocalSettings(LocalSettingsPage.FyzickeTabule), grafikon, tsmiTPhysical);
        Add(GvdCommands.LsLogicalTables, () => ShowLocalSettings(LocalSettingsPage.LogickeTabule), grafikon, tsmiTLogical);
        Add(GvdCommands.LsCatalogTables, () => ShowLocalSettings(LocalSettingsPage.KatalogoveTabule), grafikon, tsmiTCatalog);
        Add(GvdCommands.LsTabTab, () => ShowLocalSettings(LocalSettingsPage.TabTab), grafikon, tsmiTabTab);
        Add(GvdCommands.LsTableTexts, () => ShowLocalSettings(LocalSettingsPage.Texty), grafikon, tsmiTTexts);
        Add(GvdCommands.LsTableFonts, () => ShowLocalSettings(LocalSettingsPage.Pisma), grafikon, tsmiTFonts);
        Add(GvdCommands.LsTabTabEditor, () => ShowLocalSettings(LocalSettingsPage.TabTab, LocalSettingsAction.OpenTabTabEditor), grafikon,
            tsmiTabTabEditor);
        Add(GvdCommands.StateDgm, ShowStateDgm, grafikon, tsmiStateDgm);

        // spustenie rozbali ponuku programov pod prvkom, cez ktory bolo vyvolane (skratka - pod viditelnou ponukou)
        _commands.Add(GvdCommands.RunIniss,
                source => StartINISS(null, source == tssbStartINISS || !mainMenu.Visible ? tssbStartINISS : tsmiRun),
                () => HasInstallation && !_iniss.IsRestarting)
            .Bind(tsmimStartINISS, tssbStartINISS);
        Add(GvdCommands.ShutdownIniss, _iniss.ShutDown, () => _iniss.IsRunning, tsmimShutdownINISS, tsbShutdownINISS);
        Add(GvdCommands.KillIniss, AskKillINISS, () => _iniss.IsRunning, tsmimKillINISS, tsbKillINISS);
        // cakanie na ukoncenie pri restarte trva az 30 s - dalsi klik by spustil druhy restart
        Add(GvdCommands.RestartIniss, RestartINISS, () => _iniss.IsRunning && !_iniss.IsRestarting && _iniss.LastStartPath != null,
            tsmimRestartINISS, tsbRestartINISS);
        Add(GvdCommands.InissStartupSettings, () => ShowAppSettings("pStartupIniss"), null, tsmimStartupSettings, tsmiStartupSettings);
        Add(GvdCommands.InissSettings, ShowInissSettings, installation, tsmimInissSettings);

        Add(GvdCommands.InfoApp, ShowInfoApp, null, tsmiInformation, tsbInformation);
        Add(GvdCommands.UpdateNotes, () => Utils.OpenShell(GvdLinkConsts.LinkNews), null, tsmiChangelog);
        Add(GvdCommands.DateLimit, ShowDatObm, null, tsmiDatObm, tsbDatObm);
    }

    /// <summary>
    /// Povoli alebo zakaze prikazy a prvky hlavneho okna podla stavu (instalacia, grafikon, INISS).
    /// </summary>
    private void UpdateCommandStates()
    {
        _commands.UpdateStates();

        // rozbalovacie ponuky bez vlastneho prikazu
        tsmiImport.Enabled = HasInstallation;
        tsbImport.Enabled = HasInstallation;
        tsmiUpravit.Enabled = HasGrafikon;
        tscbStanica.Enabled = HasGrafikons;
        tscbObdobie.Enabled = HasGrafikons;
    }

    private void EditSelectedTrain()
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(_ctx.Document.Trains[index], index);
        }
    }

    private void DuplicateSelectedTrain()
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(_ctx.Document.Trains[index], _ctx.Document.Trains.Count, true);
        }
    }
}
