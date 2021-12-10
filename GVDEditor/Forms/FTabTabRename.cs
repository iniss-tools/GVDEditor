using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog premenovania TabTab.
/// </summary>
internal partial class FTabTabRename : Form
{
    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTabRename"/>.
    /// </summary>
    public FTabTabRename()
    {
        InitializeComponent();

        FormUtils.SetFormFont(this);
        this.ApplyTheme();
    }

    public string NewTabName { get; private set; }

    private void bEdit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbName.Text))
        {
            Utils.ShowError(@"Nezadaný názov TabTab");
            DialogResult = DialogResult.None;
            return;
        }

        NewTabName = tbName.Text;
        DialogResult = DialogResult.OK;
    }
}