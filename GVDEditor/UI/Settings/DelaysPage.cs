using GVDEditor.Properties;
using GVDEditor.Domain.Rules;

namespace GVDEditor.UI.Settings;

/// <summary>
///     Stranka Meskania v okne Globalne nastavenia - casy meskania ponukane operatorovi (Zpozdeni.txt), zoradene
///     podla velkosti. Zmeny idu rovno do <see cref="GlobData.Delays" />, Zrusit okna ich vrati.
/// </summary>
public partial class DelaysPage : UserControl, ISettingsPage
{
    private readonly GridPageSupport _grid;
    private bool _loading;

    // riadok pridany tlacidlom Novy cas - ak ostane prazdny, po skonceni upravy zmizne
    private int _newRow = -1;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public DelaysPage()
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
    ///     Naplni tabulku casmi meskania - volat az po nastaveni temy okna.
    /// </summary>
    public void LoadData()
    {
        _grid.CaptureColors();
        Fill(-1);
    }

    private void Fill(int select)
    {
        _loading = true;
        dgv.Rows.Clear();
        foreach (var delay in GlobData.Delays)
            MarkNumber(dgv.Rows[dgv.Rows.Add(delay)].Cells[colValue.Index], delay);
        _loading = false;

        if (select >= 0 && select < dgv.Rows.Count)
            dgv.CurrentCell = dgv.Rows[select].Cells[colValue.Index];

        Check();
    }

    private static bool IsNumber(string delay) => delay.Trim().Length == 0 || DelayRules.IsAcceptedByIniss(delay);

    /// <summary>
    ///     Hodnota, ktoru INISS preskoci (nie je cislo), je sivá s vysvetlenim - chybou nie je.
    /// </summary>
    private static void MarkNumber(DataGridViewCell cell, string delay)
    {
        var number = IsNumber(delay);
        cell.Style.ForeColor = number ? Color.Empty : SystemColors.GrayText;
        cell.ToolTipText = number ? "" : Resources.DelaysPage_Necislo;
    }

    private void Check()
    {
        _grid.BeginCheck();
        for (var i = 0; i < dgv.Rows.Count; i++)
            _grid.Report(dgv.Rows[i].Cells[colValue.Index], DelayRules.CheckValue(GlobData.Delays, i));

        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
        GlobData.Delays.Add("");
        _loading = true;
        _newRow = dgv.Rows.Add("");
        _loading = false;

        Check();
        _grid.Edit(_newRow, colValue);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        if (dgv.CurrentRow is not { } row)
            return;

        var index = row.Index;
        GlobData.Delays.RemoveAt(index);
        _newRow = -1;
        Fill(Math.Min(index, GlobData.Delays.Count - 1));
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0 || e.ColumnIndex != colValue.Index)
            return;

        var value = (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string ?? "").Trim();
        GlobData.Delays[e.RowIndex] = value;

        // prerobenie riadkov priamo v udalosti tabulky by bolo vnorene volanie - az po skonceni upravy
        var index = e.RowIndex;
        BeginInvoke(() => Place(index));
    }

    private void dgv_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        var index = e.RowIndex;
        if (index != _newRow)
            return;

        _newRow = -1;

        // novy cas, ktory pouzivatel nevyplnil, sa neprida (CellValueChanged uz prebehol)
        if (index >= GlobData.Delays.Count || GlobData.Delays[index].Length != 0)
            return;

        BeginInvoke(() =>
        {
            GlobData.Delays.RemoveAt(index);
            Fill(Math.Min(index, GlobData.Delays.Count - 1));
        });
    }

    /// <summary>
    ///     Zaradi upraveny cas podla velkosti - INISS ponuka casy v poradi zo suboru.
    /// </summary>
    private void Place(int index)
    {
        if (index >= GlobData.Delays.Count)
            return;

        var value = GlobData.Delays[index];
        if (value.Length == 0)
        {
            Fill(index);
            return;
        }

        GlobData.Delays.RemoveAt(index);
        var position = DelayRules.InsertIndex(GlobData.Delays, value);
        GlobData.Delays.Insert(position, value);
        Fill(position);
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void UpdateSelection()
    {
        bDelete.Enabled = dgv.CurrentRow is not null;
        var value = dgv.CurrentRow?.Index is { } i && i < GlobData.Delays.Count ? GlobData.Delays[i] : "";
        _grid.ShowHint(IsNumber(value) ? null : Resources.DelaysPage_Necislo);
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
