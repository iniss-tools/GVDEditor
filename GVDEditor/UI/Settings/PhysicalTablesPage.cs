using System.Globalization;
using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;
using Field = GVDEditor.Domain.Rules.TablePhysicalRules.Field;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Fyzicke tabule v okne Lokalne nastavenia - zoznam tabul a udaje vybranej tabule s upravou priamo
/// v poliach. Zmeny idu rovno do <see cref="_ctx.Document.TablePhysicals" />, Zrusit okna ich vrati.
/// </summary>
public partial class PhysicalTablesPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private readonly ItemListSupport<TablePhysical> _list;
    private readonly FieldMarks _marks = new();
    private readonly List<(TablePhysical Table, Field Field, string Text)> _problems = [];
    private TablePhysical? _current;
    private bool _loaded;
    private bool _loading;
    private Color _hintColor;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public PhysicalTablesPage()
    {
        InitializeComponent();
        _list = new ItemListSupport<TablePhysical>(dgv, tbFilter, () => _ctx.Document.TablePhysicals, t => [t.Name, t.Key]);
        _list.SelectionChanged += (_, _) => ShowCurrent();
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _problems.Count == 0 ? null : $"{Label(_problems[0].Table)} – {_problems[0].Text}";

    /// <inheritdoc />
    public void FocusFirstProblem()
    {
        if (_problems.Count == 0)
            return;

        var (table, field, _) = _problems[0];
        _list.Select(table);
        ShowCurrent();
        FieldControl(field).Focus();
    }

    /// <summary>
    /// Naplni stranku - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    internal void LoadData(EditorContext context)
    {
        _ctx = context;
        foreach (var header in new[] { lBasic, lComm, lAdvanced, lCommentHeader, lUseHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        lIdNote.ForeColor = SystemColors.GrayText;
        _hintColor = lHint.ForeColor;
        _marks.Capture(tbName, tbKey, nudId);
        if (_ctx.UsingStyle.DarkScrollBar)
            pDetail.SetTheme(WindowsTheme.DarkExplorer);
        _list.CaptureColors();

        FillCatalogs();
        _loaded = true;
        _list.Fill(_ctx.Document.TablePhysicals.FirstOrDefault());
        Check();
    }

    /// <inheritdoc />
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        // katalogove a logicke tabule sa mohli zmenit na inych strankach okna
        if (Visible && _loaded)
        {
            FillCatalogs();
            _list.Fill(_current);
            Check();
        }
    }

    private static string Label(TablePhysical table) => string.IsNullOrWhiteSpace(table.Name) ? table.Key : table.Name;

    private void FillCatalogs()
    {
        _loading = true;
        cbCatalog.BeginUpdate();
        cbCatalog.Items.Clear();
        cbCatalog.Items.AddRange(_ctx.Document.TableCatalogs.ToArray<object>());
        cbCatalog.SelectedItem = _current?.TableCatalog;
        cbCatalog.EndUpdate();
        _loading = false;
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
            cbCatalog.SelectedItem = table?.TableCatalog;
            nudRecCount.Value = Clamp(nudRecCount, table?.RecCount ?? 0);
            nudPort.Value = Clamp(nudPort, table?.CommunicationPort ?? 0);
            nudId.Value = Clamp(nudId, table?.ID ?? 0);
            tbXml.Text = table?.SaveXML ?? "";
            tbReverse.Text = table?.ReverseArrows ?? "";
            tbRem.Text = table?.Rem ?? "";
            tbComment.Text = table?.Comment ?? "";
        }
        finally
        {
            pDetail.ResumeLayout(true);
            _loading = false;
        }

        UpdateIdNote();
        ShowUsage();
        MarkProblems();
        ShowHint();
    }

    private static decimal Clamp(NumericUpDown nud, int value) => Math.Clamp(value, nud.Minimum, nud.Maximum);

    private void UpdateIdNote()
    {
        var manufacturer = _current?.TableCatalog?.Manufacturer;
        lIdNote.Text = manufacturer is null ? ""
            : manufacturer.IsKnownToIniss && manufacturer.MinAddress >= 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.PhysicalTablesPage_Adresa, manufacturer.Name,
                    manufacturer.MinAddress, manufacturer.MaxAddress)
                : string.Format(CultureInfo.CurrentCulture, Resources.PhysicalTablesPage_Adresa_Bez_kontroly, manufacturer.Name);
    }

    private IReadOnlyList<string> Usage(TablePhysical table) =>
        TableUsage.LogicalPositions(table, _ctx.Document.TableLogicals)
            .Select(u => string.Format(CultureInfo.CurrentCulture, Resources.TablesPage_Pouzitie_Logicka, u.Logical.Name, u.Position))
            .ToList();

    private void ShowUsage()
    {
        var usage = _current is null ? [] : Usage(_current);
        lUse.Text = UsageText.Format(usage, true, Resources.TablesPage_Nepouziva);
        bDuplicate.Enabled = _current is not null;
        bDelete.Enabled = _current is not null && usage.Count == 0;
    }

    private void Field_Changed(object? sender, EventArgs e)
    {
        if (_loading || _current is not { } table)
            return;

        if (sender == tbName)
            table.Name = tbName.Text.Trim();
        else if (sender == tbKey)
            table.Key = tbKey.Text.Trim();
        else if (sender == cbCatalog && cbCatalog.SelectedItem is TableCatalog catalog)
        {
            table.TableCatalog = catalog;
            UpdateIdNote();
        }
        else if (sender == nudRecCount)
            table.RecCount = decimal.ToInt32(nudRecCount.Value);
        else if (sender == nudPort)
            table.CommunicationPort = decimal.ToInt32(nudPort.Value);
        else if (sender == nudId)
            table.ID = decimal.ToInt32(nudId.Value);
        else if (sender == tbXml)
            table.SaveXML = tbXml.Text.Trim();
        else if (sender == tbReverse)
            table.ReverseArrows = tbReverse.Text.Trim();
        else if (sender == tbRem)
            table.Rem = tbRem.Text.Trim();
        else if (sender == tbComment)
            table.Comment = tbComment.Text;

        _list.Refresh(table);
        Check();
    }

    private Control FieldControl(Field field) => field switch
    {
        Field.Key => tbKey,
        Field.Catalog => cbCatalog,
        Field.Id => nudId,
        _ => tbName
    };

    private void Check()
    {
        _problems.Clear();
        var tables = _ctx.Document.TablePhysicals.ToList();
        for (var i = 0; i < tables.Count; i++)
            foreach (var (field, text) in TablePhysicalRules.Check(tables, i, _ctx.Document.TableCatalogs))
                _problems.Add((tables[i], field, text));

        MarkProblems();
        ShowHint();
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void MarkProblems()
    {
        _list.MarkProblems(table => _problems.FirstOrDefault(p => ReferenceEquals(p.Table, table)).Text);
        _marks.Mark(_problems.Where(p => ReferenceEquals(p.Table, _current)).Select(p => FieldControl(p.Field)));
    }

    // pod udajmi vsetky chyby vybranej tabule
    private void ShowHint()
    {
        var problems = _problems.Where(p => ReferenceEquals(p.Table, _current)).Select(p => p.Text).ToList();
        lHint.Text = string.Join(Environment.NewLine, problems);
        lHint.ForeColor = problems.Count > 0 ? SettingsWindow.ProblemColor(lHint) : _hintColor;
    }

    private void bAdd_Click(object? sender, EventArgs e)
    {
        if (_ctx.Document.TableCatalogs.FirstOrDefault() is not { } catalog)
        {
            Utils.ShowError(Resources.PhysicalTablesPage_Bez_katalogu);
            return;
        }

        var name = TableRules.Unique(_ctx.Document.TablePhysicals.Select(t => t.Name), Resources.TablesPage_Nova_tabula);
        var manufacturer = catalog.Manufacturer;
        var table = new TablePhysical
        {
            Name = name,
            Key = TableRules.Unique(_ctx.Document.TablePhysicals.Select(t => t.Key), name),
            TableCatalog = catalog,
            ID = manufacturer is { IsKnownToIniss: true, MinAddress: >= 1 } ? manufacturer.MinAddress : 1,
            CommunicationPort = _ctx.Document.TablePhysicals.LastOrDefault()?.CommunicationPort ?? 1,
            RecCount = catalog.MaxRecCount,
            SaveXML = "",
            ReverseArrows = "",
            Rem = "",
            Comment = ""
        };

        _ctx.Document.TablePhysicals.Add(table);
        Added(table);
    }

    private void bDuplicate_Click(object? sender, EventArgs e)
    {
        if (_current is not { } source)
            return;

        var table = new TablePhysical
        {
            Name = TableRules.Unique(_ctx.Document.TablePhysicals.Select(t => t.Name), source.Name),
            Key = TableRules.Unique(_ctx.Document.TablePhysicals.Select(t => t.Key), source.Key),
            TableCatalog = source.TableCatalog,
            ID = source.ID,
            CommunicationPort = source.CommunicationPort,
            RecCount = source.RecCount,
            SaveXML = source.SaveXML,
            ReverseArrows = source.ReverseArrows,
            Rem = source.Rem,
            Comment = source.Comment
        };

        _ctx.Document.TablePhysicals.Insert(_ctx.Document.TablePhysicals.IndexOf(source) + 1, table);
        Added(table);
    }

    private void Added(TablePhysical table)
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

        var index = _ctx.Document.TablePhysicals.IndexOf(table);
        _ctx.Document.TablePhysicals.RemoveAt(index);
        _list.Fill(_ctx.Document.TablePhysicals.Count == 0 ? null : _ctx.Document.TablePhysicals[Math.Min(index, _ctx.Document.TablePhysicals.Count - 1)]);
        Check();
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
