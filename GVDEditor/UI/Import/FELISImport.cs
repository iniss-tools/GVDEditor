using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;
using GVDEditor.Integration;

namespace GVDEditor.UI.Import;

/// <summary>
/// Dialog - Volby importu vlakov priamo z dat programu ELIS.
/// </summary>
public partial class FelisImport : Form
{
    internal ElisImportOptions ResultOptions = null!;

    private readonly int _existingTrainCount;

    // zdroj kratkeho hlasenia pred prepnutim na uplne nahradenie - vrati sa, ak obsluha zvoli iny rezim
    private object? _learnBeforeReplace;

    /// <summary>
    /// Vytvori novy formular typu <see cref="FelisImport" />.
    /// </summary>
    /// <param name="current">Grafikon, do ktoreho sa importuje.</param>
    /// <param name="existingTrainCount">Pocet vlakov, ktore uz grafikon obsahuje.</param>
    /// <param name="periods">Vsetky obdobia stanice vratane <paramref name="current" /> - z nich sa moze prevziat kratke hlasenie.</param>
    internal FelisImport(GVDDirectory current, int existingTrainCount, IEnumerable<GVDDirectory> periods)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _existingTrainCount = existingTrainCount;

        lStation.Text = string.Format(CultureInfo.CurrentCulture, Resources.FELISImport_Vlaky_sa_načítajú_pre_stanicu, current.GVD.ThisStation.Name);
        tbAppPath.Text = ElisImportOptions.DefaultElisDirectory;

        //ak grafikon este ziadne vlaky nema, nie je co aktualizovat ani nahradzat
        rbUpdate.Enabled = rbReplace.Enabled = existingTrainCount != 0;

        FillLearn(current, existingTrainCount, periods);
    }

    /// <summary>
    /// Ponuka grafikonov, z ktorych sa prevezme kratke hlasenie: otvoreny (ak ma vlaky) a ostatne obdobia stanice.
    /// Predvoleny je otvoreny grafikon, inak najnovsie obdobie s vlakmi.
    /// </summary>
    private void FillLearn(GVDDirectory current, int existingTrainCount, IEnumerable<GVDDirectory> periods)
    {
        cbLearn.Items.Add(new LearnItem(null, Resources.FELISImport_LearnNone));

        LearnItem? preferred = null;
        if (existingTrainCount != 0)
        {
            preferred = new LearnItem(current, string.Format(CultureInfo.CurrentCulture, Resources.FELISImport_LearnCurrent, current.Period));
            cbLearn.Items.Add(preferred);
        }

        foreach (var period in periods.Where(p => !ReferenceEquals(p, current)).OrderByDescending(p => p.GVD.StartValidTimeTable))
        {
            var item = new LearnItem(period, string.Format(CultureInfo.CurrentCulture, Resources.FELISImport_LearnOther, period.Period, period.GVD.TrainCount));
            cbLearn.Items.Add(item);
            if (preferred is null && period.GVD.TrainCount > 0)
                preferred = item;
        }

        cbLearn.SelectedItem = preferred ?? cbLearn.Items[0];
    }

    /// <summary>
    /// Uplne nahradenie z povodnych vlakov nic nepreberie - zdroj kratkeho hlasenia sa prepne na Nepreberat.
    /// Obsluha ho moze vybrat znova; pri inom rezime sa vrati povodny.
    /// </summary>
    private void rbReplace_CheckedChanged(object? sender, EventArgs e)
    {
        if (rbReplace.Checked)
        {
            _learnBeforeReplace = cbLearn.SelectedItem;
            cbLearn.SelectedIndex = 0;
        }
        else if (_learnBeforeReplace is not null && cbLearn.SelectedIndex == 0)
        {
            cbLearn.SelectedItem = _learnBeforeReplace;
            _learnBeforeReplace = null;
        }
    }

    private void bBrowse_Click(object sender, EventArgs e)
    {
        var dialog = new FolderBrowserDialog { Description = Resources.ElisImport_SelectFolder };
        if (dialog.ShowDialog(this) == DialogResult.Cancel)
            return;

        tbAppPath.Text = dialog.SelectedPath;
    }

    private void bPlanBrowse_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { Filter = Resources.FELISImport_PlanFilter + "|*.pdf", CheckFileExists = true };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            tbPlan.Text = dialog.FileName;
    }

    private void bImport_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(tbAppPath.Text))
        {
            Utils.ShowError(Resources.FELISImport_Zadaný_priečinok_neexistuje);
            DialogResult = DialogResult.None;
            return;
        }

        if (!File.Exists(Path.Combine(tbAppPath.Text, "TT.dll")))
        {
            Utils.ShowError(Resources.FELISImport_V_zadanom_priečinku_sa_nenachádza_knižnica_TT_dll);
            DialogResult = DialogResult.None;
            return;
        }

        var plan = tbPlan.Text.Trim();
        if (plan.Length != 0 && !File.Exists(plan))
        {
            Utils.ShowError(Resources.FELISImport_PlanMissing);
            DialogResult = DialogResult.None;
            return;
        }

        var mode = rbReplace.Checked && rbReplace.Enabled ? ElisImportMode.Replace
            : rbUpdate.Checked && rbUpdate.Enabled ? ElisImportMode.Update
            : ElisImportMode.Add;
        if (mode == ElisImportMode.Replace)
        {
            var answer = Utils.ShowQuestion(
                string.Format(CultureInfo.CurrentCulture, Resources.FELISImport_Nahradiť_všetky_vlaky_otázka, _existingTrainCount));

            if (answer != DialogResult.Yes)
            {
                DialogResult = DialogResult.None;
                return;
            }
        }

        ResultOptions = new ElisImportOptions
        {
            AppDirectory = tbAppPath.Text,
            OmitPassingTrains = cbSkipPassingTrains.Checked,
            ReorderTrains = cbReorder.Checked,
            Mode = mode,
            GenerateTableTexts = cbTableTexts.Checked,
            LearnFrom = (cbLearn.SelectedItem as LearnItem)?.Grafikon,
            PlanPdf = plan.Length != 0 ? plan : null
        };

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    /// <summary>
    /// Polozka ponuky grafikonov; <see langword="null" /> = nepreberat.
    /// </summary>
    private sealed record LearnItem(GVDDirectory? Grafikon, string Text)
    {
        public override string ToString() => Text;
    }
}
