using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.TabTabEditor;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP nad tabulami otvoreneho grafikonu.</summary>
[McpServerToolType]
internal static partial class TableTools
{
    [McpServerTool(Name = "list_tables", Title = "Boards", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("Boards of the open timetable: physical boards (lines and addresses), logical boards made of them, catalog board types, board texts, "
                 + "fonts and TabTab program sections.")]
    public static Task<TableList> ListTables(EditorAccess editor) =>
        editor.ReadAsync(context =>
        {
            var document = McpGuards.Open(context);
            return new TableList(
                document.TablePhysicals.Select(p => new PhysicalTableInfo(p.ID, p.Key, p.Name, p.CommunicationPort, p.TableCatalog?.Key)).ToList(),
                document.TableLogicals.Select(l => new LogicalTableInfo(l.Key, l.Name, l.ViewType?.Name,
                    l.Records.SelectMany(r => r.Positions).Select(p => p.Table?.Key).OfType<string>().Distinct().ToList())).ToList(),
                document.TableCatalogs.Select(c => new CatalogTableInfo(c.Key, c.Name, c.Manufacturer?.Name, c.Segments.Count)).ToList(),
                document.TableTexts.Select(t => new TableTextInfo(t.Key, t.Name, t.Realizations.Count, t.Trains.Count)).ToList(),
                document.TableFonts.Select(f => $"{f.FontID} - {f.Name}").ToList(),
                document.TabTabs.Select(t => t.Key).ToList());
        });

    [McpServerTool(Name = "get_tabtab", Title = "TabTab section", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("One TabTab section of the open timetable - the INISS rules recoding what boards show: its text, all check findings with line "
                 + "numbers, decoded font numbers and the board columns using it. Read tabtab_reference first to interpret the rules correctly and check "
                 + "the result for a train with preview_board_text.")]
    public static Task<TabTabInfo> GetTabTab(EditorAccess editor, [Description("Section key from list_tables.")] string key) =>
        editor.ReadAsync(context =>
        {
            var document = McpGuards.Open(context);
            var tabTab = document.TabTabs.FirstOrDefault(t => string.Equals(t.Key, key, StringComparison.OrdinalIgnoreCase))
                         ?? throw new McpException($"tabtab.notFound: TabTab section '{key}' does not exist; use list_tables.");
            var diagnostics = TabTabValidator.Validate(tabTab.Text, new GvdExprSymbols(context.Workspace, document).OptionsFor(tabTab)).Diagnostics
                .Select(d => new TabTabDiagnosticInfo(d.Severity switch { ExprSeverity.Error => "error", ExprSeverity.Warning => "warning", _ => "info" },
                    d.LineIndex + 1, d.CodeName, d.Message, d.Suggestion))
                .ToList();
            var fonts = FontCode().Matches(tabTab.Text).Select(m => int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture)).Distinct().Order()
                .Select(code => new FontCodeInfo(code, DescribeElen(new ElenFontCode(code)),
                    document.TableFonts.FirstOrDefault(f => f.FontID == code) is { } font ? $"{font.Name} ({font.Type?.Name}, size {font.Size})" : null))
                .ToList();
            var usedBy = document.TableCatalogs
                .SelectMany(c => c.Items.SelectMany(i => new[] { (Item: i, Tab: i.Tab1, Name: "TAB1"), (Item: i, Tab: i.Tab2, Name: "TAB2") })
                    .Where(x => x.Tab == tabTab)
                    .Select(x => new TabTabUsage(c.Key, x.Item.Name, x.Name, x.Item.DivType is { } divType ? divType.Id : null,
                        document.TablePhysicals.Where(p => p.TableCatalog == c).Select(p => p.Key).ToList())))
                .ToList();
            return new TabTabInfo(tabTab.Key, tabTab.Text, diagnostics, fonts, usedBy);
        });

    [McpServerTool(Name = "preview_board_text", Title = "Board text of a train", ReadOnly = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Composes the texts board columns show for a train exactly as INISS does (the preview of the TabTab editor): the result with font "
                 + "and every step of the fixed order. Optionally simulates the train state - lockout, diversion, delays. Use it to verify how a "
                 + "TabTab section behaves instead of guessing.")]
    public static Task<BoardPreview> PreviewBoardText(
        EditorAccess editor,
        [Description("Train id from list_trains.")] int train,
        [Description("Only columns using this TabTab section.")] string? section = null,
        [Description("Only columns of this catalog board (key).")] string? catalog = null,
        [Description("Track lockout on arrival (flag Prizn_VylP).")] bool lockoutArrival = false,
        [Description("Track lockout on departure (flag Prizn_VylO).")] bool lockoutDeparture = false,
        [Description("The train is diverted.")] bool diverted = false,
        [Description("Arrival delay in minutes.")] int arrivalDelay = 0,
        [Description("Departure delay in minutes.")] int departureDelay = 0,
        [Description("The train stands in the station.")] bool standing = false,
        [Description("The train has been dispatched.")] bool dispatched = false) =>
        editor.ReadAsync(context =>
        {
            var document = McpGuards.Open(context);
            var found = document.Trains.FirstOrDefault(t => t.ID == train)
                        ?? throw new McpException($"train.notFound: Train {train} does not exist; use list_trains.");
            var station = context.Directory!.GVD.ThisStation;
            var runtime = new TrainRuntime
            {
                ArrivalDelayMinutes = arrivalDelay,
                DepartureDelayMinutes = departureDelay,
                IsStanding = standing,
                IsDispatched = dispatched,
                LockoutArrival = lockoutArrival,
                LockoutDeparture = lockoutDeparture,
                IsDeflected = diverted
            };
            var trainContext = new GvdTrainContext(found, runtime, int.TryParse(station.ID, CultureInfo.InvariantCulture, out var id) ? id : 0);
            Dictionary<string, TabTabSection?> sections = new(StringComparer.Ordinal);
            var composer = new BoardColumnComposer(document, name =>
            {
                if (!sections.TryGetValue(name, out var parsed))
                    sections[name] = parsed = document.TabTabs.FirstOrDefault(t => t.Key == name) is { } tab ? TabTabSection.Parse(tab.Text) : null;
                return parsed;
            });
            var columns = document.TableCatalogs
                .Where(c => catalog is null || string.Equals(c.Key, catalog, StringComparison.OrdinalIgnoreCase))
                .SelectMany(c => c.Items
                    .Where(i => section is null || string.Equals(i.Tab1?.Key, section, StringComparison.OrdinalIgnoreCase)
                                                || string.Equals(i.Tab2?.Key, section, StringComparison.OrdinalIgnoreCase))
                    .Select(i =>
                    {
                        var result = composer.Compose(c, i, trainContext);
                        return new BoardColumnText(c.Key, i.Name, i.DivType.Id, Key(i.Tab1), Key(i.Tab2), trainContext.OwnValue(i).Text, result.Value.Text,
                            result.Value.Font, result.Error, result.Steps.Select(s => new ComposeStepInfo(s.Source, s.Value.Text, s.Value.Font, s.Note)).ToList());
                    }))
                .ToList();
            return new BoardPreview(TrainTools.Row(found), columns);
        });

    private static string? Key(TableTabTab? tab) => tab is null || tab == TableTabTab.Empty ? null : tab.Key;

    [McpServerTool(Name = "tabtab_reference", Title = "TabTab language", ReadOnly = true, Idempotent = true, OpenWorld = false)]
    [Description("Reference of the TabTab language of INISS boards: rule direction, #SWITCH/#MERGE/#VYLUKA/#ODKLON, the fixed order of composing a "
                 + "column text, DIVTYPE, conditions (precedence!) and ELEN font numbers. Read it before explaining or changing TabTab sections.")]
    public static string GetTabTabReference() => TabTabReference.Text;

    [GeneratedRegex(@"\{(\d+)\}")]
    private static partial Regex FontCode();

    /// <summary>Vyznam cisla pisma ELEN po anglicky (popisy GVDEditora su v jazyku obsluhy).</summary>
    private static string DescribeElen(ElenFontCode code)
    {
        string[] colors = ["no colour", "red", "green", "yellow"];
        string[] faces = ["fixed 6 px", "thin", "bold", "digits only"];
        var parts = new List<string> { colors[code.Color], code.ExtendedFont > 0 ? $"extended font {code.ExtendedFont}" : faces[code.Face] };
        if (code.Blinks)
            parts.Add("blinking");
        if (code.TallDigits)
            parts.Add("tall digits");
        return string.Join(", ", parts);
    }
}
