using ToolsCore.Iniss2.Grafikon;
using ToolsCore.Iniss2.Tables;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Subory jednej stanice v tvare INISS2.
/// </summary>
/// <param name="Station">cislo stanice</param>
/// <param name="Tracks">kolaje stanice</param>
/// <param name="Positions">kolaje vlakov</param>
/// <param name="MotorTrains">motorove vlaky</param>
/// <param name="Additions">dodatky vlakov</param>
/// <param name="Compositions">radenie vlakov</param>
/// <param name="Languages">jazyky hlaseni vlakov</param>
/// <param name="ReportKeys">hlasenia stanice s pismenami</param>
/// <param name="Tables">tabule stanice</param>
/// <param name="Catalogs">katalogove predlohy tabul</param>
/// <param name="TabTabs">tabulky TabTab stanice</param>
/// <param name="Texts">texty vlakov pre tabule</param>
internal sealed record Iniss2StationData(
    int Station,
    IReadOnlyList<TrackDefinition> Tracks,
    IReadOnlyList<TrainPosition> Positions,
    IReadOnlyList<MosTrain> MotorTrains,
    IReadOnlyList<TrainAddition> Additions,
    IReadOnlyList<TrainComposition> Compositions,
    IReadOnlyList<TrainLanguages> Languages,
    IReadOnlyList<ReportKey> ReportKeys,
    IReadOnlyList<TableDefinition> Tables,
    IReadOnlyList<CatalogTable> Catalogs,
    IReadOnlyList<TabTabTable> TabTabs,
    IReadOnlyList<TrainTextSet> Texts)
{
    /// <summary>Stavovy diagram prevedeny zo StateDgm.txt; null = grafikon ho nema, stanica dostane diagram sablony.</summary>
    public StateDiagram? StateDiagram { get; init; }

    /// <summary>Stanice hlasenia pri vlakoch (StaHlasVl.xml) - vyber INISS 3.39 v kratkom a dlhom hlaseni.</summary>
    public IReadOnlyList<TrainReportStations> ReportStations { get; init; } = [];

    /// <summary>Vzory trasy kratkeho hlasenia (StaHlasB.xml) pre prazdny vyber.</summary>
    public IReadOnlyList<IReadOnlyList<ReportPatternStop>> ShortReportPatterns { get; init; } = [];

    /// <summary>Vzory trasy dlheho hlasenia (StaHlasC.xml) pre prazdny vyber.</summary>
    public IReadOnlyList<IReadOnlyList<ReportPatternStop>> LongReportPatterns { get; init; } = [];
}
