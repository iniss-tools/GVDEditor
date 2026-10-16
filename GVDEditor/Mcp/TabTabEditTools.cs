using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.TabTabEditor;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP na zmeny sekcii TabTab - kontrola ako v editore TabTab, nova chyba zmenu odmietne.</summary>
[McpServerToolType]
internal static class TabTabEditTools
{
    [McpServerTool(Name = "set_tabtab", Title = "Set TabTab section", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes the text or the key of a TabTab section, or creates a section when the key is new. The text is checked like in the TabTab "
                 + "editor: an error it did not have before refuses the change, the remaining findings come back. Read tabtab_reference first and check "
                 + "the result for a train with preview_board_text.")]
    public static async Task<TabTabChangeResult> SetTabTab(
        EditorAccess editor,
        [Description("Section key; a new section is created when none has it.")] string key,
        [Description("Whole text of the section (INISS board language, see tabtab_reference).")] string? text = null,
        [Description("New key (rename); catalog board columns keep using the section.")] string? newKey = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var (detail, change) = await editor.WriteAsync(
            context => string.Format(CultureInfo.CurrentCulture, Find(context, key) is null ? Resources.Agent_NewTabTab : Resources.Agent_SetTabTab, key),
            dryRun, context =>
            {
                var sections = context.Document.TabTabs;
                var tabTab = Find(context, key);
                List<string> before = tabTab is null ? [] : Errors(context, tabTab).Select(e => e.Key).ToList();
                if (tabTab is null)
                {
                    tabTab = new TableTabTab { Key = ValidName(key, sections.Select(t => t.Key)), Text = "" };
                    sections.Add(tabTab);
                }

                if (newKey is not null && newKey != tabTab.Key)
                    tabTab.Key = ValidName(newKey, sections.Where(t => t != tabTab).Select(t => t.Key));

                if (text is not null)
                {
                    // editor a subor maju riadky CRLF; subor je v CP1250 - iny znak by sa zapisal ako '?'
                    var lines = text.ReplaceLineEndings("\r\n");
                    if (Encodings.Win1250.GetString(Encodings.Win1250.GetBytes(lines)) != lines)
                        throw new McpException("tabtab.encoding: The text contains characters the TabTab file (Windows-1250) cannot store.");
                    tabTab.Text = lines;
                }

                if (Errors(context, tabTab).Where(e => !before.Contains(e.Key)).ToList() is { Count: > 0 } added)
                    throw new McpException("tabtab.invalid: " + string.Join(" ", added.Select(e => $"Line {e.Line}: {e.Message}")));
                return TableTools.TabTabDetail(context, tabTab);
            }).ConfigureAwait(false);
        return new TabTabChangeResult(change, detail);
    }

    [McpServerTool(Name = "delete_tabtab", Title = "Delete TabTab section", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Deletes a TabTab section no catalog board column uses (as TAB1 or TAB2); a used one is refused with the list of columns.")]
    public static async Task<TabTabChangeResult> DeleteTabTab(
        EditorAccess editor,
        [Description("Section key from list_tables.")] string key,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var (_, change) = await editor.WriteAsync(_ => string.Format(CultureInfo.CurrentCulture, Resources.Agent_DeleteTabTab, key), dryRun, context =>
        {
            var tabTab = Find(context, key) ?? throw new McpException($"tabtab.notFound: TabTab section '{key}' does not exist; use list_tables.");
            var usage = context.Document.TableCatalogs
                .SelectMany(c => c.Items.SelectMany(i => new[] { (Column: i, Tab: i.Tab1, Name: "TAB1"), (Column: i, Tab: i.Tab2, Name: "TAB2") })
                    .Where(x => x.Tab == tabTab).Select(x => $"catalog board {c.Key} column {x.Column.Key} ({x.Name})"))
                .ToList();
            if (usage.Count > 0)
                throw new McpException($"tabtab.inUse: Section '{tabTab.Key}' is used by {string.Join(", ", usage)}; change those columns first.");
            return context.Document.TabTabs.Remove(tabTab);
        }).ConfigureAwait(false);
        return new TabTabChangeResult(change, null);
    }

    private static TableTabTab? Find(EditorContext context, string key) =>
        TableMapper.Find(context.Document.TabTabs.Where(t => t != TableTabTab.Empty), t => t.Key, key);

    /// <summary>Nazov sekcie bez okolitych medzier; neplatny (prazdny, znaky, rovnaky ako iny) = chyba.</summary>
    private static string ValidName(string name, IEnumerable<string> others) =>
        TabTabSections.ValidateName(name, others, out var normalized) is { } error ? throw new McpException("tabtab.invalidName: " + error) : normalized;

    /// <summary>Chyby kontroly sekcie; kluc bez cisla riadka - pridanie riadku chybu nezmeni.</summary>
    private static List<(string Key, int Line, string Message)> Errors(EditorContext context, TableTabTab tabTab) =>
        TabTabValidator.Validate(tabTab.Text, new GvdExprSymbols(context.Workspace, context.Document).OptionsFor(tabTab)).Diagnostics
            .Where(d => d.Severity == ExprSeverity.Error)
            .Select(d => ($"{d.CodeName}: {d.Message}", d.LineIndex + 1, d.Message))
            .ToList();
}
