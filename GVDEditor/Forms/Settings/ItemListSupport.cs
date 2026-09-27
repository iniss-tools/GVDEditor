using ExControls;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Zoznam poloziek vlavo na strankach so zoznamom a udajmi vybranej polozky (tabule, texty): naplnenie,
///     vyhladavanie, vyber a oznacenie poloziek s chybou. Zmenu vyberu oznami az po skonceni udalosti tabulky -
///     stranka vtedy meni tlacidla a pole, co by uprostred zmeny bunky skoncilo vnorenym volanim.
/// </summary>
/// <typeparam name="T">polozka zoznamu</typeparam>
internal sealed class ItemListSupport<T> where T : class
{
    private readonly DataGridView _dgv;
    private readonly ExTextBox _filter;
    private readonly Func<IEnumerable<T>> _items;
    private readonly Func<T, object?[]> _cells;
    private bool _loading;
    private bool _deferred;

    /// <summary>
    ///     Pripoji zoznam k tabulke <paramref name="dgv" /> a vyhladavaciemu polu <paramref name="filter" />.
    /// </summary>
    /// <param name="dgv">tabulka zoznamu</param>
    /// <param name="filter">pole na vyhladavanie</param>
    /// <param name="items">polozky v poradi</param>
    /// <param name="cells">hodnoty buniek riadka polozky</param>
    public ItemListSupport(DataGridView dgv, ExTextBox filter, Func<IEnumerable<T>> items, Func<T, object?[]> cells)
    {
        _dgv = dgv;
        _filter = filter;
        _items = items;
        _cells = cells;
        dgv.AutoGenerateColumns = false;
        dgv.CurrentCellChanged += (_, _) => RaiseSelectionChanged();
        filter.TextChanged += (_, _) =>
        {
            ApplyFilter();
            RaiseSelectionChanged();
        };
    }

    /// <summary>
    ///     Zmenil sa vybrany riadok (oznamene az po skonceni udalosti tabulky).
    /// </summary>
    public event EventHandler? SelectionChanged;

    /// <summary>
    ///     Vybrana polozka.
    /// </summary>
    public T? Current => _dgv.CurrentRow?.Tag as T;

    /// <summary>
    ///     Plocha tabulky bez riadkov dostane farbu buniek - volat po nastaveni temy.
    /// </summary>
    public void CaptureColors()
    {
        var cells = _dgv.DefaultCellStyle.BackColor;
        _dgv.BackgroundColor = cells.IsEmpty ? SystemColors.Window : cells;
    }

    /// <summary>
    ///     Naplni zoznam a vyberie <paramref name="select" /> (alebo prvu viditelnu polozku).
    /// </summary>
    public void Fill(T? select)
    {
        _loading = true;
        _dgv.Rows.Clear();
        foreach (var item in _items())
            _dgv.Rows[_dgv.Rows.Add(_cells(item))].Tag = item;
        ApplyFilter();
        _loading = false;

        var row = Rows.FirstOrDefault(r => r.Visible && ReferenceEquals(r.Tag, select)) ?? Rows.FirstOrDefault(r => r.Visible);
        if (row is not null)
            _dgv.CurrentCell = row.Cells[0];
        RaiseSelectionChanged();
    }

    /// <summary>
    ///     Obnovi bunky riadka polozky po zmene jej udajov.
    /// </summary>
    public void Refresh(T item)
    {
        if (Rows.FirstOrDefault(r => ReferenceEquals(r.Tag, item)) is not { } row)
            return;

        var values = _cells(item);
        for (var i = 0; i < values.Length && i < row.Cells.Count; i++)
            row.Cells[i].Value = values[i];
    }

    /// <summary>
    ///     Vyberie polozku; ak ju vyhladavanie skryva, vyhladavanie zrusi.
    /// </summary>
    public void Select(T item)
    {
        if (Rows.FirstOrDefault(r => ReferenceEquals(r.Tag, item)) is not { } row)
            return;

        if (!row.Visible)
        {
            _filter.Text = "";
            ApplyFilter();
        }

        _dgv.CurrentCell = row.Cells[0];
    }

    /// <summary>
    ///     Pri polozkach s chybou ukaze v prvej bunke ikonu s textom chyby.
    /// </summary>
    public void MarkProblems(Func<T, string?> problem)
    {
        foreach (var row in Rows)
            row.Cells[0].ErrorText = row.Tag is T item ? problem(item) ?? "" : "";
    }

    private IEnumerable<DataGridViewRow> Rows => _dgv.Rows.Cast<DataGridViewRow>();

    private void ApplyFilter()
    {
        var filter = _filter.Text.Trim();
        foreach (var row in Rows)
        {
            var visible = filter.Length == 0 || row.Cells.Cast<DataGridViewCell>()
                .Any(c => (c.Value as string ?? "").Contains(filter, StringComparison.CurrentCultureIgnoreCase));
            if (!visible && _dgv.CurrentRow == row)
                _dgv.CurrentCell = null;
            row.Visible = visible;
        }
    }

    private void RaiseSelectionChanged()
    {
        if (_loading)
            return;

        if (!_dgv.IsHandleCreated)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (_deferred)
            return;

        _deferred = true;
        _dgv.BeginInvoke(() =>
        {
            _deferred = false;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        });
    }
}
