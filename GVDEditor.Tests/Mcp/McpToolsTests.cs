using System.Reflection;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Mcp;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace GVDEditor.Tests.Mcp;

/// <summary>Nastroje MCP nad grafikonom zostavenym v teste a pravidla ich popisu.</summary>
[TestClass]
public sealed partial class McpToolsTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly TrainType R = new("R");
    private static readonly Operator Zssk = new(1, "ZSSK");
    private static readonly Track Track1 = new("1", "1", "Koľaj 1", Platform.None, "", "1");
    private static readonly Track Track2 = new("2", "2", "Koľaj 2", Platform.None, "", "2");
    private static readonly DateOnly From = new(2026, 12, 13);
    private static readonly DateOnly To = new(2027, 12, 11);

    [GeneratedRegex("^[a-z]+(_[a-z]+)*$")]
    private static partial Regex SnakeCase();

    private static Train Train(int id, string number, TrainType type, string departure, Track track, string validity = "", string to = "Brezová Dolina")
    {
        var train = new Train
        {
            ID = id,
            Number = number,
            Name = "",
            Type = type,
            Operator = Zssk,
            Track = track,
            Variant = 0,
            Arrival = TimeOnly.Parse(departure, System.Globalization.CultureInfo.InvariantCulture).AddMinutes(-2),
            Departure = TimeOnly.Parse(departure, System.Globalization.CultureInfo.InvariantCulture),
            ZaciatokPlatnosti = From,
            KoniecPlatnosti = To,
            DateLimitText = validity,
            Routing = Routing.Prechadzajuci,
            EndingStation = new Station("2", to)
        };
        train.StaniceZoSmeru.Add(new Station("1", "Veľká Ves"));
        train.StaniceDoSmeru.Add(new Station("2", to));
        return train;
    }

    /// <summary>Editor s otvorenym grafikonom Dolne Mesto: Os 3001 (denne), R 801 (len v sobotu), Os 3003 na 2. kolaji.</summary>
    private static EditorAccess Editor(bool open = true) => Editor(out _, open);

    /// <summary>
    /// Editor s otvorenym grafikonom ako <see cref="Editor(bool)" /> a jeho kontext. Texty na tabuliach: CIEL (stlpec
    /// s cielovou stanicou, rucny text len pri Os 3001) a POZNAMKA (bez stlpca, z ktoreho sa da generovat).
    /// </summary>
    private static EditorAccess Editor(out EditorContext context, bool open = true, IAgentHost? host = null)
    {
        context = new EditorContext(null!);
        context.OpenWorkspace(new InissWorkspace { INISSDir = @"C:\INISS", GVDDirs = [], TrainsTypes = new([Os, R]) });
        if (!open)
            return new EditorAccess(context, null, host);
        var druh = new TableTabTab { Key = "DRUH", Text = "R{34369}=R\nIC{113}=IC\n(Typ(Typ_X9)), \"@\" = #SWITCH\nBus=#VYLUKA" };
        var ciel = new TableItem
        {
            Name = "Cieľ", Tab1 = TableTabTab.Empty, Tab2 = TableTabTab.Empty, DivType = TableDivType.Translate, FillSection = TableFillSection.CielovaStanica
        };
        var catalog = new TableCatalog
        {
            Key = "K16",
            Name = "ELEN 16",
            Items = [new TableItem { Name = "Druh", Tab1 = druh, Tab2 = TableTabTab.Empty, DivType = TableDivType.Translate, FillSection = TableFillSection.TypVlaku }, ciel]
        };
        var document = new GrafikonDocument
        {
            Tracks = [Track.None, Track1, Track2],
            TabTabs = [new TableTabTab { Key = "ODCHODY", Text = "@VLAK" }, druh],
            TableCatalogs = [catalog],
            TablePhysicals = [new TablePhysical { Key = "T1", Name = "Hala", TableCatalog = catalog }],
            TableFonts = [new TableFont { FontID = 113, Name = "Piktogramy" }]
        };
        document.Trains.Add(Train(1, "3001", Os, "10:15", Track1));
        document.Trains.Add(Train(2, "801", R, "08:30", Track1, "jazdí v 6", "Žilina"));
        document.Trains.Add(Train(3, "3003", Os, "12:15", Track2));
        document.TableTexts.Add(new TableText
        {
            Key = "CIEL",
            Name = "Cieľová stanica",
            Comment = "",
            Realizations = [new TableTextRealization { Table = catalog, Item = ciel }],
            Trains = [new TableTrain { Train = document.Trains[0], Text = "Ručný text", FontID = 113 }]
        });
        document.TableTexts.Add(new TableText
        {
            Key = "POZNAMKA",
            Name = "Poznámka",
            Comment = "",
            Realizations = [new TableTextRealization { Table = catalog, Item = catalog.Items[0] }]
        });
        context.OpenDocument(document, new GVDDirectory(new DirList { DirName = "GVD2027", FullPath = @"C:\INISS\DATA\GVD2027" },
            new GVDInfo { ThisStation = new Station("99", "Dolné Mesto"), StartValidTimeTable = From, EndValidTimeTable = To }));
        return new EditorAccess(context, null, host);
    }

    [TestMethod]
    public async Task Grafikon_PrehladOtvorenehoGrafikonu()
    {
        var summary = await GrafikonTools.GetGrafikon(Editor());
        var none = await GrafikonTools.GetGrafikon(Editor(false));

        Assert.AreEqual("GVD2027", summary.Folder);
        Assert.AreEqual("Dolné Mesto", summary.Station);
        Assert.AreEqual(From, summary.ValidFrom);
        Assert.AreEqual(3, summary.Trains);
        Assert.IsNull(none.Folder);
    }

    [TestMethod]
    public async Task Vlaky_FiltreAZoradenieCasom()
    {
        var all = await TrainTools.ListTrains(Editor());
        var os = await TrainTools.ListTrains(Editor(), type: "os");
        var zilina = await TrainTools.ListTrains(Editor(), station: "žilina");
        var track2 = await TrainTools.ListTrains(Editor(), track: "2");
        var morning = await TrainTools.ListTrains(Editor(), from: new TimeOnly(8, 0), to: new TimeOnly(11, 0));
        var page = await TrainTools.ListTrains(Editor(), offset: 1, limit: 1);

        CollectionAssert.AreEqual(new[] { "801", "3001", "3003" }, all.Trains.Select(t => t.Number).ToArray());
        Assert.AreEqual(2, os.Total);
        Assert.AreEqual("801", zilina.Trains.Single().Number);
        Assert.AreEqual("3003", track2.Trains.Single().Number);
        Assert.AreEqual(2, morning.Total);
        Assert.AreEqual(3, page.Total);
        Assert.AreEqual("3001", page.Trains.Single().Number);
    }

    [TestMethod]
    public async Task Vlaky_PodlaDnaSKalendarom()
    {
        // 19. 12. 2026 je sobota, 21. 12. 2026 pondelok - R 801 ide len v sobotu
        var saturday = await TrainTools.ListTrains(Editor(), day: new DateOnly(2026, 12, 19));
        var monday = await TrainTools.ListTrains(Editor(), day: new DateOnly(2026, 12, 21));
        var outside = await TrainTools.ListTrains(Editor(), day: new DateOnly(2028, 1, 1));

        Assert.AreEqual(3, saturday.Total);
        CollectionAssert.DoesNotContain(monday.Trains.Select(t => t.Number).ToArray(), "801");
        Assert.AreEqual(0, outside.Total);
    }

    [TestMethod]
    public async Task Vlak_DetailANeexistujuci()
    {
        var train = await TrainTools.GetTrain(Editor(), 2);

        Assert.AreEqual("801", train.Train.Number);
        Assert.AreEqual("R", train.Train.Type);
        Assert.AreEqual("Žilina", train.Train.To);
        CollectionAssert.AreEqual(new[] { "Veľká Ves" }, train.StationsBefore.ToArray());
        Assert.AreEqual(To, train.ValidTo);
        var missing = await Assert.ThrowsExactlyAsync<McpException>(() => TrainTools.GetTrain(Editor(), 99));
        StringAssert.StartsWith(missing.Message, "train.notFound:");
        var closed = await Assert.ThrowsExactlyAsync<McpException>(() => TrainTools.GetTrain(Editor(false), 1));
        StringAssert.StartsWith(closed.Message, "grafikon.notOpen:");
    }

    [TestMethod]
    public async Task Platnost_DniARozsahy()
    {
        var weekend = await TrainTools.ExplainValidity(Editor(), "jazdí v 6", new DateOnly(2026, 12, 14), new DateOnly(2026, 12, 27));
        var daily = await TrainTools.ExplainValidity(Editor(), "", new DateOnly(2026, 12, 14), new DateOnly(2026, 12, 20));

        Assert.AreEqual(2, weekend.Days);
        CollectionAssert.AreEqual(new[] { new DayRange(new DateOnly(2026, 12, 19), new DateOnly(2026, 12, 19)), new DayRange(new DateOnly(2026, 12, 26), new DateOnly(2026, 12, 26)) },
            weekend.Ranges.ToArray());
        Assert.AreEqual(7, daily.Days);
        Assert.HasCount(1, daily.Ranges);
    }

    [TestMethod]
    public async Task Stanica_ATabule()
    {
        var station = await GrafikonTools.GetStation(Editor());
        var tables = await TableTools.ListTables(Editor());
        var tabTab = await TableTools.GetTabTab(Editor(), "odchody");

        CollectionAssert.AreEqual(new[] { "1", "2" }, station.Tracks.Select(t => t.Key).ToArray());
        CollectionAssert.Contains(station.TrainTypes.ToArray(), "R - R");
        CollectionAssert.AreEqual(new[] { "ODCHODY", "DRUH" }, tables.TabTabs.ToArray());
        Assert.AreEqual("@VLAK", tabTab.Text);
        Assert.IsEmpty(tabTab.UsedBy);
    }

    [TestMethod]
    public async Task TabTab_HlaseniaPismaAPouzitie()
    {
        var druh = await TableTools.GetTabTab(Editor(), "druh");
        var reference = TableTools.GetTabTabReference();

        // druh X9 nie je medzi druhmi vlakov - varovanie s cislom riadka
        Assert.IsTrue(druh.Diagnostics.Any(d => d.Line == 3), string.Join("; ", druh.Diagnostics.Select(d => $"{d.Line}: {d.Message}")));
        CollectionAssert.AreEqual(new[] { 113, 34369 }, druh.Fonts.Select(f => f.Code).ToArray());
        Assert.AreEqual("red, extended font 6", druh.Fonts.Single(f => f.Code == 34369).Elen);
        StringAssert.StartsWith(druh.Fonts.Single(f => f.Code == 113).FontList, "Piktogramy");
        var usage = druh.UsedBy.Single();
        Assert.AreEqual(("K16", "Druh", "TAB1", 3), (usage.Catalog, usage.Column, usage.Table, usage.DivType!.Value));
        CollectionAssert.AreEqual(new[] { "T1" }, usage.Boards.ToArray());
        StringAssert.Contains(reference, "#VYLUKA");
    }

    [TestMethod]
    public async Task TabulovyText_AkoVInisseAjSVylukou()
    {
        var normal = await TableTools.PreviewBoardText(Editor(), 2, section: "druh");
        var lockout = await TableTools.PreviewBoardText(Editor(), 2, section: "druh", lockoutDeparture: true);
        var other = await TableTools.PreviewBoardText(Editor(), 2, section: "odchody");

        var column = normal.Columns.Single();
        Assert.AreEqual(("K16", "Druh", 3), (column.Catalog, column.Column, column.DivType));
        Assert.AreEqual("R", column.Text);
        Assert.AreEqual(34369, column.Font);
        Assert.IsNotEmpty(column.Steps);
        // pravidlo Bus=#VYLUKA - pri vyluke na odchode sa zobrazi Bus
        Assert.AreEqual("Bus", lockout.Columns.Single().Text);
        Assert.IsEmpty(other.Columns);
        Assert.AreEqual("801", normal.Train.Number);
    }

    [TestMethod]
    public async Task Analyza_VratiProblemyZoradenePodlaZavaznosti()
    {
        var dir = Path.Combine(Path.GetTempPath(), "McpToolsTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var context = new EditorContext(null!);
            context.OpenWorkspace(new InissWorkspace { INISSDir = dir, DataDir = dir, GVDDirs = [], Stations = [], TrainsTypes = new([Os]) });
            var document = new GrafikonDocument { Tracks = [Track.None, Track1], TabTabs = [new TableTabTab { Key = "PRAZDNA", Text = "" }] };
            document.Trains.Add(Train(1, "3001", Os, "10:15", Track1));
            // grafikon uz neplatny - analyza to hlasi
            context.OpenDocument(document, new GVDDirectory(new DirList { DirName = "G", FullPath = dir },
                new GVDInfo { ThisStation = new Station("99", "Dolné Mesto"), StartValidTimeTable = new DateOnly(2020, 1, 1), EndValidData = new DateOnly(2020, 12, 31) }));

            var result = await AnalysisTools.AnalyzeGrafikon(new EditorAccess(context, null));

            Assert.IsNotEmpty(result.Problems);
            Assert.AreEqual(result.Problems.Count(p => p.Severity == "error"), result.Errors);
            var order = result.Problems.Select(p => p.Severity switch { "error" => 0, "warning" => 1, _ => 2 }).ToList();
            CollectionAssert.AreEqual(order.Order().ToList(), order, "najprv chyby, potom upozornenia a rady");
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [TestMethod]
    public async Task TextyTabul_VsetkyZnovaAjSRucnymTextom()
    {
        var editor = Editor(out var context);

        var result = await TableTextTools.RegenerateBoardTexts(editor);
        var again = await TableTextTools.RegenerateBoardTexts(editor);

        var ciel = result.Texts.Single(t => t.Key == "CIEL");
        Assert.AreEqual(("K16/Cieľ", 2, 1, 0), (ciel.Column, ciel.Added, ciel.Changed, ciel.Removed));
        Assert.AreEqual(new TrainTextChange(1, "Os 3001", "Ručný text", "Brezová Dolina"), ciel.Changes[0]);
        Assert.AreEqual(new TrainTextChange(2, "R 801", null, "Žilina"), ciel.Changes[1]);
        Assert.IsNull(result.Texts.Single(t => t.Key == "POZNAMKA").Column, "bez stlpca s cielom ci stanicami sa typ preskoci");
        Assert.IsTrue(result.Change.Changed);
        Assert.IsFalse(result.Change.DryRun);
        StringAssert.EndsWith(result.Change.UndoStep, "(agent)");
        Assert.AreEqual(-1, context.Document.TableTexts[0].Trains[0].FontID, "pismo sa vrati na predvolene");
        Assert.IsFalse(again.Change.Changed, "druhe volanie uz nic nemeni");
        Assert.IsNull(again.Change.UndoStep);
    }

    [TestMethod]
    public async Task TextyTabul_LenChybajuceANanecisto()
    {
        var editor = Editor(out var context);

        var dry = await TableTextTools.RegenerateBoardTexts(editor, onlyMissing: true, text: "ciel", dryRun: true);
        Assert.HasCount(1, context.Document.TableTexts[0].Trains, "nanecisto sa grafikon nezmeni");
        var missing = await TableTextTools.RegenerateBoardTexts(editor, onlyMissing: true, text: "ciel");

        Assert.AreEqual((true, true), (dry.Change.DryRun, dry.Change.Changed));
        Assert.IsNull(dry.Change.UndoStep);
        var (planned, done) = (dry.Texts.Single(), missing.Texts.Single());
        Assert.AreEqual((planned.Added, planned.Changed, planned.Removed), (done.Added, done.Changed, done.Removed), "nanecisto rovnaky vysledok");
        CollectionAssert.AreEqual(planned.Changes.ToArray(), done.Changes.ToArray());
        Assert.AreEqual((2, 0), (done.Added, done.Changed));
        CollectionAssert.AreEqual(new[] { "Ručný text", "Žilina", "Brezová Dolina" }, context.Document.TableTexts[0].Trains.Select(t => t.Text).ToArray());
        var unknown = await Assert.ThrowsExactlyAsync<McpException>(() => TableTextTools.RegenerateBoardTexts(editor, text: "INY"));
        StringAssert.StartsWith(unknown.Message, "tableText.notFound:");
    }

    [TestMethod]
    public async Task Zmena_ChybaVratiGrafikonAOknoDostaneKrok()
    {
        var host = new Host();
        var editor = Editor(out var context, host: host);

        var failed = await Assert.ThrowsExactlyAsync<McpException>(() => editor.WriteAsync<int>("test", false, c =>
        {
            c.Document.Trains.Clear();
            throw new McpException("test.fail: x");
        }));
        await TableTextTools.RegenerateBoardTexts(editor);
        await TableTextTools.RegenerateBoardTexts(editor, dryRun: true);

        Assert.AreEqual("test.fail: x", failed.Message);
        Assert.HasCount(3, context.Document.Trains, "chyba vratila grafikon do povodneho stavu");
        Assert.HasCount(3, host.Steps);
        Assert.IsNull(host.Steps[0]);
        StringAssert.EndsWith(host.Steps[1], "(agent)");
        Assert.IsNull(host.Steps[2], "nanecisto bez kroku");
        var busy = await Assert.ThrowsExactlyAsync<McpException>(() => TableTextTools.RegenerateBoardTexts(Editor(out _, host: new Host("editor.busy: x"))));
        StringAssert.StartsWith(busy.Message, "editor.busy:");
        var closed = await Assert.ThrowsExactlyAsync<McpException>(() => TableTextTools.RegenerateBoardTexts(Editor(false)));
        StringAssert.StartsWith(closed.Message, "grafikon.notOpen:");
    }

    /// <summary>Hlavne okno v teste: zapamata kroky spat zmien agentom (null = bez kroku), pripadne je zaneprazdnene.</summary>
    private sealed class Host(string? busy = null) : IAgentHost
    {
        public List<string?> Steps { get; } = [];

        public void Change(Func<string?> change)
        {
            if (busy is not null)
                throw new McpException(busy);
            string? step = null;
            try
            {
                step = change();
            }
            finally
            {
                Steps.Add(step);
            }
        }
    }

    [TestMethod]
    public void Nastroje_PravidlaPopisu()
    {
        var methods = GvdMcpServer.ToolMethods();

        Assert.IsGreaterThanOrEqualTo(10, methods.Count);
        Assert.IsTrue(methods.Any(m => !GvdMcpServer.IsReadOnly(m)), "aspon jeden nastroj so zmenami");
        foreach (var method in methods)
        {
            var tool = method.GetCustomAttribute<McpServerToolAttribute>()!;
            var named = method.CustomAttributes.Single(a => a.AttributeType == typeof(McpServerToolAttribute)).NamedArguments.Select(a => a.MemberName).ToList();
            string[] hints = tool.ReadOnly ? ["ReadOnly", "OpenWorld"] : ["ReadOnly", "OpenWorld", "Destructive", "Idempotent"];
            Assert.IsTrue(SnakeCase().IsMatch(tool.Name!), $"{tool.Name}: snake_case");
            Assert.IsFalse(tool.OpenWorld, $"{tool.Name}: OpenWorld explicitne false");
            foreach (var hint in hints)
                CollectionAssert.Contains(named, hint, $"{tool.Name}: {hint} explicitne");
            if (!tool.ReadOnly)
                Assert.IsTrue(method.GetParameters().Any(p => p.Name == "dryRun" && p.ParameterType == typeof(bool)), $"{tool.Name}: zmena ma dryRun");
            Assert.IsNotNull(method.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>(), $"{tool.Name}: popis");
            foreach (var parameter in method.GetParameters().Where(p => p.ParameterType != typeof(EditorAccess) && p.ParameterType != typeof(CancellationToken)))
                Assert.IsNotNull(parameter.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>(), $"{tool.Name}.{parameter.Name}: popis");
        }
        Assert.AreEqual(methods.Count, methods.Select(m => m.GetCustomAttribute<McpServerToolAttribute>()!.Name).Distinct().Count(), "jedinecne nazvy");
    }
}
