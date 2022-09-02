using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenie fyzickej tabule.
/// </summary>
public partial class FTablePhysical : Form
{
    private readonly bool copy;

    /// <summary>
    ///     Fyzicka tabula, ktoru upravuje tento dialog.
    /// </summary>
    public TablePhysical ThisTable;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTablePhysical"/>.
    /// </summary>
    /// <param name="table">Upravujuca tabula.</param>
    /// <param name="catalogs">Dostupne kataloge tabule.</param>
    /// <param name="copy">Ci sa jedna o kopiu.</param>
    public FTablePhysical(TablePhysical table, BindingList<TableCatalog> catalogs, bool copy = false)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        ThisTable = table;
        this.copy = copy;

        tbName.Text = table.Name;
        tbKey.Text = table.Key;

        cbCatalogTable.DataSource = catalogs;
        cbCatalogTable.SelectedItem = table.TableCatalog;

        tbComment.Text = table.Comment;

        nudComPort.Value = table.CommunicationPort;
        nudID.Value = table.ID;
        nudRecCount.Value = table.RecCount;

        if (string.IsNullOrEmpty(table.SaveXML))
        {
            cbXML.Checked = false;
            tbXMLName.Enabled = false;
            tbXMLName.Text = default;
        }
        else
        {
            cbXML.Checked = true;
            tbXMLName.Enabled = true;
            tbXMLName.Text = table.SaveXML;
        }

        tbREM.Text = table.Rem;
        tbReverseArrows.Text = table.ReverseArrows;

        if (!string.IsNullOrEmpty(table.Rem) || !string.IsNullOrEmpty(table.ReverseArrows)) cbAdvanced.Checked = true;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        var table = copy ? new TablePhysical() : ThisTable;

        if (string.IsNullOrEmpty(tbKey.Text) || string.IsNullOrEmpty(tbName.Text))
        {
            Utils.ShowError(Resources.Tables_Nie_sú_vyplnené_všetky_povinné_polia);
            DialogResult = DialogResult.None;
            return;
        }

        foreach (var t in GlobData.TablePhysicals)
            if (t.Key == tbKey.Text && table != t)
            {
                Utils.ShowError(Resources.Tables_Zadaný_kľúč_tabule_už_existuje);
                DialogResult = DialogResult.None;
                return;
            }

        table.Key = tbKey.Text;
        table.Name = tbName.Text;
        table.ID = decimal.ToInt32(nudID.Value);
        table.CommunicationPort = decimal.ToInt32(nudComPort.Value);
        table.RecCount = decimal.ToInt32(nudRecCount.Value);
        table.TableCatalog = (TableCatalog)cbCatalogTable.SelectedItem;
        table.Comment = tbComment.Text;
        var xml = tbXMLName.Text;
        table.SaveXML = string.IsNullOrEmpty(xml) ? "" : xml;
        table.Rem = tbREM.Text;
        table.ReverseArrows = tbReverseArrows.Text;

        if (copy) ThisTable = table;

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void cbXML_CheckedChanged(object sender, EventArgs e)
    {
        if (!cbXML.Checked)
        {
            tbXMLName.Enabled = false;
            tbXMLName.Text = default;
        }
        else
        {
            tbXMLName.Enabled = true;
            tbXMLName.Text = ThisTable.SaveXML;
        }
    }

    private void cbAdvanced_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAdvanced.Checked)
        {
            tbREM.Enabled = true;
            tbReverseArrows.Enabled = true;
        }
        else
        {
            tbREM.Enabled = false;
            tbReverseArrows.Enabled = false;
        }
    }

    private void FTablePhysical_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_TPHYSICAL);
    }
}