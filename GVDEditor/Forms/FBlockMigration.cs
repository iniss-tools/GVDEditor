using System.Globalization;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - rozdelenie priecinka so starsim zapisom grafikonu (bloky <c>/&lt;cislo stanice&gt;</c>)
///     na samostatne priecinky. Pouzivatel vidi bloky a moze upravit nazvy novych priecinkov.
/// </summary>
public partial class FBlockMigration : Form
{
    private readonly List<GvdBlock> _blocks;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FBlockMigration" />.
    /// </summary>
    /// <param name="sourcePath">Priecinok so starym zapisom (len na zobrazenie).</param>
    /// <param name="blocks">Bloky z <see cref="BlockMigrator.Analyze" />; nazvy priecinkov sa po potvrdeni zapisu spat do nich.</param>
    /// <param name="isDataRoot">Grafikon lezi priamo v DATA (bez DirList.TXT) - presuva sa do vlastneho priecinka.</param>
    internal FBlockMigration(string sourcePath, List<GvdBlock> blocks, bool isDataRoot = false)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        _blocks = blocks;
        var info = isDataRoot ? Properties.Resources.FBlockMigration_Info_DataRoot : lInfo.Text;
        lInfo.Text = $"{sourcePath}{Environment.NewLine}{info}";
        if (isDataRoot)
            lNote.Text = Properties.Resources.FBlockMigration_Note_DataRoot;

        foreach (var block in _blocks)
            dgvBlocks.Rows.Add(
                block.Index,
                $"{block.StationId} – {block.StationName}",
                block.TrainCount,
                block.StartValid.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                block.EndValid.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                block.DirName);
    }

    private void bOK_Click(object sender, EventArgs e)
    {
        dgvBlocks.EndEdit();

        var invalid = Path.GetInvalidFileNameChars();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < _blocks.Count; i++)
        {
            var name = (dgvBlocks.Rows[i].Cells[colDir.Index].Value?.ToString() ?? "").Trim();

            // ciarka by rozbila DirList.TXT (zapisuje sa bez uvodzoviek), bodku na konci Windows z nazvu odstrani
            if (name.Length == 0 || name.IndexOfAny(invalid) >= 0 || name.Contains(',') || name.EndsWith('.'))
            {
                Utils.ShowError(string.Format(Properties.Resources.BlockMigrator_Neplatny_nazov_priecinka, name));
                return;
            }

            if (!names.Add(name))
            {
                Utils.ShowError(string.Format(Properties.Resources.BlockMigrator_Duplicitny_nazov_priecinka, name));
                return;
            }

            if (Directory.Exists(Utils.CombinePath(GlobData.DataDir, name)!))
            {
                Utils.ShowError($"{name}: {Properties.Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov}");
                return;
            }

            _blocks[i].DirName = name;
        }

        DialogResult = DialogResult.OK;
    }
}
