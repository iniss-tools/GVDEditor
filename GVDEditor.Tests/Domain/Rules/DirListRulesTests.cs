using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Zoznam grafikonov (Globalne nastavenia → Grafikony, DirList.TXT): porty a poradie grafikonov.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class DirListRulesTests
{
    [TestMethod]
    [DataRow(1, -1, 0, "B|A|C")]
    [DataRow(1, 1, 2, "A|C|B")]
    [DataRow(0, 2, 2, "B|C|A")]
    public void Move_PosunieGrafikon(int index, int delta, int expectedIndex, string expected)
    {
        var items = new List<string> { "A", "B", "C" };

        Assert.AreEqual(expectedIndex, DirListRules.Move(items, index, delta));
        Assert.AreEqual(expected, string.Join("|", items));
    }

    [TestMethod]
    [DataRow(0, -1)]
    [DataRow(2, 1)]
    [DataRow(1, 0)]
    [DataRow(5, -1)]
    public void Move_MimoZoznamuNicNezmeni(int index, int delta)
    {
        var items = new List<string> { "A", "B", "C" };

        Assert.IsNull(DirListRules.Move(items, index, delta));
        Assert.AreEqual("A|B|C", string.Join("|", items));
    }

    [TestMethod]
    [DataRow("", null)]
    [DataRow("0", null)]
    [DataRow(" 5 ", 5)]
    [DataRow("255", 255)]
    public void ParsePort_PlatnaHodnota(string text, int? expected)
    {
        Assert.IsNull(DirListRules.ParsePort(text, out var port));
        Assert.AreEqual(expected, port);
    }

    [TestMethod]
    [DataRow("256")]
    [DataRow("-1")]
    [DataRow("COM2")]
    public void ParsePort_NeplatnaHodnotaJeChyba(string text)
    {
        Assert.IsNotNull(DirListRules.ParsePort(text, out _));
    }
}
