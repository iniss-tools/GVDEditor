using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;

namespace GVDEditor.Tests.Config;

/// <summary>Nastavenie simulatora tabul - API kluc ulozeny sifrovane (DPAPI).</summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableSimulatorSettingsTests
{
    [TestMethod]
    public void ApiKluc_VConfiguSifrovanyATamASpat()
    {
        var settings = new TableSimulatorSettings { ApiKey = " ts_tajny-kluc " };

        var serializer = new XmlSerializer(typeof(TableSimulatorSettings));
        using var writer = new StringWriter();
        serializer.Serialize(writer, settings);
        using var reader = XmlReader.Create(new StringReader(writer.ToString()), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        var loaded = (TableSimulatorSettings)serializer.Deserialize(reader)!;

        Assert.DoesNotContain("ts_tajny-kluc", writer.ToString());
        Assert.AreEqual("ts_tajny-kluc", loaded.ApiKey);
        Assert.AreEqual("ts_tajny-kluc", settings.Clone().ApiKey);
    }

    [TestMethod]
    public void ApiKluc_PrazdnyANecitatelny()
    {
        Assert.AreEqual("", new TableSimulatorSettings { ApiKey = "  " }.ProtectedApiKey);
        // kluc ulozeny inym pouzivatelom Windows sa neda desifrovat - ostane prazdny
        Assert.AreEqual("", new TableSimulatorSettings { ProtectedApiKey = Convert.ToBase64String(new byte[40]) }.ApiKey);
        Assert.AreEqual("", new TableSimulatorSettings { ProtectedApiKey = "nie-je-base64" }.ApiKey);
    }
}
