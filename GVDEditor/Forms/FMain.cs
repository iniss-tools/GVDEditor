using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using Iniss.Elis;
using Microsoft.VisualBasic.FileIO;
using ToolsCore;
using ToolsCore.Entities;
using ToolsCore.Forms;
using ToolsCore.Tools;
using ToolsCore.XML;
using AppRegistry = ToolsCore.Tools.AppRegistry;

namespace GVDEditor.Forms;

/// <summary>
///     Hlavný formulár
/// </summary>
public partial class FMain : Form
{
    /// <summary>
    ///     Dostupné stanice.
    /// </summary>
    public static BindingList<string> Stanice { get; } = new();

    /// <summary>
    ///     Všetky dostupne priečinky s grafikonmi.
    /// </summary>
    public static BindingList<GVDDirectory> ObdobiaList { get; } = new();

    private readonly List<GVDDirectory> _gvdDirs = new();
    private Process? _actualINISSProcess;
    private bool _error;
    private string? _lastINISSStart;
    private GVDDirectory? _newDir;
    private StateDgmTemplate _newDirTemplate = StateDgmTemplate.Slovak;
    private bool _prechod;
    private GVDDirectory? _previousSelectedGVD;
    private bool _removingGVD;
    private FWait? _waitForm;

    /// <summary>
    ///     Vytvori nový formulár typu <see cref="FMain"/>.
    /// </summary>
    public FMain()
    {
        InitializeComponent();

        mainMenu.Renderer = new ToolStripProfessionalRenderer(new FormUtils.LightColorTable());

        tsslSelTrainName.Font = GlobData.Config.Fonts.StateRow.Font;
        tsslSelTrainVariants.Font = GlobData.Config.Fonts.StateRow.Font;
        tsslTrainCount.Font = GlobData.Config.Fonts.StateRow.Font;
        tsslTrainCountWithVariants.Font = GlobData.Config.Fonts.StateRow.Font;

        dgvTrains.RowHeadersVisible = GlobData.Config.ShowRowsHeader;

        SetShortcuts();
        SetColumns();
        SetColumnsAutoWidth();

        ApplyMenuMode();

        smerovanieDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = new Bitmap(1, 1);
        backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
        backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

        SetRecentProjects();

        if (tscbStanica.ComboBox != null) tscbStanica.ComboBox.DataSource = Stanice;
        if (tscbObdobie.ComboBox != null) tscbObdobie.ComboBox.DataSource = ObdobiaList;

        this.ApplyThemeAndFonts();
        if (GlobData.UsingStyle.HighlightStatusBar)
        {
            statusStrip.BackColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.BackColor;
            tsslSelTrainName.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslSelTrainVariants.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslTrainCount.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslTrainCountWithVariants.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
        }
    }

    private void FMain_Load(object sender, EventArgs e)
    {
        AppRegistry.RegisterJumpList();

        var path = Utils.GetProjectPathFromArgs();
        if (path is null && GlobData.Config.Startup == StartupType.LastProject)
            path = AppRegistry.GetLastProject();

        if (!string.IsNullOrWhiteSpace(path))
            OpenRecentProject(path);
    }

    private bool DataSaved
    {
        get;
        set
        {
            if (value == field)
                return;

            field = value;
            if (field)
                Text = Text.Replace("*", "");
            else
                Text = Application.ProductName + @" - *" + GlobData.INISSDir;
        }
    } = true;

