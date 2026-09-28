using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
///     Typy vlakov (Globalne nastavenia → Typy vlakov, TrTypes.txt).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainTypeRulesTests
{
    private static TrainType B(string category, string? key = null) => new(category) { Key = key ?? category, TextInTable = key ?? category };

    private static TrainType C(string category, string key) => new(category, key, key);

    [TestMethod]
    [DataRow("R1", true)]
    [DataRow("Os9", true)]
    [DataRow("Sl3", true)]
    [DataRow("X5", true)]
    [DataRow("R", false)]
    [DataRow("R10", false)]
    [DataRow("Os0", false)]
    [DataRow("SC", false)]
    public void TypVlaku_VlastnyPodlaKategorie(string category, bool custom)
    {
        Assert.AreEqual(custom, TrainType.IsCustomCategory(category));
        Assert.AreEqual(custom, new TrainType(category, "K", "K").IsCustom);
    }

    [TestMethod]
    public void TypVlaku_ZmenaKategorieZmeniVlastny()
    {
        // uprava na mieste - zabudovany druh sa zmeni na vlastny typ bez noveho objektu
        var type = B("Os", "RJ");
        type.CategoryTrain = "R1";

        Assert.IsTrue(type.IsCustom);
    }

    [TestMethod]
    public void TypVlaku_Skratka()
    {
        TrainType[] types = [B("Os"), C("R1", "RJ"), C("R2", "RJ"), C("R3", ""), C("X1", "A,B")];

        Assert.IsNull(TrainTypeRules.CheckKey(types, 0));
        Assert.IsNotNull(TrainTypeRules.CheckKey(types, 1));
        Assert.IsNotNull(TrainTypeRules.CheckKey(types, 2));
        Assert.IsNotNull(TrainTypeRules.CheckKey(types, 3));
        Assert.IsNotNull(TrainTypeRules.CheckKey(types, 4));
    }

    [TestMethod]
    public void TypVlaku_ZabudovanyDruhDvakrat_JeChybaPriDruhom()
    {
        // INISS uklada typ na miesto kategorie - druhy riadok prepise prvy
        TrainType[] types = [B("R"), B("R", "Rx")];

        Assert.IsNull(TrainTypeRules.CheckCategory(types, 0));
        Assert.IsNotNull(TrainTypeRules.CheckCategory(types, 1));
    }

    [TestMethod]
    public void TypVlaku_ViacAkoDevatVlastnych_JeChyba()
    {
        Assert.IsNotNull(TrainTypeRules.CheckCategory([C("R10", "Q")], 0));
        Assert.IsNull(TrainTypeRules.CheckCategory([C("R9", "Q")], 0));
    }

    [TestMethod]
    public void TypVlaku_PrecislovanieVSkupinachPodlaPoradia()
    {
        TrainType[] types = [C("R2", "RJ"), B("Os"), C("X4", "AEx"), C("R99", "LE"), C("R5", "RR")];

        TrainTypeRules.Renumber(types);

        CollectionAssert.AreEqual(new[] { "R1", "Os", "X1", "R2", "R3" }, types.Select(t => t.CategoryTrain).ToArray());
    }

    [TestMethod]
    public void TypVlaku_PocetVSkupine()
    {
        TrainType[] types = [C("R1", "RJ"), C("R2", "RR"), B("R"), C("Os1", "A")];

        Assert.AreEqual(2, TrainTypeRules.CountInGroup(types, "R"));
        Assert.AreEqual(1, TrainTypeRules.CountInGroup(types, "R", types[0]));
        Assert.AreEqual(0, TrainTypeRules.CountInGroup(types, "X"));
    }
}
