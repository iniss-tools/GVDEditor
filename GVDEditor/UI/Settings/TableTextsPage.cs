using System.Globalization;
using System.Text.RegularExpressions;
using ExControls;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Controls;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.TableTextRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Texty na tabuliach v okne Lokalne nastavenia - zoznam typov textov a udaje vybraneho textu
/// (realizacie, texty vlakov) s upravou priamo v poliach a tabulkach. Zmeny idu rovno do
/// <see cref="GlobData.TableTexts" />, Zrusit okna ich vrati.
/// </summary>
public partial class TableTextsPage : UserControl, ISettingsPage
{
    private readonly ItemListSupport<TableText> _list;
    private readonly FieldMarks _marks = new();
    private readonly List<(TableText Text, Field Field, int Row, string Message)> _problems = [];
    private readonly ToolTip _fontTip = new();
    private readonly TableFontChoice _font;
    private GVDInfo? _gvd;
    private TableText? _current;
    private bool _loaded;
    private bool _loading;
    private Color _hintColor;

    /// <summary>
    /// Polozka rozbalovacieho zoznamu v bunke - objekt a text pre obsluhu.
    /// </summary>
    private sealed record Choice(object Ref, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TableTextsPage()
    {
        InitializeComponent();
        dgvReal.AutoGenerateColumns = false;
        dgvTrains.AutoGenerateColumns = false;
        (components ??= new Container()).Add(_fontTip);
        _font = new TableFontChoice(cbFont, _fontTip, allowColumnDefault: true);
        _font.ValueChanged += Font_ValueChanged;
        _list = new ItemListSupport<TableText>(dgv, tbFilter, () => GlobData.TableTexts, t => [t.Name, t.Key]);
        _list.SelectionChanged += (_, _) => ShowCurrent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : $"{Label(_problems[0].Text)} – {_problems[0].Message}";

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        var (text, field, row, _) = _problems[0];
        _list.Select(text);
        ShowCurrent();
        if (field == Field.Realization && row < dgvReal.Rows.Count)
        {
            dgvReal.Focus();
            dgvReal.CurrentCell = dgvReal.Rows[row].Cells[colRealTable.Index];
        }
        else
            (field == Field.Key ? tbKey : tbName).Focus();
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="gvd">grafikon - jeho stanica pri predvyplneni textov vlakov</param>
    internal void LoadData(GVDInfo gvd)
    {
        _gvd = gvd;
        foreach (var header in new[] { lBasic, lRealHeader, lTrainsHeader, lCommentHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        lRealNote.ForeColor = SystemColors.GrayText;
        _hintColor = lHint.ForeColor;
        _marks.Capture(tbName, tbKey);
        if (GlobData.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);
        _list.CaptureColors();
        foreach (var grid in new[] { dgvReal, dgvTrains })
            grid.BackgroundColor = grid.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : grid.DefaultCellStyle.BackColor;

        _loaded = true;
        _list.Fill(_selectAfterLoad ?? GlobData.TableTexts.FirstOrDefault());
        Check();
    }

    /// <summary>
    /// Vyberie text (napr. pri oprave z analyzy grafikonu); pred naplnenim stranky az po nom.
    /// </summary>
    public void SelectText(TableText text)
    {
        if (!_loaded)
        {
            _selectAfterLoad = text;
            return;
        }

        _list.Select(text);
        ShowCurrent();
    }

    private TableText? _selectAfterLoad;

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // katalogove tabule a ich stlpce sa mohli zmenit na inej stranke okna
        if (Visible && _loaded)
        {
            ShowCurrent();
            Check();
        }
    }

    private static string Label(TableText text) => string.IsNullOrWhiteSpace(text.Name) ? text.Key : text.Name;

    private static string FormatTrain(Train train) =>
        $"{train.ID}. {train.Number} {train.Type} {train.Name}".TrimEnd();

    private void ShowCurrent()
    {
        var text = _list.Current;
        _current = text;
        _loading = true;
        pDetail.SuspendLayout();
        try
        {
            tlpDetail.Enabled = text is not null;
            tbName.Text = text?.Name ?? "";
            tbKey.Text = text?.Key ?? "";
            tbComment.Text = text?.Comment ?? "";
            FillRealizations(0);
            FillTrains(null);
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        bDuplicate.Enabled = bDelete.Enabled = text is not null;
        MarkProblems();
        ShowHint();
    }

    // ---------------------------------------------------------------- realizacie

    private TableTextRealization? CurrentRealization => dgvReal.CurrentRow?.Tag as TableTextRealization;

    private void FillRealizations(int select)
    {
        var loading = _loading;
        _loading = true;
        dgvReal.Rows.Clear();
        foreach (var realization in _current?.Realizations ?? [])
        {
            var row = dgvReal.Rows[dgvReal.Rows.Add()];
            row.Tag = realization;
            SetRealizationCells(row, realization);
        }

        _loading = loading;
        if (select >= 0 && select < dgvReal.Rows.Count)
            dgvReal.CurrentCell = dgvReal.Rows[select].Cells[colRealTable.Index];
        UpdateRealizationButtons();
    }

    /// <summary>
    /// Ponuka tabul a stlpcov v riadku realizacie; tabula alebo stlpec, ktory uz neexistuje, sa ponukne tiez,
    /// aby ho bunka vedela zobrazit (chybu ukaze kontrola).
    /// </summary>
    private void SetRealizationCells(DataGridViewRow row, TableTextRealization realization)
    {
        var tables = GlobData.TableCatalogs.Select(t => new Choice(t, t.Name)).ToList();
        if (realization.Table is { } table && !GlobData.TableCatalogs.Contains(table))
            tables.Add(new Choice(table, table.Name));
        SetChoices((DataGridViewComboBoxCell)row.Cells[colRealTable.Index], tables, realization.Table);

        var items = (realization.Table?.Items ?? []).Select(i => new Choice(i, i.Name)).ToList();
        if (realization.Item is { } item && items.All(c => !ReferenceEquals(c.Ref, item)))
            items.Add(new Choice(item, item.Name));
        SetChoices((DataGridViewComboBoxCell)row.Cells[colRealItem.Index], items, realization.Item);
    }

    private static void SetChoices(DataGridViewComboBoxCell cell, List<Choice> choices, object? value)
    {
        cell.DataSource = choices;
        cell.DisplayMember = nameof(Choice.Text);
        cell.ValueMember = nameof(Choice.Ref);
        cell.Value = value;
    }

    private void dgvReal_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || _current is null || dgvReal.Rows[e.RowIndex].Tag is not TableTextRealization realization)
            return;

        var value = dgvReal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        if (e.ColumnIndex == colRealTable.Index && value is TableCatalog table && !ReferenceEquals(table, realization.Table))
        {
            // v novej tabuli stlpec s rovnakym klucom, inak prvy
            realization.Table = table;
            realization.Item = table.Items.FirstOrDefault(i => i.Key == realization.Item?.Key) ?? table.Items.FirstOrDefault()!;
            var index = e.RowIndex;
            // prerobenie buniek riadka priamo v udalosti tabulky by bolo vnorene volanie
            BeginInvoke(() =>
            {
                if (index < dgvReal.Rows.Count)
                {
                    _loading = true;
                    SetRealizationCells(dgvReal.Rows[index], realization);
                    _loading = false;
                }
            });
        }
        else if (e.ColumnIndex == colRealItem.Index && value is TableItem item)
            realization.Item = item;

        Check();
    }

    private void dgvReal_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // vyber z ponuky sa prejavi hned, nie az po opusteni bunky
        if (dgvReal.IsCurrentCellDirty && dgvReal.CurrentCell is DataGridViewComboBoxCell)
            dgvReal.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvReal_DataError(object? sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgvReal_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!_loading && IsHandleCreated)
            BeginInvoke(UpdateRealizationButtons);
    }

    private void UpdateRealizationButtons()
    {
        bRealAdd.Enabled = _current is not null && GlobData.TableCatalogs.Count > 0;
        bRealDelete.Enabled = CurrentRealization is not null;
        _font.Manufacturer = (CurrentRealization ?? _current?.Realizations.FirstOrDefault())?.Table?.Manufacturer;
    }

    private void bRealAdd_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text || GlobData.TableCatalogs.FirstOrDefault() is not { } table)
            return;

