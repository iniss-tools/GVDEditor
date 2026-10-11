using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Tests.Domain.Editing;

/// <summary>
/// Kratke hlasenie importovanych vlakov podla vlakov grafikonu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ShortReportLearningTests
{
    private static readonly Dictionary<string, bool> NoChoices = [];

    /// <summary>Stanica trasy: <c>K</c> = v kratkom hlaseni, <c>-</c> = nehlasi sa, inak len v dlhom.</summary>
    private static Station St(string id, char kind = ' ') => new(id, "S" + id, kind == 'K', kind != '-');

    private static Train Vlak(Routing routing, IEnumerable<Station> zo, IEnumerable<Station> doSmeru)
    {
        var train = new Train { Number = "1", Routing = routing };
        train.StaniceZoSmeru.AddRange(zo);
        train.StaniceDoSmeru.AddRange(doSmeru);
        return train;
    }

    private static string[] Kratke(Train train) =>
        train.StaniceZoSmeru.Concat(train.StaniceDoSmeru).Where(s => s.IsInShortReport).Select(s => s.ID).ToArray();

    [TestMethod]
    public void Koncove_SuVzdyVKratkomHlaseniAjKedSaNehlasiaHranicneBody()
    {
        // hranicny bod na zaciatku (vlak z ciziny bez priradenych stanic) koncovou stanicou nie je
        var train = Vlak(Routing.Prechadzajuci, [St("1", '-'), St("2"), St("3")], [St("4"), St("5"), St("6", '-')]);

        ShortReportLearning.Apply(train, NoChoices, null);

        CollectionAssert.AreEqual(new[] { "2", "5" }, Kratke(train));
        Assert.AreEqual("2", ShortReportLearning.Origin(train)!.ID);
        Assert.AreEqual("5", ShortReportLearning.Destination(train)!.ID);
        CollectionAssert.AreEqual(new[] { "3", "4" }, ShortReportLearning.InnerStations(train).Select(s => s.ID).ToArray());
    }

    [TestMethod]
    public void Koncove_StranaLenSNehlasenymiBodmiNemaVKratkomHlaseniNic()
    {
        // vlak z ciziny, z ktorej trasy ostal len hranicny bod
        var train = Vlak(Routing.Prechadzajuci, [St("1", '-')], [St("2"), St("3", '-')]);

        ShortReportLearning.Apply(train, NoChoices, null);

        Assert.IsNull(ShortReportLearning.Origin(train));
        Assert.AreEqual("2", ShortReportLearning.Destination(train)!.ID);
        CollectionAssert.AreEqual(new[] { "2" }, Kratke(train));
    }

    [TestMethod]
    public void Volba_PodlaStaniceNehlasenaStanicaNikdy()
    {
        var train = Vlak(Routing.Vychadzajuci, [], [St("1"), St("2", '-'), St("3"), St("4")]);
        var choices = new Dictionary<string, bool> { ["1"] = true, ["2"] = true, ["3"] = false };

        ShortReportLearning.Apply(train, choices, null);

        CollectionAssert.AreEqual(new[] { "1", "4" }, Kratke(train));
    }

    [TestMethod]
    public void RovnakyVlak_PrevezmeSvojeStaniceOstatnePodlaVolby()
    {
        var povodny = Vlak(Routing.Vychadzajuci, [], [St("1", 'K'), St("2"), St("9")]);
        var novy = Vlak(Routing.Vychadzajuci, [], [St("1"), St("2"), St("3"), St("9"), St("10")]);
        // 9 bola v povodnom vlaku cielova (vzdy v kratkom) - to nie je volba, rozhoduje volba pre stanicu
        var choices = new Dictionary<string, bool> { ["1"] = false, ["2"] = true, ["3"] = true, ["9"] = false };

        ShortReportLearning.Apply(novy, choices, povodny);

        CollectionAssert.AreEqual(new[] { "1", "3", "10" }, Kratke(novy));
    }

    [TestMethod]
    public void Hlasovanie_PocitaLenStaniceMedziKoncovymi()
    {
        var a = Vlak(Routing.Prechadzajuci, [St("1", 'K'), St("2", 'K')], [St("3"), St("4", 'K')]);
        var b = Vlak(Routing.Vychadzajuci, [], [St("2"), St("3", 'K'), St("4", 'K')]);
        var c = Vlak(Routing.Vychadzajuci, [], [St("2", 'K'), St("5", '-'), St("6", 'K')]);

        var votes = ShortReportLearning.Votes([a, b, c]);

        Assert.AreEqual(new ShortReportVote(3, 2), votes["2"]);
        Assert.AreEqual(new ShortReportVote(2, 1), votes["3"]);
        Assert.IsFalse(votes.ContainsKey("1"), "vychodiskova stanica");
        Assert.IsFalse(votes.ContainsKey("5"), "nehlasi sa");
        Assert.IsFalse(votes.ContainsKey("4"), "cielova stanica");
        Assert.IsTrue(votes["2"].IsShort);
        Assert.IsTrue(votes["3"].IsShort, "polovica staci");
        Assert.IsFalse(new ShortReportVote(3, 1).IsShort);
        Assert.IsFalse(default(ShortReportVote).IsShort);
    }
}
