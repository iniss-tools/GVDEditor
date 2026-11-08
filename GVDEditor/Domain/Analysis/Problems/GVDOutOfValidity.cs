using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

internal class GVDOutOfValidity : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="GVDOutOfValidity" /> class.</summary>
    public GVDOutOfValidity(GVDDirectory gvdDir, AnalysisScope scope)
    {
        Scope = scope;
        GVDDir = gvdDir;
    }

    private GVDDirectory GVDDir { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_DataExpired, GVDDir.PeriodFormatted, GVDDir.GVD.EndValidData);

    public string Solution => Resources.Analyzer_DataExpired_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // cez hlavne okno - po zmene obdobia obnovi vyber obdobia a oznaci grafikon ako neulozeny
        Scope.Host.ShowLocalSettings();

        //Check if the problem was solved
        return GVDDir.GVD.EndValidData < DateOnly.FromDateTime(DateTime.Now) ? FixResult.NotSolved : FixResult.Done;
    }
}
