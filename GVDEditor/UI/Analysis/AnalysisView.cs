using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Properties;
using Serilog;
using ToolsCore.Icons;
using ToolsCore.Tools;

namespace GVDEditor.UI.Analysis;

/// <summary>
/// Panel Analyza grafikonu v hlavnom okne (ako Zoznam chyb vo Visual Studiu): problemy otvoreneho grafikonu s filtrami
/// zavaznosti, opakovanie analyzy a oprava vybraneho problemu (aj dvojklikom alebo Enterom). Analyza bezi na pozadi
/// s priebehom v paneli nastrojov. Po zmene grafikonu mimo panela vysledok zastara (<see cref="MarkStale" />) a opravy
/// su nedostupne, kym analyza nebezi znova - problemy by mohli patrit uz neexistujucim udajom.
/// </summary>
internal partial class AnalysisView : UserControl
{
    private readonly Func<Task<List<IProblem>>?> _analyze;
    private readonly Action<IProblem> _fixed;
    private List<IProblem> _problems = [];
    private bool _analyzed;
    private bool _stale;
    private Task _analysis = Task.CompletedTask;

    /// <param name="analyze">
    /// spusti analyzu otvoreneho grafikonu na pozadi s priebehom v percentach; null = ziadny grafikon nie je otvoreny
    /// </param>
    /// <param name="fixed">oprava problemu prebehla (hlavne okno oznaci grafikon ako zmeneny)</param>
    public AnalysisView(Func<IProgress<int>, Task<List<IProblem>>?> analyze, Action<IProblem> @fixed)
    {
        _fixed = @fixed;
        InitializeComponent();
        dgvResults.AutoGenerateColumns = false;
        var progress = new Progress<int>(percent => tspbProgress.Value = Math.Clamp(percent, 0, 100));
        _analyze = () => analyze(progress);
        ShowProblems([]);
    }

    /// <summary>
    /// Tabulka problemov (sirky a poradie stlpcov si pamata hlavne okno).
    /// </summary>
    public DataGridView Grid => dgvResults;

    /// <summary>
    /// Prebiehajuca alebo posledna analyza (harness snimok na nu caka).
    /// </summary>
    public Task Analysis => _analysis;

    /// <summary>
    /// Pocet problemov podla zavaznosti (vratane skrytych filtrom).
    /// </summary>
    public int Count(ProblemType type) => _problems.Count(problem => problem.ProblemType == type);

    /// <summary>
    /// Spusti analyzu, ak uz nebezi.
    /// </summary>
    public Task AnalyzeAsync()
    {
        if (_analysis.IsCompleted)
            _analysis = RunAsync();
        return _analysis;
    }

    /// <summary>
    /// Grafikon sa zmenil mimo panela - vysledok uz nemusi platit.
    /// </summary>
    public void MarkStale()
    {
        if (!_analyzed || _stale)
            return;
        _stale = true;
        UpdateState();
    }

    /// <summary>
    /// Iny alebo ziadny grafikon - problemy predchadzajuceho sa zahodia.
    /// </summary>
    public void Clear()
    {
        _analyzed = false;
        _stale = false;
        ShowProblems([]);
    }

    private async Task RunAsync()
    {
        if (_analyze() is not { } analysis)
        {
            Clear();
            return;
        }

        tsbAnalyze.Enabled = false;
        tsbFix.Enabled = false;
        tspbProgress.Value = 0;
        tspbProgress.Visible = true;
        tslStatus.Text = "";
        UseWaitCursor = true;
        try
        {
            var problems = await analysis;
            _analyzed = true;
            _stale = false;
            ShowProblems(problems);
        }
        catch (Exception e)
        {
            Log.Error(e, "Analyza grafikonu zlyhala");
            Utils.ShowError(e.Message);
        }
        finally
        {
            UseWaitCursor = false;
            tspbProgress.Visible = false;
            tsbAnalyze.Enabled = true;
            UpdateState();
        }
    }

