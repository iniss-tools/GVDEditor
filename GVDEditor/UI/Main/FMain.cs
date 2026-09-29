using System.Globalization;
using AppRegistry = ToolsCore.Tools.AppRegistry;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using GVDEditor.Properties;
using GVDEditor.UI.EditTrain;
using ToolsCore.Commands;
using ToolsCore.Forms;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;
using ToolsCore.XML;
using ToolsCore;

namespace GVDEditor.UI.Main;

/// <summary>
/// Hlavný formulár. Príkazy ponuky a panela nástrojov sú v <c>FMain.Commands.cs</c>, inštalácia a grafikon
/// v <c>FMain.Grafikon.cs</c>, okná nastavení v <c>FMain.Settings.cs</c>, INISS v <c>FMain.Iniss.cs</c> a varianty
/// vlakov v <c>FMain.Variants.cs</c>.
/// </summary>
internal partial class FMain : Form, IAnalyzerHost
{
    /// <summary>
    /// Dostupné stanice.
    /// </summary>
    public static BindingList<string> Stanice { get; } = new();

    /// <summary>
    /// Všetky dostupne priečinky s grafikonmi.
    /// </summary>
    public static BindingList<GVDDirectory> ObdobiaList { get; } = new();

    private readonly List<GVDDirectory> _gvdDirs = new();
    private readonly CommandSet _commands = new();
    private readonly InissProcessService _iniss;
    private GVDDirectory? _newDir;
    private StateDgmTemplate _newDirTemplate = StateDgmTemplate.Slovak;
    private bool _prechod;
    private GVDDirectory? _previousSelectedGVD;
    private bool _removingGVD;
    private bool _grafikonLoaded;
    private bool _loading;

    /// <summary>
    /// Kontext editora - nastavenia programu, zvolena instalacia INISS a otvoreny grafikon.
    /// </summary>
    private readonly EditorContext _ctx;

    /// <summary>
    /// Vytvori nový formulár typu <see cref="FMain"/>.
    /// </summary>
    /// <param name="context">kontext editora</param>
    public FMain(EditorContext context)
    {
        _ctx = context;
        InitializeComponent();

        // stav INISSu sa hlasi vo vlakne okna - sluzba vznika az po vytvoreni prvkov
        _iniss = new InissProcessService();
        _iniss.StateChanged += (_, _) => UpdateCommandStates();
        FormClosed += (_, _) => _iniss.Dispose();

        mainMenu.Renderer = new ToolStripProfessionalRenderer(new FormUtils.LightColorTable());

        tsslSelTrainName.Font = _ctx.Config.Fonts.StateRow.Font;
        tsslSelTrainVariants.Font = _ctx.Config.Fonts.StateRow.Font;
        tsslTrainCount.Font = _ctx.Config.Fonts.StateRow.Font;
        tsslTrainCountWithVariants.Font = _ctx.Config.Fonts.StateRow.Font;
        CreateVariantMenu();

        dgvTrains.RowHeadersVisible = _ctx.Config.ShowRowsHeader;

        CreateCommands();
        _commands.ApplyShortcuts(_ctx.Config.Shortcuts);
        SetColumns();
        SetColumnsAutoWidth();

        ApplyMenuMode();

        smerovanieDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = new Bitmap(1, 1);

        SetRecentProjects();

        if (tscbStanica.ComboBox != null) tscbStanica.ComboBox.DataSource = Stanice;
        if (tscbObdobie.ComboBox != null) tscbObdobie.ComboBox.DataSource = ObdobiaList;

        this.ApplyThemeAndFonts();
        ApplyStatusBarColors();
        UpdateCommandStates();
    }

