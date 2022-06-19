namespace GVDEditor.Forms;

public partial class FAppSettingsNew : Form
{
    public const string SETTING_GENERAL = nameof(panelGeneral);
    public const string SETTING_ENVIRONMENT = nameof(panelEnvironment);
    public const string SETTING_SHORTCUTS = nameof(panelShortcuts);
    public const string SETTING_DESKTOP = nameof(panelDesktop);
    public const string SETTING_COLORS = nameof(panelColors);
    public const string SETTING_LOCALIZATION = nameof(panelLocalization);
    public const string SETTING_FONTS = nameof(panelFonts);
    public const string SETTING_LOGGING = nameof(panelLogging);
    public const string SETTING_STARTUPINISS = nameof(panelStartupINISS);

    public FAppSettingsNew()
    {
        InitializeComponent();
        linkShortcuts.Tag = SETTING_SHORTCUTS;
        linkDesktop.Tag = SETTING_DESKTOP;
        linkColors.Tag = SETTING_COLORS;
        linkLocalization.Tag = SETTING_LOCALIZATION;
        linkFonts.Tag = SETTING_FONTS;
    }

    private void EnvironmentLinksClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        var control = (LinkLabel) sender;
        optionsView.ShowPanel(control.Tag.ToString());
    }

    private void FAppSettingsNew_Load(object sender, EventArgs e)
    {

    }
}