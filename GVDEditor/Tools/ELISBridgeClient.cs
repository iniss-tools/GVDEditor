using System.Collections;
using System.Text.RegularExpressions;
using GVDEditor.Entities;
using Iniss.Elis;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Nacita vlaky priamo z dat programu ELIS cez pomocny x86 proces ELISBridge,
///     ktory hovori s 32-bitovou kniznicou TT.dll.
/// </summary>
/// <remarks>
///     Nahradzuje povodne parsovanie textu rucne exportovaneho z programu ELIS;
///     datumove obmedzenia berie priamo z kniznice namiesto parsovania poznamok.
/// </remarks>
public sealed class ELISBridgeClient
{
    /// <summary>Nazov pomocneho programu, ktory sa hlada vedla GVDEditor.exe.</summary>
    public const string BridgeExeName = "ELISBridge.exe";

    /// <summary>
    ///     Hodnota varianty vlaku, kym sa varianty neprepocitaju.
    /// </summary>
    internal const int VariantNotSet = -2;

    private const int ExitNoTimetable = 2;
    private const int ExitStationNotFound = 3;
    private const int ExitRegistrationFailed = 4;

    /// <summary>Initializes a new instance of the <see cref="ELISBridgeClient" /> class.</summary>
    /// <param name="trainTypes">Vsetky typy vlakov definovane v stanici.</param>
    /// <param name="operators">Zoznam vsetkych definovanych dopravcov.</param>
    /// <param name="gvd">Informacie o aktualnom grafikone.</param>
    /// <param name="defaultTrack">Kolaj, ktora bude priradena kazdemu vlaku.</param>
    public ELISBridgeClient(List<TrainType> trainTypes, List<Operator> operators, GVDInfo gvd, Track defaultTrack)
    {
        TrainTypes = trainTypes;
        Operators = operators;
        GVD = gvd;
        DefaultTrack = defaultTrack;
    }

    /// <summary>
    ///     Vsetky typy vlakov definovane v stanici.
    /// </summary>
    public List<TrainType> TrainTypes { get; }

    /// <summary>
    ///     Zoznam vsetkych definovanych dopravcov.
    /// </summary>
    public List<Operator> Operators { get; }

    /// <summary>
    ///     Informácie o aktualnom grafikone.
    /// </summary>
    public GVDInfo GVD { get; }

    /// <summary>
    ///     Kolaj, ktora bude priradena kazdemu vlaku.
    /// </summary>
    public Track DefaultTrack { get; }

    /// <summary>
    ///     Uz definovane vlaky. Do vysledku sa nevracaju, ale vstupuju do cislovania variant,
    ///     aby import nepridelil variantu, ktoru uz iny vlak pouziva.
    /// </summary>
    public List<Train> DefinedTrains { get; set; }

    /// <summary>
    ///     Priecinok s instalaciou aplikacie Cestovne poriadky (obsahuje TT.dll).
    ///     Ak je prazdny, pouzije sa predvolena cesta zabudovana v ELISBridge.
    /// </summary>
    public string AppDirectory { get; set; }

    /// <summary>
    ///     Priecinok s datami (.tt subory). Ak je prazdny, pouzije sa podpriecinok Data1.
    /// </summary>
    public string DataDirectory { get; set; }

    /// <summary>
    ///     Ci sa maju preskocit vlaky, ktore su prechadzajuce.
    /// </summary>
    public bool OmitPassingTrains { get; set; }

    /// <summary>
    ///     Ci sa maju vlaky (ich varianty) zoradit a prepocitat.
    /// </summary>
    public bool ReorderTrains { get; set; }

    /// <summary>
    ///     Registracne cislo pre platene cestovne poriadky. Voľne stiahnuteľné dáta ho nepotrebujú.
    /// </summary>
    public string RegistrationNumber { get; set; }

    /// <summary>
    ///     Identifikacia klienta, ak ju platene data vyzaduju.
    /// </summary>
    public string ClientString { get; set; }

