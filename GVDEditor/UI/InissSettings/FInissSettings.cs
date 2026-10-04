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
/// Okno Nastavenia INISSu - nastavenia INISSu v registri a v subore .INI vedla programu. Pri kazdej hodnote ukaze
/// ucinnu hodnotu (tak, ako ju INISS nacita pri starte), odkial pochadza a co lezi v jednotlivych vrstvach; zmeny
/// zapise tam, kde budu mat ucinok (HKLM cez jednu vyzvu UAC, HKCU priamo, .INI).
/// </summary>
internal partial class FInissSettings : Form
{
    private static readonly object WarningsNode = new();

    // moderne ikony systemu ako v analyze grafikonu, nie stare SystemIcons
    private static readonly Bitmap InfoIcon = StockIcon(ShellIconType.Info);
    private static readonly Bitmap WarningIcon = StockIcon(ShellIconType.Warning);
    private static readonly Bitmap ErrorIcon = StockIcon(ShellIconType.Error);

    private readonly EditorContext _ctx;
    private readonly IInissProcess _iniss;
    private readonly IDialogService _dialogs;
    private readonly SettingDetail _detail = new() { Dock = DockStyle.Fill };
    private InissSettingsModel? _model;
    private readonly IReadOnlyList<InissTable> _tables;
    private DriverLineMap _lines = new([], []);
    private string _appName = "";
    private string? _exePath;
    private InissRunMode _runMode;
    // zobrazena konfiguracia spustania; null = ina vetva registra (program, vetva a rezim sa vyberaju rucne)
    private RunConfiguration? _runConfig;
    // konfiguracie v zozname (moze medzi nimi byt neulozena kopia z okna Konfiguracie spustania)
    private readonly List<RunConfiguration> _configs;
    private bool _loading;
    private bool _saving;
    private bool _updatingGrid;
    private readonly HashSet<Control> _hookedEditors = [];

