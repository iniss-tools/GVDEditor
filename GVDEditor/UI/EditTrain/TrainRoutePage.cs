using System.Globalization;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Settings;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.UI.EditTrain;

/// <summary>
/// Stranka Trasa a casy v okne vlaku - trasa v poradi jazdy: stanice zo smeru, tato stanica s casmi, kolajami
/// a linkami, stanice do smeru. Cas prichodu sa zadava len pri vlaku s trasou zo smeru, cas odchodu s trasou do smeru.
/// </summary>
public partial class TrainRoutePage : UserControl, ITrainPage
{
    private readonly FieldMarks _marks = new();
    private TrainDraft _draft = null!;
    private Color _hintColor;
    private bool _loading;

    // cast trasy, do ktorej prida stanicu dvojklik v zozname - naposledy pouzita
    private RouteEditor _target = null!;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainRoutePage()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    /// Naplni stranku udajmi konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="station">stanica grafikonu</param>
    internal void LoadData(TrainDraft draft, Station? station)
    {
        _draft = draft;
        foreach (var header in new[] { lStationsHeader, lFromHeader, lToHeader, lStationName })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        _marks.Capture(mtArrival, mtDeparture, tbArrLine, tbDepLine);
        lStationName.Text = string.Format(CultureInfo.CurrentCulture, Resources.TrainRoutePage_TatoStanica, station?.Name);

        _loading = true;
        cbArrTrack.DataSource = GlobData.Tracks.ToList();
        cbDepTrack.DataSource = GlobData.Tracks.ToList();
        cbArrTrack.SelectedItem = draft.Track;
        cbDepTrack.SelectedItem = draft.TrackDeparture ?? draft.Track;
        mtArrival.Text = draft.ArrivalText;
        mtDeparture.Text = draft.DepartureText;
        tbArrLine.Text = draft.LineArrival;
        tbDepLine.Text = draft.LineDeparture;
        routeFrom.Bind(draft.RouteFrom);
        routeTo.Bind(draft.RouteTo);
        routeFrom.Changed += (_, _) => Route_Changed(routeFrom);
        routeTo.Changed += (_, _) => Route_Changed(routeTo);
        _target = draft.RouteFrom.Count == 0 && draft.RouteTo.Count != 0 ? routeTo : routeFrom;
        FillStations();
        _loading = false;
        UpdateRoute();
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => IsMine(field);

    private static bool IsMine(TrainRules.Field field) =>
        field is TrainRules.Field.Arrival or TrainRules.Field.Departure or TrainRules.Field.Route or TrainRules.Field.Track
            or TrainRules.Field.LineArrival or TrainRules.Field.LineDeparture;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        var mine = problems.Where(problem => IsMine(problem.Field)).ToList();
        _marks.Mark(mine.Where(problem => !problem.IsWarning).Select(problem => FieldControl(problem.Field)));
        TrainPageHint.Show(lHint, mine, _hintColor);
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem) => FieldControl(problem.Field).Focus();

    private Control FieldControl(TrainRules.Field field) => field switch
    {
        TrainRules.Field.Arrival => mtArrival,
        TrainRules.Field.Departure => mtDeparture,
        TrainRules.Field.Track => cbArrTrack,
        TrainRules.Field.LineArrival => tbArrLine,
        TrainRules.Field.LineDeparture => tbDepLine,
        _ => tbSearch
    };

