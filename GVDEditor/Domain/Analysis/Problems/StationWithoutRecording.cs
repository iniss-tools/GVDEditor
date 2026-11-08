using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

/// <summary>
/// Stanica, ktora sa v trasach hlasi, no zvukova banka pre nu nema nahravku.
/// </summary>
internal sealed class StationWithoutRecording(Station station, List<Train> trains, AnalysisScope scope) : IProblem
{
    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_StationNoRecording, station.Name, station.ID,
        trains.Count, TrainRules.Label(trains[0]));

    public string Solution => Resources.Analyzer_StationNoRecording_Fix;

    // pohranicne body a cudzie stanice byvaju v trase zamerne - grafikon funguje, len stanicu INISS neohlasi
    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // stanica sa z hlaseni vynecha v trase vlaku; nahravka sa doplna v RawBankEditore
        var train = trains.FirstOrDefault(t => scope.Document.Trains.Contains(t));
        if (train is null || !scope.Host.EditTrain(train))
            return FixResult.NotSolved;

        return GrafikonChecks.StationsWithoutRecording(scope.Document.Trains, scope.Workspace.Stations ?? [])
            .Any(s => string.Equals(s.Station.ID, station.ID, StringComparison.OrdinalIgnoreCase))
            ? FixResult.NotSolved
            : FixResult.Done;
    }
}
