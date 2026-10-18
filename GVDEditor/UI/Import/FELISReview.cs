using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using GVDEditor.Properties;
using Iniss.Elis;
using ToolsCore.Tools;

namespace GVDEditor.UI.Import;

/// <summary>
/// Dialog - kontrola importu z programu ELIS pred jeho dokoncenim: ktore stanice sa hlasia v kratkom hlaseni
/// a co sa urobi s vlakmi s poznamkami z ELIS. Volby sa ulozia do grafikonu a dalsi import ich navrhne.
/// </summary>
internal partial class FelisReview : Form
{
    private const string AdditionGroup = "DODATKY";

    private readonly ElisReview _review;

    // polozky ponuk v bunkach su texty - k nim hodnota, ktoru znamenaju
    private readonly Dictionary<string, ElisTrainFlag> _flags = [];
    private readonly Dictionary<string, string> _additions = [];
    private readonly Dictionary<string, int> _lockouts = [];

    /// <summary>
    /// Vytvori novy formular typu <see cref="FelisReview" />.
    /// </summary>
    /// <param name="context">Kontext editora - zvukova banka (dodatky) a vyluky instalacie.</param>
    /// <param name="review">Volby na kontrolu; po potvrdeni obsahuju volby obsluhy.</param>
    public FelisReview(EditorContext context, ElisReview review)
    {
        InitializeComponent();
        dgvStations.AutoGenerateColumns = false;
        dgvRemarks.AutoGenerateColumns = false;
        dgvRadenie.AutoGenerateColumns = false;
        this.ApplyThemeAndFonts();

        _review = review;
        FillStations();
        FillRemarks(context);
        FillRadenie();
    }

    /// <summary>
    /// Navrhy radenia z planu radenia: kazdy variant (dni s rovnakymi voznami) v samostatnom riadku.
    /// </summary>
    private void FillRadenie()
    {
        if (_review.Radenia.Count == 0 && _review.PlanError is null)
        {
            tcReview.TabPages.Remove(tpRadenie);
            return;
        }

        if (_review.PlanError is { } error)
            lRadenieInfo.Text = string.Format(CultureInfo.CurrentCulture, Resources.FELISReview_RadeniePlanError, error);

        foreach (var proposal in _review.Radenia)
        {
            var days = proposal.Radenie.DatObm.Length != 0 ? proposal.Radenie.DatObm
                : proposal.Radenie.Validity is { } validity ? $"{validity.From.ToString("d", CultureInfo.CurrentCulture)} - {validity.To.ToString("d", CultureInfo.CurrentCulture)}"
                : Resources.FELISReview_RadenieAllDays;
            var index = dgvRadenie.Rows.Add(proposal.Take, $"{proposal.Plan.Type} {proposal.TrainNumber}", days, proposal.Radenie.Text,
                string.Join("; ", proposal.Issues.Distinct().Select(IssueText)));
            dgvRadenie.Rows[index].Tag = proposal;
        }
    }

