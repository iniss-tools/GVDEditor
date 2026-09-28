using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using ToolsCore.Tools;

namespace GVDEditor.UI.Dialogs;

/// <summary>
/// Dialog - Analyza grafikonu.
/// </summary>
public partial class FAnalyzer : Form
{
    // moderne ikony systemu (rovnake ako zoznam chyb v RawBankEditore), nie stare SystemIcons
    private static readonly Bitmap InfoIcon = StockIcon(ShellIconType.Info);
    private static readonly Bitmap WarningIcon = StockIcon(ShellIconType.Warning);
    private static readonly Bitmap ErrorIcon = StockIcon(ShellIconType.Error);

    private readonly GVDDirectory GVD;
    private BindingList<IProblem> Problems = new();

    /// <summary>
    /// Ci niektora oprava zmenila grafikon v pamati - hlavne okno ho potom oznaci ako neulozeny.
    /// </summary>
    public bool DataChanged { get; private set; }

    /// <summary>
    /// Vytvori novy formular typu <see cref="FAnalyzer"/>.
    /// </summary>
    /// <param name="gvd">Aktualne vybrany grafikon na analyzovanie.</param>
    public FAnalyzer(GVDDirectory gvd)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();
        GVD = gvd;
    }

    private static Bitmap StockIcon(ShellIconType type)
    {
        using var icon = new ShellIcon(type, ShellIconSize.Small);
        return icon.ToBitmap();
    }

    private void bOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    private void bAnalyze_Click(object sender, EventArgs e) => bgWorkAnalyze.RunWorkerAsync();

    private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var cell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (e.ColumnIndex == 0)
            switch (Problems[e.RowIndex].ProblemType)
            {
                case Domain.Analysis.ProblemType.Hint:
                    e.Value = InfoIcon;
                    cell.ToolTipText = "Informácia";
                    break;
                case Domain.Analysis.ProblemType.Warning:
                    e.Value = WarningIcon;
                    cell.ToolTipText = "Upozornenie";
                    break;
                case Domain.Analysis.ProblemType.Error:
                    e.Value = ErrorIcon;
                    cell.ToolTipText = "Chyba";
                    break;
            }
        else if (e.ColumnIndex == 2)
            switch (Problems[e.RowIndex].FixType)
            {
                case Domain.Analysis.FixType.Auto:
                    e.Value = "Automaticky";
                    cell.ToolTipText = "Program opraví problém sám";
                    break;
                case Domain.Analysis.FixType.SemiAuto:
                    e.Value = "Polo-automaticky";
                    cell.ToolTipText = "Používateľ vyberie jednu možnosť opravy";
                    break;
                case Domain.Analysis.FixType.Manual:
                    e.Value = "Manuálne";
                    cell.ToolTipText = "Používateľ musí problém opraviť sám, program len navedie k riešeniu";
                    break;
            }
    }

    private void bgWorkAnalyze_DoWork(object sender, DoWorkEventArgs e)
    {
        Problems = new BindingList<IProblem>(Analyzer.FindProblems(bgWorkAnalyze, GVD));
    }

    private void bgWorkAnalyze_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        pbStatus.Value = e.ProgressPercentage;
        lStatus.Text = @$"{e.ProgressPercentage}%";
    }

    private void bgWorkAnalyze_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        dgvResults.DataSource = null;
        dgvResults.DataSource = Problems;
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
                Utils.ShowError(@$"Počas operácie sa vyskytla chyba: {error!.Message}");
                break;
            case FixResult.NotSolved:
                Utils.ShowWarning(@"Používateľ chybu neopravil.");
                break;
            case FixResult.Done:
                Utils.ShowInfo(@"Problém bol opravený.");
                Problems.Remove(problem);
                if (problem.ChangesGrafikon) DataChanged = true;
                break;
        }
    }
}