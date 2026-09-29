using System.Globalization;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Tools.Utils;

namespace GVDEditor.Formats;

/// <summary>
/// Zakladne udaje vlakov (Export3A.txt), obdobie platnosti a mapa dni (Export3B.txt) a datumove obmedzenie
/// (Export3C.txt). Riadok s ID n patri vlaku na indexe n - 1.
/// </summary>
internal static class Export3File
{
    private const string DAILY = "ide denne";

    /// <summary>
    /// Precita Export3A.txt - vlaky v poradi podla ID.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="context">druhy vlakov, stanice a dopravcovia</param>
    /// <exception cref="FormatException">neznamy druh vlaku alebo smerovanie</exception>
    public static List<Train> ReadTrains(string file, GrafikonContext context)
    {
        var trains = new List<Train>();
        ReadRows(file, FILE_EXPORT3A, (row, _) =>
        {
            var train = new Train { ID = int.Parse(row[0], CultureInfo.InvariantCulture), Number = row[1], Name = row[2] };

            foreach (var type in context.Workspace.TrainsTypes)
                if (row[3] == type.Key)
                    train.Type = type;

            if (train.Type == null)
                throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Export3_TypeMissing, row[3]));

            train.Variant = int.Parse(row[4], CultureInfo.InvariantCulture);

            train.Routing = Routing.Parse(row[5]) ?? throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.Export3_RoutingMissing, row[5]));
            train.IsMedzistatny = row[6].Contains('M');
            train.IsMiestenkovy = row[6].Contains('R');
            train.IsMimoriadny = row[6].Contains('X');
            train.IsDialkovy = row[6].Contains('D');
            train.IsIbaLozkovy = row[6].Contains('L');
            train.IsNizkopodlazny = row[6].Contains('N');
            train.IsPrestupovy = row[6].Contains('P');
            train.IsPriznakO = row[6].Contains('O');

            if (train.Routing == Routing.Vychadzajuci)
            {
                train.Departure = Time(row[8]);
                train.EndingStation = context.StationFromID(row.ElementAtOrDefault(10));
            }
            else if (train.Routing == Routing.Prechadzajuci)
            {
                train.Arrival = Time(row[7]);
                train.Departure = Time(row[8]);

                train.StartingStation = context.StationFromID(row.ElementAtOrDefault(9));
                train.EndingStation = context.StationFromID(row.ElementAtOrDefault(10));
            }
            else if (train.Routing == Routing.Konciaci)
            {
                train.Arrival = Time(row[7]);
                train.StartingStation = context.StationFromID(row.ElementAtOrDefault(9));
            }

            var idOperator = ParseIntOrDefault(row.ElementAtOrDefault(12), -1);
            train.Operator = Operator.GetFromID(context.Document.Operators, idOperator) ?? Operator.None;

            train.LineArrival = row.ElementAtOrDefault(13);
            train.LineDeparture = row.ElementAtOrDefault(14);

            trains.Add(train);
        });
        return trains;
    }

    /// <summary>
    /// Precita Export3B.txt - obdobie platnosti vlakov.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <returns>mapy dni ('0'/'1' za kazdy den obdobia) vlakov, ktore ju maju</returns>
    public static Dictionary<Train, string> ReadValidity(string file, IList<Train> trains)
    {
        var dayMaps = new Dictionary<Train, string>();
        ReadRows(file, FILE_EXPORT3B, (row, _) =>
        {
            var train = trains[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];
            train.ZaciatokPlatnosti = ParseDateOnlyAlts(row[1]);
            train.KoniecPlatnosti = ParseDateOnlyAlts(row[2]);

            // mapa sa pouzije len pre vlaky bez poznamky v Export3C (Export3 ju niekedy nevypise)
            var map = row.ElementAtOrDefaultStr(3).Trim();
            if (map.Length > 0)
                dayMaps[train] = map;
        });
        return dayMaps;
    }

    /// <summary>
    /// Precita Export3C.txt - datumove obmedzenie vlakov (poznamky). Vlak bez poznamky dostane obmedzenie
    /// podla mapy dni z Export3B.txt.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="dayMaps">mapy dni z <see cref="ReadValidity" /></param>
    public static void ReadNotes(string file, IList<Train> trains, Dictionary<Train, string> dayMaps)
    {
        ReadRows(file, FILE_EXPORT3C, (row, _) =>
        {
            var train = trains[int.Parse(row[0], CultureInfo.InvariantCulture) - 1];

            // INISS spoji vsetky poznamky ciarkou a medzerou; 0 alebo prazdne = bez obmedzenia
            var noteCount = ParseIntOrDefault(row.ElementAtOrDefault(1));
            var notes = new List<string>();
            for (var k = 0; k < noteCount; k++)
            {
                var note = row.ElementAtOrDefaultStr(2 + k);
                if (!string.IsNullOrWhiteSpace(note))
                    notes.Add(note);
            }

            train.DateLimitText = notes.Count > 0
                ? string.Join(", ", notes)
                : DateLimitTextFromMap(train, dayMaps.GetValueOrDefault(train));
        });
    }

    /// <summary>
    /// Vrati textove obmedzenie vlaku bez poznamky v Export3C podla mapy dni z Export3B.
    /// Ak mapa chyba, nesedi dlzkou na obdobie platnosti alebo ide kazdy den, vrati "ide denne".
    /// </summary>
    /// <param name="train">vlak s nacitanym obdobim platnosti</param>
    /// <param name="map">mapa dni ('0'/'1' za kazdy den obdobia) alebo <see langword="null"/></param>
    /// <returns>text obmedzenia</returns>
    internal static string DateLimitTextFromMap(Train train, string? map)
    {
        if (string.IsNullOrEmpty(map) || train.KoniecPlatnosti < train.ZaciatokPlatnosti)
            return DAILY;

        var limit = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti, insertMarks: false);
        if (map.Length != limit.TotalDays || map.Any(c => c != '0' && c != '1') || map.All(c => c == '1'))
            return DAILY;

        return limit.BitArrayToText(StringToBitArray(map));
    }

    /// <summary>
    /// Zapise Export3A.txt.
    /// </summary>
    /// <param name="file">cesta k suboru</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="gvd">hlavicka grafikonu (stanica grafikonu je zaciatok alebo koniec trasy)</param>
    /// <param name="comments">komentare na zaciatok suboru</param>
    public static void WriteTrains(string file, IEnumerable<Train> trains, GVDInfo gvd, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) =>
        {
            var row = new CsvRow
            {
                Id(index),
                train.Number.Quote(),
                (train.Name ?? "").UTFtoANSI().Quote(),
                train.Type.ToString().Quote(),
                train.Variant != -1 ? train.Variant.ToString(CultureInfo.InvariantCulture) : "-1",
                train.Routing.CharSymbol,
                Flags(train)
            };

            if (train.Routing == Routing.Vychadzajuci)
                row.AddRange(["", Time(train.Departure), gvd.ThisStation.ID, train.StaniceDoSmeru.Last().ID]);
            else if (train.Routing == Routing.Konciaci)
                row.AddRange([Time(train.Arrival), "", train.StaniceZoSmeru.First().ID, gvd.ThisStation.ID]);
            else
                row.AddRange([Time(train.Arrival), Time(train.Departure), train.StaniceZoSmeru.First().ID, train.StaniceDoSmeru.Last().ID]);

            row.Add("-1");
            row.Add(train.Operator.Id.ToString(CultureInfo.InvariantCulture));
            row.Add(string.IsNullOrEmpty(train.LineArrival) ? "" : train.LineArrival);
            row.Add(string.IsNullOrEmpty(train.LineDeparture) ? "" : train.LineDeparture);
            return row;
        }));

    /// <summary>
    /// Zapise Export3B.txt - obdobie platnosti a mapu dni podla datumoveho obmedzenia.
    /// </summary>
    public static void WriteValidity(string file, IEnumerable<Train> trains, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) =>
        {
            var limit = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti);
            return new CsvRow
            {
                Id(index),
                train.ZaciatokPlatnosti.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                train.KoniecPlatnosti.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                BitArrayToString(limit.TextToBitArray(train.DateLimitText))
            };
        }));

    /// <summary>
    /// Zapise Export3C.txt - datumove obmedzenie ako jednu poznamku.
    /// </summary>
    public static void WriteNotes(string file, IEnumerable<Train> trains, IEnumerable<string> comments) =>
        WriteRows(file, comments, trains.Select((train, index) => new CsvRow { Id(index), "1", train.DateLimitText.Quote() }));

    /// <summary>
    /// ID vlaku v suboroch vlakov - poradie od 1.
    /// </summary>
    internal static string Id(int index) => (index + 1).ToString(CultureInfo.InvariantCulture);

    private static string Time(TimeOnly? time) => time?.ToString("HH:mm", CultureInfo.InvariantCulture) ?? "";

    // prazdne pole je polnoc - rovnako cita cas aj INISS
    private static TimeOnly Time(string text) => TimeOnly.FromDateTime(ParseTime(text));

    private static string Flags(Train train)
    {
        var flags = "";
        if (train.IsIbaLozkovy) flags += "L";
        if (train.IsMimoriadny) flags += "X";
        if (train.IsMiestenkovy) flags += "R";
        if (train.IsMedzistatny) flags += "M";
        if (train.IsNizkopodlazny) flags += "N";
        if (train.IsDialkovy) flags += "D";
        if (train.IsPrestupovy) flags += "P";
        if (train.IsPriznakO) flags += "O";
        return flags;
    }
}
