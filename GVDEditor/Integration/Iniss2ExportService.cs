using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using Serilog;
using ToolsCore.Iniss.Registry;
using ToolsCore.Iniss.StateDgm;
using ToolsCore.Iniss2;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Integration;

/// <summary>
/// Stanica cielovej instalacie INISS2, ktora moze byt sablonou hlaseni - ma stavovy diagram aj hlasenia.
/// </summary>
/// <param name="Group">grafikon</param>
/// <param name="Directory">priecinok stanice</param>
/// <param name="Path">cela cesta k priecinku stanice</param>
/// <param name="Reports">hlasenia stanice</param>
internal sealed record Iniss2TemplateStation(string Group, string Directory, string Path, IReadOnlyList<ReportKey> Reports)
{
    /// <inheritdoc />
    public override string ToString() => $"{Group} / {Directory}";
}

/// <summary>
/// Co vybral pouzivatel v sprievodcovi exportu.
/// </summary>
/// <param name="Grafikons">grafikony INISS 3.39 v poradi stanic</param>
/// <param name="DataDir">priecinok dat cielovej instalacie INISS2</param>
/// <param name="Group">nazov noveho grafikonu</param>
/// <param name="StationDirectories">priecinky stanic v poradi grafikonov</param>
/// <param name="Template">sablona hlaseni; null = hlasenia z typov 3.39 a bez stavoveho diagramu</param>
/// <param name="ReportLetters">pismeno hlasenia sablony pre kluc typu hlasenia 3.39</param>
/// <param name="RunConfiguration">konfiguracia spustania INISSu, z ktorej sa zistia linky tabul; null = bez liniek</param>
/// <param name="FontBanks">subor .bin pre kluc katalogovej tabule ELEN</param>
internal sealed record Iniss2ExportPlan(
    IReadOnlyList<GVDDirectory> Grafikons,
    string DataDir,
    string Group,
    IReadOnlyList<string> StationDirectories,
    Iniss2TemplateStation? Template,
    IReadOnlyDictionary<string, string> ReportLetters,
    RunConfiguration? RunConfiguration,
    IReadOnlyDictionary<string, string> FontBanks);

/// <summary>
/// Priprava a zapis exportu do INISS2 pre sprievodcu: nacita grafikony, zisti linky, zostavi a zapise. Nic nezobrazuje.
/// </summary>
internal static class Iniss2ExportService
{
    private const string FontsDirectory = "FONTS";
    private const string TargetFontsDirectory = "TableFonts";
    private const string FontExtension = "*.bin";

    // druhy, ktorym server sirku pisma berie z binarnej banky
    private static readonly HashSet<TableManufacturer> BankManufacturers =
        [TableManufacturer.Elen, TableManufacturer.Elenold, TableManufacturer.Elen10, TableManufacturer.Elen16, TableManufacturer.Elen16Kam];

    /// <summary>
    /// Predvoleny nazov grafikonu INISS2 - rok konca platnosti, ako ich pomenuvaju data INISS2.
    /// </summary>
    public static string DefaultGroup(GVDInfo gvd)
    {
        ArgumentNullException.ThrowIfNull(gvd);
        return gvd.EndValidTimeTable.Year.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Predvoleny priecinok stanice: poradie a nazov stanice.
    /// </summary>
    public static string DefaultStationDirectory(int index, string station) =>
        string.Create(CultureInfo.InvariantCulture, $"{index + 1:00} {string.Concat(station.Split(Path.GetInvalidFileNameChars()))}");

    /// <summary>
    /// Priecinok dat instalacie INISS2 podla vybraneho priecinka (instalacia, server alebo priamo data); null = nie je to INISS2.
    /// </summary>
    public static string? DataDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            return null;

        path = Path.GetFullPath(path);
        if (File.Exists(Path.Combine(path, DirListXml.FileName)))
            return path;

        return Iniss2Installation.Find(path)?.DataDir;
    }

    /// <summary>
    /// Grafikon s tymto nazvom uz v instalacii ma vlaky.
    /// </summary>
    public static bool GroupExists(string dataDir, string group) => File.Exists(Path.Combine(dataDir, group, GvFile.FileName));

