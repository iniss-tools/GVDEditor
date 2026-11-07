using GVDEditor.Integration;
using ToolsCore.Iniss.StateDgm;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Prevod stavoveho diagramu INISS 3.39 na statedgm.xml INISS2.
/// </summary>
[TestClass]
public class Iniss2StateDgmConverterTests
{
    private static readonly Dictionary<string, int> TrainTypes = new(StringComparer.Ordinal) { ["Os"] = 0, ["R"] = 26 };

    private static readonly HashSet<string> Reports = new(StringComparer.Ordinal) { "Přijíždí", "Odjede", "Zpoždění" };

    private static Iniss2StateDgmConverter Converter() =>
        new(new Iniss2ExpressionTranspiler(TrainTypes, name => name == "1" ? "K1" : null), key => Reports.Contains(key) ? key + "_" : null);

    private static StateDgmEvent Go(string key, string next, string? report = null) =>
        new() { Key = key, NextState = next, ReportKey = report, Class = "SDEventUniPos" };

    private static StateDgmEvent Dialog(string key, string dialog) => new() { Key = key, Dialog = dialog, Class = "SDEventWithDialog" };

    private static StateDgmControl Control(int id, string design, string eventKey) => new() { CtrlId = id, DesignKey = design, EventKey = eventKey };

    // kategoria ako v predlohe: dialogy na 0 a 1, tlacidla stavov za nimi, kazdy stav ma rovnake akcie a ovladace
    private static StateDgmCategory Category(string key, params StateDgmState[] states)
    {
        var category = new StateDgmCategory { Key = key };
        foreach (var state in states)
        {
            state.Events.AddRange([Dialog("#SetKolej", "SDDlgKolej"), Dialog("#SetZpozdeni", "SDDlgZpozdeniG"), Go("#GoToPrijde", "Přijíždí", "Přijíždí"),
                Go("#GoToOdide", "Odjede", "Odjede"), Go("#GoToZmaz", "Odbaven")]);
            state.Controls.AddRange([Control(0, "KoľajN", "#SetKolej"), Control(1, "MeškanieN", "#SetZpozdeni"), Control(4, "Príde3", "#GoToPrijde"),
                Control(8, "OdídeD", "#GoToOdide"), Control(9, "Zmaž1", "#GoToZmaz")]);
            category.States.Add(state);
        }

        return category;
    }

    private static StateDgmDiagram Diagram(params StateDgmCategory[] categories)
    {
        var diagram = new StateDgmDiagram();
        diagram.Categories.AddRange(categories);
        return diagram;
    }

    private static StateDgmCategory Simple(string key = "#Projíždějící vlak") => Category(key,
        new StateDgmState
        {
            Key = "#Start", Attr = StateDgmAttr.Stoji,
            DoState = new StateDgmTableSet { OnDepartureTable = true, OnArrivalTable = true }
        },
        new StateDgmState
        {
            Key = "Přijíždí", Name = "Príde", Attr = StateDgmAttr.PotvrzenaKolej,
            DoState = new StateDgmTableSet { OnDepartureTable = true, OnPlatformTables = true, ShowPosition = true, ShowTrack = true },
            AutoMode = StateDgmDynamic.FromNumber(2), AutoTimePoint = StateDgmDynamic.FromNumber(1), AutoTimePointAdd = StateDgmDynamic.FromNumber(-300)
        },
        new StateDgmState
        {
            Key = "Odjede", Attr = StateDgmAttr.Stoji | StateDgmAttr.PotvrzenaKolej | StateDgmAttr.NyniStoji,
            AutoMode = StateDgmDynamic.FromNumber(1), AutoTimePoint = StateDgmDynamic.FromNumber(2), AutoModif = StateDgmDynamic.FromNumber(1),
            Wait = StateDgmDynamic.FromExpression("OVC"), AutoCondition = "Typ(Typ_R)"
        },
        new StateDgmState
        {
            Key = "Odbaven", Attr = StateDgmAttr.Odbaven | StateDgmAttr.Shadow, DoState = new StateDgmTableSet(),
            AutoMode = StateDgmDynamic.FromNumber(2), AutoTimePoint = StateDgmDynamic.FromNumber(2), AutoTimePointAdd = StateDgmDynamic.FromNumber(300)
        });

