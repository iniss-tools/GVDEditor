using System.Globalization;
using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Controls;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.TableCatalogRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Katalogove tabule v okne Lokalne nastavenia - zoznam predloh tabul a udaje vybranej predlohy (stlpce
/// s pravitkom, vybrany stlpec, poradie stlpcov, rozmery riadkov) s upravou priamo v poliach a tabulkach. Zmeny idu
/// rovno do <see cref="GlobData.TableCatalogs" />, Zrusit okna ich vrati.
/// </summary>
public partial class CatalogTablesPage : UserControl, ISettingsPage
{
    private readonly ItemListSupport<TableCatalog> _list;
    private readonly FieldMarks _marks = new();
    private readonly List<(TableCatalog Table, Field Field, int Column, string Message)> _problems = [];
    private readonly ToolTip _fontTip = new();
    private readonly TableFontChoice _font;
    private TableCatalog? _current;
    private string? _keyMessage;
    private bool _loaded;
    private bool _loading;
    private bool _columnPending;
    private Color _hintColor;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public CatalogTablesPage()
    {
        InitializeComponent();
        dgvColumns.AutoGenerateColumns = false;
        dgvRows.AutoGenerateColumns = false;
        (components ??= new Container()).Add(_fontTip);
        _font = new TableFontChoice(cbFont, _fontTip, allowColumnDefault: false);
        _font.ValueChanged += (_, _) => Column_Changed(cbFont, EventArgs.Empty);
        _list = new ItemListSupport<TableCatalog>(dgv, tbFilter, () => GlobData.TableCatalogs, t => [t.Name, t.Key]);
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

        var (table, field, column, _) = _problems[0];
        _list.Select(table);
        ShowCurrent();
        switch (field)
        {
            case Field.Column when column < dgvColumns.Rows.Count:
                dgvColumns.Focus();
                dgvColumns.CurrentCell = dgvColumns.Rows[column].Cells[colColName.Index];
                break;
            case Field.Order:
                bColumnOrder.Focus();
                break;
            case Field.Manufacturer:
                cbManufacturer.Focus();
                break;
            case Field.Key:
                tbKey.Focus();
                break;
            default:
                tbName.Focus();
                break;
        }
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    public void LoadData()
    {
        foreach (var header in new[] { lBasic, lColumns, lOrder, lRows, lCommentHeader, lUseHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        lColumn.Font = new Font(Font, FontStyle.Bold);
        foreach (var note in new[] { lDivNote, lOrderNote, lRowsNote })
            note.ForeColor = SystemColors.GrayText;
        _hintColor = lHint.ForeColor;
        _marks.Capture(tbName, tbKey, tbColKey);
        if (GlobData.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);
        _list.CaptureColors();
        foreach (var grid in new[] { dgvColumns, dgvRows })
            grid.BackgroundColor = grid.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : grid.DefaultCellStyle.BackColor;
        ruler.ForeColor = ForeColor;
        _fontTip.SetToolTip(bColUp, Resources.CatalogTablesPage_Hore);
        _fontTip.SetToolTip(bColDown, Resources.CatalogTablesPage_Dole);

        cbManufacturer.Items.AddRange(TableManufacturer.GetValues().ToArray<object>());
        cbFill.Items.AddRange(TableFillSection.GetValues().ToArray<object>());
        cbAlign.Items.AddRange(TableAlign.GetValues().ToArray<object>());
        cbDivType.Items.AddRange(TableDivType.GetValues().ToArray<object>());
        FillTabTabs();

        _loaded = true;
        _list.Fill(_selectAfterLoad ?? GlobData.TableCatalogs.FirstOrDefault());
        Check();
    }

    /// <summary>
    /// Vyberie tabulu (napr. pri oprave z analyzy grafikonu); pred naplnenim stranky az po nom.
    /// </summary>
    public void SelectTable(TableCatalog table)
    {
        if (!_loaded)
        {
            _selectAfterLoad = table;
            return;
        }

        _list.Select(table);
        ShowCurrent();
    }

    private TableCatalog? _selectAfterLoad;

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // sekcie TabTab, fyzicke tabule a texty sa mohli zmenit na inych strankach okna
        if (Visible && _loaded)
        {
            FillTabTabs();
            _list.Fill(_current);
            Check();
        }
    }

    private static string Label(TableCatalog table) => string.IsNullOrWhiteSpace(table.Name) ? table.Key : table.Name;

    private void FillTabTabs()
    {
        var loading = _loading;
        _loading = true;
        foreach (var combo in new[] { cbTab1, cbTab2 })
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            combo.Items.AddRange(TableCatalogEditing.WithEmptyTabTab(GlobData.TabTabs).ToArray<object>());
            combo.EndUpdate();
        }

        _loading = loading;
    }

    private void ShowCurrent()
    {
        var table = _list.Current;
        _current = table;
        _keyMessage = null;
        _loading = true;
        pDetail.SuspendLayout();
        try
        {
            tlpDetail.Enabled = table is not null;
            tbName.Text = table?.Name ?? "";
            tbKey.Text = table?.Key ?? "";
            cbManufacturer.SelectedItem = table?.Manufacturer;
            nudMaxRec.Value = Math.Clamp(table?.MaxRecCount ?? 0, nudMaxRec.Minimum, nudMaxRec.Maximum);
            nudMinHeight.Value = Math.Clamp(table?.MinHeight ?? 0, nudMinHeight.Minimum, nudMinHeight.Maximum);
            tbComment.Text = table?.Comment ?? "";
            _font.Manufacturer = table?.Manufacturer;
            FillColumns(0);
            FillRows();
            ShowOrder();
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        ShowColumn();
        ShowUsage();
        MarkProblems();
        ShowHint();
    }

    // ---------------------------------------------------------------- stlpce

    private TableItem? CurrentColumn => dgvColumns.CurrentRow?.Tag as TableItem;

    private void FillColumns(int select)
    {
        var loading = _loading;
        _loading = true;
        dgvColumns.Rows.Clear();
        foreach (var item in _current?.Items ?? [])
            dgvColumns.Rows[dgvColumns.Rows.Add(item.Name, item.Line + 1, item.Start, item.End)].Tag = item;
        _loading = loading;

        if (select >= 0 && select < dgvColumns.Rows.Count)
            dgvColumns.CurrentCell = dgvColumns.Rows[select].Cells[colColName.Index];
        UpdateRuler();
    }

    private void UpdateRuler()
    {
        var table = _current;
        var columns = (table?.Items ?? []).Select((item, i) => new CatalogRuler.Column(item.Name, item.Line, item.Start, item.End,
            _problems.Any(p => ReferenceEquals(p.Table, table) && p.Field == Field.Column && p.Column == i))).ToList();
        var limit = table?.Manufacturer == TableManufacturer.ELEN ? TableCatalogRules.ElenMaxPosition : (int?)null;
        ruler.SetColumns(columns, dgvColumns.CurrentRow?.Index ?? -1, limit);
    }

    /// <summary>
    /// Zobrazi udaje vybraneho stlpca.
    /// </summary>
    private void ShowColumn()
    {
        var item = CurrentColumn;
        var loading = _loading;
        _loading = true;
        try
        {
            foreach (var control in new Control[] { tbColKey, cbFill, cbAlign, cbFont, cbDivType, cbTab1, cbTab2 })
                control.Enabled = item is not null;
            tbColKey.Text = item?.Key ?? "";
            cbFill.SelectedItem = item?.FillSection;
            cbAlign.SelectedItem = item?.Align;
            _font.Value = item?.FontIDX ?? 0;
            cbDivType.SelectedItem = item?.DivType;
            cbTab1.SelectedItem = TabItem(cbTab1, item?.Tab1);
            cbTab2.SelectedItem = TabItem(cbTab2, item?.Tab2);
        }
        finally
        {
            _loading = loading;
        }

        ShowDivNote();
        var index = dgvColumns.CurrentRow?.Index ?? -1;
        bColDelete.Enabled = item is not null;
        bColUp.Enabled = index > 0;
        bColDown.Enabled = index >= 0 && index < dgvColumns.Rows.Count - 1;
        bColAdd.Enabled = _current is not null;
        ruler.Select(index);
    }

    // TabTab stlpca v ponuke - prazdny odkaz je polozka „Ziadny“
    private static object? TabItem(ComboBox combo, TableTabTab? tab) =>
        combo.Items.Cast<TableTabTab>().FirstOrDefault(t => tab is null || tab == TableTabTab.Empty ? t == TableTabTab.Empty : t == tab);

    private void ShowDivNote()
    {
        lDivNote.Text = (CurrentColumn?.DivType?.Id) switch
        {
            0 => Resources.CatalogTablesPage_Div0,
            1 => Resources.CatalogTablesPage_Div1,
            2 => Resources.CatalogTablesPage_Div2,
            3 => Resources.CatalogTablesPage_Div3,
            4 => Resources.CatalogTablesPage_Div4,
            _ => ""
        };
    }

    private void dgvColumns_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (_loading || !IsHandleCreated || _columnPending)
            return;

        // obnova poli az po skonceni zmeny bunky - vypnutie tlacidla s fokusom by ju vnorilo
        _columnPending = true;
        BeginInvoke(() =>
        {
            _columnPending = false;
            ShowColumn();
            MarkColumnKey();
        });
    }

    private void ruler_ColumnClicked(object? sender, CatalogRulerEventArgs e)
    {
        if (e.Column < dgvColumns.Rows.Count)
            dgvColumns.CurrentCell = dgvColumns.Rows[e.Column].Cells[colColName.Index];
    }

    private void dgvColumns_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || dgvColumns.Rows[e.RowIndex].Tag is not TableItem item)
            return;

        var value = dgvColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        var text = Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
        // nie cislo sa zapise ako 0 - kontrola stlpec oznaci
        var number = int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out var n) ? n : 0;
        if (e.ColumnIndex == colColName.Index)
            item.Name = text.Trim();
        else if (e.ColumnIndex == colColLine.Index)
            item.Line = Math.Max(0, number - 1);
        else if (e.ColumnIndex == colColStart.Index)
            item.Start = number;
        else if (e.ColumnIndex == colColEnd.Index)
            item.End = number;

        Check();
    }

    private void dgvColumns_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && sender is DataGridView grid && !grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            grid.BeginEdit(true);
    }

    private void dgvColumns_DataError(object? sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void Column_Changed(object? sender, EventArgs e)
    {
        if (_loading || CurrentColumn is not { } item)
            return;

        if (sender == cbFill && cbFill.SelectedItem is TableFillSection fill)
            item.FillSection = fill;
        else if (sender == cbAlign && cbAlign.SelectedItem is TableAlign align)
            item.Align = align;
        else if (sender == cbFont)
            item.FontIDX = _font.Value;
        else if (sender == cbDivType && cbDivType.SelectedItem is TableDivType div)
        {
            item.DivType = div;
            ShowDivNote();
        }
        else if (sender == cbTab1 && cbTab1.SelectedItem is TableTabTab tab1)
            item.Tab1 = tab1;
        else if (sender == cbTab2 && cbTab2.SelectedItem is TableTabTab tab2)
            item.Tab2 = tab2;

        Check();
    }

    /// <summary>
    /// Kluc stlpca sa zmeni az po opusteni pola - poradie stlpcov naň odkazuje a premenuje sa s nim; prazdny alebo
    /// obsadeny kluc by odkazy dvoch stlpcov zlucil, preto sa neprijme.
    /// </summary>
    private void tbColKey_Validated(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table || CurrentColumn is not { } item)
            return;

        var key = tbColKey.Text.Trim();
        if (key == item.Key)
            return;

        if (key.Length == 0 || table.Items.Any(i => !ReferenceEquals(i, item) && i.Key == key))
        {
            _keyMessage = string.Format(CultureInfo.CurrentCulture, Resources.CatalogTablesPage_Kluc_Neplatny, key);
            _loading = true;
            tbColKey.Text = item.Key;
            _loading = false;
            ShowHint();
            return;
        }

        TableCatalogEditing.RenameKey(table.ViewTypeTabs, item.Key, key);
        item.Key = key;
        _keyMessage = null;
        Check();
    }

    private void bColAdd_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table)
            return;

        var item = TableCatalogEditing.NewColumn(table, Resources.CatalogTablesPage_Novy_stlpec,
            TableCatalogRules.CellWidth(table.Manufacturer));
        var index = dgvColumns.CurrentRow is { } row ? row.Index + 1 : table.Items.Count;
        table.Items.Insert(index, item);
        ColumnsChanged(index);
        dgvColumns.Focus();
        dgvColumns.BeginEdit(true);
    }

    private void bColDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table || CurrentColumn is not { } item)
            return;

        // realizacia textu by po odstraneni ukazovala na neexistujuci stlpec
        var texts = TableCatalogEditing.TextsUsing(item, GlobData.TableTexts);
        if (texts.Count > 0)
        {
            Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.CatalogTablesPage_Stlpec_V_Textoch, item.Name,
                string.Join(", ", texts.Select(t => t.Name))));
            return;
        }

        if (TableCatalogEditing.CountKeyUsages(table.ViewTypeTabs, item.Key) > 0)
        {
            if (Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FTableCatalog_bColumnDelete_StlpecPouzity, item.Key)) !=
                DialogResult.Yes)
                return;
            TableCatalogEditing.RemoveKey(table.ViewTypeTabs, item.Key);
        }

        var index = table.Items.IndexOf(item);
        table.Items.RemoveAt(index);
        ColumnsChanged(Math.Min(index, table.Items.Count - 1));
    }

    private void bColUp_Click(object? sender, EventArgs e) => MoveColumn(-1);

    private void bColDown_Click(object? sender, EventArgs e) => MoveColumn(1);

    private void MoveColumn(int offset)
    {
        if (_current is not { } table || CurrentColumn is not { } item)
            return;

        var index = table.Items.IndexOf(item);
        var target = index + offset;
        if (target < 0 || target >= table.Items.Count)
            return;

        table.Items.RemoveAt(index);
        table.Items.Insert(target, item);
        ColumnsChanged(target);
    }

    private void ColumnsChanged(int select)
    {
        FillColumns(select);
        ShowColumn();
        ShowOrder();
        Check();
    }

    // ---------------------------------------------------------------- poradie stlpcov

    private void ShowOrder()
    {
        var tabs = _current?.ViewTypeTabs ?? [];
        lOrderNote.Text = _current is null ? ""
            : tabs.Count == 0 ? Resources.CatalogTablesPage_Poradie_Ziadne
            : string.Format(CultureInfo.CurrentCulture, Resources.CatalogTablesPage_Poradie, string.Join(", ", tabs.Select(t => t.ViewType.Name)));
    }

    private void bColumnOrder_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table)
            return;

        // okno poradia dostane kopie - pri Zrusit sa poradie nezmeni
        using var form = new FTableColumnOrder(table.Items.ToList(), table.ViewTypeTabs.Select(TableCatalogEditing.Clone).ToList());
        if (form.ShowDialog(FindForm()) != DialogResult.OK)
            return;

        table.ViewTypeTabs = form.ItemsTypeTabs.ToList();
        ShowOrder();
        Check();
    }

    // ---------------------------------------------------------------- riadky

    private void FillRows()
    {
        var loading = _loading;
        _loading = true;
        dgvRows.Rows.Clear();
        var segments = _current?.Segments ?? [];
        for (var i = 0; i < segments.Count; i++)
            dgvRows.Rows[dgvRows.Rows.Add(i + 1, segments[i].Height, segments[i].Width, segments[i].Size)].Tag = segments[i];
        _loading = loading;
        bRowsSetAll.Enabled = segments.Count > 1;
    }

    private void dgvRows_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || dgvRows.Rows[e.RowIndex].Tag is not TableSegment segment)
            return;

        var text = Convert.ToString(dgvRows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, CultureInfo.CurrentCulture);
        var number = int.TryParse(text, out var n) ? Math.Max(0, n) : 0;
        if (e.ColumnIndex == colRowHeight.Index)
            segment.Height = number;
        else if (e.ColumnIndex == colRowWidth.Index)
            segment.Width = number;
        else if (e.ColumnIndex == colRowSize.Index)
            segment.Size = number;
    }

    private void dgvRows_CellDoubleClick(object? sender, DataGridViewCellEventArgs e) => dgvColumns_CellDoubleClick(sender, e);

    private void bRowsSetAll_Click(object? sender, EventArgs e)
    {
        if (_current is not { } table || dgvRows.CurrentRow?.Tag is not TableSegment source)
            return;

        // kazdy riadok dostane vlastnu instanciu - inak by uprava jedneho zmenila vsetky
        for (var i = 0; i < table.Segments.Count; i++)
            table.Segments[i] = TableCatalogEditing.Clone(source);
        FillRows();
    }

    // ---------------------------------------------------------------- tabula

    private void Field_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table)
            return;

        if (sender == tbName)
        {
            table.Name = tbName.Text.Trim();
            // nazov predlohy vidno aj na strankach fyzickych tabul a textov
            GlobData.TableCatalogs.ResetItem(GlobData.TableCatalogs.IndexOf(table));
        }
        else if (sender == tbKey)
            table.Key = tbKey.Text.Trim();
        else if (sender == tbComment)
            table.Comment = tbComment.Text;
        else if (sender == cbManufacturer && cbManufacturer.SelectedItem is TableManufacturer manufacturer)
        {
            table.Manufacturer = manufacturer;
            _font.Manufacturer = manufacturer;
        }
        else if (sender == nudMinHeight)
            table.MinHeight = decimal.ToInt32(nudMinHeight.Value);
        else if (sender == nudMaxRec)
        {
            // pocet riadkov nasleduje pocet zaznamov, nove riadky maju minimalnu vysku
            table.MaxRecCount = decimal.ToInt32(nudMaxRec.Value);
            TableCatalogEditing.ResizeRows(table.Segments, table.MaxRecCount,
                () => new TableSegment { Height = table.MinHeight, Width = table.Segments.LastOrDefault()?.Width ?? 0, Size = 0 });
            FillRows();
        }

        _list.Refresh(table);
        Check();
    }

    private IReadOnlyList<string> Usage(TableCatalog table)
    {
        var usage = GlobData.TablePhysicals.Where(p => ReferenceEquals(p.TableCatalog, table))
            .Select(p => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Fyzicka, p.Name))
            .ToList();
        foreach (var text in GlobData.TableTexts)
        foreach (var realization in text.Realizations)
            if (ReferenceEquals(realization.Table, table))
                usage.Add(string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Text, text.Name, realization.Item?.Name));
        return usage;
    }

    private void ShowUsage()
    {
        var usage = _current is null ? [] : Usage(_current);
        lUse.Text = UsageText.Format(usage, true, Resources.TablesPage_Nepouziva);
        bDuplicate.Enabled = _current is not null;
        bDelete.Enabled = _current is not null && usage.Count == 0;
    }

    private void Check()
    {
        _problems.Clear();
        var tables = GlobData.TableCatalogs.ToList();
        for (var i = 0; i < tables.Count; i++)
            foreach (var (field, column, message) in TableCatalogRules.Check(tables, i))
                _problems.Add((tables[i], field, column, message));

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        _list.MarkProblems(table => _problems.FirstOrDefault(p => ReferenceEquals(p.Table, table)).Message);
        var own = _problems.Where(p => ReferenceEquals(p.Table, _current)).ToList();
        var warnings = _current is null ? [] : TableCatalogRules.Warnings(_current);
        foreach (DataGridViewRow row in dgvColumns.Rows)
        {
            var error = own.FirstOrDefault(p => p.Field == Field.Column && p.Column == row.Index).Message;
            var cell = row.Cells[colColName.Index];
            cell.ErrorText = error ?? "";
            cell.ToolTipText = string.Join(Environment.NewLine, warnings.Where(w => w.Column == row.Index).Select(w => w.Message));
            cell.Style.ForeColor = error is null && warnings.Any(w => w.Column == row.Index) ? SettingsWindow.ProblemColor(dgvColumns) : Color.Empty;
        }

        UpdateRuler();
        MarkColumnKey();
    }

    // okraj poli tabule a kluca vybraneho stlpca (prazdny alebo zdvojeny kluc)
    private void MarkColumnKey()
    {
        var bad = _problems.Where(p => ReferenceEquals(p.Table, _current) && p.Field is Field.Name or Field.Key)
            .Select(p => p.Field == Field.Key ? (Control)tbKey : tbName).ToList();
        if (_current is { } table && CurrentColumn is { } item)
        {
            var key = item.Key?.Trim() ?? "";
            if (key.Length == 0 || table.Items.Any(i => !ReferenceEquals(i, item) && i.Key?.Trim() == key))
                bad.Add(tbColKey);
        }

        _marks.Mark(bad);
    }

    // pod udajmi vsetky chyby vybranej tabule, inak upozornenia jej stlpcov
    private void ShowHint()
    {
        var problems = _problems.Where(p => ReferenceEquals(p.Table, _current)).Select(p => p.Message).ToList();
        if (_keyMessage is not null)
            problems.Insert(0, _keyMessage);
        if (problems.Count > 0)
        {
            lHint.Text = string.Join(Environment.NewLine, problems);
            lHint.ForeColor = SettingsWindow.ProblemColor(lHint);
            return;
        }

        var warnings = _current is null ? [] : TableCatalogRules.Warnings(_current);
        lHint.Text = warnings.Count == 0 ? ""
            : Resources.LogicalTablesPage_Upozornenia + Environment.NewLine + string.Join(Environment.NewLine, warnings.Select(w => w.Message));
        lHint.ForeColor = _hintColor;
    }

    // ---------------------------------------------------------------- zoznam

    private void bAdd_Click(object? sender, EventArgs e)
    {
        var name = TableRules.Unique(GlobData.TableCatalogs.Select(t => t.Name), Resources.TablesPage_Nova_tabula);
        var table = new TableCatalog
        {
            Name = name,
            Key = TableRules.Unique(GlobData.TableCatalogs.Select(t => t.Key), name),
            Comment = "",
            Manufacturer = TableManufacturer.LCD1,
            MaxRecCount = 1,
            MinHeight = 10,
            NumSegments = 1
        };
        TableCatalogEditing.ResizeRows(table.Segments, 1, () => new TableSegment { Height = 10 });

        GlobData.TableCatalogs.Add(table);
        Added(table);
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var table = TableCatalogEditing.Clone(source);
        table.Name = TableRules.Unique(GlobData.TableCatalogs.Select(t => t.Name), source.Name);
        table.Key = TableRules.Unique(GlobData.TableCatalogs.Select(t => t.Key), source.Key);

        GlobData.TableCatalogs.Insert(GlobData.TableCatalogs.IndexOf(source) + 1, table);
        Added(table);
    }

    private void Added(TableCatalog table)
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

        var index = GlobData.TableCatalogs.IndexOf(table);
        GlobData.TableCatalogs.RemoveAt(index);
        _list.Fill(GlobData.TableCatalogs.Count == 0 ? null : GlobData.TableCatalogs[Math.Min(index, GlobData.TableCatalogs.Count - 1)]);
        Check();
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
