using System.Globalization;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP na zmeny vlakov - rovnake kontroly a ulozenie ako okno Upravit vlak (bez dodatkov a radeni).</summary>
[McpServerToolType]
internal static class TrainEditTools
{
    [McpServerTool(Name = "set_train", Title = "Set train", ReadOnly = false, Destructive = true, Idempotent = false, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes a train (id) or adds one - empty or as a copy of another train (copyFrom) - with the checks of the Edit train window: errors "
                 + "refuse the change, warnings are returned. Only the given values change; the station lists replace the whole part of the route "
                 + "(get_train gives the current one). GVDEditor numbers the variants; supplements and formations stay (a copy takes them over). A new "
                 + "train has no board texts - regenerate_board_texts with onlyMissing adds them. Train ids are positions in the list.")]
    public static async Task<TrainChangeResult> SetTrain(
        EditorAccess editor,
        [Description("Train id from list_trains; null = a new train.")] int? id = null,
        [Description("Id of the train a new one starts as a copy of.")] int? copyFrom = null,
        [Description("Train number.")] string? number = null,
        [Description("Train type key, e.g. Os, R (get_station).")] string? type = null,
        [Description("Train name; a name from the sound bank is stored as its sound key.")] string? name = null,
        [Description("Operator name or number (get_station).")] string? @operator = null,
        [Description("Arrival time HH:mm; used when the train has stations before this one.")] string? arrival = null,
        [Description("Departure time HH:mm; used when the train has stations after this one.")] string? departure = null,
        [Description("Arrival track key (get_station).")] string? track = null,
        [Description("Departure track key; the arrival track = the same track.")] string? departureTrack = null,
        [Description("Line of arrival (up to 20 letters and digits); empty = none.")] string? lineArrival = null,
        [Description("Line of departure; empty = none.")] string? lineDeparture = null,
        [Description("Calendar restriction text, e.g. 'jazdí v 1-5' (explain_validity); empty = daily.")] string? validity = null,
        [Description("First day of validity.")] DateOnly? validFrom = null,
        [Description("Last day of validity.")] DateOnly? validTo = null,
        [Description("Stations before this station (where the train comes from), in order; empty = the train originates here.")]
        IReadOnlyList<RouteStopInput>? stationsBefore = null,
        [Description("Stations after this station (where the train goes), in order; empty = the train terminates here.")]
        IReadOnlyList<RouteStopInput>? stationsAfter = null,
        [Description("Languages of announcements besides the basic one (keys); replaces the list.")] IReadOnlyList<string>? languages = null,
        [Description("All flags of the train, replacing the current ones: international, longDistance, extraordinary, reservation, sleeperOnly, "
                     + "lowFloor, transfer, flagO, diesel.")]
        IReadOnlyList<string>? flags = null,
        [Description("Lockout number; 0 = none.")] int? lockout = null,
        [Description("A changed number, name or type also changes the other variants of the train; false = the train leaves its variant group.")]
        bool renameVariants = true,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        if (id is not null && copyFrom is not null)
            throw new McpException("train.invalid: Give id to change a train or copyFrom to add a copy, not both.");

        List<string> warnings = [];
        var (detail, change) = await editor.WriteAsync(context => Label(context.Document, id, copyFrom, type, number, name), dryRun, context =>
        {
            var document = context.Document;
            var gvd = context.Directory!.GVD;
            var trains = document.Trains;
            var train = id is { } trainId ? TrainTools.Find(document, trainId) : null;
            var source = copyFrom is { } sourceId ? TrainTools.Find(document, sourceId) : null;
            var row = train is null ? trains.Count : trains.IndexOf(train);

            TrainDraft draft;
            if ((train ?? source) is { } existing)
                draft = TrainDraft.From(existing);
            else
            {
                // ako novy vlak v okne
                draft = TrainDraft.New(gvd.StartValidTimeTable, gvd.EndValidTimeTable);
                draft.Type = context.Workspace.TrainsTypes.FirstOrDefault();
                draft.Operator = document.Operators.FirstOrDefault();
                draft.Track = draft.TrackDeparture = document.Tracks.FirstOrDefault();
            }

            if (train is not null)
                draft.LoadSiblings(trains, row);
            draft.RenameSiblings = renameVariants;
            Apply(draft, context, number, type, name, @operator, arrival, departure, track, departureTrack, lineArrival, lineDeparture, validity,
                validFrom, validTo, stationsBefore, stationsAfter, languages, flags, lockout);

            var problems = TrainRules.Check(draft, new TrainContext(trains, row, gvd.ThisStation?.ID));
            if (problems.Where(p => !p.IsWarning).Select(p => p.Message).ToList() is { Count: > 0 } errors)
                throw new McpException("train.invalid: " + string.Join(" ", errors));
            warnings.AddRange(problems.Where(p => p.IsWarning).Select(p => p.Message));

            var saved = TrainEditing.Save(draft, train, row, document);
            if (train is null)
                trains.Add(saved);
            // novy vlak, kopia alebo zmena cisla, nazvu ci typu - cisla variant prideli GVDEditor
            TrainVariants.Normalize(trains);
            TrainEditing.Renumber(trains);
            return TrainTools.Detail(saved);
        }).ConfigureAwait(false);
        return new TrainChangeResult(change, detail, warnings);
    }

