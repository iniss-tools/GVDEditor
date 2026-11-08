using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

internal class EmptyTabTab : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="EmptyTabTab" /> class.</summary>
    public EmptyTabTab(TableTabTab tabTab, AnalysisScope scope)
    {
        Scope = scope;
        TabTab = tabTab;
    }

    private TableTabTab TabTab { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTabEmpty, TabTab.Key);

    public string Solution => Resources.Analyzer_TabTabEmpty_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        Scope.Host.EditTabTab(TabTab);

        //Check if the problem was solved
        return string.IsNullOrEmpty(TabTab.Text) ? FixResult.NotSolved : FixResult.Done;
    }
}
