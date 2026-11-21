using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;

namespace GVDEditor.Tests.Config;

/// <summary>Nastavenie StationSimulatora - API kluc ulozeny sifrovane (DPAPI).</summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class StationSimulatorSettingsTests
{
    [TestMethod]
    public void ApiKluc_VConfiguSifrovanyATamASpat()
    {
        var settings = new StationSimulatorSettings { ApiKey = " ts_tajny-kluc " };

        var serializer = new XmlSerializer(typeof(StationSimulatorSettings));
        using var writer = new StringWriter();
        serializer.Serialize(writer, settings);
        using var reader = XmlReader.Create(new StringReader(writer.ToString()), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
        var loaded = (StationSimulatorSettings)serializer.Deserialize(reader)!;

        Assert.DoesNotContain("ts_tajny-kluc", writer.ToString());
        Assert.AreEqual("ts_tajny-kluc", loaded.ApiKey);
        Assert.AreEqual("ts_tajny-kluc", settings.Clone().ApiKey);
    }

    [TestMethod]
    public void ApiKluc_PrazdnyANecitatelny()
    {
        Assert.AreEqual("", new StationSimulatorSettings { ApiKey = "  " }.ProtectedApiKey);
        // kluc ulozeny inym pouzivatelom Windows sa neda desifrovat - ostane prazdny
        Assert.AreEqual("", new StationSimulatorSettings { ProtectedApiKey = Convert.ToBase64String(new byte[40]) }.ApiKey);
        Assert.AreEqual("", new StationSimulatorSettings { ProtectedApiKey = "nie-je-base64" }.ApiKey);
    }
}