    /// <summary>
    /// Vytvori okno pre otvorenu instalaciu INISSu.
    /// </summary>
    /// <param name="ctx">kontext editora (instalacia, konfiguracie spustania INISSu)</param>
    /// <param name="iniss">beziace INISSy - stav a restart po ulozeni</param>
    /// <param name="dialogs">dialogy</param>
    /// <param name="runConfig">konfiguracia spustania, ktorej nastavenia sa zobrazia (null = vybrana)</param>
    public FInissSettings(EditorContext ctx, IInissProcess iniss, IDialogService dialogs, RunConfiguration? runConfig = null)
    {
        _ctx = ctx;
        _iniss = iniss;
        _dialogs = dialogs;
        InitializeComponent();
        dgvValues.AutoGenerateColumns = false;
        // fyzicke tabule vsetkych grafikonov v poradi, v akom ich INISS indexuje (Tables\…<N>)
        _tables = InissTableMap.Build(_ctx.Workspace);
        pDetail.Controls.Add(_detail);
        cState.DefaultCellStyle.NullValue = null;
        this.ApplyThemeAndFonts();
        // prazdna plocha pod riadkami v farbe buniek, nie sivej plochy okna
        dgvValues.BackgroundColor = dgvValues.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : dgvValues.DefaultCellStyle.BackColor;

        _loading = true;
        try
        {
            // instalacia uz ponuka len programy INISS (pomocne exe vynecha)
            foreach (var exe in _ctx.Workspace.INISSExeFiles) cbProgram.Items.Add(exe);
            cbRunMode.Items.AddRange([Resources.InissSettings_RunNormal, Resources.InissSettings_RunElevated]);
            cbRunMode.SelectedIndex = 0;
            // konfiguracie spustania urcuju program, vetvu aj rezim; posledna polozka = ina vetva registra
            runConfig ??= _ctx.RunConfigurations.Selected;
            _configs = _ctx.RunConfigurations.Items.Select(i => i.Id == runConfig?.Id ? runConfig : i).ToList();
            if (runConfig is not null && !_configs.Contains(runConfig)) _configs.Add(runConfig);
            foreach (var config in _configs) cbRunConfig.Items.Add(new RunConfigItem(config));
            cbRunConfig.Items.Add(Resources.InissSettings_OtherBranch);
            var index = runConfig is null ? -1 : _configs.IndexOf(runConfig);
            cbRunConfig.SelectedIndex = index >= 0 ? index : cbRunConfig.Items.Count - 1;
            ApplyRunConfig();
        }
        finally
        {
            _loading = false;
        }

        cbRunConfig.SelectedIndexChanged += (_, _) =>
        {
            if (_loading) return;
            _loading = true;
            ApplyRunConfig();
            _loading = false;
            Reload(true);
        };
        cbProgram.SelectedIndexChanged += (_, _) =>
        {
            if (_loading) return;
            _loading = true;
            FillConfigs(AppNameForProgram());
            _loading = false;
            Reload(true);
        };
        cbConfig.SelectedIndexChanged += (_, _) => Reload(true);
        cbRunMode.SelectedIndexChanged += (_, _) => Reload(true);
        bReload.Click += (_, _) => Reload(true);
        tvSections.AfterSelect += (_, _) => FillGrid();
        tbSearch.TextChanged += (_, _) => FillGrid();
        cboxChangedOnly.CheckedChanged += (_, _) => FillGrid();
        cboxNotRead.CheckedChanged += (_, _) => FillGrid();
        // CurrentCellChanged - pri SelectionChanged z kodu este CurrentRow ukazuje na povodny riadok
        dgvValues.CurrentCellChanged += (_, _) => ShowDetail();
        dgvValues.CellBeginEdit += Grid_CellBeginEdit;
        dgvValues.CellValidating += Grid_CellValidating;
        dgvValues.CellEndEdit += Grid_CellEndEdit;
        dgvValues.CellValueChanged += Grid_CellValueChanged;
        dgvValues.CurrentCellDirtyStateChanged += Grid_CurrentCellDirtyStateChanged;
        dgvValues.CellClick += Grid_CellClick;
        dgvValues.CellDoubleClick += Grid_CellDoubleClick;
        dgvValues.CellPainting += Grid_CellPainting;
        dgvValues.DataError += (_, e) => e.ThrowException = false;
        dgvValues.EditingControlShowing += (_, e) =>
        {
            // zoznam hodnot je editovatelny - da sa napisat aj cislo, ktore v zozname nie je
            if (e.Control is not DataGridViewComboBoxEditingControl combo) return;
            combo.DropDownStyle = ComboBoxStyle.DropDown;
            if (!_hookedEditors.Add(combo)) return;
            // DataGridView sleduje len zmenu vyberu - pisanie treba oznacit samo; vyber zo zoznamu sa potvrdi hned
            combo.TextChanged += (_, _) =>
            {
                if (dgvValues.IsCurrentCellInEditMode && dgvValues.EditingControl == combo) dgvValues.NotifyCurrentCellDirty(true);
            };
            combo.SelectionChangeCommitted += (_, _) => dgvValues.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        tbSearch.KeyDown += (_, e) =>
        {
            // Esc vymaze hladanie (okno Esc nezatvara - neulozene zmeny by sa lahko stratili)
            if (e.KeyCode != Keys.Escape || tbSearch.Text.Length == 0) return;
            tbSearch.Text = "";
            e.SuppressKeyPress = true;
        };
        _detail.Edited += Detail_Edited;
        _detail.Reverted += (_, _) => RevertSelected();
        bSave.Click += async (_, _) => await SaveAsync(false);
        bSaveRestart.Click += async (_, _) => await SaveAsync(true);
        bDiscard.Click += (_, _) => DiscardAll();
        bAddLine.Click += async (_, _) => await AddLineAsync();
        bRemoveLine.Click += async (_, _) => await RemoveLineAsync();
        bClose.Click += (_, _) => Close();
        _iniss.StateChanged += Iniss_StateChanged;
        tlpGrid.SizeChanged += (_, _) => lSection.MaximumSize = new Size(Math.Max(200, tlpGrid.ClientSize.Width - 12), 0);

        Reload(false);
    }

    /// <inheritdoc />
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // deliace ciary az ked maju kontajnery skutocnu velkost (z navrhu sa pri FixedPanel neuplatnia)
        scMain.SplitterDistance = LogicalToDeviceUnits(230);
        scRight.SplitterDistance = Math.Max(scRight.Panel1MinSize, scRight.Height * 55 / 100);
    }

    private SettingRow? SelectedRow => dgvValues.CurrentRow?.Tag as SettingRow;

    private static Bitmap StockIcon(ShellIconType type)
    {
        using var icon = new ShellIcon(type, ShellIconSize.Small);
        return icon.ToBitmap();
    }

    /// <summary>Polozka zoznamu konfiguracii spustania.</summary>
    private sealed record RunConfigItem(RunConfiguration Config)
    {
        public override string ToString() => Config.Name;
    }

    /// <summary>
    /// Program, vetvu a rezim nastavi podla vybranej konfiguracie spustania (a zamkne ich); pri inej vetve ich
    /// necha na vyber.
    /// </summary>
    private void ApplyRunConfig()
    {
        _runConfig = (cbRunConfig.SelectedItem as RunConfigItem)?.Config;
        var manual = _runConfig is null;
        cbProgram.Enabled = manual && cbProgram.Items.Count > 0;
        cbConfig.Enabled = manual;
        cbRunMode.Enabled = manual;
        if (_runConfig is not { } config)
        {
            if (cbProgram.SelectedIndex < 0 && cbProgram.Items.Count > 0) cbProgram.SelectedIndex = 0;
            FillConfigs(cbConfig.SelectedItem as string ?? AppNameForProgram());
            return;
        }

        if (config.Program.Length > 0 && !cbProgram.Items.Contains(config.Program)) cbProgram.Items.Add(config.Program);
        cbProgram.SelectedItem = config.Program.Length > 0 ? config.Program : null;
        cbRunMode.SelectedIndex = config.RunAsAdmin ? 1 : 0;
        FillConfigs(config.Program.Length > 0 ? RunConfigurations.AppName(config) : config.Registry.Trim());
    }

