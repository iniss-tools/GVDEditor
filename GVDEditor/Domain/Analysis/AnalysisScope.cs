using GVDEditor.Domain.Documents;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Co analyza kontroluje a opravuje: otvoreny grafikon, instalacia INISS a hlavne okno pre opravy s pouzivatelom.
/// </summary>
internal sealed record AnalysisScope(GrafikonDocument Document, InissWorkspace Workspace, IAnalyzerHost Host)
{
    /// <summary>
    /// Nacita nastavenia INISSu vybranej konfiguracie spustania (register, .INI); null = kontroly nastaveni INISSu
    /// sa vynechaju. Vola sa na pozadi a po oprave znova.
    /// </summary>
    public Func<InissRegistryView?>? InissLoader { get; init; }
}
