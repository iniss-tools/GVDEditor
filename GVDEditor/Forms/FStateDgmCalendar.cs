using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;
using JetBrains.Annotations;
using ToolsCore.Expressions;
using ToolsCore.StateDgm;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Nahlad Kalendara akcii vlaku: pre vybrany vlak a simulovany prevadzkovy stav urci kategoriu (IndCat) a pre kazdy
///     stav kategorie vypocita to, co ukazuje INISS - rezim automatiky, ci je akcia naplanovana (AutoCondition),
///     cas spustenia (casovy bod + posun + meskanie), cakanie na ILTIS a druh hlasenia. Dynamicke hodnoty sa
///     vyhodnocuju rovnakym evaluatorom ako podmienky TabTab.
/// </summary>
public partial class FStateDgmCalendar : Form
{
    private readonly Func<StateDgmDiagram?> _diagram;
    private readonly int _homeStationId;
    private readonly GvdExprSymbols _symbols = new();
    private readonly BindingList<CalendarRow> _rows = [];
    private bool _loading = true;

    /// <summary>
    ///     Vytvori nahlad.
    /// </summary>
    /// <param name="diagram">Aktualny diagram (z editora alebo zo suboru); null = diagram nie je.</param>
    /// <param name="homeStationId">ID stanice grafikonu (pre vyrazy).</param>
    /// <param name="train">Vlak, ktory sa ma predvolit.</param>
    internal FStateDgmCalendar(Func<StateDgmDiagram?> diagram, int homeStationId, Train? train = null)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();
        if (GlobData.UsingStyle.DarkTitleBar) ExTools.SetImmersiveDarkMode(Handle, true);

        _diagram = diagram;
        _homeStationId = homeStationId;

        cbTrain.DisplayMember = nameof(TrainItem.Text);
        foreach (var t in GlobData.Trains.OrderBy(t => t.Arrival ?? t.Departure))
            cbTrain.Items.Add(new TrainItem(t));
        if (cbTrain.Items.Count > 0)
            cbTrain.SelectedIndex = Math.Max(0, cbTrain.Items.Cast<TrainItem>().ToList().FindIndex(i => i.Train == train));