    private void ShowProblems(List<IProblem> problems)
    {
        _problems = problems;
        tsbErrors.Text = string.Format(CultureInfo.CurrentCulture, Resources.Analysis_Errors, Count(ProblemType.Error));
        tsbWarnings.Text = string.Format(CultureInfo.CurrentCulture, Resources.Analysis_Warnings, Count(ProblemType.Warning));
        tsbInfos.Text = string.Format(CultureInfo.CurrentCulture, Resources.Analysis_Infos, Count(ProblemType.Hint));
        Fill();
        UpdateState();
    }

    // riadky podla filtrov, chyby na zaciatku; vybrany problem ostane vybrany, ak este je v zozname
    private void Fill()
    {
        var selected = Selected;
        var rows = _problems.Where(PassesFilter).OrderBy(Severity).ToList();
        dgvResults.DataSource = rows;

        var index = selected is null ? -1 : rows.IndexOf(selected);
        if (index >= 0)
            dgvResults.CurrentCell = dgvResults.Rows[index].Cells[cProblem.Index];
        else
            dgvResults.ClearSelection();
        UpdateState();
    }

    private static int Severity(IProblem problem) => problem.ProblemType switch
    {
        ProblemType.Error => 0,
        ProblemType.Warning => 1,
        _ => 2
    };

    private bool PassesFilter(IProblem problem) => problem.ProblemType switch
    {
        ProblemType.Error => tsbErrors.Checked,
        ProblemType.Warning => tsbWarnings.Checked,
        _ => tsbInfos.Checked
    };

    private IProblem? Selected => dgvResults.SelectedRows.Count > 0 ? dgvResults.SelectedRows[0].DataBoundItem as IProblem : null;

    private void UpdateState()
    {
        tsbFix.Enabled = _analysis.IsCompleted && !_stale && Selected is not null;
        if (!_analysis.IsCompleted)
            return;
        tslStatus.Text = _stale ? Resources.Analysis_Stale : "";
    }

    private void Repair(IProblem? problem)
    {
        if (problem is null || _stale || !_analysis.IsCompleted)
            return;

        FixResult result;
        Exception? error = null;
        try
        {
            result = problem.FixProblem();
        }
        catch (Exception e)
        {
            Log.Error(e, "Oprava problemu analyzy {Problem} zlyhala", problem.Text);
            error = e;
            result = FixResult.Error;
        }

        switch (result)
        {
            case FixResult.Error:
                Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_FixError, error!.Message));
                break;
            case FixResult.NotSolved:
                Utils.ShowWarning(Resources.Analyzer_NotFixed);
                break;
            case FixResult.Done:
                Utils.ShowInfo(Resources.Analyzer_Fixed);
                ShowProblems([.. _problems.Where(other => other != problem)]);
                _fixed(problem);
                break;
        }
    }

    private void dgvResults_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvResults.Rows[e.RowIndex].DataBoundItem is not IProblem problem)
            return;

        var cell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (e.ColumnIndex == cSeverity.Index)
        {
            var (icon, tip) = problem.ProblemType switch
            {
                ProblemType.Error => ("status-error", Resources.Analyzer_Error),
                ProblemType.Warning => ("status-warning", Resources.Analyzer_Warning),
                _ => ("status-info", Resources.Analyzer_Info)
            };
            e.Value = IconImages.For(dgvResults, icon);
            cell.ToolTipText = tip;
        }
        else if (e.ColumnIndex == cFix.Index)
        {
            (e.Value, cell.ToolTipText) = problem.FixType switch
            {
                FixType.Auto => (Resources.Analyzer_FixAuto, Resources.Analyzer_FixAuto_Tip),
                FixType.SemiAuto => (Resources.Analyzer_FixSemi, Resources.Analyzer_FixSemi_Tip),
                _ => (Resources.Analyzer_FixManual, Resources.Analyzer_FixManual_Tip)
            };
        }
    }

    private void dgvResults_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            Repair(dgvResults.Rows[e.RowIndex].DataBoundItem as IProblem);
    }

    private void dgvResults_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        e.Handled = true;
        Repair(Selected);
    }

    private void dgvResults_SelectionChanged(object? sender, EventArgs e) => UpdateState();

    private void Filter_CheckedChanged(object? sender, EventArgs e) => Fill();

    private async void tsbAnalyze_Click(object? sender, EventArgs e) => await AnalyzeAsync();

    private void tsbFix_Click(object? sender, EventArgs e) => Repair(Selected);
}
