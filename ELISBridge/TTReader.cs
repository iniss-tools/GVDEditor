using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Iniss.Elis;

/// <summary>
///     Vyhodena, ked platene data vyzaduju registraciu a tu nebolo mozne vykonat
///     (chybne alebo chybajuce registracne cislo).
/// </summary>
internal sealed class RegistrationException : Exception
{
    public RegistrationException(string message) : base(message) { }
}

/// <summary>
///     Stanica aj s jej cislom v ciselniku dopravcu (pre zeleznicu kod SR70).
/// </summary>
/// <param name="Code">Cislo stanice, napr. 5613600.</param>
/// <param name="Name">Nazov stanice tak, ako ho uvadza ELIS.</param>
internal readonly record struct ElisStationCode(int Code, string Name);

/// <summary>
///     Vycita z dat ELIS vsetky vlaky prechadzajuce zadanou stanicou.
/// </summary>
internal sealed partial class TTReader
{
    /// <summary>
    ///     Poznamka vlaku s linkou integrovaneho dopravneho systemu, napr.
    ///     <c>linka R2 [IDS PLUS] (Žilina-&gt;Čadca)</c>.
    /// </summary>
    [GeneratedRegex(@"^linka (\S+) \[([^\]]+)\] \((.+)->(.+)\)$")]
    private static partial Regex IdsLineRegex();

    private static Regex IdsLine => IdsLineRegex();

    private readonly string _dataPath;
    private readonly string? _registrationNumber;
    private readonly string? _client;

    /// <summary>Inicializuje citac nad priecinkom s datami (.tt subormi).</summary>
    /// <param name="dataPath">Priecinok s datami, typicky <c>&lt;instalacia&gt;\Data1</c>.</param>
    /// <param name="registrationNumber">Registracne cislo pre platene poriadky, alebo <see langword="null" />.</param>
    /// <param name="client">Identifikacia klienta, ak ju platene data vyzaduju, alebo <see langword="null" />.</param>
    public TTReader(string dataPath, string? registrationNumber = null, string? client = null)
    {
        if (!Directory.Exists(dataPath))
            throw new DirectoryNotFoundException($"Priečinok s dátami neexistuje: {dataPath}");

        // TTInit vnutri iba zretazi cestu s maskou *.tt, oddelovac nedoplna
        _dataPath = dataPath.EndsWith('\\') ? dataPath : dataPath + "\\";
        _registrationNumber = registrationNumber;
        _client = client;
    }

    /// <summary>Zavedie cestovne poriadky.</summary>
    /// <exception cref="InvalidOperationException">
    ///     ak sa nenacital ziadny cestovny poriadok - vratane pripadu, ked platene data
    ///     vyzaduju registraciu a ta zlyhala (chybne/chybajuce cislo).
    /// </exception>
    public void Open()
    {
        // registracia MUSI byt pred TTInit - overenie prebieha pocas nacitania kazdeho .tt
        TTNative.Register(_registrationNumber, _client);

        TTNative.TTInit(_dataPath, IntPtr.Zero);
        var count = TTNative.TTTTCount();
        var error = TTNative.TTError();

        if (count <= 0)
        {
            if (TTNative.IsRegistrationError(error))
                throw new RegistrationException(string.IsNullOrWhiteSpace(_registrationNumber)
                    ? "Dáta vyžadujú registračné číslo, ktoré nebolo zadané."
                    : "Zadané registračné číslo nie je pre tieto dáta platné.");

            var detail = error != 0 ? $" (TT.dll: {ErrorText(error)})" : string.Empty;
            throw new InvalidOperationException(
                $"V priečinku {_dataPath} sa nenašiel žiadny použiteľný cestovný poriadok{detail}.");
        }

        // nacital sa aspon jeden poriadok, ale niektory iny bol kvoli registracii zahodeny
        if (TTNative.IsRegistrationError(error))
            Console.Error.WriteLine("Upozornenie: niektoré cestovné poriadky boli vynechané, " +
                                    "lebo vyžadujú platné registračné číslo.");
    }

    /// <summary>Text chyby TT.dll; ak kniznica k danemu kodu text nema, vrati aspon kod.</summary>
    private static string ErrorText(int error)
    {
        var text = TTNative.Str(TTNative.TTErrorText(error, TTNative.DefaultLang));
        return string.IsNullOrWhiteSpace(text) ? $"kód {error}" : $"{text} [{error}]";
    }

