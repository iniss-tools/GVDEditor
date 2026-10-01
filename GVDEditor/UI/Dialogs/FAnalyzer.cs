using System.Globalization;
using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;

namespace GVDEditor.UI.Dialogs;

/// <summary>
/// Dialog - Analyza grafikonu.
/// </summary>
internal partial class FAnalyzer : Form
{
    /// <summary>
    /// Kontext editora - nastavenia programu, instalacia INISS a otvoreny grafikon.
    /// </summary>
    private readonly EditorContext _ctx;
    private readonly IAnalyzerHost _host;

    // moderne ikony systemu (rovnake ako zoznam chyb v RawBankEditore), nie stare SystemIcons
    private static readonly Bitmap InfoIcon = StockIcon(ShellIconType.Info);
    private static readonly Bitmap WarningIcon = StockIcon(ShellIconType.Warning);
    private static readonly Bitmap ErrorIcon = StockIcon(ShellIconType.Error);

    private readonly GVDDirectory _gvd;
    private BindingList<IProblem> _problems = [];

    // prebiehajuca analyza (harness snimok na nu caka)
    private Task _analysis = Task.CompletedTask;

    /// <summary>
    /// Ci niektora oprava zmenila grafikon v pamati - hlavne okno ho potom oznaci ako neulozeny.
    /// </summary>
    public bool DataChanged { get; private set; }

    /// <summary>
    /// Vytvori novy formular typu <see cref="FAnalyzer"/>.
    /// </summary>
    /// <param name="context">Kontext editora.</param>
    /// <param name="gvd">Aktualne vybrany grafikon na analyzovanie.</param>
    /// <param name="host">Hlavne okno - opravy, ktore otvaraju nastavenia alebo editor TabTab.</param>
    public FAnalyzer(EditorContext context, GVDDirectory gvd, IAnalyzerHost host)
    {
        _ctx = context;
        _host = host;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        _gvd = gvd;
    }

    private static Bitmap StockIcon(ShellIconType type)
    {
        using var icon = new ShellIcon(type, ShellIconSize.Small);
        return icon.ToBitmap();
    }

    private void bOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    private async void bAnalyze_Click(object sender, EventArgs e)
    {
        if (!_analysis.IsCompleted)
            return;

        _analysis = AnalyzeAsync();
        await _analysis;
    }

    /// <summary>
    /// Analyza bezi na pozadi, priebeh sa ukazuje v stavovom riadku okna.
    /// </summary>
    private async Task AnalyzeAsync()
    {
        bAnalyze.Enabled = false;
        var progress = new Progress<int>(percent =>
        {
            pbStatus.Value = percent;
            lStatus.Text = @$"{percent}%";
        });

        try
        {
            var problems = await Task.Run(() => Analyzer.FindProblems(_gvd, new AnalysisScope(_ctx.Document, _ctx.Workspace, _host), progress));
            _problems = new BindingList<IProblem>(problems);
            dgvResults.DataSource = null;
            dgvResults.DataSource = _problems;
        }
        catch (Exception exception)
        {
            Log.Exception(exception);
            Utils.ShowError(exception.Message);
        }
        finally
        {
            bAnalyze.Enabled = true;
        }
    }

    private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var cell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (e.ColumnIndex == 0)
            switch (_problems[e.RowIndex].ProblemType)
            {
                case Domain.Analysis.ProblemType.Hint:
                    e.Value = InfoIcon;
                    cell.ToolTipText = Resources.Analyzer_Info;
                    break;
                case Domain.Analysis.ProblemType.Warning:
                    e.Value = WarningIcon;
                    cell.ToolTipText = Resources.Analyzer_Warning;
                    break;
                case Domain.Analysis.ProblemType.Error:
                    e.Value = ErrorIcon;
                    cell.ToolTipText = Resources.Analyzer_Error;
                    break;
            }
        else if (e.ColumnIndex == 2)
            switch (_problems[e.RowIndex].FixType)
            {
                case Domain.Analysis.FixType.Auto:
                    e.Value = Resources.Analyzer_FixAuto;
                    cell.ToolTipText = Resources.Analyzer_FixAuto_Tip;
                    break;
                case Domain.Analysis.FixType.SemiAuto:
                    e.Value = Resources.Analyzer_FixSemi;
                    cell.ToolTipText = Resources.Analyzer_FixSemi_Tip;
                    break;
                case Domain.Analysis.FixType.Manual:
                    e.Value = Resources.Analyzer_FixManual;
                    cell.ToolTipText = Resources.Analyzer_FixManual_Tip;
                    break;
            }
    }

    private void dgvResults_DoubleClick(object sender, EventArgs e)
    {
        if (dgvResults.SelectedRows.Count != 0) Repair((IProblem)dgvResults.SelectedRows[0].DataBoundItem!);
    }

    private void bFixSelected_Click(object sender, EventArgs e)
    {
        if (dgvResults.SelectedRows.Count != 0) Repair((IProblem)dgvResults.SelectedRows[0].DataBoundItem!);
    }

    private void Repair(IProblem problem)
    {
        FixResult res;
        Exception? error = null;
        try
        {
            res = problem.FixProblem();
        }
        catch (Exception e)
        {
            error = e;
            res = FixResult.Error;
        }

        switch (res)
        {
            case FixResult.Error:
                Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_FixError, error!.Message));
                break;
            case FixResult.NotSolved:
                Utils.ShowWarning(Resources.Analyzer_NotFixed);
                break;
            case FixResult.Done:
                Utils.ShowInfo(Resources.Analyzer_Fixed);
                _problems.Remove(problem);
                if (problem.ChangesGrafikon) DataChanged = true;
                break;
        }
    }
}