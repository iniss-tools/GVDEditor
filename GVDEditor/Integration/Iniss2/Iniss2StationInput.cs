using GVDEditor.Domain.Documents;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Grafikon jednej stanice INISS 3.39 na export.
/// </summary>
/// <param name="Document">grafikon</param>
/// <param name="Gvd">hlavicka grafikonu</param>
internal sealed record Iniss2StationInput(GrafikonDocument Document, GVDInfo Gvd)
{
    /// <summary>Priecinok grafikonu v DirList.TXT - podla neho sa hladaju linky tabul.</summary>
    public string Grafikon { get; init; } = "";

    /// <summary>Stavovy diagram grafikonu; null = grafikon ho nema.</summary>
    public StateDgmDiagram? StateDgm { get; init; }
}