    [TestMethod]
    public void Prevod_KategorieSuGrafyAPomocneStavyDialogov()
    {
        var converter = Converter();
        var result = converter.Convert(Diagram(Simple()));

        var nodes = result.Graphs.Single().Nodes;
        Assert.AreEqual("#Projíždějící vlak", result.Graphs[0].Description);
        CollectionAssert.AreEqual(new[] { "Kolej", "Zpozdeni", "#Start", "Přijíždí", "Odjede", "Odbaven" }, nodes.Select(node => node.Key).ToArray());
        Assert.AreEqual(new StateButton("Koľaj", 0), nodes[0].Button);
        Assert.AreEqual(new StateButton("Meškanie", 1), nodes[1].Button);
        CollectionAssert.AreEqual(new[] { StateNodeFlags.Auxiliary }, nodes[1].Flags.ToArray());
        Assert.IsNull(nodes[2].Button, "do prveho stavu ziadna akcia nevedie");
        Assert.AreEqual(new StateButton("Príde", 4), nodes[3].Button);
        Assert.AreEqual(new StateButton("Odíde", 8), nodes[4].Button);
        Assert.AreEqual(("Príde", "Přijíždí_"), (nodes[3].Name, nodes[3].Report));
        Assert.AreEqual("", nodes[5].Report, "akcia bez hlasenia");
        Assert.IsEmpty(converter.Warnings);
    }

    [TestMethod]
    public void Prevod_PriznakyZAttrAStavuTabul()
    {
        var nodes = Converter().Convert(Diagram(Simple())).Graphs[0].Nodes;

        CollectionAssert.AreEqual(new[] { "STOJI", "JENAODJEZDOVE", "JENAPRIJEZDOVE" }, nodes[2].Flags.ToArray());
        CollectionAssert.AreEqual(new[] { "POTVRZENAKOLEJ", "JENAODJEZDOVE", "JENASMEROVYCH", "JEZOBRAZENAPOZICE", "JEZOBRAZENAKOLEJ" },
            nodes[3].Flags.ToArray());
        CollectionAssert.AreEqual(new[] { "STOJI", "POTVRZENAKOLEJ", "NYNISTOJI", "JENAODJEZDOVE", "JENASMEROVYCH", "JEZOBRAZENAPOZICE", "JEZOBRAZENAKOLEJ" },
            nodes[4].Flags.ToArray(), "stav bez DoState necha tabule ako predchadzajuci");
        CollectionAssert.AreEqual(new[] { "ODBAVEN", "SHADOW" }, nodes[5].Flags.ToArray());
    }

    [TestMethod]
    public void Prevod_AutomatikaSCasomPodmienkouACakanim()
    {
        var nodes = Converter().Convert(Diagram(Simple())).Graphs[0].Nodes;

        var arrival = nodes[3].Automation!;
        Assert.AreEqual((".DatumCasPrij(True).AddSeconds(-300)", "2", "", (int?)null), (arrival.Time, arrival.Mode, arrival.Condition, arrival.Variant));
        CollectionAssert.AreEqual(new[] { StateNodeFlags.KeepWithoutClient }, arrival.Flags.ToArray());
        var departure = nodes[4].Automation!;
        Assert.AreEqual((".DatumCasOdj(True)", "1", 1), (departure.Time, departure.Mode, departure.Variant));
        Assert.AreEqual("((.Druh = \"R\")) AndAlso (((.PriznakyGTN And StaniceVlaku.FLAGS.OVC) <> 0))", departure.Condition);
        Assert.AreEqual(".DatumCasOdj(True).AddSeconds(300)", nodes[5].Automation!.Time);
        Assert.IsNull(nodes[2].Automation);
    }

