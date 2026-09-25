using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Tlacidlo Zrusit v okne Lokalne nastavenia: zmeny na vsetkych zalozkach (vratane dopadu na vlaky) sa vratia.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class LocalSettingsSnapshotTests
{
    private static readonly Platform Platform1 = new("1", "Nástupište 1", "01");

    private Operator _zssk = null!;
    private Track _track1 = null!;
    private Track _track2 = null!;
    private TableLogical _logical = null!;
    private TableFont _font = null!;
    private Train _train = null!;

    [TestInitialize]
    public void Init()
    {
        _zssk = new Operator(1, "ZSSK");
        _logical = new TableLogical { Name = "Odchody" };
        _track1 = new Track("1", "1", "Koľaj 1", Platform1, "100", "1");
        _track1.Tables.Add(_logical);
        _track2 = new Track("2", "2", "Koľaj 2", Platform1, "200", "2");
        _font = new TableFont { Name = "Normal", FontID = 5 };
        _train = new Train { Track = _track1, TrackDeparture = _track2, Operator = _zssk };

        GlobData.Operators = new ExBindingList<Operator> { Operator.None, _zssk };
        GlobData.Platforms = new ExBindingList<Platform> { Platform.None, Platform1 };
        GlobData.Tracks = new ExBindingList<Track> { Track.None, _track1, _track2 };
        GlobData.Trains = new ExBindingList<Train> { _train };
        GlobData.TablePhysicals = new ExBindingList<TablePhysical>();
        GlobData.TableLogicals = new ExBindingList<TableLogical> { _logical };
        GlobData.TableCatalogs = new ExBindingList<TableCatalog>();
        GlobData.TabTabs = new ExBindingList<TableTabTab> { new() { Key = "A", Text = "[MAIN]" } };
        GlobData.TableTexts = new ExBindingList<TableText>();
        GlobData.TableFonts = new ExBindingList<TableFont> { _font };
        GlobData.CustomStations = new ExBindingList<Station> { new("900", "Vlastná", IsCustom: true) };
        GlobData.ModeTabsSections = new Dictionary<string, Dictionary<string, string>> { ["ALIGN"] = new() { ["0"] = "vľavo" } };
    }

    [TestMethod]
    public void Zrusit_VratiUpravyPridaniaAMazania()
    {
        var snapshot = LocalSettingsSnapshot.Capture();

        _zssk.Name = "RegioJet";
        GlobData.Operators.Add(new Operator(2, "Leo Express"));
        Platform1.FullName = "Iné";
        _track1.Tables.Clear();
        _track1.Key = "9";
        _font.FontID = 7;
        GlobData.TableFonts.RemoveAt(0);
        GlobData.TabTabs[0].Text = "[ZMENA]";
        GlobData.CustomStations[0].Name = "Premenovaná";
        GlobData.ModeTabsSections["ALIGN"]["0"] = "vpravo";
        GlobData.ModeTabsSections["NOVA"] = new Dictionary<string, string>();

        snapshot.Restore();

        Assert.AreEqual("ZSSK", _zssk.Name);
        CollectionAssert.AreEqual(new[] { Operator.None, _zssk }, GlobData.Operators.ToList());
        Assert.AreEqual("Nástupište 1", Platform1.FullName);
        Assert.AreEqual("1", _track1.Key);
        CollectionAssert.AreEqual(new[] { _logical }, _track1.Tables.ToList());
        CollectionAssert.AreEqual(new[] { _font }, GlobData.TableFonts.ToList());
        Assert.AreEqual(5, _font.FontID);
        Assert.AreEqual("[MAIN]", GlobData.TabTabs[0].Text);
        Assert.AreEqual("Vlastná", GlobData.CustomStations[0].Name);
        Assert.AreEqual("vľavo", GlobData.ModeTabsSections["ALIGN"]["0"]);
        Assert.IsFalse(GlobData.ModeTabsSections.ContainsKey("NOVA"));
    }

    [TestMethod]
    public void Zrusit_VratiVlakomKolajADopravcuPoZmazani()
    {
        var snapshot = LocalSettingsSnapshot.Capture();

        TrackEditing.Remove(_track1, GlobData.Tracks, GlobData.Trains);
        _train.Operator = Operator.None;
        GlobData.Operators.Remove(_zssk);

        snapshot.Restore();

        Assert.AreSame(_track1, _train.Track);
        Assert.AreSame(_track2, _train.TrackDeparture);
        Assert.AreSame(_zssk, _train.Operator);
        CollectionAssert.AreEqual(new[] { Track.None, _track1, _track2 }, GlobData.Tracks.ToList());
        CollectionAssert.Contains(GlobData.Operators.ToList(), _zssk);
    }

    [TestMethod]
    public void Zrusit_ZrusiOdberyUdalostiPridanePoSnimkeAPovodneNecha()
    {
        var povodne = 0;
        var okno = 0;
        GlobData.Tracks.ListChanged += (_, _) => povodne++;
        var snapshot = LocalSettingsSnapshot.Capture();
        GlobData.Tracks.ListChanged += (_, _) => okno++;

        snapshot.Restore();

        Assert.AreEqual(1, povodne, "obnovenie ma zavolat ResetBindings");
        Assert.AreEqual(0, okno);
    }

    [TestMethod]
    public void Snimka_NesledujeRetazceAniNeznameTypy()
    {
        var bitmap = new object();
        var snapshot = ObjectGraphSnapshot.Capture([new List<object> { "text", bitmap, new BindingList<int> { 1 } }], _ => false);

        // List + jeho pole, BindingList + vnutorny List + jeho pole; retazec a neznamy objekt sa nesleduju
        Assert.AreEqual(5, snapshot.Count);
    }
}
