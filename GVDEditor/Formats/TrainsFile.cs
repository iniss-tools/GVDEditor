using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Tools;
using static GVDEditor.Formats.FormatCommon;
using static ToolsCore.Iniss.Grafikon.GvdFileConsts;
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
        var trains = Export3File.ReadTrains(FileIn(path, FileExport3A), context);
        var dayMaps = Export3File.ReadValidity(FileIn(path, FileExport3B), trains);
        Export3File.ReadNotes(FileIn(path, FileExport3C), trains, dayMaps);

        var templates = RoutesFile.ReadTemplates(FileIn(path, FileVzory), FileIn(path, FileStahlasb), FileIn(path, FileStahlasc), context);
        RoutesFile.AssignRoutes(FileIn(path, FileVlaky), templates, trains, InfoGvdFile.Read(path), context.Workspace.TrainsTypes, context.Warnings);

        PoziceFile.Read(FileIn(path, FilePozice), trains, context.Document.Tracks);
        DoplnkyFile.Read(FileIn(path, FileDoplnky), trains, context);
        ForeignFile.Read(FileIn(path, FileForeign), trains, context.Workspace.Languages);
        VylukaFile.Read(FileIn(path, FileVyluka), trains);
        MosFile.Read(FileIn(path, FileMos), trains);

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

        Export3File.WriteTrains(FileIn(path, FileExport3A), trains, gvd, Comments(FileExport3A));
        Export3File.WriteValidity(FileIn(path, FileExport3B), trains, Comments(FileExport3B));
        Export3File.WriteNotes(FileIn(path, FileExport3C), trains, Comments(FileExport3C));

        var templates = RoutesFile.BuildTemplates(trains, gvd);
        RoutesFile.WriteTemplates(FileIn(path, FileVzory), templates, Comments(FileVzory));
        RoutesFile.WriteTrains(FileIn(path, FileVlaky), templates, Comments(FileVlaky));
        RoutesFile.WriteReportStations(FileIn(path, FileStahlasb), templates, gvd, station => station.IsInShortReport, Comments(FileStahlasb));
        RoutesFile.WriteReportStations(FileIn(path, FileStahlasc), templates, gvd, station => station.IsInLongReport, Comments(FileStahlasc));

        PoziceFile.Write(FileIn(path, FilePozice), trains, Comments(FilePozice));
        DoplnkyFile.Write(FileIn(path, FileDoplnky), trains, context.Document, Comments(FileDoplnky));
        ForeignFile.Write(FileIn(path, FileForeign), trains, context.Workspace.Languages, Comments(FileForeign));
        VylukaFile.Write(FileIn(path, FileVyluka), trains, Comments(FileVyluka));
        MosFile.Write(FileIn(path, FileMos), trains, Comments(FileMos));

        // stanice tras - pri ulozeni grafikonu subor hned prepise CustomStationsFile len vlastnymi stanicami
        var stations = new HashSet<Station> { gvd.ThisStation };
        stations.UnionWith(context.Document.CustomStations);
        foreach (var train in trains)
        {
            stations.UnionWith(train.StaniceZoSmeru);
            stations.UnionWith(train.StaniceDoSmeru);
        }

        WriteRows(FileIn(path, FileStanice), Comments(FileStanice),
            stations.Select(station => new CsvRow { station.ID, station.Name.UTFtoANSI().Quote() }));
    }

    private static string FileIn(string path, string fileName) => CombinePath(path, fileName)!;
}
