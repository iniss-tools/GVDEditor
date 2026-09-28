using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Tests.Domain.Editing;

/// <summary>
/// Tabulka Kedy hlasit dodatku a radenia - zaskrtnutie meni zoznam vybranych hlaseni tak, ako ho zapisuje grafikon.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ReportChoicesTests
{
    private static readonly ReportType Prijizdi = new("P", "Přijíždí", "P");
    private static readonly ReportType Zastavil = new("Z", "Zastavil", "Z");
    private static readonly List<ReportVariant> Variants = ReportVariant.GetDefaultValues();

    [TestMethod]
    public void KedyHlasit_Zaskrtnutie_PridaTypAVariantyVPoradiGrafikonu()
    {
        var chosen = new List<ChosenReportType>();

        ReportChoices.Set(chosen, Prijizdi, Variants[1], true, Variants);
        ReportChoices.Set(chosen, Prijizdi, Variants[0], true, Variants);
        ReportChoices.Set(chosen, Prijizdi, Variants[0], true, Variants);

        Assert.HasCount(1, chosen);
        CollectionAssert.AreEqual(new[] { Variants[0], Variants[1] }, chosen[0].Variants);
        Assert.IsTrue(ReportChoices.IsChosen(chosen, Prijizdi, Variants[0]));
        Assert.IsFalse(ReportChoices.IsChosen(chosen, Zastavil, Variants[0]));
        Assert.AreEqual(2, ReportChoices.Count(chosen));
    }

    [TestMethod]
    public void KedyHlasit_OdskrtnuliePoslednejVarianty_OdstraniTyp()
    {
        var chosen = new List<ChosenReportType> { new() { Type = Zastavil, Variants = [Variants[0]] } };

        ReportChoices.Set(chosen, Zastavil, Variants[0], false, Variants);
        ReportChoices.Set(chosen, Prijizdi, Variants[0], false, Variants);

        Assert.IsEmpty(chosen);
    }

    [TestMethod]
    public void KedyHlasit_ZapisDodatku_ZodpovedaZaskrtnutiu()
    {
        var dodatok = new Dodatok();
        ReportChoices.Set(dodatok.ChosenReports, Zastavil, Variants[1], true, Variants);

        Assert.AreEqual("0001", Dodatok.DodatokToNums(dodatok, [Prijizdi, Zastavil], Variants));
    }
}
