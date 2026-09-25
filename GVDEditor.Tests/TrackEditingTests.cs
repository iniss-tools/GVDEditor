using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Ciselnik kolaji a nastupist (Lokalne nastavenia → Nastupistia, Kolaje): zmazanie kolaje nesmie nechat v Pozice.txt
///     odkaz na neexistujucu kolaj, nastupiste bez kolaje sa do Pozice_A.txt nezapise.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrackEditingTests
{
    private static readonly Platform Platform1 = new("1", "Nástupište 1", "01");
    private static readonly Platform Platform2 = new("2", "Nástupište 2", "02");

    private static Track NewTrack(string key, Platform platform, string? text = null) =>
        new(key, key, "Koľaj " + key, platform, key + "00", text ?? key);

    private static List<Track> Tracks() => [Track.None, NewTrack("1", Platform1), NewTrack("2", Platform1), NewTrack("3", Platform2)];

    private static Train NewTrain(Track track, Track? departure = null) => new() { Track = track, TrackDeparture = departure };

    [TestMethod]
    public void OdstranenieKolaje_VlakyPresunieNaNAZrusiKolajOdchodu()
    {
        var tracks = Tracks();
        var (t1, t2, t3) = (tracks[1], tracks[2], tracks[3]);
        var stoji = NewTrain(t1);
        var odchadza = NewTrain(t2, t1);
        var prichadza = NewTrain(t1, t3);
        var ina = NewTrain(t3, t2);

        TrackEditing.Remove(t1, tracks, [stoji, odchadza, prichadza, ina]);

        CollectionAssert.DoesNotContain(tracks, t1);
        Assert.AreSame(Track.None, stoji.Track);
        Assert.IsNull(stoji.TrackDeparture);
        Assert.AreSame(t2, odchadza.Track);
        Assert.IsNull(odchadza.TrackDeparture);
        Assert.AreSame(Track.None, prichadza.Track);
        Assert.AreSame(t3, prichadza.TrackDeparture);
        Assert.AreSame(t3, ina.Track);
        Assert.AreSame(t2, ina.TrackDeparture);
    }

    [TestMethod]
    public void OdstranenieKolaje_PocetVlakovPodlaPrichoduAOdchodu()
    {
        var tracks = Tracks();
        var (t1, t2) = (tracks[1], tracks[2]);

        var usage = TrackEditing.CountUsage(t1, [NewTrain(t1), NewTrain(t1, t2), NewTrain(t2, t1), NewTrain(t2)]);

        Assert.AreEqual((2, 1), usage);
        Assert.AreEqual((0, 0), TrackEditing.CountUsage(tracks[3], [NewTrain(t1), NewTrain(t2, t1)]));
    }

    [TestMethod]
    public void OdstranenieKolaje_PoziceSaPoUlozeniZnovaNacita()
    {
        var dir = Directory.CreateTempSubdirectory("gvdtracks");
        var oldLogicals = GlobData.TableLogicals;
        try
        {
            GlobData.TableLogicals = new ExBindingList<TableLogical>();
            var tracks = Tracks();
            var (t1, t2, t3) = (tracks[1], tracks[2], tracks[3]);
            List<Train> trains = [NewTrain(t1), NewTrain(t2, t1), NewTrain(t1, t3), NewTrain(t3)];

            TrackEditing.Remove(t1, tracks, trains);

            TxtParser.WriteTracks(dir.FullName, tracks);
            var pozice = Path.Combine(dir.FullName, FileConsts.FILE_POZICE);
            TxtParser.WritePositions(pozice, trains, []);

            var readTracks = TxtParser.ReadTracks(dir.FullName);
            List<Train> readTrains = [new(), new(), new(), new()];
            TxtParser.ReadPositions(pozice, readTrains, readTracks);

            CollectionAssert.AreEqual(new[] { "N", "2", "3" }, readTracks.Select(t => t.Key).ToArray());
            CollectionAssert.AreEqual(new[] { "N", "2", "N", "3" }, readTrains.Select(t => t.Track.Key).ToArray());
            CollectionAssert.AreEqual(new[] { null, null, "3", null }, readTrains.Select(t => t.TrackDeparture?.Key).ToArray());
        }
        finally
        {
            GlobData.TableLogicals = oldLogicals;
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Pozice_NeexistujucaKolajOdchoduJeChyba()
    {
        // stav pred opravou: vlaku ostala kolaj odchodu, ktora uz v Pozice_A.txt nie je
        var dir = Directory.CreateTempSubdirectory("gvdtracks");
        try
        {
            var tracks = Tracks();
            var pozice = Path.Combine(dir.FullName, FileConsts.FILE_POZICE);
            TxtParser.WritePositions(pozice, [NewTrain(tracks[2], tracks[1])], []);
            tracks.RemoveAt(1);

            Assert.ThrowsExactly<FormatException>(() => TxtParser.ReadPositions(pozice, [new Train()], tracks));
        }
        finally
        {
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Kolaj_NazovATextNaTabuleSaZapisuDoStlpcov2A5()
    {
        var dir = Directory.CreateTempSubdirectory("gvdtracks");
        var oldLogicals = GlobData.TableLogicals;
        try
        {
            GlobData.TableLogicals = new ExBindingList<TableLogical>();

            TxtParser.WriteTracks(dir.FullName, [Track.None, NewTrack("6V", Platform1, "6")]);

            var line = File.ReadAllLines(Path.Combine(dir.FullName, FileConsts.FILE_POZICE_A), ToolsCore.Tools.Encodings.Win1250)[1];
            StringAssert.StartsWith(line, "\"6V\",\"6V\",\"Koľaj 6V\",\"Nástupište 1\",\"6\",\"1\",");

            var read = TxtParser.ReadTracks(dir.FullName)[1];
            Assert.AreEqual("6V", read.Name);
            Assert.AreEqual("6", read.TrackName);
        }
        finally
        {
            GlobData.TableLogicals = oldLogicals;
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void NahladTabule_NastupisteZoStlpca5KolajZoStlpca6()
    {
        // INISS 3.39: funkcie 19/20 (Nastupiste...) citaju stlpec 5, 21/22 (Kolej...) stlpec 6 Pozice_A
        var arrival = NewTrack("6V", Platform1, "6");
        var departure = NewTrack("5", Platform2);
        var train = new Train { Track = arrival, TrackDeparture = departure, Arrival = DateTime.Today, Departure = DateTime.Today };
        var context = new GvdTrainContext(train, new TrainRuntime(), 0);

        string Value(TableFillSection section) => context.OwnValue(new TableItem { FillSection = section }).Text;

        Assert.AreEqual("6", Value(TableFillSection.KolajPrichod));
        Assert.AreEqual("5", Value(TableFillSection.KolajOdchod));
        Assert.AreEqual("1", Value(TableFillSection.NastupistePrichod));
        Assert.AreEqual("2", Value(TableFillSection.NastupisteOdchod));

        train.Track = Track.None;
        train.TrackDeparture = null;
        Assert.AreEqual("", Value(TableFillSection.KolajPrichod));
        Assert.AreEqual("", Value(TableFillSection.NastupisteOdchod));
    }

    [TestMethod]
    public void Nastupistia_BezKolajeSaHlasia()
    {
        var platform3 = new Platform("3", "Nástupište 3", "03");
        var tracks = Tracks();

        var without = TrackEditing.PlatformsWithoutTracks([Platform.None, Platform1, Platform2, platform3], tracks);

        CollectionAssert.AreEqual(new[] { platform3 }, without);
    }

    /// <summary>
    ///     Zapise Pozice_A.txt z riadkov, nacita ho a vrati kolaje (bez Track.None).
    /// </summary>
    private static List<Track> ReadPoziceA(params string[] lines)
    {
        var dir = Directory.CreateTempSubdirectory("gvdtracks");
        var oldLogicals = GlobData.TableLogicals;
        try
        {
            GlobData.TableLogicals = new ExBindingList<TableLogical>();
            File.WriteAllLines(Path.Combine(dir.FullName, FileConsts.FILE_POZICE_A), lines, ToolsCore.Tools.Encodings.Win1250);
            return TxtParser.ReadTracks(dir.FullName).Skip(1).ToList();
        }
        finally
        {
            GlobData.TableLogicals = oldLogicals;
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Nastupiste_KolajeJednehoNastupistaZdielajuInstanciu()
    {
        var tracks = ReadPoziceA(
            "\"5V\",\"5V\",\"Kolej 5V\",\"Nástupiště 3\",\"5V\",\"3\",\"0501\",\"03\",0",
            "\"7\",\"7\",\"Kolej 7\",\"Nástupiště 2\",\"7\",\"2\",\"0700\",\"02\",0",
            "\"5Z\",\"5Z\",\"Kolej 5Z\",\"Nástupiště 3\",\"5Z\",\"3\",\"0502\",\"03\",0");

        Assert.AreSame(tracks[0].Platform, tracks[2].Platform);
        Assert.AreNotSame(tracks[0].Platform, tracks[1].Platform);

        // uprava nastupista (FLocalSettings.bNastEdit_Click) sa prejavi na oboch kolajach
        tracks[0].Platform.Key = "3A";
        Assert.AreEqual("3A", tracks[2].Platform.Key);
    }

    [TestMethod]
    public void Nastupiste_RozneUdajeVRiadkochVyhraNajcastejsia()
    {
        LoadWarnings.Clear();
        var tracks = ReadPoziceA(
            "\"1\",\"1\",\"koľaj 1\",\"nástupište \",\"1\",\"3\",\"0100\",\"03\",0",
            "\"3A\",\"3A\",\"koľaj 3A\",\"nástupište 3\",\"3A\",\"3\",\"0303\",\"03\",0",
            "\"4\",\"4\",\"koľaj 4\",\"nástupište 3\",\"4\",\"3\",\"0400\",\"03\",0");

        Assert.IsTrue(tracks.All(track => ReferenceEquals(track.Platform, tracks[1].Platform)));
        Assert.AreEqual("nástupište 3", tracks[0].Platform.FullName);
        Assert.HasCount(1, LoadWarnings.Items);
        StringAssert.Contains(LoadWarnings.Items[0], "nástupište 3");
        LoadWarnings.Clear();
    }

    [TestMethod]
    public void Nastupiste_KlucNJeVzdyPlatformNone()
    {
        LoadWarnings.Clear();
        var tracks = ReadPoziceA(
            "\"N\",\"-\",\"Neznámá\",\"Neznámé\",\"\",\"\",\"\",\"\",0",
            "\"BUS\",\"BUS\",\"koľaj BUS\",\"Nedefinované\",\"BUS\",\"N\",\"0\",\"\",0",
            "\"3V\",\"3V\",\"koľaj 3V\",\"Nedefinované\",\"3V\",\"N\",\"0301\",\"\",0");

        Assert.IsTrue(tracks.All(track => ReferenceEquals(track.Platform, Platform.None)));
        Assert.HasCount(0, LoadWarnings.Items);
    }
}