    [TestMethod]
    public void Prevod_IndexGrafuMimoRozsahuJePoslednyGraf()
    {
        var two = Converter().Convert(Diagram(Simple("A"), Simple("B")));
        var six = Converter().Convert(Diagram([.. Enumerable.Range(1, 6).Select(i => Simple(i.ToString(System.Globalization.CultureInfo.InvariantCulture)))]));
        var custom = Diagram(Simple("A"), Simple("B"));
        custom.IndCat = "Typ(Typ_R) + 1";

        Assert.AreEqual("Dim c As Integer = .IndCat6() : Return If(c >= 1 AndAlso c <= 2, c, 2)", two.GraphIndex);
        Assert.AreEqual(".IndCat6()", six.GraphIndex);
        Assert.AreEqual("Dim c As Integer = (If((.Druh = \"R\"), 1, 0) + 1) : Return If(c >= 1 AndAlso c <= 2, c, 2)", Converter().Convert(custom).GraphIndex);
    }

    [TestMethod]
    public void Prevod_StarterJePomocnyStavSOpakovanim()
    {
        var category = Simple();
        var start = category.States[0];
        start.Events.Add(new StateDgmEvent { Key = "#ReportZpozdeni", ReportKey = "Zpoždění", Class = "SDEventUniPos" });
        start.Starters.Add(new StateDgmStarter
        {
            Key = "AutoZpozdeni", EventKey = "#ReportZpozdeni", TimePointKey = "#Pravidelný odjezd", TimeOffset = -360, TimeOffsetStep = 600,
            TimePointKeyLast = "#Předpokládaný odjezd", TimeOffsetLast = -300
        });
        category.States[1].Starters.Add(new StateDgmStarter { Key = "Bez", EventKey = "#Chyba", TimePointKey = "#Pravidelný odjezd" });
        var converter = Converter();

        var nodes = converter.Convert(Diagram(category)).Graphs[0].Nodes;

        var starter = nodes.Single(node => node.Key == "AutoZpozdeni");
        Assert.AreEqual(2, nodes.IndexOf(starter), "pomocne stavy idu pred stavy");
        Assert.AreEqual(("Zpoždění_", "(.Node.sKey = \"#Start\")"), (starter.Report, starter.Condition));
        CollectionAssert.AreEqual(new[] { StateNodeFlags.Auxiliary }, starter.Flags.ToArray());
        var auto = starter.Automation!;
        Assert.AreEqual("Dim f As DateTime = .DatumCasOdj(False).AddSeconds(-360) : Return If(Now < f.AddSeconds(10), f, "
                        + "f.AddSeconds(Math.Ceiling(((Now - f).TotalSeconds - 10) / 600) * 600))", auto.Time);
        Assert.AreEqual(("2", "600", ".DatumCasOdj(True).AddSeconds(-290)"), (auto.Mode, auto.Repeat, auto.End));
        Assert.AreEqual(new Iniss2ExportWarning(Iniss2ExportIssue.StatePartNotConverted, "#Projíždějící vlak/Přijíždí: Starter Bez → #Chyba"),
            converter.Warnings.Single());
    }

    [TestMethod]
    [DataRow(true, ".DatumCasPrij(True)")]
    [DataRow(false, "Dim f As DateTime = .DatumCasPrij(True) : Return If(Now < f.AddSeconds(10), f, DateTime.MaxValue)")]
    public void Prevod_StarterBezKrokuPodlaStartLaterToo(bool later, string time)
    {
        var category = Simple();
        category.States[0].Events.Add(new StateDgmEvent { Key = "#Report", ReportKey = "Zpoždění", Class = "SDEventUniPos" });
        category.States[0].Starters.Add(new StateDgmStarter { Key = "Raz", EventKey = "#Report", TimePointKey = "#Předpokládaný příjezd", StartLaterToo = later });

        var auto = Converter().Convert(Diagram(category)).Graphs[0].Nodes.Single(node => node.Key == "Raz").Automation!;

        Assert.AreEqual((time, "", ""), (auto.Time, auto.Repeat, auto.End));
    }

