using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Entities;

namespace GVDEditor.Tests;

/// <summary>
///     Tlacidlo Zrusit v okne Globalne nastavenia: zmeny jazykov, meskani, typov vlakov a audio liniek sa vratia.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GlobalSettingsSnapshotTests
{
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

        GlobData.Languages = _languages = new ExBindingList<FyzLanguage> { _sk, _cz };
        GlobData.Delays = _delays = new ExBindingList<string> { "5", "10" };
        GlobData.TrainsTypes = _trainTypes = new ExBindingList<TrainType> { _os, _custom };
        GlobData.Audios = _audios = new ExBindingList<Audio> { _audio };
    }

    [TestMethod]
    public void Zrusit_VratiUpravyPoloziekNaMieste()
    {
        var snapshot = GlobalSettingsSnapshot.Capture();

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
        var snapshot = GlobalSettingsSnapshot.Capture();

        GlobData.Languages.RemoveAt(1);
        GlobData.Languages.Add(new FyzLanguage("D", "Německy"));
        GlobData.Delays.RemoveAt(0);
        GlobData.Delays.Insert(0, "15");
        GlobData.Delays.Add("20");
        GlobData.TrainsTypes[1] = new TrainType("X1", "LE", "Leo Express");
        GlobData.TrainsTypes.Add(new TrainType("Ex"));
        GlobData.Audios.Clear();

        snapshot.Restore();

        CollectionAssert.AreEqual(new[] { _sk, _cz }, GlobData.Languages.ToList());
        CollectionAssert.AreEqual(new[] { "5", "10" }, GlobData.Delays.ToList());
        CollectionAssert.AreEqual(new[] { _os, _custom }, GlobData.TrainsTypes.ToList());
        Assert.AreSame(_custom, GlobData.TrainsTypes[1]);
        CollectionAssert.AreEqual(new[] { _audio }, GlobData.Audios.ToList());
    }

    [TestMethod]
    public void Zrusit_PonechaTieIsteInstancieZoznamov()
    {
        var snapshot = GlobalSettingsSnapshot.Capture();
        GlobData.Delays.Add("30");

        snapshot.Restore();

        Assert.AreSame(_languages, GlobData.Languages);
        Assert.AreSame(_delays, GlobData.Delays);
        Assert.AreSame(_trainTypes, GlobData.TrainsTypes);
        Assert.AreSame(_audios, GlobData.Audios);
    }

    [TestMethod]
    public void Zrusit_ZrusiOdberyOknaAZavolaResetBindings()
    {
        var povodne = new List<ListChangedType>();
        var okno = 0;
        GlobData.TrainsTypes.ListChanged += (_, e) => povodne.Add(e.ListChangedType);
        var snapshot = GlobalSettingsSnapshot.Capture();
        GlobData.TrainsTypes.ListChanged += (_, _) => okno++;
        GlobData.TrainsTypes.FireEventOnSort = true;

        snapshot.Restore();

        CollectionAssert.AreEqual(new[] { ListChangedType.Reset }, povodne);
        Assert.AreEqual(0, okno);
        Assert.IsFalse(GlobData.TrainsTypes.FireEventOnSort);
    }
}
