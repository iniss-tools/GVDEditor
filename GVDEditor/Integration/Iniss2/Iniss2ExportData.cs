using ToolsCore.Iniss2.Grafikon;
using ToolsCore.Iniss2.Tables;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Grafikon INISS2 s jednou alebo viacerymi stanicami.
/// </summary>
/// <param name="Timetable">vlaky s trasami a casmi vo vsetkych staniciach</param>
/// <param name="RouteStations">stanice tras</param>
/// <param name="Stations">subory stanic v poradi exportovanych grafikonov</param>
/// <param name="Lockouts">vyluky vlakov</param>
/// <param name="Diversions">odklony vlakov</param>
/// <param name="UserReports">texty vyluk a odklonov zalozene obsluhou; zaporne cisla sa pri zapise zmenia na cisla v ConstReports.xml</param>
/// <param name="Lines">linky tabul na doplnenie do CommLines.xml</param>
/// <param name="FontFiles">banky pisem ELEN na skopirovanie do Data\TableFonts</param>
/// <param name="Warnings">co sa nedalo preniest presne</param>
/// <param name="TemplateStation">stanica cielovej instalacie, z ktorej sa skopiruje statedgm.xml staniciam bez vlastneho diagramu; prazdne = bez kopie</param>
internal sealed record Iniss2ExportData(
    GvFile Timetable,
    IReadOnlyList<GvStation> RouteStations,
    IReadOnlyList<Iniss2StationData> Stations,
    IReadOnlyList<TrainLockout> Lockouts,
    IReadOnlyList<TrainDiversion> Diversions,
    IReadOnlyList<ConstReport> UserReports,
    IReadOnlyList<CommLineDefinition> Lines,
    IReadOnlyList<Iniss2FontFile> FontFiles,
    IReadOnlyList<Iniss2ExportWarning> Warnings,
    string TemplateStation = "")
{
    /// <summary>Predvolene stanice hlasenia grafikonu (Default.Stations.xml).</summary>
    public IReadOnlyList<DefaultStation> DefaultStations { get; init; } = [];

    /// <summary>Funkcie dodatkov pre LogText.vb; zapis doplni tie, ktore v skripte nie su.</summary>
    public IReadOnlyList<LogTextAddition> AdditionFunctions { get; init; } = [];
}