    protected override CreateParams CreateParams
    {
        get
        {
            var handleParam = base.CreateParams;
            handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
            return handleParam;
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // pri pisani do bunky patria Delete a Insert textovemu polu - skratka Odstranit oznacene by inak zmazala cely vlak
        if (dgvTrains.EditingControl is TextBoxBase && IsTextEditingKey(keyData))
            return false;

        // polozka ponuky s podponukou svoju skratku nespracuje - Lokalne a Globalne nastavenia treba otvorit tu
        if (keyData != Keys.None && keyData == tsmiVlastnostiStanice.ShortcutKeys && tsmiVlastnostiStanice.Enabled)
        {
            ShowLocalSettings();
            return true;
        }

        if (keyData != Keys.None && keyData == tsmiGlobalSettings.ShortcutKeys && tsmiGlobalSettings.Enabled)
        {
            ShowGlobalSettings();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private static bool IsTextEditingKey(Keys keyData)
        => (keyData & Keys.KeyCode) is Keys.Delete or Keys.Insert or Keys.Back && (keyData & Keys.Alt) == 0;

    private void BackgroundWorker1_DoWork(object? sender, DoWorkEventArgs e)
    {
        // druhy blok by pri nacitani prepisal prvy a pri ulozeni by sa stary zapis znicil - radsej nenacitat nic
        if (((PathAndGVD)e.Argument!).BlocksDeclined)
        {
            Program.MainForm.Invoke(() => Utils.ShowWarning(Resources.FMain_Grafikon_s_blokmi_nenacitany));
            _error = true;
            return;
        }

        if (GlobData.Config.DebugModeGUI != DebugMode.AppCrash)
            try
            {
                ProccessData((PathAndGVD)e.Argument!);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                Program.MainForm.Invoke(delegate()
                {
                    if (GlobData.Config.DebugModeGUI == DebugMode.OnlyMessage)
                        FError.ShowError(exception.Message);
                    if (GlobData.Config.DebugModeGUI == DebugMode.DetailInfo)
                        FError.ShowError(exception.ToString());
                }
                );

                _error = true;
                return;
            }
        else
            ProccessData((PathAndGVD)e.Argument!);

        _error = false;
    }

    private static void ProccessData(PathAndGVD pathgvd)
    {
        GlobData.CustomStations = new ExBindingList<Station>(TxtParser.ReadCustomStations(pathgvd.Path, pathgvd.Gvd));

        var (tabtabs, catalogs, physicals, logicals) = TxtParser.ReadTables(pathgvd.Path);
        GlobData.TabTabs = new ExBindingList<TableTabTab>(tabtabs);
        GlobData.TableCatalogs = new ExBindingList<TableCatalog>(catalogs);
        GlobData.TablePhysicals = new ExBindingList<TablePhysical>(physicals);
        GlobData.TableLogicals = new ExBindingList<TableLogical>(logicals);

        var operators = TxtParser.ReadOperators(pathgvd.Path);
        GlobData.Operators = new ExBindingList<Operator>(operators)
        {
            FireEventOnSort = true
        };

        var tracks = TxtParser.ReadTracks(pathgvd.Path);
        GlobData.Tracks = new ExBindingList<Track>(tracks)
        {
            FireEventOnSort = true
        };

        var nastupistia = new HashSet<Platform>();
        foreach (var kolaj in GlobData.Tracks)
            nastupistia.Add(kolaj.Platform);
        GlobData.Platforms = new ExBindingList<Platform>(nastupistia.ToList());

        (GlobData.ReportVariants,GlobData.ReportTypes,GlobData.LocalLanguages) = TxtParser.ReadLocalCategori(pathgvd.Path);
        InitDruhyReportov();

        var allSounds = new List<FyzSound>();
        foreach (var language in GlobData.LocalLanguages)
            allSounds.AddRange(language.IsBasic ? GlobData.Sounds : RawBankParser.ReadFyzZvukFile(GlobData.RawBankDir, language));

        try { GlobData.Radenia = TxtParser.ReadRazeni1(pathgvd.Path, allSounds); }catch (FileNotFoundException) { }

        GlobData.Trains = new ExBindingList<Train>(TxtParser.ReadTrains(pathgvd.Path));

        GlobData.TableTexts = new ExBindingList<TableText>(TxtParser.ReadTTexts(pathgvd.Path, GlobData.Trains));
        GlobData.TableFonts = new ExBindingList<TableFont>(TxtParser.ReadTableFonts(pathgvd.Path));
    }

    private void BackgroundWorker1_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        _waitForm!.Close();

        if (!_error)
        {
            // preskocene riadky a chybajuce nahravky - pouzivatel by o nich mal vediet skor, nez grafikon ulozi
            LoadWarnings.ShowSummary();

            Kolaj.DataSource = GlobData.Tracks;
            Dopravca.DataSource = GlobData.Operators;

            _prechod = true;
            dgvTrains.DataSource = GlobData.Trains;
            _prechod = false;

            SetColumnsAutoWidth();

            //sfunkčniť tlačidlá v tool stripe
            tsbAddTrain.Enabled = true;
            tsbCopyTrain.Enabled = true;
            tsbEditTrain.Enabled = true;
            tsbDeleteTrain.Enabled = true;
            tsbSave.Enabled = true;
            tsmiSave.Enabled = true;
            tsmiAnalyze.Enabled = true;
            tsbAnalyze.Enabled = true;
            tsbStanica.Enabled = true;
            tsbGlobalSettings.Enabled = true;
            tsmiUpravit.Enabled = true;
            tsmimAddTrain.Enabled = true;
            tsmimEditTrain.Enabled = true;
            tsmiDeleteTrain.Enabled = true;
            tsmiDuplikovat.Enabled = true;
            tsmiVlastnostiStanice.Enabled = true;
            tsmiGlobalSettings.Enabled = true;
            tsbAddGVD.Enabled = true;
            tsmiNew.Enabled = true;
            SetImportEnabled(true);
            tsmiStateDgm.Enabled = true;
            ChangeEnableMenuItemsGSettings(true);
            ChangeEnableMenuItemsLSettings(true);
        }
        else
        {
            _prechod = true;
            GlobData.Trains.Clear();
            _prechod = false;

            //znefunkcnit tlacidla v tool stripe
            tsbAddTrain.Enabled = false;
            tsbCopyTrain.Enabled = false;
            tsbEditTrain.Enabled = false;
            tsbDeleteTrain.Enabled = false;
            tsbSave.Enabled = false;
            tsbStanica.Enabled = false;
            tsbGlobalSettings.Enabled = false;
            tsmiUpravit.Enabled = false;
            tsmimAddTrain.Enabled = false;
            tsmimEditTrain.Enabled = false;
            tsmiDeleteTrain.Enabled = false;
            tsmiDuplikovat.Enabled = false;
            tsmiVlastnostiStanice.Enabled = false;
            tsmiGlobalSettings.Enabled = false;
            tsmiSave.Enabled = false;
            tsbSave.Enabled = false;
            tsmiAnalyze.Enabled = false;
            tsbAnalyze.Enabled = false;
            tsbAddGVD.Enabled = false;
            tsmiNew.Enabled = false;
            SetImportEnabled(false);
            ChangeEnableMenuItemsGSettings(false);
            ChangeEnableMenuItemsLSettings(false);
        }
    }

    /// <returns><see langword="false" />, ak sa grafikon nepodarilo ulozit.</returns>
    private bool DoSave()
    {
        if (GlobData.Config.DebugModeGUI == DebugMode.AppCrash)
            DoSaveInternal();
        else
            try
            {
                DoSaveInternal();
            }
            catch (Exception e)
            {
                var monly = GlobData.Config.DebugModeGUI == DebugMode.OnlyMessage;
                Utils.ShowError(monly ? e.Message : e.ToString());
                return false;
            }

        return true;

        void DoSaveInternal()
        {
            var dir = _previousSelectedGVD!;

            if (GlobData.Config.AutoTableText)
                GenerateTableTextWhileSaving(dir);

            //ukladanie zapisuje pätnásť súborov po sebe; keby niektorý zápis zlyhal, zvyšok
            //by ostal v pôvodnom stave a grafikon by sa pri ďalšom otvorení hlásil ako chybný
            // hlavicka nesie pocet vlakov a datum poslednej upravy
            dir.GVD.TrainCount = GlobData.Trains.Count;
            dir.GVD.CreateData = DateTime.Today;

            var transaction = new FileTransaction(dir.Dir.FullPath);
            try
            {
                TxtParser.WriteTrains(dir.Dir.FullPath, GlobData.Trains, dir.GVD, GlobData.ReportVariants);
                TxtParser.WriteRazeni1(dir.Dir.FullPath, GlobData.Radenia, GlobData.LocalLanguages);

                TxtParser.WriteTables(dir.Dir.FullPath, GlobData.TabTabs, GlobData.TableCatalogs, GlobData.TablePhysicals, GlobData.TableLogicals);
                TxtParser.WriteTTexts(dir.Dir.FullPath, GlobData.TableTexts);
                TxtParser.WriteTracks(dir.Dir.FullPath, GlobData.Tracks);
                TxtParser.WriteInfoGVD(dir.Dir.FullPath, dir.GVD);
                TxtParser.WriteOperators(dir.Dir.FullPath, GlobData.Operators);
                TxtParser.WriteModeTabs(dir.Dir.FullPath, GlobData.TableFonts, GlobData.TableFontDir);
                TxtParser.WriteLocalCategori(dir.Dir.FullPath, GlobData.ReportVariants, GlobData.ReportTypes, GlobData.LocalLanguages);
                TxtParser.WriteCustomStations(dir.Dir.FullPath, GlobData.CustomStations, dir.GVD);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);

                var message = transaction.TryRollback()
                    ? string.Format(Resources.FMain_Uloženie_grafikonu_zlyhalo_zmeny_boli_vrátené, exception.Message)
                    : string.Format(Resources.FMain_Uloženie_grafikonu_zlyhalo_a_nepodarilo_sa_obnoviť, exception.Message,
                        transaction.BackupPath);

                throw new InvalidOperationException(message, exception);
            }

            transaction.Commit();
            DataSaved = true;
        }
    }

    private void ShowAnalyzeGVD()
    {
        var fan = new FAnalyzer((tscbObdobie.SelectedItem as GVDDirectory)!);
        fan.ShowDialog();

        // opravy menia grafikon v pamati - bez oznacenia by sa pri zatvoreni bez otazky stratili
        if (fan.DataChanged)
        {
            DataSaved = false;
            GlobData.Trains.ResetBindings();
        }
    }

    private void ShowNewGVD()
    {
        var nsf = new FNewGrafikon(_gvdDirs);
        var result = nsf.ShowDialog();
        if (result == DialogResult.OK)
        {
            var gvd = nsf.GvdInfo;
            var dir = nsf.NewDir;
            _newDirTemplate = nsf.Template;

            if (Stanice.Count == 0 || ObdobiaList.Count == 0)
            {
                //sfunkcnit tlacidla v toolstripe
                tsbAddGVD.Enabled = true;
                tsbAddTrain.Enabled = true;
                tsbCopyTrain.Enabled = true;
                tsbEditTrain.Enabled = true;
                tsbDeleteTrain.Enabled = true;
                tsbSave.Enabled = true;
                tsbStanica.Enabled = true;
                tscbStanica.Enabled = true;
                tscbObdobie.Enabled = true;
                tsbGlobalSettings.Enabled = true;
                tsmiNew.Enabled = true;
                SetImportEnabled(true);
                tsmiUpravit.Enabled = true;
                tsmimAddTrain.Enabled = true;
                tsmimEditTrain.Enabled = true;
                tsmiDeleteTrain.Enabled = true;
                tsmiDuplikovat.Enabled = true;
                tsmiVlastnostiStanice.Enabled = true;
                tsmiGlobalSettings.Enabled = true;
                tsmiSave.Enabled = true;
                tsbSave.Enabled = true;
                tsmiAnalyze.Enabled = true;
                tsbAnalyze.Enabled = true;
                tssbStartINISS.Enabled = true;
                tsmimStartINISS.Enabled = true;
                ChangeEnableMenuItemsGSettings(true);
                ChangeEnableMenuItemsLSettings(true);
            }

            if (!Stanice.Contains(gvd.ThisStation.Name)) Stanice.Add(gvd.ThisStation.Name);

            if ((string)tscbStanica.ComboBox.SelectedItem! == gvd.ThisStation.Name)
                ObdobiaList.Add(new GVDDirectory(dir, gvd));

            var dirs = TxtParser.ReadDirList();
            dirs.Add(dir);
            TxtParser.WriteDirList(dirs);
            GlobData.GVDDirs.Add(dir);
            Directory.CreateDirectory(dir.FullPath);
            TxtParser.WriteInfoGVD(dir.FullPath, gvd);

            var dgyv = new GVDDirectory(dir, gvd);
            _gvdDirs.Add(dgyv);
            _newDir = dgyv;

            tscbStanica.ComboBox.SelectedItem = null;
            tscbStanica.ComboBox.SelectedItem = gvd.ThisStation.Name;
        }
    }

    private void ShowImportData()
    {
        var fid = new FImportData(((GVDDirectory)tscbObdobie.ComboBox.SelectedItem!).GVD);
        if (fid.ShowDialog() != DialogResult.OK)
            return;

        // rovnako ako import z ELIS - pri nahradeni odstranit aj texty tabul odkazujuce na povodne vlaky
        if (fid.ReplaceTrains)
            RemoveAllTrains();

        foreach (var train in fid.ImportedTrains) GlobData.Trains.Add(train);
        GlobData.Trains.ResetBindings();
        DataSaved = false;
    }

    private void ShowImportELIS()
    {
        var gvdDir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;
        var gvd = gvdDir.GVD;

        var fimport = new FELISImport(gvd.ThisStation.Name, GlobData.Trains.Count);
        if (fimport.ShowDialog() != DialogResult.OK)
            return;

        var data = fimport.ResultOptions;
        data.GVDInfo = gvd;
        data.GVDPath = gvdDir.Dir.FullPath;
        data.Track = GlobData.Tracks.FirstOrDefault()!;

        //pri nahradeni sa existujuce vlaky zahodia, takze do cislovania variant nevstupuju
        data.DefTrains = data.ReplaceTrains ? new List<Train>() : GlobData.Trains.ToList();

        _error = false;
        _waitForm = new FWait("Prebieha importovanie údajov...");
        _waitForm.Show(this);
        if (!bWorkerELIS.IsBusy) bWorkerELIS.RunWorkerAsync(data);
    }

    private void ShowImportGVD()
    {
        var dialog = new FolderBrowserDialog { Description = "Vyberte priečinok obsahujúci grafikon" };
        if (dialog.ShowDialog(this) == DialogResult.Cancel) 
            return;

        var selectedPath = dialog.SelectedPath;

        // hlavicka sa cita zo zdroja - neplatny grafikon sa do DATA vobec neskopiruje
        GVDInfo gvd;
        try
        {
            gvd = TxtParser.ReadInfoGVD(selectedPath);
        }
        catch (Exception e)
        {
            Utils.ShowError($@"{Resources.FMain_Priečinok_neobsahuje_všetky_potrebné_dáta} {e.Message}");
            Log.Exception(e);
            return;
        }

        var error = GVDImport.Check(selectedPath, GlobData.DataDir, gvd, _gvdDirs, out var newDirPath);
        if (error != null)
        {
            Utils.ShowError(error);
            return;
        }

        try
        {
            if (!string.Equals(Path.GetFullPath(selectedPath).TrimEnd(Path.DirectorySeparatorChar), Path.GetFullPath(newDirPath),
                    StringComparison.OrdinalIgnoreCase))
                Utils.CopyDirectory(selectedPath, newDirPath);

            // grafikon sa dalej upravuje a uklada v kopii v DATA, nie v povodnom priecinku
            var dirList = new DirList { DirName = Path.GetFileName(newDirPath), FullPath = newDirPath };
            var dgyv = new GVDDirectory(dirList, gvd);

            var dirs = TxtParser.ReadDirList();
            dirs.Add(dirList);
            TxtParser.WriteDirList(dirs);

            GlobData.GVDDirs.Add(dirList);
            _gvdDirs.Add(dgyv);

            if (!Stanice.Contains(gvd.ThisStation.Name)) Stanice.Add(gvd.ThisStation.Name);
            if ((string?)tscbStanica.ComboBox.SelectedItem == gvd.ThisStation.Name) ObdobiaList.Add(dgyv);

            Utils.ShowInfo(string.Format(Resources.FMain_Import_grafikonu_hotovy, dgyv.PeriodFormatted, newDirPath));

            // prvy grafikon instalacie - hlavne okno ho rovno otvori a spristupni prikazy
            if (_gvdDirs.Count == 1 && InitializeDataList())
                InitializeGUI();
        }
        catch (Exception e)
        {
            Utils.ShowError(e.Message);
            Log.Exception(e);
        }
    }

    private void ShowAppSettings(string? page = null)
    {
        var old = GlobData.Config;
        var form = new FAppSettings(GlobData.Config, GlobData.Styles);
        if (page != null) form.PreselectMenuItem(page);
        if (form.ShowDialog() != DialogResult.OK)
            return;

        UpdateMainUI();

        // tieto nastavenia sa inak nacitaju len pri starte programu
        DateLimit.Loc = GlobData.Config.DateLimitLocate == AppLanguage.Czech ? DateLimit.Locale.Cz : DateLimit.Locale.Sk;
        Log.DoAppLogs = GlobData.Config.LoggingInfo;
        Log.DoErrorLogs = GlobData.Config.LoggingError;

        if (old.Language != GlobData.Config.Language || old.ClassicGUI != GlobData.Config.ClassicGUI)
            Utils.ShowInfo(Resources.FMain_Nastavenia_po_restarte);
    }

    /// <summary>
    ///     Zobrazi ponuku a panel nastrojov podla nastaveni. Bez panela nastrojov sa vyber stanice a obdobia presunie do ponuky.
    /// </summary>
    private void ApplyMenuMode()
    {
        var menu = GlobData.Config.DesktopMenuMode;
        mainMenu.Visible = menu is DesktopMenu.MsTs or DesktopMenu.MsOnly;
        toolMenu.Visible = menu is DesktopMenu.MsTs or DesktopMenu.TsOnly;

        // polozka moze byt len v jednom ToolStripe - pridanie do jedneho ju z druheho odoberie
        if (menu == DesktopMenu.MsOnly)
        {
            if (!mainMenu.Items.Contains(tscbStanica))
                mainMenu.Items.AddRange(new ToolStripItem[] { toolStripLabel1, tscbStanica, toolStripLabel2, tscbObdobie });
        }
        else if (!toolMenu.Items.Contains(tscbStanica))
        {
            var index = toolMenu.Items.IndexOf(toolStripSeparator13) + 1;
            toolMenu.Items.Insert(index++, toolStripLabel1);
            toolMenu.Items.Insert(index++, tscbStanica);
            toolMenu.Items.Insert(index++, toolStripLabel2);
            toolMenu.Items.Insert(index, tscbObdobie);
        }
    }

    private void UpdateMainUI()
    {
        ApplyMenuMode();
        dgvTrains.RowHeadersVisible = GlobData.Config.ShowRowsHeader;

        this.ApplyThemeAndFonts();
        SetColumns();
        SetColumnsAutoWidth();
        SetShortcuts();
        Refresh();
        AppInit.MsgBoxStyleInit(GlobData.UsingStyle, GlobData.Config);
        if (GlobData.UsingStyle.HighlightStatusBar)
        {
            statusStrip.BackColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.BackColor;
            tsslSelTrainName.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslSelTrainVariants.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslTrainCount.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
            tsslTrainCountWithVariants.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
        }
    }

    private void ShowInfoApp()
    {
        var form = new FAboutApp(Resources.AboutAppDescription, Resources.gvd);
        form.ShowDialog(this);
    }

    /// <returns><see langword="true" />, ak pouzivatel nastavenia ulozil.</returns>
    internal bool ShowLocalSettings(int startIndex = -1)
    {
        var dir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;
        // FLocalSettings meni dir.GVD priamo, povodne hodnoty treba zapamatat vopred
        var oldStation = dir.GVD.ThisStation.Name;
        var oldPeriod = dir.Period;
        var wasSaved = DataSaved;
        var svform = new FLocalSettings(dir, startIndex);
        var result = svform.ShowDialog();
        if (result != DialogResult.OK)
        {
            // Zrusit/krizik vratil vsetky data - obnova zoznamov nesmie grafikon oznacit ako zmeneny
            DataSaved = wasSaved;
            return false;
        }

        RefreshStationAndPeriod(dir, oldStation, oldPeriod);

        GlobData.TableFontDir = svform.FontDir;
        DataSaved = false;
        GlobData.Trains.ResetBindings();
        return true;
    }

    /// <summary>
    ///     Po zmene stanice alebo obdobia platnosti grafikonu v lokalnych nastaveniach aktualizuje comboboxy
    ///     Stanica a Obdobie tak, aby grafikon <paramref name="dir" /> ostal vybraty.
    /// </summary>
    private void RefreshStationAndPeriod(GVDDirectory dir, string oldStation, string oldPeriod)
    {
        var newStation = dir.GVD.ThisStation.Name;
        if (newStation == oldStation && dir.Period == oldPeriod) return;

        // zmena zdrojov comboboxov by cez SelectedIndexChanged znovu nacitala grafikon zo suborov
        // a zahodila neulozene zmeny (vratane tych z lokalnych nastaveni)
        tscbStanica.SelectedIndexChanged -= tscbStanica_SelectedIndexChanged;
        tscbObdobie.SelectedIndexChanged -= tscbObdobie_SelectedIndexChanged;
        try
        {
            if (newStation != oldStation)
            {
                GVDSelectionLists.RenameStation(Stanice, _gvdDirs, oldStation, newStation);

                ObdobiaList.Clear();
                foreach (var gvdDir in GVDSelectionLists.PeriodsOf(_gvdDirs, newStation)) ObdobiaList.Add(gvdDir);

                tscbStanica.ComboBox.SelectedItem = newStation;
            }
            else
            {
                ObdobiaList.ResetBindings();
            }

            tscbObdobie.ComboBox.SelectedItem = dir;
        }
        finally
        {
            tscbStanica.SelectedIndexChanged += tscbStanica_SelectedIndexChanged;
            tscbObdobie.SelectedIndexChanged += tscbObdobie_SelectedIndexChanged;
        }
    }

    private void ShowGlobalSettings(int startIndex = -1)
    {
        var gf = new FGlobalSettings(_gvdDirs.ToList(), startIndex, _previousSelectedGVD);
        var result = gf.ShowDialog();
        if (result == DialogResult.OK)
        {
            var dirlist = gf.Grafikony.Select(gvd => gvd.Dir).ToList();

            GlobData.GVDDirs = dirlist;
            TxtParser.WriteDirList(dirlist);
            TxtParser.WriteTrainTypes(GlobData.TrainsTypes);
            TxtParser.WriteZpozdeni(GlobData.Delays);
            TxtParser.WriteAudio(GlobData.Audios);
            TxtParser.WriteLanguages(GlobData.Languages.ToList());
            GlobData.LocalLanguages = GlobData.Languages.ToList(); //TODO prerobit

            // odstraneny jazyk nesmie ostat pri vlakoch - zapisal by sa do Foreign.txt
            foreach (var train in GlobData.Trains)
                train.Languages.RemoveAll(l => !GlobData.Languages.Contains(l));
            
            GlobData.Trains.ResetBindings();

            if (gf.RemovedGVDs.Count != 0)
                RemoveGrafikony(gf.RemovedGVDs);
        }
    }

    /// <summary>
    ///     Presunie odstranene grafikony do kosa a prisposobi im vyber stanice a obdobia.
    ///     DirList.TXT uz je zapisany bez nich.
    /// </summary>
    private void RemoveGrafikony(IReadOnlyCollection<GVDDirectory> removed)
    {
        var currentRemoved = _previousSelectedGVD is not null && removed.Contains(_previousSelectedGVD);

        foreach (var gvd in removed)
        {
            _gvdDirs.Remove(gvd);

            try
            {
                FileSystem.DeleteDirectory(gvd.Dir.FullPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception e)
            {
                Utils.ShowError(e.Message);
            }
        }

        if (currentRemoved)
        {
            // otvoreny grafikon uz neexistuje - jeho vlaky nesmu ostat v zozname a zmeny v nom sa nemaju ukladat
            DataSaved = true;
            _previousSelectedGVD = null;
            _prechod = true;
            GlobData.Trains.Clear();
            _prechod = false;

            if (InitializeDataList())
                InitializeGUI();
            return;
        }

        // otvoreny grafikon ostava - len zo zoznamov zmiznu odstranene obdobia a stanice bez grafikonu
        tscbStanica.SelectedIndexChanged -= tscbStanica_SelectedIndexChanged;
        tscbObdobie.SelectedIndexChanged -= tscbObdobie_SelectedIndexChanged;
        try
        {
            foreach (var gvd in removed)
                ObdobiaList.Remove(gvd);

            foreach (var station in Stanice.Where(s => _gvdDirs.All(d => d.GVD.ThisStation.Name != s)).ToList())
                Stanice.Remove(station);

            tscbStanica.ComboBox.SelectedItem = _previousSelectedGVD?.GVD.ThisStation.Name;
            tscbObdobie.ComboBox.SelectedItem = _previousSelectedGVD;
        }
        finally
        {
            tscbStanica.SelectedIndexChanged += tscbStanica_SelectedIndexChanged;
            tscbObdobie.SelectedIndexChanged += tscbObdobie_SelectedIndexChanged;
        }
    }

    private void ShowEditTrain(Train? train, int row, bool copy = false)
    {
        var gvdDir = (GVDDirectory)tscbObdobie.ComboBox.SelectedItem!;
        var eform = new FEditTrain(train, row, gvdDir.GVD, copy, gvdDir.Dir.FullPath);
        var result = eform.ShowDialog();
        if (result == DialogResult.OK)
        {
            if (train == null || row == GlobData.Trains.Count)
            {
                GlobData.Trains.Add(eform.ThisTrain!);
                DataSaved = false;
            }
            else
            {
                GlobData.Trains.ResetBindings();
                DataSaved = false;
            }
        }
    }

    private void ShowOpenDir()
    {
        var dialog = new FolderBrowserDialog { Description = "Vyberte priečinok s INISS.exe" };
        if (dialog.ShowDialog(this) == DialogResult.Cancel) 
            return;

        var selectedPath = dialog.SelectedPath;

        _prechod = true;
        GlobData.Trains.Clear();
        _prechod = false;

        if (GlobData.Config.DebugModeGUI != DebugMode.AppCrash)
            try
            {
                GlobData.PrepareGlobalData(selectedPath);
            }
            catch (Exception e)
            {
                Log.Exception(e);

                switch (GlobData.Config.DebugModeGUI)
                {
                    case DebugMode.OnlyMessage:
                        FError.ShowError(e.Message);
                        break;
                    case DebugMode.DetailInfo:
                        FError.ShowError(e.ToString());
                        break;
                }

                return;
            }
        else
            GlobData.PrepareGlobalData(selectedPath);

        if (InitializeDataList()) InitializeGUI();
    }

    private void ShowStartupINISSSettings() => ShowAppSettings("pStartupIniss");

    private void DoDeleteTrains()
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            foreach (DataGridViewRow row in dgvTrains.SelectedRows)
            {
                CheckAutoTrainVariants(row.Index);
                if (!GlobData.Config.AutoTableText)
                    DeleteTTexts((row.DataBoundItem as Train)!);

                GlobData.Trains.RemoveAt(row.Index);
            }

            GlobData.Trains.ResetBindings();

            DataSaved = false;
        }
    }

