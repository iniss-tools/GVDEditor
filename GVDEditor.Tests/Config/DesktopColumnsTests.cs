using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;
using ToolsCore.XML;

namespace GVDEditor.Tests.Config;

/// <summary>Stlpce pracovnej plochy - nazvy v jazyku programu, nie v jazyku pri nacitani config.xml.</summary>
[TestClass]
public class DesktopColumnsTests
{
    // AppInit cita config.xml skor, nez nastavi jazyk programu - nazvy sa nesmu zapamatat pri nacitani
    [TestMethod]
    [DataRow("sk", "cs", "Název", "Kolej", "Datumové omezení")]
    [DataRow("cs", "sk", "Názov", "Koľaj", "Dátumové obmedzenie")]
    public void GetValues_NazvyVJazykuProgramuPoNacitaniKonfiguracie(string loadCulture, string appCulture, string name, string track, string dateLimit)
    {
        var old = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo(loadCulture);
            var config = Deserialize(Serialize(new GVDEditorConfig()));

            CultureInfo.CurrentUICulture = new CultureInfo(appCulture);
            foreach (var columns in new[] { config.DesktopCols, config.DesktopCols with { }, new DesktopColumns() })
            {
                var values = columns.GetValues();
                Assert.AreEqual(name, NameOf(values, nameof(DesktopColumns.Name)));
                Assert.AreEqual(track, NameOf(values, nameof(DesktopColumns.Track)));
                Assert.AreEqual(dateLimit, NameOf(values, nameof(DesktopColumns.DateLimit)));
            }
        }
        finally
        {
            CultureInfo.CurrentUICulture = old;
        }
    }

    private static string NameOf(IList<DesktopColumn> columns, string propertyName) => columns.Single(c => c.PropertyName == propertyName).Name;

    private static string Serialize(GVDEditorConfig config)
    {
        using var writer = new StringWriter();
        new XmlSerializer(typeof(GVDEditorConfig)).Serialize(writer, config);
        return writer.ToString();
    }

    private static GVDEditorConfig Deserialize(string xml)
    {
        using var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        return (GVDEditorConfig)new XmlSerializer(typeof(GVDEditorConfig)).Deserialize(reader)!;
    }
}
