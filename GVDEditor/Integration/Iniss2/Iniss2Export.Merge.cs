using System.Collections;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

internal static partial class Iniss2Export
{
    /// <summary>
    /// Zaznam vlaku 3.39 v jednej stanici pripraveny na spojenie s inymi stanicami.
    /// </summary>
    /// <param name="Train">vlak</param>
    /// <param name="Station">cislo stanice</param>
    /// <param name="From">zaciatok platnosti</param>
    /// <param name="To">koniec platnosti</param>
    /// <param name="Days">dni vlaku v stanici v obdobi platnosti</param>
    /// <param name="Stops">trasa s casmi v stanici</param>
    /// <param name="Own">index bodu stanice v trase</param>
    /// <param name="Variant">varianta s udajmi vlaku bez dni</param>
    private sealed record StationRow(Train Train, int Station, DateOnly From, DateOnly To, BitArray Days, List<GvStop> Stops, int Own, GvVariant Variant)
    {
        /// <summary>
        /// Druh vlaku v GV.XML; motorovy vlak je osobny, druh MOs mu da MOS.xml.
        /// </summary>
        public string TrainType => Stops[0].TrainType;
    }

    /// <summary>
    /// Jedna jazda vlaku cez stanice: zaznamy stanic s dnom v stanici a posunom dna oproti prvej stanici jazdy.
    /// </summary>
    private sealed class Run
    {
        public List<(StationRow Row, DateOnly Date, int Offset)> Items { get; } = [];

        /// <summary>Referencny den jazdy - den v prvej stanici.</summary>
        public DateOnly Reference => Items[0].Date;
    }

    /// <summary>
    /// Spoji zaznamy vlakov z viacerych stanic do vlakov GV.XML. Zaznamy s rovnakym cislom sa spoja, ked druha stanica lezi
    /// na trase prvej za nou alebo prva na trase druhej pred nou. Den v dalsej stanici sa urci z casov: skorsi prichod nez odchod
    /// z predchadzajucej stanice je nasledujuci den.
    /// </summary>
    private sealed class TrainMerger(ExportState state)
    {
        public List<GvTrain> Merge(List<StationRow> rows)
        {
            var trains = new List<GvTrain>();

            foreach (var group in rows.GroupBy(row => row.Train.Number))
                AddTrains(trains, Runs([.. group]), [.. group.Where(row => !row.Days.HasAnySet())]);

            return trains;
        }

