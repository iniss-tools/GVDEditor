using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Varianty vlaku: cisla prideluje GVDEditor (INISS ich pouziva len ako identifikator), spolocne dni sa
///     prideluju jednej variante, pruh kalendara a zmena cisla celej skupiny.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainVariantsTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly TrainType R = new("R");
    private static readonly DateTime From = new(2026, 12, 13);
    private static readonly DateTime To = new(2027, 12, 11);

    private static Train T(string number, int variant, string limit = "", string name = "", TrainType? type = null) => new()
    {
        Number = number,
        Name = name,
        Type = type ?? Os,
        Variant = variant,
        DateLimitText = limit,
        ZaciatokPlatnosti = From,
        KoniecPlatnosti = To
    };

    private static TrainDraft Draft(Train train, List<Train> trains)
    {
        var draft = TrainDraft.From(train);
        draft.LoadSiblings(trains, trains.IndexOf(train));
        return draft;
    }

    [TestMethod]
    public void Cislovanie_JedinyVlak_DostaneMinusJedna()
    {
        var train = T("100", 3);

        CollectionAssert.AreEqual(new[] { train }, TrainVariants.Normalize([train]));
        Assert.AreEqual(-1, train.Variant);
    }

    [TestMethod]
    public void Cislovanie_PlatneCislaOstanu_OstatneDostanuNajmensieVolne()
    {
        List<Train> trains = [T("100", 3), T("100", -1), T("100", 3), T("100", 0), T("200", -1)];

        var changed = TrainVariants.Normalize(trains);

        CollectionAssert.AreEqual(new[] { 3, 1, 2, 4, -1 }, trains.Select(t => t.Variant).ToArray());
        CollectionAssert.AreEqual(new[] { trains[1], trains[2], trains[3] }, changed);
    }

    [TestMethod]
    public void Cislovanie_Medzery_SaNezhustuju()
    {
        List<Train> trains = [T("100", 1), T("100", 3)];

        Assert.IsEmpty(TrainVariants.Normalize(trains));
        CollectionAssert.AreEqual(new[] { 1, 3 }, trains.Select(t => t.Variant).ToArray());
    }

    [TestMethod]
    public void Cislovanie_InyNazovAleboTyp_JeInaSkupina()
    {
        List<Train> trains = [T("100", -1), T("100", -1, name: "Tatran"), T("100", -1, type: R)];

        Assert.IsEmpty(TrainVariants.Normalize(trains));
    }

    [TestMethod]
    public void Cislovanie_NovaVariantaKJedinemuVlaku_PovodnyDostaneJedna()
    {
        List<Train> trains = [T("100", -1), T("100", -1)];

        TrainVariants.Normalize(trains);

        CollectionAssert.AreEqual(new[] { 1, 2 }, trains.Select(t => t.Variant).ToArray());
    }

    [TestMethod]
    public void Poradie_NovyVlakJePosledny_UpravovanyPodlaCisla()
    {
        List<Train> trains = [T("100", 5), T("100", 2), T("100", 9)];
        var edited = Draft(trains[0], trains);
        var others = TrainVariants.Others(edited, new TrainContext(trains, 0));

        CollectionAssert.AreEqual(new[] { trains[1], trains[2] }, others, "zoradene podla cisla");
        Assert.AreEqual((2, 3), TrainVariants.PositionOf(edited, others));
        Assert.AreEqual(1, TrainVariants.PositionOf(trains[1], edited, others));
        Assert.AreEqual(3, TrainVariants.PositionOf(trains[2], edited, others));

        var copy = TrainDraft.From(trains[0]);
        copy.Variant = -1;
        Assert.AreEqual((4, 4), TrainVariants.PositionOf(copy, TrainVariants.Others(copy, new TrainContext(trains, 3))));
    }

    [TestMethod]
    public void SpolocneDni_PatriaTomutoVlaku_OdoberuSaDruhejVariante()
    {
        List<Train> trains = [T("100", 1, "ide v 1-6"), T("100", 2, "ide v 6,7")];
        var draft = Draft(trains[0], trains);

        Assert.IsTrue(TrainVariants.GiveCommonDays(draft, trains[1], toThis: true));

        Assert.AreEqual("ide v 1-6", draft.DateLimitText);
        Assert.AreEqual("ide v 7", draft.LimitOf(trains[1]));
        Assert.AreEqual("ide v 6,7", trains[1].DateLimitText, "druhy vlak sa zmeni az po ulozeni");
    }

    [TestMethod]
    public void SpolocneDni_PatriaDruhejVariante_OdoberuSaTomutoVlaku()
    {
        List<Train> trains = [T("100", 1, "ide v 1-6"), T("100", 2, "ide v 6,7")];
        var draft = Draft(trains[0], trains);

        Assert.IsTrue(TrainVariants.GiveCommonDays(draft, trains[1], toThis: false));

        Assert.AreEqual("ide v 1-5", draft.DateLimitText);
        Assert.IsEmpty(draft.VariantLimits);
    }

    [TestMethod]
    public void Kalendar_DniVariantAPrekrytie()
    {
        List<Train> trains = [T("100", 1, "ide v 1-6"), T("100", 2, "ide v 6,7")];
        var draft = Draft(trains[0], trains);

        var calendar = VariantCalendar.Build(draft, TrainVariants.Others(draft, new TrainContext(trains, 0)));

        Assert.HasCount(2, calendar.Rows);
        Assert.IsNull(calendar.Rows[0].Train, "upravovany vlak je prvou variantou");
        Assert.AreSame(trains[1], calendar.Rows[1].Train);
        Assert.AreEqual(From, calendar.From);
        Assert.AreEqual(To, calendar.To);
        Assert.AreEqual(52, calendar.OverlapDays, "soboty obdobia");
        Assert.IsFalse(calendar.IsOverlap(0), "13.12.2026 je nedela");
        Assert.IsTrue(calendar.IsOverlap(6), "19.12.2026 je sobota");
    }

    [TestMethod]
    public void Kalendar_InaPlatnostANecitatelneObmedzenie()
    {
        var other = T("100", 2, "ide v 1-5");
        other.KoniecPlatnosti = To.AddDays(10);
        var broken = T("100", 3, "ide 20.XII.-2.I.");
        List<Train> trains = [T("100", 1), other, broken];
        var draft = Draft(trains[0], trains);

        var calendar = VariantCalendar.Build(draft, TrainVariants.Others(draft, new TrainContext(trains, 0)));

        Assert.AreEqual(To.AddDays(10), calendar.To);
        Assert.IsTrue(calendar.Rows[2].Invalid);
        Assert.IsFalse(calendar.Rows[2].Runs.Any(run => run));
        Assert.IsFalse(calendar.Rows[0].Runs[^1], "upravovany vlak po konci platnosti nejde");
    }

    [TestMethod]
    public void Skupina_ZmenaCisla_PrenesieSaNaOstatneVarianty()
    {
        List<Train> trains = [T("100", 1, "ide v 1-5"), T("100", 2, "ide v 6,7"), T("200", -1)];
        var draft = Draft(trains[0], trains);
        draft.Number = "101";

        Assert.IsTrue(draft.RenamesSiblings);
        CollectionAssert.AreEqual(new[] { trains[1] }, TrainVariants.Others(draft, new TrainContext(trains, 0)),
            "povodne varianty ostavaju variantmi");

        draft.ApplyToSiblings();
        CollectionAssert.AreEqual(new[] { "100", "101", "200" }, trains.Select(t => t.Number).ToArray(),
            "upravovany vlak zapise ApplyTo, ostatne varianty ApplyToSiblings");
    }

    [TestMethod]
    public void Skupina_BezPrenosu_VlakZoSkupinyOdide()
    {
        List<Train> trains = [T("100", 1), T("100", 2), T("200", -1)];
        var draft = Draft(trains[0], trains);
        draft.Number = "101";
        draft.RenameSiblings = false;

        Assert.IsFalse(draft.RenamesSiblings);
        Assert.IsEmpty(TrainVariants.Others(draft, new TrainContext(trains, 0)));

        draft.ApplyToSiblings();
        Assert.AreEqual("100", trains[1].Number);
    }

    [TestMethod]
    public void Prehlad_PoradieVSkupineAPrekrytie()
    {
        List<Train> trains = [T("100", 2, "ide v 6,7"), T("100", 1, "ide v 1-6"), T("200", -1), T("100", 3, "ide 24.XII.")];

        var index = VariantIndex.Build(trains);

        Assert.AreEqual(2, index.Of(trains[0]).Position);
        Assert.AreEqual(1, index.Of(trains[1]).Position);
        Assert.AreEqual(3, index.Of(trains[3]).Count);
        Assert.AreEqual(1, index.Of(trains[2]).Count, "vlak bez variant");
        Assert.AreEqual("ide v 6", index.Of(trains[0]).Overlaps.Single(o => o.Train == trains[1]).Days);
        Assert.IsTrue(index.AreSiblings(trains[0], trains[3]));
        Assert.IsFalse(index.AreSiblings(trains[0], trains[0]));
        Assert.IsFalse(index.AreSiblings(trains[0], trains[2]));
    }
}
