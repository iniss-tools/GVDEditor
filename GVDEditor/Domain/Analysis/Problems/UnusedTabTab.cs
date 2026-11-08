using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

internal class UnusedTabTab : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="UnusedTabTab" /> class.</summary>
    public UnusedTabTab(TableTabTab table, AnalysisScope scope)
    {
        Scope = scope;
        TabTab = table;
    }

    private TableTabTab TabTab { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTabUnused, TabTab.Key);

    public string Solution => Resources.Analyzer_TabTabUnused_Fix;

    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        return Scope.Document.TabTabs.Remove(TabTab) ? FixResult.Done : FixResult.NotSolved;
    }
}
