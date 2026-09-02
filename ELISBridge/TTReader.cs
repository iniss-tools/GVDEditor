using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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
///     Vycita z dat ELIS vsetky vlaky prechadzajuce zadanou stanicou.
/// </summary>
internal sealed class TTReader
{
    private readonly string _dataPath;
    private readonly string _registrationNumber;
    private readonly string _client;

    /// <summary>Inicializuje citac nad priecinkom s datami (.tt subormi).</summary>
    /// <param name="dataPath">Priecinok s datami, typicky <c>&lt;instalacia&gt;\Data1</c>.</param>
    /// <param name="registrationNumber">Registracne cislo pre platene poriadky, alebo <see langword="null" />.</param>
    /// <param name="client">Identifikacia klienta, ak ju platene data vyzaduju, alebo <see langword="null" />.</param>
    public TTReader(string dataPath, string registrationNumber = null, string client = null)
    {
        if (!Directory.Exists(dataPath))
            throw new DirectoryNotFoundException($"Priečinok s dátami neexistuje: {dataPath}");

        // TTInit vnutri iba zretazi cestu s maskou *.tt, oddelovac nedoplna
        _dataPath = dataPath.EndsWith("\\", StringComparison.Ordinal) ? dataPath : dataPath + "\\";
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

            throw new InvalidOperationException($"V priečinku {_dataPath} sa nenašiel žiadny použiteľný cestovný poriadok.");
        }

        // nacital sa aspon jeden poriadok, ale niektory iny bol kvoli registracii zahodeny
        if (TTNative.IsRegistrationError(error))
            Console.Error.WriteLine("Upozornenie: niektoré cestovné poriadky boli vynechané, " +
                                    "lebo vyžadujú platné registračné číslo.");
    }

    /// <summary>Vrati nazvy vsetkych stanic vo vsetkych nacitanych poriadkoch.</summary>
    public List<string> GetAllStationNames()
    {
        var names = new List<string>();
        for (var tt = 0; tt < TTNative.TTTTCount(); tt++)
        for (var st = 0; st < TTNative.TTStCount(tt); st++)
            names.Add(TTNative.Str(TTNative.TTStName(tt, st)));

        names.Sort(StringComparer.CurrentCulture);
        return names;
    }

    /// <summary>
    ///     Vycita vsetky vlaky prechadzajuce stanicou <paramref name="stationName" />.
    /// </summary>
    /// <param name="stationName">Nazov stanice; porovnava sa bez diakritiky, bodiek a pomlciek.</param>
    /// <exception cref="ArgumentException">ak sa stanica v datach nenajde</exception>
    public ElisResult Read(string stationName)
    {
        GetValidity(out var validFrom, out var validTo);
        var totalDays = (validTo - validFrom).Days + 1;

        var result = new ElisResult
        {
            DataPath = _dataPath,
            StationName = stationName,
            ValidFrom = validFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ValidTo = validTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            TotalDays = totalDays
        };

        var found = false;
        for (var tt = 0; tt < TTNative.TTTTCount(); tt++)
        {
            var stations = ReadStationNames(tt);
            var myStation = IndexOfStation(stations, stationName);
            if (myStation < 0)
                continue;

            found = true;
            result.StationName = stations[myStation];
            ReadTrains(tt, myStation, stations, validFrom, totalDays, result.Trains);
        }

        if (!found)
            throw new ArgumentException($"Stanica \"{stationName}\" sa v dátach ELIS nenachádza.", nameof(stationName));

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
        DateTime validFrom, int totalDays, ICollection<ElisTrain> output)
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
        TTNative.TTTrainInfo(tt, TTNative.DefaultLang, tr,
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
            train.StationsBefore.Add(NameOf(stations, stopStation[i]));
        for (var i = position + 1; i < count; i++)
            train.StationsAfter.Add(NameOf(stations, stopStation[i]));

        var owner = TTNative.TTTrOwner(tt, tr);
        if (owner >= 0 && owner < owners.Count)
        {
            train.OperatorName = owners[owner].Name;
            train.OperatorNumber = owners[owner].Number;
        }

        var line = TTNative.TTTrLine(tt, tr);
        train.Line = line >= 0 ? TTNative.Str(TTNative.TTLineDesc(tt, TTNative.DefaultLang, line)) : string.Empty;

        train.RunsBits = ReadRunsBits(tt, tr, myStation, validFrom, totalDays);
        return train;
    }

    /// <summary>Zostavi datumove obmedzenie priamo z TT.dll - bez parsovania textovej poznamky.</summary>
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