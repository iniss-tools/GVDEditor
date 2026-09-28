using System.Globalization;
using ExControls;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.UI.Settings;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.EditTrain;

/// <summary>
/// Stranka Platnost v okne vlaku - datumove obmedzenie, obdobie platnosti a varianty vlaku: vlaky s rovnakym
/// cislom, nazvom a typom s pruhom kalendara ich dni. Cisla variant prideluje GVDEditor sam; prekrytie dni je len
/// upozornenie a spolocne dni sa daju pridelit jednej variante. Ine varianty sa zmenia az po ulozeni vlaku.
/// </summary>
public partial class TrainValidityPage : UserControl, ITrainPage
{
    private readonly FieldMarks _marks = new();
    private TrainDraft _draft = null!;
    private TrainContext _context = null!;
    private string _homeStation = "";
    private Action? _openCalendar;
    private List<Train> _others = [];
    private Dictionary<Train, string> _overlaps = new(ReferenceEqualityComparer.Instance);
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    /// Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainValidityPage()
    {
        InitializeComponent();
        dgvVariants.AutoGenerateColumns = false;
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    /// Naplni stranku udajmi konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="context">grafikon</param>
    /// <param name="homeStation">nazov stanice grafikonu (zaciatok alebo koniec trasy vychodzieho a konciaceho vlaku)</param>
    /// <param name="openCalendar">otvori Kalendar akcii vlaku; <see langword="null" />, ak sa neda zobrazit</param>
    internal void LoadData(TrainDraft draft, TrainContext context, string? homeStation, Action? openCalendar)
    {
        _draft = draft;
        _context = context;
        _homeStation = homeStation ?? "";
        _openCalendar = openCalendar;
        foreach (var header in new[] { lLimitHeader, lVariantHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        if (GlobData.UsingStyle.DarkScrollBar)
            pScroll.SetTheme(WindowsTheme.DarkExplorer);
        _marks.Capture(tbDateLimit);
        strip.RowLabel = row => $"{row.Position}/{_others.Count + 1}";
        strip.DayToolTip = DayToolTip;

        _loading = true;
        tbDateLimit.Text = draft.DateLimitText;
        dtpFrom.Value = draft.ValidFrom;
        dtpTo.Value = draft.ValidTo;
        llCalendar.Enabled = openCalendar != null;
        _loading = false;

        RefreshVariants();
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => IsMine(field);

    private static bool IsMine(TrainRules.Field field) => field is TrainRules.Field.Validity or TrainRules.Field.DateLimit;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        var mine = problems.Where(problem => IsMine(problem.Field)).ToList();
        _marks.Mark(mine.Where(problem => !problem.IsWarning && problem.Field == TrainRules.Field.DateLimit)
            .Select(_ => (Control)tbDateLimit));
        TrainPageHint.Show(lHint, mine, _hintColor);

        // dni mohla zmenit aj ina stranka (napr. pridelenie spolocnych dni) - pole ukazuje koncept
        if (tbDateLimit.Text != _draft.DateLimitText)
        {
            _loading = true;
            tbDateLimit.Text = _draft.DateLimitText;
            _loading = false;
        }

        RefreshVariants();
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem)
    {
        Control control = problem.Field == TrainRules.Field.Validity ? dtpFrom : tbDateLimit;
        control.Focus();
    }

    // ---------------------------------------------------------------- varianty

    /// <summary>
    /// Obnovi popis, pruh kalendara a tabulku variant - zavisia od cisla, nazvu a typu vlaku aj od jeho dni.
    /// </summary>
    private void RefreshVariants()
    {
        var selected = SelectedRow();
        _others = TrainVariants.Others(_draft, _context);
        _overlaps = new Dictionary<Train, string>(ReferenceEqualityComparer.Instance);
        foreach (var (other, days) in TrainVariants.Overlaps(_draft, _others))
            _overlaps[other] = days;

        var hasVariants = _others.Count != 0;
        var (position, count) = TrainVariants.PositionOf(_draft, _others);
        lVariantInfo.Text = hasVariants
            ? string.Format(CultureInfo.CurrentCulture, Resources.TrainValidityPage_Skupina, position, count, DraftLabel())
            : Resources.TrainValidityPage_ZiadneVarianty;
        strip.Visible = dgvVariants.Visible = flpOther.Visible = hasVariants;
        if (!hasVariants)
            return;

        strip.SetCalendar(VariantCalendar.Build(_draft, _others));
        FillGrid(selected);
    }

    private string DraftLabel() =>
        string.Join(" ", new[] { _draft.Type?.ToString(), _draft.Number, TrainName.ToDisplay(GlobData.TrainNames, _draft.Name) }
            .Where(part => !string.IsNullOrEmpty(part)));

    private void FillGrid(object? selected)
    {
        dgvVariants.Rows.Clear();
        var count = _others.Count + 1;
        var warning = TrainPageHint.WarningColor(dgvVariants);

        var rows = new List<(int Position, object Tag, string Route, DateTime From, DateTime To, string Limit, string Common)>
        {
            (TrainVariants.PositionOf(_draft, _others).Position, this, Route(_draft.RouteFrom.FirstOrDefault(),
                _draft.RouteTo.LastOrDefault()), _draft.ValidFrom, _draft.ValidTo, _draft.DateLimitText, "")
        };
        foreach (var other in _others)
        {
            var limit = _draft.LimitOf(other);
            if (_draft.VariantLimits.ContainsKey(other))
                limit = string.Format(CultureInfo.CurrentCulture, Resources.TrainValidityPage_ZmeniSaPoOK, limit);
            rows.Add((TrainVariants.PositionOf(other, _draft, _others), other, Route(other.StartingStation, other.EndingStation),
                other.ZaciatokPlatnosti, other.KoniecPlatnosti, limit, _overlaps.GetValueOrDefault(other, "")));
        }

        foreach (var (position, tag, route, from, to, limit, common) in rows.OrderBy(r => r.Position))
        {
            var index = dgvVariants.Rows.Add($"{position}/{count}",
                ReferenceEquals(tag, this) ? string.Format(CultureInfo.CurrentCulture, Resources.TrainValidityPage_TentoVlak, route) : route,
                $"{DateLimit.FormatDate(from)} – {DateLimit.FormatDate(to)}", limit, common);
            var row = dgvVariants.Rows[index];
            row.Tag = tag;
            if (ReferenceEquals(tag, this))
                row.DefaultCellStyle.Font = new Font(dgvVariants.Font, FontStyle.Bold);
            if (common.Length != 0)
                row.Cells[colCommon.Index].Style.ForeColor = warning;
            row.Selected = ReferenceEquals(tag, selected);
        }

        UpdateButtons();
    }

    private string Route(Station? start, Station? end) => $"{start?.Name ?? _homeStation} → {end?.Name ?? _homeStation}";

    private string DayToolTip(DateTime date, IReadOnlyList<VariantCalendar.Row> running)
    {
        var lines = new List<string> { date.ToString("dddd d. M. yyyy", CultureInfo.CurrentCulture) };
        foreach (var row in running)
        {
            var route = row.Train == null
                ? Route(_draft.RouteFrom.FirstOrDefault(), _draft.RouteTo.LastOrDefault())
                : Route(row.Train.StartingStation, row.Train.EndingStation);
            lines.Add($"{row.Position}/{_others.Count + 1}  {route}");
        }

        return string.Join(Environment.NewLine, lines);
    }

    // vybrany riadok tabulky: ina varianta (Train), tento vlak (this) alebo nic
    private object? SelectedRow() => dgvVariants.SelectedRows.Count == 0 ? null : dgvVariants.SelectedRows[0].Tag;

    private Train? SelectedOther() => SelectedRow() as Train;

    private void UpdateButtons()
    {
        // riadok vybranej varianty je v pruhu kalendara oramovany
        strip.SelectedPosition = SelectedRow() switch
        {
            Train train => TrainVariants.PositionOf(train, _draft, _others),
            null => 0,
            _ => TrainVariants.PositionOf(_draft, _others).Position
        };

        var other = SelectedOther();
        bEditOther.Enabled = other != null;
        bGiveThis.Enabled = bGiveOther.Enabled = other != null && _overlaps.ContainsKey(other);
    }

    private void OnChanged()
    {
        if (!_loading)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    private void tbDateLimit_TextChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.DateLimitText = tbDateLimit.Text;
        OnChanged();
    }

    private void Period_ValueChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.ValidFrom = dtpFrom.Value;
        _draft.ValidTo = dtpTo.Value;
        OnChanged();
    }

    private void bEditLimit_Click(object? sender, EventArgs e) => EditOwnLimit();

    private void EditOwnLimit()
    {
        if (FindForm() is not { } form || _draft.ValidTo.Date < _draft.ValidFrom.Date)
            return;

        if (FDateLimitEdit.SetDateLimit(form, _draft.ValidFrom, _draft.ValidTo, defaultValue: tbDateLimit.Text) is { } limit)
            tbDateLimit.Text = limit;
    }

    private void llCalendar_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e) => _openCalendar?.Invoke();

    private void dgvVariants_SelectionChanged(object? sender, EventArgs e) => UpdateButtons();

    private void dgvVariants_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (dgvVariants.Rows[e.RowIndex].Tag is Train)
            EditOther();
        else
            EditOwnLimit();
    }

