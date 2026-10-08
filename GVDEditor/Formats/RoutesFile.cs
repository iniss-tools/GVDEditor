using System.Globalization;
using ExControls;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static ToolsCore.Iniss.Grafikon.GvdFileConsts;
using static ToolsCore.Iniss.Tools.ParseUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Trasy vlakov: vzory tras (Vzory.txt), stanice v kratkom a dlhom hlaseni (StaHlasB.txt, StaHlasC.txt)
/// a priradenie vlakov k vzorom (Vlaky.txt).
/// </summary>
internal static class RoutesFile
{
    /// <summary>
    /// Precita vzory tras a stanice hlasene v kratkom a dlhom hlaseni.
    /// </summary>
    /// <param name="vzory">cesta k Vzory.txt</param>
    /// <param name="staHlasB">cesta k StaHlasB.txt (kratke hlasenie)</param>
    /// <param name="staHlasC">cesta k StaHlasC.txt (dlhe hlasenie)</param>
    /// <param name="context">stanice banky a grafikonu</param>
    public static List<Template> ReadTemplates(string vzory, string staHlasB, string staHlasC, GrafikonContext context)
    {
        var templates = new List<Template>();
        ReadRows(vzory, FileVzory, (row, _) =>
        {
            var template = new Template { ID = int.Parse(row[0], CultureInfo.InvariantCulture) };
            var count = int.Parse(row[1], CultureInfo.InvariantCulture);
            for (var i = 0; i < count; i++)
                template.Stations.Add(context.StationFromID(row[i + 2]));
            templates.Add(template);
        });

        ReadReportStations(staHlasB, FileStahlasb, templates, station => station.IsInShortReport = true);
        ReadReportStations(staHlasC, FileStahlasc, templates, station => station.IsInLongReport = true);
        return templates;
    }

    private static void ReadReportStations(string file, string fileName, List<Template> templates, Action<Station> mark) =>
        ReadRows(file, fileName, (row, _) =>
        {
            var template = templates[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];
            var count = int.Parse(row[1], CultureInfo.InvariantCulture);

            foreach (var station in template.Stations)
                for (var i = 0; i < count; i++)
                    if (station.ID == row[i + 2])
                        mark(station);
        });

    /// <summary>
    /// Precita Vlaky.txt a priradi vlakom trasy zo vzorov - stanice pred stanicou grafikonu su zo smeru,
    /// za nou do smeru. Kazdy vlak dostane vlastnu kopiu stanic.
    /// </summary>
    /// <param name="file">cesta k Vlaky.txt</param>
    /// <param name="templates">vzory tras z <see cref="ReadTemplates" /></param>
    /// <param name="trains">vlaky grafikonu</param>
    /// <param name="gvd">hlavicka grafikonu (stanica grafikonu)</param>
    /// <param name="trainTypes">druhy vlakov</param>
    public static void AssignRoutes(string file, List<Template> templates, List<Train> trains, GVDInfo gvd, IEnumerable<TrainType> trainTypes,
        LoadWarnings warnings) =>
        ReadRows(file, FileVlaky, (row, rowNumber) =>
        {
            var id = int.Parse(row[0], CultureInfo.InvariantCulture);
            var template = templates.Find(t => t.ID == id) ?? throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Routes_BadId, id));