    private static bool InitializeDataList()
    {
        try
        {
            var stanice = new HashSet<string>();
            var obdobiaList = new List<GVDDirectory>();
            var dirsInData = TxtParser.ReadDirList();
            foreach (var dir in dirsInData)
            {
                GVDInfo gvd;
                if (GlobData.Config.DebugModeGUI == DebugMode.AppCrash)
                {
                    gvd = TxtParser.ReadInfoGVD(dir.FullPath);
                    stanice.Add(gvd.ThisStation.Name);
                    obdobiaList.Add(new GVDDirectory(dir, gvd));
                }
                else
                {
                    try
                    {
                        gvd = TxtParser.ReadInfoGVD(dir.FullPath);
                        stanice.Add(gvd.ThisStation.Name);
                        obdobiaList.Add(new GVDDirectory(dir, gvd));
                    }
                    catch (Exception e)
                    {
                        FError.ShowError(GlobData.Config.DebugModeGUI == DebugMode.DetailInfo ? e.ToString() : e.Message);
                        Log.Exception(e);
                    }
                }
            }

            Stanice.Clear();
            foreach (var st in stanice) Stanice.Add(st);

            ObdobiaList.Clear();
            foreach (var obd in obdobiaList) ObdobiaList.Add(obd);
        }
        catch (DirectoryNotFoundException)
        {
            Utils.ShowError(Resources.FMain_Priečinok_neobsahuje_všetky_potrebné_dáta);
            return false;
        }

        AppRegistry.SetUsageOfProject(GlobData.INISSDir);
        AppRegistry.SetLastProject(GlobData.INISSDir);
        return true;
    }

