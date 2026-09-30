using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Cely grafikon: ulozenie a nacitanie vsetkych suborov (vlaky, trasy, kolaje, doplnky, jazyky, vyluky, radenia,
/// vlastne stanice). Druhe ulozenie nacitaneho grafikonu musi dat tie iste subory.
/// </summary>
[TestClass]
public class GrafikonRepositoryTests
{
    private readonly LoadWarnings _warnings = new();

    private static readonly FyzLanguage Sk = new("SK", "Slovenčina", "SK\\") { IsBasic = true };
    private static readonly FyzLanguage Gb = new("GB", "Angličtina", "GB\\");

    private static readonly FyzGroup Dodatky = new(Sk, "DODATKY", "Dodatkové hlásenia", "DODATKY\\");
    private static readonly FyzGroup Vozy = new(Sk, "VOZY1", "Vozne", "VOZY1\\");
    private static readonly FyzSound Dodatok1001 = new(Dodatky, "D1001", "D1001", "D1001.WAV", "", "Vlak má bufet", 0);
    private static readonly FyzSound Vozen = new(Vozy, "V1", "V1", "V1.WAV", "", "vozeň prvej triedy", 0);

    private static readonly Station Home = new("5000", "Dolné Mesto");
    private static readonly Station Horna = new("5100", "Horná");
    private static readonly Station Zapad = new("5200", "Západ");

    private static readonly GVDInfo Gvd = new()
    {
        ThisStation = Home, TrainCount = 2,
        StartValidTimeTable = new DateOnly(2026, 12, 13), EndValidTimeTable = new DateOnly(2027, 12, 11),
        StartValidData = new DateOnly(2026, 12, 13), EndValidData = new DateOnly(2027, 12, 11),
        CreateData = new DateOnly(2026, 11, 1)
    };

    private string _root = null!;

    [TestInitialize]
    public void Init() => _root = Directory.CreateTempSubdirectory("gvdrepo").FullName;

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_root, true);

    private static InissWorkspace Workspace() => new()
    {
        Languages = [Sk, Gb],
        TrainsTypes = new ExControls.ExBindingList<TrainType>(TrainType.GetDefaultValues()),
        Sounds = [Dodatok1001, Vozen],
        Stations = [Home, Horna, Zapad]
    };

    private static GrafikonDocument Document(InissWorkspace workspace)
    {
        var document = GrafikonDocument.CreateNew([Sk]);
        var platform = new Platform("1", "Nástupište 1", "N1");
        var track = new Track("1", "1", "Koľaj 1", platform, "K1", "1");
        document.Tracks.Add(track);
        document.Platforms.Add(platform);
        var zssk = new Operator(1, "ZSSK");
        document.Operators.Add(zssk);
        document.CustomStations.Add(new Station("9001", "Vlastná", IsCustom: true));

        var types = workspace.TrainsTypes;
        var passing = new Train
        {
            ID = 1, Number = "601", Name = "", Type = types.First(t => t.Key == "R"), Variant = -1, Routing = Routing.Prechadzajuci,
            Arrival = new TimeOnly(10, 12), Departure = new TimeOnly(10, 15), Track = track, Operator = zssk,
            ZaciatokPlatnosti = Gvd.StartValidTimeTable, KoniecPlatnosti = Gvd.EndValidTimeTable, DateLimitText = "ide v 1-5",
            IsMiestenkovy = true, LockoutNumber = 3, LineArrival = "S1", LineDeparture = ""
        };
        passing.StaniceZoSmeru.Add(Horna with { IsInShortReport = true, IsInLongReport = true });
        passing.StaniceDoSmeru.AddRange([Zapad with { IsInLongReport = true }, new Station("9001", "Vlastná") { IsInShortReport = true }]);
        passing.Languages.AddRange([Sk, Gb]);
        passing.Doplnky.Add(Dodatok.NumsToDodatok(Dodatok1001, "1000000001", document.ReportTypesP, document.ReportVariants, Routing.Prechadzajuci));

        var starting = new Train
        {
            ID = 2, Number = "3601", Name = "", Type = types.First(t => t.Key == "Os"), Variant = -1, Routing = Routing.Vychadzajuci,
            Departure = new TimeOnly(5, 12), Track = track, Operator = Operator.None,
            ZaciatokPlatnosti = Gvd.StartValidTimeTable, KoniecPlatnosti = Gvd.EndValidTimeTable, DateLimitText = "ide denne",
            IsMotorovy = true
        };
        starting.StaniceDoSmeru.Add(Zapad with { IsInShortReport = true, IsInLongReport = true });

        document.Trains.Add(passing);
        document.Trains.Add(starting);
        document.Radenia.Add(new Radenie { CisloVlaku = "601", DatObm = "", Text = "Za rušňom je vozeň prvej triedy.", Sounds = [Vozen],
            DestStation = null!, ChosenReports = ReportType.Parse(document.ReportTypes, "P", document.ReportVariants) });
        return document;
    }

    private string Save(string name, InissWorkspace workspace, GrafikonDocument document)
    {
        var dir = Directory.CreateDirectory(Path.Combine(_root, name)).FullName;
        var context = new GrafikonContext(workspace, document, AppLanguage.Slovak);
        GrafikonRepository.CreateNew(dir, Gvd, new GrafikonContext(workspace, GrafikonDocument.CreateNew([Sk]), AppLanguage.Slovak),
            _root, StateDgmTemplate.Slovak);
        GrafikonRepository.Save(dir, Gvd, context);
        return dir;
    }

    [TestMethod]
    public void Grafikon_UlozenieANacitanie_ZachovaUdajeVlakov()
    {
        var workspace = Workspace();
        _warnings.Clear();
        var dir = Save("a", workspace, Document(workspace));

        var loaded = GrafikonRepository.Load(dir, InfoGvdFile.Read(dir), workspace, _warnings);

        Assert.IsEmpty(_warnings.Items, string.Join("; ", _warnings.Items));
        Assert.HasCount(2, loaded.Trains);
        var passing = loaded.Trains[0];
        Assert.AreEqual("601", passing.Number);
        Assert.AreEqual(new TimeOnly(10, 12), passing.Arrival);
        Assert.AreEqual(new TimeOnly(10, 15), passing.Departure);
        Assert.AreEqual(Gvd.StartValidTimeTable, passing.ZaciatokPlatnosti);
        Assert.AreEqual("ide v 1-5", passing.DateLimitText);
        CollectionAssert.AreEqual(new[] { "5100" }, passing.StaniceZoSmeru.Select(s => s.ID).ToList());
        CollectionAssert.AreEqual(new[] { "5200", "9001" }, passing.StaniceDoSmeru.Select(s => s.ID).ToList());
        Assert.IsTrue(passing.StaniceDoSmeru[1].IsInShortReport);
        Assert.IsFalse(passing.StaniceDoSmeru[0].IsInShortReport);
        Assert.AreEqual("1", passing.Track.Key);
        Assert.AreEqual("ZSSK", passing.Operator.Name);
        Assert.AreEqual(3, passing.LockoutNumber);
        Assert.IsTrue(passing.IsMiestenkovy);
        CollectionAssert.AreEquivalent(new[] { Sk, Gb }, passing.Languages);
        Assert.HasCount(1, passing.Doplnky);
        Assert.HasCount(1, passing.Radenia);
        Assert.IsNull(passing.Radenia[0].Validity);
        CollectionAssert.AreEqual(new[] { Vozen }, passing.Radenia[0].Sounds);

        var starting = loaded.Trains[1];
        Assert.IsNull(starting.Arrival);
        Assert.IsTrue(starting.IsMotorovy);
        Assert.AreSame(loaded.Operators[0], starting.Operator);
        Assert.AreEqual("Vlastná", loaded.CustomStations.Single().Name);
    }

    [TestMethod]
    public void Grafikon_NacitanyAZnovaUlozeny_DaTieIsteSubory()
    {
        var workspace = Workspace();
        var first = Save("a", workspace, Document(workspace));
        var loaded = GrafikonRepository.Load(first, InfoGvdFile.Read(first), workspace, _warnings);
        var second = Save("b", workspace, loaded);

        foreach (var file in Directory.GetFiles(first))
        {
            var name = Path.GetFileName(file);
            // hlavicka suboru obsahuje cestu a cas ulozenia
            string[] Content(string path) => File.ReadAllLines(path, Encodings.Win1250).Where(line => !line.StartsWith(';')).ToArray();
            CollectionAssert.AreEqual(Content(file), Content(Path.Combine(second, name)), name);
        }
    }
}