    private void bEditOther_Click(object? sender, EventArgs e) => EditOther();

    /// <summary>
    /// Upravi dni vybranej varianty; pri prekryti navrhne dni bez spolocnych. Vlak sa zmeni az po ulozeni.
    /// </summary>
    private void EditOther()
    {
        if (SelectedOther() is not { } other || FindForm() is not { } form || _draft.ValidTo.Date < _draft.ValidFrom.Date)
            return;

        var proposal = _overlaps.ContainsKey(other) ? TrainVariants.WithoutCommonDays(_draft, other) : null;
        if (FDateLimitEdit.SetDateLimit(form, _draft.ValidFrom, _draft.ValidTo, other, defaultValue: _draft.LimitOf(other),
                proposal: proposal) is not { } limit)
            return;

        TrainVariants.SetLimit(_draft, other, limit);
        OnChanged();
    }

    private void bGiveThis_Click(object? sender, EventArgs e) => GiveCommonDays(true);

    private void bGiveOther_Click(object? sender, EventArgs e) => GiveCommonDays(false);

    /// <summary>
    /// Spolocne dni s vybranou variantou prideli tomuto vlaku alebo jej.
    /// </summary>
    private void GiveCommonDays(bool toThis)
    {
        if (SelectedOther() is not { } other)
            return;

        if (!TrainVariants.GiveCommonDays(_draft, other, toThis))
        {
            Utils.ShowError(Resources.TrainValidityPage_DniSaNedajuPrecitat);
            return;
        }

        if (!toThis)
        {
            _loading = true;
            tbDateLimit.Text = _draft.DateLimitText;
            _loading = false;
        }

        OnChanged();
    }
}
