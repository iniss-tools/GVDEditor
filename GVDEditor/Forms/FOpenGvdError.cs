using ExControls;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

public partial class FOpenGvdError : Form
{
    private FOpenGvdError(string msg)
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();
        pbIcon.Image = new ShellIcon(ShellIconType.Error,ShellIconSize.Large).ToBitmap();
        rtbMessage.Text = msg;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    public static void ShowError(string msg)
    {
        var dialog = new FOpenGvdError(msg);
        dialog.ShowDialog();
    }
}