    /// <summary>Vrati nazvy vsetkych stanic vo vsetkych nacitanych poriadkoch.</summary>
    public static List<string> GetAllStationNames()
    {
        var names = new List<string>();
        for (var tt = 0; tt < TTNative.TTTTCount(); tt++)
        for (var st = 0; st < TTNative.TTStCount(tt); st++)
            names.Add(TTNative.Str(TTNative.TTStName(tt, st)));

        names.Sort(StringComparer.CurrentCulture);
        return names;
    }

    /// <summary>
    ///     Vrati stanice aj s ich cislom v ciselniku (pre zeleznicne poriadky kod SR70),
    ///     zoradene podla nazvu. Stanice bez cisla vynechava.
    /// </summary>
    /// <param name="skipped">Pocet stanic, ktore ziadne cislo nemaju.</param>
    public static List<ElisStationCode> GetStationCodes(out int skipped)
    {
        skipped = 0;
        var codes = new List<ElisStationCode>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var tt = 0; tt < TTNative.TTTTCount(); tt++)
        for (var st = 0; st < TTNative.TTStCount(tt); st++)
        {
            var name = TTNative.Str(TTNative.TTStName(tt, st));
            if (string.IsNullOrWhiteSpace(name) || !seen.Add(name))
                continue;

            var code = TTNative.TTStKey(tt, st);
            if (code == 0)
            {
                skipped++;
                continue;
            }

            codes.Add(new ElisStationCode(code, name));
        }

