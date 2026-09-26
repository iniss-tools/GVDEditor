using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Casy meskania (Globalne nastavenia → Meskania, Zpozdeni.TXT).
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
}