    [McpServerTool(Name = "delete_train", Title = "Delete train", ReadOnly = false, Destructive = true, Idempotent = false, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Deletes a train with its board texts. The ids of the following trains go down by one; a variant left alone becomes the only one.")]
    public static async Task<TrainChangeResult> DeleteTrain(
        EditorAccess editor,
        [Description("Train id from list_trains.")] int id,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var (_, change) = await editor.WriteAsync(
            context => string.Format(CultureInfo.CurrentCulture, Resources.History_DeleteTrain,
                context.Document.Trains.FirstOrDefault(t => t.ID == id) is { } train ? TrainRules.Label(train) : id),
            dryRun, context =>
            {
                var document = context.Document;
                TrainEditing.Remove(TrainTools.Find(document, id), document);
                // varianta, ktora ostala sama, dostane -1
                TrainVariants.Normalize(document.Trains);
                TrainEditing.Renumber(document.Trains);
                return true;
            }).ConfigureAwait(false);
        return new TrainChangeResult(change, null, []);
    }

    // ---------------------------------------------------------------- pomocne

    /// <summary>Zadane hodnoty do konceptu - stanice, typy a kolaje podla zoznamov grafikonu.</summary>
    private static void Apply(TrainDraft draft, EditorContext context, string? number, string? type, string? name, string? @operator, string? arrival,
        string? departure, string? track, string? departureTrack, string? lineArrival, string? lineDeparture, string? validity, DateOnly? validFrom,
        DateOnly? validTo, IReadOnlyList<RouteStopInput>? stationsBefore, IReadOnlyList<RouteStopInput>? stationsAfter, IReadOnlyList<string>? languages,
        IReadOnlyList<string>? flags, int? lockout)
    {
        var document = context.Document;
        if (number is not null)
            draft.Number = number;
        if (type is not null)
            draft.Type = TableMapper.Option(context.Workspace.TrainsTypes, t => t.Key, type, "train type");
        // nazov z get_train je ulozena hodnota - ta ostane; iny text sa zapise ako v okne (nazov zo zvukovej banky ako jej kluc)
        if (name is not null && name != draft.Name)
            draft.Name = TrainName.ToStored(context.Workspace.TrainNames, name);
        if (@operator is not null)
            draft.Operator = document.Operators.FirstOrDefault(o => o.Id.ToString(CultureInfo.InvariantCulture) == @operator)
                             ?? TableMapper.Option(document.Operators, o => o.Name, @operator, "operator");
        if (arrival is not null)
            draft.ArrivalText = Time(arrival);
        if (departure is not null)
            draft.DepartureText = Time(departure);
        if (track is not null)
        {
            // odchod z tej istej kolaje ide s prichodom - ako na stranke Trasa
            var previous = draft.Track;
            draft.Track = TableMapper.Option(document.Tracks, t => t.Key, track, "track");
            if (previous is null || draft.TrackDeparture is null || draft.TrackDeparture.EqualsKeys(previous))
                draft.TrackDeparture = draft.Track;
        }

        if (departureTrack is not null)
            draft.TrackDeparture = TableMapper.Option(document.Tracks, t => t.Key, departureTrack, "track");
        if (lineArrival is not null)
            draft.LineArrival = lineArrival;
        if (lineDeparture is not null)
            draft.LineDeparture = lineDeparture;
        if (validity is not null)
            draft.DateLimitText = validity;
        if (validFrom is { } from)
            draft.ValidFrom = from;
        if (validTo is { } to)
            draft.ValidTo = to;
        if (lockout is { } lockoutNumber)
            draft.LockoutNumber = lockoutNumber;

        if (stationsBefore is not null)
            Route(draft.RouteFrom, stationsBefore, context);
        if (stationsAfter is not null)
            Route(draft.RouteTo, stationsAfter, context);

        if (languages is not null)
        {
            var offered = GrafikonLanguageRules.Offered(context.Workspace.Languages, document.LocalLanguages, draft.Languages)
                .Where(l => !l.IsBasic).ToList();
            var chosen = languages.Select(key => offered.FirstOrDefault(l => string.Equals(l.Key, key, StringComparison.OrdinalIgnoreCase))
                                                 ?? TableMapper.Option(offered, l => l.Name, key, "language")).Distinct().ToList();
            draft.Languages.Clear();
            draft.Languages.AddRange(chosen);
        }

        if (flags is not null)
        {
            var set = flags.Select(f => TableMapper.Option(TrainTools.FlagFields, field => field.Name, f, "flag").Name).ToHashSet();
            // priznaky sa zapisu cez docasny vlak - koncept ich ma ako samostatne vlastnosti
            var values = new Train();
            foreach (var field in TrainTools.FlagFields)
                field.Set(values, set.Contains(field.Name));
            draft.IsMedzistatny = values.IsMedzistatny;
            draft.IsDialkovy = values.IsDialkovy;
            draft.IsMimoriadny = values.IsMimoriadny;
            draft.IsMiestenkovy = values.IsMiestenkovy;
            draft.IsIbaLozkovy = values.IsIbaLozkovy;
            draft.IsNizkopodlazny = values.IsNizkopodlazny;
            draft.IsPrestupovy = values.IsPrestupovy;
            draft.IsPriznakO = values.IsPriznakO;
            draft.IsMotorovy = values.IsMotorovy;
        }
    }

    /// <summary>Cas HH:mm (aj HH:mm:ss z get_train); iny text ostane - kontrola vlaku ho odmietne.</summary>
    private static string Time(string text) =>
        TimeOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time) ? time.ToString("HH:mm", CultureInfo.InvariantCulture) : text;

    /// <summary>Cast trasy zo zadanych stanic - nova stanica ako v okne (vlastna kopia s priznakmi hlasenia).</summary>
    private static void Route(List<Station> route, IReadOnlyList<RouteStopInput> stops, EditorContext context)
    {
        var stations = context.Workspace.Stations.Concat(context.Document.CustomStations).ToList();
        var result = stops.Select(stop =>
        {
            var found = stations.FirstOrDefault(s => s.ID == stop.Station);
            if (found is null)
            {
                var named = stations.Where(s => string.Equals(s.Name, stop.Station, StringComparison.OrdinalIgnoreCase)).DistinctBy(s => s.ID).ToList();
                found = named switch
                {
                    [var single] => single,
                    [] => throw new McpException($"station.notFound: Station '{stop.Station}' is neither in the sound bank nor among the stations of the timetable."),
                    _ => throw new McpException($"station.ambiguous: More stations are named '{stop.Station}': "
                                                + string.Join(", ", named.Select(s => s.ID)) + "; give the number.")
                };
            }

            return new Station(found.ID, found.Name, stop.Short, stop.Long);
        }).ToList();
        route.Clear();
        route.AddRange(result);
    }

    /// <summary>Popis kroku spat podla vlaku pred zmenou.</summary>
    private static string Label(GrafikonDocument document, int? id, int? copyFrom, string? type, string? number, string? name)
    {
        var train = document.Trains.FirstOrDefault(t => t.ID == (id ?? copyFrom));
        return id is not null
            ? string.Format(CultureInfo.CurrentCulture, Resources.History_EditTrain, train is null ? id : TrainRules.Label(train))
            : copyFrom is not null
                ? string.Format(CultureInfo.CurrentCulture, Resources.History_CopyTrain, train is null ? copyFrom : TrainRules.Label(train))
                : string.Format(CultureInfo.CurrentCulture, Resources.History_NewTrain, string.Join(" ", new[] { type, number, name }.Where(s => !string.IsNullOrEmpty(s))));
    }
}