    private static string IssueText(PlanRadeniaIssue issue)
    {
        var data = issue.Text.Split(' ', 2);
        return issue.Kind switch
        {
            PlanRadeniaIssueKind.MissingSound => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueMissingSound, issue.Text),
            PlanRadeniaIssueKind.UnknownKind => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueUnknownKind, issue.Text),
            PlanRadeniaIssueKind.BistroAsRestaurant => Resources.PlanRadenia_IssueBistro,
            PlanRadeniaIssueKind.MixedAsSecond => Resources.PlanRadenia_IssueMixed,
            PlanRadeniaIssueKind.UnknownStation => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueUnknownStation, issue.Text),
            PlanRadeniaIssueKind.BadNote => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueBadNote, issue.Text),
            PlanRadeniaIssueKind.UncertainNote => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueUncertainNote, issue.Text),
            PlanRadeniaIssueKind.JoinedCars => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueJoined, issue.Text),
            PlanRadeniaIssueKind.NoLocomotive => Resources.PlanRadenia_IssueNoLocomotive,
            PlanRadeniaIssueKind.CarFixed => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueCarFixed, data[0], data.Length > 1 ? data[1] : ""),
            PlanRadeniaIssueKind.CarMissing => string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_IssueCarMissing, issue.Text),
            _ => issue.Text
        };
    }

    private void FillStations()
    {
        if (_review.Stations.Count == 0)
        {
            tcReview.TabPages.Remove(tpShort);
            return;
        }

        cbKeepTrainShort.Visible = _review.Same.Count != 0;
        cbKeepTrainShort.Checked = _review.KeepTrainShort;
        cbKeepTrainShort.Text = string.Format(CultureInfo.CurrentCulture, Resources.FELISReview_KeepTrainShort, _review.Same.Count);

        foreach (var choice in _review.Stations)
        {
            var index = dgvStations.Rows.Add(choice.Station.Name, choice.Trains, choice.Short, Hint(choice));
            dgvStations.Rows[index].Tag = choice;
        }
    }

    /// <summary>
    /// Odkial je navrh pre stanicu, alebo aspon druh stanice v ELIS.
    /// </summary>
    private static string Hint(ElisStationChoice choice) => choice.Source switch
    {
        ElisShortSource.Saved => Resources.FELISReview_HintSaved,
        ElisShortSource.Grafikon => string.Format(CultureInfo.CurrentCulture, Resources.FELISReview_HintGrafikon, choice.Vote.Short, choice.Vote.Trains),
        _ when ElisImportService.IsMajor(choice.ElisType) => Resources.FELISReview_HintJunction,
        _ when (choice.ElisType & ElisStop.TypeRegionalSeat) != 0 => Resources.FELISReview_HintRegional,
        _ when (choice.ElisType & ElisStop.TypeDistrictSeat) != 0 => Resources.FELISReview_HintDistrict,
        _ => ""
    };

    private void FillRemarks(EditorContext context)
    {
        if (_review.Remarks.Count == 0)
        {
            tcReview.TabPages.Remove(tpRemarks);
            return;
        }

        _flags[Resources.FELISReview_FlagNone] = ElisTrainFlag.None;
        _flags[Resources.FELISReview_FlagReservation] = ElisTrainFlag.Reservation;
        _flags[Resources.FELISReview_FlagLowFloor] = ElisTrainFlag.LowFloor;
        _flags[Resources.FELISReview_FlagSleeperOnly] = ElisTrainFlag.SleeperOnly;

        _additions[Resources.FELISReview_AdditionNone] = "";
        foreach (var sound in context.Workspace.Sounds.Where(sound => sound.Group.Key.Equals(AdditionGroup, StringComparison.OrdinalIgnoreCase)))
        {
            var code = Dodatok.CodeFromKey(sound.Key);
            _additions.TryAdd(string.IsNullOrEmpty(sound.Text) ? code : $"{code} – {sound.Text}", code);
        }

        _lockouts[Resources.FEditTrain_Vyluka_None] = 0;
        _lockouts[$"1 – {Resources.FEditTrain_Vyluka_BuiltIn}"] = 1;
        foreach (var text in context.Workspace.LogZvukTexts.Where(text => text.IsLockout && text.Code > 1))
            if (!_lockouts.ContainsValue(text.Code))
                _lockouts[text.ToString()] = text.Code;

        // volba z minuleho importu moze odkazovat na dodatok alebo vyluku, ktora v instalacii uz nie je
        foreach (var action in _review.Remarks.Select(choice => choice.Action))
        {
            if (action.Addition.Length != 0 && !_additions.ContainsValue(action.Addition))
                _additions[action.Addition] = action.Addition;
            if (action.Lockout != 0 && !_lockouts.ContainsValue(action.Lockout))
                _lockouts[$"{action.Lockout} – {Resources.FEditTrain_Vyluka_Unknown}"] = action.Lockout;
        }

        colRemFlag.Items.AddRange(_flags.Keys.Cast<object>().ToArray());
        colRemAddition.Items.AddRange(_additions.Keys.Cast<object>().ToArray());
        colRemLockout.Items.AddRange(_lockouts.Keys.Cast<object>().ToArray());

        foreach (var choice in _review.Remarks)
        {
            var index = dgvRemarks.Rows.Add(choice.Text, choice.Trains,
                Display(_flags, choice.Action.Flag), Display(_additions, choice.Action.Addition), Display(_lockouts, choice.Action.Lockout));
            dgvRemarks.Rows[index].Tag = choice;
        }
    }

    private static string Display<T>(Dictionary<string, T> items, T value) =>
        items.First(item => EqualityComparer<T>.Default.Equals(item.Value, value)).Key;

    private static T Value<T>(Dictionary<string, T> items, object? display, T fallback) =>
        display is string text && items.TryGetValue(text, out var value) ? value : fallback;

    private void bOK_Click(object sender, EventArgs e)
    {
        dgvStations.EndEdit();
        dgvRemarks.EndEdit();
        foreach (DataGridViewRow row in dgvStations.Rows)
            if (row.Tag is ElisStationChoice choice)
                choice.Short = row.Cells[colStShort.Index].Value is true;

        foreach (DataGridViewRow row in dgvRemarks.Rows)
            if (row.Tag is ElisRemarkChoice choice)
                choice.Action = new ElisRemarkAction(
                    Value(_flags, row.Cells[colRemFlag.Index].Value, ElisTrainFlag.None),
                    Value(_additions, row.Cells[colRemAddition.Index].Value, ""),
                    Value(_lockouts, row.Cells[colRemLockout.Index].Value, 0));

        dgvRadenie.EndEdit();
        foreach (DataGridViewRow row in dgvRadenie.Rows)
            if (row.Tag is RadenieProposal proposal)
                proposal.Take = row.Cells[colRadTake.Index].Value is true;

        // Visible je false aj vtedy, ked je zobrazena druha karta - rozhoduje, ci su vlaky na prevzatie
        _review.KeepTrainShort = _review.Same.Count != 0 && cbKeepTrainShort.Checked;
        DialogResult = DialogResult.OK;
    }
}
