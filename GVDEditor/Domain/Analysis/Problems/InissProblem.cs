namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Problem z kontroly nastaveni INISSu - opravi sa v okne Nastavenia INISSu, potom sa kontrola zopakuje.
/// </summary>
internal sealed class InissProblem(InissFinding finding, string grafikon, AnalysisScope scope) : IProblem
{
    public string Text => finding.Text;

    public string Solution => finding.Solution;

    public ProblemType ProblemType => finding.Type;

    public FixType FixType => FixType.Manual;

    // nastavenia INISSu sa zapisuju hned v ich okne, nie s grafikonom
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowInissSettings(finding.Section);
        if (scope.InissLoader?.Invoke() is not { } view) return FixResult.Done;
        return InissChecks.Evaluate(grafikon, view).Any(f => f.Key == finding.Key) ? FixResult.NotSolved : FixResult.Done;
    }
}
