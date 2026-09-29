using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Integration;
using GVDEditor.Properties;
using GVDEditor.Services;
using GVDEditor.UI.Dialogs;
using GVDEditor.UI.EditTrain;
using GVDEditor.UI.Import;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;
using ToolsCore.XML;
using AppRegistry = ToolsCore.Tools.AppRegistry;

namespace GVDEditor.UI.Main;

internal partial class FMain
{
    // ------------------------------------------------------------------ instalacia INISS

    private void ShowOpenDir()
    {
        var dialog = new FolderBrowserDialog { Description = Resources.FMain_Vyberte_priecinok_s_INISS };
        if (dialog.ShowDialog(this) == DialogResult.Cancel)
            return;

        OpenInstallation(dialog.SelectedPath);
    }

    private void RecentDirsClick(object? sender, EventArgs e)
    {
        var menuItem = (ToolStripMenuItem)sender!;
        OpenRecentProject(menuItem.Text!);
    }

    private void OpenRecentProject(string fullPath) => OpenInstallation(fullPath);

    /// <summary>
    /// Otvori instalaciu INISS v priecinku <paramref name="path" /> - zavrie otvoreny grafikon (neulozene zmeny
    /// ponukne ulozit) a nacita zoznam grafikonov.
    /// </summary>
    private void OpenInstallation(string path)
    {
        if (!ConfirmSaveChanges())
            return;

        CloseGrafikon();
        _installationOpen = false;
        _previousSelectedGVD = null;

        try
        {
            _ctx.OpenWorkspace(WorkspaceRepository.Load(path, _loadWarnings));
        }
        catch (Exception e) when (CatchErrors)
        {
            // instalacia sa nacitala len z casti - okno sa vrati do stavu bez otvorenej instalacie
            ShowException(e);
            ClearGrafikonLists();
            UpdateCommandStates();
            return;
        }

        if (InitializeDataList())
            InitializeGUI();
        else
            UpdateCommandStates();
    }

    private void ClearGrafikonLists()
    {
        WithoutSelectionEvents(() =>
        {
            _gvdDirs.Clear();
            _periods.Clear();
            _stations.Clear();
        });
        Text = Application.ProductName;
    }

    /// <summary>
    /// Ukaze varovania z nacitania instalacie a grafikonu (ak nejake su) a zoznam vyprazdni.
    /// </summary>
    private void ShowLoadWarnings()
    {
        if (_loadWarnings.TakeSummary() is { } summary)
            _dialogs.ShowWarning(summary);
    }

    private bool InitializeDataList()
    {
        try
        {
            var stanice = new HashSet<string>();
            var obdobiaList = new List<GVDDirectory>();
            var dirsInData = DirListFile.Read(_ctx.Workspace.DataDir);
            foreach (var dir in dirsInData)
            {
                try
                {
                    var gvd = InfoGvdFile.Read(dir.FullPath);
                    stanice.Add(gvd.ThisStation.Name);
                    obdobiaList.Add(new GVDDirectory(dir, gvd));
                }
                catch (Exception e) when (CatchErrors)
                {
                    ShowException(e);
                }
            }

            _stations.Clear();
            foreach (var st in stanice) _stations.Add(st);

            _periods.Clear();
            foreach (var obd in obdobiaList) _periods.Add(obd);
        }
        catch (DirectoryNotFoundException)
        {
            _dialogs.ShowError(Resources.FMain_Priečinok_neobsahuje_všetky_potrebné_dáta);
            return false;
        }

        AppRegistry.SetUsageOfProject(_ctx.Workspace.INISSDir);
        AppRegistry.SetLastProject(_ctx.Workspace.INISSDir);
        return true;
    }

    private void InitializeGUI()
    {
        _installationOpen = true;

        //vlozit stanice a obdobia do combo boxov v tool stripe
        _gvdDirs.Clear();
        _gvdDirs.AddRange(_periods);

        //premenovat form podla aktualne otvoreneho priecinka
        Text = Application.ProductName + @" - " + _ctx.Workspace.INISSDir;
        DataSaved = true;
        FillInissPrograms();
        UpdateCommandStates();

        //otvoreny projekt sa presunul na zaciatok zoznamu poslednych projektov
        SetRecentProjects();

        // vyber prveho obdobia nacita jeho grafikon
        tscbStanica.ComboBox.SelectedItem = null;
        tscbObdobie.ComboBox.SelectedItem = null;
        tscbStanica.ComboBox.SelectedItem = _stations.FirstOrDefault();
        tscbObdobie.ComboBox.SelectedItem = _periods.FirstOrDefault();
    }

