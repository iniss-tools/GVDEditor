using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Zoznam pisiem tabul (Lokalne nastavenia → Pisma, ModeTabs.txt [FONT]).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class FontRulesTests
{
    private static TableFont F(string name, int id) => new() { Name = name, FontID = id, FileName = "" };

    [TestMethod]
    public void Pismo_NazovJePovinny()
    {
        Assert.IsNull(FontRules.CheckName("Tenké"));
        Assert.IsNotNull(FontRules.CheckName(" "));
    }

    [TestMethod]
    public void Pismo_RovnakeCislo_JeChybaPriObochPismach()
    {
        TableFont[] fonts = [F("Tenké", 80), F("Tučné", 96), F("Tenké (kópia)", 80)];

        Assert.IsNotNull(FontRules.CheckId(fonts, 0));
        Assert.IsNull(FontRules.CheckId(fonts, 1));
        Assert.IsNotNull(FontRules.CheckId(fonts, 2));
    }

    [TestMethod]
    [DataRow(new int[0], 80)] // tenke
    [DataRow(new[] { 80 }, 81)] // tenke cervene
    [DataRow(new[] { 80, 81, 82, 83 }, 96)] // tucne
    public void Pismo_NoveDostanePrvyVolnyVzhlad(int[] used, int expected)
    {
        Assert.AreEqual(expected, FontRules.SuggestId(used));
    }
}
