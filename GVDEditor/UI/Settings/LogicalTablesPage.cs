using System.Globalization;
using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.TableLogicalRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Logicke tabule v okne Lokalne nastavenia - zoznam tabul a udaje vybranej tabule so zostavou
/// (ktore zaznamy idu na ktoru fyzicku tabulu) s upravou priamo v poliach a v tabulke. Zmeny idu rovno do
/// <see cref="GlobData.TableLogicals" />, Zrusit okna ich vrati.
/// </summary>
public partial class LogicalTablesPage : UserControl, ISettingsPage
{
    private readonly ItemListSupport<TableLogical> _list;
    private readonly FieldMarks _marks = new();
    private readonly List<(TableLogical Table, Field Field, int Row, string Message)> _problems = [];

    // zostava kazdej tabule, ktoru pouzivatel otvoril; null = umiestnenia sa zostavou vyjadrit nedaju (len na citanie)
    private readonly Dictionary<TableLogical, List<TableLogicalSegment>?> _layouts = [];

    // text pola Stanica, ktory nie je platne cislo - do tabule sa nezapisal
    private readonly Dictionary<TableLogical, string> _invalidStation = [];
    private List<TableLogicalSegment> _warningsSource = [];
    private Station? _station;
    private TableLogical? _current;
    private bool _loaded;
    private bool _loading;
    private Color _hintColor;

