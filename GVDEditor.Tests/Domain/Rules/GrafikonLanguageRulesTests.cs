using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Jazyky grafikonu (Lokalne nastavenia → Jazyky hlaseni) a ich citanie z lokalneho Categori.txt.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GrafikonLanguageRulesTests
{
    private FyzLanguage _sk = null!;
    private FyzLanguage _gb = null!;
    private FyzLanguage _de = null!;
    private List<FyzLanguage> _global = null!;

    [TestInitialize]
    public void Init()
    {
        _sk = new FyzLanguage("SK", "Slovensky") { IsBasic = true };
        _gb = new FyzLanguage("GB", "Anglicky");
        _de = new FyzLanguage("D", "Nemecky");
        _global = [_sk, _gb, _de];
    }

    private static Radenie Radenie(string number, FyzLanguage language) => new()
    {
        CisloVlaku = number,
        DatObm = "",
        Text = "",
        Sounds = [new FyzSound(new FyzGroup(language, "R1", "Radenie", "R1\\"), "R001", "R001", "R001.WAV", "", "", 0)]
    };

    [TestMethod]
    public void Sync_ZmazanyJazykVypadne_NovySaNepridaAPoradieJeGlobalne()
    {
        // GB sa v globalnych nastaveniach zmazal, D pribudol
        var synced = GrafikonLanguageRules.Sync([_gb, _sk], [_sk, _de]);

        CollectionAssert.AreEqual(new[] { _sk }, synced);
    }

    [TestMethod]
    public void Sync_PonechaVyberGrafikonuVGlobalnomPoradi()
    {
        CollectionAssert.AreEqual(new[] { _sk, _de }, GrafikonLanguageRules.Sync([_de, _sk], _global));
    }

    [TestMethod]
    public void Check_BezJazyka_JeChyba()
    {
        Assert.IsNotNull(GrafikonLanguageRules.Check([], _global, []));
    }

    [TestMethod]
    public void Check_VypnutyHlavnyJazyk_JeChyba()
    {
        Assert.IsNotNull(GrafikonLanguageRules.Check([_gb], _global, []));
    }

    [TestMethod]
    public void Check_RadenieSNahravkouVypnutehoJazyka_JeChyba()
    {
        var problem = GrafikonLanguageRules.Check([_sk], _global, [Radenie("1234", _gb)]);

        Assert.IsNotNull(problem);
        StringAssert.Contains(problem, "1234");
    }

    [TestMethod]
    public void Check_PlatnyVyber_NemaChybu()
    {
        Assert.IsNull(GrafikonLanguageRules.Check([_sk, _gb], _global, [Radenie("1234", _gb)]));
    }

    [TestMethod]
    public void Warnings_VlakSVypnutymJazykom_JeUpozornenie()
    {
        var train = new Train { Languages = [_de] };

        Assert.HasCount(1, GrafikonLanguageRules.Warnings([_sk, _gb], _global, [train]));
        Assert.IsEmpty(GrafikonLanguageRules.Warnings([_sk, _de], _global, [train]));
    }

    [TestMethod]
    public void Offered_JazykyGrafikonuAJuPouziteVGlobalnomPoradi()
    {
        CollectionAssert.AreEqual(new[] { _sk, _de }, GrafikonLanguageRules.Offered(_global, [_sk], [_de]));
    }

    [TestMethod]
    public void ReadLocalCategori_NemeniNazovAniHlavnyJazykGlobalnychJazykov()
    {
        var dir = Path.Combine(Path.GetTempPath(), "GrafikonLanguageRulesTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            // lokalny subor ma iny nazov jazyka aj priznak hlavneho jazyka - INISS ich berie z globalneho suboru
            File.WriteAllText(Path.Combine(dir, GvdFileConsts.FILE_CATEGORI),
                "[MAIN]\r\nCOUNT_BASIC_REPORT_VARIANT=0\r\nCOUNT_TYPE_BASIC_REPORT=0\r\nCOUNT_LANGUAGES=2\r\n\r\n" +
                "[LANGUAGE_01]\r\nKEY=\"SK\"\r\nIS_BASIC=0\r\nNAME=\"SK\"\r\n\r\n" +
                "[LANGUAGE_02]\r\nKEY=\"GB\"\r\nIS_BASIC=1\r\nNAME=\"EN\"\r\n");

            var (_, _, languages) = CategoriFile.ReadLocal(dir, _global);

            CollectionAssert.AreEqual(new[] { _sk, _gb }, languages);
            Assert.AreEqual("Slovensky", _sk.Name);
            Assert.IsTrue(_sk.IsBasic);
            Assert.AreEqual("Anglicky", _gb.Name);
            Assert.IsFalse(_gb.IsBasic);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }
}
