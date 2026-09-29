using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static GVDEditor.Formats.GvdFileConsts;
using static ToolsCore.Iniss.Tools.PathUtils;

namespace GVDEditor.Formats;

/// <summary>
/// Vlaky grafikonu - suhrn suborov Export3A/B/C, Vzory, StaHlasB/C, Vlaky, Pozice, Doplnky, Foreign, Vyluka a Mos.
/// Kazdy subor ma vlastnu triedu, tu sa len cita a zapisuje v spravnom poradi.
/// </summary>
internal static class TrainsFile
{
    /// <summary>
    /// Nacita vlaky grafikonu.
    /// </summary>
    /// <param name="path">cesta do priecinka grafikonu</param>
    /// <param name="context">instalacia INISS a nacitavany grafikon (kolaje, dopravcovia, typy hlaseni, radenia)</param>
    /// <returns>vlaky v poradi podla ID</returns>
    public static List<Train> Read(string path, GrafikonContext context)
    {
        var trains = Export3File.ReadTrains(FileIn(path, FILE_EXPORT3A), context);
        var dayMaps = Export3File.ReadValidity(FileIn(path, FILE_EXPORT3B), trains);
        Export3File.ReadNotes(FileIn(path, FILE_EXPORT3C), trains, dayMaps);

        var templates = RoutesFile.ReadTemplates(FileIn(path, FILE_VZORY), FileIn(path, FILE_STAHLASB), FileIn(path, FILE_STAHLASC), context);
        RoutesFile.AssignRoutes(FileIn(path, FILE_VLAKY), templates, trains, InfoGvdFile.Read(path), context.Workspace.TrainsTypes, context.Warnings);

        PoziceFile.Read(FileIn(path, FILE_POZICE), trains, context.Document.Tracks);
        DoplnkyFile.Read(FileIn(path, FILE_DOPLNKY), trains, context);
        ForeignFile.Read(FileIn(path, FILE_FOREIGN), trains, context.Workspace.Languages);
        VylukaFile.Read(FileIn(path, FILE_VYLUKA), trains);
        MosFile.Read(FileIn(path, FILE_MOS), trains);

        foreach (var train in trains)
            train.Radenia.AddRange(context.Document.Radenia.Where(radenie => train.Number == radenie.CisloVlaku));

        return trains;
    }

    /// <summary>
    /// Zapise vlaky grafikonu.
    /// </summary>
    /// <param name="path">cesta do priecinka grafikonu</param>
    /// <param name="trains">vlaky v poradi podla ID</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="context">instalacia INISS, dokument grafikonu (typy a varianty hlaseni, vlastne stanice) a jazyk hlaviciek</param>
    public static void Write(string path, IList<Train> trains, GVDInfo gvd, GrafikonContext context)
    {
        IEnumerable<string> Comments(string file) => GenerateComment(path, file, gvd, context.CommentLanguage);

        Export3File.WriteTrains(FileIn(path, FILE_EXPORT3A), trains, gvd, Comments(FILE_EXPORT3A));
        Export3File.WriteValidity(FileIn(path, FILE_EXPORT3B), trains, Comments(FILE_EXPORT3B));
        Export3File.WriteNotes(FileIn(path, FILE_EXPORT3C), trains, Comments(FILE_EXPORT3C));

        var templates = RoutesFile.BuildTemplates(trains, gvd);
        RoutesFile.WriteTemplates(FileIn(path, FILE_VZORY), templates, Comments(FILE_VZORY));
        RoutesFile.WriteTrains(FileIn(path, FILE_VLAKY), templates, Comments(FILE_VLAKY));
        RoutesFile.WriteReportStations(FileIn(path, FILE_STAHLASB), templates, gvd, station => station.IsInShortReport, Comments(FILE_STAHLASB));
        RoutesFile.WriteReportStations(FileIn(path, FILE_STAHLASC), templates, gvd, station => station.IsInLongReport, Comments(FILE_STAHLASC));

        PoziceFile.Write(FileIn(path, FILE_POZICE), trains, Comments(FILE_POZICE));
        DoplnkyFile.Write(FileIn(path, FILE_DOPLNKY), trains, context.Document, Comments(FILE_DOPLNKY));
        ForeignFile.Write(FileIn(path, FILE_FOREIGN), trains, context.Workspace.Languages, Comments(FILE_FOREIGN));
        VylukaFile.Write(FileIn(path, FILE_VYLUKA), trains, Comments(FILE_VYLUKA));
        MosFile.Write(FileIn(path, FILE_MOS), trains, Comments(FILE_MOS));

        // stanice tras - pri ulozeni grafikonu subor hned prepise CustomStationsFile len vlastnymi stanicami
        var stations = new HashSet<Station> { gvd.ThisStation };
        stations.UnionWith(context.Document.CustomStations);
        foreach (var train in trains)
        {
            stations.UnionWith(train.StaniceZoSmeru);
            stations.UnionWith(train.StaniceDoSmeru);
        }

        WriteRows(FileIn(path, FILE_STANICE), Comments(FILE_STANICE),
            stations.Select(station => new CsvRow { station.ID, station.Name.UTFtoANSI().Quote() }));
    }

    private static string FileIn(string path, string fileName) => CombinePath(path, fileName)!;
}
