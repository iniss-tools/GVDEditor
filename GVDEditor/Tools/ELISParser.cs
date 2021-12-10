using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Trieda spracujuca text exportovany z programu ELIS do objektov.
/// </summary>
public class ELISParser
{
    private const string FORMAT_EX = "Chyba v súbore na riadku {0}. ";
    private const int NOT_SET = -2;

    /// <summary>Initializes a new instance of the <see cref="ELISParser" /> class.</summary>
    public ELISParser(string path, List<Train> definedTrains, List<TrainType> trainTypes, List<Operator> operators, GVDInfo gvd,
        Track defaultTrack)
    {
        Path = path;
        DefinedTrains = definedTrains;
        TrainTypes = trainTypes;
        Operators = operators;
        GVD = gvd;
        DefaultTrack = defaultTrack;
    }

    /// <summary>
    ///     Cesta k vygenerovanemu ELIS suboru.
    /// </summary>
    public string Path { get; }

    /// <summary>
    ///     Zoznam uz definovanych vlakov.
    /// </summary>
    public List<Train> DefinedTrains { get; }

    /// <summary>
    ///     Vsetky typy vlakov definovane v stanici.
    /// </summary>
    public List<TrainType> TrainTypes { get; }

    /// <summary>
    ///     Zoznam vsetkych definovanych dopravcov.
    /// </summary>
    public List<Operator> Operators { get; }

    /// <summary>
    ///     Informacie o aktualnom grafikone.
    /// </summary>
    public GVDInfo GVD { get; }

    /// <summary>
    ///     kolaj, ktora bude priradena kazdemu vlaku.
    /// </summary>
    public Track DefaultTrack { get; }

    /// <summary>
    ///     Ci sa maju preskocit vlaky, ktore su prechadzajuce.
    /// </summary>
    public bool OmitPassingTrains { get; set; }

    /// <summary>
    ///     Ci su v subore definovani dopravcovia ku kazdemu vlaku.
    /// </summary>
    public bool DefinedOperators { get; set; } = true;

    /// <summary>
    ///     Ci su v subore definovane datumove obmedzenia ku kazdemu vlaku.
    /// </summary>
    public bool DefinedDateLimits { get; set; } = true;

    /// <summary>
    ///     Ci sa maju vlaky (ich varianty) zoradit a prepocitat.
    /// </summary>
    public bool ReorderTrains { get; set; }

    /// <summary>
    ///     Ci sa ma pouzit novy format formatovania.
    /// </summary>
    public bool NewFormat { get; set; }

    /// <summary>
    ///     Spracuje textovy subor exportovany z programu ELIS do zoznamu vlakov ako objekty typu <see cref="Train" />.
    /// </summary>
    /// <exception cref="ArgumentException">ak sa zisti neplatny typ vlaku alebo datum</exception>
    /// <exception cref="FormatException">ak sa zistia nedefinovane stanice alebo subor obsahuje neocakavane znaky</exception>
    public List<Train> ReadELISFile()
    {
        if (!File.Exists(Path)) return null;

        var invalidStations = new List<string>();
        List<Train> trains;
        int countdef;

        if (DefinedTrains == null)
        {
            trains = new List<Train>();
            countdef = 0;
        }
        else
        {
            trains = new List<Train>(DefinedTrains);
            countdef = DefinedTrains.Count;
        }

        //vyber formatu
        if (NewFormat)
            ReadWithNewFormat(trains, invalidStations);
        else
            ReadWithOldFormat(trains, invalidStations);

        //vypis neplatnych stanic (ak nejake su)
        if (invalidStations.Count != 0)
        {
            var builder = new StringBuilder();
            foreach (var invalid in invalidStations)
                builder.Append(invalid + "\r\n");

            throw new FormatException($"Stanice {builder} nie sú definované.");
        }

        //ci sa maju zoradit vlaky podla variant
        if (ReorderTrains)
            //zotriedenie vlakov podla variant
            for (var i = 0; i < trains.Count; i++)
            {
                var t1 = trains[i];
                if (t1.Variant != NOT_SET)
                    continue;

                var variants = trains.Where((t2, j) => i != j && Train.IsSameVariant(t1, t2)).ToList();

                variants.Add(t1);

                Train.ReorderVariants(variants);
            }

        //vratenie vysledku (a vymazanie duplicitnych vlakov, ktore prisli s argumentom deftrains)
        trains.RemoveRange(0, countdef);
        return trains;
    }