    /// <summary>
    ///     Priradenie nazvov stanic z ELIS k staniciam grafikonu: nazov -> ID stanice,
    ///     alebo <see cref="TxtParser.ELIS_MAP_SKIP" /> ak sa ma stanica z trasy vynechat.
    ///     Pouzije sa este pred automatickym rozpoznavanim nazvu.
    /// </summary>
    public Dictionary<string, string> StationMap { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Nacita data z programu ELIS. Bezi na pozadi - nevyzaduje ziadnu interakciu.
    /// </summary>
    /// <exception cref="FileNotFoundException">ak sa nenajde pomocny program ELISBridge</exception>
    /// <exception cref="InvalidOperationException">ak sa nepodari nacitat data ELIS</exception>
    public ElisResult LoadData() => RunBridge();

    /// <summary>
    ///     Vrati nazvy stanic z <paramref name="result" />, ktore sa nepodarilo priradit
    ///     k ziadnej stanici grafikonu ani cez <see cref="StationMap" />.
    /// </summary>
    public List<string> FindUnresolvedStations(ElisResult result)
    {
        var unresolved = new List<string>();

        foreach (var train in result.Trains)
        foreach (var name in train.StationsBefore.Concat(train.StationsAfter))
            if (!unresolved.Contains(name) && !StationMap.ContainsKey(name) && Resolve(name) is null)
                unresolved.Add(name);

        unresolved.Sort(StringComparer.CurrentCulture);
        return unresolved;
    }

    /// <summary>
    ///     Spracuje data programu ELIS do zoznamu vlakov ako objekty typu <see cref="Train" />.
    /// </summary>
    /// <returns>Vlaky prechadzajuce stanicou aktualneho grafikonu.</returns>
    /// <exception cref="FileNotFoundException">ak sa nenajde pomocny program ELISBridge</exception>
    /// <exception cref="FormatException">ak sa zistia nedefinovane typy vlakov</exception>
    /// <exception cref="InvalidOperationException">ak sa nepodari nacitat data ELIS</exception>
    public List<Train> ReadTrains() => Convert(LoadData());

    /// <summary>
    ///     Spusti pomocny program a vrati jeho vystup.
    /// </summary>
    private ElisResult RunBridge()
    {
        var exe = FindBridgeExe();
        var output = Path.Combine(Path.GetTempPath(), $"elis-{Guid.NewGuid():N}.xml");

        try
        {
            var arguments = new StringBuilder();
            arguments.Append("--station \"").Append(GVD.ThisStation.Name).Append("\" ");
            arguments.Append("--out \"").Append(output).Append('"');
            if (!string.IsNullOrEmpty(AppDirectory))
                arguments.Append(" --app \"").Append(AppDirectory.TrimEnd('\\')).Append('"');
            if (!string.IsNullOrEmpty(DataDirectory))
                arguments.Append(" --data \"").Append(DataDirectory.TrimEnd('\\')).Append('"');
            if (!string.IsNullOrEmpty(RegistrationNumber))
                arguments.Append(" --reg \"").Append(RegistrationNumber.Trim()).Append('"');
            if (!string.IsNullOrEmpty(ClientString))
                arguments.Append(" --client \"").Append(ClientString.Trim()).Append('"');

            var info = new ProcessStartInfo(exe, arguments.ToString())
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8
            };

            string error;
            int exitCode;
            using (var process = Process.Start(info))
            {
                if (process is null)
                    throw new InvalidOperationException($"Program {BridgeExeName} sa nepodarilo spustiť.");

                error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException(exitCode switch
                {
                    ExitNoTimetable => $"V dátach ELIS sa nenašiel žiadny cestovný poriadok.\r\n{error}",
                    ExitStationNotFound => $"Stanica {GVD.ThisStation.Name} sa v dátach ELIS nenachádza.\r\n{error}",
                    ExitRegistrationFailed => string.IsNullOrEmpty(error)
                        ? "Cestovný poriadok vyžaduje platné registračné číslo."
                        : error,
                    _ => $"Načítanie dát ELIS zlyhalo.\r\n{error}"
                });

            return ElisResult.Load(output);
        }
        finally
        {
            if (File.Exists(output))
                File.Delete(output);
        }
    }

    /// <summary>
    ///     Nájde pomocný program vedľa spusteného GVDEditora.
    /// </summary>
    private static string FindBridgeExe()
    {
        var directory = Path.GetDirectoryName(Application.ExecutablePath);
        var exe = Path.Combine(directory ?? string.Empty, BridgeExeName);
        if (File.Exists(exe))
            return exe;

        throw new FileNotFoundException(
            $"Pomocný program {BridgeExeName} sa nenašiel v priečinku {directory}. " +
            "Bez neho sa dáta z programu ELIS načítať nedajú.", exe);
    }

    /// <summary>
    ///     Prevedie vystup pomocneho programu na entity GVDEditora.
    /// </summary>
    public List<Train> Convert(ElisResult result)
    {
        var trains = new List<Train>();
        var invalidTypes = new List<string>();

        var validFrom = result.ValidFromDate;
        var validTo = result.ValidToDate;
        
        var dateLimit = new DateLimit(validFrom, validTo, insertMarks: false);
        var saveCheck = new DateLimit(validFrom, validTo);

        foreach (var source in result.Trains)
        {
            var routing = GetRouting(source);
            if (OmitPassingTrains && routing == Routing.Prechadzajuci)
                continue;

            var type = TrainTypes.FirstOrDefault(t => t.Key == source.Type);
            if (type is null)
            {
                if (!invalidTypes.Contains(source.Type))
                    invalidTypes.Add(source.Type);
                continue;
            }

            var train = new Train
            {
                Type = type,
                Number = source.Number,
                Name = source.Name,
                Routing = routing,
                Variant = VariantNotSet,
                Track = DefaultTrack,
                Operator = GetOperator(source),
                ZaciatokPlatnosti = validFrom,
                KoniecPlatnosti = validTo,
                Arrival = ToTime(source.ArrivalMinutes),
                Departure = ToTime(source.DepartureMinutes),
                DateLimitText = dateLimit.BitArrayToText(ToBitArray(source.RunsBits, dateLimit.TotalDays))
            };

            VerifyDateLimit(train, saveCheck);

            AddStations(source.StationsBefore, train.StaniceZoSmeru);
            AddStations(source.StationsAfter, train.StaniceDoSmeru);

            var firstZo = train.StaniceZoSmeru.FirstOrDefault();
            var lastDo = train.StaniceDoSmeru.LastOrDefault();
            if (firstZo is not null)
            {
                firstZo.IsInShortReport = true;
                train.StartingStation = firstZo;
            }

            if (lastDo is not null)
            {
                lastDo.IsInShortReport = true;
                train.EndingStation = lastDo;
            }

            trains.Add(train);
        }

        if (invalidTypes.Count != 0)
            throw new FormatException("Nasledujúce typy vlakov nie sú definované:\r\n    " +
                                      string.Join("\r\n    ", invalidTypes));

        SetVariants(trains);
        return trains;
    }

    /// <summary>
    ///     Pridelí variantu kazdemu naimportovanemu vlaku.
    /// </summary>
    /// <remarks>
    ///     Musi zbehnut vzdy - <see cref="VariantNotSet" /> je iba docasny sentinel a v modeli
    ///     by skoncil ako neplatna hodnota (editacia vlaku povoluje az od -1).
    ///     Uz definovane vlaky sa do skupin zaratavaju, aby varianty nekolidovali - ak import
    ///     doplni dalsi vlak k uz existujucemu, precisluje sa cela skupina vratane neho.
    /// </remarks>
    private void SetVariants(List<Train> imported)
    {
        var all = new List<Train>(imported.Count + (DefinedTrains?.Count ?? 0));
        if (DefinedTrains is not null)
            all.AddRange(DefinedTrains);
        all.AddRange(imported);

        foreach (var group in all.GroupBy(k => new { k.Number, k.Name, k.Type }))
        {
            var variants = group.ToList();

            //prepocitanie prekryvajucich sa datumovych obmedzeni si pyta pouzivatel,
            //samotne ocislovanie variant musi prebehnut tak ci tak
            if (ReorderTrains)
                Train.ReorderVariants(variants);
            else if (variants.Count == 1)
                variants[0].Variant = -1;
            else
                for (var i = 0; i < variants.Count; i++)
                    variants[i].Variant = i + 1;
        }
    }

    /// <summary>
    ///     Overi, ze vygenerovane datumove obmedzenie sa da rozparsovat spat.
    /// </summary>
    /// <remarks>
    ///     Ukladanie grafikonu (<c>TxtParser.WriteTrains</c>) prevadza <see cref="Train.DateLimitText" />
    ///     spat na bitove pole. Ked to zlyha, spadne az ulozenie - teda dlho po importe a s chybou,
    ///     ktora o vlaku nic nepovie. Radsej to zistime hned tu.
    /// </remarks>
    /// <exception cref="FormatException">ak sa text neda rozparsovat spat</exception>
    private static void VerifyDateLimit(Train train, DateLimit saveCheck)
    {
        try
        {
            saveCheck.TextToBitArray(train.DateLimitText);
        }
        catch (Exception e)
        {
            throw new FormatException(
                $"Vlak {train.Type} {train.Number} {train.Name} má dátumové obmedzenie, " +
                $"ktoré sa nedá spracovať: \"{train.DateLimitText}\".", e);
        }
    }

    private static Routing GetRouting(ElisTrain train)
    {
        if (train.StationsBefore.Count == 0)
            return Routing.Vychadzajuci;

        return train.StationsAfter.Count == 0 ? Routing.Konciaci : Routing.Prechadzajuci;
    }

    /// <summary>
    ///     Prida stanice do trasy. Nerozpoznane a vedome vynechane stanice sa preskocia,
    ///     rovnako ako opakovanie tej istej stanice bezprostredne za sebou - to vznika,
    ///     ked sa hranicny bod priradi k stanici, ktora uz v trase je.
    /// </summary>
    private void AddStations(IEnumerable<string> names, ICollection<Station> target)
    {
        Station previous = null;

        foreach (var name in names)
        {
            var station = ResolveMapped(name);
            if (station is null)
                continue;

            if (previous is not null && previous.ID == station.ID)
                continue;

            station.IsInLongReport = true;
            target.Add(station);
            previous = station;
        }
    }

    /// <summary>
    ///     Najde stanicu pre nazov z ELIS - najprv podla ulozeneho priradenia, potom automaticky.
    /// </summary>
    /// <returns><see langword="null" />, ak sa stanica nenasla alebo sa ma vynechat.</returns>
    private Station ResolveMapped(string name)
    {
        if (StationMap.TryGetValue(name, out var mapped))
            return mapped == TxtParser.ELIS_MAP_SKIP ? null : Station.GetFromID(mapped);

        return Resolve(name);
    }

    /// <summary>
    ///     Automaticky priradi nazov z ELIS k stanici grafikonu.
    /// </summary>
    /// <remarks>
    ///     ELIS pise niektore nazvy inak nez zvukova banka - skracuje "nad" na "n." a k
    ///     dvojjazycnym nazvom pridava druhy jazyk do zatvorky. Najprv sa skusa presna zhoda
    ///     (spravanie zvysku aplikacie), az potom porovnanie v kanonickom tvare.
    /// </remarks>
    /// <returns><see langword="null" />, ak sa stanica nenasla.</returns>
    public static Station Resolve(string name)
    {
        var exact = Station.GetFromName(name);
        if (exact is not null)
            return exact;

        var wanted = Canonical(name);
        if (wanted.Length == 0)
            return null;

        foreach (var station in AllStations())
            if (Canonical(station.Name) == wanted)
                return new Station(station.ID, station.Name);

        return null;
    }

    /// <summary>
    ///     Navrhne najblizsiu stanicu k nazvu z ELIS - pouziva sa ako predvolba v dialogu priradenia.
    /// </summary>
    /// <returns><see langword="null" />, ak sa nenaslo nic dost podobne.</returns>
    public static Station Suggest(string name)
    {
        var wanted = Canonical(name);
        if (wanted.Length < 4)
            return null;

        //hranicny bod nie je stanica - nenavrhujeme nic, predvolba bude "vynechat"
        if (IsBorderPoint(name))
            return null;

        Station best = null;
        var bestLength = 0;

        foreach (var station in AllStations())
        {
            var candidate = Canonical(station.Name);
            var common = CommonPrefixLength(wanted, candidate);

            //zhoda musi pokryvat vacsinu kratsieho z nazvov, inak je to nahoda
            if (common < 4 || common < Math.Min(wanted.Length, candidate.Length) * 3 / 4)
                continue;

            if (common > bestLength)
            {
                bestLength = common;
                best = station;
            }
        }

        return best is null ? null : new Station(best.ID, best.Name);
    }

    /// <summary>Vsetky stanice zo zvukovej banky aj pouzivatelom definovane.</summary>
    private static IEnumerable<Station> AllStations() => GlobData.Stations.Concat(GlobData.CustomStations);

    /// <summary>Ci nazov oznacuje statnu hranicu alebo iny technicky bod, nie stanicu.</summary>
    public static bool IsBorderPoint(string name) =>
        !string.IsNullOrEmpty(name) &&
        Regex.IsMatch(name, @"\bGr\b|\(\s*Gr\s*\)|\bšt\s*\.?\s*hr\b", RegexOptions.IgnoreCase);

    /// <summary>
    ///     Prevedie nazov stanice na tvar, v ktorom sa daju porovnavat nazvy z ELIS
    ///     a nazvy zo zvukovej banky - bez diakritiky, interpunkcie, medzier,
    ///     s rozvinutou skratkou "n." a bez alternativneho nazvu v zatvorke.
    /// </summary>
    private static string Canonical(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        //"Český Těšín (Czeski Cieszyn)" -> "Český Těšín"
        var text = Regex.Replace(name, @"\s*\([^)]*\)", string.Empty);

        //"Krásna n.Hornádom" -> "Krásna nad Hornádom"; "Ostrava hl.n." zostava nedotknute
        text = Regex.Replace(text, @"\bn\.\s*(?=\p{L})", "nad ");

        text = text.Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
        return Utils.RemoveDiacritics(text).ToLowerInvariant();
    }

    private static int CommonPrefixLength(string a, string b)
    {
        var max = Math.Min(a.Length, b.Length);
        var i = 0;
        while (i < max && a[i] == b[i])
            i++;

        return i;
    }

    private Operator GetOperator(ElisTrain train)
    {
        if (int.TryParse(train.OperatorNumber, out var id))
        {
            var byId = Operator.GetFromID(Operators, id);
            if (byId is not null)
                return byId;
        }

        return Operator.GetFromName(Operators, train.OperatorName) ?? Operator.None;
    }

    /// <summary>
    ///     Prevedie cas v minutach od polnoci na <see cref="DateTime" /> rovnako, ako to robi textovy parser.
    /// </summary>
    private static DateTime? ToTime(int minutes)
    {
        if (minutes == ElisTrain.NoTime)
            return null;

        var inDay = minutes % (24 * 60);
        return Utils.ParseTime($"{inDay / 60:00}:{inDay % 60:00}");
    }

    private static BitArray ToBitArray(string bits, int totalDays)
    {
        var result = new BitArray(totalDays);
        if (string.IsNullOrEmpty(bits))
            return result;

        for (var i = 0; i < totalDays && i < bits.Length; i++)
            result[i] = bits[i] == '1';

        return result;
    }
}
