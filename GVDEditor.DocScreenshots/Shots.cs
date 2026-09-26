using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using GVDEditor.Controls;
using GVDEditor.Entities;
using GVDEditor.Forms;
using GVDEditor.Tools;
using ToolsCore.StateDgm;

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
                Resize(form, 1420, 600);
                SelectTrain(form, trains.IndexOf(express));
            }, dispose: false);

            // nový grafikon pre ďalšiu stanicu na obdobie 2026/2027
            Shot("novy-grafikon/novy-grafikon", () => new FNewGrafikon(FMain.ObdobiaList.ToList()), form =>
            {
                foreach (var name in new[] { "dtpDataOd", "dtpGVDOd" })
                    ((ExControls.ExDateTimePicker)Field(form, name)).Value = new DateTime(2026, 12, 13);
                foreach (var name in new[] { "dtpDataDo", "dtpGVDDo" })
                    ((ExControls.ExDateTimePicker)Field(form, name)).Value = new DateTime(2027, 12, 11);
                var station = (ComboBox)Field(form, "cbStationName");
                station.SelectedIndex = station.Items.Cast<object>().ToList().FindIndex(o => o.ToString() == "Veľká Ves");
                ((TextBoxBase)Field(form, "tbDirIniss")).Select(0, 0);
            });
            // na záložke Radenie vybrané radenie v pracovné dni
            Shot("uprava-vlaku", () => new FEditTrain(express, trains.IndexOf(express), gvdDir.GVD, false, gvdDir.Dir.FullPath),
                form =>
                {
                    SelectListItem(form, "listRadenia", 0);

                    // dodatok D1002 hlásený pri Přijíždí (obe podoby) a pri Zastavil (dlhé) - pridaný tlačidlom Pridať
                    var table = (DataGridView)Field(form, "dgvDoplnokSet");
                    foreach (DataGridViewRow row in table.Rows)
                    {
                        var type = row.Cells[0].Value as string;
                        if (type == "Přijíždí")
                            row.Cells[1].Value = row.Cells[2].Value = true;
                        else if (type == "Zastavil")
                            row.Cells[2].Value = true;
                    }

                    SelectListItem(form, "listAllDoplnky", 1);
                    form.GetType().GetMethod("bDoplnkyAdd_Click", BindingFlags.NonPublic | BindingFlags.Instance)!
                        .Invoke(form, [form, EventArgs.Empty]);
                }, tabs: true);

            // skladanie radenia: vybraná druhá nahrávka „číslo“ a priečinok s vlastnosťami vozňov
            Shot("radenie/uprava-radenia", () => new FRadenie([.. express.Radenia[0].Sounds]), form =>
            {
                SelectCombo(form, "cbSoundDir", ((ComboBox)Field(form, "cbSoundDir")).Items.IndexOf(ToolsCore.Entities.FyzGroupType.VOZY1));
                SelectListItem(form, "listAllSounds", 0);
                SelectListItem(form, "listRadenie", 5);
            });
            // na záložkách Nástupištia a Koľaje vybrať skutočné nástupište a koľaj, nie zástupné "N"
            Shot("lokalne-nastavenia", () => new FLocalSettings(gvdDir), form =>
            {
                SelectListItem(form, "listNastupistia", 1);
                SelectListItem(form, "listDopravcovia", 1);
                SelectListItem(form, "listKolaje", 1);
            }, tabs: true);
            Shot("globalne-nastavenia", () => new FGlobalSettings(FMain.ObdobiaList.ToList()), tabs: true);
            Shot("nastavenia-programu/nastavenia-programu", () => new FAppSettings(GlobData.Config, GlobData.Styles));
            Shot("nastavenia-programu/komponenty", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pDesktopComponents");
                return form;
            });
            Shot("nastavenia-programu/stlpce", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pDesktopColumns");
                return form;
            });
            Shot("nastavenia-programu/lokalizacia", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pLocalization");
                return form;
            });
            Shot("nastavenia-programu/klavesove-skratky", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pShortcuts");
                return form;
            });
            Shot("nastavenia-programu/styly", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pStyles");
                return form;
            });
            Shot("nastavenia-programu/pisma", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pFonts");
                return form;
            });
            Shot("nastavenia-programu/logovanie", () =>
            {
                var form = new FAppSettings(GlobData.Config, GlobData.Styles);
                form.PreselectMenuItem("pLogging");
                return form;
            });
            // stránka Spúšťanie INISS s argumentmi zadanými zaškrtnutím
            Shot("spustanie-iniss/nastavenia-spustania", () =>
            {
                var config = GlobData.Config with { StartupINISSConfig = new GVDEditor.XML.StartupINISS { CmdArgs = "/Minimize /NoRestore" } };
                var form = new FAppSettings(config, GlobData.Styles);
                form.PreselectMenuItem("pStartupIniss");
                return form;
            });
            // rozdelenie staršieho zápisu: jeden priečinok s dvoma obdobiami stanice (bloky /9900100)
            Shot("migracia-blokov/rozdelenie", () => new FBlockMigration(@"C:\INISS\DATA\DolneMesto",
            [
                new GvdBlock(1, 9900100, "Dolné Mesto", 118, new DateTime(2025, 12, 14), new DateTime(2026, 12, 12)) { DirName = "DolneMesto.2026" },
                new GvdBlock(2, 9900100, "Dolné Mesto", 124, new DateTime(2026, 12, 13), new DateTime(2027, 12, 11)) { DirName = "DolneMesto.2027_2" }
            ]), form => Resize(form, 760, 330));

            // import dát z CSV s hlavičkou: tri nové vlaky s trasou podľa čísel staníc
            const string importSample =
                "Číslo;Typ;Príchod;Odchod;Koľaj;Dátumové obmedzenie;Všetky stanice\n" +
                "3611;Os;;06:20;2;ide v 1-5;9900100,9900110,9900120\n" +
                "3612;Os;19:40;;2;ide v 6,7;9900120,9900110,9900100\n" +
                "1921;REX;08:48;08:50;1;ide denne;9900130,9900100,9900140\n";
            FImportData ImportForm()
            {
                var form = new FImportData(gvdDir.GVD);
                ((CheckBox)Field(form, "cboxFirstHeader")).Checked = true;
                form.GetType().GetMethod("LoadText", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(form, [importSample]);
                return form;
            }

            Shot("import-dat/import-dat", ImportForm, form => Resize(form, 900, 520));
            Shot("import-dat/typ-stlpca", () => new FColumnTypeSelect(), form => SelectListItem(form, "listColumnTypes", 5));

            // import z ELIS: voľby importu a priradenie staníc, ktoré ELIS pomenúva inak
            Shot("import-z-elis/import-z-elis", () => new FELISImport(gvdDir.GVD.ThisStation.Name, trains.Count),
                form => ((TextBoxBase)Field(form, "tbAppPath")).Select(0, 0));
            Shot("import-z-elis/priradenie-stanic",
                () => new FELISStations(["Hraničná št.hr.", "Lipová zastávka", "Nová Obec", "Podhradie mesto"]),
                form => Resize(form, 720, 420));

            // analýza s nájdenými problémami: prázdny a nepoužitý TabTab a uplynutá platnosť dát (po snímke sa vráti)
            var emptyTab = new TableTabTab { Key = "Rezerva", Text = "" };
            var endValidData = gvdDir.GVD.EndValidData;
            GlobData.TabTabs.Add(emptyTab);
            gvdDir.GVD.EndValidData = new DateTime(2026, 6, 30);
            Shot("analyza-grafikonu/analyza-grafikonu", () => new FAnalyzer(gvdDir), form =>
            {
                Resize(form, 820, 360);
                form.GetType().GetMethod("bAnalyze_Click", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(form, [form, EventArgs.Empty]);
                var worker = (BackgroundWorker)Field(form, "bgWorkAnalyze");
                Pump.Until(() => !worker.IsBusy);
                Pump.Events();
                var grid = (DataGridView)Field(form, "dgvResults");
                grid.ClearSelection();
                if (grid.Rows.Count > 0) grid.Rows[0].Selected = true;
            });
            GlobData.TabTabs.Remove(emptyTab);
            gvdDir.GVD.EndValidData = endValidData;
            // generátor s obdobím grafikonu a vygenerovaným poľom bitov
            var gvdInfo = gvdDir.GVD;
            const string sampleLimit = "ide v 1-5, nejde 24.XII., 31.XII.";
            Shot("datumove-obmedzenia/generator", () => new FDatObm(gvdInfo.StartValidTimeTable, gvdInfo.EndValidTimeTable), form =>
            {
                ((TextBox)Field(form, "tbDatObm")).Text = sampleLimit;
                form.GetType().GetMethod("bGenerate_Click", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(form, [form, EventArgs.Empty]);
                var bits = (TextBox)Field(form, "tbBitArray");
                bits.SelectionStart = 0;
                bits.SelectionLength = 0;
            });

            // editor dátumového obmedzenia s kalendárom pre Ex 521 (normálne sa otvára modálne cez SetDateLimit)
            Shot("datumove-obmedzenia/editor", () =>
            {
                var form = (Form)Activator.CreateInstance(typeof(FDateLimitEdit), nonPublic: true)!;
                var type = form.GetType();
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                type.GetField("_train", flags)!.SetValue(form, express);
                type.GetField("_dateLimit", flags)!.SetValue(form,
                    new Tools.DateLimit(gvdInfo.StartValidTimeTable, gvdInfo.EndValidTimeTable, insertMarks: false));
                type.GetField("_textChanging", flags)!.SetValue(form, true);
                ((TextBox)Field(form, "tbDateLimit")).Text = sampleLimit;
                ((TextBox)Field(form, "tbOldDateLimit")).Text = express.DateLimitText;
                type.GetMethod("InitCalendar", flags)!.Invoke(form, [gvdInfo.StartValidTimeTable, gvdInfo.EndValidTimeTable]);
                type.GetMethod("TextToGrid", flags)!.Invoke(form, null);
                type.GetField("_textChanging", flags)!.SetValue(form, false);
                return form;
            }, form =>
            {
                foreach (var box in Descendants(form).OfType<TextBox>())
                    box.SelectionLength = 0;
                form.ActiveControl = null;
            });
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

            // editor s chybou v pravidle (neuložená úprava) - ukážka podčiarknutia a zoznamu problémov s opravou
            var smer = GlobData.TabTabs.First(t => t.Key == "Smer");
            Shot("tabule/editor-tabtab-problemy", () => new FTabTab(smer, station), form =>
            {
                Resize(form, 1100, 620);
                var scintilla = ((Controls.MyScintilla)Field(form, "scText")).Scintilla;
                scintilla.Text = smer.Text + "\r\nTyp(Typ_RR), \"R\" = #SWITCH";
                form.GetType().GetMethod("ValidateDocument", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(form, null);
                Pump.Events();
                var problems = (DataGridView)Field(form, "dgvProblems");
                if (problems.Rows.Count > 0)
                {
                    problems.ClearSelection();
                    problems.Rows[0].Selected = true;
                }
            });

            // náhľad textu na tabuli pre Ex 521 s meškaním na odchode
            Shot("tabule/nahlad-na-tabuli",
                () => new FTabTabPreview(name => GlobData.TabTabs.FirstOrDefault(t => t.Key == name)?.Text, "Druh",
                    int.Parse(station.ID, CultureInfo.InvariantCulture)),
                form =>
                {
                    var trainBox = (ComboBox)Field(form, "cbTrain");
                    for (var i = 0; i < trainBox.Items.Count; i++)
                        if (trainBox.GetItemText(trainBox.Items[i]).Contains("521", StringComparison.Ordinal))
                            trainBox.SelectedIndex = i;
                    ((NumericUpDown)Field(form, "nudDelayDep")).Value = 5;
                    ((CheckBox)Field(form, "chkOnlySection")).Checked = false;
                    Resize(form, 1180, 600);
                    Pump.Events();

                    // riadok Smer - spodný panel ukáže postup vyhodnotenia vrátane textu z TTexts
                    var grid = (DataGridView)Field(form, "dgvResult");
                    var row = grid.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => Equals(r.Cells["cColumn"].Value, "Smer"));
                    if (row != null)
                    {
                        grid.ClearSelection();
                        row.Selected = true;
                        grid.CurrentCell = row.Cells["cColumn"];
                    }
                    grid.FirstDisplayedScrollingColumnIndex = 0;
                });

            // editor s vybraným stavom „Zastavil“ prechádzajúceho vlaku; graf len s prechodmi vybraného stavu
            var diagram = TxtParser.ReadStateDgm(gvdDir.Dir.FullPath)!;
            var passing = diagram.Categories[1];
            var arrived = passing.States.First(s => s.Key == "Zastavil");
            Shot("stavovy-diagram/stavovy-diagram", () => new FStateDgm(gvdDir), form =>
            {
                Resize(form, 1360, 820);
                LogStateDgmProblems(form);
                ((ToolStripButton)Field(form, "_tsbAllEdges")).Checked = false;
                var own = (StateDgmDiagram)Field(form, "_d");
                SelectStateDgmNode(form, own.Categories[1], own.Categories[1].States.First(s => s.Key == arrived.Key));
            });

            // dialóg akcie - prechod do stavu Odjede s hlásením a tlačidlom
            var departs = arrived.Events.First(e => e.NextState == "Odjede");
            Shot("stavovy-diagram/akcia", () =>
            {
                var editor = new SdEventEditor();
                editor.Bind(departs, arrived.Controls.FirstOrDefault(c => c.EventKey == departs.Key),
                    passing.States.Select(s => s.Key), diagram.Designs.Select(d => d.Key), 0);
                return new FStateDgmItem(Properties.Resources.FStateDgm_Akcia_Titul, editor, 520, 380);
            }, form =>
            {
                // bez zvýrazneného textu v rozbaľovacích poliach a v kľúči
                foreach (var box in Descendants(form).OfType<ComboBox>().Where(c => c.DropDownStyle == ComboBoxStyle.DropDown))
                    box.SelectionLength = 0;
                foreach (var box in Descendants(form).OfType<TextBox>())
                    box.SelectionLength = 0;
                form.ActiveControl = form.AcceptButton as Control ?? Descendants(form).OfType<Button>().First();
            });

            // kalendár akcií pre rýchlik s meškaním
            Shot("stavovy-diagram/kalendar-akcii",
                () => new FStateDgmCalendar(() => diagram, int.Parse(station.ID, CultureInfo.InvariantCulture), express),
                form =>
                {
                    ((NumericUpDown)Field(form, "nudDelayArr")).Value = 5;
                    ((NumericUpDown)Field(form, "nudDelayDep")).Value = 5;
                    Resize(form, 1200, 400);
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
                // prepnutie záložky občas nestihne prekresliť hlavičky - vynútiť pred zachytením
                form.Refresh();
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

    /// <summary>
    ///     V navigátore editora stavového diagramu zbalí vzhľady, časové body a ostatné kategórie a vyberie stav.
    /// </summary>
    private static void SelectStateDgmNode(Form form, StateDgmCategory category, StateDgmState state)
    {
        var tree = (TreeView)Field(form, "tvNav");
        foreach (TreeNode root in tree.Nodes)
        {
            if (root.Nodes.Count == 0) continue;
            var hasCategory = root.Nodes.Cast<TreeNode>().Any(n => n.Tag == category);
            if (!hasCategory)
            {
                root.Collapse();
                continue;
            }

            foreach (TreeNode cat in root.Nodes)
                if (cat.Tag == category)
                    cat.Expand();
                else
                    cat.Collapse();
        }

        var node = Descend(tree.Nodes).First(n => n.Tag == state);
        tree.SelectedNode = node;
        tree.Nodes[0].EnsureVisible();
        node.EnsureVisible();

        static IEnumerable<TreeNode> Descend(TreeNodeCollection nodes)
        {
            foreach (TreeNode n in nodes)
            {
                yield return n;
                foreach (var c in Descend(n.Nodes))
                    yield return c;
            }
        }
    }

    private static void SelectListItem(Form form, string name, int index) =>
        ((ListBox)form.Controls.Find(name, true).Single()).SelectedIndex = index;

    private static object Field(Form form, string name) =>
        form.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(form)!;

    private static void SelectCombo(Form form, string name, int index) =>
        ((ComboBox)form.Controls.Find(name, true).Single()).SelectedIndex = index;

    private static void SelectTrain(Form main, int index)
    {
        var grid = Descendants(main).OfType<DataGridView>().First();
        grid.ClearSelection();
        grid.Rows[index].Selected = true;
        grid.CurrentCell = grid.Rows[index].Cells.Cast<DataGridViewCell>().First(c => c.Visible);

        // klik na riadok ukáže vybraný vlak a počet jeho variantov v stavovom riadku
        main.GetType().GetMethod("dgvTrains_CellClick", BindingFlags.NonPublic | BindingFlags.Instance)?
            .Invoke(main, [grid, new DataGridViewCellEventArgs(grid.CurrentCell.ColumnIndex, index)]);
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