        private List<Run> Runs(List<StationRow> rows)
        {
            var stations = rows.Select(row => row.Station).Distinct().ToList();
            var order = stations.Count > 1 ? StationOrder(rows, stations) : stations;
            var merging = order is not null;
            if (order is null)
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TrainNotMerged, rows[0].Train.Number));
                order = stations;
            }

            var runs = new List<Run>();
            var index = new Dictionary<(int Station, DateOnly Date), List<Run>>();

            foreach (var station in order)
            {
                var position = order.IndexOf(station);
                foreach (var row in rows.Where(row => row.Station == station))
                {
                    var earlier = rows.Where(other => order.IndexOf(other.Station) < position).ToList();
                    var predecessors = merging
                        ? earlier.Where(other => Step(other, row) is not null).OrderByDescending(other => order.IndexOf(other.Station)).ToList()
                        : [];
                    var attached = false;

                    foreach (var date in Dates(row))
                    {
                        var (run, offset) = Predecessor(predecessors, row, date, index);
                        if (run is null)
                        {
                            run = new Run();
                            runs.Add(run);
                        }
                        else
                        {
                            attached = true;
                        }

                        run.Items.Add((row, date, offset));
                        if (!index.TryGetValue((station, date), out var list))
                            index[(station, date)] = list = [];
                        list.Add(run);
                    }

                    // zaznam s rovnakym cislom v inej stanici sa nespojil - trasa nesedi alebo sa dni nestretli
                    if (merging && earlier.Count > 0 && !attached && row.Train.Routing != Routing.Vychadzajuci && row.Days.HasAnySet())
                        state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TrainNotMerged, row.Train.Number));
                }
            }

            return runs;
        }

        // jazda v predchadzajucej stanici, na ktoru zaznam nadvazuje; najblizsia stanica ma prednost
        private static (Run? Run, int Offset) Predecessor(List<StationRow> predecessors, StationRow row, DateOnly date,
            Dictionary<(int Station, DateOnly Date), List<Run>> index)
        {
            foreach (var previous in predecessors)
            {
                var step = Step(previous, row)!.Value;
                var day = date.AddDays(-step);
                if (!index.TryGetValue((previous.Station, day), out var candidates))
                    continue;

                foreach (var run in candidates)
                {
                    var item = run.Items.FindIndex(item => ReferenceEquals(item.Row, previous) && item.Date == day);
                    if (item >= 0 && run.Items.TrueForAll(other => other.Row.Station != row.Station))
                        return (run, run.Items[item].Offset + step);
                }
            }

            return (null, 0);
        }

        // poradie stanic na trase vlaku s cislom; null = trasy si odporuju
        private static List<int>? StationOrder(List<StationRow> rows, List<int> stations)
        {
            var edges = new HashSet<(int From, int To)>();
            foreach (var from in rows)
            {
                foreach (var to in rows.Where(to => to.Station != from.Station && Step(from, to) is not null))
                    edges.Add((from.Station, to.Station));
            }

            var order = new List<int>();
            var remaining = stations.ToList();
            while (remaining.Count > 0)
            {
                var next = remaining.FindIndex(station => !edges.Any(edge => edge.To == station && remaining.Contains(edge.From)));
                if (next < 0)
                    return null;

                order.Add(remaining[next]);
                remaining.RemoveAt(next);
            }

            return order;
        }

        // o kolko dni neskor je vlak v stanici to nez v stanici from; null = vlak z from do to nepokracuje
        private static int? Step(StationRow from, StationRow to)
        {
            if (from.Train.Routing == Routing.Konciaci || to.Train.Routing == Routing.Vychadzajuci || !Follows(from, to))
                return null;

            var departure = from.Stops[from.Own].Departure ?? from.Stops[from.Own].Arrival;
            var arrival = to.Stops[to.Own].Arrival ?? to.Stops[to.Own].Departure;
            if (departure is null || arrival is null)
                return null;

            var day = from.Stops[from.Own].Departure.HasValue ? from.Stops[from.Own].DepartureDay : from.Stops[from.Own].ArrivalDay;
            return day + (arrival < departure ? 1 : 0);
        }

        // stanica to je na trase za stanicou from alebo stanica from na trase pred stanicou to; trasa opacnym smerom nesedi
        private static bool Follows(StationRow from, StationRow to)
        {
            var ahead = Ahead(from, to.Station);
            var behind = Behind(to, from.Station);
            if (Behind(from, to.Station) && !ahead || Ahead(to, from.Station) && !behind)
                return false;

            return ahead || behind;
        }

        private static bool Ahead(StationRow row, int station) => row.Stops.FindIndex(row.Own + 1, stop => stop.Station == station) >= 0;

        private static bool Behind(StationRow row, int station) => row.Own > 0 && row.Stops.FindLastIndex(row.Own - 1, stop => stop.Station == station) >= 0;

        private static IEnumerable<DateOnly> Dates(StationRow row)
        {
            for (var i = 0; i < row.Days.Length; i++)
            {
                if (row.Days[i])
                    yield return row.From.AddDays(i);
            }
        }

        // jazdy s rovnakymi zaznamami a posunmi su jedna varianta; varianty vlaku s rovnakym klucom nesmu mat spolocny den,
        // server by v nom vzal len prvu; zaznam bez dni ostane ako varianta, ktora nejde nikdy, aby sa udaje nestratili
        private static void AddTrains(List<GvTrain> trains, List<Run> runs, List<StationRow> idle)
        {
            var variants = new List<(List<(StationRow Row, int Offset)> Items, HashSet<DateOnly> Days)>();
            foreach (var run in runs)
            {
                var items = run.Items.Select(item => (item.Row, item.Offset)).ToList();
                var index = variants.FindIndex(variant => variant.Items.SequenceEqual(items));
                if (index < 0)
                {
                    index = variants.Count;
                    variants.Add((items, []));
                }

                variants[index].Days.Add(run.Reference);
            }

            variants.AddRange(idle.Select(row => (new List<(StationRow Row, int Offset)> { (row, 0) }, new HashSet<DateOnly>())));

            var buckets = new List<(StationRow First, List<(List<(StationRow Row, int Offset)> Items, HashSet<DateOnly> Days)> Variants, HashSet<DateOnly> Days)>();
            foreach (var variant in variants)
            {
                var first = variant.Items[0].Row;
                var bucket = buckets.FindIndex(bucket => SameTrain(bucket.First, first) && !bucket.Days.Overlaps(variant.Days));
                if (bucket < 0)
                {
                    bucket = buckets.Count;
                    buckets.Add((first, [], []));
                }

                buckets[bucket].Variants.Add(variant);
                buckets[bucket].Days.UnionWith(variant.Days);
            }

            foreach (var (first, list, _) in buckets)
            {
                trains.Add(new GvTrain(first.From, first.To, list.Select(variant => Variant(variant.Items, first.From, first.To, variant.Days)).ToList()));
            }
        }

        private static bool SameTrain(StationRow a, StationRow b) =>
            a.Train.Number == b.Train.Number && a.Train.Name == b.Train.Name && a.TrainType == b.TrainType && a.From == b.From && a.To == b.To;

        // trasa z prvej stanice; dalsia stanica dostane svoje casy posunute o dni od prvej a trasu za sebou zo svojho zaznamu
        private static GvVariant Variant(List<(StationRow Row, int Offset)> items, DateOnly from, DateOnly to, HashSet<DateOnly> days)
        {
            var first = items[0].Row;
            var stops = first.Stops.ToList();
            var own = first.Own;

            foreach (var (row, offset) in items.Skip(1))
            {
                var at = stops.FindIndex(own + 1, stop => stop.Station == row.Station);
                List<GvStop> head;
                if (at >= 0)
                {
                    head = stops.Take(at).ToList();
                }
                else
                {
                    var previous = stops[own].Station;
                    var start = row.Stops.FindLastIndex(row.Own - 1, stop => stop.Station == previous);
                    head = [.. stops.Take(own + 1), .. row.Stops.Skip(start + 1).Take(row.Own - start - 1)];
                }

                var stop = row.Stops[row.Own] with
                {
                    ArrivalDay = row.Stops[row.Own].ArrivalDay + offset,
                    DepartureDay = row.Stops[row.Own].DepartureDay + offset,
                    Number = "",
                    TrainType = "",
                    TrainName = "",
                    Line = ""
                };

                // udaje vlaku sa zapisu znova, len ked sa za stanicou zmenia
                var section = head.Last(previous => previous.Number.Length > 0);
                var name = row.Train.Name.Length > 0 ? row.Train.Name : section.TrainName;
                var line = !string.IsNullOrEmpty(row.Train.LineDeparture) ? row.Train.LineDeparture : section.Line;
                if (section.Number != row.Train.Number || section.TrainType != row.TrainType || section.TrainName != name || section.Line != line)
                    stop = stop with { Number = row.Train.Number, TrainType = row.TrainType, TrainName = name, Line = line };

                own = head.Count;
                stops = [.. head, stop, .. row.Stops.Skip(row.Own + 1)];
            }

            var bits = new BitArray(to.DayNumber - from.DayNumber + 1);
            foreach (var day in days)
                bits[day.DayNumber - from.DayNumber] = true;

            return first.Variant with { Validity = GvValidity.Text(from, to, bits), Stops = stops };
        }
    }
}
