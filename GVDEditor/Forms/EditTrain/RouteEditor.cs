using GVDEditor.Entities;

namespace GVDEditor.Forms.EditTrain;

/// <summary>
///     Jedna cast trasy vlaku (zo smeru alebo do smeru): stanice v poradi jazdy so stlpcami dlheho a kratkeho hlasenia.
///     Meni priamo zoznam stanic konceptu vlaku.
/// </summary>
public partial class RouteEditor : UserControl
{
    private BindingList<Station> _stations = [];

    /// <summary>
    ///     Vytvori prazdny zoznam; stanice priradi <see cref="Bind" />.
    /// </summary>
    public RouteEditor()
    {
        InitializeComponent();
        dgv.AutoGenerateColumns = false;
    }

    /// <summary>
    ///     Pridala alebo odobrala sa stanica, alebo sa zmenilo ich poradie.
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    ///     Pocet stanic trasy.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Count => _stations.Count;

    /// <summary>
    ///     Upravuje zoznam <paramref name="stations" /> (zoznam konceptu, nie kopiu).
    /// </summary>
    internal void Bind(List<Station> stations)
    {
        _stations = new BindingList<Station>(stations);
        dgv.DataSource = _stations;
        UpdateButtons();
    }

    /// <summary>
    ///     Prida stanicu na koniec trasy (nova stanica je v dlhom hlaseni) a vyberie ju.
    /// </summary>
    internal void Add(Station station)
    {
        _stations.Add(new Station(station.ID, station.Name, IsInLongReport: true));
        Select(_stations.Count - 1);
        OnChanged();
    }

    private int SelectedIndex => dgv.SelectedRows.Count == 0 ? -1 : dgv.SelectedRows[0].Index;

    private void Select(int index)
    {
        if (index < 0 || index >= dgv.Rows.Count)
            return;

        dgv.ClearSelection();
        dgv.Rows[index].Selected = true;
        dgv.CurrentCell = dgv.Rows[index].Cells[colName.Index];
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        var index = SelectedIndex;
        bUp.Enabled = index > 0;
        bDown.Enabled = index >= 0 && index < _stations.Count - 1;
        bRemove.Enabled = index >= 0;
        bClear.Enabled = _stations.Count != 0;
    }

    private void OnChanged()
    {
        UpdateButtons();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void MoveSelected(int offset)
    {
        var index = SelectedIndex;
        var target = index + offset;
        if (index < 0 || target < 0 || target >= _stations.Count)
            return;

        var station = _stations[index];
        _stations.RemoveAt(index);
        _stations.Insert(target, station);
        Select(target);
        OnChanged();
    }

    private void Remove()
    {
        var index = SelectedIndex;
        if (index < 0)
            return;

        _stations.RemoveAt(index);
        Select(Math.Min(index, _stations.Count - 1));
        OnChanged();
    }

    private void bUp_Click(object? sender, EventArgs e) => MoveSelected(-1);

    private void bDown_Click(object? sender, EventArgs e) => MoveSelected(1);

    private void bRemove_Click(object? sender, EventArgs e) => Remove();

    // zmena sa prevezme az tlacidlom OK okna, takze nechcene vymazanie vrati Zrusit
    private void bClear_Click(object? sender, EventArgs e)
    {
        _stations.Clear();
        OnChanged();
    }

    private void dgv_SelectionChanged(object? sender, EventArgs e) => UpdateButtons();

    // zaskrtnutie Dlhe/Kratke sa zapise hned, nie az pri opusteni bunky
    private void dgv_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgv.IsCurrentCellDirty && dgv.CurrentCell is DataGridViewCheckBoxCell)
            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Delete:
                Remove();
                break;
            case Keys.Up when e.Control:
                MoveSelected(-1);
                break;
            case Keys.Down when e.Control:
                MoveSelected(1);
                break;
            default:
                return;
        }

        e.Handled = true;
    }
}
