using System.Collections;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP nad vlakmi otvoreneho grafikonu.</summary>
[McpServerToolType]
internal static class TrainTools
{
    private const int MaxLimit = 500;

    [McpServerTool(Name = "list_trains", Title = "Trains", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("Trains of the open timetable (with unsaved changes), sorted by time. All filters are optional and combine: text in number or name, "
                 + "train type key, station (origin, destination or route), track key, time window of arrival or departure and a day the train runs on.")]
    public static Task<TrainList> ListTrains(
        EditorAccess editor,
        [Description("Text in the train number or name.")] string? text = null,
        [Description("Train type key, e.g. Os, R.")] string? type = null,
        [Description("Text in the origin, destination or a route station.")] string? station = null,
        [Description("Track key.")] string? track = null,
        [Description("Arrival or departure at or after this time.")] TimeOnly? from = null,
        [Description("Arrival or departure at or before this time.")] TimeOnly? to = null,
        [Description("Only trains running on this day (calendar restriction applied).")] DateOnly? day = null,
        [Description("Trains to skip.")] int offset = 0,
        [Description("Most trains to return, 1-500.")] int limit = 100) =>
        editor.ReadAsync(context =>
        {
            var document = McpGuards.Open(context);
            if (limit is < 1 or > MaxLimit)
                throw new McpException($"trains.limit: The limit must be 1 to {MaxLimit}.");
            var matching = document.Trains
                .Where(t => text is null || Contains(t.Number, text) || Contains(t.Name, text))
                .Where(t => type is null || string.Equals(t.Type?.Key, type, StringComparison.OrdinalIgnoreCase))
                .Where(t => station is null || Stations(t).Any(s => Contains(s.Name, station)))
                .Where(t => track is null || string.Equals(t.Track?.Key, track, StringComparison.OrdinalIgnoreCase)
                                          || string.Equals(t.TrackDeparture?.Key, track, StringComparison.OrdinalIgnoreCase))
                .Where(t => from is null && to is null || Times(t).Any(time => (from is null || time >= from) && (to is null || time <= to)))
                .Where(t => day is null || RunsOn(t, day.Value))
                .OrderBy(t => Times(t).DefaultIfEmpty(TimeOnly.MinValue).Min())
                .ThenBy(t => t.Number, StringComparer.Ordinal)
                .ToList();
            return new TrainList(matching.Count, matching.Skip(Math.Max(0, offset)).Take(limit).Select(Row).ToList());
        });

    [McpServerTool(Name = "get_train", Title = "Train", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("All data of one train of the open timetable: times, tracks, route stations, validity, flags, announcement languages, supplements and "
                 + "formations.")]
    public static Task<TrainDetail> GetTrain(EditorAccess editor, [Description("Train id from list_trains.")] int id) =>
        editor.ReadAsync(context =>
        {
            var train = McpGuards.Open(context).Trains.FirstOrDefault(t => t.ID == id)
                        ?? throw new McpException($"train.notFound: Train {id} does not exist; use list_trains.");
            return new TrainDetail(
                Row(train),
                train.ZaciatokPlatnosti,
                train.KoniecPlatnosti,
                train.StaniceZoSmeru.Select(s => s.Name).ToList(),
                train.StaniceDoSmeru.Select(s => s.Name).ToList(),
                train.TrackDeparture?.Key,
                train.LineArrival,
                train.LineDeparture,
                train.Languages.Select(l => l.Name).ToList(),
                Flags(train),
                train.LockoutNumber,
                train.Doplnky.Select(d => new TrainSupplement(d.Name, d.Sound?.Name, Reports(d.ChosenReports))).ToList(),
                train.Radenia.Select(r => new TrainFormation(r.Text, r.DestStation?.Name, r.CisloVlaku, r.Validity?.ToString(), Reports(r.ChosenReports)))
                    .ToList());
        });

    [McpServerTool(Name = "explain_validity", Title = "Explain validity text", ReadOnly = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Days on which a calendar restriction text (as in train validity, e.g. 'jazdí v 1-5, nejazdí 24.XII.') applies, within the validity "
                 + "of the open timetable or a given period, and the text as GVDEditor would write it.")]
    public static Task<ValidityDays> ExplainValidity(
        EditorAccess editor,
        [Description("Calendar restriction text; empty = daily.")] string text,
        [Description("First day of the period; default = start of the open timetable.")] DateOnly? from = null,
        [Description("Last day of the period; default = end of the open timetable.")] DateOnly? to = null) =>
        editor.ReadAsync(context =>
        {
            var start = from ?? context.Directory?.GVD.StartValidTimeTable
                ?? throw new McpException("validity.period: No timetable is open; give from and to.");
            var end = to ?? context.Directory?.GVD.EndValidTimeTable ?? start;
            if (end < start)
                throw new McpException("validity.period: The period ends before it starts.");
            var limit = new DateLimit(start, end);
            BitArray bits;
            try
            {
                bits = limit.TextToBitArray(text);
            }
            catch (Exception e) when (e is not McpException)
            {
                throw new McpException("validity.text: " + e.Message);
            }
            return new ValidityDays(limit.BitArrayToText(bits), bits.Cast<bool>().Count(b => b), Ranges(bits, start));
        });

    public static TrainRow Row(Train t) =>
        new(t.ID, t.Number, t.Variant, t.Type?.Key, t.Name, t.Routing?.Name, t.StartingStation?.Name, t.EndingStation?.Name, t.Arrival, t.Departure,
            t.Track?.Key, t.Operator?.Name, t.DateLimitText);

    private static bool Contains(string? value, string text) => value?.Contains(text, StringComparison.CurrentCultureIgnoreCase) == true;

    private static IEnumerable<Station> Stations(Train t) =>
        t.StaniceZoSmeru.Concat(t.StaniceDoSmeru).Append(t.StartingStation).Append(t.EndingStation).OfType<Station>();

    private static IEnumerable<TimeOnly> Times(Train t) => new[] { t.Arrival, t.Departure }.OfType<TimeOnly>();

    /// <summary>Vlak ide v den <paramref name="day" /> podla platnosti a kalendarneho obmedzenia.</summary>
    private static bool RunsOn(Train t, DateOnly day)
    {
        if (day < t.ZaciatokPlatnosti || day > t.KoniecPlatnosti)
            return false;
        try
        {
            var bits = new DateLimit(t.ZaciatokPlatnosti, t.KoniecPlatnosti).TextToBitArray(t.DateLimitText);
            var index = day.DayNumber - t.ZaciatokPlatnosti.DayNumber;
            return index < bits.Length && bits[index];
        }
        catch (Exception e) when (e is not McpException)
        {
            // neplatny text ukaze analyza - vlak sa nevylucuje
            return true;
        }
    }

    private static List<string> Flags(Train t) =>
        new (bool On, string Name)[]
            {
                (t.IsMedzistatny, "international"), (t.IsDialkovy, "longDistance"), (t.IsMimoriadny, "extraordinary"), (t.IsMiestenkovy, "reservation"),
                (t.IsIbaLozkovy, "sleeperOnly"), (t.IsNizkopodlazny, "lowFloor"), (t.IsPrestupovy, "transfer"), (t.IsPriznakO, "flagO"),
                (t.IsMotorovy, "diesel")
            }
            .Where(f => f.On).Select(f => f.Name).ToList();

    private static List<string> Reports(IEnumerable<ChosenReportType>? reports) =>
        reports?.Select(r => r.Variants is { Count: > 0 } variants ? $"{r.Type.Name} ({string.Join(", ", variants.Select(v => v.Name))})" : r.Type.Name)
            .ToList() ?? [];

    private static List<DayRange> Ranges(BitArray bits, DateOnly start)
    {
        List<DayRange> ranges = [];
        for (var i = 0; i < bits.Length; i++)
        {
            if (!bits[i])
                continue;
            var first = i;
            while (i + 1 < bits.Length && bits[i + 1])
                i++;
            ranges.Add(new DayRange(start.AddDays(first), start.AddDays(i)));
        }
        return ranges;
    }
}
