using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Entities;

namespace GVDEditor.Tests;

/// <summary>
///     Radenia v okne vlaku: uprava sa da zrusit (okno meni kopie), pri ulozeni ostane zachovana identita objektov
///     zdielanych medzi GlobData.Radenia a vlakmi s rovnakym cislom a radenie ineho cisla sa nezmeni.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class RadeniaEditingTests
{
    private static readonly ReportType Odjede = new("Odjede", "Odjede", "O");
    private static readonly List<ReportVariant> Variants = ReportVariant.GetDefaultValues();

    private static readonly FyzSound SoundA = new() { Name = "R001" };
    private static readonly FyzSound SoundB = new() { Name = "R002" };

    private static Radenie NewRadenie(string number, string datObm) => new()
    {
        CisloVlaku = number,
        ZacPlatnosti = new DateTime(2026, 1, 1),
        KonPlatnosti = new DateTime(2026, 12, 31),
        DatObm = datObm,
        Text = "A",
        Sounds = [SoundA],
        ChosenReports = [new ChosenReportType { Type = Odjede, Variants = [Variants[0]] }]
    };

    private sealed record Grafikon(List<Radenie> Global, Train A, Train B, Train Other)
    {
        public List<Train> Trains => [A, B, Other];
        public Radenie R1 => Global[0];
        public Radenie R2 => Global[1];
        public Radenie R3 => Global[2];
    }

    // dva vlaky s cislom 1001 (varianty) zdielaju tie iste radenia s GlobData.Radenia, vlak 2002 ma vlastne
    private static Grafikon Graph()
    {
        var r1 = NewRadenie("1001", "jede denne");
        var r2 = NewRadenie("1001", "jede v 6");
        var r3 = NewRadenie("2002", "jede denne");

        var a = new Train { Number = "1001" };
        var b = new Train { Number = "1001" };
        var other = new Train { Number = "2002" };
        a.Radenia.AddRange([r1, r2]);
        b.Radenia.AddRange([r1, r2]);
        other.Radenia.Add(r3);

        return new Grafikon([r1, r2, r3], a, b, other);
    }

    // to, co robi tlacidlo Upravit na zalozke Radenie
    private static void Edit(Radenie radenie)
    {
        radenie.DatObm = "jede v 7";
        radenie.ZacPlatnosti = new DateTime(2026, 2, 1);
        radenie.Text = "B";
        radenie.Sounds = [SoundB];
        radenie.ChosenReports = [new ChosenReportType { Type = Odjede, Variants = [Variants[1]] }];
    }

    private static void AssertUnchanged(Radenie radenie, string datObm)
    {
        Assert.AreEqual(datObm, radenie.DatObm);
        Assert.AreEqual("A", radenie.Text);
        CollectionAssert.AreEqual(new[] { SoundA }, radenie.Sounds);
        Assert.HasCount(1, radenie.ChosenReports);
        CollectionAssert.AreEqual(new[] { Variants[0] }, radenie.ChosenReports[0].Variants);
    }

    [TestMethod]
    public void RadeniaVlaku_UpravaBezUlozenia_NezmeniGrafikon()
    {
        var g = Graph();

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Assert.AreNotSame(g.R1, editing.Items[0]);

        Edit(editing.Items[0]);
        editing.Items[0].ChosenReports.Add(new ChosenReportType { Type = Odjede });
        editing.Items.RemoveAt(1);

        AssertUnchanged(g.R1, "jede denne");
        CollectionAssert.AreEqual(new[] { g.R1, g.R2 }, g.A.Radenia);
        Assert.HasCount(3, g.Global);
    }

    [TestMethod]
    public void RadeniaVlaku_UlozenieUpravy_PrepisePovodnyObjektVoVsetkychZoznamoch()
    {
        var g = Graph();
        var r1 = g.R1;

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Edit(editing.Items[0]);
        editing.Commit(g.A, g.Global, g.Trains);

        CollectionAssert.AreEqual(new[] { r1, g.R2, g.R3 }, g.Global);
        CollectionAssert.AreEqual(new[] { r1, g.R2 }, g.A.Radenia);
        CollectionAssert.AreEqual(new[] { r1, g.R2 }, g.B.Radenia);

        Assert.AreEqual("jede v 7", r1.DatObm);
        Assert.AreEqual(new DateTime(2026, 2, 1), r1.ZacPlatnosti);
        Assert.AreEqual("B", r1.Text);
        CollectionAssert.AreEqual(new[] { SoundB }, r1.Sounds);
        CollectionAssert.AreEqual(new[] { Variants[1] }, r1.ChosenReports.Single().Variants);
    }

    [TestMethod]
    public void RadeniaVlaku_UlozeniePridanychAOdstranenych_ZmeniLenVlakySRovnakymCislom()
    {
        var g = Graph();
        var (r1, r2, r3) = (g.R1, g.R2, g.R3);

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        editing.Items.RemoveAt(1);
        var added = NewRadenie(null!, "jede v 7");
        editing.Items.Add(added);
        editing.Commit(g.A, g.Global, g.Trains);

        CollectionAssert.AreEqual(new[] { r1, r3, added }, g.Global);
        CollectionAssert.AreEqual(new[] { r1, added }, g.A.Radenia);
        CollectionAssert.AreEqual(new[] { r1, added }, g.B.Radenia);
        CollectionAssert.AreEqual(new[] { r3 }, g.Other.Radenia);
        Assert.AreEqual("1001", added.CisloVlaku);
        Assert.AreEqual("jede v 6", r2.DatObm);
    }

    [TestMethod]
    public void RadeniaVlaku_PrevzatieRadeniInehoVlaku_UpraviJehoObjektyAOdstraniOsiroteneRadenia()
    {
        var g = Graph();
        var trains = new List<Train> { g.A, g.Other };
        var r3 = g.R3;

        // jediny vlak 1001 zmeni cislo na 2002 a prevezme radenia vlaku 2002
        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Edit(editing.Items[0]);
        Assert.IsFalse(editing.Shows(g.Other.Radenia));
        editing.Load(g.Other.Radenia);
        Edit(editing.Items[0]);
        g.A.Number = "2002";
        editing.Commit(g.A, g.Global, trains);

        // radenia cisla 1001 uz nema ziadny vlak
        CollectionAssert.AreEqual(new[] { r3 }, g.Global);
        CollectionAssert.AreEqual(new[] { r3 }, g.A.Radenia);
        CollectionAssert.AreEqual(new[] { r3 }, g.Other.Radenia);
        Assert.AreEqual("jede v 7", r3.DatObm);
        Assert.AreEqual("2002", r3.CisloVlaku);
    }

    [TestMethod]
    public void RadeniaVlaku_CisloUzSaNezhodujeSInymVlakom_VratiVlastneRadenia()
    {
        var g = Graph();
        var (r1, r2, r3) = (g.R1, g.R2, g.R3);

        var editing = new RadeniaEditing();
        editing.LoadOwn(g.A.Radenia);
        Edit(editing.Items[0]);

        // vlastne radenia zobrazene - uprava ostane
        editing.RestoreOwn();
        Assert.AreEqual("jede v 7", editing.Items[0].DatObm);

        // cislo 2002 prevzalo radenie, potom sa zmenilo na 3003 (bez radenia)
        editing.Load(g.Other.Radenia);
        editing.RestoreOwn();
        Assert.IsTrue(editing.Shows(g.A.Radenia));
        Assert.HasCount(2, editing.Items);

        g.A.Number = "3003";
        editing.Commit(g.A, g.Global, g.Trains);

        // vlak B si radenia 1001 necha, vlak A dostane vlastne kopie; radenie vlaku 2002 ostane
        CollectionAssert.AreEqual(new[] { r1, r2 }, g.B.Radenia);
        CollectionAssert.AreEqual(new[] { r3 }, g.Other.Radenia);
        Assert.HasCount(2, g.A.Radenia);
        Assert.IsTrue(g.A.Radenia.All(r => r.CisloVlaku == "3003" && r != r1 && r != r2));
        Assert.HasCount(5, g.Global);
    }

    [TestMethod]
    public void RadeniaVlaku_VariantaSRovnakymCislom_ZobrazujeRovnakeRadenia()
    {
        var g = Graph();

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Edit(editing.Items[0]);
        editing.Items.RemoveAt(1);

        // okno uz zobrazuje radenia vlaku B - prevzatim by sa upravy zahodili
        Assert.IsTrue(editing.Shows(g.B.Radenia));
        Assert.IsFalse(editing.Shows(g.Other.Radenia));
    }

    [TestMethod]
    public void RadeniaVlaku_KopiaVlakuSInymCislom_DostaneVlastneRadeniaAZdrojOstaneNezmeneny()
    {
        var g = Graph();
        var (r1, r2, r3) = (g.R1, g.R2, g.R3);

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Edit(editing.Items[0]);
        editing.Items.RemoveAt(1);
        var copy = new Train { Number = "3003" };
        editing.Commit(copy, g.Global, g.Trains);

        var own = copy.Radenia.Single();
        Assert.AreNotSame(r1, own);
        Assert.AreEqual("3003", own.CisloVlaku);
        Assert.AreEqual("jede v 7", own.DatObm);
        CollectionAssert.AreEqual(new[] { r1, r2, r3, own }, g.Global);

        AssertUnchanged(r1, "jede denne");
        Assert.AreEqual("1001", r1.CisloVlaku);
        CollectionAssert.AreEqual(new[] { r1, r2 }, g.A.Radenia);
        CollectionAssert.AreEqual(new[] { r1, r2 }, g.B.Radenia);
    }

    [TestMethod]
    public void RadeniaVlaku_KopiaVlakuSRovnakymCislom_ZdielaRadeniaSoZdrojom()
    {
        var g = Graph();
        var r1 = g.R1;

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        Edit(editing.Items[0]);
        var copy = new Train { Number = "1001" };
        editing.Commit(copy, g.Global, g.Trains);

        Assert.HasCount(3, g.Global);
        Assert.AreEqual("jede v 7", r1.DatObm);
        CollectionAssert.AreEqual(new[] { r1, g.R2 }, copy.Radenia);
        CollectionAssert.AreEqual(new[] { r1, g.R2 }, g.A.Radenia);
    }

    [TestMethod]
    public void RadeniaVlaku_NovyVlak_DostaneRadeniaUzHnedPoUlozeni()
    {
        var g = Graph();

        // novy vlak s cislom, ktore este radenie nema
        var editing = new RadeniaEditing();
        editing.Load([]);
        var added = NewRadenie(null!, "jede denne");
        editing.Items.Add(added);
        var train = new Train { Number = "4004" };
        editing.Commit(train, g.Global, g.Trains);

        CollectionAssert.AreEqual(new[] { added }, train.Radenia);
        Assert.AreEqual("4004", added.CisloVlaku);
        Assert.HasCount(4, g.Global);

        // novy vlak s cislom 1001 prevezme radenia variant
        editing = new RadeniaEditing();
        editing.Load([]);
        editing.Load(g.A.Radenia);
        train = new Train { Number = "1001" };
        editing.Commit(train, g.Global, g.Trains);

        CollectionAssert.AreEqual(new[] { g.R1, g.R2 }, train.Radenia);
        Assert.HasCount(4, g.Global);
    }

    [TestMethod]
    public void RadeniaVlaku_ZmenaCislaJednejVarianty_RadeniaOstanuDruhejVariante()
    {
        var g = Graph();
        var (r1, r2) = (g.R1, g.R2);

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        editing.Items.RemoveAt(1);
        g.A.Number = "1002";
        editing.Commit(g.A, g.Global, g.Trains);

        CollectionAssert.AreEqual(new[] { r1, r2 }, g.B.Radenia);
        Assert.AreEqual("1001", r1.CisloVlaku);
        var own = g.A.Radenia.Single();
        Assert.AreNotSame(r1, own);
        Assert.AreEqual("1002", own.CisloVlaku);
        Assert.HasCount(4, g.Global);
    }

    [TestMethod]
    public void RadeniaVlaku_ZmenaCislaJedinehoVlaku_PrecislujePovodneRadenia()
    {
        var g = Graph();
        var trains = new List<Train> { g.A, g.Other };
        var (r1, r2, r3) = (g.R1, g.R2, g.R3);

        var editing = new RadeniaEditing();
        editing.Load(g.A.Radenia);
        editing.Items.RemoveAt(1);
        g.A.Number = "1002";
        editing.Commit(g.A, g.Global, trains);

        CollectionAssert.AreEqual(new[] { r1, r3 }, g.Global);
        Assert.AreEqual("1002", r1.CisloVlaku);
        CollectionAssert.AreEqual(new[] { r1 }, g.A.Radenia);
        CollectionAssert.DoesNotContain(g.Global, r2);
    }
}
