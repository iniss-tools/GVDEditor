using System.Reflection;
using GVDEditor.Config;
using Microsoft.Extensions.DependencyInjection;
using ToolsCore.Mcp;

namespace GVDEditor.Mcp;

/// <summary>
/// Server MCP vnutri GVDEditora (spolocny server <see cref="McpServerHost" />) - agent cita otvoreny grafikon aj
/// s neulozenymi zmenami a s povolenim ho meni cez <see cref="EditorAccess" />.
/// </summary>
internal sealed class GvdMcpServer : IMcpServerStatus, IDisposable
{
    public const string Pattern = McpServerHost.Pattern;

    /// <summary>Pokyny pre agenta pri pripojeni.</summary>
    private const string Instructions =
        "GVDEditor edits timetables (grafikon) of INISS, the Slovak and Czech railway station information system (boards and announcements). "
        + "Tools read the timetable open in the running editor including unsaved changes; list_grafikons shows the other timetables of the "
        + "installation. Train data use Slovak terms: routing originates/passes/terminates, validity is a calendar restriction text "
        + "(explain_validity expands it to days), TabTab is the INISS language deciding what boards show - read tabtab_reference before explaining it and do not guess its "
        + "semantics.";

    /// <summary>Pokyny podla povolenia zmien.</summary>
    private const string ReadOnlyInstructions = " Changes are made by the user in GVDEditor.";

    private const string WriteInstructions =
        " Tools that change the timetable work in the editor's memory only: each call is one undo step the user can revert in GVDEditor, nothing is "
        + "saved - the user reviews the changes and saves the timetable. Call a change with dryRun first when its effect is not obvious.";

    private readonly McpServerHost _host =
        new(new McpServerDefinition("GVDEditor", Instructions, ReadOnlyInstructions, WriteInstructions, typeof(GvdMcpServer).Assembly));

    /// <inheritdoc />
    public string? Address => _host.Address;

    /// <inheritdoc />
    public string? Error => _host.Error;

    /// <inheritdoc />
    public McpSettings? Settings => _host.Settings;

    /// <summary>
    /// Server podla <paramref name="settings" />: zastavi beziaci a pri zapnutom nastaveni spusti novy. Chybu nehadze -
    /// je v <see cref="Error" /> (GVDEditor bez servera funguje dalej).
    /// </summary>
    public Task ApplyAsync(EditorAccess editor, McpServerSettings settings) =>
        _host.ApplyAsync(settings, services => services.AddSingleton(editor));

    /// <summary>Staticke metody nastrojov GVDEditora.</summary>
    public static IReadOnlyList<MethodInfo> ToolMethods() => McpServerHost.ToolMethods(typeof(GvdMcpServer).Assembly);

    /// <summary>Nastroj grafikon nemeni.</summary>
    public static bool IsReadOnly(MethodInfo method) => McpServerHost.IsReadOnly(method);

    public void Dispose() => _host.Dispose();
}
