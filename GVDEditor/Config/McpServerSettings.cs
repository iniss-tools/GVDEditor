using ToolsCore.Mcp;

namespace GVDEditor.Config;

/// <summary>
/// Server MCP pre agentov (Claude Code, VS Code) - bezi v GVDEditore na tomto pocitaci a pristupuje sa k nemu klucom.
/// </summary>
public sealed class McpServerSettings() : McpSettings("GVDEditor", "gvdeditor", Prefix, DefaultServerPort)
{
    /// <summary>Predvoleny port servera.</summary>
    public const int DefaultServerPort = 5490;

    /// <summary>Zaciatok kluca - odlisi ho od klucov simulatora (<c>ts_</c>) a GVXEditora (<c>gvx_</c>).</summary>
    public const string Prefix = "gvd_";
}