    /// <summary>
    /// Naplní menu naposledy otvorenými projektmi zoradenými od naposledy otvoreného.
    /// </summary>
    private void SetRecentProjects()
    {
        tsmiRecent.DropDownItems.Clear();
        tssbRecentDirs.DropDownItems.Clear();

        var recentDirs = AppRegistry.GetOpenedProjects();

        foreach (var dir in recentDirs)
        {
            var itemA = new ToolStripMenuItem(dir.Path);
            var itemB = new ToolStripMenuItem(dir.Path);

            itemA.Click += RecentDirsClick;
            itemB.Click += RecentDirsClick;
            itemA.ApplyThemeAndFont();
            itemB.ApplyThemeAndFont();
            tsmiRecent.DropDownItems.Add(itemA);
            tssbRecentDirs.DropDownItems.Add(itemB);
        }

        var enabled = recentDirs.Length != 0 && recentDirs[0].Path != "";
        tsmiRecent.Enabled = enabled;
        tssbRecentDirs.Enabled = enabled;
    }

    // ------------------------------------------------------------------ vyber a nacitanie grafikonu

    /// <summary>
    /// Zmeni vyber stanice a obdobia bez nacitania grafikonu (bez udalosti SelectedIndexChanged).
    /// </summary>
    private void WithoutSelectionEvents(Action change)
    {
        tscbStanica.SelectedIndexChanged -= tscbStanica_SelectedIndexChanged;
        tscbObdobie.SelectedIndexChanged -= tscbObdobie_SelectedIndexChanged;
        try
        {
            change();
        }
        finally
        {
            tscbStanica.SelectedIndexChanged += tscbStanica_SelectedIndexChanged;
            tscbObdobie.SelectedIndexChanged += tscbObdobie_SelectedIndexChanged;
        }
    }

