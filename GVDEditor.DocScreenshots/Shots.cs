using System.Globalization;
using System.Reflection;
using System.Text;
using GVDEditor.Entities;
using GVDEditor.Forms;

namespace GVDEditor.DocScreenshots;

/// <summary>
///     Zoznam snímok. Každá snímka je okno (a pri oknách so záložkami každá záložka zvlášť),
///     uložené ako <c>&lt;priečinok&gt;/&lt;názov&gt;-light.png</c> a <c>-dark.png</c>.
///     Názov záložky sa odvodí z jej textu (Fyzické tabule → fyzicke-tabule).
/// </summary>
internal sealed class Shots(Program.Options options, string theme, List<string> log)
{
    private int _count;

    public int Run(string gvdPath)
    {
        var installDir = Path.GetDirectoryName(Path.GetDirectoryName(gvdPath))!;
        log.Add($"krok: otvorenie hlavného okna ({theme})");
        var main = Program.OpenMain(installDir);
        try
        {
            var gvdDir = FMain.ObdobiaList.First();
            var trains = GlobData.Trains;
            var express = trains.First(t => t.Number == "521");

            // hlavné okno s vybraným rýchlikom
            Shot("hlavne-okno/hlavne-okno", main, form =>
            {
                Resize(form, 1360, 600);
                SelectTrain(form, trains.IndexOf(express));
            }, dispose: false);

            Shot("novy-grafikon/novy-grafikon", () => new FNewGrafikon());
            Shot("uprava-vlaku", () => new FEditTrain(express, trains.IndexOf(express), gvdDir.GVD, false, gvdDir.Dir.FullPath), tabs: true);
            Shot("lokalne-nastavenia", () => new FLocalSettings(gvdDir), tabs: true);
            Shot("globalne-nastavenia", () => new FGlobalSettings(FMain.ObdobiaList.ToList()), tabs: true);
            Shot("nastavenia-programu/nastavenia-programu", () => new FAppSettings(GlobData.Config, GlobData.Styles));
            Shot("analyza-grafikonu/analyza-grafikonu", () => new FAnalyzer(gvdDir));
            Shot("datumove-obmedzenia/generator", () => new FDatObm());
            // editory tabúľ nad ukážkovými tabuľami (DemoTables)
            var station = gvdDir.GVD.ThisStation;
            var catalog = GlobData.TableCatalogs[0];
            Shot("tabule/katalogova-tabula", () => new FTableCatalog(catalog, GlobData.TabTabs.ToList()),
                form => SelectListItem(form, "listColumns", catalog.Items.FindIndex(i => i.Key == "Smer")));
            Shot("tabule/poradie-stlpcov", () => new FTableColumnOrder(catalog.Items, catalog.ViewTypeTabs),
                form => SelectCombo(form, "cbViewMode", 1));
            Shot("tabule/fyzicka-tabula", () => new FTablePhysical(GlobData.TablePhysicals[0], GlobData.TableCatalogs));
            Shot("tabule/logicka-tabula", () => new FTableLogical(GlobData.TableLogicals[0], GlobData.TablePhysicals, false, station));
            Shot("tabule/text-na-tabuli", () => new FTableText(GlobData.TableTexts[0], GlobData.TableCatalogs, gvdDir.GVD, 0));
            // prvá položka zoznamu je zabudovaná prázdna "Žiadny"
            var druh = GlobData.TabTabs.First(t => t.Key == "Druh");
            Shot("tabule/editor-tabtab", () => new FTabTab(druh, station), form =>
            {
                Resize(form, 1100, 620);
                LogTabTabProblems(form, druh);
            });

            Shot("stavovy-diagram/stavovy-diagram", () => new FStateDgm(gvdDir), form =>
            {
                Resize(form, 1280, 760);
                LogStateDgmProblems(form);
            });
        }
        finally
        {
            // Dispose namiesto Close - Close by sa pri neuloženom grafikone pýtal na uloženie
            main.Dispose();
        }

        return _count;
    }

    private void Shot(string name, Func<Form> create, Action<Form>? setup = null, bool tabs = false) =>
        Shot(name, create(), setup, tabs: tabs);

