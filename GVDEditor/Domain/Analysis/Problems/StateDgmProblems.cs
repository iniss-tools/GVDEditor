using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Stavovy diagram sa neda nacitat, alebo ma chyby ci upozornenia validatora.
/// </summary>
internal sealed class StateDgmProblems(string dir, List<StateDgmDiagnostic> diagnostics, string? readError, AnalysisScope scope) : IProblem
{
    public string Text
    {
        get
        {
            if (readError is not null)
                return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StateDgmRead, readError);

            var errors = diagnostics.Count(d => d.IsError);
            var first = diagnostics.OrderByDescending(d => d.IsError).First();
            var where = string.IsNullOrEmpty(first.Path) ? first.Message : $"{first.Path}: {first.Message}";
            return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StateDgmProblems, errors, diagnostics.Count - errors, where);
        }
    }

    public string Solution => Resources.Analyzer_StateDgmProblems_Fix;

    public ProblemType ProblemType => readError is not null || diagnostics.Any(d => d.IsError) ? ProblemType.Error : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    // diagram sa zapise hned v editore, nie pri ulozeni grafikonu
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowLocalSettings(LocalSettingsPage.StavovyDiagram, LocalSettingsAction.OpenStateDgmEditor);
        var again = GrafikonChecks.ValidateStateDgm(dir, scope, out var error);
        return error is null && again.Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}
