using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using Iniss.Elis;
using ToolsCore.XML;

namespace GVDEditor.Integration;

/// <summary>
/// Volby importu vlakov z ELIS zadane pouzivatelom.
/// </summary>
internal sealed class ElisImportOptions
{
    /// <summary>
    /// Predvolene umiestnenie aplikacie Cestovne poriadky.
    /// </summary>
    public const string DefaultElisDirectory = @"C:\Program Files (x86)\Cestovné poriadky";

    public string AppDirectory { get; init; } = null!;
    public string RegistrationNumber { get; init; } = null!;
    public bool OmitPassingTrains { get; init; }
    public bool ReorderTrains { get; init; }

    /// <summary>
    /// Ci sa maju povodne vlaky pred pridanim naimportovanych odstranit.
    /// </summary>
    public bool ReplaceTrains { get; init; }
}

/// <summary>
/// Medzivysledok importu z ELIS - nacitane data a stanice, ktore treba priradit rucne.
/// </summary>
internal sealed class ElisImport
{
    public required ELISBridgeClient Client { get; init; }
    public required ElisResult Data { get; init; }
    public required List<string> Unresolved { get; init; }
    public required string GVDPath { get; init; }
    public required GVDInfo GVDInfo { get; init; }

    /// <summary>
    /// Ci sa maju povodne vlaky pred pridanim naimportovanych odstranit. Nesie sa spolu s vysledkom - stav
    /// okolo asynchronneho behu sa lahko strati a chyba by sa prejavila az tichym nenahradenim vlakov.
    /// </summary>
    public bool ReplaceTrains { get; init; }
}

/// <summary>
/// Import vlakov z programu ELIS (Cestovne poriadky): nacitanie dat na pozadi, priradenie stanic a prevod na vlaky.
/// </summary>
internal static class ElisImportService
{
    /// <summary>
    /// Prva faza importu na pozadi - nacita data z ELIS a zisti, ktore stanice sa nepodarilo priradit. Pouzivatela sa
    /// tu nic nepyta; nepriradene stanice doriesi <see cref="SaveStationMap" /> a prevod <see cref="Convert" />.
    /// </summary>
    /// <param name="options">Volby importu.</param>
    /// <param name="gvdDir">Grafikon, do ktoreho sa importuje.</param>
    /// <param name="types">Typy vlakov instalacie.</param>
    /// <param name="operators">Dopravcovia grafikonu.</param>
    /// <param name="track">Kolaj pre vlaky bez kolaje.</param>
    /// <param name="existingTrains">Vlaky grafikonu, ktore ostanu (vstupuju do cislovania variant).</param>
    /// <param name="stations">Stanice zvukovej banky a grafikonu.</param>
    public static Task<ElisImport> LoadAsync(ElisImportOptions options, GVDDirectory gvdDir, IReadOnlyList<TrainType> types,
        IReadOnlyList<Operator> operators, Track track, IReadOnlyList<Train> existingTrains, StationDirectory stations)
    {
        // zoznamy sa skopiruju este na UI vlakne - pocas importu ich okno moze menit
        var typeList = types.ToList();
        var operatorList = operators.ToList();
        var trains = options.ReplaceTrains ? new List<Train>() : existingTrains.ToList();
        var gvdPath = gvdDir.Dir.FullPath;
        var gvd = gvdDir.GVD;

        return Task.Run(() =>
        {
            var client = new ELISBridgeClient(typeList, operatorList, gvd, track, stations)
            {
                AppDirectory = options.AppDirectory,
                RegistrationNumber = options.RegistrationNumber,
                DefinedTrains = trains,
                OmitPassingTrains = options.OmitPassingTrains,
                ReorderTrains = options.ReorderTrains,
                StationMap = ElisMapFile.Read(gvdPath)
            };

            var data = client.LoadData();
            return new ElisImport
            {
                Client = client,
                Data = data,
                Unresolved = client.FindUnresolvedStations(data),
                GVDPath = gvdPath,
                GVDInfo = gvd,
                ReplaceTrains = options.ReplaceTrains
            };
        });
    }

    /// <summary>
    /// Prida rucne priradene stanice a priradenie ulozi do grafikonu, aby sa nabuduce nepytalo znova.
    /// </summary>
    /// <returns>chyba zapisu (import moze pokracovat), inak <see langword="null" />.</returns>
    public static Exception? SaveStationMap(ElisImport import, IReadOnlyDictionary<string, string> resolved, AppLanguage language)
    {
        foreach (var (elis, station) in resolved)
            import.Client.StationMap[elis] = station;

        try
        {
            ElisMapFile.Write(import.GVDPath, import.Client.StationMap, import.GVDInfo, language);
            return null;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Prevedie nacitane data na vlaky grafikonu.
    /// </summary>
    public static List<Train> Convert(ElisImport import) => import.Client.Convert(import.Data);
}
