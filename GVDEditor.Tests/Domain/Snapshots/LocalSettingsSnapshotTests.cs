using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Snapshots;

namespace GVDEditor.Tests.Domain.Snapshots;

/// <summary>
/// Tlacidlo Zrusit v okne Lokalne nastavenia: zmeny na vsetkych zalozkach (vratane dopadu na vlaky) sa vratia.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class LocalSettingsSnapshotTests
{
    private GrafikonDocument _doc = new();

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

        _doc.Operators = new ExBindingList<Operator> { Operator.None, _zssk };
        _doc.Platforms = new ExBindingList<Platform> { Platform.None, Platform1 };
        _doc.Tracks = new ExBindingList<Track> { Track.None, _track1, _track2 };
        _doc.Trains = new ExBindingList<Train> { _train };
        _doc.TablePhysicals = new ExBindingList<TablePhysical>();
        _doc.TableLogicals = new ExBindingList<TableLogical> { _logical };
        _doc.TableCatalogs = new ExBindingList<TableCatalog>();
        _doc.TabTabs = new ExBindingList<TableTabTab> { new() { Key = "A", Text = "[MAIN]" } };
        _doc.TableTexts = new ExBindingList<TableText>();
        _doc.TableFonts = new ExBindingList<TableFont> { _font };
        _doc.CustomStations = new ExBindingList<Station> { new("900", "Vlastná", IsCustom: true) };
        _doc.ModeTabsSections = new Dictionary<string, Dictionary<string, string>> { ["ALIGN"] = new() { ["0"] = "vľavo" } };
    }

    [TestMethod]
    public void Zrusit_VratiUpravyPridaniaAMazania()
    {
        var snapshot = LocalSettingsSnapshot.Capture(_doc);

        _zssk.Name = "RegioJet";
        _doc.Operators.Add(new Operator(2, "Leo Express"));
        Platform1.FullName = "Iné";
        _track1.Tables.Clear();
        _track1.Key = "9";
        _font.FontID = 7;
        _doc.TableFonts.RemoveAt(0);
        _doc.TabTabs[0].Text = "[ZMENA]";
        _doc.CustomStations[0].Name = "Premenovaná";
        _doc.ModeTabsSections["ALIGN"]["0"] = "vpravo";
        _doc.ModeTabsSections["NOVA"] = new Dictionary<string, string>();

        snapshot.Restore();

        Assert.AreEqual("ZSSK", _zssk.Name);
        CollectionAssert.AreEqual(new[] { Operator.None, _zssk }, _doc.Operators.ToList());
        Assert.AreEqual("Nástupište 1", Platform1.FullName);
        Assert.AreEqual("1", _track1.Key);
        CollectionAssert.AreEqual(new[] { _logical }, _track1.Tables.ToList());
        CollectionAssert.AreEqual(new[] { _font }, _doc.TableFonts.ToList());
        Assert.AreEqual(5, _font.FontID);
        Assert.AreEqual("[MAIN]", _doc.TabTabs[0].Text);
        Assert.AreEqual("Vlastná", _doc.CustomStations[0].Name);
        Assert.AreEqual("vľavo", _doc.ModeTabsSections["ALIGN"]["0"]);
        Assert.IsFalse(_doc.ModeTabsSections.ContainsKey("NOVA"));
    }

    [TestMethod]
    public void Zrusit_VratiVlakomKolajADopravcuPoZmazani()
    {
        var snapshot = LocalSettingsSnapshot.Capture(_doc);

        TrackEditing.Remove(_track1, _doc.Tracks, _doc.Trains);
        _train.Operator = Operator.None;
        _doc.Operators.Remove(_zssk);

        snapshot.Restore();

        Assert.AreSame(_track1, _train.Track);
        Assert.AreSame(_track2, _train.TrackDeparture);
        Assert.AreSame(_zssk, _train.Operator);
        CollectionAssert.AreEqual(new[] { Track.None, _track1, _track2 }, _doc.Tracks.ToList());
        CollectionAssert.Contains(_doc.Operators.ToList(), _zssk);
    }

    [TestMethod]
    public void Zrusit_ZrusiOdberyUdalostiPridanePoSnimkeAPovodneNecha()
    {
        var povodne = 0;
        var okno = 0;
        _doc.Tracks.ListChanged += (_, _) => povodne++;
        var snapshot = LocalSettingsSnapshot.Capture(_doc);
        _doc.Tracks.ListChanged += (_, _) => okno++;

        snapshot.Restore();

        Assert.AreEqual(1, povodne, "obnovenie ma zavolat ResetBindings");
        Assert.AreEqual(0, okno);
    }

    [TestMethod]
    public void Zrusit_VlakyLenPrekresliBezPrestavaniaZoznamu()
    {
        var types = new List<ListChangedType>();
        _doc.Trains.ListChanged += (_, e) => types.Add(e.ListChangedType);
        var snapshot = LocalSettingsSnapshot.Capture(_doc);

        snapshot.Restore();

        // Reset by v hlavnom okne prestavil riadky tabulky vlakov a grafikon by sa oznacil ako zmeneny
        CollectionAssert.AreEqual(new[] { ListChangedType.ItemChanged }, types);
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
