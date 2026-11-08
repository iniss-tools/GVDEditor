using System.Globalization;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Analysis.Problems;

internal class UnusedTable : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="UnusedTable" /> class.</summary>
    public UnusedTable(ITable table, AnalysisScope scope)
    {
        Scope = scope;
        Table = table;
    }

    private ITable Table { get; }

    public string Text
    {
        get
        {
            var tabname = Table switch
            {
                TableCatalog => Resources.Analyzer_TableCatalog,
                TablePhysical => Resources.Analyzer_TablePhysical,
                TableLogical => Resources.Analyzer_TableLogical,
                _ => ""
            };
            return string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TableUnused, tabname, Table.Key);
        }
    }

    public string Solution => Resources.Analyzer_TableUnused_Fix;

    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        return Table switch
        {
            TableCatalog tc => Scope.Document.TableCatalogs.Remove(tc) ? FixResult.Done : FixResult.NotSolved,
            TablePhysical tb => Scope.Document.TablePhysicals.Remove(tb) ? FixResult.Done : FixResult.NotSolved,
            TableLogical tl => Scope.Document.TableLogicals.Remove(tl) ? FixResult.Done : FixResult.NotSolved,
            _ => FixResult.NotSolved
        };
    }
}
