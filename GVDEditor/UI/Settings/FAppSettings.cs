using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Mcp;
using GVDEditor.Properties;
using ExControls;
using ToolsCore.Tools;
using ToolsCore;
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
        cboxMcpEnabled.Checked = Config.McpServer.Enabled;
        cboxMcpWrites.Checked = Config.McpServer.AllowWrites;
        nudMcpPort.Value = Math.Clamp(Config.McpServer.Port, (int)nudMcpPort.Minimum, (int)nudMcpPort.Maximum);
        UpdateMcpStatus();
    }

    /// <summary>Nastavenie servera MCP z okna (kluc sa nemeni - ten meni len tlacidlo Novy kluc).</summary>
    private McpServerSettings McpFromForm()
    {
        var settings = Config.McpServer.Clone();
        settings.Enabled = cboxMcpEnabled.Checked;
        settings.AllowWrites = cboxMcpWrites.Checked;
        settings.Port = decimal.ToInt32(nudMcpPort.Value);
        return settings;
    }

    /// <summary>Okno sa lisi od nastavenia, s ktorym server bezi - prejavi sa az po ulozeni.</summary>
    private bool McpPending => !McpFromForm().SameAs(_mcp?.Settings ?? _ctx.Config.McpServer);

    private void UpdateMcpStatus()
    {
        bMcpClaude.Enabled = bMcpVsCode.Enabled = cboxMcpWrites.Enabled = cboxMcpEnabled.Checked;
        lMcpStatus.Text = McpPending ? Resources.FAppSettings_Mcp_Pending
            : _mcp?.Address is { } address ? string.Format(CultureInfo.CurrentCulture, Resources.FAppSettings_Mcp_Running, address)
            : _mcp?.Error is { } error ? string.Format(CultureInfo.CurrentCulture, Resources.FAppSettings_Mcp_Failed, error)
            : Resources.FAppSettings_Mcp_Off;
    }

    private void McpSettingChanged(object? sender, EventArgs e) => UpdateMcpStatus();

    /// <summary>Konfiguracia pre klienta do schranky; kluc sa vytvori, ak este nie je.</summary>
    private void CopyMcpConfig(Func<string, string, string> text)
    {
        var key = Config.McpServer.EnsureKey();
        Clipboard.SetText(text(McpFromForm().Address, key));
        lMcpStatus.Text = McpPending ? Resources.FAppSettings_Mcp_CopiedPending : Resources.FAppSettings_Mcp_Copied;
    }

    private void bMcpClaude_Click(object? sender, EventArgs e) =>
        CopyMcpConfig((address, key) => $"claude mcp add --transport http gvdeditor {address} --header \"Authorization: Bearer {key}\"");

    // patri do pouzivatelskeho mcp.json VS Code (MCP: Open User Configuration), nie do priecinka projektu - je v nom kluc
    private void bMcpVsCode_Click(object? sender, EventArgs e) =>
        CopyMcpConfig((address, key) => $$"""
            {
              "servers": {
                "gvdeditor": {
                  "type": "http",
                  "url": "{{address}}",
                  "headers": { "Authorization": "Bearer {{key}}" }
                }
              }
            }
            """);

    private void bMcpNewKey_Click(object? sender, EventArgs e)
    {
        if (Config.McpServer.Key.Length > 0 && Utils.ShowQuestion(Resources.FAppSettings_Mcp_NewKey) != DialogResult.Yes)
            return;
        Config.McpServer.Key = McpServerSettings.NewKey();
        UpdateMcpStatus();
    }

    /// <inheritdoc />
    protected override bool OnSaving()
    {

        Config.DateLimitLocate = (AppLanguage)cbDateLimitLanguage.SelectedValue!;

        Config.AutoTableText = cboxTabTextAutoGenerate.Checked;
        Config.PlayerSoundsOffset = decimal.ToInt32(nudPlayerWordPause.Value);
        Config.InissSettingsShowEvaluation = cboxInissEvaluation.Checked;
        Config.McpServer.Enabled = cboxMcpEnabled.Checked;
        Config.McpServer.AllowWrites = cboxMcpWrites.Checked;
        Config.McpServer.Port = decimal.ToInt32(nudMcpPort.Value);
        if (Config.McpServer.Enabled)
            Config.McpServer.EnsureKey();
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
