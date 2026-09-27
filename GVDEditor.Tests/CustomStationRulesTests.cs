using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Vlastne stanice grafikonu (Lokalne nastavenia → Vlastne stanice, Stanice.txt).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class CustomStationRulesTests
{
    private static readonly Station[] Bank = [new("5693001", "Horná Ves"), new("5693002", "Dolná Lehota")];

    [TestMethod]
    public void VlastnaStanica_CisloANazovMimoBanky_SuVPoriadku()
    {
        Assert.IsNull(CustomStationRules.CheckId(["5693100", "5693101"], 0, Bank));
        Assert.IsNull(CustomStationRules.CheckName(["Zastávka pri lese", "Chata"], 0, Bank, "Horná Ves"));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("abc")]
    [DataRow("-5")]
    [DataRow("0")]
    [DataRow("+5")]
    [DataRow("5 1")]
    public void VlastnaStanica_CisloNieJeKladneCeleCislo_JeChyba(string id)
    {
        Assert.IsNotNull(CustomStationRules.CheckId([id], 0, Bank));
    }

    [TestMethod]
    public void VlastnaStanica_CisloStaniceZvukovejBanky_JeChyba()
    {
        Assert.IsNotNull(CustomStationRules.CheckId(["5693001"], 0, Bank));
    }

    [TestMethod]
    public void VlastnaStanica_RovnakeCislo_JeChybaPriObochStaniciach()
    {
        string[] ids = ["7", "7"];

        Assert.IsNotNull(CustomStationRules.CheckId(ids, 0, Bank));
        Assert.IsNotNull(CustomStationRules.CheckId(ids, 1, Bank));
    }

    [TestMethod]
    public void VlastnaStanica_NazovZoZvukovejBanky_JeChyba()
    {
        // GVDEditor by ju pri dalsom otvoreni grafikonu vynechal
        Assert.IsNotNull(CustomStationRules.CheckName(["Dolná Lehota"], 0, Bank, "Horná Ves"));
    }

    [TestMethod]
    public void VlastnaStanica_NazovStaniceGrafikonu_JeChyba()
    {
        Assert.IsNotNull(CustomStationRules.CheckName(["Nová Obec"], 0, Bank, "Nová Obec"));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("Stanica \"A\"")]
    public void VlastnaStanica_PrazdnyAleboSUvodzovkami_JeChyba(string name)
    {
        Assert.IsNotNull(CustomStationRules.CheckName([name], 0, Bank, "Horná Ves"));
    }

    [TestMethod]
    public void VlastnaStanica_NoveCislo_JeZaNajvyssim()
    {
        Assert.AreEqual("5693003", CustomStationRules.SuggestId(Bank.Select(s => s.ID)));
        Assert.AreEqual("5693101", CustomStationRules.SuggestId(["5693100", "x", "5693001"]));
        Assert.AreEqual("1", CustomStationRules.SuggestId([]));
    }
}