        text.Realizations.Add(new TableTextRealization { Table = table, Item = table.Items.FirstOrDefault()! });
        FillRealizations(text.Realizations.Count - 1);
        Check();
        dgvReal.Focus();
    }

    private void bRealDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text || CurrentRealization is not { } realization)
            return;

        var index = text.Realizations.IndexOf(realization);
        text.Realizations.RemoveAt(index);
        FillRealizations(Math.Min(index, text.Realizations.Count - 1));
        Check();
    }

    // ---------------------------------------------------------------- texty vlakov

    private TableTrain? CurrentTrain => dgvTrains.CurrentRow?.Tag as TableTrain;

    private void FillTrains(TableTrain? select)
    {
        var loading = _loading;
        _loading = true;
        dgvTrains.Rows.Clear();
        foreach (var train in _current?.Trains ?? [])
        {
            var index = dgvTrains.Rows.Add(FormatTrain(train.Train), train.Text, TableFontChoice.Describe(train.FontID));
            dgvTrains.Rows[index].Tag = train;
        }

        var row = dgvTrains.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ReferenceEquals(r.Tag, select));
        if (row is not null)
            dgvTrains.CurrentCell = row.Cells[colText.Index];

        // vlaky grafikonu, ktore este text nemaju
        cbAddTrain.BeginUpdate();
        cbAddTrain.Items.Clear();
        if (_current is not null)
            cbAddTrain.Items.AddRange(TableTextGenerating.TrainsWithoutText(GlobData.Trains, _current.Trains)
                .Select(t => new Choice(t, FormatTrain(t))).ToArray<object>());
        if (cbAddTrain.Items.Count > 0)
            cbAddTrain.SelectedIndex = 0;
        cbAddTrain.EndUpdate();
        _loading = loading;

        UpdateTrainControls();
    }

    private void UpdateTrainControls()
    {
        var train = CurrentTrain;
        var loading = _loading;
        _loading = true;
        _font.Value = train?.FontID ?? -1;
        _loading = loading;
        cbFont.Enabled = train is not null;
        bTrainRemove.Enabled = train is not null;
        cbAddTrain.Enabled = bTrainAdd.Enabled = cbAddTrain.Items.Count > 0;
        _fontTip.SetToolTip(bTrainAdd, cbAddTrain.Items.Count > 0 ? "" : Resources.TableTextsPage_Bez_vlakov);
    }

    private void dgvTrains_CurrentCellChanged(object? sender, EventArgs e)
    {
        if (!_loading && IsHandleCreated)
            BeginInvoke(UpdateTrainControls);
    }

    private void dgvTrains_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || e.ColumnIndex != colText.Index || dgvTrains.Rows[e.RowIndex].Tag is not TableTrain train)
            return;

        // tabulator a novy riadok by v subore rozbili zaznam
        train.Text = Regex.Replace(dgvTrains.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string ?? "", @"\t|\n|\r", "");
    }

    private void dgvTrains_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == colText.Index)
            dgvTrains.BeginEdit(true);
    }

    private void Font_ValueChanged(object? sender, EventArgs e)
    {
        if (_loading || CurrentTrain is not { } train)
            return;

        train.FontID = _font.Value;
        if (dgvTrains.CurrentRow is { } row)
            row.Cells[colFont.Index].Value = TableFontChoice.Describe(train.FontID);
    }

    private void bTrainAdd_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text || cbAddTrain.SelectedItem is not Choice { Ref: Train train } || _gvd is null)
            return;

        // text sa predvyplni podla stlpca vybranej realizacie (ak ho generovanie podporuje)
        var realization = CurrentRealization ?? text.Realizations.FirstOrDefault();
        var tableTrain = TableTextGenerating.CreateFor(train, realization?.Item?.FillSection, _gvd.ThisStation);

        // zoznam drzi poradie vlakov v grafikone
        var index = 0;
        while (index < text.Trains.Count && text.Trains[index].Train.ID <= train.ID)
            index++;
        text.Trains.Insert(index, tableTrain);

        FillTrains(tableTrain);
        dgvTrains.Focus();
        dgvTrains.BeginEdit(true);
    }

    private void bTrainRemove_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text || CurrentTrain is not { } train)
            return;

        var index = text.Trains.IndexOf(train);
        text.Trains.RemoveAt(index);
        FillTrains(text.Trains.Count == 0 ? null : text.Trains[Math.Min(index, text.Trains.Count - 1)]);
    }

    private void bGenerate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text || _gvd is null)
            return;

        if ((CurrentRealization ?? text.Realizations.FirstOrDefault()) is not { Table: { } table, Item: { } item })
        {
            Utils.ShowError(Resources.TableTextsPage_Generovat_Bez);
            return;
        }

        if (!TableTextGenerating.IsSupported(item.FillSection))
        {
            Utils.ShowError(Resources.FTableText_Generate_TTexts_Wrong_Item);
            return;
        }

        if (Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture, Resources.FTableText_Generate_TTexts_Info, table.Name,
                item.Name)) != DialogResult.Yes)
            return;

        text.Trains = TableTextGenerating.Generate(GlobData.Trains, item.FillSection, _gvd.ThisStation).ToList();
        FillTrains(text.Trains.FirstOrDefault());
    }

    // ---------------------------------------------------------------- text

    private void Field_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } text)
            return;

        if (sender == tbName)
            text.Name = tbName.Text.Trim();
        else if (sender == tbKey)
            text.Key = tbKey.Text.Trim();
        else if (sender == tbComment)
            text.Comment = tbComment.Text;

        _list.Refresh(text);
        Check();
    }

    private void Check()
    {
        _problems.Clear();
        var texts = GlobData.TableTexts.ToList();
        for (var i = 0; i < texts.Count; i++)
            foreach (var (field, row, message) in TableTextRules.Check(texts, i, GlobData.TableCatalogs))
                _problems.Add((texts[i], field, row, message));

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        _list.MarkProblems(text => _problems.FirstOrDefault(p => ReferenceEquals(p.Text, text)).Message);
        var own = _problems.Where(p => ReferenceEquals(p.Text, _current)).ToList();
        _marks.Mark(own.Where(p => p.Field != Field.Realization).Select(p => p.Field == Field.Key ? (Control)tbKey : tbName));
        foreach (DataGridViewRow row in dgvReal.Rows)
            row.Cells[colRealTable.Index].ErrorText = own.FirstOrDefault(p => p.Field == Field.Realization && p.Row == row.Index).Message ?? "";
    }

    // pod udajmi vsetky chyby vybraneho textu
    private void ShowHint()
    {
        var problems = _problems.Where(p => ReferenceEquals(p.Text, _current)).Select(p => p.Message).ToList();
        lHint.Text = string.Join(Environment.NewLine, problems);
        lHint.ForeColor = problems.Count > 0 ? SettingsWindow.ProblemColor(lHint) : _hintColor;
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        var name = TableRules.Unique(GlobData.TableTexts.Select(t => t.Name), Resources.TablesPage_Novy_text);
        var text = new TableText
        {
            Name = name,
            Key = TableRules.Unique(GlobData.TableTexts.Select(t => t.Key), name),
            Comment = ""
        };

        GlobData.TableTexts.Add(text);
        Added(text);
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var text = new TableText
        {
            Name = TableRules.Unique(GlobData.TableTexts.Select(t => t.Name), source.Name),
            Key = TableRules.Unique(GlobData.TableTexts.Select(t => t.Key), source.Key),
            Comment = source.Comment,
            Realizations = source.Realizations.Select(TableTextGenerating.Clone).ToList(),
            Trains = source.Trains.Select(TableTextGenerating.Clone).ToList()
        };

        GlobData.TableTexts.Insert(GlobData.TableTexts.IndexOf(source) + 1, text);
        Added(text);
    }

    private void Added(TableText text)
    {
        _list.Fill(text);
        ShowCurrent();
        Check();
        tbName.Focus();
        tbName.SelectAll();
    }

    private void bDelete_Click(object? sender, EventArgs e)
    {
        if (_current is not { } text)
            return;

        var index = GlobData.TableTexts.IndexOf(text);
        GlobData.TableTexts.RemoveAt(index);
        _list.Fill(GlobData.TableTexts.Count == 0 ? null : GlobData.TableTexts[Math.Min(index, GlobData.TableTexts.Count - 1)]);
        Check();
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
