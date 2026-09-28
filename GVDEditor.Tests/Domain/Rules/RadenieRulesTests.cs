using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Calendar;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using ToolsCore.Entities;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
///     Kontrola radenia vlaku pred pridanim alebo upravou (zalozka Radenie v okne vlaku).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class RadenieRulesTests
{
    private static readonly DateTime From = new(2026, 12, 13);
    private static readonly DateTime To = new(2027, 12, 11);
    private static readonly Station Kosice = new("5", "Košice");

    private static Radenie Radenie(string datObm, Station? dest = null, DateTime? to = null) => new()
    {
        ZacPlatnosti = From,
        KonPlatnosti = to ?? To,
        DatObm = datObm,
        Text = "A",
        Sounds = [new FyzSound { Name = "R001" }],
        DestStation = dest!
    };

    private static List<RadenieRules.Field> Fields(Radenie radenie, IReadOnlyList<Radenie> radenia, int index = -1) =>
        RadenieRules.Check(radenie, radenia, index).Select(p => p.Field).ToList();

    [TestMethod]
    public void Radenie_Platne_NemaChyby()
    {
        Assert.IsEmpty(Fields(Radenie("ide v 1-5"), [Radenie("ide v 6,7")]));
    }

    [TestMethod]
    public void Radenie_KoniecPlatnostiNiePoZaciatku_JeChyba()
    {
        CollectionAssert.AreEqual(new[] { RadenieRules.Field.Validity }, Fields(Radenie("", to: From), []));
    }

    [TestMethod]
    public void Radenie_NecitatelneObmedzenie_JeChyba()
    {
        CollectionAssert.AreEqual(new[] { RadenieRules.Field.DateLimit }, Fields(Radenie("ide 20.XII.-2.I."), []));
    }

    [TestMethod]
    public void Radenie_BezNahravok_JeChyba()
    {
        var radenie = Radenie("");
        radenie.Sounds = [];

        CollectionAssert.AreEqual(new[] { RadenieRules.Field.Sounds }, Fields(radenie, []));
    }

    [TestMethod]
    public void Radenie_PrekrytieSInymRovnakehoObdobiaACiela_JeChybaSoSpolocnymiDnami()
    {
        List<Radenie> radenia = [Radenie("ide v 6,7")];

        var problem = RadenieRules.Check(Radenie("ide v 1-6"), radenia, -1).Single();

        Assert.AreEqual(RadenieRules.Field.DateLimit, problem.Field);
        StringAssert.Contains(problem.Message, "ide v 6");
    }

    [TestMethod]
    public void Radenie_PrekrytieSInymCielomAleboSebou_NieJeChyba()
    {
        List<Radenie> radenia = [Radenie("ide v 6,7"), Radenie("ide v 1-6", Kosice)];

        Assert.IsEmpty(Fields(Radenie("ide v 1-6", Kosice), radenia, 1), "upravovane radenie sa neporovnava so sebou");
        Assert.IsEmpty(Fields(Radenie("ide v 1-6"), [Radenie("ide v 6,7", to: To.AddDays(-1))]), "ine obdobie");
    }

    [TestMethod]
    public void Radenie_PrekrytieVZozname_HlasiSaLenPriNeskorsom()
    {
        List<Radenie> radenia = [Radenie("ide v 1-5"), Radenie("")];

        Assert.IsEmpty(Fields(radenia[0], radenia, 0));
        CollectionAssert.AreEqual(new[] { RadenieRules.Field.DateLimit }, Fields(radenia[1], radenia, 1));
    }

    [TestMethod]
    public void Radenie_BezObdobiaPlatnosti_KontrolujuSaLenNahravky()
    {
        var radenie = new Radenie { DatObm = "", Text = "A" };
        List<Radenie> radenia = [new Radenie { DatObm = "", Text = "B", Sounds = [new FyzSound()] }];

        CollectionAssert.AreEqual(new[] { RadenieRules.Field.Sounds }, Fields(radenie, radenia));
    }
}
