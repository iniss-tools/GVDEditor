using System.Collections;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using Iniss.PlanRadenia;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Druh vozna, ako sa hlasi v radeni.
/// </summary>
internal enum CarKind
{
    /// <summary>Druh sa nepodarilo urcit.</summary>
    Unknown,
    First,
    Second,

    /// <summary>Vozen s oddielmi prvej aj druhej triedy.</summary>
    FirstSecond,
    Sleeper,
    Couchette,
    Restaurant,
    Bistro,

    /// <summary>Autovozen - cestujuci v nom necestuju, nehlasi sa.</summary>
    CarCarrier
}

/// <summary>
/// Co treba pri navrhu radenia skontrolovat.
/// </summary>
internal enum PlanRadeniaIssueKind
{
    /// <summary>Zvuk chyba v zvukovej banke (skupina/kluc v <see cref="PlanRadeniaIssue.Text" />).</summary>
    MissingSound,

    /// <summary>Druh vozna sa nepodarilo urcit (cislo vozna).</summary>
    UnknownKind,

    /// <summary>Banka nema bistro vozen - hlasi sa ako restauracny.</summary>
    BistroAsRestaurant,

    /// <summary>Banka nema vozen prvej a druhej triedy - hlasi sa ako vozen druhej triedy.</summary>
    MixedAsSecond,

    /// <summary>Cielova stanica vozni z poznamky nie je medzi stanicami grafikonu (nazov z planu).</summary>
    UnknownStation,

    /// <summary>Dni z poznamky sa nepodarilo spracovat (text poznamky).</summary>
    BadNote,

    /// <summary>Poznamka sa precitala nespolahlivo (text poznamky).</summary>
    UncertainNote,

    /// <summary>Vozne sa do vlaku pridavaju v inej stanici (nazov stanice).</summary>
    JoinedCars,

    /// <summary>Na konci prveho riadku radenia nie je rusen - vozne su zoradene zlava.</summary>
    NoLocomotive,

    /// <summary>Cislo vozna v poznamke opravene (povodne a opravene cislo oddelene medzerou).</summary>
    CarFixed,

    /// <summary>Vozen z poznamky v radeni nie je (cislo vozna).</summary>
    CarMissing
}

/// <summary>
/// Upozornenie k navrhu radenia.
/// </summary>
internal sealed record PlanRadeniaIssue(PlanRadeniaIssueKind Kind, string Text = "");

/// <summary>
/// Navrh radenia vlaku z planu radenia - jeden variant (dni, v ktore idu rovnake vozne).
/// </summary>
internal sealed class RadenieProposal
{
    public required string TrainNumber { get; init; }

    /// <summary>Vlak v plane radenia.</summary>
    public required PlanTrain Plan { get; init; }

    public required Radenie Radenie { get; init; }

    public List<PlanRadeniaIssue> Issues { get; } = [];

    /// <summary>Ci sa radenie prevezme do grafikonu.</summary>
    public bool Take { get; set; } = true;

    /// <summary>Ci v hlaseni chyba niektory zvuk banky.</summary>
    public bool HasMissingSounds => Issues.Any(i => i.Kind == PlanRadeniaIssueKind.MissingSound);
}

/// <summary>
/// Vstup prevodu, ktory nezavisi od vlaku.
/// </summary>
internal sealed class PlanRadeniaContext
{
    /// <summary>Zvuky zakladneho jazyka banky.</summary>
    public required IReadOnlyList<FyzSound> Sounds { get; init; }

    /// <summary>Obdobie grafikonu - k nemu sa vztahuju dni poznamok aj radenia.</summary>
    public required DateOnly From { get; init; }

    public required DateOnly To { get; init; }

    /// <summary>Stanica grafikonu podla nazvu z planu radenia, alebo <see langword="null" />.</summary>
    public required Func<string, Station?> Station { get; init; }

    /// <summary>V ktorych hlaseniach sa radenie hlasi (pre vlak bez vlastneho radenia).</summary>
    public required List<ChosenReportType> Reports { get; init; }
}

