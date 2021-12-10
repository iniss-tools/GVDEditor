using GVDEditor.Entities;
using GVDEditor.Forms;

namespace GVDEditor.Tools;

internal enum FixType
{
    /// <summary>
    ///     Program problem opravi uplne sam automaticky.
    /// </summary>
    Auto,

    /// <summary>
    ///     Pouzivatel musi vybrat jednu z ponukanych moznosti, aby chybu opravil.
    /// </summary>
    SemiAuto,

    /// <summary>
    ///     Pouzivatel musi chybu opravit sam a program mu len ukaze, kde ma chybu opravit.
    /// </summary>
    Manual
}

internal enum ProblemType
{
    /// <summary>
    ///     Len informacia pre pouzivatela. Grafikon je uplne funkcny.
    /// </summary>
    Hint,

    /// <summary>
    ///     Grafikon nemusi fungovat uplne spravne, ale je spustitelny.
    /// </summary>
    Warning,

    /// <summary>
    ///     Zavazna chyba v grafikone. INISS pravdepodobne nespusti tento grafikon.
    /// </summary>
    Error
}

internal enum FixResult
{
    /// <summary>
    ///     Ak bola chyba opravena.
    /// </summary>
    Done,

    /// <summary>
    ///     Ak pouzivatel chybu neopravil.
    /// </summary>
    NotSolved,

    /// <summary>
    ///     Ak pocas opravy doslo k chybe.
    /// </summary>
    Error
}

internal interface IProblem
{
    public string Text { get; }

    public string Solution { get; }

    public ProblemType ProblemType { get; }

    public FixType FixType { get; }

    public FixResult FixProblem();
}

/// <summary>
///     Analyzuje a opravuje problemy najdene v grafikone.
/// </summary>
internal static class Analyzer
{
    public static List<IProblem> FindProblems(BackgroundWorker bw, GVDDirectory gvd)
    {
        List<IProblem> problems = new();

        //1. Check GVD validity
        var now = DateTime.Now;
        if (gvd.GVD.EndValidData < now)
        {
            var problem = new GVDOutOfValidity(gvd);
            problems.Add(problem);
        }

        bw.ReportProgress(5);

        //2. Check Empty TabTabs
        foreach (var tab in GlobData.TabTabs)
            if (string.IsNullOrEmpty(tab.Text))
            {
                var problem = new EmptyTabTab(tab);
                problems.Add(problem);
            }

        bw.ReportProgress(10);

        //3. Check using Catalog tables in TPhysic and in TableTextRealization AND Segments
        foreach (var catalog in GlobData.TableCatalogs)
        {
            if (catalog.Segments.Count == 0)
            {
                var problem = new TableWithoutSegments(catalog);
                problems.Add(problem);
            }

            var unused = GlobData.TablePhysicals.All(physical => physical.TableCatalog != catalog);

            if (!unused) continue;

            foreach (var tt in GlobData.TableTexts)
            foreach (var realization in tt.Realizations)
                if (realization.Table == catalog)
                    unused = false;

            if (unused)
            {
                var problem = new UnusedTable(catalog);
                problems.Add(problem);
            }
        }

        bw.ReportProgress(25);

        //4. Check using Physic tables in Tlogical
        foreach (var physical in GlobData.TablePhysicals)
        {
            var unused = true;
            foreach (var logical in GlobData.TableLogicals)
            {
                foreach (var rec in logical.Records)
                {
                    if (rec.Positions.Any(position => position.Table == physical)) unused = false;

                    if (!unused) break;
                }

                if (!unused) break;
            }

            if (unused)
            {
                var problem = new UnusedTable(physical);
                problems.Add(problem);
            }
        }

        bw.ReportProgress(50);

        //5. Check using TabTabs
        foreach (var tab in GlobData.TabTabs)
        {
            var unused = true;
            foreach (var catalog in GlobData.TableCatalogs)
            {
                foreach (var item in catalog.Items)
                    if (item.Tab1 == tab || item.Tab2 == tab)
                    {
                        unused = false;
                        break;
                    }

                if (!unused) break;
            }

            if (unused)
            {
                var problem = new UnusedTabTab(tab);
                problems.Add(problem);
            }
        }

        bw.ReportProgress(75);

        //6. Check TTexts
        for (var i = 0; i < GlobData.TableTexts.Count; i++)
        {
            var tableText = GlobData.TableTexts[i];
            if (tableText.Realizations.Count == 0)
            {
                var problem = new TableTextWithoutRealization(tableText, gvd, i);
                problems.Add(problem);
            }

            if (tableText.Trains.Count == 0)
            {
                var problem = new TableTextWithoutTrains(tableText, gvd, i);
                problems.Add(problem);
            }
        }

        bw.ReportProgress(100);

        return problems;
    }
}

