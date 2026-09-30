using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Snapshots;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Domain.Snapshots;

/// <summary>
/// Tlacidlo Zrusit v okne Globalne nastavenia: zmeny jazykov, meskani, typov vlakov a audio liniek sa vratia.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GlobalSettingsSnapshotTests
{
    private InissWorkspace _ws = new();

    private static readonly Station Stanica = new("100", "Stanica");

    private FyzLanguage _sk = null!;
    private FyzLanguage _cz = null!;
    private TrainType _os = null!;
    private TrainType _custom = null!;
    private Audio _audio = null!;
    private ExBindingList<FyzLanguage> _languages = null!;
    private ExBindingList<string> _delays = null!;
    private ExBindingList<TrainType> _trainTypes = null!;
    private ExBindingList<Audio> _audios = null!;

    [TestInitialize]
    public void Init()
    {
        _sk = new FyzLanguage("SK", "Slovensky") { IsBasic = true };
        _cz = new FyzLanguage("CZ", "Česky");
        _os = new TrainType("Os");
        _custom = new TrainType("R1", "RR", "RegioRapid");
        _audio = new Audio { Station = Stanica, Name = "Nástupištia", ShortName = "NAST", QueueName = "NAST", Mixer = "", SoundCard = "1" };

        _ws.Languages = _languages = new ExBindingList<FyzLanguage> { _sk, _cz };
        _ws.Delays = _delays = new ExBindingList<string> { "5", "10" };
        _ws.TrainsTypes = _trainTypes = new ExBindingList<TrainType> { _os, _custom };
        _ws.Audios = _audios = new ExBindingList<Audio> { _audio };
    }

    [TestMethod]
    public void Zrusit_VratiUpravyPoloziekNaMieste()
    {
        var snapshot = GlobalSettingsSnapshot.Capture(_ws);

        _sk.Name = "Iný";
        _sk.Key = "GB";
        _sk.IsBasic = false;
        _os.TextInTable = "Osobný";
        _custom.CategoryTrain = "R2";
        _audio.Name = "Hala";
        _audio.Station = new Station("200", "Iná");
        _audio.Node = "COM3";

        snapshot.Restore();

        Assert.AreEqual("Slovensky", _sk.Name);
        Assert.AreEqual("SK", _sk.Key);
        Assert.IsTrue(_sk.IsBasic);
        Assert.AreEqual("Os", _os.TextInTable);
        Assert.AreEqual("R1", _custom.CategoryTrain);
        Assert.AreEqual("Nástupištia", _audio.Name);
        Assert.AreSame(Stanica, _audio.Station);
        Assert.AreEqual("", _audio.Node);
    }

    [TestMethod]
    public void Zrusit_VratiPridaniaMazaniaANahradenia()
    {
        var snapshot = GlobalSettingsSnapshot.Capture(_ws);

        _ws.Languages.RemoveAt(1);
        _ws.Languages.Add(new FyzLanguage("D", "Německy"));
        _ws.Delays.RemoveAt(0);
        _ws.Delays.Insert(0, "15");
        _ws.Delays.Add("20");
        _ws.TrainsTypes[1] = new TrainType("X1", "LE", "Leo Express");
        _ws.TrainsTypes.Add(new TrainType("Ex"));
        _ws.Audios.Clear();

        snapshot.Restore();

        CollectionAssert.AreEqual(new[] { _sk, _cz }, _ws.Languages.ToList());
        CollectionAssert.AreEqual(new[] { "5", "10" }, _ws.Delays.ToList());
        CollectionAssert.AreEqual(new[] { _os, _custom }, _ws.TrainsTypes.ToList());
        Assert.AreSame(_custom, _ws.TrainsTypes[1]);
        CollectionAssert.AreEqual(new[] { _audio }, _ws.Audios.ToList());
    }

    [TestMethod]
    public void Zrusit_PonechaTieIsteInstancieZoznamov()
    {
        var snapshot = GlobalSettingsSnapshot.Capture(_ws);
        _ws.Delays.Add("30");

        snapshot.Restore();

        Assert.AreSame(_languages, _ws.Languages);
        Assert.AreSame(_delays, _ws.Delays);
        Assert.AreSame(_trainTypes, _ws.TrainsTypes);
        Assert.AreSame(_audios, _ws.Audios);
    }

    [TestMethod]
    public void Zrusit_ZrusiOdberyOknaAZavolaResetBindings()
    {
        var povodne = new List<ListChangedType>();
        var okno = 0;
        _ws.TrainsTypes.ListChanged += (_, e) => povodne.Add(e.ListChangedType);
        var snapshot = GlobalSettingsSnapshot.Capture(_ws);
        _ws.TrainsTypes.ListChanged += (_, _) => okno++;
        _ws.TrainsTypes.FireEventOnSort = true;

        snapshot.Restore();

        CollectionAssert.AreEqual(new[] { ListChangedType.Reset }, povodne);
        Assert.AreEqual(0, okno);
        Assert.IsFalse(_ws.TrainsTypes.FireEventOnSort);
    }
}
