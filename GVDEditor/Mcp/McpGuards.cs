using ModelContextProtocol;

namespace GVDEditor.Mcp;

/// <summary>Spolocne kontroly nastrojov.</summary>
internal static class McpGuards
{
    /// <summary>Otvoreny grafikon; bez neho chyba nastroja.</summary>
    public static Domain.Documents.GrafikonDocument Open(EditorContext context) =>
        context.Directory is null
            ? throw new McpException("grafikon.notOpen: No timetable is open in GVDEditor; open one or call list_grafikons.")
            : context.Document;
}