    private void InitializeGUI()
    {
        //vlozit stanice a obdobia do combo boxov v tool stripe
        _gvdDirs.Clear();
        _gvdDirs.AddRange(ObdobiaList);

        tscbStanica.ComboBox.SelectedItem = null;
        tscbObdobie.ComboBox.SelectedItem = null;

        //vybrat prvy item 
        tscbStanica.ComboBox.SelectedItem = Stanice.FirstOrDefault();
        tscbObdobie.ComboBox.SelectedItem = ObdobiaList.FirstOrDefault();

        //premenovat form podla aktualne otvoreneho priecinka
        Text = Application.ProductName + @" - " + GlobData.INISSDir;


        if (Stanice.Count == 0 || ObdobiaList.Count == 0)
        {
            //znefunkcnit niektore tlacidla tlacidla v toolstripe kvoli nevybratemu ziadnemu grafikonu

            tsbAddGVD.Enabled = true;
            tssbStartINISS.Enabled = true;
            tsmimStartINISS.Enabled = true;
            tsbAddTrain.Enabled = false;
            tsbCopyTrain.Enabled = false;
            tsbEditTrain.Enabled = false;
            tsbDeleteTrain.Enabled = false;
            tsbSave.Enabled = false;
            tsbStanica.Enabled = false;
            tsbGlobalSettings.Enabled = false;
            tsmiNew.Enabled = true;
            SetImportEnabled(false);
            tsmiUpravit.Enabled = false;
            tsmimAddTrain.Enabled = false;
            tsmimEditTrain.Enabled = false;
            tsmiDeleteTrain.Enabled = false;
            tsmiDuplikovat.Enabled = false;
            tsmiVlastnostiStanice.Enabled = false;
            tsmiGlobalSettings.Enabled = false;
            tsmiSave.Enabled = false;
            tsbSave.Enabled = false;
            tsmiAnalyze.Enabled = false;
            tsbAnalyze.Enabled = false;
            tscbStanica.Enabled = false;
            tscbObdobie.Enabled = false;
            ChangeEnableMenuItemsGSettings(false);
            ChangeEnableMenuItemsLSettings(false);
        }
        else
        {
            //sfunkcnit tlacidla v toolstripe

            tsbAddGVD.Enabled = true;
            tsbAddTrain.Enabled = true;
            tsbCopyTrain.Enabled = true;
            tsbEditTrain.Enabled = true;
            tsbDeleteTrain.Enabled = true;
            tsmiSave.Enabled = true;
            tsbSave.Enabled = true;
            tsmiAnalyze.Enabled = true;
            tsbAnalyze.Enabled = true;
            tsbStanica.Enabled = true;
            tscbStanica.Enabled = true;
            tscbObdobie.Enabled = true;
            tsbGlobalSettings.Enabled = true;
            tsmiNew.Enabled = true;
            SetImportEnabled(true);
            tsmiUpravit.Enabled = true;
            tsmiUpravit.Enabled = true;
            tsmimAddTrain.Enabled = true;
            tsmimEditTrain.Enabled = true;
            tsmiDeleteTrain.Enabled = true;
            tsmiDuplikovat.Enabled = true;
            tsmiVlastnostiStanice.Enabled = true;
            tsmiGlobalSettings.Enabled = true;
            tssbStartINISS.Enabled = true;
            tsmimStartINISS.Enabled = true;
            ChangeEnableMenuItemsGSettings(true);
            ChangeEnableMenuItemsLSettings(true);
        }

        DataSaved = true;

        // programy z predtym otvorenej instalacie - nechat len polozky pred oddelovacom
        RemoveItemsAfter(tssbStartINISS.DropDownItems, toolStripSeparator8);
        RemoveItemsAfter(tsmiRun.DropDownItems, toolStripSeparator14);

        foreach (var file in GlobData.INISSExeFiles)
        {
            ToolStripItem item1 = new ToolStripMenuItem(file);
            item1.Click += InissStartItemOnClick;
            tssbStartINISS.DropDownItems.Add(item1);

            ToolStripItem item2 = new ToolStripMenuItem(file);
            item2.Click += InissStartItemOnClick;
            tsmiRun.DropDownItems.Add(item2);
        }

        if (GlobData.INISSExeFiles.Count > 0) tsmimStartINISS.Enabled = true;

        foreach (ToolStripItem item in tssbStartINISS.DropDownItems) item.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;

        foreach (ToolStripItem item in tsmiRun.DropDownItems) item.ForeColor = GlobData.UsingStyle.ControlsColorScheme.Button.ForeColor;

        //otvoreny projekt sa presunul na zaciatok zoznamu poslednych projektov
        SetRecentProjects();
    }

    private static void RemoveItemsAfter(ToolStripItemCollection items, ToolStripItem separator)
    {
        var index = items.IndexOf(separator);
        while (items.Count > index + 1)
        {
            var item = items[items.Count - 1];
            items.Remove(item);
            item.Dispose();
        }
    }

    private void InissStartItemOnClick(object? sender, EventArgs e)
    {
        if (sender is ToolStripItem tsmi)
            StartINISS(Utils.CombinePath(GlobData.INISSDir, tsmi.Text!)!, tsmiRun);
    }

    /// <summary>
    ///     Ci bezi INISS spusteny z GVDEditora.
    /// </summary>
    private bool IsINISSRunning
    {
        get
        {
            try
            {
                return _actualINISSProcess is { HasExited: false };
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }

    /// <summary>
    ///     Spusti program <paramref name="path" /> (null = naposledy spusteny, inak rozbali ponuku
    ///     <paramref name="dropDown" />). Ak INISS uz bezi, ponukne jeho nutene ukoncenie.
    /// </summary>
    private void StartINISS(string? path, ToolStripDropDownItem dropDown)
    {
        if (IsINISSRunning)
        {
            if (Utils.ShowQuestion(Resources.FMain_InissStartItemOnClick) == DialogResult.Yes)
                KillINISS();
            return;
        }

        path ??= _lastINISSStart;
        if (path == null)
        {
            dropDown.ShowDropDown();
            return;
        }

        if (ConfirmSaveBeforeINISS() && ExecuteINISS(path))
            _lastINISSStart = path;
    }

    /// <summary>
    ///     INISS cita data grafikonu pri starte - neulozene zmeny by v nom chybali.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel spustenie zrusil alebo sa grafikon nepodarilo ulozit.</returns>
    private bool ConfirmSaveBeforeINISS()
    {
        if (DataSaved || string.IsNullOrEmpty(GlobData.INISSDir))
            return true;

        return Utils.ShowQuestion(Resources.FMain_Ulozit_pred_spustenim_INISS, MessageBoxButtons.YesNoCancel) switch
        {
            DialogResult.Yes => DoSave(),
            DialogResult.No => true,
            _ => false
        };
    }

    /// <summary>
    ///     Naplní menu naposledy otvorenými projektmi zoradenými od naposledy otvoreného.
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

    private void RecentDirsClick(object? sender, EventArgs e)
    {
        var menuItem = (ToolStripMenuItem)sender!;
        OpenRecentProject(menuItem.Text!);
    }

    private void OpenRecentProject(string fullPath)
    {
        _prechod = true;
        GlobData.Trains.Clear();
        _prechod = false;

        if (GlobData.Config.DebugModeGUI != DebugMode.AppCrash)
            try
            {
                GlobData.PrepareGlobalData(fullPath);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);

                switch (GlobData.Config.DebugModeGUI)
                {
                    case DebugMode.OnlyMessage:
                        FError.ShowError(exception.Message);
                        break;
                    case DebugMode.DetailInfo:
                        FError.ShowError(exception.ToString());
                        break;
                }

                return;
            }
        else
            GlobData.PrepareGlobalData(fullPath);

        if (InitializeDataList()) 
            InitializeGUI();
    }

    private void tsbInformation_Click(object sender, EventArgs e)
        => ShowInfoApp();

    private void tscbStanica_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tscbStanica.ComboBox.SelectedItem != null)
        {
            var dirs = new List<GVDDirectory>();
            var stanica = tscbStanica.ComboBox.SelectedItem.ToString();
            foreach (var gvdDir in _gvdDirs)
                if (stanica == gvdDir.GVD.ThisStation.Name)
                    dirs.Add(gvdDir);

            ObdobiaList.Clear();

            foreach (var dir in dirs) ObdobiaList.Add(dir);

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

    private void tscbObdobie_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_removingGVD)
            return;

        var dir = (GVDDirectory?)tscbObdobie.ComboBox.SelectedItem;

        if (dir == null) return;

        if (!string.IsNullOrEmpty(GlobData.INISSDir) && !DataSaved)
        {
            var result = Utils.ShowQuestion(Resources.FMain_Save_Changes, MessageBoxButtons.YesNoCancel);
            switch (result)
            {
                case DialogResult.Yes when DoSave():
                    break;
                case DialogResult.No:
                    DataSaved = true;
                    break;
                default:
                    // zrusene alebo neulozene - ostava otvoreny povodny grafikon, vyber sa k nemu musi vratit
                    RestoreSelection(_previousSelectedGVD);
                    return;
            }
        }

        _previousSelectedGVD = dir;

