using GVDEditor.Config;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.Iniss2;

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
