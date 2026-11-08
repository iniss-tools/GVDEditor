using System.Globalization;
using ToolsCore.Iniss.StateDgm;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Prevod stavoveho diagramu INISS 3.39 (StateDgm.txt) na statedgm.xml INISS2. Kategoria je graf, stav je uzol v rovnakom
/// poradi. Tlacidlo stavu je ovladac 3.39, ktoreho akcia do stavu vedie (poradie CtrlID), hlasenie je typ hlasenia tychto akcii.
/// Automatika stavu ostava automatikou uzla; cas je prichod alebo odchod s meskanim ako v 3.39, cakanie na udalost ILTISu
/// je podmienka automatiky. Dialogy kolaje a meskania su pomocne uzly <c>Kolej</c> a <c>Zpozdeni</c>, starter je pomocny uzol
/// s opakovanou automatikou, ktora plati len v stavoch so starterom. Tlacidla v INISS2 su dostupne v kazdom stave.
/// </summary>
/// <param name="transpiler">preklad vyrazov</param>
/// <param name="reportName">nazov hlasenia INISS2 pre typ hlasenia 3.39; null = hlasenie v cieli nie je</param>
internal sealed class Iniss2StateDgmConverter(Iniss2ExpressionTranspiler transpiler, Func<string, string?> reportName)
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    // pomocne uzly, ktorych tlacidlo otvori okno klienta - server ich pozna podla kluca
    private const string TrackNode = "Kolej";
    private const string DelayNode = "Zpozdeni";
    private const string TrackDialog = "SDDlgKolej";
    private static readonly string[] DelayDialogs = ["SDDlgZpozdeni", "SDDlgZpozdeniG"];

    // INISS 3.39 spusti starter este 10 s po termine
    private const int StarterTolerance = 10;

    // triedy akcii, ktorych spravanie INISS2 vyjadri prechodom do stavu alebo dialogom
    private static readonly string[] PlainEvents = ["SDEventUniPos", "SDEventChangeState", "SDEventSelectState", "SDEventWithDialog"];

    private static readonly (StateDgmAttr Bit, string Flag)[] AttrFlags =
    [
        (StateDgmAttr.Stoji, StateNodeFlags.Standing),
        (StateDgmAttr.PotvrzenaKolej, StateNodeFlags.TrackConfirmed),
        (StateDgmAttr.Odbaven, StateNodeFlags.Dispatched),
        (StateDgmAttr.Shadow, StateNodeFlags.Shadow),
        (StateDgmAttr.NyniStoji, StateNodeFlags.StandingNow)
    ];

    // zabudovane casove body: pravidelny cas bez meskania, predpokladany s meskanim
    private static readonly Dictionary<string, string> BuiltInTimePoints = new(StringComparer.Ordinal)
    {
        ["#Pravidelný příjezd"] = ".DatumCasPrij(False)",
        ["#Pravidelný odjezd"] = ".DatumCasOdj(False)",
        ["#Předpokládaný příjezd"] = ".DatumCasPrij(True)",
        ["#Předpokládaný odjezd"] = ".DatumCasOdj(True)"
    };

    private readonly List<Iniss2ExportWarning> _warnings = [];
    private StateDgmDiagram _diagram = new();

    /// <summary>
    /// Co sa nedalo preniest presne.
    /// </summary>
    public IReadOnlyList<Iniss2ExportWarning> Warnings => _warnings;

    /// <summary>
    /// Prevedie diagram.
    /// </summary>
    /// <param name="diagram">stavovy diagram 3.39</param>
    public StateDiagram Convert(StateDgmDiagram diagram)
    {
        ArgumentNullException.ThrowIfNull(diagram);
        _diagram = diagram;

        return new StateDiagram(GraphIndex(diagram)) { Graphs = diagram.Categories.Select(Graph).ToList() };
    }

    // cislo kategorie mimo 1 az pocet kategorii INISS 3.39 nahradi poslednou
    private string GraphIndex(StateDgmDiagram diagram)
    {
        var count = diagram.Categories.Count;
        var text = diagram.EffectiveIndCat.Trim();
        var code = Number(text, StateDgmKeys.IndCat);
        if (code is null)
            return Literal(count);

        var known = text.ToUpperInvariant() switch
        {
            "INDCAT6" => count >= 6,
            "INDCAT8" => count >= 8,
            _ => int.TryParse(text, NumberStyles.Integer, Inv, out var value) && value >= 1 && value <= count
        };

        return known ? code : $"Dim c As Integer = {code} : Return If(c >= 1 AndAlso c <= {Literal(count)}, c, {Literal(count)})";
    }

    private StateGraph Graph(StateDgmCategory category)
    {
        var states = category.States;
        var nodes = new List<StateNode>();
        var keys = new HashSet<string>(states.Select(state => state.Key), StringComparer.Ordinal);

        CheckEvents(category);

        // dialogy kolaje a meskania: tlacidlo podla prveho ovladaca s dialogom
        foreach (var (key, dialogs) in new[] { (TrackNode, new[] { TrackDialog }), (DelayNode, DelayDialogs) })
        {
            var control = Controls(category).FirstOrDefault(pair => pair.Event?.Dialog is { } dialog && dialogs.Contains(dialog, StringComparer.Ordinal));
            if (control.Control is not null && keys.Add(key))
            {
                nodes.Add(new StateNode(key)
                {
                    Flags = [StateNodeFlags.Auxiliary],
                    Button = new StateButton(ButtonText(control.Control.DesignKey, key), control.Control.CtrlId)
                });
            }
        }

        nodes.AddRange(Starters(category, keys));

        StateDgmTableSet? tables = null;
        foreach (var state in states)
        {
            tables = state.DoState ?? tables;
            nodes.Add(Node(category, state, tables));
        }

        return new StateGraph(category.Key) { Nodes = Positions(nodes) };
    }

    private StateNode Node(StateDgmCategory category, StateDgmState state, StateDgmTableSet? tables)
    {
        var where = Where(category, state);
        var incoming = Incoming(category, state.Key);
        var control = incoming.FirstOrDefault(pair => pair.Control is not null).Control;

        if (state.UndoState is not null)
            Warn(Iniss2ExportIssue.StatePartNotConverted, where + StateDgmKeys.UndoState);

        return new StateNode(state.Key)
        {
            Name = state.Name,
            Report = Report(where, incoming.Select(pair => pair.Event.ReportKey ?? "").ToList()),
            Flags = Flags(state.Attr, tables),
            Button = control is null ? null : new StateButton(ButtonText(control.DesignKey, state.Name.Length > 0 ? state.Name : state.Key), control.CtrlId),
            Automation = incoming.Count > 0 ? Automation(state, where) : null
        };
    }

    // akcie kategorie, ktore vedu do stavu; najprv tie s ovladacom (tlacidlo urcuje hlasenie), potom ostatne v poradi stavov
    private static List<(StateDgmEvent Event, StateDgmControl? Control)> Incoming(StateDgmCategory category, string key)
    {
        var events = category.States.SelectMany(state => state.Events.Where(e => e.NextState == key)
            .Select(e => (Event: e, Control: state.Controls.FirstOrDefault(control => control.EventKey == e.Key)))).ToList();
        return [.. events.Where(pair => pair.Control is not null), .. events.Where(pair => pair.Control is null)];
    }

    private static IEnumerable<(StateDgmControl? Control, StateDgmEvent? Event)> Controls(StateDgmCategory category) =>
        category.States.SelectMany(state => state.Controls.Select(control => ((StateDgmControl?)control, state.FindEvent(control.EventKey))));

    private string Report(string where, List<string> keys)
    {
        var distinct = keys.Distinct(StringComparer.Ordinal).ToList();
        if (distinct.Count > 1)
            Warn(Iniss2ExportIssue.StateReportConflict, where + string.Join(", ", distinct));

        return distinct.Count == 0 ? "" : ReportName(distinct[0]);
    }

    private string ReportName(string key)
    {
        if (key.Length == 0)
            return "";

        if (reportName(key) is { } name)
            return name;

        Warn(Iniss2ExportIssue.StateReportMissing, key);
        return "";
    }

    private static List<string> Flags(StateDgmAttr attr, StateDgmTableSet? tables)
    {
        var flags = AttrFlags.Where(pair => attr.HasFlag(pair.Bit)).Select(pair => pair.Flag).ToList();
        if (tables is null)
            return flags;

        if (tables.OnDepartureTable)
            flags.Add(StateNodeFlags.OnDeparture);
        if (tables.OnArrivalTable)
            flags.Add(StateNodeFlags.OnArrival);
        if (tables.OnPlatformTables)
            flags.Add(StateNodeFlags.OnPlatform);
        if (tables.ShowPosition)
            flags.Add(StateNodeFlags.ShowPlatform);
        if (tables.ShowTrack)
            flags.Add(StateNodeFlags.ShowTrack);
        return flags;
    }

    // automatika: rezim, cas prichodu alebo odchodu s meskanim a posunom, podmienka a cakanie na udalost ILTISu
    private StateAutomation? Automation(StateDgmState state, string where)
    {
        if (!state.HasAutomation || state.AutoTimePoint is not { } point)
            return null;

        var mode = state.AutoMode!.IsExpression
            ? Number(state.AutoMode.Expression!, where + StateDgmKeys.AutoMode) is { } modeCode ? $"Dim m As Integer = {modeCode} : Return CByte(If(m >= 0 AndAlso m <= 2, m, 0))" : null
            : Literal(state.AutoMode.Number!.Value);
        if (mode is null)
            return null;

        string time;
        if (point.IsExpression)
        {
            if (Number(point.Expression!, where + StateDgmKeys.AutoTimePoint) is not { } pointCode)
                return null;
            time = $"If({pointCode} = 2, .DatumCasOdj(True), .DatumCasPrij(True))";
        }
        else
        {
            time = point.Number == (int)StateDgmAutoTimePoint.Departure ? ".DatumCasOdj(True)" : ".DatumCasPrij(True)";
        }

        if (state.AutoTimePointAdd is { } add)
        {
            var seconds = add.IsExpression ? Number(add.Expression!, where + StateDgmKeys.AutoTimePointAdd) : add.Number == 0 ? null : add.Number!.Value.ToString(Inv);
            if (seconds is not null)
                time += $".AddSeconds({seconds})";
        }

        var conditions = new List<string>();
        if (!string.IsNullOrWhiteSpace(state.AutoCondition) && Condition(state.AutoCondition, where + StateDgmKeys.AutoCondition) is { } condition)
            conditions.Add(condition);
        if (state.Wait is { } wait && Wait(wait.Text, where) is { } events)
            conditions.Add(events);
        else if (state.WaitPath is { } path && (path.IsExpression || path.Number != 0))
            conditions.Add(Iniss2ExpressionTranspiler.WaitPath);

        int? variant = null;
        if (state.AutoModif is { } modif)
        {
            if (modif.IsExpression)
                Warn(Iniss2ExportIssue.StatePartNotConverted, where + StateDgmKeys.AutoModif);
            else if (modif.Number is 1 or 2)
                variant = modif.Number;
        }

        return new StateAutomation(time)
        {
            Mode = mode,
            Condition = string.Join(" AndAlso ", conditions.Select(Parenthesize)),
            Variant = variant,
            Flags = [StateNodeFlags.KeepWithoutClient]
        };
    }

    // startery s rovnakym nastavenim su jeden pomocny uzol; automatika plati, ked je vlak v niektorom zo stavov so starterom
    private List<StateNode> Starters(StateDgmCategory category, HashSet<string> keys)
    {
        var groups = new List<(StarterKey Key, List<string> States)>();
        foreach (var state in category.States)
        {
            foreach (var starter in state.Starters)
            {
                var where = Where(category, state) + StateDgmKeys.Starter + " " + starter.Key;
                if (state.FindEvent(starter.EventKey) is not { } target)
                {
                    Warn(Iniss2ExportIssue.StatePartNotConverted, where + " → " + starter.EventKey);
                    continue;
                }

                if (target.ChangesState || target.Dialog is { Length: > 0 })
                {
                    Warn(Iniss2ExportIssue.StatePartNotConverted, where + " → " + target.Key);
                    continue;
                }

                var key = new StarterKey(starter.Key, starter.TimePointKey, starter.TimeOffset, starter.TimeOffsetStep ?? 0,
                    starter.TimePointKeyLast ?? starter.TimePointKey, starter.TimeOffsetLast ?? 0, starter.StartLaterToo, target.ReportKey ?? "");
                var index = groups.FindIndex(group => group.Key == key);
                if (index < 0)
                    groups.Add((key, [state.Key]));
                else if (!groups[index].States.Contains(state.Key))
                    groups[index].States.Add(state.Key);
            }
        }

        var nodes = new List<StateNode>();
        foreach (var (key, states) in groups)
        {
            var where = $"{category.Key}/{key.Name}: ";
            var first = TimePoint(key.TimePoint, where);
            var last = TimePoint(key.LastTimePoint, where);
            var report = ReportName(key.Report);
            if (first is null || last is null || report.Length == 0)
                continue;

            var start = Shift(first, key.Offset);
            var time = key.Step > 0 && !key.StartLaterToo
                ? $"Dim f As DateTime = {start} : Return If(Now < f.AddSeconds({StarterTolerance}), f, f.AddSeconds(Math.Ceiling(((Now - f).TotalSeconds - {StarterTolerance}) / {Literal(key.Step)}) * {Literal(key.Step)}))"
                : key.StartLaterToo
                    ? start
                    : $"Dim f As DateTime = {start} : Return If(Now < f.AddSeconds({StarterTolerance}), f, DateTime.MaxValue)";

            var name = key.Name;
            for (var i = 2; !keys.Add(name); i++)
                name = key.Name + " " + i.ToString(Inv);

            nodes.Add(new StateNode(name)
            {
                Report = report,
                Flags = [StateNodeFlags.Auxiliary],
                Condition = string.Join(" OrElse ", states.Select(state => $"(.Node.sKey = {Quote(state)})")),
                Automation = new StateAutomation(time)
                {
                    Mode = Literal((int)StateDgmAutoMode.Automatic),
                    Repeat = key.Step > 0 ? Literal(key.Step) : "",
                    End = key.Step > 0 ? Shift(last, key.LastOffset + StarterTolerance) : "",
                    Flags = [StateNodeFlags.KeepWithoutClient]
                }
            });
        }

        return nodes;
    }

    // casovy bod: zabudovany alebo vlastny z hlavicky (min/max dvoch posunutych bodov); cas vstupu do stavu INISS2 nema
    private string? TimePoint(string key, string where, int depth = 0)
    {
        if (BuiltInTimePoints.TryGetValue(key, out var builtIn))
            return builtIn;

        var point = _diagram.TimePoints.FirstOrDefault(tp => tp.Key == key);
        if (point is null || point.TimePointKey1.Length == 0 || point.TimePointKey2.Length == 0 || depth > _diagram.TimePoints.Count)
        {
            Warn(Iniss2ExportIssue.StatePartNotConverted, where + StateDgmKeys.TimePointKey + " " + key);
            return null;
        }

        var first = TimePoint(point.TimePointKey1, where, depth + 1);
        var second = TimePoint(point.TimePointKey2, where, depth + 1);
        if (first is null || second is null)
            return null;

        var a = Shift(first, point.Offset1);
        var b = Shift(second, point.Offset2);
        var choose = point.Operator == StateDgmKeys.OperatorMax ? ">=" : "<=";
        return $"If({a} {choose} {b}, {a}, {b})";
    }

    private static string Shift(string time, int seconds) => seconds == 0 ? time : $"{time}.AddSeconds({seconds.ToString(Inv)})";

    // akcie, ktorych spravanie INISS2 nema: tlacidlo len s hlasenim, zmena meskania, skrytie vlaku, vnorene akcie
    private void CheckEvents(StateDgmCategory category)
    {
        var reported = new HashSet<string>(StringComparer.Ordinal);
        foreach (var state in category.States)
        {
            foreach (var e in state.Events)
            {
                var plain = PlainEvents.Contains(e.Class, StringComparer.Ordinal) && e.HideShow is null or 0 && e.DelayArrival is null or 0
                            && e.DelayDeparture is null or 0 && e.DoEvent is null && e.UndoEvent is null && e.AskBeforeReport is null or 0;
                var buttonReportOnly = !e.ChangesState && e.Dialog is null or "" && state.Controls.Any(control => control.EventKey == e.Key);
                if ((!plain || buttonReportOnly) && reported.Add(e.Key))
                    Warn(Iniss2ExportIssue.StatePartNotConverted, Where(category, state) + StateDgmKeys.Event + " " + e.Key);
            }
        }
    }

    private string? Number(string text, string where) => Translate($"{where} = {text}", () => transpiler.Number(text));

    private string? Condition(string text, string where) => Translate($"{where} = {text}", () => transpiler.Condition(text));

    private string? Wait(string text, string where) => Translate($"{where}{StateDgmKeys.Wait} = {text}", () => transpiler.Wait(text));

    // problemy prekladu sa hlasia s miestom v diagrame
    private string? Translate(string detail, Func<string?> translate)
    {
        var before = transpiler.Problems.Count;
        var result = translate();
        if (transpiler.Problems.Count > before)
            Warn(Iniss2ExportIssue.StateExpressionNotConverted, detail);
        return result;
    }

    // tlacidla musia ist v poradi uzlov vzostupne, inak klient hlasi chybu - vtedy sa ocisluju znova
    private static List<StateNode> Positions(List<StateNode> nodes)
    {
        var buttons = nodes.Where(node => node.Button is not null).Select(node => node.Button!.Position).ToList();
        if (buttons.Zip(buttons.Skip(1)).All(pair => pair.First < pair.Second))
            return nodes;

        var position = 0;
        return nodes.Select(node => node.Button is { } button ? node with { Button = button with { Position = position++ } } : node).ToList();
    }

    // text tlacidla z kluca vzhladu bez pripony stavu tlacidla (KoľajN, Príde3, OdídeD)
    private static string ButtonText(string design, string fallback)
    {
        if (design.Length == 0)
            return fallback;

        return design.Length > 1 && design[^1] is 'N' or 'D' or '1' or '2' or '3' && char.IsLower(design[^2]) ? design[..^1] : design;
    }

    private static string Where(StateDgmCategory category, StateDgmState state) => $"{category.Key}/{state.Key}: ";

    private static string Parenthesize(string code) => "(" + code + ")";

    private static string Quote(string text) => "\"" + text.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

    private static string Literal(int value) => value < 0 ? "(" + value.ToString(Inv) + ")" : value.ToString(Inv);

    private void Warn(Iniss2ExportIssue issue, string detail)
    {
        var warning = new Iniss2ExportWarning(issue, detail);
        if (!_warnings.Contains(warning))
            _warnings.Add(warning);
    }

    private sealed record StarterKey(string Name, string TimePoint, int Offset, int Step, string LastTimePoint, int LastOffset, bool StartLaterToo, string Report);
}