    /// <summary>
    /// Stanice cielovej instalacie so stavovym diagramom a hlaseniami v poradi DirList.xml.
    /// </summary>
    public static List<Iniss2TemplateStation> TemplateStations(string dataDir)
    {
        var stations = new List<Iniss2TemplateStation>();
        foreach (var group in DirListXml.Read(dataDir))
        {
            foreach (var item in group.Items)
            {
                var path = Path.Combine(dataDir, group.Name, item.Directory);
                if (File.Exists(Path.Combine(path, Iniss2Export.StateDiagramFile)) && File.Exists(Path.Combine(path, ReportKeysFile.FileName)))
                    stations.Add(new Iniss2TemplateStation(group.Name, item.Directory, path, ReportKeysFile.Read(path)));
            }
        }

        return stations;
    }

    /// <summary>
    /// Predvolene priradenie typov hlaseni 3.39 hlaseniam sablony: rovnake pismeno, inak ziadne.
    /// </summary>
    public static Dictionary<string, string> DefaultLetters(IEnumerable<ReportType> types, IReadOnlyList<ReportKey> template) =>
        types.ToDictionary(type => type.Key, type => template.FirstOrDefault(report => string.Equals(report.Key, type.Char, StringComparison.OrdinalIgnoreCase))?.Key ?? "");

    /// <summary>
    /// Katalogove tabule grafikonov, ktore v INISS2 potrebuju banku pisem ELEN; pouzite fyzickymi tabulami so zariadenim.
    /// </summary>
    public static List<TableCatalog> BankCatalogs(IEnumerable<GrafikonDocument> documents) =>
        documents.SelectMany(document => document.TablePhysicals)
            .Where(physical => physical.ID != -1 && physical.TableCatalog is not null && BankManufacturers.Contains(physical.TableCatalog.Manufacturer))
            .Select(physical => physical.TableCatalog)
            .DistinctBy(catalog => catalog.Key)
            .ToList();

