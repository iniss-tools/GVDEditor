using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Tools;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Audio.txt (Globalne nastavenia → Audio): testovaci okruh TEST a riadky za prvym '/'.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class AudioFileTests
{
    private string _dir = null!;
    private string? _oldDataDir;

    [TestInitialize]
    public void Init()
    {
        _dir = Path.Combine(Path.GetTempPath(), "AudioFileTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);

        var prop = typeof(GlobData).GetProperty(nameof(GlobData.DataDir))!;
        _oldDataDir = (string?)prop.GetValue(null);
        prop.SetValue(null, _dir);

        GlobData.Stations = [new Station("9900100", "Dolné Mesto")];
        GlobData.CustomStations = new ExBindingList<Station>();
    }

    [TestCleanup]
    public void Cleanup()
    {
        typeof(GlobData).GetProperty(nameof(GlobData.DataDir))!.SetValue(null, _oldDataDir);
        Directory.Delete(_dir, true);
    }

    private string File_ => Path.Combine(_dir, GvdFileConsts.FILE_AUDIO);

    [TestMethod]
    public void Audio_TestARiadkyZaLomkou_PrezijuNacitanieAZapis()
    {
        File.WriteAllLines(File_,
        [
            "9900100,Dolné Mesto,Dolné Mesto,Hlásenie,",
            "TEST,Test,Test,TestHlas,",
            "/koniec okruhov",
            "9900200,Stará linka,STARA,Stara,"
        ], Encodings.Win1250);

        var audios = AudioFile.Read(GlobData.DataDir);

        CollectionAssert.AreEqual(new[] { "9900100", "TEST" }, audios.Select(a => a.Station.ID).ToArray());

        AudioFile.Write(GlobData.DataDir, audios);
        var lines = File.ReadAllLines(File_, Encodings.Win1250);

        StringAssert.StartsWith(lines[1], "TEST,Test,Test,TestHlas", lines[1]);
        Assert.AreEqual("/koniec okruhov", lines[2]);
        Assert.AreEqual("9900200,Stará linka,STARA,Stara,", lines[3]);
        Assert.AreEqual(2, AudioFile.Read(GlobData.DataDir).Count);
    }
}