internal class UnusedTable : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="UnusedTable" /> class.</summary>
    public UnusedTable(ITable table)
    {
        Table = table;
    }

    public ITable Table { get; }

    public string Text
    {
        get
        {
            var tabname = Table switch
            {
                TableCatalog => "Katalógová",
                TablePhysical => "Fyzická",
                TableLogical => "Logická",
                _ => ""
            };
            return $"{tabname} tabuľa {Table.Key} sa nikde nepoužíva.";
        }
    }

    public string Solution => "Odstrániť tabuľu";

    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        return Table switch
        {
            TableCatalog tc => GlobData.TableCatalogs.Remove(tc) ? FixResult.Done : FixResult.NotSolved,
            TablePhysical tb => GlobData.TablePhysicals.Remove(tb) ? FixResult.Done : FixResult.NotSolved,
            TableLogical tl => GlobData.TableLogicals.Remove(tl) ? FixResult.Done : FixResult.NotSolved,
            _ => FixResult.NotSolved
        };
    }
}

internal class UnusedTabTab : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="UnusedTabTab" /> class.</summary>
    public UnusedTabTab(TableTabTab table)
    {
        TabTab = table;
    }

    public TableTabTab TabTab { get; }

    public string Text => $"TabTab {TabTab.Key} sa nikde nepoužíva.";

    public string Solution => "Odstrániť TabTab";

    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        return GlobData.TabTabs.Remove(TabTab) ? FixResult.Done : FixResult.NotSolved;
    }
}

internal class TableWithoutSegments : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="TableWithoutSegments" /> class.</summary>
    public TableWithoutSegments(TableCatalog table)
    {
        Table = table;
    }

    public TableCatalog Table { get; }

    public string Text => $"Katalógova tabuľa {Table.Key} nemá nastavené žiadne riadky (segmenty).";

    public string Solution => "Upraviť segmenty katalógovej tabule";

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        var form = new FTableCatalog(Table, GlobData.TabTabs);
        form.ShowDialog();

        //Check if the problem was solved
        return Table.Segments.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}

internal class TableTextWithoutRealization : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="TableTextWithoutRealization" /> class.</summary>
    public TableTextWithoutRealization(TableText text, GVDDirectory gvdDir, int row)
    {
        TText = text;
        GVDDir = gvdDir;
        Row = row;
    }

    public TableText TText { get; }

    public GVDDirectory GVDDir { get; }

    public int Row { get; }

    public string Text => $"Table Text je \"{TText.Key}\" nemá žiadnu realizáciu.";

    public string Solution => "Pridať realizácie textu";

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        var form = new FTableText(TText, GlobData.TableCatalogs, GVDDir.GVD, Row);
        form.ShowDialog();

        //Check if the problem was solved
        return TText.Realizations.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}

internal class TableTextWithoutTrains : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="TableTextWithoutTrains" /> class.</summary>
    public TableTextWithoutTrains(TableText text, GVDDirectory gvdDir, int row)
    {
        TText = text;
        GVDDir = gvdDir;
        Row = row;
    }

    public TableText TText { get; }

    public GVDDirectory GVDDir { get; }

    public int Row { get; }

    public string Text => $"Table Text je \"{TText.Key}\" nemá nastavené žiadne texty vlakov.";

    public string Solution => "Pridať realizácie textu";

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        var form = new FTableText(TText, GlobData.TableCatalogs, GVDDir.GVD, Row);
        form.ShowDialog();

        //Check if the problem was solved
        return TText.Trains.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}

internal class EmptyTabTab : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="EmptyTabTab" /> class.</summary>
    public EmptyTabTab(TableTabTab tabTab)
    {
        TabTab = tabTab;
    }

    public TableTabTab TabTab { get; }

    public string Text => $"TabTab {TabTab.Key} je prázdny";

    public string Solution => "Upraviť TabTab";

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        var form = new FTabTab(TabTab);
        form.ShowDialog();

        //Check if the problem was solved
        return string.IsNullOrEmpty(TabTab.Text) ? FixResult.NotSolved : FixResult.Done;
    }
}

internal class GVDOutOfValidity : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="GVDOutOfValidity" /> class.</summary>
    public GVDOutOfValidity(GVDDirectory gvdDir)
    {
        GVDDir = gvdDir;
    }

    public GVDDirectory GVDDir { get; }

    public string Text => $"Grafikon {GVDDir.PeriodFormatted} je po platnosti";

    public string Solution => "Zmeniť platnosť grafikonu";

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        var form = new FLocalSettings(GVDDir, 0);
        form.ShowDialog();

        //Check if the problem was solved
        return GVDDir.GVD.EndValidData < DateTime.Now ? FixResult.NotSolved : FixResult.Done;
    }
}