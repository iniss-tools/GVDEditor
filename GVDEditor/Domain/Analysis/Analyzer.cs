using GVDEditor.Domain.Analysis.Problems;
using GVDEditor.Domain.Entities;
using GVDEditor.TabTabEditor;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Domain.Analysis;

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

        //7b. Font numbers that ELEN tables would get as a control character (without bit 0x40)
        problems.AddRange(ElenFontChecks.Problems(scope));

        //8. Trains - the rules of the train window (data from files and imports never went through it)
        problems.AddRange(GrafikonChecks.Trains(gvd, scope));
        progress?.Report(88);

        //9. Announced stations without a recording in the sound bank
        problems.AddRange(GrafikonChecks.StationRecordings(scope));

        //10. State diagram - missing, unreadable or with validator errors
        problems.AddRange(GrafikonChecks.StateDgm(gvd, scope));

        //10b. INISS settings of the selected run configuration - lines for the tables, disabled tables, file names
        problems.AddRange(InissChecks.Problems(gvd, scope));

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
