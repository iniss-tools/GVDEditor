using System.Globalization;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;

namespace GVDEditor.Domain.Analysis.Problems;

internal class TableWithoutSegments : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="TableWithoutSegments" /> class.</summary>
    public TableWithoutSegments(TableCatalog table, AnalysisScope scope)
    {
        Scope = scope;
        Table = table;
    }

    private TableCatalog Table { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_CatalogNoRows, Table.Key);

    public string Solution => Resources.Analyzer_CatalogNoRows_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // riadky pribudnu s poctom zaznamov na stranke Katalogove tabule - okno sa otvori s touto tabulou
        Scope.Host.ShowLocalSettings(LocalSettingsPage.KatalogoveTabule, select: Table);

        //Check if the problem was solved
        return Table.Segments.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}
