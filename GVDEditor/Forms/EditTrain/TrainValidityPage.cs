using System.Globalization;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms.Settings;
using GVDEditor.Properties;
using GVDEditor.Tools;

namespace GVDEditor.Forms.EditTrain;

/// <summary>
///     Stranka Platnost v okne vlaku - datumove obmedzenie, obdobie platnosti a varianta vlaku s prehladom ostatnych
///     variant. Prekrytie s inou variantou je len upozornenie; obmedzenie inej varianty sa da upravit tu a zapise sa po OK.
/// </summary>
public partial class TrainValidityPage : UserControl, ITrainPage
{
    private readonly FieldMarks _marks = new();
    private TrainDraft _draft = null!;
    private TrainContext _context = null!;
    private Action? _openCalendar;
    private List<Train> _others = [];
    private Dictionary<Train, string> _overlaps = new(ReferenceEqualityComparer.Instance);
    private Color _hintColor;
    private bool _loading;

    /// <summary>
    ///     Vytvori stranku; udaje nacita az <see cref="LoadData" />.
    /// </summary>
    public TrainValidityPage()
    {
        InitializeComponent();
        dgvVariants.AutoGenerateColumns = false;
    }

    /// <inheritdoc />
    public event EventHandler? Changed;

    /// <summary>
    ///     Naplni stranku udajmi konceptu - volat az po nastaveni temy okna.
    /// </summary>
    /// <param name="draft">koncept vlaku</param>
    /// <param name="context">grafikon a nastavenia variant</param>
    /// <param name="openCalendar">otvori Kalendar akcii vlaku; <see langword="null" />, ak sa neda zobrazit</param>
    internal void LoadData(TrainDraft draft, TrainContext context, Action? openCalendar)
    {
        _draft = draft;
        _context = context;
        _openCalendar = openCalendar;
        foreach (var header in new[] { lLimitHeader, lVariantHeader })
            header.Font = new Font(Font, FontStyle.Bold);
        _hintColor = lHint.ForeColor;
        lVariantNote.ForeColor = SystemColors.GrayText;
        if (GlobData.UsingStyle.DarkScrollBar)
            pScroll.SetTheme(WindowsTheme.DarkExplorer);
        _marks.Capture(tbDateLimit, nudVariant);

        _loading = true;
        tbDateLimit.Text = draft.DateLimitText;
        dtpFrom.Value = draft.ValidFrom;
        dtpTo.Value = draft.ValidTo;
        nudVariant.Value = Math.Clamp(draft.Variant, (int)nudVariant.Minimum, (int)nudVariant.Maximum);
        nudVariant.Enabled = !context.AutoVariant;
        lVariantNote.Text = context.AutoVariant ? Resources.TrainValidityPage_Automaticky : Resources.TrainValidityPage_BezVariantov;
        llCalendar.Enabled = openCalendar != null;
        _loading = false;

        RefreshVariants();
    }

    /// <inheritdoc />
    bool ITrainPage.Handles(TrainRules.Field field) => IsMine(field);

    private static bool IsMine(TrainRules.Field field) =>
        field is TrainRules.Field.Validity or TrainRules.Field.DateLimit or TrainRules.Field.Variant;

    /// <inheritdoc />
    void ITrainPage.ShowProblems(IReadOnlyList<TrainRules.Problem> problems)
    {
        var mine = problems.Where(problem => IsMine(problem.Field)).ToList();
        _marks.Mark(mine.Where(problem => !problem.IsWarning).Select(problem => problem.Field switch
        {
            TrainRules.Field.Variant => (Control)nudVariant,
            _ => tbDateLimit
        }));
        TrainPageHint.Show(lHint, mine, _hintColor);
        RefreshVariants();
    }

    /// <inheritdoc />
    void ITrainPage.FocusField(TrainRules.Problem problem)
    {
        Control control = problem.Field switch
        {
            TrainRules.Field.Validity => dtpFrom,
            TrainRules.Field.Variant => nudVariant,
            _ => tbDateLimit
        };
        control.Focus();
    }