/// <summary>
/// Prevod radenia vlaku z planu radenia ZSSK na radenie INISS: hlasenie zo zvukov banky (ako
/// <c>Za rušňom sú radené: vozeň druhej triedy číslo 381, vozne druhej triedy číslo 369 až 367 a ležadlový vozeň číslo 363.</c>)
/// a dni platnosti podla poznamok pod radenim - vozne, ktore nejdu kazdy den, rozdelia radenie na varianty.
/// </summary>
internal static partial class PlanRadeniaConversion
{
    /// <summary>Priemerna odchylka citania poznamky, od ktorej je poznamka neista.</summary>
    private const double UncertainNote = 0.35;

    // skupiny zvukov banky; poradie: bez interpunkcie, s ciarkou, s bodkou
    private static readonly string[] KindSingle = ["Vozy1", "Vozy2", "Vozy3"];
    private static readonly string[] KindSingleAnd = ["Vozy4", "Vozy5", "Vozy6"];
    private static readonly string[] Number = ["CISLO1", "CISLO_", "CISLO3"];
    private static readonly string[] NumberAnd = ["CISLO4", "CISLO5", "CISLO6"];
    private static readonly string[] NumberUntil = ["CISLO7", "CISLO8", "CISLO9"];
    private static readonly string[] StationName = ["R3", "R1", "R4"];
    private const string Words = "SLOVA";
    private const string Position = "Poz7";

    /// <summary>Interpunkcia za prvkom hlasenia - vyberie skupinu zvuku.</summary>
    private enum Ending
    {
        /// <summary>Nasleduje posledna skupina so spojkou „a“ alebo dalsi prvok skupiny.</summary>
        Plain,
        Comma,
        Period
    }

    /// <summary>Druh vozna podla piktogramov (a farby, ak piktogram triedy chyba).</summary>
    public static CarKind KindOf(PlanVehicle vehicle)
    {
        var icons = vehicle.Icons;
        bool Has(string text) => icons.Any(icon => icon.Contains(text, StringComparison.Ordinal));
        if (Has("lôžkový vozeň"))
            return CarKind.Sleeper;
        if (Has("ležadlový vozeň"))
            return CarKind.Couchette;
        if (Has("reštauračný vozeň"))
            return CarKind.Restaurant;
        if (Has("bistro vozeň"))
            return CarKind.Bistro;
        if (Has("autovozeň"))
            return CarKind.CarCarrier;
        bool first = Has("1. triedy"), second = Has("2. triedy");
        if (first && second)
            return CarKind.FirstSecond;
        if (first)
            return CarKind.First;
        if (second)
            return CarKind.Second;

        return vehicle.Colors.OrderByDescending(c => c.Share).FirstOrDefault()?.Name switch
        {
            "yellow" => CarKind.First,
            "green" => CarKind.Second,
            "red" => CarKind.Restaurant,
            "navy" => CarKind.Sleeper,
            "lightblue" => CarKind.Couchette,
            "gray" => CarKind.CarCarrier,
            _ => CarKind.Unknown
        };
    }

    /// <summary>Vozen v radeni s dnami, v ktore ide.</summary>
    private sealed class Car
    {
        public required string Number { get; init; }
        public required CarKind Kind { get; init; }

        /// <summary>Ciel vozna, ak ide inam nez vlak.</summary>
        public Station? Destination { get; set; }

        public required BitArray Days { get; init; }
    }

