using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Properties;
using GVDEditor.TabTabEditor;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Domain.Analysis;

internal enum FixType
{
    /// <summary>
    /// Program problem opravi uplne sam automaticky.
    /// </summary>
    Auto,

    /// <summary>
    /// Pouzivatel musi vybrat jednu z ponukanych moznosti, aby chybu opravil.
    /// </summary>
    SemiAuto,

    /// <summary>
    /// Pouzivatel musi chybu opravit sam a program mu len ukaze, kde ma chybu opravit.
    /// </summary>
    Manual
}

internal enum ProblemType
{
    /// <summary>
    /// Len informacia pre pouzivatela. Grafikon je uplne funkcny.
    /// </summary>
    Hint,

    /// <summary>
    /// Grafikon nemusi fungovat uplne spravne, ale je spustitelny.
    /// </summary>
    Warning,

    /// <summary>
    /// Zavazna chyba v grafikone. INISS pravdepodobne nespusti tento grafikon.
    /// </summary>
    Error
}

internal enum FixResult
{
    /// <summary>
    /// Ak bola chyba opravena.
    /// </summary>
    Done,

    /// <summary>
    /// Ak pouzivatel chybu neopravil.
    /// </summary>
    NotSolved,

    /// <summary>
    /// Ak pocas opravy doslo k chybe.
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

    /// <summary>
    /// Ci oprava meni grafikon v pamati (a treba ho potom ulozit). Oprava mimo grafikonu, napr. zmazanie
    /// vyrovnavacej pamate INISSu, sa zapise hned.
    /// </summary>
    public bool ChangesGrafikon => true;
}

/// <summary>
/// Analyzuje a opravuje problemy najdene v grafikone.
/// </summary>
internal static class Analyzer
{
    /// <summary>
    /// Najde problemy grafikonu <paramref name="gvd" />. Bezi na pozadi - priebeh v percentach hlasi cez
    /// <paramref name="progress" />.
    /// </summary>
    public static List<IProblem> FindProblems(GVDDirectory gvd, AnalysisScope scope, IProgress<int>? progress = null)
    {
        List<IProblem> problems = [];

        //1. Check GVD validity
        var now = DateTime.Now;
        if (gvd.GVD.EndValidData < DateOnly.FromDateTime(now))
        {
            var problem = new GVDOutOfValidity(gvd, scope);
            problems.Add(problem);
        }

        progress?.Report(5);

        //2. Check Empty TabTabs
        foreach (var tab in scope.Document.TabTabs)
            if (string.IsNullOrEmpty(tab.Text))
            {
                var problem = new EmptyTabTab(tab, scope);
                problems.Add(problem);
            }

        progress?.Report(10);

        //3. Check using Catalog tables in TPhysic and in TableTextRealization AND Segments
        foreach (var catalog in scope.Document.TableCatalogs)
        {
            if (catalog.Segments.Count == 0)
            {
                var problem = new TableWithoutSegments(catalog, scope);
                problems.Add(problem);
            }

            var unused = scope.Document.TablePhysicals.All(physical => physical.TableCatalog != catalog);

            if (!unused) continue;

            foreach (var tt in scope.Document.TableTexts)
            foreach (var realization in tt.Realizations)
                if (realization.Table == catalog)
                    unused = false;

            if (unused)
            {
                var problem = new UnusedTable(catalog, scope);
                problems.Add(problem);
            }
        }

        progress?.Report(25);

        //4. Check using Physic tables in Tlogical
        foreach (var physical in scope.Document.TablePhysicals)
        {
            var unused = true;
            foreach (var logical in scope.Document.TableLogicals)
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
                var problem = new UnusedTable(physical, scope);
                problems.Add(problem);
            }
        }

        progress?.Report(50);

