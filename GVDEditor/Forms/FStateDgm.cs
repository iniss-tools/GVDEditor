using ExControls;
using GVDEditor.Controls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using JetBrains.Annotations;
using ToolsCore;
using ToolsCore.Expressions;
using ToolsCore.StateDgm;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Editor stavoveho diagramu vlaku (StateDgm.txt): navigator (kategorie → stavy, vzhlady, casove body),
///     panel vlastnosti, akcie s tlacidlami a startery vybraneho stavu, kontrola diagramu a text suboru.
/// </summary>
public partial class FStateDgm : Form
{
    private const string TAG_HEADER = "header";
    private const string TAG_DESIGNS = "designs";
    private const string TAG_TIMEPOINTS = "timepoints";
    private const string TAG_CATEGORIES = "categories";

    private readonly string _dir;
    private readonly string _stationName;
    private StateDgmDiagram _d;
    private bool _dirty;
    private bool _textStale = true;

    private readonly SdHeaderEditor _headerEditor = new();
    private readonly SdCategoryEditor _categoryEditor = new();
    private readonly SdStateEditor _stateEditor = new();
    private readonly SdDesignEditor _designEditor = new();
    private readonly SdTimePointEditor _timePointEditor = new();
    private readonly Label _emptyEditor = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = SystemColors.GrayText };

    private readonly System.Windows.Forms.Timer _validateTimer = new() { Interval = 500 };
    private readonly ExBindingList<ProblemRow> _problems = new() { Sortable = true };
    private readonly BindingList<EventRow> _eventRows = [];
    private readonly BindingList<StarterRow> _starterRows = [];
    private readonly ShellIcon _iconError = new(ShellIconType.Error, ShellIconSize.Small);
    private readonly ShellIcon _iconWarning = new(ShellIconType.Warning, ShellIconSize.Small);
    private readonly ShellIcon _iconInfo = new(ShellIconType.Info, ShellIconSize.Small);
    private readonly ImageList _treeImages = new() { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(16, 16) };

    private StateDgmCategory? _selCategory;
    private StateDgmState? _selState;
    private bool _suppressTree;

    /// <summary>
    ///     Otvori editor diagramu grafikonu.
    /// </summary>
    /// <param name="dir">Priecinok grafikonu.</param>
    internal FStateDgm(GVDDirectory dir) : this(dir.Dir.FullPath, dir.GVD.ThisStation?.Name ?? dir.Dir.DirName)
    {
    }

    /// <summary>
    ///     Otvori editor diagramu v danom priecinku.
    /// </summary>
    /// <param name="dirPath">Priecinok grafikonu so suborom StateDgm.txt.</param>
    /// <param name="stationName">Meno stanice do titulku.</param>
    internal FStateDgm(string dirPath, string stationName)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();
        if (GlobData.UsingStyle.DarkTitleBar) ExTools.SetImmersiveDarkMode(Handle, true);

        _dir = dirPath;
        _stationName = stationName;

        SdEditorContext.Symbols = new GvdExprSymbols();
        SdEditorContext.ReportKeys = (GlobData.ReportTypes ?? []).Select(r => r.Key).ToList();

        _treeImages.Images.Add("file", GlobalResources.flow_chart);
        _treeImages.Images.Add("designs", GlobalResources.colors);
        _treeImages.Images.Add("timepoint", GlobalResources.clock);
        _treeImages.Images.Add("categories", GlobalResources.folder);
        _treeImages.Images.Add("cat0", GlobalResources.vychodzia_st);
        _treeImages.Images.Add("cat1", GlobalResources.prechadza_st);
        _treeImages.Images.Add("cat2", GlobalResources.konecna_st);
        _treeImages.Images.Add("state", GlobalResources.train);
        _treeImages.Images.Add("design", GlobalResources.push_button);
        tvNav.ImageList = _treeImages;

        foreach (var ed in Editors)
        {
            ed.Dock = DockStyle.Fill;
            ed.Changed += Editor_Changed;
        }

        dgvEvents.DataSource = _eventRows;
        dgvStarters.DataSource = _starterRows;
        dgvProblems.DataSource = _problems;
        _validateTimer.Tick += (_, _) =>
        {
            _validateTimer.Stop();
            ValidateDiagram();
        };

        _d = LoadDiagram();
        pnlGraph.Controls.Add(new Label { Dock = DockStyle.Fill, Text = Resources.FStateDgm_GrafNeskor, TextAlign = ContentAlignment.MiddleCenter, ForeColor = SystemColors.GrayText });
        tsbCalendar.Enabled = false;
    }

    private IEnumerable<SdEditorBase> Editors => [_headerEditor, _categoryEditor, _stateEditor, _designEditor, _timePointEditor];

    private StateDgmDiagram LoadDiagram()
    {
        try
        {
            var d = TxtParser.ReadStateDgm(_dir);
            if (d != null) return d;
            _dirty = true; // subor chyba - po ulozeni vznikne z predlohy
            return StateDgmDiagram.Parse(TxtParser.StateDgmTemplateText(StateDgmTemplate.Slovak));
        }
        catch (StateDgmParseException e)
        {
            MessageBox.Show(this, string.Format(Resources.FStateDgm_SuborChyba, TxtParser.StateDgmPath(_dir), e.Line + 1, e.Message), Resources.FStateDgm_SuborChyba_Nadpis,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _dirty = true;
            return StateDgmDiagram.Parse(TxtParser.StateDgmTemplateText(StateDgmTemplate.Slovak));
        }
    }

    private void FStateDgm_Load(object sender, EventArgs e)
    {
        // rozlozenie: navigator vlavo (pevny), vlastnosti vpravo (pevne), dolne mriezky ~ tretina vysky
        scMain.FixedPanel = FixedPanel.Panel1;
        scRight.FixedPanel = FixedPanel.Panel2;
        scMain.SplitterDistance = LogicalToDeviceUnits(280);
        scRight.SplitterDistance = Math.Max(200, scRight.Width - LogicalToDeviceUnits(430));
        scOuter.SplitterDistance = Math.Max(200, scOuter.Height * 62 / 100);

        UpdateTitle();
        BuildTree();
        if (tvNav.Nodes.Count > 0) tvNav.SelectedNode = FirstStateNode() ?? tvNav.Nodes[0];
        ValidateDiagram();
    }

    #region Strom

    private void BuildTree(object? select = null)
    {
        _suppressTree = true;
        tvNav.BeginUpdate();
        tvNav.Nodes.Clear();

        tvNav.Nodes.Add(new TreeNode(Resources.FStateDgm_Diagram) { ImageKey = "file", SelectedImageKey = "file", Tag = TAG_HEADER });

        var designs = new TreeNode(Resources.FStateDgm_Vzhlady) { ImageKey = "designs", SelectedImageKey = "designs", Tag = TAG_DESIGNS };
        foreach (var d in _d.Designs)
            designs.Nodes.Add(new TreeNode(d.Key) { ImageKey = "design", SelectedImageKey = "design", Tag = d });
        tvNav.Nodes.Add(designs);

        var tps = new TreeNode(Resources.FStateDgm_CasoveBody) { ImageKey = "timepoint", SelectedImageKey = "timepoint", Tag = TAG_TIMEPOINTS };
        foreach (var k in StateDgmKeys.BuiltInTimePoints)
            tps.Nodes.Add(new TreeNode($"{k} {Resources.FStateDgm_Zabudovany}") { ImageKey = "timepoint", SelectedImageKey = "timepoint", Tag = k, ForeColor = SystemColors.GrayText });
        foreach (var t in _d.TimePoints)
            tps.Nodes.Add(new TreeNode(TimePointText(t)) { ImageKey = "timepoint", SelectedImageKey = "timepoint", Tag = t });
        tvNav.Nodes.Add(tps);

        var cats = new TreeNode(Resources.FStateDgm_Kategorie) { ImageKey = "categories", SelectedImageKey = "categories", Tag = TAG_CATEGORIES };
        foreach (var c in _d.Categories)
        {
            var cn = new TreeNode(CategoryText(c)) { ImageKey = CategoryImage(c), SelectedImageKey = CategoryImage(c), Tag = c };
            foreach (var s in c.States)
            {
                var sn = new TreeNode(StateText(s)) { ImageKey = "state", SelectedImageKey = "state", Tag = s };
                foreach (var t in s.TimePoints)
                    sn.Nodes.Add(new TreeNode(TimePointText(t)) { ImageKey = "timepoint", SelectedImageKey = "timepoint", Tag = t });
                cn.Nodes.Add(sn);
            }

            cats.Nodes.Add(cn);
        }

        tvNav.Nodes.Add(cats);
        tvNav.ExpandAll();
        tvNav.EndUpdate();
        _suppressTree = false;

        if (select != null && FindNode(select) is { } n)
        {
            tvNav.SelectedNode = n;
            n.EnsureVisible();
        }
        else if (tvNav.SelectedNode == null && tvNav.Nodes.Count > 0)
        {
            tvNav.SelectedNode = tvNav.Nodes[0];
        }
    }

    private TreeNode? FirstStateNode()
    {
        foreach (TreeNode root in tvNav.Nodes)
            if (Equals(root.Tag, TAG_CATEGORIES))
                foreach (TreeNode cat in root.Nodes)
                    if (cat.Nodes.Count > 0)
                        return cat.Nodes[0];
        return null;
    }

    private TreeNode? FindNode(object tag) => FindNode(tvNav.Nodes, tag);

    private static TreeNode? FindNode(TreeNodeCollection nodes, object tag)
    {
        foreach (TreeNode n in nodes)
        {
            if (ReferenceEquals(n.Tag, tag) || (tag is string s && Equals(n.Tag, s))) return n;
            if (FindNode(n.Nodes, tag) is { } f) return f;
        }

        return null;
    }

    private static string CategoryText(StateDgmCategory c) => c.Name.Length > 0 ? c.Name : c.Key;
    private static string CategoryImage(StateDgmCategory c) => c.Icon % 3 is 1 ? "cat1" : c.Icon % 3 is 2 ? "cat2" : "cat0";
    private static string StateText(StateDgmState s) => s.Name.Length > 0 && s.Name != s.Key ? $"{s.Key} ({s.Name})" : s.Key;
    private static string TimePointText(StateDgmTimePoint t) => t.Key.Length > 0 ? t.Key : "?";

    private void RefreshSelectedNodeText()
    {
        var n = tvNav.SelectedNode;
        if (n == null) return;
        var text = n.Tag switch
        {
            StateDgmCategory c => CategoryText(c),
            StateDgmState s => StateText(s),
            StateDgmDesign d => d.Key,
            StateDgmTimePoint t => TimePointText(t),
            _ => n.Text
        };
        if (n.Text != text) n.Text = text;
        if (n.Tag is StateDgmCategory cat)
            n.ImageKey = n.SelectedImageKey = CategoryImage(cat);
    }

    private void tvNav_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (_suppressTree) return;
        ShowSelection(e.Node?.Tag);
    }

    private void ShowSelection(object? tag)
    {
        _selCategory = null;
        _selState = null;
        Control editor;
        switch (tag)
        {
            case StateDgmCategory c:
                _selCategory = c;
                _categoryEditor.Bind(c);
                editor = _categoryEditor;
                break;
            case StateDgmState s:
                _selState = s;
                _selCategory = _d.Categories.FirstOrDefault(c => c.States.Contains(s));
                _stateEditor.Bind(s);
                editor = _stateEditor;
                break;
            case StateDgmDesign d:
                _designEditor.Bind(d);
                editor = _designEditor;
                break;
            case StateDgmTimePoint t:
                var owner = _d.Categories.SelectMany(c => c.States).FirstOrDefault(s => s.TimePoints.Contains(t));
                _selState = owner;
                _timePointEditor.Bind(t, _d.AllTimePointKeys.Concat(owner?.TimePoints.Select(x => x.Key) ?? []));
                editor = _timePointEditor;
                break;
            case string str when str == TAG_HEADER:
                _headerEditor.Bind(_d);
                editor = _headerEditor;
                break;
            case string str when str != TAG_HEADER && !str.StartsWith('#'):
                _emptyEditor.Text = "";
                editor = _emptyEditor;
                break;
            default:
                _emptyEditor.Text = tag as string ?? "";
                editor = _emptyEditor;
                break;
        }

        if (!pnlProps.Controls.Contains(editor))
        {
            pnlProps.SuspendLayout();
            pnlProps.Controls.Clear();
            FormUtils.ChangeStyleOfControls(GlobData.UsingStyle, new[] { editor });
            editor.Font = Font;
            pnlProps.Controls.Add(editor);
            pnlProps.ResumeLayout();
        }

        FillStateGrids();
        UpdateToolbar();
    }

    private void UpdateToolbar()
    {
        var tag = tvNav.SelectedNode?.Tag;
        tsmiNewState.Enabled = _selCategory != null;
        tsbDelete.Enabled = tag is StateDgmCategory or StateDgmState or StateDgmDesign or StateDgmTimePoint;
        tsbUp.Enabled = tsbDown.Enabled = tsbDelete.Enabled;
        tsbEvAdd.Enabled = tsbStAdd.Enabled = _selState != null;
        dgvEvents_SelectionChanged(this, EventArgs.Empty);
        dgvStarters_SelectionChanged(this, EventArgs.Empty);
    }

    #endregion

    #region Zmeny a kontrola

    private void Editor_Changed(object? sender, EventArgs e)
    {
        MarkDirty();
        RefreshSelectedNodeText();
    }

    private void MarkDirty()
    {
        _dirty = true;
        _textStale = true;
        UpdateTitle();
        _validateTimer.Stop();
        _validateTimer.Start();
    }

    private void UpdateTitle()
    {
        Text = string.Format(Resources.FStateDgm_Title, _stationName) + (_dirty ? " *" : "");
    }

    private void ValidateDiagram()
    {
        var diags = StateDgmValidator.Validate(_d, new StateDgmValidationOptions
        {
            ReportKeys = SdEditorContext.ReportKeys.Count > 0 ? SdEditorContext.ReportKeys : null,
            Symbols = SdEditorContext.Symbols
        });
        _problems.RaiseListChangedEvents = false;
        _problems.Clear();
        foreach (var d in diags) _problems.Add(new ProblemRow(d));
        _problems.RaiseListChangedEvents = true;
        _problems.ResetBindings();

        var errors = diags.Count(x => x.IsError);
        var warnings = diags.Count(x => x.Severity == ExprSeverity.Warning);
        var infos = diags.Count - errors - warnings;
        tsslStatus.Text = diags.Count == 0 ? Resources.FStateDgm_BezProblemov : string.Format(Resources.FStateDgm_PocetProblemov, errors, warnings, infos);
        tpProblems.Text = diags.Count == 0 ? Resources.FStateDgm_Problemy : $"{Resources.FStateDgm_Problemy} ({diags.Count})";

        if (tcCenter.SelectedTab == tpText) RefreshText();
    }

    private void RefreshText()
    {
        if (!_textStale) return;
        tbText.Text = _d.ToText();
        _textStale = false;
    }

    private void tcCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tcCenter.SelectedTab == tpText) RefreshText();
    }

    private void tsbCheck_Click(object sender, EventArgs e)
    {
        ValidateDiagram();
        tcBottom.SelectedTab = tpProblems;
    }

    #endregion

    #region Ulozenie

    private bool Save()
    {
        ValidateDiagram();
        var errors = _problems.Count(p => p.Diagnostic.IsError);
        if (errors > 0 && MessageBox.Show(this, string.Format(Resources.FStateDgm_UlozitSChybami, errors), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            tcBottom.SelectedTab = tpProblems;
            return false;
        }

        try
        {
            TxtParser.WriteStateDgm(_dir, _d);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        _dirty = false;
        UpdateTitle();
        tsslStatus.Text = string.Format(Resources.FStateDgm_Ulozene, DateTime.Now.ToShortTimeString()) + "  –  " + tsslStatus.Text;
        return true;
    }

    private void tsbSave_Click(object sender, EventArgs e) => Save();

    private void FStateDgm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.Handled = true;
            Save();
        }
    }

    private void FStateDgm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_dirty) return;
        switch (MessageBox.Show(this, Resources.FStateDgm_NeulozeneZmeny, Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
            case DialogResult.Yes:
                if (!Save()) e.Cancel = true;
                break;
            case DialogResult.Cancel:
                e.Cancel = true;
                break;
        }
    }

    #endregion

    #region Pridanie, odstranenie, poradie

    private void tsmiNewCategory_Click(object sender, EventArgs e)
    {
        var c = new StateDgmCategory { Key = UniqueKey(_d.Categories.Select(x => x.Key), "#Kategorie"), Name = Resources.FStateDgm_NovaKategoria };
        c.States.Add(NewState(StateDgmKeys.START_STATE));
        _d.Categories.Add(c);
        MarkDirty();
        BuildTree(c);
    }

    private void tsmiNewState_Click(object sender, EventArgs e)
    {
        if (_selCategory == null) return;
        var s = NewState(UniqueKey(_selCategory.States.Select(x => x.Key), _selCategory.States.Count == 0 ? StateDgmKeys.START_STATE : "Stav"));
        var at = _selState != null ? _selCategory.States.IndexOf(_selState) + 1 : _selCategory.States.Count;
        _selCategory.States.Insert(at, s);
        MarkDirty();
        BuildTree(s);
    }

    private static StateDgmState NewState(string key) => new()
    {
        Key = key,
        DoState = new StateDgmTableSet { OnDepartureTable = true, ShowTrack = true, ShowPosition = true }
    };

    private void tsmiNewDesign_Click(object sender, EventArgs e)
    {
        var d = new StateDgmDesign { Key = UniqueKey(_d.Designs.Select(x => x.Key), "Vzhlad"), Bitmaps = "0-0,1,2" };
        _d.Designs.Add(d);
        MarkDirty();
        BuildTree(d);
    }

    private void tsmiNewTimePoint_Click(object sender, EventArgs e)
    {
        var t = new StateDgmTimePoint();
        if (_selState != null)
        {
            t.Key = UniqueKey(_selState.TimePoints.Select(x => x.Key), StateDgmKeys.START_TIME);
            _selState.TimePoints.Add(t);
        }
        else
        {
            t.Key = UniqueKey(_d.TimePoints.Select(x => x.Key), "#Bod");
            t.TimePointKey1 = StateDgmKeys.BuiltInTimePoints[1];
            t.TimePointKey2 = StateDgmKeys.BuiltInTimePoints[3];
            _d.TimePoints.Add(t);
        }

        MarkDirty();
        BuildTree(t);
    }

    private static string UniqueKey(IEnumerable<string> existing, string baseKey)
    {
        var set = existing.ToHashSet(StringComparer.Ordinal);
        if (!set.Contains(baseKey)) return baseKey;
        for (var i = 2;; i++)
            if (!set.Contains(baseKey + i))
                return baseKey + i;
    }

    private void tsbDelete_Click(object sender, EventArgs e)
    {
        var tag = tvNav.SelectedNode?.Tag;
        var name = tag switch
        {
            StateDgmCategory c => CategoryText(c),
            StateDgmState s => StateText(s),
            StateDgmDesign d => d.Key,
            StateDgmTimePoint t => t.Key,
            _ => null
        };
        if (name == null) return;
        if (MessageBox.Show(this, string.Format(Resources.FStateDgm_OdstranitOtazka, name), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        object? select = null;
        switch (tag)
        {
            case StateDgmCategory c:
                _d.Categories.Remove(c);
                select = TAG_CATEGORIES;
                break;
            case StateDgmState s:
                var cat = _d.Categories.First(x => x.States.Contains(s));
                cat.States.Remove(s);
                select = cat;
                break;
            case StateDgmDesign d:
                _d.Designs.Remove(d);
                select = TAG_DESIGNS;
                break;
            case StateDgmTimePoint t:
                if (!_d.TimePoints.Remove(t))
                    foreach (var s in _d.Categories.SelectMany(x => x.States))
                        if (s.TimePoints.Remove(t))
                        {
                            select = s;
                            break;
                        }

                select ??= TAG_TIMEPOINTS;
                break;
        }

        MarkDirty();
        BuildTree(select);
    }

    private void tsbUp_Click(object sender, EventArgs e) => Move(-1);

    private void tsbDown_Click(object sender, EventArgs e) => Move(1);

    private void Move(int delta)
    {
        var tag = tvNav.SelectedNode?.Tag;
        var moved = tag switch
        {
            StateDgmCategory c => MoveIn(_d.Categories, c, delta),
            StateDgmState s => MoveIn(_d.Categories.First(x => x.States.Contains(s)).States, s, delta),
            StateDgmDesign d => MoveIn(_d.Designs, d, delta),
            StateDgmTimePoint t => _d.TimePoints.Contains(t) ? MoveIn(_d.TimePoints, t, delta) : _selState != null && MoveIn(_selState.TimePoints, t, delta),
            _ => false
        };
        if (!moved) return;
        MarkDirty();
        BuildTree(tag);
    }

    private static bool MoveIn<T>(List<T> list, T item, int delta)
    {
        var i = list.IndexOf(item);
        var j = i + delta;
        if (i < 0 || j < 0 || j >= list.Count) return false;
        (list[i], list[j]) = (list[j], list[i]);
        return true;
    }

    private void tsmiTpl_Click(object sender, EventArgs e)
    {
        var (template, name) = sender == tsmiTplCZ ? (StateDgmTemplate.Czech, Resources.FStateDgm_PredlohaCZ)
            : sender == tsmiTplILTIS ? (StateDgmTemplate.SlovakIltis, Resources.FStateDgm_PredlohaILTIS)
            : (StateDgmTemplate.Slovak, Resources.FStateDgm_PredlohaSK);
        if (MessageBox.Show(this, string.Format(Resources.FStateDgm_PredlohaOtazka, name), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _d = StateDgmDiagram.Parse(TxtParser.StateDgmTemplateText(template));
        MarkDirty();
        BuildTree();
        tvNav.SelectedNode = FirstStateNode() ?? tvNav.Nodes[0];
        ValidateDiagram();
    }

    #endregion

    #region Akcie a startery vybraneho stavu

    private void FillStateGrids()
    {
        _eventRows.RaiseListChangedEvents = false;
        _starterRows.RaiseListChangedEvents = false;
        _eventRows.Clear();
        _starterRows.Clear();
        if (_selState != null)
        {
            var used = new HashSet<StateDgmEvent>();
            foreach (var c in _selState.Controls.OrderBy(c => c.CtrlId))
            {
                var ev = _selState.Events.FirstOrDefault(x => x.Key == c.EventKey && !used.Contains(x));
                if (ev != null) used.Add(ev);
                _eventRows.Add(new EventRow(ev, c));
            }

            foreach (var ev in _selState.Events.Where(x => !used.Contains(x)))
                _eventRows.Add(new EventRow(ev, null));
            foreach (var st in _selState.Starters)
                _starterRows.Add(new StarterRow(st));
        }

        _eventRows.RaiseListChangedEvents = true;
        _starterRows.RaiseListChangedEvents = true;
        _eventRows.ResetBindings();
        _starterRows.ResetBindings();
        tpEvents.Text = _selState == null ? Resources.FStateDgm_AkcieTlacidla : $"{Resources.FStateDgm_AkcieTlacidla} ({_eventRows.Count})";
        tpStarters.Text = _selState == null || _starterRows.Count == 0 ? Resources.FStateDgm_Startery : $"{Resources.FStateDgm_Startery} ({_starterRows.Count})";
    }

    private EventRow? SelectedEventRow => dgvEvents.CurrentRow?.DataBoundItem as EventRow;
    private StarterRow? SelectedStarterRow => dgvStarters.CurrentRow?.DataBoundItem as StarterRow;

    private void dgvEvents_SelectionChanged(object sender, EventArgs e)
    {
        var has = SelectedEventRow != null && _selState != null;
        tsbEvEdit.Enabled = tsbEvDelete.Enabled = has;
        tsbEvUp.Enabled = has && dgvEvents.CurrentRow!.Index > 0;
        tsbEvDown.Enabled = has && dgvEvents.CurrentRow!.Index < _eventRows.Count - 1;
    }

    private void dgvStarters_SelectionChanged(object sender, EventArgs e)
    {
        tsbStEdit.Enabled = tsbStDelete.Enabled = SelectedStarterRow != null && _selState != null;
    }

    private void dgvEvents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) EditEvent(SelectedEventRow);
    }

    private void dgvStarters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) EditStarter(SelectedStarterRow?.Starter);
    }

    private void tsbEvAdd_Click(object sender, EventArgs e) => EditEvent(null);

    private void tsbEvEdit_Click(object sender, EventArgs e) => EditEvent(SelectedEventRow);

    private void EditEvent(EventRow? row)
    {
        if (_selState == null || _selCategory == null) return;
        var ev = row?.Event ?? new StateDgmEvent { Key = UniqueKey(_selState.Events.Select(x => x.Key), "#Akcia"), Class = "SDEventUniPos" };
        var control = row?.Control;
        var isNew = row?.Event == null;

        var editor = new SdEventEditor();
        var nextId = _selState.Controls.Count == 0 ? 0 : _selState.Controls.Max(c => c.CtrlId) + 1;
        editor.Bind(ev, control, _selCategory.States.Select(s => s.Key), _d.Designs.Select(d => d.Key), nextId);
        using var dlg = new FStateDgmItem(Resources.FStateDgm_Akcia_Titul, editor, 520, 640);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        var newControl = editor.Apply(ev, control);
        if (isNew) _selState.Events.Add(ev);
        if (control != null && newControl == null) _selState.Controls.Remove(control);
        if (control == null && newControl != null) _selState.Controls.Add(newControl);
        // tlacidla ostatnych akcii, ktore ukazovali na stary kluc
        if (row?.Event != null && control != null && control.EventKey != ev.Key) control.EventKey = ev.Key;

        MarkDirty();
        FillStateGrids();
        _stateEditor.RefreshControls();
        SelectEventRow(ev);
    }

    private void SelectEventRow(StateDgmEvent ev)
    {
        foreach (DataGridViewRow r in dgvEvents.Rows)
            if (r.DataBoundItem is EventRow er && er.Event == ev)
            {
                dgvEvents.CurrentCell = r.Cells[0];
                break;
            }
    }

    private void tsbEvDelete_Click(object sender, EventArgs e)
    {
        var row = SelectedEventRow;
        if (row == null || _selState == null) return;
        if (MessageBox.Show(this, string.Format(Resources.FStateDgm_OdstranitOtazka, row.Key), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (row.Event != null) _selState.Events.Remove(row.Event);
        if (row.Control != null) _selState.Controls.Remove(row.Control);
        MarkDirty();
        FillStateGrids();
        _stateEditor.RefreshControls();
    }

    private void tsbEvUp_Click(object sender, EventArgs e) => MoveEvent(-1);

    private void tsbEvDown_Click(object sender, EventArgs e) => MoveEvent(1);

    private void MoveEvent(int delta)
    {
        var row = SelectedEventRow;
        if (row == null || _selState == null || dgvEvents.CurrentRow == null) return;
        var idx = dgvEvents.CurrentRow.Index;
        var other = idx + delta;
        if (other < 0 || other >= _eventRows.Count) return;
        var target = _eventRows[other];

        // poradie tlacidiel urcuje CtrlID, poradie akcii ich zoznam
        if (row.Control != null && target.Control != null)
            (row.Control.CtrlId, target.Control.CtrlId) = (target.Control.CtrlId, row.Control.CtrlId);
        if (row.Event != null && target.Event != null)
        {
            var i = _selState.Events.IndexOf(row.Event);
            var j = _selState.Events.IndexOf(target.Event);
            (_selState.Events[i], _selState.Events[j]) = (_selState.Events[j], _selState.Events[i]);
        }

        MarkDirty();
        FillStateGrids();
        _stateEditor.RefreshControls();
        if (row.Event != null) SelectEventRow(row.Event);
    }

    private void tsbStAdd_Click(object sender, EventArgs e) => EditStarter(null);

    private void tsbStEdit_Click(object sender, EventArgs e) => EditStarter(SelectedStarterRow?.Starter);

    private void EditStarter(StateDgmStarter? starter)
    {
        if (_selState == null) return;
        var isNew = starter == null;
        starter ??= new StateDgmStarter
        {
            Key = UniqueKey(_selState.Starters.Select(x => x.Key), "Starter"),
            EventKey = _selState.Events.FirstOrDefault()?.Key ?? "",
            TimePointKey = StateDgmKeys.BuiltInTimePoints[1],
            TimeOffset = -360,
            TimeOffsetStep = 600,
            TimePointKeyLast = StateDgmKeys.BuiltInTimePoints[3],
            TimeOffsetLast = -300
        };
        var editor = new SdStarterEditor();
        editor.Bind(starter, _selState.Events.Select(x => x.Key), _d.AllTimePointKeys.Concat(_selState.TimePoints.Select(t => t.Key)));
        using var dlg = new FStateDgmItem(Resources.FStateDgm_Starter_Titul, editor, 520, 520);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        editor.Apply(starter);
        if (isNew) _selState.Starters.Add(starter);
        MarkDirty();
        FillStateGrids();
    }

    private void tsbStDelete_Click(object sender, EventArgs e)
    {
        var row = SelectedStarterRow;
        if (row == null || _selState == null) return;
        if (MessageBox.Show(this, string.Format(Resources.FStateDgm_OdstranitOtazka, row.Key), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _selState.Starters.Remove(row.Starter);
        MarkDirty();
        FillStateGrids();
    }

    #endregion

    #region Problemy

    private void dgvProblems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex != cProbType.Index || e.RowIndex < 0) return;
        if (dgvProblems.Rows[e.RowIndex].DataBoundItem is not ProblemRow pr) return;
        e.Value = pr.Diagnostic.Severity switch
        {
            ExprSeverity.Error => _iconError.ToBitmap(),
            ExprSeverity.Warning => _iconWarning.ToBitmap(),
            _ => _iconInfo.ToBitmap()
        };
        e.FormattingApplied = true;
    }

    private void dgvProblems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvProblems.Rows[e.RowIndex].DataBoundItem is ProblemRow pr)
            GoTo(pr.Diagnostic.Location);
    }

    private void GoTo(StateDgmLocation loc)
    {
        object? target = loc.Kind switch
        {
            StateDgmElementKind.Diagram => TAG_HEADER,
            StateDgmElementKind.Design => _d.Designs.ElementAtOrDefault(loc.Index),
            StateDgmElementKind.TimePoint when loc.Category < 0 => _d.TimePoints.ElementAtOrDefault(loc.Index),
            StateDgmElementKind.TimePoint => StateAt(loc)?.TimePoints.ElementAtOrDefault(loc.Index),
            StateDgmElementKind.Category => _d.Categories.ElementAtOrDefault(loc.Category),
            _ => StateAt(loc)
        };
        if (target == null) return;
        if (FindNode(target) is { } n)
        {
            tvNav.SelectedNode = n;
            n.EnsureVisible();
        }

        switch (loc.Kind)
        {
            case StateDgmElementKind.Event when _selState != null:
                tcBottom.SelectedTab = tpEvents;
                if (_selState.Events.ElementAtOrDefault(loc.Index) is { } ev) SelectEventRow(ev);
                break;
            case StateDgmElementKind.Control when _selState != null:
                tcBottom.SelectedTab = tpEvents;
                if (_selState.Controls.ElementAtOrDefault(loc.Index) is { } c)
                    foreach (DataGridViewRow r in dgvEvents.Rows)
                        if (r.DataBoundItem is EventRow er && er.Control == c)
                        {
                            dgvEvents.CurrentCell = r.Cells[0];
                            break;
                        }

                break;
            case StateDgmElementKind.Starter when _selState != null:
                tcBottom.SelectedTab = tpStarters;
                if (loc.Index >= 0 && loc.Index < dgvStarters.Rows.Count) dgvStarters.CurrentCell = dgvStarters.Rows[loc.Index].Cells[0];
                break;
        }
    }

    private StateDgmState? StateAt(StateDgmLocation loc) => _d.Categories.ElementAtOrDefault(loc.Category)?.States.ElementAtOrDefault(loc.State);

    private void tsbCalendar_Click(object sender, EventArgs e)
    {
        // Kalendar akcii vlaku - dalsi krok
    }

    #endregion

    #region Riadky mriezok

    /// <summary>Riadok zoznamu problemov (vlastnosti su DataPropertyName stlpcov).</summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class ProblemRow(StateDgmDiagnostic diagnostic)
    {
        public StateDgmDiagnostic Diagnostic { get; } = diagnostic;

        public int Severity => diagnostic.Severity switch
        {
            ExprSeverity.Error => 0,
            ExprSeverity.Warning => 1,
            _ => 2
        };

        public string Code => diagnostic.Code.ToString();
        public string Path => diagnostic.Path;
        public string Message => diagnostic.Message;
    }

    /// <summary>Riadok akcie s jej tlacidlom.</summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class EventRow(StateDgmEvent? ev, StateDgmControl? control)
    {
        public StateDgmEvent? Event { get; } = ev;
        public StateDgmControl? Control { get; } = control;

        public string CtrlId => control?.CtrlId.ToString() ?? Resources.FStateDgm_BezTlacidla;
        public string Design => control?.DesignKey ?? "";
        public string Key => ev?.Key ?? control?.EventKey ?? "";
        public string Class => ev?.Class ?? "";
        public string NextState => ev?.NextState ?? "";
        public string ReportKey => ev?.ReportKey ?? "";
        public string Dialog => ev?.Dialog ?? "";
    }

    /// <summary>Riadok startera.</summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class StarterRow(StateDgmStarter starter)
    {
        public StateDgmStarter Starter { get; } = starter;
        public string Key => starter.Key;
        public string EventKey => starter.EventKey;
        public string Text => SdStarterEditor.Sentence(starter);
    }

    #endregion
}