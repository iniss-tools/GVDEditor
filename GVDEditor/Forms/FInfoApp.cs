using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Informacie o aplikacii.
/// </summary>
public partial class FInfoApp : Form
{
    /// <summary>
    ///     Vytvori novy formular typu <see cref="FInfoApp"/>.
    /// </summary>
    public FInfoApp()
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();

        lAppVersion.Text = Application.ProductVersion;
        lAppName.Text = Application.ProductName;
        lAppName.Font = new Font(lAppName.Font, FontStyle.Bold);

        linkWeb.Text = LinkConsts.LINK_INFO_APP;
        linkEmail.Text = LinkConsts.EMAIL;
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(LinkConsts.LINK_INFO_APP);
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("mailto:" + LinkConsts.EMAIL);
    }

    private void InformationForm_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_INFO_APP);
    }
}