        //5. Check using TabTabs
        foreach (var tab in scope.Document.TabTabs)
        {
            var unused = true;
            foreach (var catalog in scope.Document.TableCatalogs)
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
                var problem = new UnusedTabTab(tab, scope);
                problems.Add(problem);
            }
        }

        //5b. Check TabTab rules and conditions (what INISS logs at load + GVDEditor warnings)
        var symbols = new GvdExprSymbols(scope.Workspace, scope.Document);
        foreach (var tab in scope.Document.TabTabs)
        {
            if (string.IsNullOrEmpty(tab.Text)) continue;
            var result = TabTabValidator.Validate(tab.Text, symbols.OptionsFor(tab));
            if (result.Diagnostics.Any(d => d.Severity != ExprSeverity.Info))
                problems.Add(new TabTabProblems(tab, result, scope));
        }

        progress?.Report(75);

        //6. Check TTexts
        for (var i = 0; i < scope.Document.TableTexts.Count; i++)
        {
            var tableText = scope.Document.TableTexts[i];
            if (tableText.Realizations.Count == 0)
            {
                var problem = new TableTextWithoutRealization(tableText, scope);
                problems.Add(problem);
            }

            if (tableText.Trains.Count == 0)
            {
                var problem = new TableTextWithoutTrains(tableText, scope);
                problems.Add(problem);
            }
        }

        progress?.Report(80);

        //7. Tables, texts, platforms, tracks, operators and fonts - the rules of the Local settings pages
        problems.AddRange(GrafikonChecks.Settings(scope));

        //8. Trains - the rules of the train window (data from files and imports never went through it)
        problems.AddRange(GrafikonChecks.Trains(gvd, scope));
        progress?.Report(88);

        //9. Announced stations without a recording in the sound bank
        problems.AddRange(GrafikonChecks.StationRecordings(scope));

        //10. State diagram - missing, unreadable or with validator errors
        problems.AddRange(GrafikonChecks.StateDgm(gvd, scope));

        progress?.Report(95);

        //11. Check Zpozdeni.DAT cache - INISS Zpozdeni.TXT necita, kym existuje .DAT (nekontroluje ani cas suborov)
        var zpozdeniTxt = PathUtils.CombinePath(scope.Workspace.DataDir, GvdFileConsts.FileZpozdeni)!;
        var zpozdeniDat = PathUtils.CombinePath(scope.Workspace.DataDir, GvdFileConsts.FileZpozdeniDat)!;
        if (File.Exists(zpozdeniTxt) && File.Exists(zpozdeniDat) &&
            File.GetLastWriteTimeUtc(zpozdeniTxt) > File.GetLastWriteTimeUtc(zpozdeniDat))
        {
            var problem = new StaleZpozdeniCache(zpozdeniDat);
            problems.Add(problem);
        }

        progress?.Report(100);

        return problems;
    }
}

internal class StaleZpozdeniCache : IProblem
{
    /// <summary>Initializes a new instance of the <see cref="StaleZpozdeniCache" /> class.</summary>
    public StaleZpozdeniCache(string cachePath)
    {
        CachePath = cachePath;
    }

    private string CachePath { get; }

    public string Text =>
        string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ZpozdeniCacheOld, GvdFileConsts.FileZpozdeni, GvdFileConsts.FileZpozdeniDat);

    public string Solution => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_ZpozdeniCacheOld_Fix, GvdFileConsts.FileZpozdeniDat);

    public bool ChangesGrafikon => false;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        try
        {
            File.Delete(CachePath);
            return FixResult.Done;
        }
        catch (Exception e)
        {
            Log.Exception(e, string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_DeleteFailed, CachePath));
            return FixResult.Error;
        }
    }
}

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

internal class UnusedTabTab : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="UnusedTabTab" /> class.</summary>
    public UnusedTabTab(TableTabTab table, AnalysisScope scope)
    {
        Scope = scope;
        TabTab = table;
    }

    private TableTabTab TabTab { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTabUnused, TabTab.Key);

    public string Solution => Resources.Analyzer_TabTabUnused_Fix;

    public ProblemType ProblemType => ProblemType.Hint;

    public FixType FixType => FixType.Auto;

    public FixResult FixProblem()
    {
        return Scope.Document.TabTabs.Remove(TabTab) ? FixResult.Done : FixResult.NotSolved;
    }
}

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

internal class TableTextWithoutTrains : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="TableTextWithoutTrains" /> class.</summary>
    public TableTextWithoutTrains(TableText text, AnalysisScope scope)
    {
        Scope = scope;
        TText = text;
    }

    private TableText TText { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TextNoTrains, TText.Key);

    public string Solution => Resources.Analyzer_TextNoTrains_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        // text sa upravuje na stranke Texty na tabuliach - okno sa otvori s tymto textom
        Scope.Host.ShowLocalSettings(LocalSettingsPage.Texty, select: TText);

        //Check if the problem was solved
        return TText.Trains.Count == 0 ? FixResult.NotSolved : FixResult.Done;
    }
}

internal class EmptyTabTab : IProblem
{
    private AnalysisScope Scope { get; }

    /// <summary>Initializes a new instance of the <see cref="EmptyTabTab" /> class.</summary>
    public EmptyTabTab(TableTabTab tabTab, AnalysisScope scope)
    {
        Scope = scope;
        TabTab = tabTab;
    }

    private TableTabTab TabTab { get; }

    public string Text => string.Format(CultureInfo.CurrentCulture, Resources.Analyzer_TabTabEmpty, TabTab.Key);

    public string Solution => Resources.Analyzer_TabTabEmpty_Fix;

    public ProblemType ProblemType => ProblemType.Warning;

    public FixType FixType => FixType.Manual;

    public FixResult FixProblem()
    {
        Scope.Host.EditTabTab(TabTab);

        //Check if the problem was solved
        return string.IsNullOrEmpty(TabTab.Text) ? FixResult.NotSolved : FixResult.Done;
    }
}

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