using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using Field = GVDEditor.Domain.Rules.PlatformTrackRules.Field;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Nastupistia a kolaje stanice (Lokalne nastavenia → Nastupistia a kolaje, Pozice_A.txt).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class PlatformTrackRulesTests
{
    private static Track T(string key, string sound = "0001") => new()
    {
        Key = key, Name = key, FullName = "Koľaj " + key, TrackName = key, SoundName = sound, Platform = Platform.None
    };

    [TestMethod]
    public void Nastupiste_PovinnePoliaAJedinecneOznacenie()
    {
        Platform[] platforms = [new("1", "Nástupište 1", "01"), new("1", "Nástupište 1b", "01"), new("2", " ", "02"), new("3", "N3", "")];

        Assert.AreEqual(Field.Key, PlatformTrackRules.CheckPlatform(platforms, 0)?.Field);
        Assert.AreEqual(Field.Key, PlatformTrackRules.CheckPlatform(platforms, 1)?.Field);
        Assert.AreEqual(Field.FullName, PlatformTrackRules.CheckPlatform(platforms, 2)?.Field);
        Assert.AreEqual(Field.SoundName, PlatformTrackRules.CheckPlatform(platforms, 3)?.Field);
    }

    [TestMethod]
    public void Kolaj_VPoriadku()
    {
        Assert.IsNull(PlatformTrackRules.CheckTrack([T("1"), T("2")], 0));
    }

    [TestMethod]
    public void Kolaj_RovnakeOznacenieAChybajuciZvuk()
    {
        Track[] tracks = [T("1"), T(" 1 "), T("3", "")];

        Assert.AreEqual(Field.Key, PlatformTrackRules.CheckTrack(tracks, 0)?.Field);
        Assert.AreEqual(Field.Key, PlatformTrackRules.CheckTrack(tracks, 1)?.Field);
        Assert.AreEqual(Field.SoundName, PlatformTrackRules.CheckTrack(tracks, 2)?.Field);
    }

    [TestMethod]
    public void Kolaj_ChybajuciTextNaTabuli()
    {
        var track = T("5");
        track.TrackName = "";

        Assert.AreEqual(Field.TrackName, PlatformTrackRules.CheckTrack([track], 0)?.Field);
    }

    [TestMethod]
    public void NoveOznacenie_ZaNajvyssimCislom()
    {
        Assert.AreEqual("4", PlatformTrackRules.SuggestKey(["1", "3", "2a", null]));
        Assert.AreEqual("1", PlatformTrackRules.SuggestKey(["N"]));
    }
}
