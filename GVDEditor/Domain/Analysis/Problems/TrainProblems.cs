using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Vlak s chybami alebo upozorneniami okna vlaku.
/// </summary>
internal sealed class TrainProblems(Train train, List<TrainRules.Problem> problems, string? homeStationId, AnalysisScope scope) : IProblem
{
    public string Text
    {
        get
        {
            var first = problems.OrderBy(p => p.IsWarning).First();
            var text = string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_Train, TrainRules.Label(train), first.Message);
            return problems.Count > 1 ? text + " " + string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_More, problems.Count - 1) : text;
        }
    }

    public string Solution => Resources.Analyzer_Train_Fix;

    public ProblemType ProblemType => problems.Any(p => !p.IsWarning) ? ProblemType.Error : ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        if (!scope.Host.EditTrain(train))
            return FixResult.NotSolved;

        var index = scope.Document.Trains.IndexOf(train);
        return index < 0 || GrafikonChecks.CheckTrain(scope.Document.Trains, index, homeStationId).Count == 0 ? FixResult.Done : FixResult.NotSolved;
    }
}