    /// <summary>Nazov vetvy pre vybrany program (bez /Reg:).</summary>
    private string AppNameForProgram() => cbProgram.SelectedItem is string exe ? InissRegistry.AppNameFor(exe, null) : "";

    private void FillConfigs(string select)
    {
        cbConfig.Items.Clear();
        var names = InissRegistry.AppNames().ToList();
        if (select.Length > 0 && !names.Contains(select, StringComparer.OrdinalIgnoreCase)) names.Insert(0, select);
        foreach (var n in names) cbConfig.Items.Add(n);
        var index = names.FindIndex(n => string.Equals(n, select, StringComparison.OrdinalIgnoreCase));
        cbConfig.SelectedIndex = index >= 0 ? index : names.Count > 0 ? 0 : -1;
    }

    // --- nacitanie ---

    private void Reload(bool askDiscard)
    {
        if (_loading) return;
        var app = cbConfig.SelectedItem as string ?? "";
        var exe = cbProgram.SelectedItem is string file ? PathUtils.CombinePath(_ctx.Workspace.INISSDir, file) : null;
        var mode = cbRunMode.SelectedIndex == 1 ? InissRunMode.Elevated : InissRunMode.Normal;
        if (askDiscard && _model?.Pending.Count > 0 && _dialogs.ShowQuestion(Resources.InissSettings_DiscardQuestion) != DialogResult.Yes)
        {
            // vratit vyber, ktory zodpoveda zobrazenym datam
            _loading = true;
            cbRunConfig.SelectedIndex = _runConfig is null ? cbRunConfig.Items.Count - 1 : _configs.IndexOf(_runConfig);
            ApplyRunConfig();
            cbConfig.SelectedItem = _appName;
            cbRunMode.SelectedIndex = _runMode == InissRunMode.Elevated ? 1 : 0;
            cbProgram.SelectedItem = _exePath is null ? null : Path.GetFileName(_exePath);
            _loading = false;
            return;
        }

        _appName = app;
        _exePath = exe;
        _runMode = mode;
        if (app.Length == 0)
        {
            _model = null;
            _lines = new DriverLineMap([], []);
            BuildTree();
            UpdateInfo();
            return;
        }

        UseWaitCursor = true;
        try
        {
            var source = InissRegistry.LoadSource(app, exe, mode, _tables.ToDictionary(t => t.Index, t => t.ToInfo()));
            _model = new InissSettingsModel(RegResolver.Resolve(source), exe is null ? null : InissRegistry.IniPathFor(exe), InissRegistry.CanWriteMachine(app));
            _lines = DriverLines.Build(_model.Config, _tables);
        }
        finally
        {
            UseWaitCursor = false;
        }

        BuildTree();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        var parts = new List<string>();
        if (_model is { } m)
        {
            var src = m.Config.Source;
            parts.Add(src.Version is { } v ? string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Info_Version, v) : Resources.InissSettings_Info_VersionUnknown);
            if (!src.Machine.Exists && !src.User.Exists && !src.VirtualStore.Exists) parts.Add(Resources.InissSettings_Info_NoConfig);
            parts.Add(m.Config.UserBranchActive ? Resources.InissSettings_Info_UserBranch : Resources.InissSettings_Info_NoUserBranch);
            if (src.Ini is not null) parts.Add(Resources.InissSettings_Info_Ini);
            if (src.RunMode == InissRunMode.Normal && src.VirtualStore.Exists) parts.Add(Resources.InissSettings_Info_VirtualStore);
        }

