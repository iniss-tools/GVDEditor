using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Dopravcovia v okne Lokalne nastavenia - ciselnik dopravcov grafikonu (Vlastnik.txt) s upravou
/// priamo v tabulke. Zmeny idu rovno do <see cref="_ctx.Document.Operators" />, Zrusit okna ich vrati.
/// </summary>
public partial class OperatorsPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private readonly GridPageSupport _grid;

    // naplnanie tabulky kodom nema spustat zapis do dopravcov
    private bool _loading;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public OperatorsPage()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
        _grid = new GridPageSupport(dgv, lHint);
    }

    /// <inheritdoc />
    public event EventHandler? ProblemsChanged;

    /// <inheritdoc />
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirstProblem => _grid.FirstProblem;

    /// <inheritdoc />
    public void FocusFirstProblem() => _grid.FocusFirstProblem();

    /// <summary>
    /// Naplni tabulku dopravcami grafikonu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    internal void LoadData(EditorContext context)
    {
        _ctx = context;
        _grid.CaptureColors();
        _loading = true;
        dgv.Rows.Clear();
        foreach (var op in _ctx.Document.Operators)
            if (op != Operator.None)
                AddRow(op);
        _loading = false;

        Check();
    }

    private int AddRow(Operator op)
    {
        var index = dgv.Rows.Add(op.Id.ToString(CultureInfo.InvariantCulture), op.Name, CountTrains(op));
        dgv.Rows[index].Tag = op;
        return index;
    }

    private int CountTrains(Operator op) => _ctx.Document.Trains.Count(train => train.Operator == op);

    private Operator? CurrentOperator => dgv.CurrentRow?.Tag as Operator;

    private void Check()
    {
        _grid.BeginCheck();
        var names = dgv.Rows.Cast<DataGridViewRow>().Select(row => ((Operator)row.Tag!).Name).ToList();
        for (var i = 0; i < dgv.Rows.Count; i++)
            _grid.Report(dgv.Rows[i].Cells[colName.Index], OperatorRules.CheckName(names, i));

        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
        var op = new Operator(OperatorRules.NextId(_ctx.Document.Operators.Select(o => o.Id)), "");
        _ctx.Document.Operators.Add(op);

        _loading = true;
        var index = AddRow(op);
        _loading = false;

        Check();
        _grid.Edit(index, colName);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        var op = CurrentOperator;
        if (op is null)
            return;

        var trains = _ctx.Document.Trains.Where(train => train.Operator == op).ToList();
        if (trains.Count > 0 && Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture,
                Resources.OperatorsPage_Odstranit_pouzity, op.Name, trains.Count)) != DialogResult.Yes)
            return;

        foreach (var train in trains)
            train.Operator = Operator.None;

        _ctx.Document.Operators.Remove(op);
        dgv.Rows.RemoveAt(dgv.CurrentRow!.Index);
        Check();
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || e.ColumnIndex != colName.Index)
            return;

        var row = dgv.Rows[e.RowIndex];
        var op = (Operator)row.Tag!;
        op.Name = (row.Cells[e.ColumnIndex].Value as string ?? "").Trim();
        _ctx.Document.Operators.ResetItem(_ctx.Document.Operators.IndexOf(op));
        Check();
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void UpdateSelection()
    {
        bDelete.Enabled = CurrentOperator is not null;
        _grid.ShowHint(null);
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
