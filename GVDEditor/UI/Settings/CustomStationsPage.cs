using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Stranka Vlastne stanice v okne Lokalne nastavenia - stanice mimo zvukovej banky (Stanice.txt) s upravou
/// priamo v tabulke. Zmeny idu rovno do <see cref="_ctx.Document.CustomStations" />, Zrusit okna ich vrati.
/// </summary>
public partial class CustomStationsPage : UserControl, ISettingsPage
{
    /// <summary>
    /// Kontext editora - nastavi ho <c>LoadData</c>.
    /// </summary>
    private EditorContext _ctx = null!;

    private readonly GridPageSupport _grid;
    private string _gvdStationName = "";
    private bool _loading;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public CustomStationsPage()
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
    /// Naplni tabulku vlastnymi stanicami - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name=\"context\">kontext editora</param>
    /// <param name="gvdStationName">nazov stanice grafikonu (vlastna stanica sa nesmie volat rovnako)</param>
    internal void LoadData(EditorContext context, string gvdStationName)
    {
        _ctx = context;
        _gvdStationName = gvdStationName;
        _grid.CaptureColors();
        _loading = true;
        dgv.Rows.Clear();
        foreach (var station in _ctx.Document.CustomStations)
            AddRow(station);
        _loading = false;

        Check();
    }

    private int AddRow(Station station)
    {
        var trains = CountTrains(station.ID);
        var index = dgv.Rows.Add(station.ID, station.Name, trains);
        var row = dgv.Rows[index];
        row.Tag = station;

        // vlaky sa na stanicu odkazuju cislom - zmena by ich odtrhla
        var id = row.Cells[colId.Index];
        id.ReadOnly = trains > 0;
        if (trains > 0)
        {
            id.ToolTipText = string.Format(CultureInfo.CurrentCulture, Resources.CustomStationsPage_Cislo_zamknute, trains);
            GridPageSupport.MarkLocked(id);
        }
        return index;
    }

    private int CountTrains(string id) =>
        _ctx.Document.Trains.Count(train =>
            train.StartingStation?.ID == id || train.EndingStation?.ID == id ||
            train.StaniceZoSmeru.Any(station => station.ID == id) || train.StaniceDoSmeru.Any(station => station.ID == id));

    // Station je record - dve rovnake stanice by IndexOf/Remove podla hodnoty zamenili
    private int IndexOf(Station station)
    {
        for (var i = 0; i < _ctx.Document.CustomStations.Count; i++)
            if (ReferenceEquals(_ctx.Document.CustomStations[i], station))
                return i;
        return -1;
    }

    private Station? CurrentStation => dgv.CurrentRow?.Tag as Station;

    private void Check()
    {
        _grid.BeginCheck();
        var stations = dgv.Rows.Cast<DataGridViewRow>().Select(row => (Station)row.Tag!).ToList();
        var ids = stations.Select(s => s.ID).ToList();
        var names = stations.Select(s => s.Name).ToList();
        for (var i = 0; i < dgv.Rows.Count; i++)
        {
            _grid.Report(dgv.Rows[i].Cells[colId.Index], CustomStationRules.CheckId(ids, i, _ctx.Workspace.Stations));
            _grid.Report(dgv.Rows[i].Cells[colName.Index], CustomStationRules.CheckName(names, i, _ctx.Workspace.Stations, _gvdStationName));
        }

        _grid.Defer(UpdateSelection);
        ProblemsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateSelection()
    {
        bDelete.Enabled = CurrentStation is not null;
        _grid.ShowHint(dgv.CurrentCell is { ReadOnly: true, ColumnIndex: var col } cell && col == colId.Index ? cell.ToolTipText : null);
    }

    private void bAdd_Click(object sender, EventArgs e)
    {
        var id = CustomStationRules.SuggestId(_ctx.Workspace.Stations.Concat(_ctx.Document.CustomStations).Select(s => s.ID));
        var station = new Station(id, "", IsCustom: true);
        _ctx.Document.CustomStations.Add(station);

        _loading = true;
        var index = AddRow(station);
        _loading = false;

        Check();
        _grid.Edit(index, colName);
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
        var station = CurrentStation;
        if (station is null)
            return;

        var trains = CountTrains(station.ID);
        if (trains > 0 && Utils.ShowQuestion(string.Format(CultureInfo.CurrentCulture,
                Resources.CustomStationsPage_Odstranit_pouzitu, station.Name, trains)) != DialogResult.Yes)
            return;

        _ctx.Document.CustomStations.RemoveAt(IndexOf(station));
        dgv.Rows.RemoveAt(dgv.CurrentRow!.Index);
        Check();
    }

    private void dgv_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loading || e.RowIndex < 0)
            return;

        var row = dgv.Rows[e.RowIndex];
        var station = (Station)row.Tag!;
        var value = (row.Cells[e.ColumnIndex].Value?.ToString() ?? "").Trim();
        if (e.ColumnIndex == colId.Index)
            station.ID = value;
        else if (e.ColumnIndex == colName.Index)
            station.Name = value;
        else
            return;

        _ctx.Document.CustomStations.ResetItem(IndexOf(station));
        Check();
    }

    private void dgv_CurrentCellChanged(object? sender, EventArgs e) => _grid.Defer(UpdateSelection);

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && !dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            dgv.BeginEdit(true);
    }

    private void dgv_KeyDown(object? sender, KeyEventArgs e) =>
        GridPageSupport.HandleKeys(e, () => bAdd_Click(this, EventArgs.Empty), () => bDelete_Click(this, EventArgs.Empty));
}
