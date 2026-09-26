using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;
using ToolsCore.Entities;

namespace GVDEditor.Tests;

/// <summary>
///     Jazyky stanice (Globalne nastavenia → Jazyky) podla pravidiel INISSu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class LanguageRulesTests
{
    private static readonly string[] Bank = ["SK", "CZ", "GB", "D", "PL"];

    private static FyzLanguage L(string key, bool basic = false) => new(key, key) { IsBasic = basic };

    [TestMethod]
    public void Jazyky_HlavnyADvaDalsie_SuVPoriadku()
    {
        Assert.IsNull(LanguageRules.Check([L("SK", true), L("GB"), L("D")], Bank));
    }

    [TestMethod]
    public void Jazyky_Styri_JeChyba()
    {
        Assert.IsNotNull(LanguageRules.Check([L("SK", true), L("CZ"), L("GB"), L("D")], Bank));
    }

    [TestMethod]
    public void Jazyky_KlucKtoryINISSNepozna_JeChyba()
    {
        // PL v zvukovej banke je, INISS ho vsak preskoci
        Assert.IsNotNull(LanguageRules.Check([L("SK", true), L("PL")], Bank));
    }

    [TestMethod]
    public void Jazyky_KlucMimoZvukovejBanky_JeChyba()
    {
        Assert.IsNotNull(LanguageRules.Check([L("SK", true), L("GB")], ["SK"]));
    }

    [TestMethod]
    public void Jazyky_BezHlavneho_JeChyba()
    {
        Assert.IsNotNull(LanguageRules.Check([L("SK"), L("GB")], Bank));
    }

    [TestMethod]
    public void Jazyky_DvaHlavne_JeChyba()
    {
        Assert.IsNotNull(LanguageRules.Check([L("SK", true), L("CZ", true)], Bank));
    }

    [TestMethod]
    public void Jazyky_DvakratTenIstyKluc_JeChyba()
    {
        Assert.IsNotNull(LanguageRules.Check([L("SK", true), L("GB"), L("GB")], Bank));
    }
}