    private void Shot(string name, Form form, Action<Form>? setup = null, bool dispose = true, bool tabs = false)
    {
        log.Add($"krok: {name} ({theme})");
        try
        {
            if (!form.Visible)
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(60, 60);
                form.ShowInTaskbar = false;
                form.Show();
            }

            Pump.Events();
            setup?.Invoke(form);
            Pump.Events();

            var tabControl = tabs ? MainTabControl(form) : null;
            if (tabControl is null)
            {
                Save(name, form);
                return;
            }

            foreach (TabPage page in tabControl.TabPages)
            {
                tabControl.SelectedTab = page;
                Pump.Events();
                Save($"{name}/{Slug(page.Text)}", form);
            }
        }
        catch (Exception e)
        {
            log.Add($"{name} ({theme}): {e.GetType().Name}: {e.Message}");
        }
        finally
        {
            if (dispose)
                form.Dispose();
        }
    }

    private void Save(string name, Form form)
    {
        if (options.Only is not null && !name.Contains(options.Only, StringComparison.OrdinalIgnoreCase))
            return;

        var file = Path.Combine(options.OutDir, $"{name}-{theme}.png".Replace('/', Path.DirectorySeparatorChar));
        WindowCapture.Save(form, file);
        _count++;
    }

    /// <summary>
    ///     Okná, ktoré sa otvárajú maximalizované, by mali na snímke šírku celej obrazovky.
    /// </summary>
    private static void Resize(Form form, int width, int height)
    {
        form.WindowState = FormWindowState.Normal;
        form.Location = new Point(40, 40);
        form.Size = new Size(width, height);
    }

    /// <summary>
    ///     Problémy, ktoré editor stavového diagramu hlási v ukážkovom grafikone (majú byť na snímke nula).
    /// </summary>
    private void LogStateDgmProblems(Form form)
    {
        if (theme != "light" || form.GetType().GetField("_problems", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(form)
                is not System.Collections.IList problems)
            return;

        foreach (var problem in problems)
            log.Add("  stavový diagram: " + problem.GetType().GetProperty("Diagnostic")?.GetValue(problem));
    }

    /// <summary>
    ///     Prejde všetky sekcie v editore TabTab a zapíše problémy, ktoré v nich editor hlási; potom vráti výber.
    /// </summary>
    private void LogTabTabProblems(Form form, TableTabTab selected)
    {
        if (theme != "light")
            return;

        var list = (ListBox)form.GetType().GetField("lbTabTabs", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
        var problems = (System.Collections.IList)form.GetType().GetField("_problemRows", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;
        var validate = form.GetType().GetMethod("ValidateDocument", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var keep = list.SelectedIndex;

        for (var i = 0; i < list.Items.Count; i++)
        {
            list.SelectedIndex = i;
            validate.Invoke(form, null);
            Pump.Events();
            foreach (var problem in problems)
                log.Add($"  TabTab {list.GetItemText(list.Items[i])}: {problem.GetType().GetProperty("Diagnostic")?.GetValue(problem)}");
        }

        list.SelectedIndex = keep;
        validate.Invoke(form, null);
        log.Add($"  TabTab: skontrolovaných {list.Items.Count} sekcií, vybraná {selected.Key}");
    }

    private static void SelectListItem(Form form, string name, int index) =>
        ((ListBox)form.Controls.Find(name, true).Single()).SelectedIndex = index;

    private static void SelectCombo(Form form, string name, int index) =>
        ((ComboBox)form.Controls.Find(name, true).Single()).SelectedIndex = index;

    private static void SelectTrain(Form main, int index)
    {
        var grid = Descendants(main).OfType<DataGridView>().First();
        grid.ClearSelection();
        grid.Rows[index].Selected = true;
        grid.CurrentCell = grid.Rows[index].Cells.Cast<DataGridViewCell>().First(c => c.Visible);
    }

    /// <summary>
    ///     Hlavný TabControl okna - najväčší, ktorý nie je vnorený v inom TabControle.
    /// </summary>
    private static TabControl? MainTabControl(Form form) =>
        Descendants(form).OfType<TabControl>()
            .Where(t => !Ancestors(t).OfType<TabControl>().Any())
            .MaxBy(t => t.Width * t.Height);

    private static IEnumerable<Control> Descendants(Control control)
    {
        foreach (Control child in control.Controls)
        {
            yield return child;
            foreach (var nested in Descendants(child))
                yield return nested;
        }
    }

    private static IEnumerable<Control> Ancestors(Control control)
    {
        for (var parent = control.Parent; parent is not null; parent = parent.Parent)
            yield return parent;
    }

    /// <summary>
    ///     „Fyzické tabule“ → „fyzicke-tabule“.
    /// </summary>
    private static string Slug(string text)
    {
        var sb = new StringBuilder();
        foreach (var c in text.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                continue;
            sb.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-');
        }

        return string.Join('-', sb.ToString().Split('-', StringSplitOptions.RemoveEmptyEntries));
    }
}
