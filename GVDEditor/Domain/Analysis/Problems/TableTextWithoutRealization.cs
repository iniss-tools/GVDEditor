using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;

namespace GVDEditor.Domain.Analysis.Problems;

internal class TableTextWithoutRealization : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="TableTextWithoutRealization" /> class.</summary>
    public TableTextWithoutRealization(TableText text, AnalysisScope scope)
    {
        Scope = scope;
        TText = text;
    }

    private TableText TText { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TextNoRealization, TText.Key);

    public string Solution => Resources.Analyzer_TextNoRealization_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // text sa upravuje na stranke Texty na tabuliach - okno sa otvori s tymto textom
        Scope.Host.ShowLocalSettings(LocalSettingsPage.Texty, select: TText);

        //Check if the problem was solved
        return TText.Realizations.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}
