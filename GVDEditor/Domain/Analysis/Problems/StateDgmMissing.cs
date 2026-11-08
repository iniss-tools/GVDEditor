using GVDEditor.Formats;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;

namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Grafikon nema stavovy diagram - INISS jeho vlaky nedokaze obsluhovat.
/// </summary>
internal sealed class StateDgmMissing(string dir, AnalysisScope scope) : IProblem
{
    public string Text => Resources.Analyzer_StateDgmMissing;

    public string Solution => Resources.Analyzer_StateDgmMissing_Fix;

    public ProblemType ProblemType => ProblemType.Error;

    public FixType FixType => FixType.Manual;

    // diagram sa zapise hned v editore, nie pri ulozeni grafikonu
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowLocalSettings(LocalSettingsPage.StavovyDiagram);
        return File.Exists(StateDgmFile.PathOf(dir)) ? FixResult.Done : FixResult.NotSolved;
    }
}
