using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Analyza grafikonu.
/// </summary>
public partial class FAnalyzer : Form
{
    private readonly GVDDirectory GVD;
    private BindingList<IProblem> Problems;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FAnalyzer"/>.
    /// </summary>
    /// <param name="gvd">Aktualne vybrany grafikon na analyzovanie.</param>
    public FAnalyzer(GVDDirectory gvd)
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();
        GVD = gvd;
    }

    private void bOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    private void bAnalyze_Click(object sender, EventArgs e) => bgWorkAnalyze.RunWorkerAsync();

    private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var cell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (e.ColumnIndex == 0)
            switch (Problems[e.RowIndex].ProblemType)
            {
                case Tools.ProblemType.Hint:
                    e.Value = SystemIcons.Information;
                    cell.ToolTipText = "Informácia";
                    break;
                case Tools.ProblemType.Warning:
                    e.Value = SystemIcons.Warning;
                    cell.ToolTipText = "Upozornenie";
                    break;
                case Tools.ProblemType.Error:
                    e.Value = SystemIcons.Error;
                    cell.ToolTipText = "Chyba";
                    break;
            }
        else if (e.ColumnIndex == 2)
            switch (Problems[e.RowIndex].FixType)
            {
                case Tools.FixType.Auto:
                    e.Value = "Automaticky";
                    cell.ToolTipText = "Program opraví problém sám";
                    break;
                case Tools.FixType.SemiAuto:
                    e.Value = "Polo-automaticky";
                    cell.ToolTipText = "Používateľ vyberie jednu možnosť opravy";
                    break;
                case Tools.FixType.Manual:
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
        if (dgvResults.SelectedRows.Count != 0) Repair(dgvResults.SelectedRows[0].DataBoundItem as IProblem);
    }

    private void bFixSelected_Click(object sender, EventArgs e)
    {
        if (dgvResults.SelectedRows.Count != 0) Repair(dgvResults.SelectedRows[0].DataBoundItem as IProblem);
    }

    private void Repair(IProblem problem)
    {
        FixResult res;
        Exception error = null;
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
                break;
        }
    }
}