        //ak sa jedna o novy grafikon
        if (Equals(dir, _newDir))
        {
            GlobData.Tracks = new ExBindingList<Track> { Track.None };
            GlobData.Platforms = new ExBindingList<Platform> { Platform.None };

            GlobData.Operators = new ExBindingList<Operator> { Operator.None };

            GlobData.TabTabs = new ExBindingList<TableTabTab>();
            GlobData.TableCatalogs = new ExBindingList<TableCatalog>();
            GlobData.TablePhysicals = new ExBindingList<TablePhysical>();
            GlobData.TableLogicals = new ExBindingList<TableLogical>();
            GlobData.TableTexts = new ExBindingList<TableText>();
            GlobData.TableFonts = new ExBindingList<TableFont>();
            GlobData.ModeTabsSections = new Dictionary<string, Dictionary<string, string>>();

            GlobData.CustomStations = new ExBindingList<Station>();

            GlobData.Radenia = new List<Radenie>();

            GlobData.ReportTypes = ReportType.GetDefaultValuesSK();
            GlobData.ReportVariants = ReportVariant.GetDefaultValues();

            _prechod = true;
            GlobData.Trains.Clear();
            _prechod = false;

            TxtParser.WriteTrains(dir.Dir.FullPath, GlobData.Trains.ToList(), dir.GVD, GlobData.ReportVariants);

            TxtParser.WriteTables(dir.Dir.FullPath, GlobData.TabTabs, GlobData.TableCatalogs, GlobData.TablePhysicals, GlobData.TableLogicals);
            TxtParser.WriteTTexts(dir.Dir.FullPath, GlobData.TableTexts);

            TxtParser.WriteTracks(dir.Dir.FullPath, GlobData.Tracks);

            TxtParser.WriteInfoGVD(dir.Dir.FullPath, dir.GVD);

            TxtParser.WriteOperators(dir.Dir.FullPath, GlobData.Operators);

            TxtParser.WriteModeTabs(dir.Dir.FullPath, GlobData.TableFonts, GlobData.TableFontDir);

            TxtParser.WriteStateDgm(dir.Dir.FullPath, _newDirTemplate);

            TxtParser.WriteLocalCategori(dir.Dir.FullPath, GlobData.ReportVariants, GlobData.ReportTypes, GlobData.Languages);

            TxtParser.WriteRazeniDefault(dir.Dir.FullPath);
            TxtParser.WriteRazeni1Default(dir.Dir.FullPath);

            _newDir = null;
            return;
        }

        _prechod = true;
        dgvTrains.DataSource = null;
        GlobData.Trains.Clear();
        _prechod = false;

        //starsi zapis - viac grafikonov v jednom priecinku; po rozdeleni sa zoznam obdobi nacita znova a vyberie prvy novy
        var blocks = AnalyzeBlocks(dir);
        if (blocks.Count > 0 && MigrateBlocks(dir, blocks))
            return;

