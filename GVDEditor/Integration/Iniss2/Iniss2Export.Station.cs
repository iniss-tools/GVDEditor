using System.Collections;
using System.Globalization;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Calendar;
using ToolsCore.Iniss.StateDgm;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

internal static partial class Iniss2Export
{
    /// <summary>
    /// Prevod grafikonu jednej stanice: najprv zaznamy vlakov na spojenie, potom subory stanice nad spojenym grafikonom.
    /// </summary>
    private sealed partial class StationBuilder(GrafikonDocument document, GVDInfo gvd, string grafikon, StateDgmDiagram? stateDgm, Iniss2ExportSource source,
        ExportState state)
    {
        private readonly Dictionary<Train, BitArray> _days = [];
        private readonly Dictionary<Train, StationRow> _rows = [];
        private List<Train> _trains = [];

        /// <summary>Cislo stanice; plati po <see cref="Prepare" />.</summary>
        public int Station { get; private set; }

        /// <summary>Vyluky vlakov stanice; plati po <see cref="Files" />.</summary>
        public List<TrainLockout> Lockouts { get; private set; } = [];

        /// <summary>Odklony vlakov stanice; plati po <see cref="Files" />.</summary>
        public List<TrainDiversion> Diversions { get; private set; } = [];

        /// <summary>Vyluky a odklony obsluhy prevedene v tejto stanici.</summary>
        public HashSet<TrainNotice> ExportedNotices { get; } = [];

        /// <summary>Vyluky a odklony obsluhy, ktore pre vlak stanice neplatia v ziadny jeho den.</summary>
        public HashSet<TrainNotice> OutsideNotices { get; } = [];

        /// <summary>
        /// Zaznamy vlakov stanice s trasou, casmi a dnami v stanici.
        /// </summary>
        /// <exception cref="Iniss2ExportException">stanica nema cislo</exception>
        public List<StationRow> Prepare()
        {
            _trains = document.Trains.ToList();
            Station = Number(gvd.ThisStation) ?? throw new Iniss2ExportException(Iniss2ExportIssue.StationWithoutNumber, gvd.ThisStation.Name);
            state.Stations[Station] = new GvStation(Station, gvd.ThisStation.Name);

            var groups = _trains
                .GroupBy(train => (train.Number, train.Name, train.Type.Key, From(train), To(train)))
                .Select(group => group.OrderBy(train => train.Variant).ToList())
                .ToList();

            groups.ForEach(SeparateVariants);

            var rows = groups.SelectMany(group => group).Select(Row).ToList();
            foreach (var row in rows)
                _rows[row.Train] = row;
            return rows;
        }

        /// <summary>
        /// Subory stanice; obdobia vlakov v stanici sa beru zo spojeneho grafikonu ako na serveri.
        /// </summary>
        /// <param name="timetable">spojeny grafikon</param>
        public Iniss2StationData Files(GvFile timetable)
        {
            var numbers = NumbersOf(_trains, timetable.TrainRanges(Station));
            Lockouts = ToLockouts(numbers);
            Diversions = ToDiversions(numbers, Station);

            var (tables, catalogs) = ToTables();
            var reportStations = ToReportStations(numbers);
            return new Iniss2StationData(Station, document.Tracks.Where(track => track.Key != Track.None.Key).Select(ToTrack).ToList(),
                ToPositions(numbers), ToMotorTrains(numbers), ToAdditions(numbers), ToCompositions(numbers), ToLanguages(numbers), ToReportKeys(),
                tables, catalogs, ToTabTabs(catalogs), ToTrainTexts(numbers))
            {
                StateDiagram = ToStateDiagram(),
                ReportStations = reportStations.Trains,
                ShortReportPatterns = reportStations.ShortPatterns,
                LongReportPatterns = reportStations.LongPatterns
            };
        }

        private StationRow Row(Train train)
        {
            if (train.IsDialkovy)
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.LongDistanceFlag, train.Number));

            var stops = Stops(train, Station);
            var variant = new GvVariant
            {
                Operator = train.Operator is null || train.Operator == Operator.None ? "" : train.Operator.Name,
                Flags = Flags(train),
                Stops = stops
            };

