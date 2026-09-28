using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
///     Audio linky (Globalne nastavenia → Audio, Audio.txt) a porty grafikonov (Grafikony, DirList.TXT).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class AudioRulesTests
{
    private static Audio Line(string station, string name, string queue = "", string node = "") => new()
    {
        Station = new Station(station, station),
        Name = name,
        ShortName = name,
        QueueName = queue.Length == 0 ? name : queue,
        Mixer = "",
        SoundCard = "",
        Node = node
    };

    [TestMethod]
    public void Audio_BeznaLinka_JeVPoriadku()
    {
        Audio[] audios = [Line("5616590", "Dolné Mesto"), Line("TEST", "Test", "TestHlas")];

        Assert.IsNull(AudioRules.Check(audios, 0));
        Assert.IsNull(AudioRules.Check(audios, 1));
    }

    [TestMethod]
    public void Audio_RovnakyNazov_ChybaAzPriDruhej()
    {
        Audio[] audios = [Line("5616590", "Hlásenie"), Line("5614616", "Hlásenie", "Druha")];

        Assert.IsNull(AudioRules.Check(audios, 0));
        Assert.AreEqual(AudioRules.Field.Name, AudioRules.Check(audios, 1)?.Field);
    }

    [TestMethod]
    public void Audio_DvaTestovacieOkruhy_JeChyba()
    {
        Audio[] audios = [Line("TEST", "Test"), Line("test", "Test 2")];

        Assert.AreEqual(AudioRules.Field.Station, AudioRules.Check(audios, 1)?.Field);
    }

    [TestMethod]
    public void Audio_PrazdnaFronta_JeChyba()
    {
        var audio = Line("5616590", "Dolné Mesto");
        audio.QueueName = " ";

        Assert.AreEqual(AudioRules.Field.Queue, AudioRules.Check([audio], 0)?.Field);
    }

    [TestMethod]
    public void Audio_RovnakaFrontaInyUzol_JeChyba()
    {
        Audio[] same = [Line("5616590", "A", "Spolocna"), Line("5614616", "B", "Spolocna", "0")];
        Audio[] other = [Line("5616590", "A", "Spolocna"), Line("5614616", "B", "Spolocna", "2")];

        // prazdny uzol a 0 su ten isty pocitac
        Assert.IsNull(AudioRules.Check(same, 1));
        Assert.AreEqual(AudioRules.Field.Node, AudioRules.Check(other, 1)?.Field);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("0", true)]
    [DataRow("63", true)]
    [DataRow("64", false)]
    [DataRow("E1", true)]
    [DataRow("E99", true)]
    [DataRow("E0", false)]
    [DataRow("EE12", true)]
    [DataRow("Zcokolvek", true)]
    [DataRow("X5", false)]
    [DataRow("-1", false)]
    public void Audio_SpinanieZosilnovaca(string value, bool valid)
    {
        Assert.AreEqual(valid, AudioRules.CheckAmplifier(value) is null);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("0", true)]
    [DataRow("65535", true)]
    [DataRow("65536", false)]
    [DataRow("abc", false)]
    public void Audio_ParameterUstredne(string value, bool valid)
    {
        Assert.AreEqual(valid, AudioRules.CheckExchange(value) is null);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("0", true)]
    [DataRow("12", true)]
    [DataRow("COM3", true)]
    [DataRow("com3", true)]
    [DataRow(@"\\.\COM3", true)]
    [DataRow("//./COM3", true)]
    [DataRow("COM", false)]
    [DataRow("uzol", false)]
    public void Audio_Uzol(string value, bool valid)
    {
        Assert.AreEqual(valid, AudioRules.CheckNode(value) is null);
    }

    [TestMethod]
    public void Audio_NovyNazov_JeJedinecny()
    {
        Audio[] audios = [Line("1", "Hlásenie"), Line("2", "Hlásenie 2")];

        Assert.AreEqual("Hlásenie 3", AudioRules.UniqueName(audios, "Hlásenie"));
        Assert.AreEqual("Hlásenie 3", AudioRules.UniqueName(audios, "Hlásenie 2"));
        Assert.AreEqual("Iné", AudioRules.UniqueName(audios, "Iné"));
    }

    [TestMethod]
    [DataRow("", null, true)]
    [DataRow("0", null, true)]
    [DataRow(" 4 ", 4, true)]
    [DataRow("255", 255, true)]
    [DataRow("256", null, false)]
    [DataRow("-1", null, false)]
    [DataRow("x", null, false)]
    public void Grafikon_Port(string text, int? expected, bool valid)
    {
        var error = DirListRules.ParsePort(text, out var port);

        Assert.AreEqual(valid, error is null);
        Assert.AreEqual(expected, port);
    }
}