    private void OnChanged()
    {
        if (!_loading)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Cas prichodu ma zmysel len s trasou zo smeru, cas odchodu s trasou do smeru; zadany cas sa pri
    /// vyprazdneni trasy nemaze, aby sa po jej doplneni vratil.
    /// </summary>
    private void UpdateRoute()
    {
        mtArrival.Enabled = _draft.RouteFrom.Count != 0;
        mtDeparture.Enabled = _draft.RouteTo.Count != 0;

        var routing = _draft.Routing;
        lRouting.Text = routing == Routing.Prechadzajuci ? Resources.TrainRoutePage_Prechadzajuci
            : routing == Routing.Konciaci ? Resources.TrainRoutePage_Konciaci
            : routing == Routing.Vychadzajuci ? Resources.TrainRoutePage_Vychadzajuci
            : Resources.TrainRoutePage_BezTrasy;
    }

    private void Route_Changed(RouteEditor editor)
    {
        _target = editor;
        UpdateRoute();
        OnChanged();
    }

    private void routeFrom_Enter(object? sender, EventArgs e) => _target = routeFrom;

    private void routeTo_Enter(object? sender, EventArgs e) => _target = routeTo;

    // ---------------------------------------------------------------- dostupne stanice

    /// <summary>
    /// Stanice zo zvukovej banky alebo vlastne stanice grafikonu, ktorych nazov obsahuje hladany text
    /// (bez ohladu na velkost pismen a diakritiku).
    /// </summary>
    private void FillStations()
    {
        var search = Normalize(tbSearch.Text);
        var source = cbCustom.Checked ? (IEnumerable<Station>)GlobData.CustomStations : GlobData.Stations;
        var selected = listStations.SelectedItem as Station;

        listStations.BeginUpdate();
        listStations.Items.Clear();
        foreach (var station in source)
            if (search.Length == 0 || Normalize(station.Name).Contains(search, StringComparison.Ordinal))
                listStations.Items.Add(station);
        listStations.EndUpdate();

        if (selected != null && listStations.Items.IndexOf(selected) is var index and >= 0)
            listStations.SelectedIndex = index;
        else if (listStations.Items.Count != 0)
            listStations.SelectedIndex = 0;
        bAddFrom.Enabled = bAddTo.Enabled = listStations.SelectedIndex != -1;
    }

    private static string Normalize(string text) => StringUtils.RemoveDiacritics(text.Trim()).ToLowerInvariant();

    private void Filter_Changed(object? sender, EventArgs e) => FillStations();

    private void AddSelected(RouteEditor target)
    {
        if (listStations.SelectedItem is not Station station)
            return;

        target.Add(station);
        _target = target;
    }

    private void bAddFrom_Click(object? sender, EventArgs e) => AddSelected(routeFrom);

    private void bAddTo_Click(object? sender, EventArgs e) => AddSelected(routeTo);

    private void listStations_DoubleClick(object? sender, EventArgs e) => AddSelected(_target);

    private void listStations_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        AddSelected(_target);
        e.Handled = e.SuppressKeyPress = true;
    }

    // sipky z hladania prechadzaju zoznamom, Enter prida vybranu stanicu
    private void tbSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Down when listStations.SelectedIndex < listStations.Items.Count - 1:
                listStations.SelectedIndex++;
                break;
            case Keys.Up when listStations.SelectedIndex > 0:
                listStations.SelectedIndex--;
                break;
            case Keys.Enter:
                AddSelected(_target);
                break;
            default:
                return;
        }

        e.Handled = e.SuppressKeyPress = true;
    }

    // ---------------------------------------------------------------- tato stanica

    private void Time_TextChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.ArrivalText = mtArrival.Text;
        _draft.DepartureText = mtDeparture.Text;
        OnChanged();
    }

    private void Line_TextChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.LineArrival = tbArrLine.Text.Trim();
        _draft.LineDeparture = tbDepLine.Text.Trim();
        OnChanged();
    }

    // kolaj odchodu ide s kolajou prichodu, kym sa nezmeni zvlast - vlak v stanici prechadza na inu kolaj zriedka
    private void cbArrTrack_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        var previous = _draft.Track;
        _draft.Track = cbArrTrack.SelectedItem as Track;
        if (previous == null || _draft.TrackDeparture == null || _draft.TrackDeparture.EqualsKeys(previous))
        {
            cbDepTrack.SelectedItem = _draft.Track;
            _draft.TrackDeparture = _draft.Track;
        }

        OnChanged();
    }

    private void cbDepTrack_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        _draft.TrackDeparture = cbDepTrack.SelectedItem as Track;
        OnChanged();
    }
}