            return new StationRow(train, Station, From(train), To(train), DaysOf(train), stops,
                stops.FindIndex(stop => stop.Flags.HasFlag(GvStopFlags.Interest)), variant);
        }

        // vyluka vlaku z grafikonu plati v jeho dni, vyluka obsluhy v obdobi, ktore zadala
        private List<TrainLockout> ToLockouts(Dictionary<string, NumberDays> numbers)
        {
            var lockouts = new List<TrainLockout>();

            foreach (var (number, data) in numbers)
            {
                var records = new List<(string Key, BitArray Days)>();
                foreach (var row in data.Rows.Where(row => row.Train.LockoutNumber != 0 && row.Days.HasAnySet()))
                {
                    var key = NoticeKey(LockoutKind, row.Train.LockoutNumber);
                    var index = records.FindIndex(record => record.Key == key);
                    if (index < 0)
                        records.Add((key, new BitArray(row.Days)));
                    else
                        records[index].Days.Or(row.Days);
                }

                lockouts.AddRange(records.Select(record => new TrainLockout(number, record.Key, data.Range.From, data.Range.To, ValidityOf(record.Days, data))));
            }

            foreach (var notice in source.Lockouts)
            {
                if (NoticeValidity(notice, numbers) is { } validity)
                {
                    ExportedNotices.Add(notice);
                    lockouts.Add(new TrainLockout(notice.Number, NoticeKey(LockoutKind, notice.Code), notice.From, notice.To, validity));
                }
            }

            return lockouts;
        }

        private List<TrainDiversion> ToDiversions(Dictionary<string, NumberDays> numbers, int station)
        {
            var diversions = new List<TrainDiversion>();
            foreach (var notice in source.Diversions)
            {
                if (NoticeValidity(notice, numbers) is not { } validity)
                    continue;

                ExportedNotices.Add(notice);
                diversions.Add(new TrainDiversion(notice.Number, station, NoticeKey(DiversionKind, notice.Code), notice.From, notice.To, validity));
            }

            return diversions;
        }

        // poznamka vyluky alebo odklonu obsluhy v jeho obdobi; null = netyka sa vlakov stanice
        private string? NoticeValidity(TrainNotice notice, Dictionary<string, NumberDays> numbers)
        {
            if (notice.Station != 0 && Number(gvd.ThisStation) != notice.Station || !numbers.TryGetValue(notice.Number, out var data))
                return null;

            var count = notice.To.DayNumber - notice.From.DayNumber + 1;
            var days = new BitArray(count, true);
            if (notice.Days.Length > 0)
            {
                try
                {
                    days = new DateLimit(notice.From, notice.To, insertMarks: false).TextToBitArray(notice.Days);
                }
                catch (DateLimit.ParseException)
                {
                    days = new BitArray(0);
                }

                if (days.Length != count)
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.BadValidity, notice.Number));
                    return null;
                }
            }

            var shift = notice.From.DayNumber - data.Range.From.DayNumber;
            var running = false;
            for (var i = 0; i < count && !running; i++)
                running = days[i] && i + shift >= 0 && i + shift < data.All.Length && data.All[i + shift];

            if (!running)
            {
                OutsideNotices.Add(notice);
                return null;
            }

            if (count > GvValidity.MaxDays && !days.HasAllSet())
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.PeriodTooLong, notice.Number));

            return GvValidity.Text(notice.From, notice.To, days);
        }

        // zabudovany text 3.39 je v INISS2 funkcia LogText s rovnakym nazvom; text obsluhy je vzor so stanicami v ConstReports.xml,
        // nazov vzoru je predloha 3.39 s podtrzitkami namiesto medzier
        private string NoticeKey(char kind, int code)
        {
            var text = source.UserTexts.FirstOrDefault(text => text.Kind == kind && text.Code == code);
            if (text is null)
                return string.Create(CultureInfo.InvariantCulture, $"{kind}{code}");

            var index = state.UserReports.FindIndex(user => ReferenceEquals(user.Text, text));
            if (index < 0)
            {
                index = state.UserReports.Count;
                state.UserReports.Add((text, new ConstReport(-(index + 1), text.Name, text.Template.Replace(' ', '_'), StationsOf(text))));
            }

            return state.UserReports[index].Report.Key.ToString(CultureInfo.InvariantCulture);
        }

        private List<int> StationsOf(LogZvukText text)
        {
            var stations = new List<int>();
            foreach (var id in text.StationIds)
            {
                if (int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var number) && number > 0)
                    stations.Add(number);
                else
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.StationWithoutNumber, id));
            }

            return stations;
        }

        private DateOnly From(Train train) => train.ZaciatokPlatnosti == default ? gvd.StartValidTimeTable : train.ZaciatokPlatnosti;

        private DateOnly To(Train train) => train.KoniecPlatnosti == default ? gvd.EndValidTimeTable : train.KoniecPlatnosti;

        // server vezme v kazdy den prvu variantu, ktora ide, druha by sa stratila; variant s menej dnami je vynimka z toho s viacerymi
        private void SeparateVariants(List<Train> variants)
        {
            var taken = new BitArray(DaysOf(variants[0]).Length);
            var overlap = false;

            foreach (var train in variants.OrderBy(train => Count(DaysOf(train))))
            {
                var days = DaysOf(train);
                var free = new BitArray(taken).Not().And(days);
                overlap |= Count(free) != Count(days);
                _days[train] = free;
                taken.Or(days);
            }

            if (overlap)
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.VariantOverlap, variants[0].Number));
        }

        private static int Count(BitArray days) => days.Cast<bool>().Count(day => day);

        private static GvVariantFlags Flags(Train train) =>
            (train.IsPriznakO ? GvVariantFlags.O : GvVariantFlags.None)
            | (train.IsMedzistatny ? GvVariantFlags.M : GvVariantFlags.None)
            | (train.IsMiestenkovy ? GvVariantFlags.R : GvVariantFlags.None)
            | (train.IsIbaLozkovy ? GvVariantFlags.L : GvVariantFlags.None)
            | (train.IsDialkovy ? GvVariantFlags.D : GvVariantFlags.None)
            | (train.IsMimoriadny ? GvVariantFlags.X : GvVariantFlags.None)
            | (train.IsNizkopodlazny ? GvVariantFlags.N : GvVariantFlags.None);

        // dni vlaku v jeho obdobi; INISS 3.39 ich pocita v jazyku a so sviatkami stanice
        private BitArray DaysOf(Train train)
        {
            if (_days.TryGetValue(train, out var known))
                return known;

            var from = From(train);
            var to = To(train);
            var count = Math.Max(0, to.DayNumber - from.DayNumber + 1);
            var days = new BitArray(count, true);

            if (!string.IsNullOrEmpty(train.DateLimitText))
            {
                try
                {
                    days = new DateLimit(from, to, insertMarks: false).TextToBitArray(train.DateLimitText);
                }
                catch (DateLimit.ParseException)
                {
                    days = new BitArray(0);
                }

                if (days.Length != count)
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.BadValidity, train.Number));
                    days = new BitArray(count);
                }
            }

            if (count > GvValidity.MaxDays && !days.HasAllSet())
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.PeriodTooLong, train.Number));

            _days[train] = days;
            return days;
        }

        private List<GvStop> Stops(Train train, int station)
        {
            var before = train.Routing == Routing.Vychadzajuci ? [] : train.StaniceZoSmeru;
            var after = train.Routing == Routing.Konciaci ? [] : train.StaniceDoSmeru;

            // INISS 3.39 dava vlaku s trasou bez stanice grafikonu celu trasu do oboch smerov
            if (before.Count > 0 && before.SequenceEqual(after))
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.RouteWithoutStation, train.Number));
                before = [];
                after = [];
            }

            var stops = new List<GvStop>();
            AddStops(stops, Route(before, train.Routing == Routing.Vychadzajuci ? null : train.StartingStation));
            var own = stops.Count;
            stops.Add(OwnStop(train, station));
            AddStops(stops, Route(after, train.Routing == Routing.Konciaci ? null : train.EndingStation));

            var type = train.IsMotorovy || train.Type.Key == MotorType ? PassengerType : train.Type.Key;
            var arrivalLine = train.LineArrival ?? "";
            var departureLine = train.LineDeparture ?? "";

            // cislo a linka platia od bodu, kde su, a len v bode s cislom vlaku
            stops[0] = stops[0] with { Number = train.Number, TrainType = type, TrainName = train.Name, Line = own > 0 && arrivalLine.Length > 0 ? arrivalLine : FirstOf(train, arrivalLine, departureLine) };

            if (own > 0 && departureLine.Length > 0 && departureLine != stops[0].Line)
                stops[own] = stops[own] with { Number = train.Number, TrainType = type, TrainName = train.Name, Line = departureLine };

            return stops;
        }

        private static string FirstOf(Train train, string arrivalLine, string departureLine) =>
            train.Routing == Routing.Konciaci || departureLine.Length == 0 ? arrivalLine : departureLine;

        // trasa bez stanic: aspon vychodzia alebo konecna stanica z Export3A
        private static List<Station> Route(List<Station> route, Station? end) =>
            route.Count > 0 || end is null ? route : [end];

        private void AddStops(List<GvStop> stops, IEnumerable<Station> route)
        {
            foreach (var station in route)
            {
                if (Number(station) is { } number)
                {
                    state.Stations.TryAdd(number, new GvStation(number, station.Name));
                    stops.Add(new GvStop { Station = number });
                }
                else if (station.ID != ToolsCore.Iniss.Grafikon.Station.None.ID)
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.StationWithoutNumber, station.Name));
                }
            }
        }

        private static GvStop OwnStop(Train train, int station)
        {
            var arrival = Minutes(train.Arrival);
            var departure = Minutes(train.Departure);

            return new GvStop
            {
                Station = station,
                Arrival = arrival,
                Departure = departure,
                // vlak prechadzajuci cez polnoc odchadza az nasledujuci den
                DepartureDay = departure < arrival ? 1 : 0,
                Flags = GvStopFlags.Interest | (train.IsPrestupovy ? GvStopFlags.Pr : GvStopFlags.None)
            };
        }

        private static int? Minutes(TimeOnly? time) => time is { } value ? value.Hour * 60 + value.Minute : null;

        private static int? Number(Station station) =>
            int.TryParse(station.ID, NumberStyles.None, CultureInfo.InvariantCulture, out var number) && number > 0 ? number : null;

        private static TrackDefinition ToTrack(Track track) => new()
        {
            Key = track.Key,
            Name = track.Name,
            TrackText = track.TrackName,
            PlatformText = track.Platform == Platform.None ? "" : track.Platform.Key,
            PlatformTrackText = track.PlatformTrackText,
            AltTrackText = track.AltTrackText,
            TrackSound = track.SoundName,
            PlatformSound = track.Platform.SoundName
        };

        // server vztahuje koliaje, dodatky, radenie aj jazyky na cislo vlaku a jeho obdobie v stanici
        private Dictionary<string, NumberDays> NumbersOf(List<Train> trains, Dictionary<string, (DateOnly From, DateOnly To)> ranges)
        {
            var numbers = new Dictionary<string, NumberDays>();

            foreach (var group in trains.GroupBy(train => train.Number))
            {
                if (!ranges.TryGetValue(group.Key, out var range))
                    continue;

                var rows = group.Select(train => new TrainDays(train, InRange(train, range))).ToList();
                if (group.Any(train => Count(InRange(train, range)) < Count(DaysOf(train))))
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.StationDayOutsidePeriod, group.Key));
                var all = new BitArray(Length(range));
                rows.ForEach(row => all.Or(row.Days));
                numbers[group.Key] = new NumberDays(range, rows, all);
            }

            return numbers;
        }

        private List<TrainPosition> ToPositions(Dictionary<string, NumberDays> numbers)
        {
            var positions = new List<TrainPosition>();

            foreach (var (number, data) in numbers)
            {
                var records = new List<(string Arrival, string Departure, BitArray Days)>();
                var conflict = false;

                for (var day = 0; day < data.All.Length; day++)
                {
                    var running = data.Running(day);
                    if (running.Count == 0)
                        continue;

                    var arrivals = ArrivalTracks(running);
                    var departures = DepartureTracks(running);
                    conflict |= arrivals.Count > 1 || departures.Count > 1;

                    // Pozice.txt INISSu dava vlaku kolaj prichodu aj odchodu, aj ked prichod alebo odchod nema - vyrazy ju citaju
                    var arrival = arrivals.FirstOrDefault("");
                    if (arrival.Length == 0)
                        arrival = running.Select(train => TrackKey(train.Track)).FirstOrDefault(key => key.Length > 0, "");
                    var departure = departures.FirstOrDefault("");
                    if (departure.Length == 0)
                        departure = running.Select(train => TrackKey(train.TrackDeparture ?? train.Track)).FirstOrDefault(key => key.Length > 0, "");
                    if (arrival.Length == 0 && departure.Length == 0)
                        continue;

                    var index = records.FindIndex(record => record.Arrival == arrival && record.Departure == departure);
                    if (index < 0)
                    {
                        index = records.Count;
                        records.Add((arrival, departure, new BitArray(data.All.Length)));
                    }

                    records[index].Days[day] = true;
                }

                if (conflict)
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.SharedNumberTracks, number));

                foreach (var (arrival, departure, days) in records)
                    positions.Add(new TrainPosition(number, arrival, departure, ValidityOf(days, data)));
            }

            return positions;
        }

        // prichod urcuje vlak, ktory v stanici konci, odchod vlak, ktory z nej vychadza; prechadzajuci vlak s rovnakym cislom je len jeho cast
        private static List<string> ArrivalTracks(List<Train> running) =>
            running.Where(train => train.Routing != Routing.Vychadzajuci)
                .OrderByDescending(train => train.Routing == Routing.Konciaci)
                .Select(train => TrackKey(train.Track))
                .Where(key => key.Length > 0)
                .Distinct()
                .ToList();

        private static List<string> DepartureTracks(List<Train> running) =>
            running.Where(train => train.Routing != Routing.Konciaci)
                .OrderByDescending(train => train.Routing == Routing.Vychadzajuci)
                .Select(train => TrackKey(train.TrackDeparture ?? train.Track))
                .Where(key => key.Length > 0)
                .Distinct()
                .ToList();

        private static string TrackKey(Track? track) => track is null || track.Key == Track.None.Key ? "" : track.Key;

        private static List<MosTrain> ToMotorTrains(Dictionary<string, NumberDays> numbers)
        {
            var motors = new List<MosTrain>();

            foreach (var (number, data) in numbers)
            {
                var days = new BitArray(data.All.Length);
                foreach (var row in data.Rows.Where(row => IsMotor(row.Train)))
                    days.Or(row.Days);

                if (days.HasAnySet())
                    motors.Add(new MosTrain(number, ValidityOf(days, data)));
            }

            return motors;
        }

        // dodatok vlaku 3.39 plati vo vsetky dni vlaku; rovnaky dodatok vlakov s jednym cislom sa spoji
        private List<TrainAddition> ToAdditions(Dictionary<string, NumberDays> numbers)
        {
            var additions = new List<TrainAddition>();

            foreach (var (number, data) in numbers)
            {
                var records = new List<(string Key, string Reports, BitArray Days)>();

                foreach (var row in data.Rows.Where(row => row.Days.HasAnySet()))
                {
                    foreach (var addition in row.Train.Doplnky)
                    {
                        var key = AdditionPrefix + Dodatok.CodeFromKey(addition.Sound.Key);
                        var description = addition.Sound.Text is { Length: > 0 } text ? text : addition.Sound.Name is { Length: > 0 } name ? name : key;
                        state.AdditionFunctions.TryAdd(key, new LogTextAddition(key, description, $"{addition.Sound.Group.Key}/{addition.Sound.Key}"));
                        var reports = Reports(addition.ChosenReports, TypesFor(row.Train.Routing));
                        var index = records.FindIndex(record => record.Key == key && record.Reports == reports);
                        if (index < 0)
                            records.Add((key, reports, new BitArray(row.Days)));
                        else
                            records[index].Days.Or(row.Days);
                    }
                }

                additions.AddRange(records.Select(record => new TrainAddition(number, record.Key, record.Reports, ValidityOf(record.Days, data))));
            }

            return additions;
        }

        private List<TrainComposition> ToCompositions(Dictionary<string, NumberDays> numbers)
        {
            var compositions = new List<TrainComposition>();

            foreach (var composition in document.Radenia)
            {
                if (!numbers.TryGetValue(composition.CisloVlaku, out var data))
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.CompositionWithoutTrain, composition.CisloVlaku));
                    continue;
                }

                // radenie pre ciel bez cisla by platilo pre kazdy ciel
                var end = composition.DestStation is { } destination ? Number(destination) : 0;
                if (end is null)
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.StationWithoutNumber, composition.DestStation!.Name));
                    continue;
                }

                var days = CompositionDays(composition, data);
                if (days is null)
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.BadValidity, composition.CisloVlaku));
                    continue;
                }

                // radenie mimo dni vlaku by server nikdy nevybral
                if (!new BitArray(days).And(data.All).HasAnySet())
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.CompositionOutsidePeriod, composition.CisloVlaku));
                    continue;
                }

                compositions.Add(new TrainComposition(composition.CisloVlaku, end.Value, Reports(composition.ChosenReports, document.ReportTypes),
                    ValidityOf(days, data), composition.Sounds.Select(sound => $"{sound.Language.Key}/{sound.Group.Key}/{sound.Key}").ToList()));
            }

            return compositions;
        }

        // dni radenia v obdobi cisla vlaku; null = poznamka sa neda rozvinut
        private static BitArray? CompositionDays(Radenie composition, NumberDays data)
        {
            if (composition.Validity is not { } validity)
                return new BitArray(data.All.Length, true);

            var count = validity.To.DayNumber - validity.From.DayNumber + 1;
            BitArray own;
            if (string.IsNullOrEmpty(composition.DatObm))
            {
                own = new BitArray(Math.Max(0, count), true);
            }
            else
            {
                try
                {
                    own = new DateLimit(validity.From, validity.To, insertMarks: false).TextToBitArray(composition.DatObm);
                }
                catch (DateLimit.ParseException)
                {
                    return null;
                }

                if (own.Length != count)
                    return null;
            }

            var days = new BitArray(data.All.Length);
            var shift = validity.From.DayNumber - data.Range.From.DayNumber;
            for (var i = 0; i < own.Length; i++)
            {
                if (own[i] && i + shift >= 0 && i + shift < days.Length)
                    days[i + shift] = true;
            }

            return days;
        }

        // jazyky vlakov s jednym cislom sa v den spoja; pre cislo a den plati prvy zaznam, preto maju zaznamy rozne dni
        private List<TrainLanguages> ToLanguages(Dictionary<string, NumberDays> numbers)
        {
            var result = new List<TrainLanguages>();

            foreach (var (number, data) in numbers)
            {
                var records = new List<(List<string> Languages, BitArray Days)>();

                for (var day = 0; day < data.All.Length; day++)
                {
                    var running = data.Running(day);
                    var used = source.Languages.Where(language => running.Any(train => train.Languages.Contains(language))).ToList();
                    if (used.Count == 0)
                        continue;

                    List<string> keys = used.Count == source.Languages.Count
                        ? [ToolsCore.Iniss2.Grafikon.ForeignFile.AllLanguages]
                        : used.Where(language => !language.IsBasic).Select(language => language.Key).ToList();
                    if (keys.Count == 0)
                        continue;

                    var index = records.FindIndex(record => record.Languages.SequenceEqual(keys));
                    if (index < 0)
                    {
                        index = records.Count;
                        records.Add((keys, new BitArray(data.All.Length)));
                    }

                    records[index].Days[day] = true;
                }

                result.AddRange(records.Select(record => new TrainLanguages(number, ValidityOf(record.Days, data), record.Languages)));
            }

            return result;
        }

        // hlasenia sablony, inak z lokalneho Categori.txt: pismeno je znak typu hlasenia 3.39, nazov jeho kluc
        private List<ReportKey> ToReportKeys() =>
            source.ReportTemplate is { } template ? [.. template] : document.ReportTypes.Select(type => new ReportKey(type.Key, type.Char, string.Join(' ',
                new[] { type.BaseTrain ? "V" : "", type.PassThrough ? "P" : "", type.TerminateTrain ? "K" : "" }.Where(category => category.Length > 0))))
                .ToList();

        // typy hlasenia, ktore mapa dodatku 3.39 pozna pri smerovani vlaku
        private List<ReportType> TypesFor(Routing routing)
        {
            if (routing == Routing.Vychadzajuci)
                return document.ReportTypes.Where(type => type.BaseTrain).ToList();

            return routing == Routing.Prechadzajuci
                ? document.ReportTypes.Where(type => type.PassThrough).ToList()
                : document.ReportTypes.Where(type => type.TerminateTrain).ToList();
        }

        // pismena vybranych typov a variantov; vsetky typy, ktore vlak pozna, su prazdna hodnota
        private string Reports(IEnumerable<ChosenReportType> chosen, IEnumerable<ReportType> applicable)
        {
            var letters = new List<string>();
            foreach (var report in chosen)
            {
                foreach (var variant in report.Variants.Where(variant => variant is not null))
                {
                    var letter = Letter(report.Type, variant);
                    if (!letters.Contains(letter))
                        letters.Add(letter);
                }
            }

            letters.Remove("");
            if (letters.Count == 0)
                return NoReports;

            var all = applicable.SelectMany(type => document.ReportVariants.Select(variant => Letter(type, variant))).Where(letter => letter.Length > 0);
            return all.All(letters.Contains) ? "" : string.Concat(letters);
        }

        // prvy variant hlasenia (dlhe) ma velke pismeno, dalsie male - tak cita INISS 3.39 aj Razeni1.txt; pismeno moze dat sablona
        private string Letter(ReportType type, ReportVariant variant)
        {
            var index = document.ReportVariants.IndexOf(variant);
            var first = index < 0 ? variant.Key == 0 : index == 0;
            var letter = source.ReportLetters.TryGetValue(type.Key, out var mapped) ? mapped : type.Char;
            return first ? letter.ToUpperInvariant() : letter.ToLowerInvariant();
        }

        private static bool IsMotor(Train train) => train.IsMotorovy || train.Type.Key == MotorType;

        // poznamka len vtedy, ked zaznam neplati vo vsetky dni, ked vlak s tym cislom stanicou ide; ine dni nehraju rolu
        private static string ValidityOf(BitArray days, NumberDays data)
        {
            var inside = new BitArray(days).And(data.All);
            return new BitArray(inside).Xor(data.All).HasAnySet() ? GvValidity.Text(data.Range.From, data.Range.To, inside) : "";
        }

        private static int Length((DateOnly From, DateOnly To) range) => range.To.DayNumber - range.From.DayNumber + 1;

        private BitArray InRange(Train train, (DateOnly From, DateOnly To) range)
        {
            var days = DaysOf(train);
            var result = new BitArray(Length(range));
            var shift = From(train).DayNumber - range.From.DayNumber;

            for (var i = 0; i < days.Length; i++)
            {
                if (days[i] && i + shift >= 0 && i + shift < result.Length)
                    result[i + shift] = true;
            }

            return result;
        }
    }

    /// <summary>
    /// Vlak a jeho dni v obdobi cisla vlaku.
    /// </summary>
    private sealed record TrainDays(Train Train, BitArray Days);

    /// <summary>
    /// Cislo vlaku v stanici: obdobie podla servera, vlaky s tym cislom a vsetky ich dni.
    /// </summary>
    private sealed record NumberDays((DateOnly From, DateOnly To) Range, List<TrainDays> Rows, BitArray All)
    {
        /// <summary>
        /// Vlaky, ktore v den idu.
        /// </summary>
        public List<Train> Running(int day) => Rows.Where(row => row.Days[day]).Select(row => row.Train).ToList();
    }
}
