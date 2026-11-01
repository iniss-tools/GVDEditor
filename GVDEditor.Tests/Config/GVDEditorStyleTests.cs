using System.Globalization;
using System.Xml.Linq;
using GVDEditor.Config;
using ToolsCore.XML;

namespace GVDEditor.Tests.Config;

/// <summary>Farby editora TabTab a stlpca typ vlaku - nazvy v jazyku programu a prevod stareho suboru stylov.</summary>
[TestClass]
public class GVDEditorStyleTests
{
    [TestMethod]
    [DataRow("sk", "Reťazec", "Funkcia", "Predvolené")]
    [DataRow("cs", "Řetězec", "Funkce", "Výchozí")]
    public void NazvyFarieb_VJazykuProgramu(string culture, string text, string function, string fallback)
    {
        var old = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            Assert.AreEqual(text, Key("TabTab.String").Name);
            Assert.AreEqual(function, Key("TabTab.Function").Name);
            Assert.AreEqual(fallback, Key("TabTab.Default").Name);
            Assert.AreEqual("Os", Key("TrainType.Os").Name);
        }
        finally
        {
            CultureInfo.CurrentUICulture = old;
        }
    }

    [TestMethod]
    public void Zaklad_NastaviFarbyEditoraATypovVlakov()
    {
        var styles = Styles<GVDEditorStyle>.CreateDefault();
        var light = styles[StyleNames.Light];
        var dark = styles[StyleNames.Dark];

        Assert.AreEqual(Color.Purple, light.TabTabEditorScheme.Number.ForeColor);
        Assert.AreEqual(Color.PaleGoldenrod, dark.TabTabEditorScheme.Number.ForeColor);
        Assert.IsTrue(dark.TrainTypeColumnScheme.R.Bold);
        Assert.AreEqual(Color.White, dark.TrainTypeColumnScheme.Os.ForeColor);
    }

    [TestMethod]
    public void StarySubor_PismoEditoraAZmenyFarieb()
    {
        var styles = Styles<GVDEditorStyle>.FromLegacy(XElement.Parse("""
            <STYLES>
              <GVDEditorStyle name="Dark" used="true">
                <DarkTitlebar>true</DarkTitlebar>
                <TrainType>
                  <Font family="Segoe UI" gunit="pt" size="9" style="r" />
                  <Os bold="false" b="Transparent" f="White" />
                  <R bold="true" b="Transparent" f="Red" />
                </TrainType>
                <TabTabEditor>
                  <Font family="Courier New" gunit="pt" size="12" style="r" />
                  <Number bold="false" f="PaleGoldenrod" />
                  <Comment bold="true" f="#1D9E75" />
                </TabTabEditor>
              </GVDEditorStyle>
            </STYLES>
            """));

        var dark = styles[StyleNames.Dark];
        Assert.AreEqual(new FontChoice("Courier New", 12), dark.LegacyEditorFont);
        CollectionAssert.AreEquivalent(new[] { "TabTab.Comment" }, dark.Overrides.Keys.ToArray());
        Assert.IsTrue(dark.TabTabEditorScheme.Comment.Bold);
        Assert.AreEqual(Color.FromArgb(0x1D, 0x9E, 0x75).ToArgb(), dark.TabTabEditorScheme.Comment.ForeColor.ToArgb());
    }

    private static ColorKey Key(string id) => new GVDEditorStyle().ColorKeys.Single(k => k.Id == id);
}
