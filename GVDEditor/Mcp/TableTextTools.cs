using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP nad textami na tabuliach otvoreneho grafikonu.</summary>
[McpServerToolType]
internal static class TableTextTools
{
    /// <summary>Kolko zmien textov vlakov sa vypise pri jednom type textu.</summary>
    private const int MaxChanges = 20;

    [McpServerTool(Name = "regenerate_board_texts", Title = "Regenerate board texts", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Generates the board texts of trains (destination, stations on the way) from their routes like GVDEditor does, for every board text type "
                 + "with a realization in a column showing the destination or stations. By default all texts are generated again and texts or fonts edited "
                 + "by the user are replaced; onlyMissing only adds texts to trains without one. Run it with dryRun first to see what would change.")]
    public static async Task<RegenerateTextsResult> RegenerateBoardTexts(
        EditorAccess editor,
        [Description("Only add texts to trains that have none; existing texts stay.")] bool onlyMissing = false,
        [Description("Only this board text type (key from list_tables); null = all types.")] string? text = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var (texts, change) = await editor.WriteAsync(onlyMissing ? Resources.Agent_AddBoardTexts : Resources.Agent_RegenerateBoardTexts, dryRun, context =>
        {
            var document = context.Document;
            var station = context.Directory!.GVD.ThisStation;
            IEnumerable<TableText> selected = text is null
                ? document.TableTexts
                : [document.TableTexts.FirstOrDefault(t => string.Equals(t.Key, text, StringComparison.OrdinalIgnoreCase))
                   ?? throw new McpException($"tableText.notFound: Board text type '{text}' does not exist; use list_tables.")];
            return selected.Select(t => Generate(t, document.Trains, station, onlyMissing)).ToList();
        }).ConfigureAwait(false);
        return new RegenerateTextsResult(change, texts);
    }

    /// <summary>Vygeneruje texty jedneho typu ako pri ulozeni (alebo len chybajuce ako pri importe) a porovna ich s povodnymi.</summary>
    private static BoardTextResult Generate(TableText text, IReadOnlyCollection<Train> trains, Station station, bool onlyMissing)
    {
        if (TableTextGenerating.FindGeneratingRealization(text) is not { } realization)
            return new BoardTextResult(text.Key, text.Name, null, 0, 0, 0, []);

        var before = new Dictionary<Train, (string Text, int Font)>(ReferenceEqualityComparer.Instance);
        foreach (var tableTrain in text.Trains)
            before.TryAdd(tableTrain.Train, (tableTrain.Text, tableTrain.FontID));

        if (onlyMissing)
            TableTextGenerating.AddMissing([text], trains, station);
        else
            TableTextGenerating.RegenerateAll([text], trains, station);

        var after = new Dictionary<Train, (string Text, int Font)>(ReferenceEqualityComparer.Instance);
        foreach (var tableTrain in text.Trains)
            after.TryAdd(tableTrain.Train, (tableTrain.Text, tableTrain.FontID));

        var added = after.Keys.Count(t => !before.ContainsKey(t));
        var removed = before.Keys.Count(t => !after.ContainsKey(t));
        var changed = after.Count(pair => before.TryGetValue(pair.Key, out var old) && old != pair.Value);
        // v poradi grafikonu, odstranene (vlak uz v grafikone nie je) na konci
        var changes = trains.Concat(before.Keys.Where(t => !after.ContainsKey(t)))
            .Where(t => before.TryGetValue(t, out var old) ? !after.TryGetValue(t, out var now) || now != old : after.ContainsKey(t))
            .Take(MaxChanges)
            .Select(t => new TrainTextChange(t.ID, TrainRules.Label(t), before.TryGetValue(t, out var old) ? old.Text : null,
                after.TryGetValue(t, out var now) ? now.Text : null))
            .ToList();
        return new BoardTextResult(text.Key, text.Name, $"{realization.Table.Key}/{realization.Item.Name}", added, changed, removed, changes);
    }
}