    /// <summary>
    /// Polozka ponuky stanic - cislo a nazov.
    /// </summary>
    private sealed record StationItem(int Id, string Name)
    {
        public override string ToString() => $"{Id} – {Name}";
    }

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public LogicalTablesPage()
    {
        InitializeComponent();
        dgvZostava.AutoGenerateColumns = false;
        _list = new ItemListSupport<TableLogical>(dgv, tbFilter, () => GlobData.TableLogicals, t => [t.Name, t.Key]);
        _list.SelectionChanged += (_, _) => ShowCurrent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : $"{Label(_problems[0].Table)} – {_problems[0].Message}";

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        var (table, field, row, _) = _problems[0];
        _list.Select(table);
        ShowCurrent();
        if (field == Field.Segment && row < dgvZostava.Rows.Count)
        {
            dgvZostava.Focus();
            dgvZostava.CurrentCell = dgvZostava.Rows[row].Cells[colFirst.Index];
        }
        else
            FieldControl(field).Focus();
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="station">stanica grafikonu - prva v ponuke stanic</param>
    internal void LoadData(Station station)
    {
        _station = station;
        foreach (var header in new[] { lBasic, lZostava, lCommentHeader, lUseHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        lStationNote.ForeColor = lZostavaNote.ForeColor = SystemColors.GrayText;
        _hintColor = lHint.ForeColor;
        _marks.Capture(tbName, tbKey, nudCount);
        if (GlobData.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);
        _list.CaptureColors();
        dgvZostava.BackgroundColor = dgvZostava.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : dgvZostava.DefaultCellStyle.BackColor;

        cbType.Items.AddRange(TableViewType.GetValues().ToArray<object>());
        FillStations();

        _loaded = true;
        _list.Fill(GlobData.TableLogicals.FirstOrDefault());
        Check();
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // fyzicke tabule a kolaje sa mohli zmenit na inych strankach okna
        if (Visible && _loaded)
        {
            _list.Fill(_current);
            Check();
        }
    }

    private static string Label(TableLogical table) => string.IsNullOrWhiteSpace(table.Name) ? table.Key : table.Name;

    /// <summary>
    /// Ponuka stanic: najprv stanica tohto grafikonu, potom stanice ostatnych grafikonov. Pole je editovatelne,
    /// takze sa da zadat aj ine cislo.
    /// </summary>
    private void FillStations()
    {
        var items = new List<StationItem>();

        void AddStation(Station? station)
        {
            if (station == null || !int.TryParse(station.ID, out var id) || id == 0 || items.Any(item => item.Id == id))
                return;
            items.Add(new StationItem(id, station.Name));
        }

        AddStation(_station);
        foreach (var dir in GlobData.GVDDirs)
        {
            try
            {
                AddStation(TxtParser.ReadInfoGVD(dir.FullPath).ThisStation);
            }
            catch (Exception)
            {
                // priecinok bez citatelneho Grafikon.txt - do ponuky sa nedostane
            }
        }

        cbStation.Items.Clear();
        cbStation.Items.AddRange(items.ToArray<object>());
    }

    /// <summary>
    /// Zostava tabule - pri prvom otvoreni tabule sa rozlozi z umiestneni zaznamov.
    /// </summary>
    private List<TableLogicalSegment>? SegmentsOf(TableLogical table)
    {
        if (_layouts.TryGetValue(table, out var layout))
            return layout;

        var segments = TableLogicalLayout.FromRecords(table.Records);
        layout = TableLogicalLayout.IsExpressible(table.Records, segments) ? segments : null;
        _layouts[table] = layout;
        return layout;
    }

    private void ShowCurrent()
    {
        var table = _list.Current;
        _current = table;
        _loading = true;
        pDetail.SuspendLayout();
        try
        {
            tlpDetail.Enabled = table is not null;
            tbName.Text = table?.Name ?? "";
            tbKey.Text = table?.Key ?? "";
            cbType.SelectedItem = table?.ViewType;
            nudCount.Value = Math.Clamp(table?.Records.Count ?? 0, nudCount.Minimum, nudCount.Maximum);
            ShowStation(table);
            tbComment.Text = table?.Comment ?? "";

            cbAddPhysical.BeginUpdate();
            cbAddPhysical.Items.Clear();
            cbAddPhysical.Items.AddRange(GlobData.TablePhysicals.ToArray<object>());
            if (cbAddPhysical.Items.Count > 0)
                cbAddPhysical.SelectedIndex = 0;
            cbAddPhysical.EndUpdate();

            FillZostava(0);
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        var editable = table is not null && SegmentsOf(table) is not null;
        nudCount.Enabled = editable;
        lZostavaNote.Text = table is null ? ""
            : editable ? Resources.LogicalTablesPage_Zostava
            : Resources.FTableLogical_Zostava_nevyjadriteľná;
        bDuplicate.Enabled = table is not null;
        ShowUsage();
        MarkProblems();
        ShowHint();
    }

    private void ShowStation(TableLogical? table)
    {
        if (table is not null && _invalidStation.TryGetValue(table, out var text))
        {
            cbStation.SelectedItem = null;
            cbStation.Text = text;
            return;
        }

        // IDSTATION: 0 = neuvedene (prazdne pole), inak stanica z ponuky alebo vlastne cislo
        var id = table?.IdStation ?? 0;
        var match = cbStation.Items.Cast<StationItem>().FirstOrDefault(item => item.Id == id);
        cbStation.SelectedItem = match;
        if (match is null)
            cbStation.Text = id == 0 ? "" : id.ToString(CultureInfo.InvariantCulture);
    }

    private IReadOnlyList<string> Usage(TableLogical table) =>
        GlobData.Tracks.Where(track => track.Tables.Any(t => ReferenceEquals(t, table)))
            .Select(track => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Kolaj, track.Name))
            .ToList();

    private void ShowUsage()
    {
        var usage = _current is null ? [] : Usage(_current);
        lUse.Text = UsageText.Format(usage, true, Resources.TablesPage_Nepouziva);
        bDelete.Enabled = _current is not null && usage.Count == 0;
    }

    // ---------------------------------------------------------------- zostava

    private void FillZostava(int select)
    {
        var loading = _loading;
        _loading = true;
        dgvZostava.Rows.Clear();
        var layout = _current is null ? null : SegmentsOf(_current);
        var segments = layout ?? (_current is null ? [] : TableLogicalLayout.FromRecords(_current.Records));
        foreach (var segment in segments)
        {
            var row = dgvZostava.Rows[dgvZostava.Rows.Add(segment.Table?.Name ?? "", segment.FirstRecord, segment.LastRecord, segment.StartRow)];
            row.Tag = segment;
            SetTypeCell(row, segment);
        }

        dgvZostava.ReadOnly = layout is null;
        _loading = loading;
        if (select >= 0 && select < dgvZostava.Rows.Count)
            dgvZostava.CurrentCell = dgvZostava.Rows[select].Cells[colFirst.Index];
        UpdateZostavaButtons();
    }

    /// <summary>
    /// Bunke typu zobrazenia ponukne len typy, ktore podporuje katalog fyzickej tabule riadku (plus aktualny typ,
    /// ak ho katalog nepodporuje - aby sa dal zobrazit a nestratil sa).
    /// </summary>
    private void SetTypeCell(DataGridViewRow row, TableLogicalSegment segment)
    {
        var types = TableLogicalLayout.SupportedViewTypes(segment.Table);
        if (types.Count == 0)
            types = TableViewType.GetValues().ToList();
        if (segment.TypeView != null && !types.Contains(segment.TypeView))
            types.Add(segment.TypeView);

        var cell = (DataGridViewComboBoxCell)row.Cells[colTypeView.Index];
        cell.DataSource = types;
        cell.DisplayMember = nameof(TableViewType.Name);
        cell.ValueMember = nameof(TableViewType.This);
        cell.Value = segment.TypeView;
    }

    private void UpdateZostavaButtons()
    {
        var editable = _current is not null && SegmentsOf(_current) is not null;
        cbAddPhysical.Enabled = bAddPhysical.Enabled = editable && cbAddPhysical.Items.Count > 0;
        bSegmentRemove.Enabled = editable && dgvZostava.CurrentRow?.Tag is TableLogicalSegment;
    }

    /// <summary>
    /// Po zmene zostavy zapise umiestnenia zaznamov do tabule.
    /// </summary>
    private void ApplyZostava()
    {
        if (_current is not { } table || SegmentsOf(table) is not { } segments)
            return;

        table.Records = TableLogicalLayout.ToRecords(segments, decimal.ToInt32(nudCount.Value));
        Check();
    }

    private void dgvZostava_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || dgvZostava.Rows[e.RowIndex].Tag is not TableLogicalSegment segment)
            return;

        var value = dgvZostava.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        // nie cislo sa zapise ako 0 - kontrola riadok oznaci ako neplatny
        var number = int.TryParse(Convert.ToString(value, CultureInfo.CurrentCulture), out var n) ? n : 0;
        if (e.ColumnIndex == colFirst.Index)
            segment.FirstRecord = number;
        else if (e.ColumnIndex == colLast.Index)
            segment.LastRecord = number;
        else if (e.ColumnIndex == colStartRow.Index)
            segment.StartRow = number;
        else if (e.ColumnIndex == colTypeView.Index && value is TableViewType type)
            segment.TypeView = type;

        ApplyZostava();
    }

    private void dgvZostava_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // vyber v combo bunke sa ma prejavit hned, nie az po opusteni bunky
        if (dgvZostava.IsCurrentCellDirty && dgvZostava.CurrentCell is DataGridViewComboBoxCell)
            dgvZostava.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvZostava_DataError(object? sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgvZostava_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!_loading && IsHandleCreated)
            BeginInvoke(() =>
            {
                UpdateZostavaButtons();
                ShowHint();
            });
    }

