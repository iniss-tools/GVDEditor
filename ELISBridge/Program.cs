using System;
using System.IO;
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
                                    
           ELISBridge.exe --station <nazov> [--app <priecinok>] [--data <priecinok>] [--out <subor>]
           ELISBridge.exe --list-stations [--app <priecinok>] [--data <priecinok>]
           
         --station        nazov stanice, pre ktoru sa vlaky vycitaju
         --app            priecinok s TT.dll (predvolene \"{DefaultApp}\")
         --data           priecinok s .tt datami (predvolene <app>\\Data1)
         --out            subor pre vysledne XML (bez neho ide na standardny vystup)
         --reg            registracne cislo pre platene cestovne poriadky
         --client         identifikacia klienta, ak ju platene data vyzaduju
         --list-stations  vypise nazvy vsetkych stanic v datach
         
         Navratove kody: 0 = ok, 1 = chyba, 2 = ziadny cestovny poriadok,
                         3 = stanica nenajdena, 4 = chybne/chybajuce registracne cislo
         """;

    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string app = null, data = null, station = null, output = null, reg = null, client = null;
        var listStations = false;

        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "--app": app = Next(args, ref i); break;
                case "--data": data = Next(args, ref i); break;
                case "--station": station = Next(args, ref i); break;
                case "--out": output = Next(args, ref i); break;
                case "--reg": reg = Next(args, ref i); break;
                case "--client": client = Next(args, ref i); break;
                case "--list-stations": listStations = true; break;
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

        if (!listStations && string.IsNullOrEmpty(station))
        {
            Console.Error.WriteLine("Chýba parameter --station.");
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

            var result = reader.Read(station);

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