    /// <summary>
    /// Navrhne radenia vlaku: po jednom na kazdu skupinu dni, v ktore idu rovnake vozne.
    /// </summary>
    /// <param name="plan">Vlak v plane radenia.</param>
    /// <param name="trains">Varianty vlaku grafikonu s tymto cislom (dni jazdy, cielova stanica, hlasenia radenia).</param>
    /// <param name="planFrom">Platnost planu radenia od.</param>
    /// <param name="planTo">Platnost planu radenia do.</param>
    /// <param name="context">Banka, obdobie grafikonu a stanice.</param>
    public static List<RadenieProposal> Propose(PlanTrain plan, IReadOnlyList<Train> trains, DateOnly planFrom, DateOnly planTo, PlanRadeniaContext context)
    {
        var from = planFrom > context.From ? planFrom : context.From;
        var to = planTo < context.To ? planTo : context.To;
        if (to < from || trains.Count == 0)
            return [];

        var limit = new DateLimit(context.From, context.To, insertMarks: false);
        var issues = new List<PlanRadeniaIssue>();
        issues.AddRange(plan.Warnings.Concat(plan.Notes.SelectMany(n => n.Warnings)).Select(ReaderIssue).OfType<PlanRadeniaIssue>());

        // dni v obdobi radenia, v ktore ide niektory variant vlaku
        var span = to.DayNumber - from.DayNumber + 1;
        var trainDays = new BitArray(span);
        foreach (var train in trains)
            foreach (var day in TrainMerging.RunningDays(train))
                if (day >= from && day <= to)
                    trainDays[day.DayNumber - from.DayNumber] = true;

        var cars = new List<Car>();
        foreach (var vehicle in plan.Vehicles.Where(v => !v.IsLocomotive))
        {
            var kind = KindOf(vehicle);
            if (kind == CarKind.CarCarrier)
                continue;
            if (kind == CarKind.Unknown)
                issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.UnknownKind, vehicle.Number));
            cars.Add(new Car { Number = vehicle.Number, Kind = kind, Days = new BitArray(trainDays) });
        }

        ApplyNotes(plan, cars, limit, context, from, issues);

        var ending = trains.Select(t => t.EndingStation).FirstOrDefault(s => s is not null && s.ID.Length > 0);
        var reports = trains.SelectMany(t => t.Radenia).Select(r => r.ChosenReports).FirstOrDefault(r => r.Count > 0) ?? context.Reports;

        // varianty: dni s rovnakymi voznami
        var variants = new Dictionary<string, (List<Car> Cars, BitArray Days)>();
        for (var d = 0; d < span; d++)
        {
            if (!trainDays[d])
                continue;
            var running = cars.Where(c => c.Days[d]).ToList();
            var key = string.Join(",", running.Select(c => cars.IndexOf(c)));
            if (!variants.TryGetValue(key, out var variant))
                variants[key] = variant = (running, new BitArray(span));
            variant.Days[d] = true;
        }

        var periodLimit = new DateLimit(from, to, insertMarks: false);
        var result = new List<RadenieProposal>();
        foreach (var (running, days) in variants.Values)
        {
            if (running.Count == 0)
                continue;
            var picker = new SoundPicker(context.Sounds);
            var sounds = Compose(running, ending, picker, out var variantIssues);
            var allDays = days.Cast<bool>().All(b => b) && from == context.From && to == context.To;
            var radenie = new Radenie
            {
                CisloVlaku = plan.Number,
                Validity = allDays ? null : new ValidityPeriod(from, to),
                DatObm = allDays || days.Cast<bool>().All(b => b) ? "" : periodLimit.BitArrayToText(days),
                Sounds = sounds,
                ChosenReports = reports.Select(r => new ChosenReportType { Type = r.Type, Variants = [.. r.Variants] }).ToList(),
                DestStation = null!
            };
            radenie.Text = Radenie.SoundsToString(sounds);
            var proposal = new RadenieProposal { TrainNumber = plan.Number, Plan = plan, Radenie = radenie };
            proposal.Issues.AddRange(issues);
            proposal.Issues.AddRange(variantIssues);
            proposal.Issues.AddRange(picker.Missing.Select(m => new PlanRadeniaIssue(PlanRadeniaIssueKind.MissingSound, m)));
            proposal.Take = !proposal.HasMissingSounds;
            result.Add(proposal);
        }

        return result;
    }

    /// <summary>Upozornenie citacky planu (kod a udaje) ako upozornenie navrhu.</summary>
    private static PlanRadeniaIssue? ReaderIssue(string warning)
    {
        var parts = warning.Split(' ', 2);
        var data = parts.Length > 1 ? parts[1] : "";
        return parts[0] switch
        {
            PlanWarnings.NoLocomotive => new PlanRadeniaIssue(PlanRadeniaIssueKind.NoLocomotive),
            PlanWarnings.CarFixed => new PlanRadeniaIssue(PlanRadeniaIssueKind.CarFixed, data),
            PlanWarnings.CarMissing => new PlanRadeniaIssue(PlanRadeniaIssueKind.CarMissing, data),
            _ => null
        };
    }

    /// <summary>
    /// Pouzije poznamky pod radenim: dni jazdy vozni (obmedzenie, pridanie v stanici) a ciel vozni.
    /// </summary>
    private static void ApplyNotes(PlanTrain plan, List<Car> cars, DateLimit limit, PlanRadeniaContext context, DateOnly from, List<PlanRadeniaIssue> issues)
    {
        foreach (var note in plan.Notes)
        {
            if (note.Score > UncertainNote)
                issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.UncertainNote, note.Text));

            var targets = note.Cars.Select(number => FindCar(cars, number)).OfType<Car>().ToList();
            switch (note.Kind)
            {
                case PlanNoteKind.Restriction or PlanNoteKind.Joined when note.Rest.Length > 0:
                {
                    BitArray bits;
                    try
                    {
                        bits = limit.TextToBitArray(DayText(note.Rest));
                    }
                    catch (Exception e) when (e is DateLimit.ParseException or ArgumentException or FormatException)
                    {
                        issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.BadNote, note.Text));
                        continue;
                    }

                    var offset = from.DayNumber - context.From.DayNumber;
                    foreach (var car in targets)
                        for (var d = 0; d < car.Days.Length; d++)
                            if (car.Days[d] && !bits[offset + d])
                                car.Days[d] = false;
                    if (note.Kind == PlanNoteKind.Joined)
                        issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.JoinedCars, note.Station));
                    break;
                }
                case PlanNoteKind.Joined:
                    issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.JoinedCars, note.Station));
                    break;
                case PlanNoteKind.Destination:
                {
                    var station = context.Station(note.Station);
                    if (station is null)
                        issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.UnknownStation, note.Station));
                    else
                        foreach (var car in targets)
                            car.Destination = station;
                    break;
                }
                default:
                    if (note.Text.Length > 0)
                        issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.BadNote, note.Text));
                    break;
            }
        }
    }

    /// <summary>Vozen podla cisla z poznamky; <c>#n</c> = n-ty vozen bez cisla.</summary>
    private static Car? FindCar(List<Car> cars, string number)
    {
        if (!number.StartsWith('#'))
            return cars.FirstOrDefault(c => c.Number == number);
        return int.TryParse(number.AsSpan(1), out var n) ? cars.Where(c => c.Number.Length == 0).ElementAtOrDefault(n - 1) : null;
    }

    /// <summary>Rozsah dni v tyzdni s medzerami (① - ④ po prevode na 1 - 4).</summary>
    [GeneratedRegex(@"\b([1-7X])\s*-\s*([1-7X])\b")]
    private static partial Regex DayRange();

    /// <summary>
    /// Text dni poznamky v tvare datumoveho obmedzenia GVDEditora: dni v tyzdni v kruzku na cislice, kladivka na X.
    /// </summary>
    internal static string DayText(string rest)
    {
        var text = new StringBuilder(rest.Length);
        foreach (var c in rest)
            text.Append(c switch
            {
                >= '①' and <= '⑦' => (char)('1' + (c - '①')),
                '⚒' => 'X',
                _ => c
            });
        return DayRange().Replace(text.ToString(), "$1-$2").Trim().TrimEnd(',');
    }

    /// <summary>
    /// Skupina vozni v hlaseni: vozne za sebou s rovnakou nahravkou druhu a cielom, bud vsetky s cislom, alebo
    /// vsetky bez neho.
    /// </summary>
    private sealed class Group(string key, Station? destination)
    {
        /// <summary>Kluc nahravky druhu vozna (v2, lev…).</summary>
        public string Key { get; } = key;

        public Station? Destination { get; } = destination;

        /// <summary>Cisla vozni; prazdne pri voznoch bez cisla.</summary>
        public List<string> Numbers { get; } = [];

        public int Count { get; set; }
    }

    /// <summary>Prvok hlasenia: skupiny zvukov podla interpunkcie (alebo jeden zvuk bez nej) a kluc zvuku.</summary>
    private sealed record Token(string[] Groups, string Key);

    /// <summary>
    /// Hlasenie radenia zo zvukov banky: poloha (za rusnom), je/su radene a skupiny vozni s cislami a cielom.
    /// </summary>
    private static List<FyzSound> Compose(List<Car> cars, Station? ending, SoundPicker picker, out List<PlanRadeniaIssue> issues)
    {
        issues = [];
        var groups = new List<Group>();
        foreach (var car in cars)
        {
            var key = KindKey(car.Kind, picker, issues);
            var numbered = car.Number.Length > 0;
            if (groups.Count == 0 || groups[^1].Key != key || !Same(groups[^1].Destination, car.Destination) ||
                numbered != (groups[^1].Numbers.Count > 0))
                groups.Add(new Group(key, car.Destination));
            groups[^1].Count++;
            if (numbered)
                groups[^1].Numbers.Add(car.Number);
        }

        // ciel sa hlasi, ak niektore vozne idu inam nez vlak; vozne bez poznamky idu do cielovej stanice vlaku
        var destinations = groups.Any(g => g.Destination is not null && !Same(g.Destination, ending));
        var tokens = new List<Token> { new([Position], "zalok"), new([Words], cars.Count == 1 ? "jeraz" : "jsouraz") };
        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var last = i == groups.Count - 1;
            var and = last && groups.Count > 1;
            var plural = group.Count > 1;
            var kindKey = group.Key;
            // vozne bez cisla: nahravka s poctom (dva vozne druhej triedy), ak ju banka ma
            if (plural && group.Numbers.Count == 0 && picker.Has(KindGroups(and, true)[0], group.Count + kindKey))
                kindKey = group.Count + kindKey;
            var groupTokens = new List<Token> { new(KindGroups(and, plural), kindKey) };

            if (group.Numbers.Count > 0)
            {
                groupTokens.Add(new Token([Words], "cislo"));
                groupTokens.AddRange(NumberTokens(group.Numbers));
            }

            var destination = group.Destination ?? ending;
            var nextDestination = last ? null : groups[i + 1].Destination ?? ending;
            if (destinations && destination is not null && (last || !Same(destination, nextDestination)))
            {
                groupTokens.Add(new Token([Words], "DoStan"));
                groupTokens.Add(new Token(StationName, destination.ID));
            }

            // interpunkcia za skupinou: bodka na konci, pred poslednou skupinou (so spojkou a) nic, inak ciarka;
            // prvky vnutri skupiny su bez interpunkcie
            var end = last ? Ending.Period : i == groups.Count - 2 ? Ending.Plain : Ending.Comma;
            for (var t = 0; t < groupTokens.Count; t++)
            {
                var token = groupTokens[t];
                tokens.Add(token with { Groups = [t == groupTokens.Count - 1 ? Pick(token.Groups, end) : token.Groups[0]] });
            }
        }

        var sounds = new List<FyzSound>();
        foreach (var token in tokens)
            if (picker.Get(token.Groups[0], token.Key) is { } sound)
                sounds.Add(sound);
        return sounds;
    }

    private static string Pick(string[] groups, Ending ending) => groups.Length == 1 ? groups[0] : groups[(int)ending];

    private static string[] KindGroups(bool and, bool plural)
    {
        var names = and ? KindSingleAnd : KindSingle;
        return plural ? names.Select(n => n + "M").ToArray() : names;
    }

    /// <summary>
    /// Cisla vozni skupiny: jedno cislo, dve (N a M), rad po sebe iducich (N az M), inak zoznam (N, M a K).
    /// </summary>
    private static List<Token> NumberTokens(List<string> numbers)
    {
        if (numbers.Count == 1)
            return [new Token(Number, numbers[0])];
        if (numbers.Count == 2)
            return [new Token([Number[0]], numbers[0]), new Token(NumberAnd, numbers[1])];

        var values = numbers.Select(n => int.TryParse(n, out var v) ? v : int.MinValue).ToList();
        var step = values[1] - values[0];
        var consecutive = step is 1 or -1 && values.Zip(values.Skip(1)).All(p => p.Second - p.First == step);
        if (consecutive)
            return [new Token([Number[0]], numbers[0]), new Token(NumberUntil, numbers[^1])];

        var list = numbers.Take(numbers.Count - 2).Select(n => new Token([Number[1]], n)).ToList();
        list.Add(new Token([Number[0]], numbers[^2]));
        list.Add(new Token(NumberAnd, numbers[^1]));
        return list;
    }

    /// <summary>Kluc zvuku druhu vozna v banke (podla textu zvuku v skupine Vozy1).</summary>
    private static string KindKey(CarKind kind, SoundPicker picker, List<PlanRadeniaIssue> issues)
    {
        string? Find(string text) => picker.KeyByText(KindSingle[0], text);

        switch (kind)
        {
            case CarKind.First:
                return Find("vozen prvej triedy") ?? "v1";
            case CarKind.Sleeper:
                return Find("lozkovy vozen") ?? "luv";
            case CarKind.Couchette:
                return Find("lezadlovy vozen") ?? "lev";
            case CarKind.Restaurant:
                return Find("restauracny vozen") ?? "rv";
            case CarKind.Bistro:
                if (Find("bistro vozen") is { } bistro)
                    return bistro;
                issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.BistroAsRestaurant));
                return Find("restauracny vozen") ?? "rv";
            case CarKind.FirstSecond:
                if (Find("vozen prvej a druhej triedy") is { } mixed)
                    return mixed;
                issues.Add(new PlanRadeniaIssue(PlanRadeniaIssueKind.MixedAsSecond));
                return Find("vozen druhej triedy") ?? "v2";
            default:
                return Find("vozen druhej triedy") ?? "v2";
        }
    }

    private static bool Same(Station? a, Station? b) => a is null ? b is null : b is not null && a.ID == b.ID;

    /// <summary>
    /// Vyhladavanie zvukov banky podla skupiny a kluca (bez ohladu na velkost pismen); chybajuce zvuky si pamata.
    /// </summary>
    private sealed class SoundPicker(IReadOnlyList<FyzSound> sounds)
    {
        private readonly Dictionary<(string, string), FyzSound> _map = sounds
            .GroupBy(s => (s.Group.Key.ToUpperInvariant(), s.Key.ToUpperInvariant()))
            .ToDictionary(g => g.Key, g => g.First());

        public List<string> Missing { get; } = [];

        /// <summary>Ci banka zvuk ma (bez zapamatania ako chybajuci).</summary>
        public bool Has(string group, string key) => _map.ContainsKey((group.ToUpperInvariant(), key.ToUpperInvariant()));

        public FyzSound? Get(string group, string key)
        {
            if (_map.TryGetValue((group.ToUpperInvariant(), key.ToUpperInvariant()), out var sound))
                return sound;
            var name = $"{group}/{key}";
            if (!Missing.Contains(name))
                Missing.Add(name);
            return null;
        }

        /// <summary>Kluc zvuku skupiny, ktoreho text je (bez diakritiky a velkosti pismen) <paramref name="text" />.</summary>
        public string? KeyByText(string group, string text) =>
            sounds.FirstOrDefault(s => s.Group.Key.Equals(group, StringComparison.OrdinalIgnoreCase) &&
                                       StringUtils.RemoveDiacritics(s.Text.Trim().TrimEnd(',', '.').ToLowerInvariant()) == text)?.Key;
    }
}