    private void bAddPhysical_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table || SegmentsOf(table) is not { } segments || cbAddPhysical.SelectedItem is not TablePhysical physical)
            return;

        var count = decimal.ToInt32(nudCount.Value);
        if (count < 1)
        {
            Utils.ShowError(Resources.FTableLogical_Najprv_počet_záznamov);
            return;
        }

        if (segments.Any(s => ReferenceEquals(s.Table, physical)) &&
            Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FTableLogical_Tabuľa_už_v_zostave, physical)) !=
            DialogResult.Yes)
            return;

        segments.Add(TableLogicalLayout.NewSegment(physical, count, table.ViewType));
        ApplyZostava();
        FillZostava(segments.Count - 1);
        MarkProblems();
    }

    private void bSegmentRemove_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table || SegmentsOf(table) is not { } segments ||
            dgvZostava.CurrentRow?.Tag is not TableLogicalSegment segment)
            return;

        var index = segments.IndexOf(segment);
        segments.RemoveAt(index);
        ApplyZostava();
        FillZostava(Math.Min(index, segments.Count - 1));
        MarkProblems();
    }

    // ---------------------------------------------------------------- tabula

    private void Field_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table)
            return;

        if (sender == tbName)
        {
            table.Name = tbName.Text.Trim();
            // nazov vidno aj v zozname logickych tabul kolaje (stranka Nastupistia a kolaje)
            GlobData.TableLogicals.ResetItem(GlobData.TableLogicals.IndexOf(table));
        }
        else if (sender == tbKey)
            table.Key = tbKey.Text.Trim();
        else if (sender == tbComment)
            table.Comment = tbComment.Text;
        else if (sender == nudCount)
        {
            if (SegmentsOf(table) is { } segments)
            {
                TableLogicalLayout.Resize(segments, table.Records.Count, decimal.ToInt32(nudCount.Value));
                table.Records = TableLogicalLayout.ToRecords(segments, decimal.ToInt32(nudCount.Value));
                FillZostava(dgvZostava.CurrentRow?.Index ?? 0);
            }
        }

        _list.Refresh(table);
        Check();
    }

    private void cbType_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table || cbType.SelectedItem is not TableViewType type)
            return;

        // novy typ prejde na riadky zostavy s doterajsim typom, ak ho ich fyzicka tabula podporuje
        var oldType = table.ViewType;
        table.ViewType = type;
        if (SegmentsOf(table) is { } segments && TableLogicalLayout.ChangeViewType(segments, oldType, type))
        {
            ApplyZostava();
            FillZostava(dgvZostava.CurrentRow?.Index ?? 0);
        }

        Check();
    }

    private void cbStation_TextChanged(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table)
            return;

        var id = cbStation.SelectedItem is StationItem item && cbStation.Text == item.ToString()
            ? item.Id
            : TableLogicalRules.ParseStation(cbStation.Text);
        if (id is { } value)
        {
            table.IdStation = value;
            _invalidStation.Remove(table);
        }
        else
            _invalidStation[table] = cbStation.Text;

        Check();
    }

    private Control FieldControl(Field field) => field switch
    {
        Field.Key => tbKey,
        Field.Count => nudCount,
        Field.Station => cbStation,
        Field.Segment => dgvZostava,
        _ => tbName
    };

    private void Check()
    {
        _problems.Clear();
        var tables = GlobData.TableLogicals.ToList();
        for (var i = 0; i < tables.Count; i++)
        {
            var layout = _layouts.GetValueOrDefault(tables[i]);
            foreach (var (field, row, message) in TableLogicalRules.Check(tables, i, layout))
                _problems.Add((tables[i], field, row, message));
            if (_invalidStation.ContainsKey(tables[i]))
                _problems.Add((tables[i], Field.Station, -1, Resources.FTableLogical_Neplatné_číslo_stanice));
        }

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        _list.MarkProblems(table => _problems.FirstOrDefault(p => ReferenceEquals(p.Table, table)).Message);
        var own = _problems.Where(p => ReferenceEquals(p.Table, _current)).ToList();
        _marks.Mark(own.Select(p => FieldControl(p.Field)));

        _warningsSource = _current is null ? [] : SegmentsOf(_current) ?? [];
        var warnings = TableLogicalRules.Warnings(_warningsSource);
        foreach (DataGridViewRow row in dgvZostava.Rows)
        {
            var error = own.FirstOrDefault(p => p.Field == Field.Segment && p.Row == row.Index).Message;
            row.Cells[colPhysical.Index].ErrorText = error ?? "";
            row.Cells[colPhysical.Index].ToolTipText = string.Join(Environment.NewLine,
                warnings.Where(w => w.Row == row.Index).Select(w => w.Message));
            row.Cells[colPhysical.Index].Style.ForeColor = error is null && warnings.Any(w => w.Row == row.Index)
                ? SettingsWindow.ProblemColor(dgvZostava)
                : Color.Empty;
        }
    }

    // pod udajmi vsetky chyby vybranej tabule, inak upozornenia jej zostavy
    private void ShowHint()
    {
        var problems = _problems.Where(p => ReferenceEquals(p.Table, _current)).Select(p => p.Message).ToList();
        if (problems.Count > 0)
        {
            lHint.Text = string.Join(Environment.NewLine, problems);
            lHint.ForeColor = SettingsWindow.ProblemColor(lHint);
            return;
        }

        var warnings = TableLogicalRules.Warnings(_warningsSource);
        lHint.Text = warnings.Count == 0 ? ""
            : Resources.LogicalTablesPage_Upozornenia + Environment.NewLine + string.Join(Environment.NewLine, warnings.Select(w => w.Message));
        lHint.ForeColor = _hintColor;
    }

    // ---------------------------------------------------------------- zoznam

    private void bAdd_Click(object? sender, EventArgs e)
    {
        var name = TableRules.Unique(GlobData.TableLogicals.Select(t => t.Name), Resources.TablesPage_Nova_tabula);
        var table = new TableLogical
        {
            Name = name,
            Key = TableRules.Unique(GlobData.TableLogicals.Select(t => t.Key), name),
            ViewType = TableViewType.Odchodova,
            TypeViewFlags = "",
            Comment = "",
            Records = TableLogicalLayout.ToRecords([], 1)
        };

        GlobData.TableLogicals.Add(table);
        Added(table);
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var table = new TableLogical
        {
            Name = TableRules.Unique(GlobData.TableLogicals.Select(t => t.Name), source.Name),
            Key = TableRules.Unique(GlobData.TableLogicals.Select(t => t.Key), source.Key),
            ViewType = source.ViewType,
            TypeViewFlags = source.TypeViewFlags,
            IdStation = source.IdStation,
            Comment = source.Comment,
            Records = TableLogicalLayout.CloneRecords(source.Records)
        };

        GlobData.TableLogicals.Insert(GlobData.TableLogicals.IndexOf(source) + 1, table);
        Added(table);
    }

    private void Added(TableLogical table)
    {
        _list.Fill(table);
        ShowCurrent();
        Check();
        tbName.Focus();
        tbName.SelectAll();
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table || Usage(table).Count > 0)
            return;

        var index = GlobData.TableLogicals.IndexOf(table);
        GlobData.TableLogicals.RemoveAt(index);
        _layouts.Remove(table);
        _invalidStation.Remove(table);
        _list.Fill(GlobData.TableLogicals.Count == 0 ? null : GlobData.TableLogicals[Math.Min(index, GlobData.TableLogicals.Count - 1)]);
        Check();
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
