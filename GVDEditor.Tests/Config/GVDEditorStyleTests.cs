using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;

namespace GVDEditor.Tests.Config;

/// <summary>Farby editora TabTab a stlpca typ vlaku - nazvy v jazyku programu, nie v jazyku pri nacitani styles.xml.</summary>
[TestClass]
public class GVDEditorStyleTests
{
    // AppInit cita styles.xml skor, nez nastavi jazyk programu - nazvy sa nesmu zapamatat pri nacitani
    [TestMethod]
    [DataRow("sk", "cs", "Řetězec", "Funkce", "Výchozí", "Tlačítko")]
    [DataRow("cs", "sk", "Reťazec", "Funkcia", "Predvolené", "Tlačidlo")]
    public void NazvyFarieb_VJazykuProgramuPoNacitaniStylu(string loadCulture, string appCulture, string text, string function, string fallback, string button)
    {
        var old = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo(loadCulture);
            var light = Deserialize(Serialize(GVDEditorStyle.DefaultLightStyle));
            var dark = Deserialize(Serialize(GVDEditorStyle.DefaultDarkStyle));

            CultureInfo.CurrentUICulture = new CultureInfo(appCulture);
            foreach (var style in new[] { light, dark, dark with { TabTabEditorScheme = dark.TabTabEditorScheme with { } }, GVDEditorStyle.DefaultDarkStyle })
            {
                Assert.AreEqual(text, style.TabTabEditorScheme.String.Name);
                Assert.AreEqual(function, style.TabTabEditorScheme.Function.Name);
                Assert.AreEqual(fallback, style.TabTabEditorScheme.Default.ToString());
                Assert.AreEqual("Os", style.TrainTypeColumnScheme.Os.Name);
                Assert.AreEqual(button, style.ControlsColorScheme.Button.Name);
            }
        }
        finally
        {
            CultureInfo.CurrentUICulture = old;
        }
    }

    private static string Serialize(GVDEditorStyle style)
    {
        using var writer = new StringWriter();
        new XmlSerializer(typeof(GVDEditorStyle)).Serialize(writer, style);
        return writer.ToString();
    }

    private static GVDEditorStyle Deserialize(string xml)
    {
        using var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        return (GVDEditorStyle)new XmlSerializer(typeof(GVDEditorStyle)).Deserialize(reader)!;
    }
}