    private void tscbStanica_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (tscbStanica.ComboBox.SelectedItem != null)
        {
            var dirs = new List<GVDDirectory>();
            var stanica = tscbStanica.ComboBox.SelectedItem.ToString();
            foreach (var gvdDir in _gvdDirs)
                if (stanica == gvdDir.GVD.ThisStation.Name)
                    dirs.Add(gvdDir);

            _periods.Clear();

            foreach (var dir in dirs) _periods.Add(dir);

            if (dirs.Count != 0 && _newDir == null)
            {
                tscbObdobie.ComboBox.SelectedItem = null;
                tscbObdobie.ComboBox.SelectedIndex = 0;
            }
            else if (_newDir != null)
            {
                tscbObdobie.ComboBox.SelectedItem = null;
                tscbObdobie.ComboBox.SelectedItem = _newDir;
            }

            _previousSelectedGVD ??= (GVDDirectory?)tscbObdobie.ComboBox.SelectedItem;
        }
    }

    private void tscbObdobie_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_removingGVD)
            return;

        var dir = (GVDDirectory?)tscbObdobie.ComboBox.SelectedItem;

        if (dir == null) return;

        if (!ConfirmSaveChanges())
        {
            // zrusene alebo neulozene - ostava otvoreny povodny grafikon, vyber sa k nemu musi vratit
            RestoreSelection(_previousSelectedGVD);
            return;
        }

        _previousSelectedGVD = dir;

        //ak sa jedna o novy grafikon
        if (Equals(dir, _newDir))
        {
            // novy grafikon nesmie nic prevziat z predchadzajuceho (jazyky, priecinok pisiem...)
            _prechod = true;
            _ctx.OpenDocument(GrafikonDocument.CreateNew(_ctx.Workspace.Languages));
            BindDocument();
            _prechod = false;

            GrafikonRepository.CreateNew(dir.Dir.FullPath, dir.GVD, _ctx.Grafikon, _ctx.Workspace.DataDir, _newDirTemplate);

            _newDir = null;
            _grafikonLoaded = true;
            UpdateCommandStates();
            return;
        }

        _prechod = true;
        dgvTrains.DataSource = null;
        _ctx.Document.Trains.Clear();
        _prechod = false;

        //starsi zapis - viac grafikonov v jednom priecinku; po rozdeleni sa zoznam obdobi nacita znova a vyberie prvy novy
        var blocks = AnalyzeBlocks(dir);
        if (blocks.Count > 0 && MigrateBlocks(dir, blocks))
            return;

        // druhy blok by pri nacitani prepisal prvy a pri ulozeni by sa stary zapis znicil - radsej nenacitat nic
        if (blocks.Count > 0)
        {
            _dialogs.ShowWarning(Resources.FMain_Grafikon_s_blokmi_nenacitany);
            ShowLoadWarnings();
            CloseGrafikon();
            UpdateCommandStates();
            return;
        }

        LoadGrafikon(dir);
    }

    /// <summary>
    /// Nacita grafikon na pozadi. Okno medzitym ukazuje povodny grafikon a neda sa pouzivat; nacitany grafikon sa
    /// otvori naraz, pri chybe sa okno vrati do stavu bez otvoreneho grafikonu.
    /// </summary>
    private async void LoadGrafikon(GVDDirectory dir)
    {
        if (_loading)
            return;

        _loading = true;
        Enabled = false;
        var waitForm = new FWait(_ctx);
        waitForm.Show(this);

        GrafikonDocument? document = null;
        Exception? error = null;
        try
        {
            document = await GrafikonService.LoadAsync(dir, _ctx.Workspace, _loadWarnings);
        }
        catch (Exception e) when (CatchErrors)
        {
            error = e;
        }
        finally
        {
            // najprv povolit - zatvorenie okna vlastneneho zakazanym oknom by aktivovalo okno ineho programu
            // a GVDEditor by sa pri naslednom hlaseni skryl za neho
            Enabled = true;
            waitForm.Close();
            _loading = false;
        }

        if (error != null)
            ShowException(error);

        // preskocene riadky a chybajuce nahravky - pouzivatel by o nich mal vediet skor, nez grafikon ulozi
        // pri chybe nacitania by inak ostali v zozname a ukazali sa pri dalsom grafikone
        ShowLoadWarnings();

        if (document != null)
        {
            // vsetky data grafikonu sa vymenia naraz - okno medzitym videlo povodny grafikon
            _ctx.OpenDocument(document);
            BindDocument();
            _grafikonLoaded = true;
        }
        else
        {
            // nacitanie mohlo skoncit v polovici - nesmie ostat zmes dat stareho a noveho grafikonu
            // globalne nastavenia a novy grafikon nepotrebuju otvoreny grafikon - napr. chybajuci typ vlaku sa da doplnit
            // a grafikon potom nacitat znova
            CloseGrafikon();
        }

        UpdateCommandStates();
    }

    /// <summary>
    /// Zavrie otvoreny grafikon - zoznam vlakov ostane naviazany, len prazdny.
    /// </summary>
    private void CloseGrafikon()
    {
        _prechod = true;
        _ctx.CloseDocument();
        _prechod = false;
        _grafikonLoaded = false;
    }

    /// <summary>
    /// Vrati vyber stanice a obdobia na grafikon <paramref name="dir" /> bez jeho opatovneho nacitania.
    /// </summary>
    private void RestoreSelection(GVDDirectory? dir)
    {
        if (dir == null) return;

        WithoutSelectionEvents(() =>
        {
            var station = dir.GVD.ThisStation.Name;
            if (!_periods.Contains(dir))
            {
                _periods.Clear();
                foreach (var gvdDir in GVDSelectionLists.PeriodsOf(_gvdDirs, station)) _periods.Add(gvdDir);
            }

            tscbStanica.ComboBox.SelectedItem = station;
            tscbObdobie.ComboBox.SelectedItem = dir;
        });
    }

    private List<GvdBlock> AnalyzeBlocks(GVDDirectory dir)
    {
        try
        {
            //grafikon priamo v DATA (bez DirList.TXT) sa presuva do vlastneho priecinka vzdy, aj ked ma jediny blok
            return BlockMigrator.Analyze(_ctx.Workspace.DataDir, dir.Dir.FullPath, dir.GVD, dir.Dir.DirName, _ctx.Stations, dir.Dir.IsDataRoot);
        }
        catch (Exception e)
        {
            //chybny Export3A ohlasi az nacitanie grafikonu
            Log.Exception(e);
            return [];
        }
    }

    /// <summary>
    /// Ponukne rozdelenie priecinka s blokmi <c>/stanica</c> do samostatnych priecinkov.
    /// </summary>
    /// <returns><see langword="true" />, ak sa grafikon rozdelil a zoznam obdobi bol nacitany znova.</returns>
    private bool MigrateBlocks(GVDDirectory dir, List<GvdBlock> blocks)
    {
        var question = dir.Dir.IsDataRoot
            ? string.Format(Resources.FMain_Grafikon_v_koreni_otazka, dir.Dir.FullPath, blocks.Count)
            : string.Format(Resources.FMain_Grafikon_obsahuje_bloky_otazka, dir.Dir.FullPath, blocks.Count);
        if (_dialogs.ShowQuestion(question) != DialogResult.Yes)
            return false;

        using var form = new FBlockMigration(_ctx, dir.Dir.FullPath, blocks, dir.Dir.IsDataRoot);
        if (form.ShowDialog(this) != DialogResult.OK)
            return false;

        List<DirList> newDirs;
        try
        {
            newDirs = BlockMigrator.Migrate(_ctx.Workspace.DataDir, dir.Dir.FullPath, dir.Dir, dir.GVD, blocks);
        }
        catch (Exception e)
        {
            Log.Exception(e);
            _dialogs.ShowError(string.Format(Resources.FMain_Rozdelenie_zlyhalo, e.Message));
            return false;
        }

        _dialogs.ShowInfo(string.Format(Resources.FMain_Grafikon_rozdeleny, string.Join(", ", newDirs.Select(d => d.DirName)), dir.Dir.FullPath));

        _ctx.Workspace.GVDDirs = DirListFile.Read(_ctx.Workspace.DataDir);
        DataSaved = true;
        _previousSelectedGVD = null;

        if (!InitializeDataList())
            return true;

        _gvdDirs.Clear();
        _gvdDirs.AddRange(_periods);

        var first = _gvdDirs.FirstOrDefault(o => o.Dir.DirName.Equals(newDirs[0].DirName, StringComparison.OrdinalIgnoreCase));

        //zmena stanice by sama vybrala prve obdobie a spustila nacitanie - vybrat treba az prvy novy priecinok
        _removingGVD = true;
        tscbStanica.ComboBox.SelectedItem = null;
        tscbStanica.ComboBox.SelectedItem = first?.GVD.ThisStation.Name ?? _stations.FirstOrDefault();
        tscbObdobie.ComboBox.SelectedItem = null;
        _removingGVD = false;

        tscbObdobie.ComboBox.SelectedItem = first ?? _periods.FirstOrDefault();
        return true;
    }

    // ------------------------------------------------------------------ ulozenie

    /// <summary>
    /// Ak ma otvoreny grafikon neulozene zmeny, opyta sa na ich ulozenie.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel akciu zrusil alebo sa grafikon nepodarilo ulozit.</returns>
    private bool ConfirmSaveChanges()
    {
        if (!HasInstallation || DataSaved)
            return true;

        switch (_dialogs.ShowQuestion(Resources.FMain_Save_Changes, MessageBoxButtons.YesNoCancel))
        {
            case DialogResult.Yes:
                return DoSave();
            case DialogResult.No:
                DataSaved = true;
                return true;
            default:
                return false;
        }
    }

    /// <returns><see langword="false" />, ak sa grafikon nepodarilo ulozit.</returns>
    private bool DoSave()
    {
        // dva vlaky s rovnakym cislom, nazvom, typom a variantou by INISS nerozlisil (trasu by dostal len prvy)
        NormalizeVariants();

        try
        {
            GrafikonService.Save(_previousSelectedGVD!, _ctx.Grafikon, _ctx.Config.AutoTableText);
        }
        catch (Exception e) when (CatchErrors)
        {
            _dialogs.ShowError(_ctx.Config.DebugModeGUI == DebugMode.OnlyMessage ? e.Message : e.ToString());
            return false;
        }

        DataSaved = true;
        return true;
    }

    // ------------------------------------------------------------------ novy grafikon, import, analyza

    private void ShowAnalyzeGVD()
    {
        var fan = new FAnalyzer(_ctx, (tscbObdobie.SelectedItem as GVDDirectory)!, this);
        fan.ShowDialog();

        // opravy menia grafikon v pamati - bez oznacenia by sa pri zatvoreni bez otazky stratili
        if (fan.DataChanged)
        {
            DataSaved = false;
            _ctx.Document.Trains.ResetBindings();
        }
    }

    private void ShowNewGVD()
    {
        var nsf = new FNewGrafikon(_ctx, _gvdDirs);
        if (nsf.ShowDialog() != DialogResult.OK)
            return;

        var gvd = nsf.GvdInfo;
        _newDirTemplate = nsf.Template;

        var dgyv = GrafikonService.Register(_ctx.Workspace, nsf.NewDir, gvd);
        _gvdDirs.Add(dgyv);
        _newDir = dgyv;

        if (!_stations.Contains(gvd.ThisStation.Name)) _stations.Add(gvd.ThisStation.Name);

        if ((string?)tscbStanica.ComboBox.SelectedItem == gvd.ThisStation.Name)
            _periods.Add(dgyv);

        UpdateCommandStates();

        // vyber stanice vyberie novy grafikon a vytvori jeho subory
        tscbStanica.ComboBox.SelectedItem = null;
        tscbStanica.ComboBox.SelectedItem = gvd.ThisStation.Name;
    }

    private void ShowImportGVD()
    {
        var dialog = new FolderBrowserDialog { Description = Resources.FMain_Vyberte_priecinok_s_grafikonom };
        if (dialog.ShowDialog(this) == DialogResult.Cancel)
            return;

        GVDDirectory dgyv;
        try
        {
            dgyv = GrafikonService.Import(_ctx.Workspace, dialog.SelectedPath, _gvdDirs);
        }
        catch (Exception e)
        {
            Log.Exception(e);
            _dialogs.ShowError(e.Message);
            return;
        }

        _gvdDirs.Add(dgyv);
        var station = dgyv.GVD.ThisStation.Name;
        if (!_stations.Contains(station)) _stations.Add(station);
        if ((string?)tscbStanica.ComboBox.SelectedItem == station) _periods.Add(dgyv);

        _dialogs.ShowInfo(string.Format(Resources.FMain_Import_grafikonu_hotovy, dgyv.PeriodFormatted, dgyv.Dir.FullPath));

        // prvy grafikon instalacie - hlavne okno ho rovno otvori a spristupni prikazy
        if (_gvdDirs.Count == 1 && InitializeDataList())
            InitializeGUI();
        else
            UpdateCommandStates();
    }

    private void ShowImportData()
    {
        var fid = new FImportData(_ctx, ((GVDDirectory)tscbObdobie.ComboBox.SelectedItem!).GVD);
        if (fid.ShowDialog() != DialogResult.OK)
            return;

        // rovnako ako import z ELIS - pri nahradeni odstranit aj texty tabul odkazujuce na povodne vlaky
        if (fid.ReplaceTrains)
            RemoveAllTrains();

        foreach (var train in fid.ImportedTrains) _ctx.Document.Trains.Add(train);
        NormalizeVariants();
        _ctx.Document.Trains.ResetBindings();
        DataSaved = false;
    }

    /// <summary>
    /// Import vlakov z ELIS: data sa nacitaju na pozadi, nepriradene stanice priradi pouzivatel a vlaky sa pridaju
    /// do grafikonu.
    /// </summary>
    private async void ShowImportELIS()
    {
        var gvdDir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;

        var fimport = new FELISImport(gvdDir.GVD.ThisStation.Name, _ctx.Document.Trains.Count);
        if (fimport.ShowDialog() != DialogResult.OK)
            return;

        ElisImport? import = null;
        Exception? error = null;
        var waitForm = new FWait(_ctx, Resources.FMain_Import_prebieha);
        waitForm.Show(this);
        Enabled = false;
        try
        {
            import = await ElisImportService.LoadAsync(fimport.ResultOptions, gvdDir, _ctx.Workspace.TrainsTypes, _ctx.Document.Operators,
                _ctx.Document.Tracks.FirstOrDefault()!, _ctx.Document.Trains, _ctx.Stations);
        }
        catch (Exception e) when (CatchErrors)
        {
            error = e;
        }
        finally
        {
            Enabled = true;
            waitForm.Close();
        }

        if (error != null)
        {
            ShowException(error);
            return;
        }

        Cursor.Current = Cursors.AppStarting;

        //stanice, ktore ELIS pomenuva inak, doriesi pouzivatel - a volba sa zapamata
        if (import!.Unresolved.Count != 0 && !ResolveStations(import))
            return;

        List<Train> imported;
        try
        {
            imported = ElisImportService.Convert(import);
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
            _dialogs.ShowError(exception.Message);
            return;
        }

        var removed = 0;
        if (import.ReplaceTrains)
        {
            removed = _ctx.Document.Trains.Count;
            RemoveAllTrains();
        }

        foreach (var train in imported) _ctx.Document.Trains.Add(train);
        NormalizeVariants();
        _ctx.Document.Trains.ResetBindings();

        DataSaved = false;

        _dialogs.ShowInfo(string.Format(Resources.FMain_Import_z_ELIS_dokončený, removed, imported.Count));
    }

    /// <summary>
    /// Necha pouzivatela priradit stanice, ktore sa nepodarilo rozpoznat automaticky,
    /// a priradenie ulozi do grafikonu.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel import zrusil.</returns>
    private bool ResolveStations(ElisImport import)
    {
        var dialog = new FELISStations(_ctx, import.Unresolved, import.Client.Stations);
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return false;

        //ulozenie priradenia nie je kriticke - import moze pokracovat, len sa nabuduce spyta znova
        if (ElisImportService.SaveStationMap(import, dialog.Result, _ctx.Config.Language) is { } error)
            Log.Exception(error);

        return true;
    }

    // ------------------------------------------------------------------ vlaky

    private void ShowEditTrain(Train? train, int row, bool copy = false, EditTrainPage startPage = EditTrainPage.Vlak)
    {
        var gvdDir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;
        var eform = new FEditTrain(_ctx, train, row, gvdDir.GVD, copy, gvdDir.Dir.FullPath, startPage);
        var result = eform.ShowDialog();
        if (result == DialogResult.OK)
        {
            if (train == null || row == _ctx.Document.Trains.Count)
                _ctx.Document.Trains.Add(eform.ThisTrain!);
            else
                _ctx.Document.Trains.ResetBindings();

            // novy vlak, kopia alebo zmena cisla, nazvu ci typu - cisla variant prideli GVDEditor
            NormalizeVariants();
            DataSaved = false;
        }
    }

    private void DoDeleteTrains()
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            foreach (DataGridViewRow row in dgvTrains.SelectedRows)
            {
                if (!_ctx.Config.AutoTableText)
                    DeleteTTexts((row.DataBoundItem as Train)!);

                _ctx.Document.Trains.RemoveAt(row.Index);
            }

            // varianta, ktora ostala sama, dostane -1
            NormalizeVariants();
            _ctx.Document.Trains.ResetBindings();

            DataSaved = false;
        }
    }

    /// <summary>
    /// Odstrani vsetky vlaky grafikonu aj texty tabul, ktore sa na ne odvolavaju.
    /// </summary>
    private void RemoveAllTrains()
    {
        if (!_ctx.Config.AutoTableText)
            foreach (var train in _ctx.Document.Trains)
                DeleteTTexts(train);

        _prechod = true;
        _ctx.Document.Trains.Clear();
        _prechod = false;
    }

    private void DeleteTTexts(Train vlak)
    {
        foreach (var tt in _ctx.Document.TableTexts)
            for (var i = tt.Trains.Count - 1; i >= 0; i--)
                if (tt.Trains[i].Train == vlak)
                    tt.Trains.RemoveAt(i);
    }
}
