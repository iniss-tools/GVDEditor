using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Casy meskania (Globalne nastavenia → Meskania, Zpozdeni.TXT).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class DelayRulesTests
{
    [TestMethod]
    [DataRow("15", true)]
    [DataRow(" 15 ", true)]
    [DataRow("+5", true)]
    [DataRow("-5", true)]
    [DataRow("0", true)]
    [DataRow("VICE480", false)]
    [DataRow("15 min", false)]
    [DataRow("1.5", false)]
    public void Meskanie_PrevodAkoINISS(string value, bool accepted)
    {
        Assert.AreEqual(accepted, DelayRules.IsAcceptedByIniss(value));
    }

    [TestMethod]
    [DataRow("25", 2)]
    [DataRow("5", 0)]
    [DataRow("480", 4)]
    [DataRow("VICE", 5)]
    public void Meskanie_ZaradiSaPodlaVelkosti(string value, int expected)
    {
        string[] delays = ["10", "20", "30", "60", "VICE480"];

        Assert.AreEqual(expected, DelayRules.InsertIndex(delays, value));
    }

    [TestMethod]
    public void Meskanie_NecislenaHodnota_NieJeChyba()
    {
        // INISS ju len preskoci - pouzivatel ju moze ponechat (upozornenie, nie chyba)
        Assert.IsNull(DelayRules.CheckValue(["10", "VICE480"], 1));
    }

    [TestMethod]
    public void Meskanie_Prazdne_JeChyba()
    {
        Assert.IsNotNull(DelayRules.CheckValue(["10", " "], 1));
    }

    [TestMethod]
    public void Meskanie_RovnakyCas_JeChybaPriObochRiadkoch()
    {
        string[] delays = ["10", "20", " 20"];

        Assert.IsNull(DelayRules.CheckValue(delays, 0));
        Assert.IsNotNull(DelayRules.CheckValue(delays, 1));
        Assert.IsNotNull(DelayRules.CheckValue(delays, 2));
    }
}
