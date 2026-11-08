using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.TabTabEditor;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.Domain.Analysis.Problems;

internal class TabTabProblems : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="TabTabProblems" /> class.</summary>
    public TabTabProblems(TableTabTab tabTab, TabTabValidationResult result, AnalysisScope scope)
    {
        Scope = scope;
        TabTab = tabTab;
        Result = result;
    }

    private TableTabTab TabTab { get; }

    private TabTabValidationResult Result { get; }

    public string Text
    {
        get
        {
            var first = Result.Diagnostics.First(d => d.Severity != ExprSeverity.Info);
            var counts = Result.ErrorCount > 0
                ? string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTab_pocet_chyb, Result.ErrorCount, Result.WarningCount)
                : string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTab_pocet_varovani, Result.WarningCount);
            return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTabProblems, TabTab.Key, counts, first.LineIndex + 1, first.Message);
        }
    }

    public string Solution => Resources.Analyzer_Upravit_TabTab;

    public ProblemType ProblemType => Result.ErrorCount > 0 ? ProblemType.Error : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        Scope.Host.EditTabTab(TabTab);

        var again = TabTabValidator.Validate(TabTab.Text, new GvdExprSymbols(Scope.Workspace, Scope.Document).OptionsFor(TabTab));
        return again.Diagnostics.Any(d => d.Severity != ExprSeverity.Info) ? FixResult.NotSolved : FixResult.Done;
    }
}