        if (RunningInstances().Count > 0) parts.Add(Resources.InissSettings_Info_Running);
        lInfo.Text = string.Join("  ·  ", parts);
        UpdateChanges();
    }

    private void Iniss_StateChanged(object? sender, EventArgs e)
    {
        if (IsDisposed) return;
        UpdateInfo();
    }

    // --- strom a tabulka ---

    private void BuildTree()
    {
        var selected = tvSections.SelectedNode?.Tag switch
        {
            ResolvedSection s => s.Name,
            RegGroup g => g.ToString(),
            _ => null
        };
        tvSections.BeginUpdate();
        try
        {
            tvSections.Nodes.Clear();
            if (_model is not { } m) return;
            var problems = m.AllRows().Count(r => r.Severity >= RegSeverity.Warning);
            if (problems > 0)
                tvSections.Nodes.Add(new TreeNode(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_WarningsNode, problems)) { Tag = WarningsNode });
            foreach (var group in Enum.GetValues<RegGroup>())
            {
                var sections = m.Config.Sections.Where(s => s.Definition.Group == group).ToList();
                if (sections.Count == 0) continue;
                var node = new TreeNode(InissSettingsModel.GroupText(group)) { Tag = group };
                foreach (var s in sections)
                {
                    // linka: "Driver0 – linka 2 · ELEN"
                    var line = _lines.Lines.FirstOrDefault(l => l.Section == s.Name);
                    node.Nodes.Add(new TreeNode(line is null ? s.Name : $"{s.Name} – {line.Summary}") { Tag = s });
                }
                tvSections.Nodes.Add(node);
            }

            tvSections.ExpandAll();
            var restore = Flatten(tvSections.Nodes).FirstOrDefault(n => n.Tag switch
            {
                ResolvedSection s => s.Name == selected,
                RegGroup g => g.ToString() == selected,
                _ => false
            });
            tvSections.SelectedNode = restore ?? (tvSections.Nodes.Count > 0 ? tvSections.Nodes[0] : null);
            if (tvSections.SelectedNode is not null) tvSections.SelectedNode.EnsureVisible();
        }
        finally
        {
            tvSections.EndUpdate();
        }

        FillGrid();
    }

    private static IEnumerable<TreeNode> Flatten(System.Collections.IEnumerable nodes)
    {
        foreach (TreeNode n in nodes)
        {
            yield return n;
            foreach (var c in Flatten(n.Nodes)) yield return c;
        }
    }

    private void FillGrid()
    {
        var selectedKey = SelectedRow?.Key;
        dgvValues.Rows.Clear();
        if (_model is not { } m)
        {
            lSection.Text = "";
            _detail.Clear();
            return;
        }

        IEnumerable<SettingRow> rows;
        var search = tbSearch.Text.Trim();
        if (search.Length > 0)
        {
            rows = m.AllRows().Where(r => InissSettingsModel.Matches(r, search));
            lSection.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_SearchResults, search);
        }
        else
        {
            switch (tvSections.SelectedNode?.Tag)
            {
                case ResolvedSection s:
                    rows = InissSettingsModel.RowsOf(s);
                    lSection.Text = SectionText(s);
                    break;
                case RegGroup g:
                    rows = m.Config.Sections.Where(s => s.Definition.Group == g).SelectMany(InissSettingsModel.RowsOf);
                    lSection.Text = g == RegGroup.Boards ? JoinLines(InissSettingsModel.GroupText(g), OrphanTablesText()) : InissSettingsModel.GroupText(g);
                    break;
                default:
                    rows = m.AllRows().Where(r => r.Severity >= RegSeverity.Warning);
                    lSection.Text = Resources.InissSettings_WarningsDescription;
                    break;
            }
        }

        if (cboxChangedOnly.Checked)
            rows = rows.Where(r => r.Setting is null || !r.Setting.IsDefault || m.PendingFor(r) is not null);
        if (!cboxNotRead.Checked)
            rows = rows.Where(r => r.Setting is null || r.Setting.IsRead || r.Setting.Layers.Count > 0);
        var multiSection = search.Length > 0 || tvSections.SelectedNode?.Tag is not ResolvedSection;
        foreach (var row in rows)
        {
            var index = dgvValues.Rows.Add();
            var gridRow = dgvValues.Rows[index];
            gridRow.Tag = row;
            var key = multiSection ? $"{row.Section}\\{row.Name}" : row.Name;
            // hodnoty jednotlivych tabul: "Enabled3 – Prichody (Kosice.2025)"
            gridRow.Cells[cName.Index].Value = row.Setting?.Table is { } table ? $"{key} – {table.Name} ({table.Grafikon})" : key;
            gridRow.Cells[cValue.Index] = CreateValueCell(row);
            // ReadOnly sa da nastavit az bunke, ktora uz je v riadku
            if (InissSettingsModel.KindOf(row) == CellKind.ReadOnly) gridRow.Cells[cValue.Index].ReadOnly = true;
            UpdateGridRow(gridRow);
        }

        UpdateLineButtons();
        var restore = dgvValues.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (r.Tag as SettingRow)?.Key == selectedKey);
        if (restore is not null) dgvValues.CurrentCell = restore.Cells[cName.Index];
        ShowDetail();
    }

    /// <summary>Bunka hodnoty podla toho, ako sa hodnota upravuje.</summary>
    private static DataGridViewCell CreateValueCell(SettingRow row)
    {
        switch (InissSettingsModel.KindOf(row))
        {
            case CellKind.Check:
                return new DataGridViewCheckBoxCell { Style = { Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding(4, 0, 0, 0) } };
            case CellKind.Choice:
                var combo = new DataGridViewComboBoxCell { DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing };
                combo.Items.AddRange(InissSettingsModel.ChoiceItems(row.Setting!.Setting).Cast<object>().ToArray());
                return combo;
            default:
                return new DataGridViewTextBoxCell();
        }
    }

    /// <summary>Hodnota, ktoru riadok ukazuje: neulozena zmena, pri obnoveni predvolena, inak ucinna.</summary>
    private static object? ShownValue(SettingRow row, PendingChange? pending) =>
        pending is null ? row.Setting?.Value : pending.Value ?? row.Setting?.DefaultValue;

    private void UpdateGridRow(DataGridViewRow gridRow)
    {
        if (gridRow.Tag is not SettingRow row || _model is not { } m) return;
        _updatingGrid = true;
        try
        {
            var pending = m.PendingFor(row);
            var setting = row.Setting;
            var shown = ShownValue(row, pending);
            var cell = gridRow.Cells[cValue.Index];
            switch (cell)
            {
                case DataGridViewCheckBoxCell:
                    cell.Value = shown is int n && n != 0;
                    break;
                case DataGridViewComboBoxCell combo:
                    var text = InissSettingsModel.Format(shown, setting?.Setting);
                    if (!combo.Items.Contains(text)) combo.Items.Add(text);
                    cell.Value = text;
                    break;
                default:
                    cell.Value = setting is null
                        ? string.Join(", ", row.Extra.Select(l => InissSettingsModel.Format(l.Raw, null)).Distinct())
                        : pending is { Value: null } ? Resources.InissSettings_PendingReset : InissSettingsModel.Format(shown, setting.Setting);
                    break;
            }

            gridRow.Cells[cSource.Index].Value = pending is not null
                ? "→ " + (pending.Value is null ? Resources.InissSettings_Source_Default
                    : InissSettingsModel.ShortLocationText(pending.Target == RegWriteTarget.Ini ? RegLocation.Ini : setting?.RegistryLocation ?? RegLocation.Machine))
                : setting is not null ? InissSettingsModel.SourceText(setting.Source, true)
                : string.Join(", ", row.Extra.Select(l => InissSettingsModel.ShortLocationText(l.Location)).Distinct());
            gridRow.Cells[cDefault.Index].Value = setting is null ? "—"
                : setting.DefaultValue is null ? setting.Setting.DynamicDefaultText ?? "—" : InissSettingsModel.Format(setting.DefaultValue, setting.Setting);

            var state = gridRow.Cells[cState.Index];
            state.Value = row.Severity switch
            {
                RegSeverity.Error => ErrorIcon,
                RegSeverity.Warning => WarningIcon,
                RegSeverity.Info => InfoIcon,
                _ => null
            };
            state.ToolTipText = string.Join(Environment.NewLine, row.Diagnostics.Select(d => d.Message));
            gridRow.DefaultCellStyle.Font = pending is not null ? new Font(dgvValues.Font, FontStyle.Bold) : null;
        }
        finally
        {
            _updatingGrid = false;
        }
    }

    // --- uprava v tabulke ---

    private SettingRow? RowAt(int rowIndex) => rowIndex >= 0 ? dgvValues.Rows[rowIndex].Tag as SettingRow : null;

    /// <summary>Text bunky na upravu - cislo bez popisu, farba #RRGGBB (vo zobrazeni je formatovana hodnota).</summary>
    private void Grid_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        if (e.ColumnIndex != cValue.Index || RowAt(e.RowIndex) is not { Setting: { } setting } row || _model is not { } m)
        {
            e.Cancel = true;
            return;
        }

        if (dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex] is not DataGridViewTextBoxCell cell) return;
        _updatingGrid = true;
        cell.Value = InissSettingsModel.EditText(ShownValue(row, m.PendingFor(row)), setting.Setting);
        _updatingGrid = false;
    }

    private void Grid_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex != cValue.Index || !dgvValues.IsCurrentCellInEditMode || RowAt(e.RowIndex) is not { Setting: { } setting }) return;
        var cell = dgvValues.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (cell is not (DataGridViewTextBoxCell or DataGridViewComboBoxCell)) return;
        var text = cell is DataGridViewComboBoxCell && dgvValues.EditingControl is ComboBox box ? box.Text : e.FormattedValue as string;
        var ok = InissSettingsModel.TryParse(text, setting.Setting, out _);
        // napisany text musi byt polozkou zoznamu, inak ho bunka neprijme
        if (ok && cell is DataGridViewComboBoxCell combo && text is not null && !combo.Items.Contains(text))
        {
            combo.Items.Add(text);
            if (dgvValues.EditingControl is ComboBox editor)
            {
                editor.Items.Add(text);
                editor.SelectedItem = text;
            }
        }

        dgvValues.Rows[e.RowIndex].ErrorText = ok ? "" : setting.Setting.Type == RegValueType.Color ? Resources.InissSettings_InvalidColor : Resources.InissSettings_InvalidNumber;
        e.Cancel = !ok;
    }

    private void Grid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        var gridRow = dgvValues.Rows[e.RowIndex];
        gridRow.ErrorText = "";
        if (e.ColumnIndex == cValue.Index && RowAt(e.RowIndex) is { Setting: { } setting } row && gridRow.Cells[e.ColumnIndex] is DataGridViewTextBoxCell cell
            && InissSettingsModel.TryParse(cell.Value as string, setting.Setting, out var value))
            ApplyValue(row, value, gridRow);
        else
            UpdateGridRow(gridRow);
    }

    // zaskrtavacie pole sa zapise hned, nie az pri opusteni bunky
    private void Grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvValues.IsCurrentCellDirty && dgvValues.CurrentCell is DataGridViewCheckBoxCell)
            dgvValues.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void Grid_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_updatingGrid || e.ColumnIndex != cValue.Index || RowAt(e.RowIndex) is not { Setting: { } setting } row) return;
        var gridRow = dgvValues.Rows[e.RowIndex];
        switch (gridRow.Cells[e.ColumnIndex])
        {
            case DataGridViewCheckBoxCell check:
                ApplyValue(row, check.Value is true ? 1 : 0, gridRow);
                break;
            case DataGridViewComboBoxCell combo when InissSettingsModel.TryParse(combo.Value as string, setting.Setting, out var value):
                ApplyValue(row, value, gridRow);
                break;
        }
    }

    // jedno kliknutie do stlpca hodnoty zacne upravu (zaskrtavacie pole sa rovno prepne)
    private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != cValue.Index || dgvValues.IsCurrentCellInEditMode) return;
        if (RowAt(e.RowIndex)?.Setting?.Setting.Type == RegValueType.Color) return;
        dgvValues.BeginEdit(true);
    }

    // farba sa vybera dvojklikom - zadat sa da aj ako #RRGGBB
    private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex != cValue.Index || RowAt(e.RowIndex) is not { Setting: { Setting.Type: RegValueType.Color } } row || _model is not { } m) return;
        dgvValues.EndEdit();
        var current = ShownValue(row, m.PendingFor(row)) as int? ?? RegValues.SystemColor;
        using var dialog = new ColorDialog { FullOpen = true, Color = current == RegValues.SystemColor ? SystemColors.Window : ColorTranslator.FromWin32(current) };
        if (dialog.ShowDialog(this) == DialogResult.OK) ApplyValue(row, ColorTranslator.ToWin32(dialog.Color), dgvValues.Rows[e.RowIndex]);
    }

    // vzorka farby pred textom bunky
    private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != cValue.Index || RowAt(e.RowIndex) is not { Setting: { Setting.Type: RegValueType.Color } } row || _model is not { } m
            || e.Graphics is null || dgvValues.IsCurrentCellInEditMode && dgvValues.CurrentCell?.RowIndex == e.RowIndex)
            return;
        if (ShownValue(row, m.PendingFor(row)) is not int color || color == RegValues.SystemColor) return;
        e.Paint(e.CellBounds, DataGridViewPaintParts.All);
        var size = e.CellBounds.Height - 8;
        var swatch = new Rectangle(e.CellBounds.Right - size - 6, e.CellBounds.Top + 4, size * 2, size);
        swatch.X = e.CellBounds.Right - swatch.Width - 6;
        using var brush = new SolidBrush(ColorTranslator.FromWin32(color));
        e.Graphics.FillRectangle(brush, swatch);
        e.Graphics.DrawRectangle(SystemPens.ControlDark, swatch);
        e.Handled = true;
    }

    private void ApplyValue(SettingRow row, object? value, DataGridViewRow gridRow)
    {
        if (_model is not { } m) return;
        var target = m.PendingFor(row)?.Target ?? InissSettingsModel.DefaultTarget(row);
        m.SetPending(row, value, target, false);
        UpdateGridRow(gridRow);
        _detail.UpdatePending(row, m.PendingFor(row));
        UpdateChanges();
    }

    private void ShowDetail()
    {
        if (SelectedRow is { } row && _model is { } m) _detail.Bind(row, m.PendingFor(row), m);
        else _detail.Clear();
    }

    // --- zmeny ---

    private void Detail_Edited(object? sender, SettingEdit e)
    {
        if (SelectedRow is not { } row || _model is not { } m) return;
        if (e.Reset)
            m.SetPending(row, null, e.Target, true);
        else
        {
            // zmena ciela - hodnota ostava (neulozena alebo ucinna)
            var pending = m.PendingFor(row);
            var value = pending is null ? row.Setting?.Value : pending.Value;
            if (value is null) return;
            m.SetPending(row, value, e.Target, false);
        }

        UpdateGridRow(dgvValues.CurrentRow!);
        _detail.UpdatePending(row, m.PendingFor(row));
        if (e.Reset) ShowDetail();
        UpdateChanges();
    }

    private void RevertSelected()
    {
        if (SelectedRow is not { } row || _model is not { } m) return;
        m.Revert(row);
        UpdateGridRow(dgvValues.CurrentRow!);
        ShowDetail();
        UpdateChanges();
    }

    private void DiscardAll()
    {
        if (_model is not { } m || m.Pending.Count == 0) return;
        if (_dialogs.ShowQuestion(Resources.InissSettings_DiscardQuestion) != DialogResult.Yes) return;
        m.RevertAll();
        FillGrid();
        UpdateChanges();
    }

    // --- linky k tabuliam ---

    /// <summary>Popis sekcie; pri linke aj tabule na nej a zistenia, pri Tables tabule bez ovladaca.</summary>
    private string SectionText(ResolvedSection s)
    {
        var text = s.FromIni ? JoinLines(s.Definition.Description, Resources.InissSettings_SectionFromIni) : s.Definition.Description;
        if (_lines.Lines.FirstOrDefault(l => l.Section == s.Name) is { } line)
        {
            var tables = line.Tables.Count == 0 ? "—"
                : Shorten(line.Tables.Select(t => $"{t.Table.Table.Key}{(t.Automatic ? " (" + Resources.InissSettings_LineAuto + ")" : "")}").ToList());
            text = JoinLines(text, string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineTables, line.Summary, tables));
            text = JoinLines([text, .. line.Problems.Select(p => $"• {InissSettingsModel.SeverityText(p.Severity)}: {p.Text}")]);
        }
        else if (s.Definition.Name is "Tables")
            text = JoinLines(text, OrphanTablesText());

        return text;
    }

    /// <summary>Text o tabuliach, ktorym INISS nic neposle (prazdny, ak take nie su); rovnake dovody spolu.</summary>
    private string OrphanTablesText() =>
        _lines.Unserved.Count == 0 ? ""
            : JoinLines([Resources.InissSettings_TablesWithoutDriver,
                .. _lines.Unserved.GroupBy(u => u.Reason).Select(g => $"• {g.Key}: {Shorten(g.Select(u => u.Table.Table.Key).ToList())}")]);

    /// <summary>Prvych niekolko nazvov a pocet dalsich - dlhy zoznam by odsunul tabulku hodnot.</summary>
    private static string Shorten(List<string> names, int max = 6) =>
        names.Count <= max ? string.Join(", ", names)
            : string.Join(", ", names.Take(max)) + string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_AndMore, names.Count - max);

    private static string JoinLines(params string[] parts) => string.Join(Environment.NewLine, parts.Where(p => p.Length > 0));

    /// <summary>Tlacidla liniek: pridat pri tabuliach a linkach, odstranit pri vybranej linke.</summary>
    private void UpdateLineButtons()
    {
        var tag = tvSections.SelectedNode?.Tag;
        var driver = tag is ResolvedSection { Definition.Name: "Driver" };
        bAddLine.Visible = _model is not null && tbSearch.Text.Trim().Length == 0
                                              && (driver || tag is RegGroup.Boards || tag is ResolvedSection { Definition.Name: "Tables" });
        bRemoveLine.Visible = bAddLine.Visible && driver;
    }

    private async Task AddLineAsync()
    {
        if (_model is not { } m) return;
        if (m.Pending.Count > 0)
        {
            _dialogs.ShowInfo(Resources.InissSettings_PendingFirst);
            return;
        }

        var registryTarget = m.Config.UserBranchActive ? Resources.InissSettings_Target_User
            : m.CanWriteMachine ? Resources.InissSettings_Target_Machine : Resources.InissSettings_Target_MachineUac;
        using var wizard = new FDriverWizard(m.Config, _lines, _tables, InissRegistry.SerialPorts(), registryTarget, m.IniPath is not null);
        if (wizard.ShowDialog(this) != DialogResult.OK) return;
        var section = wizard.Section;
        if (await ApplyAsync(m, RegWritePlanner.Plan(m.Config, wizard.Changes())))
            SelectSection(section);
    }

    private async Task RemoveLineAsync()
    {
        if (_model is not { } m || tvSections.SelectedNode?.Tag is not ResolvedSection { Definition.Name: "Driver" } section) return;
        if (m.Pending.Count > 0)
        {
            _dialogs.ShowInfo(Resources.InissSettings_PendingFirst);
            return;
        }

        var summary = _lines.Lines.FirstOrDefault(l => l.Section == section.Name)?.Summary ?? "";
        if (_dialogs.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_RemoveLineQuestion, section.Name, summary)) != DialogResult.Yes)
            return;
        await ApplyAsync(m, RegWritePlanner.PlanRemoveSection(m.Config, section.Name));
    }

    /// <summary>
    /// Beziace INISSy, ktore citaju zobrazenu vetvu registra.
    /// </summary>
    private List<InissInstance> RunningInstances() => _appName.Length == 0 ? []
        : _iniss.Instances.Where(i => string.Equals(RunConfigurations.AppName(i.Configuration), _appName, StringComparison.OrdinalIgnoreCase)).ToList();

    private void SelectSection(string section)
    {
        var node = Flatten(tvSections.Nodes).FirstOrDefault(n => n.Tag is ResolvedSection s && string.Equals(s.Name, section, StringComparison.OrdinalIgnoreCase));
        if (node is not null) tvSections.SelectedNode = node;
    }

    private void UpdateChanges()
    {
        var count = _model?.Pending.Count ?? 0;
        lChanges.Text = count == 0 ? "" : string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_ChangesCount, count);
        bSave.Enabled = bDiscard.Enabled = count > 0 && !_saving;
        bSaveRestart.Enabled = count > 0 && !_saving && RunningInstances().Any(i => !i.IsRestarting);
    }

    private async Task SaveAsync(bool restart)
    {
        if (_model is not { } m || m.Pending.Count == 0) return;
        var pending = m.Pending.ToList();
        var plan = RegWritePlanner.Plan(m.Config, pending.Select(p => p.ToChange()));
        if (_dialogs.ShowQuestion(Summary(m, pending, plan)) != DialogResult.Yes) return;

        if (!await ApplyAsync(m, plan)) return;
        var running = RunningInstances();
        if (restart && running.Count > 0)
        {
            try
            {
                foreach (var instance in running.Where(i => !i.IsRestarting))
                {
                    var config = _ctx.RunConfigurations.Find(instance.Configuration.Id) ?? instance.Configuration;
                    await _iniss.RestartAsync(instance, InissLaunch.Create(config, _ctx.Workspace.INISSDir),
                        () => _dialogs.ShowQuestion(Resources.FMain_INISS_sa_neukoncil) == DialogResult.Yes);
                }
            }
            catch (InvalidOperationException ex)
            {
                _dialogs.ShowError(ex.Message);
            }
        }
        else if (running.Count > 0)
        {
            _dialogs.ShowInfo(Resources.InissSettings_SavedRestartNeeded);
        }
    }

    /// <summary>
    /// Zapise plan (HKLM pripadne so zvysenim prav) a po uspechu zahodi neulozene zmeny a nacita konfiguraciu znova.
    /// </summary>
    /// <returns>zapis prebehol</returns>
    private async Task<bool> ApplyAsync(InissSettingsModel m, RegWritePlan plan)
    {
        _saving = true;
        UpdateChanges();
        UseWaitCursor = true;
        RegApplyResult result;
        try
        {
            var app = _appName;
            var ini = m.IniPath;
            result = await Task.Run(() => InissRegistry.Apply(plan, app, ini));
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException or System.Security.SecurityException)
        {
            _dialogs.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_SaveError, ex.Message));
            return false;
        }
        finally
        {
            UseWaitCursor = false;
            _saving = false;
            UpdateChanges();
        }

        switch (result)
        {
            case RegApplyResult.Cancelled:
                _dialogs.ShowWarning(Resources.InissSettings_SaveCancelled);
                return false;
            case RegApplyResult.Failed:
                _dialogs.ShowError(Resources.InissSettings_SaveFailed);
                return false;
        }

        m.RevertAll();
        Reload(false);
        return true;
    }

    /// <summary>Suhrn zmien pred zapisom (pred → po, kam).</summary>
    private static string Summary(InissSettingsModel m, List<PendingChange> pending, RegWritePlan plan)
    {
        var sb = new StringBuilder(Resources.InissSettings_SummaryIntro).AppendLine().AppendLine();
        foreach (var p in pending.OrderBy(p => p.Row.Key, StringComparer.OrdinalIgnoreCase))
        {
            var setting = p.Row.Setting;
            var before = setting is null ? Resources.InissSettings_SummaryExtra : InissSettingsModel.Format(setting.Value, setting.Setting);
            var after = p.Value is null ? Resources.InissSettings_PendingReset : InissSettingsModel.Format(p.Value, setting?.Setting);
            var where = p.Value is null ? Resources.InissSettings_SummaryEverywhere
                : p.Target == RegWriteTarget.Ini ? Resources.InissSettings_Source_Ini
                : setting?.RegistryLocation == RegLocation.User ? Resources.InissSettings_Source_User : Resources.InissSettings_Source_Machine;
            sb.Append("• ").Append(p.Row.Section).Append('\\').Append(p.Row.Name).Append(": ").Append(before).Append(" → ").Append(after)
                .Append(" (").Append(where).AppendLine(")");
        }

        var removed = pending.Where(p => p.Value is not null).Select(p => p.Row.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);
        bool Removes(RegLocation location) =>
            plan.Ops.Any(o => o.IsDelete && o.Location == location && removed.Contains(InissSettingsModel.KeyOf(o.Section, o.Name)));
        if (Removes(RegLocation.VirtualStore)) sb.AppendLine().AppendLine(Resources.InissSettings_SummaryVirtualStore);
        if (Removes(RegLocation.Ini)) sb.AppendLine().AppendLine(Resources.InissSettings_SummaryIniRemoved);
        if (plan.WritesMachine && !m.CanWriteMachine) sb.AppendLine().AppendLine(Resources.InissSettings_SummaryUac);
        if (plan.RegistryNotRead.Count > 0) sb.AppendLine().AppendLine(Resources.InissSettings_SummaryNotRead);
        return sb.ToString().TrimEnd();
    }

    /// <inheritdoc />
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_saving)
        {
            e.Cancel = true;
            return;
        }

        if (_model?.Pending.Count > 0 && _dialogs.ShowQuestion(Resources.InissSettings_CloseQuestion) != DialogResult.Yes)
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
        base.OnFormClosed(e);
    }
}
