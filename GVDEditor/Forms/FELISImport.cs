using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Volby importu vlakov priamo z dat programu ELIS.
/// </summary>
public partial class FELISImport : Form
{
    internal FMain.SendData ResultOptions = null!;

    private readonly int _existingTrainCount;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FELISImport" />.
    /// </summary>
    /// <param name="stationName">Nazov stanice, pre ktoru sa vlaky nacitaju.</param>
    /// <param name="existingTrainCount">Pocet vlakov, ktore uz grafikon obsahuje.</param>
    public FELISImport(string stationName, int existingTrainCount)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _existingTrainCount = existingTrainCount;

        lStation.Text = string.Format(Resources.FELISImport_Vlaky_sa_načítajú_pre_stanicu, stationName);
        tbAppPath.Text = FMain.SendData.DefaultElisDirectory;

        //ak grafikon este ziadne vlaky nema, nie je co nahradzat
        cbReplace.Enabled = existingTrainCount != 0;
    }

    private void bBrowse_Click(object sender, EventArgs e)
    {
        var dialog = new FolderBrowserDialog { Description = "Vyberte priečinok s aplikáciou Cestovné poriadky" };
        if (dialog.ShowDialog(this) == DialogResult.Cancel)
            return;

        tbAppPath.Text = dialog.SelectedPath;
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

        var replace = cbReplace.Checked && cbReplace.Enabled;
        if (replace)
        {
            var answer = Utils.ShowQuestion(
                string.Format(Resources.FELISImport_Nahradiť_všetky_vlaky_otázka, _existingTrainCount));

            if (answer != DialogResult.Yes)
            {
                DialogResult = DialogResult.None;
                return;
            }
        }

        ResultOptions = new FMain.SendData
        {
            AppDirectory = tbAppPath.Text,
            RegistrationNumber = tbReg.Text.Trim(),
            OmitPassingTrains = cbSkipPassingTrains.Checked,
            ReorderTrains = cbReorder.Checked,
            ReplaceTrains = replace
        };

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
}