    private void ReadWithNewFormat(ICollection<Train> trains, ICollection<string> invalidStations)
    {
        using var file = new StreamReader(File.OpenRead(Path));
        var riadok = 0;
        string line = null;

        while (!file.EndOfStream)
            try
            {
                if (line is null)
                {
                    line = file.ReadLine();
                    riadok++;
                }

                //inicializacia vlaku a prveho riadku
                var firstrow = line;

                if (firstrow == null)
                    return;

                var threeParts = firstrow.Replace("\t\t\t\t\t\t", "\t").Split('\t');
                var frow = threeParts[0].Split(' ');
                var count = frow.Length;

                var train = new Train();

                //typ vlaku
                foreach (var type in TrainTypes.Where(type => frow[0] == type.Key)) train.Type = type;
                if (train.Type == null) throw new ArgumentException($"Neexistujúci typ vlaku {frow[0]}");

                //cislo vlaku
                train.Number = frow[1];

                //nazov vlaku
                var trname = "";
                for (var i = 2; i < count; i++)
                    trname += frow[i] + " ";
                train.Name = trname.Trim();

                //obdobie platnosti
                train.ZaciatokPlatnosti = Utils.ParseDateAlts(threeParts[1]);
                train.KoniecPlatnosti = Utils.ParseDateAlts(threeParts[2]);

                //spracovanie variant
                train.Variant = NOT_SET;

                //stanice vlaku
                Station station = null;
                var thisst = false;
                while (true)
                {
                    line = file.ReadLine();
                    riadok++;

                    var rowsts = SplitRow(line, true);
                    var count2 = rowsts.Length;
                    if (line.StartsWith("\t\t") || !line.StartsWith("\t"))
                    {
                        if (station != null && station.ID == GVD.ThisStation.ID)
                        {
                            train.Arrival = train.Departure;
                            train.Departure = null;
                            train.Routing = Routing.Konciaci;
                        }

                        break;
                    }

                    var stname = SplitRow(rowsts[0])[0]; //bez znakov O, !, ... oddelene 2 medzerami
                    station = Station.GetFromName(stname);
                    if (station == null)
                    {
                        invalidStations.Add($"{stname} ({riadok})");
                        continue;
                    }

                    if (station.ID == GVD.ThisStation.ID)
                    {
                        thisst = true;

                        if (Utils.TryParseTime(rowsts[count2 - 3], out var prichod))
                            train.Arrival = prichod;

                        try
                        {
                            train.Departure = Utils.ParseTime(rowsts[count2 - 2]);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException($"String {rowsts[count2 - 2]} sa nedá previesť na DateTime.", e);
                        }

                        continue;
                    }

                    station.IsInLongReport = true;

                    //este nebola domovska stanica
                    if (!thisst)
                        train.StaniceZoSmeru.Add(station);
                    //uz bola domovska stanica
                    else
                        train.StaniceDoSmeru.Add(station);
                }

                //nastavenie vychodzej/konecnej stanice
                var firstZo = train.StaniceZoSmeru.FirstOrDefault();
                var lastDo = train.StaniceDoSmeru.LastOrDefault();
                if (firstZo != null)
                {
                    firstZo.IsInShortReport = true;
                    train.StartingStation = firstZo;
                }

                if (lastDo != null)
                {
                    lastDo.IsInShortReport = true;
                    train.EndingStation = lastDo;
                }

                //nastavenie smerovania
                if (train.StaniceZoSmeru.Count != 0 && train.StaniceDoSmeru.Count != 0)
                    train.Routing = Routing.Prechadzajuci;
                else if (train.StaniceZoSmeru.Count == 0)
                    train.Routing = Routing.Vychadzajuci;
                else if (train.StaniceDoSmeru.Count == 0)
                    train.Routing = Routing.Konciaci;

                //citanie poznamok (dopravca, daterem)
                if (DefinedOperators)
                {
                    var lineOp = line;

                    if (lineOp != null)
                    {
                        lineOp = lineOp.Trim();
                        var sep = lineOp.Split(';');
                        train.Operator = Operator.GetFromName(Operators, sep[0]) ?? Operator.None;
                    }

                    if (DefinedDateLimits)
                    {
                        line = file.ReadLine();
                        riadok++;
                    }
                }
                else
                {
                    train.Operator = Operator.None;
                }

                var withoutLimit = false;

                if (DefinedDateLimits)
                {
                    var lineLim = line;
                    if (lineLim != null)
                    {
                        lineLim = lineLim.Trim();
                        var dr = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti);
                        try
                        {
                            dr.TextToBitArray(lineLim);
                            train.DateLimitText = lineLim;
                        }
                        catch (Exception)
                        {
                            //automaticky tu bude ide denne
                            train.DateLimitText = "";
                            withoutLimit = true;
                        }
                    }
                }

                //preskocenie poznamok - okrem riadku s linkou
                while (true)
                {
                    if (withoutLimit)
                    {
                        withoutLimit = false;
                    }
                    else
                    {
                        line = file.ReadLine();
                        riadok++;
                    }

                    if (line is null || !line.StartsWith("\t\t"))
                        break;

                    line = line.Trim();
                    if (line.StartsWith("linka"))
                    {
                        var route = line.Substring(line.IndexOf('('));
                        route = route.Replace(")", "");
                        var fromTo = route.Split(new[] { "->" }, StringSplitOptions.None);

                        var cols = line.Split(' ');
                        if (fromTo[0] == GVD.ThisStation.Name)
                        {
                            train.LineDeparture = cols[1];
                        }
                        else if (fromTo[1] == GVD.ThisStation.Name)
                        {
                            train.LineArrival = cols[1];
                        }
                        else
                        {
                            train.LineArrival = cols[1];
                            train.LineDeparture = cols[1];
                        }
                    }
                }

                //nastavenie kolaje
                train.Track = DefaultTrack;

                //ak vlak neobsahuje ThisStation, bude preskoceny
                //a ak vlak nie je prechadzajuci alebo nema preskocit prechadzajuce tak ma podmienku uskutocnit 
                var isPassing = train.Routing == Routing.Prechadzajuci;
                if (thisst && (!isPassing || !OmitPassingTrains)) trains.Add(train);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(FORMAT_EX, riadok) + e.Message, e);
            }
    }

    private void ReadWithOldFormat(ICollection<Train> trains, ICollection<string> invalidStations)
    {
        using var file = new StreamReader(File.OpenRead(Path));
        var riadok = 0;

        while (!file.EndOfStream)
            try
            {
                //inicializacia vlaku a prveho riadku
                var firstrow = file.ReadLine();
                riadok++;
                if (firstrow == null)
                    return;

                var row = firstrow.Split(' ');
                var count = row.Length;
                var train = new Train();

                //typ vlaku
                foreach (var type in TrainTypes.Where(type => row[0] == type.Key)) train.Type = type;
                if (train.Type == null) throw new ArgumentException($"Neexistujúci typ vlaku {row[0]}");

                //cislo vlaku
                train.Number = row[1];

                //nazov vlaku
                var trname = "";
                for (var i = 2; i < count - 5; i++)
                    trname += row[i] + " ";
                train.Name = trname.Trim();

                //obdobie platnosti
                train.ZaciatokPlatnosti = Utils.ParseDateAlts(row[count - 3]);
                train.KoniecPlatnosti = Utils.ParseDateAlts(row[count - 1]);

                //spracovanie variant
                train.Variant = NOT_SET;

                //preskocenie
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                riadok += 3;

                //stanice vlaku
                Station station = null;
                var thisst = false;
                while (true)
                {
                    var rowsts = SplitRow(file.ReadLine());
                    riadok++;
                    var count2 = rowsts.Length;
                    if (rowsts[0].StartsWith("-") || rowsts[0].StartsWith("="))
                    {
                        if (station != null && station.ID == GVD.ThisStation.ID)
                        {
                            train.Arrival = train.Departure;
                            train.Departure = null;
                            train.Routing = Routing.Konciaci;
                        }

                        break;
                    }

                    station = Station.GetFromName(rowsts[0]);
                    if (station == null)
                    {
                        invalidStations.Add($"{rowsts[0]} ({riadok})");
                        continue;
                    }

                    if (station.ID == GVD.ThisStation.ID)
                    {
                        thisst = true;

                        if (Utils.TryParseTime(rowsts[count2 - 3], out var prichod))
                            train.Arrival = prichod;

                        try
                        {
                            train.Departure = Utils.ParseTime(rowsts[count2 - 2]);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException($"String {rowsts[count2 - 2]} sa nedá previesť na DateTime.", e);
                        }

                        continue;
                    }

                    station.IsInLongReport = true;

                    //este nebola domovska stanica
                    if (!thisst)
                        train.StaniceZoSmeru.Add(station);
                    //uz bola domovska stanica
                    else
                        train.StaniceDoSmeru.Add(station);
                }

                //nastavenie vychodzej/konecnej stanice
                var firstZo = train.StaniceZoSmeru.FirstOrDefault();
                var lastDo = train.StaniceDoSmeru.LastOrDefault();
                if (firstZo != null)
                {
                    firstZo.IsInShortReport = true;
                    train.StartingStation = firstZo;
                }

                if (lastDo != null)
                {
                    lastDo.IsInShortReport = true;
                    train.EndingStation = lastDo;
                }

                //nastavenie smerovania
                if (train.StaniceZoSmeru.Count != 0 && train.StaniceDoSmeru.Count != 0)
                    train.Routing = Routing.Prechadzajuci;
                else if (train.StaniceZoSmeru.Count == 0)
                    train.Routing = Routing.Vychadzajuci;
                else if (train.StaniceDoSmeru.Count == 0)
                    train.Routing = Routing.Konciaci;

                //citanie poznamok (dopravca, daterem)
                if (DefinedOperators)
                {
                    var lineOp = file.ReadLine();
                    riadok++;
                    if (lineOp != null)
                    {
                        lineOp = lineOp.Substring(2);
                        var sep = lineOp.Split(';');
                        train.Operator = Operator.GetFromName(Operators, sep[0]) ?? Operator.None;
                    }

                    if (!DefinedDateLimits)
                    {
                        file.ReadLine();
                        riadok++;
                    }
                }
                else
                {
                    train.Operator = Operator.None;
                }

                if (DefinedDateLimits)
                {
                    var firstl = "";
                    var dateRemIsComming = false;
                    while (true)
                    {
                        var tmp = file.ReadLine();
                        riadok++;
                        if (tmp != null && tmp.StartsWith("* "))
                        {
                            firstl = tmp;
                            dateRemIsComming = true;
                            break;
                        }

                        if (tmp == null || tmp.StartsWith("=")) break;
                    }

                    var lineDateRem = "";
                    if (dateRemIsComming)
                    {
                        firstl = firstl.Substring(2).Trim();
                        lineDateRem += firstl + " ";

                        while (true)
                        {
                            var linedr = file.ReadLine();
                            riadok++;
                            if (linedr != null)
                            {
                                if (linedr.StartsWith("=")) break;

                                linedr = linedr.Trim();
                                lineDateRem += linedr + " ";
                            }
                        }

                        lineDateRem = lineDateRem.Trim();
                        var dr = new DateLimit(train.ZaciatokPlatnosti, train.KoniecPlatnosti);
                        dr.TextToBitArray(lineDateRem); //tu sa vyhodi pripadna vynimka
                        train.DateLimitText = lineDateRem;
                    }
                }

                //nastavenie kolaje
                train.Track = DefaultTrack;

                //preskocenie
                file.ReadLine();
                file.ReadLine();
                riadok += 2;

                //ak vlak neobsahuje ThisStation, bude preskoceny
                //a ak vlak nie je prechadzajuci alebo nema preskocit prechadzajuce tak ma podmienku uskutocnit 
                var isPassing = train.Routing == Routing.Prechadzajuci;
                if (thisst && (!isPassing || !OmitPassingTrains)) trains.Add(train);
            }
            catch (Exception e)
            {
                throw new FormatException(string.Format(FORMAT_EX, riadok) + e.Message, e);
            }
    }

    private static string[] SplitRow(string row, bool tab = false)
    {
        var splitted = row.Split(new[] { tab ? "\t" : "  " }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < splitted.Length; i++)
        {
            splitted[i] = splitted[i].Trim();
        }

        return splitted;
    }
}