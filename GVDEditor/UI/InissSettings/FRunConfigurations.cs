using System.Globalization;
using ExControls;
using GVDEditor.Config;
using GVDEditor.Integration;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Okno Konfiguracie spustania INISSu - zoznam konfiguracii otvorenej instalacie (na tomto pocitaci a zdielane
/// v datach instalacie) a vlastnosti vybranej: program, vetva registra, spravca, parametre, aktivatory grafikonov,
/// co pred spustenim a pri beziacom INISSe. Pracuje nad kopiami - ulozi hlavne okno po OK.
/// </summary>
internal partial class FRunConfigurations : Form
{
    private static readonly Bitmap InfoIcon = StockIcon(ShellIconType.Info);
    private static readonly Bitmap WarningIcon = StockIcon(ShellIconType.Warning);
    private static readonly Bitmap ErrorIcon = StockIcon(ShellIconType.Error);

    private readonly EditorContext _ctx;
    private readonly IInissProcess _iniss;
    private readonly IDialogService _dialogs;
    private readonly List<RunConfiguration> _items;
    private readonly Dictionary<ExCheckBox, InissSwitch> _switches;
    private readonly IReadOnlyDictionary<int, List<string>> _activatorTargets;
    private readonly IReadOnlyList<string> _branches;
    private readonly RunEnvironment _environment;
    private RunConfiguration? _current;
    private bool _updating;
    private bool _dirty;
    private Font? _groupFont;
    private readonly ToolTip _toolTip = new();
    // nacitane nastavenia INISSu podla vetvy, rezimu a programu - register sa necita pri kazdom stlaceni klavesy
    private readonly Dictionary<string, ResolvedConfig> _resolved = [];

    /// <summary>
    /// Vytvori okno pre konfiguracie otvorenej instalacie.
    /// </summary>
    /// <param name="ctx">kontext editora (instalacia, konfiguracie spustania)</param>
    /// <param name="iniss">beziace INISSy (oznacenie v zozname, okno Nastavenia INISSu)</param>
    /// <param name="dialogs">dialogy</param>
    public FRunConfigurations(EditorContext ctx, IInissProcess iniss, IDialogService dialogs)
    {
        _ctx = ctx;
        _iniss = iniss;
        _dialogs = dialogs;
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _items = ctx.RunConfigurations.Items.Select(i => i with { }).ToList();
        _branches = InissRegistry.AppNames();
        _environment = InissEnvironment.Create(ctx.Workspace, _items, false) with
        {
            RegistryBranches = _branches,
            VirtualStoreShadows = c => c.RunAsAdmin || c.Program.Length == 0 ? [] : InissEnvironment.VirtualStoreShadows(Resolved(c))
        };
        _activatorTargets = _environment.ActivatorTargets;
        _switches = new Dictionary<ExCheckBox, InissSwitch>
        {
            [cboxMinimize] = Switch("Minimize"), [cboxMultiuse] = Switch("Multiuse"), [cboxRemote] = Switch("Remote"),
            [cboxExport] = Switch("Export"), [cboxExportHlas] = Switch("ExportHlas"), [cboxImport] = Switch("Import"),
            [cboxImportDat] = Switch("ImportDat"), [cboxNoRestore] = Switch("NoRestore")
        };

        foreach (var exe in ctx.Workspace.INISSExeFiles)
            cbProgram.Items.Add(new ProgramItem(exe, ProgramText(exe)));
        foreach (var branch in _branches) cbRegistry.Items.Add(branch);
        cbSaveBefore.Items.AddRange([Resources.Run_SaveAsk, Resources.Run_SaveAlways, Resources.Run_SaveNever]);
        cbWhenRunning.Items.AddRange([Resources.Run_WhenRunningAsk, Resources.Run_WhenRunningRestart, Resources.Run_WhenRunningNew]);
        cbProgram.Format += (_, e) => e.Value = (e.ListItem as ProgramItem)?.Text ?? e.Value;

        tvConfigs.AfterSelect += (_, _) => ShowDetail((tvConfigs.SelectedNode?.Tag as RunConfiguration) ?? FirstIn(tvConfigs.SelectedNode));
        bAdd.Click += (_, _) => AddConfiguration(null);
        bCopy.Click += (_, _) =>
        {
            if (_current is not null) AddConfiguration(_current);
        };
        bRemove.Click += (_, _) => RemoveCurrent();
        tbName.TextChanged += (_, _) => Edit(c => c.Name = tbName.Text.Trim(), true);
        cbProgram.SelectedIndexChanged += (_, _) => Edit(c => c.Program = (cbProgram.SelectedItem as ProgramItem)?.File ?? c.Program, false);
        cbRegistry.TextChanged += (_, _) => Edit(c => c.Registry = cbRegistry.Text.Trim(), false);
        cboxAdmin.CheckedChanged += (_, _) => Edit(c => c.RunAsAdmin = cboxAdmin.Checked, false);
        cboxShared.CheckedChanged += (_, _) => Edit(c => c.Shared = cboxShared.Checked, true);
        foreach (var (box, sw) in _switches)
            box.CheckedChanged += (_, _) => Edit(c => sw.Set(c, box.Checked), false);
        tbExtra.TextChanged += (_, _) => Edit(c => c.ExtraArguments = tbExtra.Text.Trim(), false);
        cbSaveBefore.SelectedIndexChanged += (_, _) => Edit(c => c.SaveBefore = (SaveBeforeRun)Math.Max(0, cbSaveBefore.SelectedIndex), false);
        cboxAnalyze.CheckedChanged += (_, _) => Edit(c => c.AnalyzeBefore = cboxAnalyze.Checked, false);
        cbWhenRunning.SelectedIndexChanged += (_, _) => Edit(c => c.WhenRunning = (WhenAlreadyRunning)Math.Max(0, cbWhenRunning.SelectedIndex), false);
        bInissSettings.Click += (_, _) => ShowInissSettings();
        bLogs.Click += (_, _) => OpenLogs();
        bOK.Click += (_, _) => Accept(false);
        bRun.Click += (_, _) => Accept(true);
        _iniss.StateChanged += Iniss_StateChanged;

        SelectedId = ctx.RunConfigurations.Selected?.Id;
        BuildTree(SelectedId);
    }

    /// <summary>Konfiguracie po ulozeni okna.</summary>
    public IReadOnlyList<RunConfiguration> Result => _items;

    /// <summary>Vybrana konfiguracia.</summary>
    public string? SelectedId { get; private set; }

    /// <summary>Pouzivatel chce vybranu konfiguraciu po ulozeni hned spustit.</summary>
    public bool RunAfterSave { get; private set; }

    /// <inheritdoc />
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // deliaca ciara az ked ma kontajner skutocnu velkost (z navrhu sa pri FixedPanel neuplatni)
        scMain.SplitterDistance = LogicalToDeviceUnits(250);
        // sirka textov upozorneni az po skalovani okna; pri zmene sirky (aj posuvnik) znova
        FitChecks();
        pDetail.ClientSizeChanged += (_, _) => FitChecks();
    }

    private static Bitmap StockIcon(ShellIconType type)
    {
        using var icon = new ShellIcon(type, ShellIconSize.Small);
        return icon.ToBitmap();
    }

    private static InissSwitch Switch(string name) => RunConfigurations.Switches.First(s => s.Name == name);

    /// <summary>Polozka zoznamu programov - subor a text s verziou.</summary>
    private sealed record ProgramItem(string File, string Text)
    {
        public override string ToString() => Text;
    }

    private string ProgramText(string exe)
    {
        var path = PathUtils.CombinePath(_ctx.Workspace.INISSDir, exe)!;
        if (!File.Exists(path))
            return string.Format(CultureInfo.CurrentCulture, Resources.Run_ProgramMissing, exe);
        return InissRegistry.ExeVersion(path) is { } version
            ? string.Format(CultureInfo.CurrentCulture, Resources.Run_ProgramVersion, exe, version)
            : exe;
    }

    // --- zoznam ---

    private void BuildTree(string? select)
    {
        _updating = true;
        tvConfigs.BeginUpdate();
        try
        {
            tvConfigs.Nodes.Clear();
            var bold = _groupFont ??= new Font(tvConfigs.Font, FontStyle.Bold);
            var local = new TreeNode(Resources.Run_GroupLocal) { Tag = false, NodeFont = bold };
            var shared = new TreeNode(Resources.Run_GroupShared) { Tag = true, NodeFont = bold };
            TreeNode? selected = null;
            foreach (var item in _items.OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                var node = new TreeNode(NodeText(item)) { Tag = item, ToolTipText = RunConfigurations.CommandLine(item) };
                (item.Shared ? shared : local).Nodes.Add(node);
                if (item.Id == select) selected = node;
            }

            tvConfigs.Nodes.Add(local);
            if (shared.Nodes.Count > 0) tvConfigs.Nodes.Add(shared);
            tvConfigs.ExpandAll();
            tvConfigs.SelectedNode = selected ?? FirstNode();
        }
        finally
        {
            tvConfigs.EndUpdate();
            _updating = false;
        }

        ShowDetail(tvConfigs.SelectedNode?.Tag as RunConfiguration);
    }

    private TreeNode? FirstNode() => tvConfigs.Nodes.Cast<TreeNode>().SelectMany(g => g.Nodes.Cast<TreeNode>()).FirstOrDefault();

    private static RunConfiguration? FirstIn(TreeNode? group) => group?.Nodes.Count > 0 ? group.Nodes[0].Tag as RunConfiguration : null;

    private string NodeText(RunConfiguration config)
    {
        var running = _iniss.Instances.Count(i => i.Configuration.Id == config.Id);
        var name = config.Name.Length > 0 ? config.Name : Resources.Run_Unnamed;
        return running > 0 ? string.Format(CultureInfo.CurrentCulture, Resources.Run_NodeRunning, name) : name;
    }

    private TreeNode? NodeOf(RunConfiguration config) =>
        tvConfigs.Nodes.Cast<TreeNode>().SelectMany(g => g.Nodes.Cast<TreeNode>()).FirstOrDefault(n => ReferenceEquals(n.Tag, config));

    private void Iniss_StateChanged(object? sender, EventArgs e)
    {
        if (IsDisposed) return;
        foreach (var node in tvConfigs.Nodes.Cast<TreeNode>().SelectMany(g => g.Nodes.Cast<TreeNode>()))
            if (node.Tag is RunConfiguration config) node.Text = NodeText(config);
    }

    private void AddConfiguration(RunConfiguration? copyOf)
    {
        RunConfiguration config;
        if (copyOf is not null)
        {
            config = copyOf with
            {
                Id = RunConfiguration.NewId(),
                Name = RunConfigurations.UniqueName(string.Format(CultureInfo.CurrentCulture, Resources.Run_CopyName, copyOf.Name), _items.Select(i => i.Name))
            };
        }
        else
        {
            var program = (cbProgram.SelectedItem as ProgramItem)?.File ?? _ctx.Workspace.INISSExeFiles.FirstOrDefault() ?? "";
            var name = program.Length > 0 ? RunConfigurations.DefaultName(program) : Resources.Run_Unnamed;
            config = new RunConfiguration { Name = RunConfigurations.UniqueName(name, _items.Select(i => i.Name)), Program = program };
        }

        _items.Add(config);
        _dirty = true;
        BuildTree(config.Id);
        tbName.Focus();
        tbName.SelectAll();
    }

    private void RemoveCurrent()
    {
        if (_current is null) return;
        var index = _items.IndexOf(_current);
        _items.Remove(_current);
        _dirty = true;
        var next = _items.Count == 0 ? null : _items[Math.Min(index, _items.Count - 1)];
        BuildTree(next?.Id);
    }

    // --- detail ---

    private void ShowDetail(RunConfiguration? config)
    {
        if (_updating) return;
        _current = config;
        SelectedId = config?.Id ?? SelectedId;
        _updating = true;
        try
        {
            tlpDetail.Enabled = config is not null;
            bCopy.Enabled = bRemove.Enabled = config is not null;
            bRun.Enabled = config is not null;
            if (config is null)
            {
                ClearChecks();
                return;
            }

            tbName.Text = config.Name;
            var program = cbProgram.Items.Cast<ProgramItem>().FirstOrDefault(p => string.Equals(p.File, config.Program, StringComparison.OrdinalIgnoreCase));
            if (program is null && config.Program.Length > 0)
            {
                program = new ProgramItem(config.Program, ProgramText(config.Program));
                cbProgram.Items.Add(program);
            }

            cbProgram.SelectedItem = program;
            cbRegistry.Text = config.Registry;
            cboxAdmin.Checked = config.RunAsAdmin;
            cboxShared.Checked = config.Shared;
            foreach (var (box, sw) in _switches) box.Checked = sw.Get(config);
            tbExtra.Text = config.ExtraArguments;
            cbSaveBefore.SelectedIndex = (int)config.SaveBefore;
            cboxAnalyze.Checked = config.AnalyzeBefore;
            cbWhenRunning.SelectedIndex = (int)config.WhenRunning;
            BuildActivators(config);
        }
        finally
        {
            _updating = false;
        }

        UpdateDerived();
    }

    /// <summary>
    /// Zaskrtavacie polia aktivatorov /1-/9: cislice, ktore pouzivaju grafikony instalacie, a zapnute cislice bez
    /// grafikonu (aby sa dali vypnut).
    /// </summary>
    private void BuildActivators(RunConfiguration config)
    {
        flpActivators.SuspendLayout();
        foreach (Control c in flpActivators.Controls.Cast<Control>().ToList())
        {
            flpActivators.Controls.Remove(c);
            c.Dispose();
        }

        var active = config.ActivatorList;
        var digits = _activatorTargets.Keys.Union(active).Order().ToList();
        if (digits.Count == 0)
        {
            flpActivators.Controls.Add(new Label { AutoSize = true, Margin = new Padding(3, 4, 3, 3), Text = Resources.Run_NoActivators });
        }

        foreach (var n in digits)
        {
            var text = _activatorTargets.TryGetValue(n, out var names)
                ? string.Format(CultureInfo.CurrentCulture, Resources.Run_Activator, n, string.Join(", ", names))
                : string.Format(CultureInfo.CurrentCulture, Resources.Run_ActivatorUnused, n);
            var box = new ExCheckBox { AutoSize = true, Margin = new Padding(3, 3, 3, 3), Text = text, Checked = active.Contains(n), Tag = n };
            box.CheckedChanged += (_, _) => Edit(c =>
            {
                var list = c.ActivatorList.ToList();
                list.Remove(n);
                if (box.Checked) list.Add(n);
                c.ActivatorList = list;
            }, false);
            flpActivators.Controls.Add(box);
        }

        FormUtils.ChangeStyleOfControls(_ctx.UsingStyle, flpActivators.Controls);
        flpActivators.ResumeLayout(true);
    }

    /// <summary>
    /// Zmena vlastnosti vybranej konfiguracie z prvku okna.
    /// </summary>
    /// <param name="change">uprava konfiguracie</param>
    /// <param name="rebuildTree">meni sa nazov alebo ulozisko - prestavat zoznam</param>
    private void Edit(Action<RunConfiguration> change, bool rebuildTree)
    {
        if (_updating || _current is null) return;
        change(_current);
        _dirty = true;
        if (rebuildTree)
        {
            var node = NodeOf(_current);
            var group = _current.Shared ? 1 : 0;
            if (node is not null && tvConfigs.Nodes.IndexOf(node.Parent) == group)
                node.Text = NodeText(_current);
            else
            {
                // presun medzi skupinami - prestavat strom, detail ostava (fokus v prvku)
                var current = _current;
                _updating = true;
                BuildTree(current.Id);
                _updating = false;
                _current = current;
            }
        }

        UpdateDerived();
    }

    /// <summary>
    /// Odvodene udaje vybranej konfiguracie: vetva registra, prikazovy riadok a zistenia.
    /// </summary>
    private void UpdateDerived()
    {
        if (_current is not { } config) return;
        var app = config.Program.Length > 0 ? RunConfigurations.AppName(config) : config.Registry.Trim();
        var exists = _branches.Contains(app, StringComparer.OrdinalIgnoreCase);
        lBranchInfo.Text = app.Length == 0 ? ""
            : string.Format(CultureInfo.CurrentCulture, config.Registry.Trim().Length == 0 ? Resources.Run_BranchByProgram : Resources.Run_BranchByArgument, app)
              + "  ·  " + (exists ? Resources.Run_BranchExists : Resources.Run_BranchMissing);
        tbCommandLine.Text = RunConfigurations.CommandLine(config);
        var node = NodeOf(config);
        if (node is not null) node.ToolTipText = tbCommandLine.Text;

        var checks = RunConfigurationChecks.Check(config, _environment with { Others = _items });
        ShowChecks(checks);
        bInissSettings.Enabled = app.Length > 0;
        bLogs.Enabled = config.Program.Length > 0;
        _toolTip.SetToolTip(bLogs, config.Program.Length > 0 ? LogsText(config) : null);
    }

    private void ClearChecks()
    {
        foreach (Control c in flpChecks.Controls.Cast<Control>().ToList())
        {
            flpChecks.Controls.Remove(c);
            c.Dispose();
        }
    }

    private void ShowChecks(List<RunCheck> checks)
    {
        flpChecks.SuspendLayout();
        ClearChecks();
        foreach (var check in checks)
        {
            var row = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false, Margin = new Padding(0, 0, 0, 2) };
            var icon = check.Severity switch
            {
                RunCheckSeverity.Error => ErrorIcon,
                RunCheckSeverity.Warning => WarningIcon,
                _ => InfoIcon
            };
            row.Controls.Add(new PictureBox { Image = icon, SizeMode = PictureBoxSizeMode.CenterImage, Size = new Size(20, 20), Margin = new Padding(3, 0, 3, 0) });
            row.Controls.Add(new Label { AutoSize = true, MaximumSize = new Size(ChecksTextWidth(), 0), Margin = new Padding(0, 3, 3, 0), Text = check.Text });
            flpChecks.Controls.Add(row);
        }

        FormUtils.ChangeStyleOfControls(_ctx.UsingStyle, flpChecks.Controls);
        flpChecks.ResumeLayout(true);
    }

    /// <summary>
    /// Sirka textu upozornenia: sirka detailu bez okrajov, ikony a rezervy na posuvnik.
    /// </summary>
    private int ChecksTextWidth()
    {
        var used = tlpDetail.Padding.Horizontal + flpChecks.Margin.Horizontal + LogicalToDeviceUnits(26 + 12) + SystemInformation.VerticalScrollBarWidth;
        return Math.Max(LogicalToDeviceUnits(200), pDetail.ClientSize.Width - used);
    }

    private void FitChecks()
    {
        var width = ChecksTextWidth();
        flpChecks.SuspendLayout();
        foreach (var label in flpChecks.Controls.Cast<Control>().SelectMany(r => r.Controls.OfType<Label>()))
            label.MaximumSize = new Size(width, 0);
        flpChecks.ResumeLayout(true);
    }

    // --- akcie ---

    private void ShowInissSettings()
    {
        if (_current is null) return;
        using var f = new FInissSettings(_ctx, _iniss, _dialogs, _current);
        f.ShowDialog(this);
    }

    private void OpenLogs()
    {
        if (_current is null) return;
        var (dir, _) = InissEnvironment.LogDirectory(Resolved(_current), _ctx.Workspace.INISSDir);

        if (!Directory.Exists(dir))
        {
            _dialogs.ShowWarning(string.Format(CultureInfo.CurrentCulture, Resources.Run_LogsMissing, dir));
            return;
        }

        Utils.OpenShell(dir);
    }

    /// <summary>
    /// Nastavenia INISSu konfiguracie (z vyrovnavacej pamate okna).
    /// </summary>
    private ResolvedConfig Resolved(RunConfiguration config)
    {
        var key = string.Join("|", RunConfigurations.AppName(config), config.RunAsAdmin, config.Program).ToUpperInvariant();
        if (!_resolved.TryGetValue(key, out var resolved))
            _resolved[key] = resolved = InissEnvironment.Resolve(config, _ctx.Workspace.INISSDir);
        return resolved;
    }

    /// <summary>
    /// Bublina tlacidla Priecinok logov: kam INISS tejto konfiguracie loguje a odkial to vie.
    /// </summary>
    private string LogsText(RunConfiguration config)
    {
        var (dir, setting) = InissEnvironment.LogDirectory(Resolved(config), _ctx.Workspace.INISSDir);
        var source = setting is null ? Resources.Run_LogsDefault : InissSettingsModel.SourceText(setting.Source);
        return string.Format(CultureInfo.CurrentCulture, config.RunAsAdmin ? Resources.Run_LogsToolTipAdmin : Resources.Run_LogsToolTip, dir, source);
    }

    /// <summary>
    /// Overi nazvy a programy; pri chybe vyberie konfiguraciu a okno nezavrie.
    /// </summary>
    private void Accept(bool run)
    {
        var names = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
        foreach (var item in _items.OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase))
        {
            var problem = item.Name.Length == 0 ? Resources.Run_NameEmpty
                : !names.Add(item.Name) ? string.Format(CultureInfo.CurrentCulture, Resources.Run_NameDuplicate, item.Name)
                : item.Program.Length == 0 ? string.Format(CultureInfo.CurrentCulture, Resources.Run_ProgramEmpty, item.Name)
                : null;
            if (problem is null) continue;

            tvConfigs.SelectedNode = NodeOf(item);
            _dialogs.ShowWarning(problem);
            return;
        }

        RunAfterSave = run && _current is not null;
        SelectedId = _current?.Id ?? SelectedId;
        DialogResult = DialogResult.OK;
    }

    /// <inheritdoc />
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (DialogResult != DialogResult.OK && _dirty && _dialogs.ShowQuestion(Resources.Run_DiscardQuestion) != DialogResult.Yes)
        {
            e.Cancel = true;
            return;
        }

        base.OnFormClosing(e);
    }

    /// <inheritdoc />
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _iniss.StateChanged -= Iniss_StateChanged;
        _groupFont?.Dispose();
        _toolTip.Dispose();
        base.OnFormClosed(e);
    }
}