    private void FMain_Load(object sender, EventArgs e)
    {
        AppRegistry.RegisterJumpList();

        var path = Utils.GetProjectPathFromArgs();
        if (path is null && _ctx.Config.Startup == StartupType.LastProject)
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
                Text = Application.ProductName + @" - *" + _ctx.Workspace.INISSDir;
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

        // skratky prikazov - funguju aj pri skrytej ponuke a pri polozkach s podponukou (Lokalne a Globalne nastavenia)
        if (_commands.ProcessShortcut(keyData))
            return true;

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private static bool IsTextEditingKey(Keys keyData)
        => (keyData & Keys.KeyCode) is Keys.Delete or Keys.Insert or Keys.Back && (keyData & Keys.Alt) == 0;

    /// <summary>
    /// Ci sa maju chyby zachytit a ukazat pouzivatelovi (inak program pri chybe spadne - rezim ladenia).
    /// </summary>
    private bool CatchErrors => _ctx.Config.DebugModeGUI != DebugMode.AppCrash;

    /// <summary>
    /// Zaznamena chybu a ukaze ju podla rezimu ladenia.
    /// </summary>
    private void ShowException(Exception e)
    {
        Log.Exception(e);
        switch (_ctx.Config.DebugModeGUI)
        {
            case DebugMode.OnlyMessage:
                FError.ShowError(e.Message);
                break;
            case DebugMode.DetailInfo:
                FError.ShowError(e.ToString());
                break;
        }
    }

    /// <summary>
    /// Naviaze tabulku vlakov a jej stlpce Kolaj a Dopravca na otvoreny grafikon.
    /// </summary>
    private void BindDocument()
    {
        Kolaj.DataSource = _ctx.Document.Tracks;
        Dopravca.DataSource = _ctx.Document.Operators;

        var prechod = _prechod;
        _prechod = true;
        dgvTrains.DataSource = _ctx.Document.Trains;
        _prechod = prechod;
        _ctx.Document.Trains.ListChanged += (_, _) => InvalidateVariants();
        InvalidateVariants();

        SetColumnsAutoWidth();
    }

    /// <summary>
    /// Zobrazi ponuku a panel nastrojov podla nastaveni. Bez panela nastrojov sa vyber stanice a obdobia presunie do ponuky.
    /// </summary>
    private void ApplyMenuMode()
    {
        var menu = _ctx.Config.DesktopMenuMode;
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
        dgvTrains.RowHeadersVisible = _ctx.Config.ShowRowsHeader;

        this.ApplyThemeAndFonts();
        SetColumns();
        SetColumnsAutoWidth();
        _commands.ApplyShortcuts(_ctx.Config.Shortcuts);
        Refresh();
        AppInit.MsgBoxStyleInit(_ctx.UsingStyle, _ctx.Config);
        ApplyStatusBarColors();
    }

    private void ApplyStatusBarColors()
    {
        if (!_ctx.UsingStyle.HighlightStatusBar)
            return;

        statusStrip.BackColor = _ctx.UsingStyle.ControlsColorScheme.Highlight.BackColor;
        tsslSelTrainName.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
        tsslSelTrainVariants.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
        tsslTrainCount.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
        tsslTrainCountWithVariants.ForeColor = _ctx.UsingStyle.ControlsColorScheme.Highlight.ForeColor;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // pri neuspesnom ulozeni okno nezatvorit, zmeny by sa stratili
        if (!ConfirmSaveChanges())
            e.Cancel = true;
    }

    // ------------------------------------------------------------------ tabulka vlakov

    private void dgvTrains_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex != -1 && dgvTrains.Columns[e.ColumnIndex].Name == "Ostatne")
            if (dgvTrains.CurrentRow != null && e.RowIndex != -1)
                ShowEditTrain(_ctx.Document.Trains[e.RowIndex], e.RowIndex);