        codes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        return codes;
    }

    /// <summary>
    ///     Vycita vsetky vlaky prechadzajuce zadanou stanicou.
    /// </summary>
    /// <param name="stationCode">
    ///     Cislo stanice (SR70) - ak je kladne, hlada sa najprv podla neho a je to jednoznacne.
    /// </param>
    /// <param name="stationName">
    ///     Nazov stanice; pouzije sa, ak sa podla cisla nic nenaslo. Porovnava sa bez
    ///     diakritiky, bodiek a pomlciek.
    /// </param>
    /// <exception cref="ArgumentException">ak sa stanica v datach nenajde</exception>
    public ElisResult Read(int stationCode, string? stationName)
    {
        GetValidity(out var validFrom, out var validTo);
        var totalDays = (validTo - validFrom).Days + 1;

        var result = new ElisResult
        {
            DataPath = _dataPath,
            StationName = stationName ?? string.Empty,
            ValidFrom = validFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ValidTo = validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            TotalDays = totalDays
        };

        var found = false;
        for (var tt = 0; tt < TTNative.TTTTCount(); tt++)
        {
            var stations = ReadStationNames(tt);

            var myStation = stationCode > 0 ? TTNative.TTSearchStKey(tt, stationCode) : -1;
            if (myStation < 0 && !string.IsNullOrEmpty(stationName))
                myStation = IndexOfStation(stations, stationName);
            if (myStation < 0)
                continue;

            found = true;
            result.StationName = stations[myStation];
            result.StationCode = TTNative.TTStKey(tt, myStation);
            ReadTrains(tt, myStation, stations, validFrom, totalDays, result.Trains);
        }

        if (!found)
        {
            var wanted = (stationCode > 0, string.IsNullOrEmpty(stationName)) switch
            {
                (true, true) => $"s číslom {stationCode}",
                (true, false) => $"\"{stationName}\" ({stationCode})",
                _ => $"\"{stationName}\""
            };
            throw new ArgumentException($"Stanica {wanted} sa v dátach ELIS nenachádza.", nameof(stationName));
        }

        return result;
    }

    /// <summary>Precita rozsah platnosti cestovneho poriadku.</summary>
    private static void GetValidity(out DateTime from, out DateTime to)
    {
        var f = new int[3];
        var t = new int[3];
        TTNative.TTGetValidityRange(0, 0, f, t);

        // zlozky su v poradi den, mesiac, rok
        from = new DateTime(f[2], f[1], f[0]);
        to = new DateTime(t[2], t[1], t[0]);
    }

    private static List<string> ReadStationNames(int tt)
    {
        var count = TTNative.TTStCount(tt);
        var names = new List<string>(count);
        for (var st = 0; st < count; st++)
            names.Add(TTNative.Str(TTNative.TTStName(tt, st)));

        return names;
    }

    private static int IndexOfStation(List<string> stations, string wanted)
    {
        var normalized = Normalize(wanted);
        for (var i = 0; i < stations.Count; i++)
            if (Normalize(stations[i]) == normalized)
                return i;

        return -1;
    }

    /// <summary>Zhoduje sa s porovnavanim v <c>Station.GetFromName</c> - bez diakritiky, bodiek a pomlciek.</summary>
    private static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var stripped = text.Replace(".", string.Empty).Replace("-", string.Empty).ToLowerInvariant();
        var decomposed = stripped.Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder(decomposed.Length);
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        }

        return builder.ToString();
    }

    private static void ReadTrains(int tt, int myStation, List<string> stations,
        DateTime validFrom, int totalDays, List<ElisTrain> output)
    {
        var owners = ReadOwners(tt);
        var trainCount = TTNative.TTTrCount(tt);

        for (var tr = 0; tr < trainCount; tr++)
        {
            var count = TTNative.TTTrainRouteExt(tt, tr, out var route, 1);
            if (count <= 0 || route == IntPtr.Zero)
                continue;

            // buffer trasy je zdielany globál - vycitat treba pred akymkolvek dalsim volanim
            var stopStation = new int[count];
            var stopArrival = new int[count];
            var stopDeparture = new int[count];
            var position = -1;

            for (var i = 0; i < count; i++)
            {
                var item = route + i * TTNative.RouteItemSize;
                stopStation[i] = (ushort)Marshal.ReadInt16(item, 0);
                stopArrival[i] = ToMinutes((ushort)Marshal.ReadInt16(item, 2));
                stopDeparture[i] = ToMinutes((ushort)Marshal.ReadInt16(item, 4));

                if (stopStation[i] == myStation)
                    position = i;
            }

            if (position < 0)
                continue;

            output.Add(BuildTrain(tt, tr, position, count, stopStation, stopArrival, stopDeparture,
                stations, owners, myStation, validFrom, totalDays));
        }
    }

    private static ElisTrain BuildTrain(int tt, int tr, int position, int count,
        int[] stopStation, int[] stopArrival, int[] stopDeparture,
        List<string> stations, IList<OwnerInfo> owners, int myStation,
        DateTime validFrom, int totalDays)
    {
        //cislo/nazov/typ platne prave v nasej stanici - nie globalne (medzistatne vlaky
        //maju v kazdej sieti ine cislo, niektore menia po trase nazov ci typ)
        TTNative.TTTrainStationInfo(tt, TTNative.DefaultLang, tr, myStation,
            out var pNumber, out var pName, out var pType, out _);

        var train = new ElisTrain
        {
            Number = TTNative.Str(pNumber),
            Name = TTNative.Str(pName),
            Type = TTNative.Str(pType),
            ArrivalMinutes = stopArrival[position],
            DepartureMinutes = stopDeparture[position]
        };

        for (var i = 0; i < position; i++)
            train.StationsBefore.Add(StopOf(tt, stations, stopStation[i]));
        for (var i = position + 1; i < count; i++)
            train.StationsAfter.Add(StopOf(tt, stations, stopStation[i]));

        var owner = TTNative.TTTrOwner(tt, tr);
        if (owner >= 0 && owner < owners.Count)
        {
            train.OperatorName = owners[owner].Name;
            train.OperatorNumber = owners[owner].Number;
        }

        ReadLines(tt, tr, position, count, stopStation, stations, train);

        train.RunsBits = ReadRunsBits(tt, tr, myStation, validFrom, totalDays);
        return train;
    }

    /// <summary>
    ///     Doplni vlaku linku IDS a traťové číslo, oddelene pre prichod a odchod - vlak moze
    ///     do stanice prist po jednej trati a odist po inej.
    /// </summary>
    private static void ReadLines(int tt, int tr, int position, int count,
        int[] stopStation, List<string> stations, ElisTrain train)
    {
        //traťové čísla: retazec trojic "cislo:odKodu:doKodu"
        var parts = TTNative.Str(TTNative.TTTrLines(tt, tr, -1, -1, 0)).Split(':');
        for (var i = 0; i + 2 < parts.Length; i += 3)
        {
            var from = IndexOfCode(tt, stopStation, count, parts[i + 1]);
            var to = IndexOfCode(tt, stopStation, count, parts[i + 2]);
            Assign(parts[i], from, to, position,
                v => train.RailLineArrival = v, v => train.RailLineDeparture = v);
        }

        //linka IDS: hotovy text poznamky, napr. "linka R2 [IDS PLUS] (Žilina->Čadca)"
        for (var r = 0; r < TTNative.TTTrRem1Count(tt, tr); r++)
        {
            var match = IdsLine.Match(TTNative.Str(TTNative.TTTrRem1(tt, TTNative.DefaultLang, tr, r)));
            if (!match.Success)
                continue;

            var from = IndexOfName(stopStation, count, stations, match.Groups[3].Value);
            var to = IndexOfName(stopStation, count, stations, match.Groups[4].Value);
            var applied = Assign(match.Groups[1].Value, from, to, position,
                v => train.LineArrival = v, v => train.LineDeparture = v);

            if (applied)
                train.LineSystem = match.Groups[2].Value;
        }
    }

    /// <summary>
    ///     Priradi linku prichodu a/alebo odchodu podla toho, kde na useku
    ///     &lt;<paramref name="from" />, <paramref name="to" />&gt; lezi nasa stanica.
    /// </summary>
    /// <returns><see langword="true" />, ak sa linka niekam priradila.</returns>
    private static bool Assign(string line, int from, int to, int position,
        Action<string> setArrival, Action<string> setDeparture)
    {
        if (string.IsNullOrEmpty(line) || from < 0 || to < 0 || from >= to)
            return false;

        //do stanice sa po tejto trati prichadza, ak usek v nej alebo za nou konci
        var arrival = position > from && position <= to;
        var departure = position >= from && position < to;

        if (arrival)
            setArrival(line);
        if (departure)
            setDeparture(line);

        return arrival || departure;
    }

    /// <summary>Poradie zastavky s danym cislom stanice (SR70) na trase vlaku, alebo -1.</summary>
    private static int IndexOfCode(int tt, int[] stopStation, int count, string code)
    {
        if (!int.TryParse(code, NumberStyles.None, CultureInfo.InvariantCulture, out var key) || key <= 0)
            return -1;

        for (var i = 0; i < count; i++)
            if (TTNative.TTStKey(tt, stopStation[i]) == key)
                return i;

        return -1;
    }

    /// <summary>Poradie zastavky s danym nazvom na trase vlaku, alebo -1.</summary>
    private static int IndexOfName(int[] stopStation, int count, List<string> stations, string name)
    {
        var wanted = Normalize(name);
        for (var i = 0; i < count; i++)
            if (Normalize(NameOf(stations, stopStation[i])) == wanted)
                return i;

        return -1;
    }

    /// <summary>Zostavi datumove obmedzenie priamo z TT.dll - bez parsovania textovej poznamky.</summary>
    [SuppressMessage("Performance", "CA1806:Do not ignore method results")]
    private static string ReadRunsBits(int tt, int tr, int myStation, DateTime validFrom, int totalDays)
    {
        var bits = new StringBuilder(totalDays);
        for (var i = 0; i < totalDays; i++)
        {
            var day = validFrom.AddDays(i);
            var runs = TTNative.TTTrainRuns(tt, tr, day.Day, day.Month, day.Year, myStation, 0);
            bits.Append(runs != 0 ? '1' : '0');
        }

        TTNative.TTError(); // vycistenie pripadneho kodu 18 (datum mimo rozsahu)
        
        return bits.ToString();
    }

    /// <summary>Zastavka trasy aj s cislom stanice (0, ak ho ELIS nema).</summary>
    private static ElisStop StopOf(int tt, List<string> stations, int index) => new()
    {
        Name = NameOf(stations, index),
        Code = index >= 0 && index < stations.Count ? TTNative.TTStKey(tt, index) : 0
    };

    private static string NameOf(List<string> stations, int index)
        => index >= 0 && index < stations.Count ? stations[index] : index.ToString(CultureInfo.InvariantCulture);

    private static int ToMinutes(ushort raw) => raw == TTNative.NoTime ? ElisTrain.NoTime : raw;

    private static List<OwnerInfo> ReadOwners(int tt)
    {
        var count = TTNative.TTGetOwnersCount(tt);
        var owners = new List<OwnerInfo>(count);

        for (var i = 0; i < count; i++)
        {
            var desc = TTNative.TTOwnerDesc(tt, TTNative.DefaultLang, i);
            owners.Add(new OwnerInfo
            {
                Name = TTNative.Str(TTNative.TTGetField("ON", desc)),
                Number = TTNative.Str(TTNative.TTGetField("ONo", desc))
            });
        }

        return owners;
    }

    private struct OwnerInfo
    {
        public string Name;
        public string Number;
    }
}