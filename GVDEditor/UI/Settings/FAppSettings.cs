using GVDEditor.Config;
using GVDEditor.Mcp;
using GVDEditor.Properties;
using ExControls;
using ToolsCore.Tools;
using ToolsCore.Commands;
using ToolsCore.Forms;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.UI.Settings;

internal partial class FAppSettings : FAppSettingsBase
{
    private new GVDEditorConfig Config => (GVDEditorConfig)base.Config;
    private new Styles<GVDEditorStyle> Styles => (Styles<GVDEditorStyle>)base.Styles;


    /// <inheritdoc />
    protected override string ReopenLastText => Resources.Settings_ReopenLast;


    /// <inheritdoc />
    protected override string EditorFontText => Resources.Settings_EditorFontHint;

    private readonly EditorContext _ctx;
    private readonly GvdMcpServer? _mcp;

    /// <param name="context">kontext editora</param>
    /// <param name="mcp">beziaci server MCP - jeho stav sa ukaze v nastaveni; null = bez stavu (harness snimok)</param>
    /// <param name="commands">prikazy hlavneho okna - strom skratiek podla jeho ponuky; null = podla katalogu</param>
    public FAppSettings(EditorContext context, GvdMcpServer? mcp = null, IEnumerable<AppCommand>? commands = null)
        : base(context.Config, new Styles<GVDEditorStyle>(context.Session.Styles), context.UsingStyle)
    {
        _ctx = context;
        _mcp = mcp;
        InitializeComponent();
        mcpPage.HintText = Resources.FAppSettings_Mcp_Hint;
        mcpPage.WritesText = Resources.FAppSettings_Mcp_Writes;
        mcpPage.WritesHintText = Resources.FAppSettings_Mcp_WritesHint;

        SetCommands(GvdCommands.Settings, commands);

        cbDateLimitLanguage.BindEnum(new Dictionary<AppLanguage, string>
        {
            [AppLanguage.Slovak] = "Slovenčina",
            [AppLanguage.Czech] = "Čeština" 
        });
        cbDateLimitLanguage.SelectedValue = Config.DateLimitLocate;
    }

    /// <inheritdoc />
    protected override void OnLoad()
    {
        base.OnLoad();
        cboxTabTextAutoGenerate.Checked = Config.AutoTableText;
        nudPlayerWordPause.Value = Config.PlayerSoundsOffset;
        cboxInissEvaluation.Checked = Config.InissSettingsShowEvaluation;
        mcpPage.LoadSettings(Config.McpServer, _mcp);
    }

    /// <inheritdoc />
    protected override bool OnSaving()
    {

        Config.DateLimitLocate = (AppLanguage)cbDateLimitLanguage.SelectedValue!;

        Config.AutoTableText = cboxTabTextAutoGenerate.Checked;
        Config.PlayerSoundsOffset = decimal.ToInt32(nudPlayerWordPause.Value);
        Config.InissSettingsShowEvaluation = cboxInissEvaluation.Checked;
        mcpPage.SaveSettings();
        return base.OnSaving();
    }

    /// <inheritdoc />
    protected override void SaveData()
    {
        // kopie - okno po Pouzit ostava otvorene a dalsie upravy v nom nesmu menit nastavenia programu
        _ctx.Session.Config = Config with { };
        _ctx.Session.Styles = new Styles<GVDEditorStyle>(Styles);
        _ctx.Session.UsingStyle = _ctx.Session.Styles[UsingStyle.Name];
        
        var configsDir = ToolsCore.AppPaths.ConfigDir;
        if (!Directory.Exists(configsDir))
            Directory.CreateDirectory(configsDir);

        Styles<GVDEditorStyle>.WriteData(PathUtils.CombinePath(configsDir, ToolsCore.FileConsts.FileStyles)!, _ctx.Session.Styles);
        XmlSerialization.WriteData(PathUtils.CombinePath(configsDir, ToolsCore.FileConsts.FileConfig)!, _ctx.Config);
    }
}