    /// <summary>
    ///     Obnovi tabulku ostatnych variant - zavisi od cisla, nazvu a typu vlaku aj od jeho obmedzenia.
    /// </summary>
    private void RefreshVariants()
    {
        var selected = SelectedOther();
        _others = TrainVariants.Others(_draft, _context);
        _overlaps = new Dictionary<Train, string>(ReferenceEqualityComparer.Instance);
        foreach (var (other, days) in TrainVariants.Overlaps(_draft, _others))
            _overlaps[other] = days;

        lOthers.Text = _others.Count == 0 ? Resources.TrainValidityPage_ZiadneVarianty : Resources.TrainValidityPage_DalsieVarianty;
        dgvVariants.Visible = bEditOther.Visible = _others.Count != 0;

        dgvVariants.Rows.Clear();
        var warning = TrainPageHint.WarningColor(dgvVariants);
        foreach (var other in _others)
        {
            var limit = _draft.LimitOf(other);
            if (_draft.VariantLimits.ContainsKey(other))
                limit = string.Format(CultureInfo.CurrentCulture, Resources.TrainValidityPage_ZmeniSaPoOK, limit);

            var index = dgvVariants.Rows.Add(other.Variant.ToString(CultureInfo.InvariantCulture),
                $"{DateLimit.FormatDate(other.ZaciatokPlatnosti)} – {DateLimit.FormatDate(other.KoniecPlatnosti)}", limit,
                _overlaps.GetValueOrDefault(other, ""));
            var row = dgvVariants.Rows[index];
            row.Tag = other;
            if (_overlaps.ContainsKey(other))
                row.Cells[colCommon.Index].Style.ForeColor = warning;
            if (ReferenceEquals(other, selected))
                row.Selected = true;
        }

        UpdateButtons();
    }

    private Train? SelectedOther() => dgvVariants.SelectedRows.Count == 0 ? null : dgvVariants.SelectedRows[0].Tag as Train;

    private void UpdateButtons() => bEditOther.Enabled = SelectedOther() != null;

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

    private void nudVariant_ValueChanged(object? sender, EventArgs e)
    {
        if (_loading)
            return;

        _draft.Variant = decimal.ToInt32(nudVariant.Value);
        OnChanged();
    }

    private void bEditLimit_Click(object? sender, EventArgs e)
    {
        if (FindForm() is not { } form || _draft.ValidTo.Date < _draft.ValidFrom.Date)
            return;

        if (FDateLimitEdit.SetDateLimit(form, _draft.ValidFrom, _draft.ValidTo, defaultValue: tbDateLimit.Text) == DialogResult.OK)
            tbDateLimit.Text = FDateLimitEdit.Result;
    }

    private void llCalendar_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e) => _openCalendar?.Invoke();

    private void dgvVariants_SelectionChanged(object? sender, EventArgs e) => UpdateButtons();

    private void dgvVariants_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            EditOther();
    }

    private void bEditOther_Click(object? sender, EventArgs e) => EditOther();

    /// <summary>
    ///     Upravi obmedzenie vybranej varianty; pri prekryti navrhne obmedzenie bez spolocnych dni. Vlak sa zmeni az po OK.
    /// </summary>
    private void EditOther()
    {
        if (SelectedOther() is not { } other || FindForm() is not { } form || _draft.ValidTo.Date < _draft.ValidFrom.Date)
            return;

        var proposal = _overlaps.ContainsKey(other) ? TrainVariants.WithoutCommonDays(_draft, other) : null;
        if (FDateLimitEdit.SetDateLimit(form, _draft.ValidFrom, _draft.ValidTo, other, defaultValue: _draft.LimitOf(other),
                proposal: proposal) != DialogResult.OK)
            return;

        if (FDateLimitEdit.Result == (other.DateLimitText ?? ""))
            _draft.VariantLimits.Remove(other);
        else
            _draft.VariantLimits[other] = FDateLimitEdit.Result;
        OnChanged();
    }
}