    [TestMethod]
    public void Prevod_VlastnyCasovyBodAStartTime()
    {
        var diagram = Diagram(Simple());
        diagram.TimePoints.Add(new StateDgmTimePoint
        {
            Key = "Skor", TimePointKey1 = "#Pravidelný odjezd", Offset1 = -60, TimePointKey2 = "#Předpokládaný odjezd", Operator = StateDgmKeys.OperatorMin
        });
        var start = diagram.Categories[0].States[0];
        start.Events.Add(new StateDgmEvent { Key = "#Report", ReportKey = "Zpoždění", Class = "SDEventUniPos" });
        start.Starters.Add(new StateDgmStarter { Key = "Vlastny", EventKey = "#Report", TimePointKey = "Skor", StartLaterToo = true });
        start.Starters.Add(new StateDgmStarter { Key = "Vstup", EventKey = "#Report", TimePointKey = StateDgmKeys.StartTime, StartLaterToo = true });
        var converter = Converter();

        var nodes = converter.Convert(diagram).Graphs[0].Nodes;

        Assert.AreEqual("If(.DatumCasOdj(False).AddSeconds(-60) <= .DatumCasOdj(True), .DatumCasOdj(False).AddSeconds(-60), .DatumCasOdj(True))",
            nodes.Single(node => node.Key == "Vlastny").Automation!.Time);
        Assert.IsFalse(nodes.Any(node => node.Key == "Vstup"), "cas vstupu do stavu INISS2 nema");
        Assert.AreEqual(Iniss2ExportIssue.StatePartNotConverted, converter.Warnings.Single().Issue);
    }

    [TestMethod]
    public void Prevod_HlaseniaAkcii()
    {
        var category = Simple();
        category.States[3].Events.Add(Go("#Iny", "Přijíždí", "Vjíždí"));
        category.States[0].Events.Single(e => e.Key == "#GoToOdide").ReportKey = "Neznáme";
        var converter = Converter();

        var nodes = converter.Convert(Diagram(category)).Graphs[0].Nodes;

        Assert.AreEqual("Přijíždí_", nodes[3].Report, "hlasenie akcie s tlacidlom ma prednost");
        CollectionAssert.AreEqual(new[]
        {
            new Iniss2ExportWarning(Iniss2ExportIssue.StateReportConflict, "#Projíždějící vlak/Přijíždí: Přijíždí, Vjíždí"),
            new Iniss2ExportWarning(Iniss2ExportIssue.StateReportConflict, "#Projíždějící vlak/Odjede: Neznáme, Odjede"),
            new Iniss2ExportWarning(Iniss2ExportIssue.StateReportMissing, "Neznáme")
        }, converter.Warnings.ToArray());
    }

    [TestMethod]
    public void Prevod_NepodporovaneCastiANeprelozitelnyVyraz()
    {
        var category = Simple();
        category.States[1].UndoState = new StateDgmTableSet();
        category.States[1].Events.Add(new StateDgmEvent { Key = "#Skry", NextState = "Odjede", ReportKey = "Odjede", HideShow = 1, Class = "SDEventUniPos" });
        category.States[2].AutoCondition = "NEZNAMA(";
        var converter = Converter();

        var nodes = converter.Convert(Diagram(category)).Graphs[0].Nodes;

        Assert.AreEqual("(((.PriznakyGTN And StaniceVlaku.FLAGS.OVC) <> 0))", nodes[4].Automation!.Condition, "neprelozitelna podmienka sa vynecha");
        CollectionAssert.AreEquivalent(new[]
        {
            new Iniss2ExportWarning(Iniss2ExportIssue.StatePartNotConverted, "#Projíždějící vlak/Přijíždí: Event #Skry"),
            new Iniss2ExportWarning(Iniss2ExportIssue.StatePartNotConverted, "#Projíždějící vlak/Přijíždí: UndoState"),
            new Iniss2ExportWarning(Iniss2ExportIssue.StateExpressionNotConverted, "#Projíždějící vlak/Odjede: AutoCondition = NEZNAMA(")
        }, converter.Warnings.ToArray());
    }

    [TestMethod]
    public void Prevod_TlacidlaMimoPoradiaSaPrecisluju()
    {
        var category = Simple();
        foreach (var state in category.States)
            state.Controls.Single(control => control.CtrlId == 8).CtrlId = 2;

        var buttons = Converter().Convert(Diagram(category)).Graphs[0].Nodes.Where(node => node.Button is not null).Select(node => node.Button!.Position);

        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, buttons.ToArray());
    }
}
