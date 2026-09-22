using System.Globalization;
using System.Text;

namespace Iniss.Elis;

/// <summary>
///     x86 host nad TT.dll. GVDEditor bezi ako 64-bitovy proces a 32-bitovu TT.dll
///     zavolat priamo nemoze, preto s nou hovori cez tento pomocny program.
/// </summary>
internal static class Program
{
    private const string DefaultApp = @"C:\Program Files (x86)\Cestovné poriadky";

    private const string Usage =
        $"""
         ELISBridge - vycitanie grafikonu z dat programu ELIS (Cestovne poriadky, CHAPS)
                                    
           ELISBridge.exe (--code <SR70> | --station <nazov>) [--app <priecinok>] [--data <priecinok>] [--out <subor>]
           ELISBridge.exe --list-stations [--app <priecinok>] [--data <priecinok>]
           ELISBridge.exe --station-codes [--out <subor>] [--app <priecinok>] [--data <priecinok>]

         --code           cislo stanice (SR70), pre ktoru sa vlaky vycitaju - jednoznacne
         --station        nazov stanice; pouzije sa, ak --code chyba alebo sa v datach nenasiel
         --app            priecinok s TT.dll (predvolene \"{DefaultApp}\")
         --data           priecinok s .tt datami (predvolene <app>\\Data1)
         --out            subor pre vystup (bez neho ide na standardny vystup)
         --reg            registracne cislo pre platene cestovne poriadky
         --client         identifikacia klienta, ak ju platene data vyzaduju
         --list-stations  vypise nazvy vsetkych stanic v datach
         --station-codes  vypise stanice ako <kod SR70>,"<nazov>", zoradene podla nazvu;
                          do suboru (--out) sa zapisu v kodovani Windows-1250
         --version        vypise verziu TT.dll a ELISBridge.exe

         Navratove kody: 0 = ok, 1 = chyba, 2 = ziadny cestovny poriadok,
                         3 = stanica nenajdena, 4 = chybne/chybajuce registracne cislo
         """;

    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string? app = null, data = null, station = null, output = null, reg = null, client = null;
        var code = 0;
        var listStations = false;
        var stationCodes = false;
        var showVersion = false;

        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "--app": 
                    app = Next(args, ref i); 
                    break;
                case "--data": 
                    data = Next(args, ref i); 
                    break;
                case "--station":
                    station = Next(args, ref i);
                    break;
                case "--code":
                    var text = Next(args, ref i);
                    if (!int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out code) || code <= 0)
                    {
                        Console.Error.WriteLine($"Prepínač --code vyžaduje kladné celé číslo, nie \"{text}\".");
                        return 1;
                    }
                    break;
                case "--out": 
                    output = Next(args, ref i); 
                    break;
                case "--reg": 
                    reg = Next(args, ref i); 
                    break;
                case "--client": 
                    client = Next(args, ref i); 
                    break;
                case "--list-stations": 
                    listStations = true; 
                    break;
                case "--station-codes": 
                    stationCodes = true; 
                    break;
                case "--version":
                    showVersion = true;
                    break;
                case "--help":
                case "-h":
                    Console.WriteLine(Usage);
                    return 0;
                default:
                    Console.Error.WriteLine($"Neznámy prepínač: {args[i]}");
                    Console.Error.WriteLine(Usage);
                    return 1;
            }

        if (string.IsNullOrEmpty(app))
            app = DefaultApp;
        if (string.IsNullOrEmpty(data))
            data = Path.Combine(app, "Data1");

        if (showVersion)
            return WriteVersion(app);

        if (!listStations && !stationCodes && code == 0 && string.IsNullOrEmpty(station))
        {
            Console.Error.WriteLine("Chýba parameter --code alebo --station.");
            Console.Error.WriteLine(Usage);
            return 1;
        }

        try
        {
            TTNative.LoadFrom(app);
            var reader = new TTReader(data, reg, client);
            reader.Open();

            if (listStations)
            {
                foreach (var name in reader.GetAllStationNames())
                    Console.WriteLine(name);

                return 0;
            }

            if (stationCodes)
            {
                WriteStationCodes(reader, output);
                return 0;
            }

            var result = reader.Read(code, station);

            if (string.IsNullOrEmpty(output))
                WriteToConsole(result);
            else
                result.Save(output);

            Console.Error.WriteLine($"Vlakov v stanici {result.StationName}: {result.Trains.Count}" +
                                    $" (platnosť {result.ValidFrom} - {result.ValidTo})");
            return 0;
        }
        catch (RegistrationException e)
        {
            Console.Error.WriteLine(e.Message);
            return 4;
        }
        catch (InvalidOperationException e)
        {
            Console.Error.WriteLine(e.Message);
            return 2;
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine(e.Message);
            return 3;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.ToString());
            return 1;
        }
    }

    /// <summary>
    ///     Vypise verziu ELISBridge a TT.dll. Kniznica sa musi najprv zaviest cez
    ///     <see cref="TTNative.LoadFrom" /> - bez toho ju P/Invoke hlada len vedla .exe a v PATH.
    /// </summary>
    private static int WriteVersion(string app)
    {
        Console.WriteLine($"ELISBridge.exe: {typeof(Program).Assembly.GetName().Version}");

        try
        {
            TTNative.LoadFrom(app);
            Console.WriteLine($"TT.dll: {TTNative.Str(TTNative.TTVer())} ({Path.Combine(app, "TT.dll")})");
            return 0;
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException or DllNotFoundException)
        {
            Console.Error.WriteLine(e.Message);
            return 1;
        }
    }

    /// <summary>
    ///     Zapise ciselnik stanic vo formate <c>5613600,"Košice"</c> - jedna stanica na riadok.
    /// </summary>
    /// <param name="output">Cielovy subor, alebo <see langword="null" /> pre standardny vystup.</param>
    private static void WriteStationCodes(TTReader reader, string? output)
    {
        var stations = reader.GetStationCodes(out var skipped);
        var lines = stations.Select(s => $"{s.Code},\"{s.Name.Replace("\"", "\"\"")}\"");

        if (string.IsNullOrEmpty(output))
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
        else
        {
            var cp1250 = Encoding.GetEncoding(1250);
            File.WriteAllLines(output, lines, cp1250);

            //co sa do Windows-1250 nezmesti, ulozi sa ako '?' - nech to nezostane nepovsimnute
            foreach (var name in stations.Select(s => s.Name)
                         .Where(n => cp1250.GetString(cp1250.GetBytes(n)) != n))
                Console.Error.WriteLine($"Upozornenie: názov \"{name}\" sa v kódovaní Windows-1250 zapísať nedá.");
        }

        Console.Error.WriteLine($"Staníc s kódom: {stations.Count}" +
                                (skipped != 0 ? $", bez kódu vynechaných: {skipped}" : string.Empty));
    }

    private static void WriteToConsole(ElisResult result)
    {
        var temp = Path.GetTempFileName();
        try
        {
            result.Save(temp);
            Console.Out.Write(File.ReadAllText(temp, Encoding.UTF8));
        }
        finally
        {
            File.Delete(temp);
        }
    }

    private static string Next(string[] args, ref int i)
    {
        return i + 1 >= args.Length 
            ? throw new ArgumentException($"Prepínač {args[i]} vyžaduje hodnotu.") 
            : args[++i];
    }
}