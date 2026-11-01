using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GVDEditor.Config;

namespace GVDEditor.Tests.Config;

/// <summary>Velkosti okien su v konfiguracii spolu v prvku Windows a kopia konfiguracie je nezavisla.</summary>
[TestClass]
public class WindowPlacementsTests
{
    [TestMethod]
    public void Ulozenie_VelkostiOkienVPrvkuWindows()
    {
        var config = new GVDEditorConfig();
        config.Windows.EditTrain = new WindowPlacement { Width = 1272, Height = 808 };
        config.Windows.InissSettings = new WindowPlacement { Width = 1194, Height = 779, Maximized = true };

        using var writer = new StringWriter();
        new XmlSerializer(typeof(GVDEditorConfig)).Serialize(writer, config);
        var root = XElement.Parse(writer.ToString());
        using var reader = XmlReader.Create(new StringReader(writer.ToString()));
        var loaded = (GVDEditorConfig)new XmlSerializer(typeof(GVDEditorConfig)).Deserialize(reader)!;

        var windows = root.Element("Windows")!;
        Assert.AreEqual("1272", (string?)windows.Element("EditTrain")?.Attribute("Width"));
        Assert.IsNull(root.Element("EditTrainWindow"));
        Assert.AreEqual(808, loaded.Windows.EditTrain!.Height);
        Assert.IsTrue(loaded.Windows.InissSettings!.Maximized);
        Assert.IsNull(loaded.Windows.LocalSettings);
    }

    [TestMethod]
    public void Kopia_VelkostiOkienSuNezavisle()
    {
        var config = new GVDEditorConfig();
        var copy = config with { };

        copy.Windows.LocalSettings = new WindowPlacement { Width = 900 };

        Assert.IsNull(config.Windows.LocalSettings);
    }
}