        _error = false;
        _waitForm = new FWait();
        _waitForm.Show(this);
        if (!backgroundWorker1.IsBusy)
            backgroundWorker1.RunWorkerAsync(new PathAndGVD { Path = dir.Dir.FullPath, Gvd = dir.GVD, BlocksDeclined = blocks.Count > 0 });
    }

    /// <summary>
    ///     Vrati vyber stanice a obdobia na grafikon <paramref name="dir" /> bez jeho opatovneho nacitania.
    /// </summary>
    private void RestoreSelection(GVDDirectory? dir)
    {
        if (dir == null) return;

        tscbStanica.SelectedIndexChanged -= tscbStanica_SelectedIndexChanged;
        tscbObdobie.SelectedIndexChanged -= tscbObdobie_SelectedIndexChanged;
        try
        {
            var station = dir.GVD.ThisStation.Name;
            if (!ObdobiaList.Contains(dir))
            {
                ObdobiaList.Clear();
                foreach (var gvdDir in GVDSelectionLists.PeriodsOf(_gvdDirs, station)) ObdobiaList.Add(gvdDir);
            }

            tscbStanica.ComboBox.SelectedItem = station;
            tscbObdobie.ComboBox.SelectedItem = dir;
        }
        finally
        {
            tscbStanica.SelectedIndexChanged += tscbStanica_SelectedIndexChanged;
            tscbObdobie.SelectedIndexChanged += tscbObdobie_SelectedIndexChanged;
        }
    }

    private static List<GvdBlock> AnalyzeBlocks(GVDDirectory dir)
    {
        try
        {
            //grafikon priamo v DATA (bez DirList.TXT) sa presuva do vlastneho priecinka vzdy, aj ked ma jediny blok
            return BlockMigrator.Analyze(dir.Dir.FullPath, dir.GVD, dir.Dir.DirName, dir.Dir.IsDataRoot);
        }
        catch (Exception e)
        {
            //chybny Export3A ohlasi az nacitanie grafikonu
            Log.Exception(e);
            return new List<GvdBlock>();
        }
    }

    /// <summary>
    ///     Ponukne rozdelenie priecinka s blokmi <c>/stanica</c> do samostatnych priecinkov.
    /// </summary>
    /// <returns><see langword="true" />, ak sa grafikon rozdelil a zoznam obdobi bol nacitany znova.</returns>
    private bool MigrateBlocks(GVDDirectory dir, List<GvdBlock> blocks)
    {
        var question = dir.Dir.IsDataRoot
            ? string.Format(Resources.FMain_Grafikon_v_koreni_otazka, dir.Dir.FullPath, blocks.Count)
            : string.Format(Resources.FMain_Grafikon_obsahuje_bloky_otazka, dir.Dir.FullPath, blocks.Count);
        if (Utils.ShowQuestion(question) != DialogResult.Yes)
            return false;

        using var form = new FBlockMigration(dir.Dir.FullPath, blocks, dir.Dir.IsDataRoot);
        if (form.ShowDialog(this) != DialogResult.OK)
            return false;

        List<DirList> newDirs;
        try
        {
            newDirs = BlockMigrator.Migrate(dir.Dir.FullPath, dir.Dir, dir.GVD, blocks);
        }
        catch (Exception e)
        {
            Log.Exception(e);
            Utils.ShowError(string.Format(Resources.FMain_Rozdelenie_zlyhalo, e.Message));
            return false;
        }

        Utils.ShowInfo(string.Format(Resources.FMain_Grafikon_rozdeleny, string.Join(", ", newDirs.Select(d => d.DirName)), dir.Dir.FullPath));

        GlobData.GVDDirs = TxtParser.ReadDirList();
        DataSaved = true;
        _previousSelectedGVD = null;

        if (!InitializeDataList())
            return true;

        _gvdDirs.Clear();
        _gvdDirs.AddRange(ObdobiaList);

        var first = _gvdDirs.FirstOrDefault(o => o.Dir.DirName.Equals(newDirs[0].DirName, StringComparison.OrdinalIgnoreCase));

        //zmena stanice by sama vybrala prve obdobie a spustila nacitanie - vybrat treba az prvy novy priecinok
        _removingGVD = true;
        tscbStanica.ComboBox.SelectedItem = null;
        tscbStanica.ComboBox.SelectedItem = first?.GVD.ThisStation.Name ?? Stanice.FirstOrDefault();
        tscbObdobie.ComboBox.SelectedItem = null;
        _removingGVD = false;

        tscbObdobie.ComboBox.SelectedItem = first ?? ObdobiaList.FirstOrDefault();
        return true;
    }

    private static void InitDruhyReportov()
    {
        GlobData.ReportTypesV.Clear();
        GlobData.ReportTypesP.Clear();
        GlobData.ReportTypesK.Clear();

        foreach (var reportType in GlobData.ReportTypes)
        {
            if (reportType.BaseTrain) GlobData.ReportTypesV.Add(reportType);
            if (reportType.PassThrough) GlobData.ReportTypesP.Add(reportType);
            if (reportType.TerminateTrain) GlobData.ReportTypesK.Add(reportType);
        }
    }

    private void dgvTrains_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex != -1 && dgvTrains.Columns[e.ColumnIndex].Name == "Ostatne")
            if (dgvTrains.CurrentRow != null && e.RowIndex != -1 && GlobData.Trains[e.RowIndex] != new Train())
                ShowEditTrain(GlobData.Trains[e.RowIndex], e.RowIndex);

        if (e.RowIndex != -1)
        {
            tsslSelTrainName.Visible = true;
            tsslSelTrainVariants.Visible = true;
            tsslSelTrainName.Text = $@"{GlobData.Trains[e.RowIndex].Type} {GlobData.Trains[e.RowIndex].Number}";
            tsslSelTrainVariants.Text = CountSelTrainVariants(GlobData.Trains[e.RowIndex]).ToString();
        }
    }

    private static int CountSelTrainVariants(Train train)
    {
        var count = 0;
        foreach (var t in GlobData.Trains)
            if (t.NumberVariant.Number == train.Number)
                count++;
        return count;
    }

    private void dgvTrains_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        if (dgvTrains.Columns[e.ColumnIndex].Name == @"Kolaj" && e.RowIndex != -1 && e.RowIndex < GlobData.Trains.Count)
            GlobData.Trains[e.RowIndex].Track = GlobData.Tracks[0];
        else if (dgvTrains.Columns[e.ColumnIndex].Name == @"Dopravca" && e.RowIndex != -1 &&
                 e.RowIndex < GlobData.Trains.Count)
            GlobData.Trains[e.RowIndex].Operator = GlobData.Operators[0];
        else
            Utils.ShowError(Resources.FMain_dgvTrains_DataError_Tabuľka_obsahuje_nesprávny_údaj + e.Exception!.Message);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!string.IsNullOrEmpty(GlobData.INISSDir) && !DataSaved)
        {
            var result = Utils.ShowQuestion(Resources.FMain_Save_Changes, MessageBoxButtons.YesNoCancel);
            switch (result)
            {
                case DialogResult.Yes:
                    // pri neuspesnom ulozeni okno nezatvorit, zmeny by sa stratili
                    e.Cancel = !DoSave();
                    break;
                case DialogResult.No:
                    e.Cancel = false;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }
    }

    private void tsbSave_Click(object sender, EventArgs e) => DoSave();

    private void tsbOpen_Click(object sender, EventArgs e) => ShowOpenDir();

    private void tsmiImportData_Click(object sender, EventArgs e) => ShowImportData();

    private void tsmiImportGVD_Click(object sender, EventArgs e) => ShowImportGVD();

    private void tsmiImportELIS_Click(object sender, EventArgs e) => ShowImportELIS();

    private void tsmimImportELIS_Click(object sender, EventArgs e) => ShowImportELIS();

    private void tsmimImportData_Click(object sender, EventArgs e) => ShowImportData();

    private void tsmimImportGVD_Click(object sender, EventArgs e) => ShowImportGVD();

    private void tsbAnalyze_Click(object sender, EventArgs e) => ShowAnalyzeGVD();

    private void tsbAddTrain_Click(object sender, EventArgs e) => ShowEditTrain(null, GlobData.Trains.Count);

    private void tsbCopyTrain_Click(object sender, EventArgs e)
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(GlobData.Trains[index], GlobData.Trains.Count, true);
        }
    }

    private void tsbEditTrain_Click(object sender, EventArgs e)
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(GlobData.Trains[index], index);
        }
    }

    private void tsbDeleteTrain_Click(object sender, EventArgs e) => DoDeleteTrains();

    private void tsbLocalSettings_Click(object sender, EventArgs e) => ShowLocalSettings();

    private void tsbAddGVD_Click(object sender, EventArgs e) => ShowNewGVD();

    private void dgvTrains_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Delete) DoDeleteTrains();
    }

    private void dgvTrains_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (dgvTrains.Columns[e.ColumnIndex].Name == @"cisloDataGridViewTextBoxColumn" &&
            (string)e.FormattedValue! == "") e.Cancel = true;

        if (dgvTrains.Columns[e.ColumnIndex].Name == @"odchodDataGridViewTextBoxColumn" &&
            (string)e.FormattedValue! == "" &&
            dgvTrains.Rows[e.RowIndex].Cells[@"prichodDataGridViewTextBoxColumn"].Value == null)
            e.Cancel = true;
        else if (dgvTrains.Columns[e.ColumnIndex].Name == @"prichodDataGridViewTextBoxColumn" &&
                 (string)e.FormattedValue! == "" &&
                 dgvTrains.Rows[e.RowIndex].Cells[@"odchodDataGridViewTextBoxColumn"].Value == null) e.Cancel = true;

        if (dgvTrains.Columns[e.ColumnIndex].Name == @"DatumoveObmedzenieText")
        {
            var value = e.FormattedValue as string;
            var thistrain = GlobData.Trains[e.RowIndex];
            var dateRemThis = new DateLimit(thistrain.ZaciatokPlatnosti, thistrain.KoniecPlatnosti,
                insertMarks: false);
            try
            {
                dateRemThis.TextToBitArray(value!);
            }
            catch (Exception exception)
            {
                Utils.ShowError(exception.Message);
                e.Cancel = true;
                return;
            }

            var i = 0;
            foreach (var train in GlobData.Trains)
            {
                if (Train.IsSameVariant(train, thistrain) && i != e.RowIndex && 
                    train.ZaciatokPlatnosti == thistrain.ZaciatokPlatnosti && 
                    train.KoniecPlatnosti == thistrain.KoniecPlatnosti && 
                    dateRemThis.Overlap(thistrain.DateLimitText, train.DateLimitText))
                {
                    var obmand = dateRemThis.TextAnd(train.DateLimitText, thistrain.DateLimitText);
                    var result = Utils.ShowQuestion(string.Format(Resources.FEditTrain_DateRem_zasahuje_do_ineho_vlaku, train.Type,
                        train.Number, train.Name, obmand));
                    if (result == DialogResult.Yes)
                    {
                        var result2 = FDateLimitEdit.SetDateLimit(this, thistrain.ZaciatokPlatnosti, thistrain.KoniecPlatnosti, train,
                            true, train.DateLimitText);
                        if (result2 == DialogResult.OK) 
                            train.DateLimitText = FDateLimitEdit.Result;
                    }
                }

                i++;
            }
        }
    }

    private void tsbGlobalSettings_Click(object sender, EventArgs e) => ShowGlobalSettings();

    private void tsmiNew_Click(object sender, EventArgs e) => ShowNewGVD();

    private void tsmiOpen_Click(object sender, EventArgs e) => ShowOpenDir();

    private void tsmiSave_Click(object sender, EventArgs e) => DoSave();

    private void tsmiAnalyze_Click(object sender, EventArgs e) => ShowAnalyzeGVD();

    private void tsmimAddTrain_Click(object sender, EventArgs e) => ShowEditTrain(null, GlobData.Trains.Count);

    private void tsmimEditTrain_Click(object sender, EventArgs e)
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(GlobData.Trains[index], index);
        }
    }

    private void tsmiDeleteTrain_Click(object sender, EventArgs e) => DoDeleteTrains();

    private void tsmiDuplikovat_Click(object sender, EventArgs e)
    {
        if (dgvTrains.SelectedRows.Count > 0)
        {
            var index = dgvTrains.SelectedRows[0].Index;
            ShowEditTrain(GlobData.Trains[index], GlobData.Trains.Count, true);
        }
    }

    private void tsmiLocalSettings_Click(object sender, EventArgs e) => ShowLocalSettings();

    private void tsmiGlobalSettings_Click(object sender, EventArgs e) => ShowGlobalSettings();

    private void tsmiAppSettings_Click(object sender, EventArgs e) => ShowAppSettings();

    private void tsmiInformation_Click(object sender, EventArgs e) => ShowInfoApp();

    private void tsmiChangelog_Click(object sender, EventArgs e) => Utils.OpenShell(LinkConsts.LINK_NEWS);

    private void dgvTrains_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        if (GlobData.Trains.Count != 0)
        {
            for (var i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                GlobData.Trains[i].ID = i + 1;

                if (GlobData.Trains[i].Routing == null)
                {
                    var vlak = GlobData.Trains[i];
                    throw new ArgumentNullException(
                        $"Vlak {vlak.Type} {vlak.NumberVariant} {vlak.Name} nemá definované smerovanie.");
                }

                if (GlobData.Trains[i].Routing == Routing.Prechadzajuci)
                {
                    dgvTrains.Rows[i].Cells[@"odchodDataGridViewTextBoxColumn"].ReadOnly = false;
                    dgvTrains.Rows[i].Cells[@"prichodDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else if (GlobData.Trains[i].Routing == Routing.Vychadzajuci)
                {
                    dgvTrains.Rows[i].Cells[@"odchodDataGridViewTextBoxColumn"].ReadOnly = false;
                    dgvTrains.Rows[i].Cells[@"prichodDataGridViewTextBoxColumn"].ReadOnly = true;
                }
                else
                {
                    dgvTrains.Rows[i].Cells[@"odchodDataGridViewTextBoxColumn"].ReadOnly = true;
                    dgvTrains.Rows[i].Cells[@"prichodDataGridViewTextBoxColumn"].ReadOnly = false;
                }
            }

            tsslTrainCountWithVariants.Text = dgvTrains.Rows.Count.ToString();
            tsslTrainCount.Text = $@"({CountTrainVariants()})";
        }
    }

    private void dgvTrains_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
        for (var i = 1; i <= GlobData.Trains.Count; i++) GlobData.Trains[i - 1].ID = i;

        if (!_prechod) DataSaved = false;

        tsslTrainCountWithVariants.Text = dgvTrains.Rows.Count.ToString();
        tsslTrainCount.Text = $@"({CountTrainVariants()})";

        if (GlobData.Trains.Count == 0)
        {
            tsslSelTrainName.Visible = false;
            tsslSelTrainVariants.Visible = false;
        }
    }

    private static int CountTrainVariants()
    {
        var trains = new HashSet<(string num, TrainType type, string name)>();
        foreach (var t in GlobData.Trains) 
            trains.Add((t.Number, t.Type, t.Name));

        return trains.Count;
    }

    private void dgvTrains_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex != -1) DataSaved = false;
    }

    private void tsbAppSettings_Click(object sender, EventArgs e) => ShowAppSettings();

    private void tsmiStartupSettings_Click(object sender, EventArgs e) => ShowStartupINISSSettings();

    private void tsmimStartupSettings_Click(object sender, EventArgs e) => ShowStartupINISSSettings();

    private void dgvTrains_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        void SetFromScheme(ColorSetting sett)
        {
            e.CellStyle.ForeColor = sett.ForeColor != SystemColors.ControlText ? sett.ForeColor : Color.White;
            if (sett.BackColor != Color.Transparent) 
                e.CellStyle.BackColor = sett.BackColor;
            e.CellStyle.Font = sett.Bold
                ? new Font(GlobData.UsingStyle.TrainTypeColumnScheme.Font, FontStyle.Bold)
                : GlobData.UsingStyle.TrainTypeColumnScheme.Font;
        }

        var typColumn = dgvTrains.Columns["typDataGridViewTextBoxColumn"];
        if (typColumn != null && e.ColumnIndex == typColumn.Index)
            if (e.RowIndex < GlobData.Trains.Count)
            {
                var type = GlobData.Trains[e.RowIndex].Type;
                if (type.IsCustom)
                {
                    var stype = type.CategoryTrain.ToUpper();
                    if (stype.StartsWith("X"))
                        SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.X);
                    else if (stype.StartsWith("R"))
                        SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.R);
                    else if (stype.StartsWith("SL"))
                        SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.Sl);
                    else if (stype.StartsWith("OS")) 
                        SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.Os);
                }
                else
                {
                    switch (type.CategoryTrain)
                    {
                        case "R":
                        case "REX":
                        case "RR":
                            SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.R);
                            break;
                        case "IC":
                        case "EC":
                        case "EN":
                        case "SC":
                            SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.X);
                            break;
                        default:
                            SetFromScheme(GlobData.UsingStyle.TrainTypeColumnScheme.Os);
                            break;
                    }
                }
            }
    }

    private static void GenerateTableTextWhileSaving(GVDDirectory dir) =>
        TableTextGenerating.RegenerateAll(GlobData.TableTexts, GlobData.Trains, dir.GVD.ThisStation);

    private void SetColumns()
    {
        static void SetCol(DataGridViewColumn column, DesktopColumn format)
        {
            column.Visible = format.Visible;
            column.MinimumWidth = format.MinWidth;
            column.DisplayIndex = format.Order;
        }

        SetCol(cisloDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Number);
        SetCol(typDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Type);
        SetCol(nameDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Name);
        SetCol(LinkaPrichod, GlobData.Config.DesktopCols.LinkaPrichod);
        SetCol(LinkaOdchod, GlobData.Config.DesktopCols.LinkaOdchod);
        SetCol(smerovanieDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Routing);
        SetCol(prichodDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Prichod);
        SetCol(odchodDataGridViewTextBoxColumn, GlobData.Config.DesktopCols.Odchod);
        SetCol(dgvcVychodziaStanica, GlobData.Config.DesktopCols.VychodziaStanica);
        SetCol(dgvcKonecnaStanica, GlobData.Config.DesktopCols.KonecnaStanica);
        SetCol(DatumoveObmedzenieText, GlobData.Config.DesktopCols.DateLimit);
        SetCol(Kolaj, GlobData.Config.DesktopCols.Track);
        SetCol(Dopravca, GlobData.Config.DesktopCols.Operator);
        SetCol(Ostatne, GlobData.Config.DesktopCols.OtherBtn);

        dgvTrains.Columns[dgvTrains.Columns.Count - 1].AutoSizeMode = GlobData.Config.FitLastColumn
            ? DataGridViewAutoSizeColumnMode.Fill
            : DataGridViewAutoSizeColumnMode.None;
    }

    private void SetColumnsAutoWidth()
    {
        for (var i = 0; i < dgvTrains.Columns.Count - 1; i++)
        {
            dgvTrains.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            var widthCol = dgvTrains.Columns[i].Width;
            dgvTrains.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvTrains.Columns[i].Width = widthCol;
        }
    }

    private void SetShortcuts()
    {
        var sc = GlobData.Config.Shortcuts;
        tsmiNew.ShortcutKeys = (Keys)sc.New.Shortcut.Value;
        tsmiOpen.ShortcutKeys = (Keys)sc.Open.Shortcut.Value;
        tsmiImportData.ShortcutKeys = (Keys)sc.ImportData.Shortcut.Value;
        tsmiImportGVD.ShortcutKeys = (Keys)sc.ImportGvd.Shortcut.Value;
        tsmiSave.ShortcutKeys = (Keys)sc.Save.Shortcut.Value;
        tsmiAnalyze.ShortcutKeys = (Keys)sc.Analyze.Shortcut.Value;

        tsmimAddTrain.ShortcutKeys = (Keys)sc.AddTrain.Shortcut.Value;
        tsmimEditTrain.ShortcutKeys = (Keys)sc.EditTrain.Shortcut.Value;
        tsmiDeleteTrain.ShortcutKeys = (Keys)sc.DeleteTrains.Shortcut.Value;
        tsmiDuplikovat.ShortcutKeys = (Keys)sc.DuplicateTrain.Shortcut.Value;

        tsmiVlastnostiStanice.ShortcutKeys = (Keys)sc.LocalSettings.Shortcut.Value;
        tsmiGlobalSettings.ShortcutKeys = (Keys)sc.GlobalSettings.Shortcut.Value;
        tsmiAppSettings.ShortcutKeys = (Keys)sc.AppSettings.Shortcut.Value;

        tsmiInformation.ShortcutKeys = (Keys)sc.InfoApp.Shortcut.Value;
        tsmiChangelog.ShortcutKeys = (Keys) sc.UpdateNotes.Shortcut.Value;

        tsmimStartINISS.ShortcutKeys = (Keys)sc.RunINISS.Shortcut.Value;
        tsmimShutdownINISS.ShortcutKeys = (Keys)sc.ShutdownINISS.Shortcut.Value;
        tsmimKillINISS.ShortcutKeys = (Keys)sc.KillINISS.Shortcut.Value;
        tsmimRestartINISS.ShortcutKeys = (Keys)sc.RestartINISS.Shortcut.Value;

        tsmiGrafikon.ShortcutKeys = (Keys)sc.LSGvd.Shortcut.Value;
        tsmiStanice.ShortcutKeys = (Keys)sc.LSStations.Shortcut.Value;
        tsmiDopravcovia.ShortcutKeys = (Keys)sc.LSOperators.Shortcut.Value;
        tsmiPlatforms.ShortcutKeys = (Keys)sc.LSPlatforms.Shortcut.Value;
        tsmiKolaje.ShortcutKeys = (Keys)sc.LSTracks.Shortcut.Value;
        tsmiTPhysical.ShortcutKeys = (Keys)sc.LSPhysicalTables.Shortcut.Value;
        tsmiTLogical.ShortcutKeys = (Keys)sc.LSLogicalsTables.Shortcut.Value;
        tsmiTCatalog.ShortcutKeys = (Keys)sc.LSCatalogTables.Shortcut.Value;
        tsmiTabTab.ShortcutKeys = (Keys)sc.LSTabTab.Shortcut.Value;
        tsmiTTexts.ShortcutKeys = (Keys)sc.LSTTexts.Shortcut.Value;
        tsmiTFonts.ShortcutKeys = (Keys)sc.LSTFonts.Shortcut.Value;
        tsmiTabTabEditor.ShortcutKeys = (Keys)sc.LSTabTabEditor.Shortcut.Value;

        tsmiGrafikony.ShortcutKeys = (Keys)sc.GSGvds.Shortcut.Value;
        tsmiLanguages.ShortcutKeys = (Keys)sc.GSLanguages.Shortcut.Value;
        tsmiMeskania.ShortcutKeys = (Keys)sc.GSDelays.Shortcut.Value;
        tsmiTypyVlakov.ShortcutKeys = (Keys)sc.GSTrainTypes.Shortcut.Value;
        tsmiAudio.ShortcutKeys = (Keys)sc.GSAudio.Shortcut.Value;

        tsmiDatObm.ShortcutKeys = (Keys)sc.DateLimit.Shortcut.Value;
    }

    private static void CheckAutoTrainVariants(int index)
    {
        if (!GlobData.Config.AutoVariant) return;

        var id = 0;
        var seltrains = new List<Train>();
        var dtrain = GlobData.Trains[index];

        foreach (var train in GlobData.Trains)
        {
            if (train.Number == dtrain.Number && string.Equals(train.Name, dtrain.Name) &&
                Equals(train.Type, dtrain.Type) && index != id) seltrains.Add(train);

            id++;
        }

        if (seltrains.Count == 1)
            seltrains[0].Variant = -1;
        else if (seltrains.Count != 0)
            for (var i = 0; i < seltrains.Count; i++)
                seltrains[i].Variant = i + 1;
    }

    private void bWorkerELIS_DoWork(object sender, DoWorkEventArgs e)
    {
        var data = (SendData)e.Argument!;

        if (GlobData.Config.DebugModeGUI != DebugMode.AppCrash)
            try
            {
                e.Result = CallELISBridge(data);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);

                if (GlobData.Config.DebugModeGUI == DebugMode.OnlyMessage)
                    Utils.ShowError(exception.Message);
                if (GlobData.Config.DebugModeGUI == DebugMode.DetailInfo)
                    Utils.ShowError(exception.ToString());

                _error = true;
                return;
            }
        else
            e.Result = CallELISBridge(data);

        _error = false;
    }

    /// <summary>
    ///     Prva faza importu - nacita data z ELIS a zisti, ktore stanice sa nepodarilo priradit.
    ///     Bezi na pozadi, takze sa tu nesmie nic pytat pouzivatela; priradenie stanic
    ///     dokoncuje az <see cref="bWorkerELIS_RunWorkerCompleted" />.
    /// </summary>
    private static ElisImport CallELISBridge(SendData data)
    {
        var client = new ELISBridgeClient(GlobData.TrainsTypes.ToList(), GlobData.Operators.ToList(), data.GVDInfo, data.Track)
        {
            AppDirectory = data.AppDirectory,
            RegistrationNumber = data.RegistrationNumber,
            DefinedTrains = data.DefTrains,
            OmitPassingTrains = data.OmitPassingTrains,
            ReorderTrains = data.ReorderTrains,
            StationMap = TxtParser.ReadElisStationMap(data.GVDPath)
        };

        var elisData = client.LoadData();
        return new ElisImport
        {
            Client = client,
            Data = elisData,
            Unresolved = client.FindUnresolvedStations(elisData),
            GVDPath = data.GVDPath,
            GVDInfo = data.GVDInfo,
            ReplaceTrains = data.ReplaceTrains
        };
    }

    /// <summary>
    ///     Medzivysledok importu z ELIS medzi vlaknom na pozadi a dokoncenim v GUI.
    /// </summary>
    internal sealed class ElisImport
    {
        public ELISBridgeClient Client { get; set; } = null!;
        public ElisResult Data { get; set; } = null!;
        public List<string> Unresolved { get; set; } = null!;
        public string GVDPath { get; set; } = null!;
        public GVDInfo GVDInfo { get; set; } = null!;

        /// <summary>
        ///     Ci sa maju povodne vlaky pred pridanim naimportovanych odstranit. Nesie sa
        ///     spolu s vysledkom, nie v poli formulara - stav okolo asynchronneho behu
        ///     sa lahko stratí a chyba by sa prejavila az tichym nenahradenim vlakov.
        /// </summary>
        public bool ReplaceTrains { get; set; }
    }

    private void bWorkerELIS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        _waitForm!.Close();
        Cursor.Current = Cursors.AppStarting;

        if (!_error)
        {
            var import = (ElisImport)e.Result!;

            //stanice, ktore ELIS pomenuva inak, doriesi pouzivatel - a volba sa zapamata
            if (import.Unresolved.Count != 0 && !ResolveStations(import))
                return;

            List<Train> imported;
            try
            {
                imported = import.Client.Convert(import.Data);
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                Utils.ShowError(exception.Message);
                return;
            }

            var removed = 0;
            if (import.ReplaceTrains)
            {
                removed = GlobData.Trains.Count;
                RemoveAllTrains();
            }

            foreach (var train in imported) GlobData.Trains.Add(train);
            GlobData.Trains.ResetBindings();

            DataSaved = false;

            Utils.ShowInfo(string.Format(Resources.FMain_Import_z_ELIS_dokončený, removed, imported.Count));
        }
    }

    /// <summary>
    ///     Necha pouzivatela priradit stanice, ktore sa nepodarilo rozpoznat automaticky,
    ///     a priradenie ulozi do grafikonu.
    /// </summary>
    /// <returns><see langword="false" />, ak pouzivatel import zrusil.</returns>
    private bool ResolveStations(ElisImport import)
    {
        var dialog = new FELISStations(import.Unresolved);
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return false;

        foreach (var pair in dialog.Result)
            import.Client.StationMap[pair.Key] = pair.Value;

        try
        {
            TxtParser.WriteElisStationMap(import.GVDPath, import.Client.StationMap, import.GVDInfo);
        }
        catch (Exception e)
        {
            //ulozenie priradenia nie je kriticke - import moze pokracovat, len sa nabuduce spyta znova
            Log.Exception(e);
        }

        return true;
    }

    /// <summary>
    ///     Odstrani vsetky vlaky grafikonu aj texty tabul, ktore sa na ne odvolavaju.
    /// </summary>
    private void RemoveAllTrains()
    {
        if (!GlobData.Config.AutoTableText)
            foreach (var train in GlobData.Trains)
                DeleteTTexts(train);

        _prechod = true;
        GlobData.Trains.Clear();
        _prechod = false;
    }

    private static void DeleteTTexts(Train vlak)
    {
        foreach (var tt in GlobData.TableTexts)
            for (var i = tt.Trains.Count - 1; i >= 0; i--)
                if (tt.Trains[i].Train == vlak)
                    tt.Trains.RemoveAt(i);
    }

    private bool ExecuteINISS(string fileName)
    {
        var process = new Process { StartInfo = { FileName = fileName, UseShellExecute = true } };
        if (GlobData.Config.StartupINISSConfig.RunAsAdmin) process.StartInfo.Verb = "runas";
        process.StartInfo.Arguments = GlobData.Config.StartupINISSConfig.CmdArgs;
        process.EnableRaisingEvents = true;
        process.Exited += ProcOnExited;
        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            // napr. zamietnute spustenie ako administrator - proces nebezi, nesmie ostat ako "beziaci"
            process.Dispose();
            Utils.ShowError(string.Format(Resources.FMain_Nepodarilo_sa_spustiť_vybraný_program, e.Message));
            return false;
        }

        _actualINISSProcess = process;
        SetINISSControlsEnabled(true);
        return true;
    }

    private void SetINISSControlsEnabled(bool running)
    {
        tsbShutdownINISS.Enabled = running;
        tsmimShutdownINISS.Enabled = running;
        tsbKillINISS.Enabled = running;
        tsmimKillINISS.Enabled = running;
        tsbRestartINISS.Enabled = running;
        tsmimRestartINISS.Enabled = running;
    }

    private void ProcOnExited(object? sender, EventArgs e)
    {
        // pri restarte uz moze bezat novy proces - ukoncenie stareho ho nesmie prestat sledovat
        if (!ReferenceEquals(sender, _actualINISSProcess))
        {
            (sender as Process)?.Dispose();
            return;
        }

        if (IsDisposed || !IsHandleCreated) return;

        BeginInvoke(() =>
        {
            if (!ReferenceEquals(sender, _actualINISSProcess)) return;

            _actualINISSProcess?.Dispose();
            _actualINISSProcess = null;
            SetINISSControlsEnabled(false);
        });
    }

    private void tssbStartINISS_ButtonClick(object sender, EventArgs e) => StartINISS(null, tssbStartINISS);

    private void tsmimStartINISS_Click(object sender, EventArgs e) => StartINISS(null, tsmiRun);

    private void KillINISS()
    {
        try
        {
            _actualINISSProcess?.Kill();
        }
        catch (Exception e)
        {
            // napr. INISS spusteny ako administrator a GVDEditor bez opravneni
            Utils.ShowError(string.Format(Resources.FMain_INISS_neda_ukoncit, e.Message));
        }
    }

    /// <summary>
    ///     Zavrie okno INISSu rovnako ako krizik - INISS sa ukonci riadne (a moze sa opytat na potvrdenie).
    /// </summary>
    private void ShutDownINISS()
    {
        try
        {
            if (IsINISSRunning)
                _actualINISSProcess!.CloseMainWindow();
        }
        catch (InvalidOperationException)
        {
        }
    }

    /// <summary>
    ///     Riadne ukonci INISS a spusti ho znova. Ak sa INISS do casoveho limitu neukonci (napr. caka na potvrdenie),
    ///     ponukne nutene ukoncenie.
    /// </summary>
    private async void RestartINISS()
    {
        var process = _actualINISSProcess;
        var path = _lastINISSStart;
        if (process == null || path == null || !ConfirmSaveBeforeINISS())
            return;

        ShutDownINISS();
        using (var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
        {
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                if (Utils.ShowQuestion(Resources.FMain_INISS_sa_neukoncil) != DialogResult.Yes)
                    return;

                KillINISS();
                try
                {
                    await process.WaitForExitAsync();
                }
                catch (InvalidOperationException)
                {
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        if (!IsINISSRunning)
            ExecuteINISS(path);
    }

    private void tsbKillINISS_Click(object sender, EventArgs e) => AskKillINISS();

    private void tsmimKillINISS_Click(object sender, EventArgs e) => AskKillINISS();

    /// <summary>
    ///     Nutene ukoncenie na priamy prikaz - s potvrdenim, predvolena skratka F10 sa lahko stlaci omylom.
    /// </summary>
    private void AskKillINISS()
    {
        if (IsINISSRunning && Utils.ShowQuestion(Resources.FMain_Vynutit_ukoncenie_INISS) == DialogResult.Yes)
            KillINISS();
    }

    private void tsbShutdownINISS_Click(object sender, EventArgs e) => ShutDownINISS();

    private void tsmimShutdownINISS_Click(object sender, EventArgs e) => ShutDownINISS();

    private void tsbRestartINISS_Click(object sender, EventArgs e) => RestartINISS();

    private void tsmimRestartINISS_Click(object sender, EventArgs e) => RestartINISS();

    //LOCAL SETTINGS
    private void tsmiGrafikon_Click(object sender, EventArgs e) => ShowLocalSettings(0);

    private void tsmiStanice_Click(object sender, EventArgs e) => ShowLocalSettings(1);

    private void tsmiDopravcovia_Click(object sender, EventArgs e) => ShowLocalSettings(2);

    private void tsmiPlatforms_Click(object sender, EventArgs e) => ShowLocalSettings(3);

    private void tsmiKolaje_Click(object sender, EventArgs e) => ShowLocalSettings(4);

    private void tsmiTPhysical_Click(object sender, EventArgs e) => ShowLocalSettings(5);

    private void tsmiTLogical_Click(object sender, EventArgs e) => ShowLocalSettings(6);

    private void tsmiTCatalog_Click(object sender, EventArgs e) => ShowLocalSettings(7);

    private void tsmiTabTab_Click(object sender, EventArgs e) => ShowLocalSettings(8);

    private void tsmiTTexts_Click(object sender, EventArgs e) => ShowLocalSettings(9);

    private void tsmiTFonts_Click(object sender, EventArgs e) => ShowLocalSettings(10);

    private void tsmiTabTabEditor_Click(object sender, EventArgs e) => ShowLocalSettings(-2);

    //GLOBAL SETTINGS
    private void tsmiGrafikony_Click(object sender, EventArgs e) => ShowGlobalSettings(0);

    private void tsmiLanguages_Click(object sender, EventArgs e) => ShowGlobalSettings(1);

    private void tsmiMeskania_Click(object sender, EventArgs e) => ShowGlobalSettings(2);

    private void tsmiTypyVlakov_Click(object sender, EventArgs e) => ShowGlobalSettings(3);

    private void tsmiAudio_Click(object sender, EventArgs e) => ShowGlobalSettings(4);

    //DATE LIMIT
    private void tsmiDatObm_Click(object sender, EventArgs e) => ShowDatObm();

    private void tsmiStateDgm_Click(object sender, EventArgs e) => ShowStateDgm();

    /// <summary>
    ///     Otvori editor stavoveho diagramu aktualneho grafikonu.
    /// </summary>
    private void ShowStateDgm()
    {
        if (tscbObdobie.ComboBox.SelectedItem is not GVDDirectory dir) return;
        using var f = new FStateDgm(dir);
        f.ShowDialog(this);
    }

    private void tsbDatObm_Click(object sender, EventArgs e) => ShowDatObm();

    private void ShowDatObm()
    {
        var gvd = (tscbObdobie.ComboBox?.SelectedItem as GVDDirectory)?.GVD;
        using var fobm = new FDatObm(gvd?.StartValidTimeTable, gvd?.EndValidTimeTable);
        fobm.ShowDialog(this);
    }

    /// <summary>
    ///     Import grafikonu je dostupny vzdy, ked je otvorena instalacia - aj bez grafikonu; import dat a z ELIS
    ///     potrebuju otvoreny grafikon.
    /// </summary>
    private void SetImportEnabled(bool grafikonOpen)
    {
        tsmiImport.Enabled = true;
        tsbImport.Enabled = true;
        tsmiImportGVD.Enabled = true;
        tsmimImportGVD.Enabled = true;
        tsmiImportData.Enabled = grafikonOpen;
        tsmimImportData.Enabled = grafikonOpen;
        tsmiImportELIS.Enabled = grafikonOpen;
        tsmimImportELIS.Enabled = grafikonOpen;
    }

    private void ChangeEnableMenuItemsLSettings(bool enabled)
    {
        tsmiGrafikon.Enabled = enabled;
        tsmiStanice.Enabled = enabled;
        tsmiDopravcovia.Enabled = enabled;
        tsmiPlatforms.Enabled = enabled;
        tsmiKolaje.Enabled = enabled;
        tsmiTPhysical.Enabled = enabled;
        tsmiTLogical.Enabled = enabled;
        tsmiTCatalog.Enabled = enabled;
        tsmiTabTab.Enabled = enabled;
        tsmiTTexts.Enabled = enabled;
        tsmiTFonts.Enabled = enabled;
        tsmiTabTabEditor.Enabled = enabled;
        tsmiStateDgm.Enabled = enabled;
    }

    private void ChangeEnableMenuItemsGSettings(bool enabled)
    {
        tsmiGrafikony.Enabled = enabled;
        tsmiLanguages.Enabled = enabled;
        tsmiMeskania.Enabled = enabled;
        tsmiTypyVlakov.Enabled = enabled;
        tsmiAudio.Enabled = enabled;
    }

    private class PathAndGVD
    {
        public string Path { get; set; } = null!;
        public GVDInfo Gvd { get; set; } = null!;

        /// <summary>Priecinok obsahuje bloky /stanica a pouzivatel ich odmietol rozdelit - grafikon sa nesmie nacitat.</summary>
        public bool BlocksDeclined { get; set; }
    }

    internal class SendData
    {
        /// <summary>
        ///     Predvolene umiestnenie aplikacie Cestovne poriadky.
        /// </summary>
        public const string DefaultElisDirectory = @"C:\Program Files (x86)\Cestovné poriadky";

        public string AppDirectory { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public string GVDPath { get; set; } = null!;
        public List<Train> DefTrains { get; set; } = null!;
        public GVDInfo GVDInfo { get; set; } = null!;
        public Track Track { get; set; } = null!;
        public bool OmitPassingTrains { get; set; }
        public bool ReorderTrains { get; set; }
        public bool ReplaceTrains { get; set; }
    }
}
