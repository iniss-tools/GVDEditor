using System.Collections;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration.Iniss2;

internal static partial class Iniss2Export
{
    /// <summary>
    /// Stanice hlasenia jednej stanice: zaznamy vlakov, vzory pre prazdny vyber v kratkom a dlhom hlaseni.
    /// </summary>
    /// <param name="Trains">zaznamy StaHlasVl.xml</param>
    /// <param name="ShortPatterns">vzory StaHlasB.xml</param>
    /// <param name="LongPatterns">vzory StaHlasC.xml</param>
    private sealed record ReportStationsData(
        List<TrainReportStations> Trains,
        List<IReadOnlyList<ReportPatternStop>> ShortPatterns,
        List<IReadOnlyList<ReportPatternStop>> LongPatterns);

    /// <summary>
    /// Predvolene stanice grafikonu do smeru: stanica je v kratkom alebo dlhom hlaseni, ked ju tak hlasi aspon jeden vlak.
    /// </summary>
    private static List<DefaultStation> DefaultStations(IEnumerable<(int Station, bool Short, bool Long)> reported, Dictionary<int, GvStation> stations) =>
        reported.GroupBy(item => item.Station)
            .Select(group => new DefaultStation(group.Key, stations.TryGetValue(group.Key, out var station) ? station.Name : "")
            {
                Short = group.Any(item => item.Short),
                Long = group.Any(item => item.Long)
            })
            .ToList();

    private sealed partial class StationBuilder
    {
        // jeden variant hlasenia: INISS 3.39 cita len StaHlasC.txt a jeho vyber plati aj pre kratke hlasenie
        private bool SingleVariant => document.ReportVariants.Count <= 1;

        /// <summary>
        /// Stanice do smeru, ktore vlaky stanice hlasia - pre predvolene stanice grafikonu.
        /// </summary>
        public IEnumerable<(int Station, bool Short, bool Long)> ReportedToStations() =>
            _trains.Where(train => train.Routing != Routing.Konciaci)
                .SelectMany(train => train.StaniceDoSmeru)
                .Where(station => station.IsInLongReport || station.IsInShortReport)
                .Select(station => (Number(station), SingleVariant ? station.IsInLongReport : station.IsInShortReport, station.IsInLongReport))
                .Where(item => item.Item1 is not null)
                .Select(item => (item.Item1!.Value, item.Item2, item.Item3));

        // vyber INISS 3.39 pri vlaku do StaHlasVl.xml po dnoch ako ostatne subory stanice; prazdny vyber server nahradi vzorom
        // alebo predvolenymi stanicami, preto ho vyjadri vzor trasy so ziadnou hlasenou stanicou
        private ReportStationsData ToReportStations(Dictionary<string, NumberDays> numbers)
        {
            var trains = new List<TrainReportStations>();
            var shortPatterns = new List<IReadOnlyList<ReportPatternStop>>();
            var longPatterns = new List<IReadOnlyList<ReportPatternStop>>();

            foreach (var (number, data) in numbers)
            {
                var records = new List<(TrainReportStations Stations, BitArray Days)>();
                for (var day = 0; day < data.All.Length; day++)
                {
                    var train = data.Running(day).FirstOrDefault();
                    if (train is null)
                        continue;

                    var stations = Selection(train, number);
                    if (stations.ShortFrom.Count + stations.LongFrom.Count + stations.ShortTo.Count + stations.LongTo.Count == 0)
                        continue;

                    var index = records.FindIndex(record => Same(record.Stations, stations));
                    if (index < 0)
                    {
                        index = records.Count;
                        records.Add((stations, new BitArray(data.All.Length)));
                    }

                    records[index].Days[day] = true;
                }

                trains.AddRange(records.Select(record => record.Stations with { Validity = ValidityOf(record.Days, data) }));
            }

            foreach (var train in _trains.Where(train => DaysOf(train).HasAnySet() && _rows.ContainsKey(train)))
            {
                var row = _rows[train];
                var selection = Selection(train, train.Number);
                List<int> from = [.. row.Stops.Take(row.Own + 1).Select(stop => stop.Station)];
                List<int> to = [.. row.Stops.Skip(row.Own).Select(stop => stop.Station)];
                if (selection.ShortFrom.Count == 0)
                    AddPattern(shortPatterns, from);
                if (selection.LongFrom.Count == 0)
                    AddPattern(longPatterns, from);
                if (selection.ShortTo.Count == 0)
                    AddPattern(shortPatterns, to);
                if (selection.LongTo.Count == 0)
                    AddPattern(longPatterns, to);
            }

            return new ReportStationsData(trains, shortPatterns, longPatterns);
        }

        private TrainReportStations Selection(Train train, string number)
        {
            var from = train.Routing == Routing.Vychadzajuci ? [] : train.StaniceZoSmeru;
            var to = train.Routing == Routing.Konciaci ? [] : train.StaniceDoSmeru;
            return new TrainReportStations(number, "")
            {
                ShortFrom = Numbers(from, SingleVariant ? station => station.IsInLongReport : station => station.IsInShortReport),
                LongFrom = Numbers(from, station => station.IsInLongReport),
                ShortTo = Numbers(to, SingleVariant ? station => station.IsInLongReport : station => station.IsInShortReport),
                LongTo = Numbers(to, station => station.IsInLongReport)
            };
        }

        private static List<int> Numbers(List<Station> route, Func<Station, bool> reported) =>
            [.. route.Where(reported).Select(Number).OfType<int>()];

        private static bool Same(TrainReportStations a, TrainReportStations b) =>
            a.ShortFrom.SequenceEqual(b.ShortFrom) && a.LongFrom.SequenceEqual(b.LongFrom) && a.ShortTo.SequenceEqual(b.ShortTo)
            && a.LongTo.SequenceEqual(b.LongTo);

        private static void AddPattern(List<IReadOnlyList<ReportPatternStop>> patterns, List<int> route)
        {
            if (!patterns.Exists(pattern => pattern.Select(stop => stop.Station).SequenceEqual(route)))
                patterns.Add([.. route.Select(station => new ReportPatternStop(station, false))]);
        }
    }
}