            var count = int.Parse(row[1], CultureInfo.InvariantCulture);
            for (var i = 0; i < count; i++)
            {
                var number = row[2 + i * 4];
                var name = ParseStringOrDefault(row[3 + i * 4]);
                var typeKey = ParseStringOrDefault(row[4 + i * 4]);
                var variant = int.Parse(row[5 + i * 4], CultureInfo.InvariantCulture);

                // INISS pri chybe v udajoch vlaku zahodi len zvysok riadka - rovnako to len zalogujeme
                var type = trainTypes.FirstOrDefault(t => t.Key == typeKey);
                if (type == null)
                {
                    warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.Routes_UnknownType, FileVlaky, rowNumber, typeKey, number));
                    break;
                }

                var train = Train.GetTrain(trains, number, name, type, variant);
                if (train == null)
                {
                    warnings.Add(string.Format(CultureInfo.CurrentCulture, Resources.Routes_NoDefinition, FileVlaky, rowNumber, number, type.Key, FileExport3A));
                    break;
                }

                var stations = Station.CopyRoute(template.Stations);
                var split = template.Stations.FindIndex(station => station.ID == gvd.ThisStation.ID);
                // trasa bez stanice grafikonu (chybne data): povodne spravanie - cela trasa je zo smeru aj do smeru
                train.StaniceZoSmeru.AddRange(split < 0 ? stations : stations.Take(split));
                train.StaniceDoSmeru.AddRange(split < 0 ? stations : stations.Skip(split + 1));
            }
        });

    /// <summary>
    /// Zoskupi vlaky podla trasy (stanice zo smeru, stanica grafikonu, stanice do smeru) - kazda roznych trasa
    /// je jeden vzor. Vzory su cislovane od 1 v poradi prveho vlaku s danou trasou.
    /// </summary>
    public static List<Template> BuildTemplates(IList<Train> trains, GVDInfo gvd)
    {
        var groups = new Dictionary<EquatableCollection<Station>, List<Train>>();
        foreach (var train in trains)
        {
            var route = new EquatableCollection<Station>(Route(train, gvd));
            if (!groups.TryGetValue(route, out var members))
                groups.Add(route, members = []);
            members.Add(train);
        }

        return groups.Select((pair, index) => new Template { ID = index + 1, Stations = pair.Key.ToList(), Trains = pair.Value }).ToList();
    }

    private static List<Station> Route(Train train, GVDInfo gvd) =>
        [.. train.StaniceZoSmeru, gvd.ThisStation, .. train.StaniceDoSmeru];

    /// <summary>
    /// Zapise Vzory.txt - stanice kazdeho vzoru.
    /// </summary>
    public static void WriteTemplates(string file, IEnumerable<Template> templates, IEnumerable<string> comments) =>
        WriteRows(file, comments, templates.Select(template =>
        {
            var row = new CsvRow { Id(template), template.Stations.Count.ToString(CultureInfo.InvariantCulture) };
            row.AddRange(template.Stations.Select(station => station.ID));
            return row;
        }));

    /// <summary>
    /// Zapise Vlaky.txt - vlaky kazdeho vzoru (cislo, nazov, druh, varianta).
    /// </summary>
    public static void WriteTrains(string file, IEnumerable<Template> templates, IEnumerable<string> comments) =>
        WriteRows(file, comments, templates.Select(template =>
        {
            var row = new CsvRow { Id(template), template.Trains.Count.ToString(CultureInfo.InvariantCulture) };
            foreach (var train in template.Trains)
                row.AddRange([train.Number.Quote(), train.Name.Quote(), train.Type.Key.Quote(), train.Variant.ToString(CultureInfo.InvariantCulture)]);
            return row;
        }));

    /// <summary>
    /// Zapise StaHlasB.txt alebo StaHlasC.txt - stanice vzoru hlasene v kratkom alebo dlhom hlaseni (bez stanice grafikonu).
    /// </summary>
    public static void WriteReportStations(string file, IEnumerable<Template> templates, GVDInfo gvd, Func<Station, bool> reported,
        IEnumerable<string> comments) =>
        WriteRows(file, comments, templates.Select(template =>
        {
            var stations = template.Stations.Where(station => reported(station) && station.ID != gvd.ThisStation.ID).ToList();
            var row = new CsvRow { Id(template), stations.Count.ToString(CultureInfo.InvariantCulture) };
            row.AddRange(stations.Select(station => station.ID));
            return row;
        }));

    private static string Id(Template template) => template.ID.ToString(CultureInfo.InvariantCulture);
}