        dgvCalendar.DataSource = _rows;
        _loading = false;
        Recompute();
    }

    /// <summary>Pouzivatel vybral riadok stavu.</summary>
    public event EventHandler<StateDgmState>? StateSelected;

    /// <summary>Kategoria, do ktorej vybrany vlak patri (po poslednom prepocte).</summary>
    public StateDgmCategory? Category { get; private set; }

    /// <summary>Prepocita po zmene diagramu v editore.</summary>
    public void RefreshPreview() => Recompute();

    private sealed record TrainItem(Train Train)
    {
        public string Text => $"{Train.Type.Key} {Train.Number}{(string.IsNullOrEmpty(Train.Name) ? "" : " " + Train.Name)}  "
                              + $"{Train.Arrival?.ToString("HH:mm") ?? "–"} / {Train.Departure?.ToString("HH:mm") ?? "–"}  {Train.Routing.Symbol}";
    }

    /// <summary>Riadok kalendara (vlastnosti su DataPropertyName stlpcov).</summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal sealed class CalendarRow
    {
        public StateDgmState State { get; init; } = null!;
        public string Event { get; init; } = "";
        public string Mode { get; init; } = "";
        public bool Planned { get; init; }
        public string TimePoint { get; init; } = "";
        public string Add { get; init; } = "";
        public string Delay { get; init; } = "";
        public string Result { get; init; } = "";
        public string Wait { get; init; } = "";
        public bool Short { get; init; }
        public string Note { get; init; } = "";
    }

    private void Input_Changed(object sender, EventArgs e)
    {
        if (!_loading) Recompute();
    }

    private void Recompute()
    {
        var selectedKey = (dgvCalendar.CurrentRow?.DataBoundItem as CalendarRow)?.State.Key;
        _rows.RaiseListChangedEvents = false;
        _rows.Clear();
        Category = null;
        lCategory.Text = "";

        var d = _diagram();
        if (d == null)
        {
            lCategory.Text = Resources.FLocalSettings_SD_Chyba_Nie;
        }
        else if (cbTrain.SelectedItem is TrainItem ti)
        {
            var runtime = new TrainRuntime
            {
                ArrivalDelayMinutes = (int)nudDelayArr.Value,
                DepartureDelayMinutes = (int)nudDelayDep.Value,
                IsStanding = chkStanding.Checked,
                LockoutArrival = chkLockArr.Checked,
                LockoutDeparture = chkLockDep.Checked,
                IsDeflected = chkDeflected.Checked
            };
            var ctx = new GvdTrainContext(ti.Train, runtime, _homeStationId);
            var eval = new ExprEvaluator(ctx, ExprEvalSite.Default, runtime.Now);

            // kategoria podla IndCat (mimo rozsahu → posledna, ako INISS)
            var (cat, catNote) = Evaluate(eval, d.EffectiveIndCat, ExprContext.Condition);
            var index = cat ?? 0;
            if (d.Categories.Count > 0)
            {
                Category = index >= 1 && index <= d.Categories.Count ? d.Categories[index - 1] : d.Categories[^1];
                lCategory.Text = string.Format(Resources.FStateDgmCalendar_Kategoria, Category.Name.Length > 0 ? Category.Name : Category.Key, cat?.ToString() ?? "?")
                                 + (catNote != null ? "  –  " + catNote : "")
                                 + (index < 1 || index > d.Categories.Count ? "  –  " + Resources.FStateDgmCalendar_MimoRozsahu : "");
            }

            if (Category != null)
                foreach (var s in Category.States)
                    _rows.Add(Row(s, ti.Train, runtime, eval));
        }

        _rows.RaiseListChangedEvents = true;
        _rows.ResetBindings();
        if (selectedKey != null)
            foreach (DataGridViewRow r in dgvCalendar.Rows)
                if (r.DataBoundItem is CalendarRow cr && cr.State.Key == selectedKey)
                {
                    dgvCalendar.CurrentCell = r.Cells[0];
                    break;
                }
    }

    private (int? Value, string? Note) Evaluate(ExprEvaluator eval, string? text, ExprContext context)
    {
        if (string.IsNullOrWhiteSpace(text)) return (null, null);
        var p = ExprParser.Parse(text, context, _symbols);
        if (!p.Success) return (null, string.Format(Resources.FStateDgmCalendar_ChybaVyrazu, text, p.Error?.Message));
        try
        {
            return (eval.Evaluate(p.Root!), null);
        }
        catch (ExprEvaluationException e)
        {
            return (null, string.Format(Resources.FStateDgmCalendar_ChybaVyrazu, text, e.Message));
        }
    }

    private (int? Value, string? Note) Dynamic(ExprEvaluator eval, StateDgmDynamic? v, ExprContext context = ExprContext.Condition)
    {
        if (v == null) return (null, null);
        if (!v.IsExpression) return (v.Number, null);
        return Evaluate(eval, v.Expression, context);
    }

    private CalendarRow Row(StateDgmState s, Train train, TrainRuntime runtime, ExprEvaluator eval)
    {
        var notes = new List<string>();
        void Note(string? n)
        {
            if (n != null) notes.Add(n);
        }

        // INISS: najprv ciselne hodnoty, potom ich prepise vysledok vyrazu; vysledok mimo rozsahu sa ignoruje
        var (mode, n1) = Dynamic(eval, s.AutoMode);
        Note(n1);
        if (mode is < 0 or > 2) mode = s.AutoMode is { IsExpression: false } ? s.AutoMode.Number : 0;
        var (tp, n2) = Dynamic(eval, s.AutoTimePoint);
        Note(n2);
        if (tp is not (1 or 2)) tp = s.AutoTimePoint is { IsExpression: false } ? s.AutoTimePoint.Number : null;
        var (add, n3) = Dynamic(eval, s.AutoTimePointAdd);
        Note(n3);
        var (modif, n4) = Dynamic(eval, s.AutoModif);
        Note(n4);
        var (wait, n5) = Dynamic(eval, s.Wait, ExprContext.StateDgmWait);
        Note(n5);
        var planned = (mode ?? 0) > 0;
        if (planned && s.AutoCondition != null)
        {
            var (cond, n6) = Evaluate(eval, s.AutoCondition, ExprContext.Condition);
            Note(n6);
            planned = cond is not 0 && cond != null;
        }

        DateTime? baseTime = tp == 1 ? train.Arrival : tp == 2 ? train.Departure : null;
        var delay = tp == 1 ? runtime.ArrivalDelayMinutes : tp == 2 ? runtime.DepartureDelayMinutes : 0;
        var result = baseTime?.AddSeconds(add ?? 0).AddMinutes(delay);
        var waitNames = wait is { } w && w != 0 ? StateDgmDynamic.WaitName((StateDgmWaitEvent)unchecked((uint)w)) : "";

        return new CalendarRow
        {
            State = s,
            Event = s.Name.Length > 0 ? s.Name : s.Key,
            Mode = mode switch
            {
                1 => Resources.FStateDgmCalendar_Poloautomat,
                2 => Resources.FStateDgmCalendar_Automat,
                _ => Resources.FStateDgmCalendar_Manualny
            },
            Planned = planned,
            TimePoint = tp switch
            {
                1 => Resources.FStateDgm_Graf_Prichod,
                2 => Resources.FStateDgm_Graf_Odchod,
                _ => ""
            },
            Add = add is { } a && (mode ?? 0) > 0 ? a.ToString() : "",
            Delay = tp != null && (mode ?? 0) > 0 ? (delay * 60).ToString() : "",
            Result = (mode ?? 0) > 0 ? result?.ToString("HH:mm:ss") ?? "–" : "",
            Wait = waitNames,
            Short = modif == 1,
            Note = string.Join("; ", notes)
        };
    }

    private void dgvCalendar_SelectionChanged(object sender, EventArgs e)
    {
        if (!_loading && dgvCalendar.CurrentRow?.DataBoundItem is CalendarRow r)
            StateSelected?.Invoke(this, r.State);
    }
}
