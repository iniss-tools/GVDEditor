using System.Globalization;
using GVDEditor.Integration;
using GVDEditor.Properties;
using GVDEditor.UI.Export;

namespace GVDEditor.Tests.UI.Export;

/// <summary>
/// Texty upozorneni exportu do INISS2 v sprievodcovi.
/// </summary>
[TestClass]
public class Iniss2ExportTextsTests
{
    [TestMethod]
    [DataRow("sk")]
    [DataRow("cs")]
    public void Upozornenia_KazdeMaTextVJazyku(string culture)
    {
        var previous = Resources.Culture;
        try
        {
            Resources.Culture = CultureInfo.GetCultureInfo(culture);
            var texts = Enum.GetValues<Iniss2ExportIssue>().Select(Iniss2ExportTexts.Issue).ToList();

            Assert.IsTrue(texts.All(text => !string.IsNullOrWhiteSpace(text)));
            Assert.HasCount(texts.Count, texts.Distinct().ToList(), "kazde upozornenie ma vlastny text");
        }
        finally
        {
            Resources.Culture = previous;
        }
    }

    [TestMethod]
    public void Chyba_SpojiCastAPopis()
    {
        var text = Iniss2ExportTexts.Error(new Iniss2ExportException(Iniss2ExportIssue.StationTwice, "Lipovec"));

        Assert.StartsWith("Lipovec", text);
        Assert.EndsWith(Iniss2ExportTexts.Issue(Iniss2ExportIssue.StationTwice), text);
    }
}
