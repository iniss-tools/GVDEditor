using GVDEditor.Domain.Analysis;

namespace GVDEditor.Tests.Domain.Analysis;

/// <summary>
/// Zberac varovani pri nacitani - suhrn pre pouzivatela.
/// </summary>
[TestClass]
public class LoadWarningsTests
{
    [TestMethod]
    public void Suhrn_BezVarovani_Null()
    {
        Assert.IsNull(new LoadWarnings().TakeSummary());
    }

    [TestMethod]
    public void Suhrn_ObmedziPocetRiadkovAVyprazdniZoznam()
    {
        var warnings = new LoadWarnings();
        for (var i = 1; i <= 5; i++)
            warnings.Add("Varovanie " + i);

        var summary = warnings.TakeSummary(maxLines: 3);

        Assert.IsNotNull(summary);
        StringAssert.Contains(summary, "Varovanie 3");
        Assert.DoesNotContain("Varovanie 4", summary);
        Assert.IsEmpty(warnings.Items);
        Assert.IsNull(warnings.TakeSummary());
    }
}