    /// <summary>
    /// Subory bank pisem: z priecinka FONTS dat INISS 3.39 a z Data\TableFonts cielovej instalacie.
    /// </summary>
    public static List<string> FontCandidates(string inissDataDir, string? targetDataDir)
    {
        var folders = new List<string> { Path.Combine(inissDataDir, FontsDirectory) };
        if (targetDataDir is not null)
            folders.Add(Path.Combine(targetDataDir, TargetFontsDirectory));

        return folders.Where(Directory.Exists).SelectMany(folder => Directory.EnumerateFiles(folder, FontExtension)).Order(StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// Grafikony vybrane v sprievodcovi: otvoreny sa berie z editora aj s neulozenymi zmenami, ostatne sa nacitaju.
    /// </summary>
    public static List<GrafikonDocument> LoadDocuments(InissWorkspace workspace, IReadOnlyList<GVDDirectory> grafikons, GVDDirectory? open,
        GrafikonDocument openDocument)
    {
        ArgumentNullException.ThrowIfNull(workspace);

        return grafikons.Select(dir => open is not null && string.Equals(dir.Dir.FullPath, open.Dir.FullPath, StringComparison.OrdinalIgnoreCase)
            ? openDocument
            : GrafikonRepository.Load(dir.Dir.FullPath, dir.GVD, workspace, new LoadWarnings())).ToList();
    }

    /// <summary>
    /// Prevedie vybrane grafikony. Nic nezapisuje.
    /// </summary>
    /// <param name="workspace">instalacia INISS 3.39</param>
    /// <param name="documents">grafikony v poradi planu</param>
    /// <param name="plan">volby sprievodcu</param>
    /// <param name="loadSource">nacitanie nastaveni INISSu pre linky tabul; null = register a .INI tohto pocitaca</param>
    /// <exception cref="Iniss2ExportException">stanica nema cislo alebo je dvakrat</exception>
    public static Iniss2ExportData Build(InissWorkspace workspace, IReadOnlyList<GrafikonDocument> documents, Iniss2ExportPlan plan,
        Func<string, string?, InissRunMode, IReadOnlyDictionary<int, RegTableInfo>?, InissConfigSource>? loadSource = null)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        ArgumentNullException.ThrowIfNull(plan);

        var (lines, lineWarnings) = TableLines(workspace, plan, loadSource);
        var source = Iniss2ExportSource.FromWorkspace(workspace) with
        {
            TableLines = lines,
            FontBanks = plan.FontBanks,
            TemplateStation = plan.Template?.Path ?? "",
            ReportTemplate = plan.Template?.Reports,
            ReportLetters = plan.Template is null ? new Dictionary<string, string>() : plan.ReportLetters
        };

        var stateWarnings = new List<Iniss2ExportWarning>();
        var inputs = plan.Grafikons.Select((dir, index) => new Iniss2StationInput(documents[index], dir.GVD)
        {
            Grafikon = dir.Dir.DirName,
            StateDgm = ReadStateDgm(dir.Dir.FullPath, dir.GVD.ThisStation.Name, stateWarnings)
        }).ToList();
        var data = Iniss2Export.Build(inputs, source);
        return data with { Warnings = [.. lineWarnings, .. stateWarnings, .. data.Warnings] };
    }

    // stavovy diagram grafikonu zo suboru - editor ho uklada hned, otvoreny grafikon ho v pamati nema
    private static StateDgmDiagram? ReadStateDgm(string directory, string station, List<Iniss2ExportWarning> warnings)
    {
        try
        {
            return StateDgmFile.Read(directory);
        }
        catch (Exception e) when (e is StateDgmParseException or IOException or UnauthorizedAccessException)
        {
            Log.Warning(e, "Stavovy diagram grafikonu {Directory} sa nepodarilo nacitat, export pouzije diagram sablony", directory);
            warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.StateDiagramUnreadable, $"{station}: {e.Message}"));
            return null;
        }
    }

    /// <summary>
    /// Kontrola cielovej instalacie pred zapisom: hlasenia LogText, nazvy tabuliek TabTab a beziaci server.
    /// </summary>
    public static List<Iniss2ExportWarning> Check(string dataDir, Iniss2ExportData data)
    {
        var warnings = new List<Iniss2ExportWarning>();
        warnings.AddRange(Iniss2Export.CheckLogText(dataDir, data));
        warnings.AddRange(Iniss2Export.CheckTabTabGlobal(dataDir, data));
        if (data.TemplateStation.Length == 0)
        {
            warnings.AddRange(data.Stations.Where(station => station.StateDiagram is null)
                .Select(station => new Iniss2ExportWarning(Iniss2ExportIssue.StateDiagramMissing, station.Station.ToString(CultureInfo.InvariantCulture))));
        }

        return warnings;
    }

    /// <summary>
    /// Zapise export ako novy grafikon. Server musi byt zastaveny.
    /// </summary>
    /// <exception cref="Iniss2ExportException">server bezi alebo grafikon uz ma vlaky</exception>
    public static void Write(Iniss2ExportData data, Iniss2ExportPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        if (Iniss2Export.IsServerRunning())
            throw new Iniss2ExportException(Iniss2ExportIssue.ServerRunning, plan.DataDir);

        Log.Information("Export do INISS2: grafikony {Grafikons} do {DataDir}, grafikon {Group}", plan.Grafikons.Select(dir => dir.Dir.DirName), plan.DataDir,
            plan.Group);
        Iniss2Export.Write(data, plan.DataDir, plan.Group, plan.StationDirectories);
    }

    // linky tabul z konfiguracie spustania INISSu 3.39 ako pri presmerovani na simulator
    private static (List<Iniss2TableLine> Lines, List<Iniss2ExportWarning> Warnings) TableLines(InissWorkspace workspace, Iniss2ExportPlan plan,
        Func<string, string?, InissRunMode, IReadOnlyDictionary<int, RegTableInfo>?, InissConfigSource>? loadSource)
    {
        if (plan.RunConfiguration is null)
            return ([], []);

        var tables = InissTableMap.Build(workspace.GVDDirs);
        var resolved = InissEnvironment.Resolve(plan.RunConfiguration, workspace.INISSDir, tables.ToDictionary(table => table.Index, table => table.ToInfo()),
            loadSource);
        var map = DriverLines.Build(resolved, tables);
        var station = plan.Grafikons.Count > 0 ? plan.Grafikons[0].GVD.ThisStation.Name : "";
        return Iniss2Export.TableLines(map, section => resolved.Find(section, "TablePortParam")?.Value as string, station);
    }
}
