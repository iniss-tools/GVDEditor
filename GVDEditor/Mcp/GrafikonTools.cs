using GVDEditor.Domain.Entities;
using ModelContextProtocol.Server;

namespace GVDEditor.Mcp;

/// <summary>Nastroje MCP nad instalaciou INISS a otvorenym grafikonom.</summary>
[McpServerToolType]
internal static class GrafikonTools
{
    [McpServerTool(Name = "get_grafikon", Title = "Open timetable", ReadOnly = true, Idempotent = true, OpenWorld = false, UseStructuredContent = true)]
    [Description("The timetable (grafikon) open in GVDEditor: INISS installation folder, timetable folder, station, validity and number of trains. "
                 + "Includes unsaved changes made in the editor.")]
    public static Task<GrafikonSummary> GetGrafikon(EditorAccess editor) =>
        editor.ReadAsync(context => new GrafikonSummary(
            context.Workspace.INISSDir,
            context.Directory?.Dir.DirName,
            context.Directory?.GVD.ThisStation.Name,
            context.Directory?.GVD.StartValidTimeTable,
            context.Directory?.GVD.EndValidTimeTable,
            context.Document.Trains.Count));

    [McpServerTool(Name = "list_grafikons", Title = "Timetables of the installation", ReadOnly = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Timetable folders of the INISS installation chosen in GVDEditor with station, validity and number of trains as saved on disk. "
                 + "Other tools read the open timetable only.")]
    public static async Task<GrafikonList> ListGrafikons(EditorAccess editor)
    {
        var (dirs, open, inissDir) = await editor.ReadAsync(context =>
            (context.Workspace.GVDDirs.ToList(), context.Directory?.Dir.FullPath, context.Workspace.INISSDir));
        // hlavicky grafikonov z disku - mimo vlakna okna
        var grafikons = dirs.Select(dir =>
        {
            var gvd = ReadHeader(dir.FullPath);
            return new GrafikonFolder(dir.DirName, gvd?.ThisStation.Name, gvd?.StartValidTimeTable, gvd?.EndValidTimeTable, gvd?.TrainCount,
                string.Equals(dir.FullPath, open, StringComparison.OrdinalIgnoreCase));
        }).ToList();
        return new GrafikonList(inissDir, grafikons);
    }

    [McpServerTool(Name = "get_station", Title = "Station of the timetable", ReadOnly = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Station data of the open timetable: tracks with their logical boards, platforms, operators, train types, custom stations and "
                 + "announcement languages.")]
    public static Task<StationInfo> GetStation(EditorAccess editor) =>
        editor.ReadAsync(context =>
        {
            var document = McpGuards.Open(context);
            return new StationInfo(
                context.Directory?.GVD.ThisStation.Name,
                document.Tracks.Where(t => t != Track.None).Select(TableMapper.Track).ToList(),
                document.Platforms.Select(p => $"{p.Key} - {p.FullName}").ToList(),
                document.Operators.Select(o => $"{o.Id} - {o.Name}").ToList(),
                context.Workspace.TrainsTypes.Select(t => $"{t.Key} - {t.TextInTable}").ToList(),
                document.CustomStations.Select(s => s.Name).ToList(),
                document.LocalLanguages.Select(l => l.Name).ToList());
        });

    private static GVDInfo? ReadHeader(string path)
    {
        try
        {
            return InfoGvdFile.Read(path);
        }
        catch (Exception e) when (e is IOException or FormatException or UnauthorizedAccessException)
        {
            return null;
        }
    }
}