        if (e.RowIndex != -1)
        {
            tsslSelTrainName.Visible = true;
            tsslSelTrainVariants.Visible = true;
            tsslSelTrainName.Text = $@"{_ctx.Document.Trains[e.RowIndex].Type} {_ctx.Document.Trains[e.RowIndex].Number}";
            tsslSelTrainVariants.Text = CountSelTrainVariants(_ctx.Document.Trains[e.RowIndex]).ToString();
        }
    }

    private void dgvTrains_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        if (dgvTrains.Columns[e.ColumnIndex].Name == @"Kolaj" && e.RowIndex != -1 && e.RowIndex < _ctx.Document.Trains.Count)
            _ctx.Document.Trains[e.RowIndex].Track = _ctx.Document.Tracks[0];
        else if (dgvTrains.Columns[e.ColumnIndex].Name == @"Dopravca" && e.RowIndex != -1 &&
                 e.RowIndex < _ctx.Document.Trains.Count)
            _ctx.Document.Trains[e.RowIndex].Operator = _ctx.Document.Operators[0];
        else
            Utils.ShowError(Resources.FMain_dgvTrains_DataError_Tabuľka_obsahuje_nesprávny_údaj + e.Exception!.Message);
    }

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
            var thistrain = _ctx.Document.Trains[e.RowIndex];
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
            foreach (var train in _ctx.Document.Trains)
            {
                if (Train.IsSameVariant(train, thistrain) && i != e.RowIndex &&
                    train.ZaciatokPlatnosti == thistrain.ZaciatokPlatnosti &&
                    train.KoniecPlatnosti == thistrain.KoniecPlatnosti &&
                    dateRemThis.Overlap(thistrain.DateLimitText, train.DateLimitText))
                {
                    var obmand = dateRemThis.TextAnd(train.DateLimitText, thistrain.DateLimitText);
                    var result = Utils.ShowQuestion(string.Format(Resources.FEditTrain_DateRem_zasahuje_do_ineho_vlaku, train.Type,
                        train.Number, TrainName.ToDisplay(_ctx.Workspace.TrainNames, train.Name), obmand));
                    if (result == DialogResult.Yes)
                    {
                        if (FDateLimitEdit.SetDateLimit(this, _ctx.UsingStyle, thistrain.ZaciatokPlatnosti.ToDateTime(), thistrain.KoniecPlatnosti.ToDateTime(), train,
                                true, train.DateLimitText) is { } limit)
                            train.DateLimitText = limit;
                    }
                }

                i++;
            }
        }
    }

    private void dgvTrains_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        if (_ctx.Document.Trains.Count != 0)
        {
            for (var i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                _ctx.Document.Trains[i].ID = i + 1;

                if (_ctx.Document.Trains[i].Routing == null)
                {
                    var vlak = _ctx.Document.Trains[i];
                    throw new ArgumentNullException(
                        string.Format(CultureInfo.CurrentCulture, Resources.FMain_TrainNoRouting, vlak.Type, vlak.NumberVariant, TrainName.ToDisplay(_ctx.Workspace.TrainNames, vlak.Name)));
                }

                if (_ctx.Document.Trains[i].Routing == Routing.Prechadzajuci)
                {
                    dgvTrains.Rows[i].Cells[@"odchodDataGridViewTextBoxColumn"].ReadOnly = false;
                    dgvTrains.Rows[i].Cells[@"prichodDataGridViewTextBoxColumn"].ReadOnly = false;
                }
                else if (_ctx.Document.Trains[i].Routing == Routing.Vychadzajuci)
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
        for (var i = 1; i <= _ctx.Document.Trains.Count; i++) _ctx.Document.Trains[i - 1].ID = i;

        if (!_prechod) DataSaved = false;

        tsslTrainCountWithVariants.Text = dgvTrains.Rows.Count.ToString();
        tsslTrainCount.Text = $@"({CountTrainVariants()})";

        if (_ctx.Document.Trains.Count == 0)
        {
            tsslSelTrainName.Visible = false;
            tsslSelTrainVariants.Visible = false;
        }
    }

    private int CountTrainVariants()
    {
        var trains = new HashSet<(string num, TrainType type, string name)>();
        foreach (var t in _ctx.Document.Trains)
            trains.Add((t.Number, t.Type, t.Name));

        return trains.Count;
    }

    private void dgvTrains_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex != -1) DataSaved = false;
        InvalidateVariants();
    }

    // podfarbenie variant vybraneho vlaku - zmes pozadia tabulky a farby vyberu
    private Color SiblingColor()
    {
        var back = dgvTrains.DefaultCellStyle.BackColor;
        var mark = _ctx.UsingStyle.ControlsColorScheme.Highlight.BackColor;
        return Color.FromArgb((back.R * 4 + mark.R) / 5, (back.G * 4 + mark.G) / 5, (back.B * 4 + mark.B) / 5);
    }

    private void dgvTrains_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        void SetFromScheme(ColorSetting sett)
        {
            e.CellStyle.ForeColor = sett.ForeColor != SystemColors.ControlText ? sett.ForeColor : Color.White;
            if (sett.BackColor != Color.Transparent)
                e.CellStyle.BackColor = sett.BackColor;
            e.CellStyle.Font = sett.Bold
                ? new Font(_ctx.UsingStyle.TrainTypeColumnScheme.Font, FontStyle.Bold)
                : _ctx.UsingStyle.TrainTypeColumnScheme.Font;
        }

        if (e.RowIndex >= 0 && e.RowIndex < _ctx.Document.Trains.Count)
        {
            var rowTrain = _ctx.Document.Trains[e.RowIndex];

            // varianty vybraneho vlaku su podfarbene
            if (CurrentTrain is { } current && Variants.AreSiblings(current, rowTrain))
                e.CellStyle.BackColor = SiblingColor();

            // cislo vlaku s variantmi: poradie v skupine, pri prekryti dni s inou variantou upozornenie
            if (e.ColumnIndex == cisloDataGridViewTextBoxColumn.Index && Variants.Of(rowTrain) is { Count: > 1 } info)
            {
                e.Value = $"{rowTrain.Number}  {info.Position}/{info.Count}" + (info.Overlaps.Count != 0 ? "  ⚠" : "");
                e.FormattingApplied = true;
            }
        }

        // v grafikone je kluc zvuku nazvu vlaku, v tabulke sa zobrazuje jeho nazov
        if (e.ColumnIndex == nameDataGridViewTextBoxColumn.Index && e.Value is string { Length: > 0 } trainName)
        {
            e.Value = TrainName.ToDisplay(_ctx.Workspace.TrainNames, trainName);
            e.FormattingApplied = true;
        }

        var typColumn = dgvTrains.Columns["typDataGridViewTextBoxColumn"];
        if (typColumn != null && e.ColumnIndex == typColumn.Index)
            if (e.RowIndex < _ctx.Document.Trains.Count)
            {
                var type = _ctx.Document.Trains[e.RowIndex].Type;
                if (type.IsCustom)
                {
                    var stype = type.CategoryTrain.ToUpper();
                    if (stype.StartsWith("X"))
                        SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.X);
                    else if (stype.StartsWith("R"))
                        SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.R);
                    else if (stype.StartsWith("SL"))
                        SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.Sl);
                    else if (stype.StartsWith("OS"))
                        SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.Os);
                }
                else
                {
                    switch (type.CategoryTrain)
                    {
                        case "R":
                        case "REX":
                        case "RR":
                            SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.R);
                            break;
                        case "IC":
                        case "EC":
                        case "EN":
                        case "SC":
                            SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.X);
                            break;
                        default:
                            SetFromScheme(_ctx.UsingStyle.TrainTypeColumnScheme.Os);
                            break;
                    }
                }
            }
    }

    private void SetColumns()
    {
        static void SetCol(DataGridViewColumn column, DesktopColumn format)
        {
            column.Visible = format.Visible;
            column.MinimumWidth = format.MinWidth;
            column.DisplayIndex = format.Order;
        }

        SetCol(cisloDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Number);
        SetCol(typDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Type);
        SetCol(nameDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Name);
        SetCol(LinkaPrichod, _ctx.Config.DesktopCols.LinkaPrichod);
        SetCol(LinkaOdchod, _ctx.Config.DesktopCols.LinkaOdchod);
        SetCol(smerovanieDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Routing);
        SetCol(prichodDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Prichod);
        SetCol(odchodDataGridViewTextBoxColumn, _ctx.Config.DesktopCols.Odchod);
        SetCol(dgvcVychodziaStanica, _ctx.Config.DesktopCols.VychodziaStanica);
        SetCol(dgvcKonecnaStanica, _ctx.Config.DesktopCols.KonecnaStanica);
        SetCol(DatumoveObmedzenieText, _ctx.Config.DesktopCols.DateLimit);
        SetCol(Kolaj, _ctx.Config.DesktopCols.Track);
        SetCol(Dopravca, _ctx.Config.DesktopCols.Operator);
        SetCol(Ostatne, _ctx.Config.DesktopCols.OtherBtn);

        dgvTrains.Columns[dgvTrains.Columns.Count - 1].AutoSizeMode = _ctx.Config.FitLastColumn
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

        // varianty mozu byt aj v riadkoch mimo obrazovky
        FitNumberColumn();
    }
}
