using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Rules;
using ToolsCore.Entities;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Jazyky stanice (Globalne nastavenia → Jazyky) podla pravidiel INISSu.
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

    [TestMethod]
    public void Jazyk_ChybaSaOznaciPriRiadkuSKlucom()
    {
        // stranka Jazyky oznaci chybu pri konkretnom riadku, hlavny jazyk kontroluje zvlast
        FyzLanguage[] languages = [L("SK", true), L("PL"), L("GB"), L("GB")];

        Assert.IsNull(LanguageRules.CheckLanguage(languages, 0, Bank));
        Assert.IsNotNull(LanguageRules.CheckLanguage(languages, 1, Bank));
        Assert.IsNotNull(LanguageRules.CheckLanguage(languages, 2, Bank));
        Assert.IsNotNull(LanguageRules.CheckLanguage(languages, 3, Bank));
    }

    [TestMethod]
    public void Jazyky_HlavnyPraveJeden()
    {
        Assert.IsNull(LanguageRules.CheckBasic([L("SK", true), L("GB")]));
        Assert.IsNotNull(LanguageRules.CheckBasic([L("SK"), L("GB")]));
        Assert.IsNotNull(LanguageRules.CheckBasic([]));
    }
}
