using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using Iniss.Elis;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Import z ELIS bez mosta: prevod vlakov (<see cref="ElisBridgeClient.Convert" />), priprava kontroly
/// (<see cref="ElisImportService.BuildReview" />) a pouzitie volieb (<see cref="ElisImportService.Apply" />).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ElisImportTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly Station Home = new("100", "Stredná");

    private static readonly GVDInfo Gvd = new()
    {
        ThisStation = Home, StartValidTimeTable = new DateOnly(2025, 12, 14), EndValidTimeTable = new DateOnly(2026, 12, 12)
    };

    private static readonly StationDirectory Stations = new(
        [Home, new Station("1", "Horná"), new Station("2", "Horná Gr."), new Station("3", "Viedeň"), new Station("4", "Dolná"),
            new Station("5", "Prejazd"), new Station("6", "Cieľ"), new Station("7", "Krajská")], []);

    private static ElisStop Stop(int code, string name, int type = 0, int flags = 0) => new() { Code = code, Name = name, StationType = type, RouteFlags = flags };

    private static ElisTrain Train(string number, ElisStop[] before, ElisStop[] after, params string[] remarks) => new()
    {
        Type = "Os", Number = number, Name = "", OperatorName = "", OperatorNumber = "", DepartureMinutes = 8 * 60,
        RunsBits = new string('1', 364), StationsBefore = [.. before], StationsAfter = [.. after], Remarks = [.. remarks]
    };

    private static ElisResult Result(params ElisTrain[] trains) => new()
    {
        DataPath = "", StationName = "Stredná", ValidFrom = "2025-12-14", ValidTo = "2026-12-12", TotalDays = 364, Trains = [.. trains]
    };

    private static ElisBridgeClient Client() => new([Os], [Operator.None], Gvd, new Track { Key = "1", Name = "1" }, Stations);

    private static ElisImport Import(ElisBridgeClient client, ElisResult data, IReadOnlyList<Train>? source = null,
        Dictionary<string, bool>? savedShort = null, Dictionary<string, ElisRemarkAction>? savedRemarks = null) => new()
    {
        Client = client, Data = data, Unresolved = [], GVDPath = "", GVDInfo = Gvd, Mode = ElisImportMode.Add, GenerateTableTexts = true,
        SourceTrains = source ?? [], SavedShort = savedShort ?? [], SavedRemarks = savedRemarks ?? []
    };

    [TestMethod]
    public void Prevod_HranicneBodyAPrejazdySaNehlasiaVlakJeMedzistatny()
    {
        var data = Result(Train("1",
            [Stop(3, "Viedeň", ElisStop.TypeForeign), Stop(2, "Horná Gr.", ElisStop.TypeBorderPoint, ElisStop.FlagPassing), Stop(1, "Horná")],
            [Stop(4, "Dolná"), Stop(5, "Prejazd", flags: ElisStop.FlagPassing), Stop(6, "Cieľ")]));

        var train = Client().Convert(data).Trains.Single();

        CollectionAssert.AreEqual(new[] { true, false, true }, train.StaniceZoSmeru.Select(s => s.IsInLongReport).ToArray());
        CollectionAssert.AreEqual(new[] { true, false, true }, train.StaniceDoSmeru.Select(s => s.IsInLongReport).ToArray());
        Assert.IsTrue(train.IsMedzistatny);
        Assert.AreEqual("3", train.StartingStation!.ID);
        Assert.AreEqual("6", train.EndingStation!.ID);
        CollectionAssert.AreEqual(new[] { "3", "6" },
            train.StaniceZoSmeru.Concat(train.StaniceDoSmeru).Where(s => s.IsInShortReport).Select(s => s.ID).ToArray());
    }

    [TestMethod]
    public void Prevod_HranicnyBodPriradenyKStaniciSaZluciSNou()
    {
        // hranicny bod pred stanicou aj za nou - stanica sa hlasi, lebo sa v nej zastavuje
        var client = Client();
        client.StationMap["Dolná Gr."] = "4";
        var data = Result(Train("1", [],
            [Stop(0, "Dolná Gr.", ElisStop.TypeBorderPoint, ElisStop.FlagPassing), Stop(4, "Dolná"), Stop(0, "Dolná Gr.", ElisStop.TypeBorderPoint, ElisStop.FlagPassing),
                Stop(6, "Cieľ")]));

        var train = client.Convert(data).Trains.Single();

        CollectionAssert.AreEqual(new[] { "4", "6" }, train.StaniceDoSmeru.Select(s => s.ID).ToArray());
        Assert.IsTrue(train.StaniceDoSmeru[0].IsInLongReport);
        Assert.IsTrue(train.IsMedzistatny, "vlak ide cez hranicny bod");
    }

    [TestMethod]
    public void Kontrola_NavrhyZUlozenychVolieb_ZGrafikonu_PodlaDruhuStanice()
    {
        var data = Result(
            Train("1", [], [Stop(1, "Horná", ElisStop.TypeDistrictSeat), Stop(4, "Dolná", ElisStop.TypeJunction), Stop(7, "Krajská", ElisStop.TypeRegionalSeat),
                Stop(5, "Prejazd"), Stop(6, "Cieľ")], "reštauračný alebo bistro vozeň", "povinne miestenkový vlak"),
            Train("2", [], [Stop(4, "Dolná", ElisStop.TypeJunction), Stop(6, "Cieľ")], "reštauračný alebo bistro vozeň"));
        var client = Client();
        var conversion = client.Convert(data);

        // grafikon hlasi Prejazd (5) v kratkom hlaseni, Krajsku (7) nie; Dolna (4) ma ulozenu volbu
        var source = new Train { Number = "9", Type = Os, Routing = Routing.Vychadzajuci };
        source.StaniceDoSmeru.AddRange([new Station("5", "Prejazd", true, true), new Station("7", "Krajská", false, true), new Station("6", "Cieľ", true, true)]);
        var savedRemarks = new Dictionary<string, ElisRemarkAction> { ["reštauračný alebo bistro vozeň"] = new(ElisTrainFlag.None, "1003", 0) };
        var import = Import(client, data, [source], new Dictionary<string, bool> { ["4"] = false }, savedRemarks);

        var review = ElisImportService.BuildReview(import, conversion);

        var byId = review.Stations.ToDictionary(c => c.Station.ID);
        Assert.AreEqual(2, byId["4"].Trains);
        Assert.AreEqual((ElisShortSource.Saved, false), (byId["4"].Source, byId["4"].Short));
        Assert.AreEqual((ElisShortSource.Grafikon, true), (byId["5"].Source, byId["5"].Short));
        Assert.AreEqual((ElisShortSource.Grafikon, false), (byId["7"].Source, byId["7"].Short));
        Assert.AreEqual((ElisShortSource.None, false), (byId["1"].Source, byId["1"].Short), "okresne mesto sa nenavrhuje");
        Assert.AreEqual("4", review.Stations[0].Station.ID, "najviac vlakov najprv");

        var remarks = review.Remarks.ToDictionary(c => c.Text);
        Assert.IsTrue(remarks["reštauračný alebo bistro vozeň"].Saved);
        Assert.AreEqual(2, remarks["reštauračný alebo bistro vozeň"].Trains);
        Assert.AreEqual(ElisTrainFlag.Reservation, remarks["povinne miestenkový vlak"].Action.Flag);
        Assert.IsEmpty(review.Same);
    }

    [TestMethod]
    public void Kontrola_TypStaniceNavrhujeUzlyAKoncoveStanice()
    {
        var data = Result(Train("1", [], [Stop(1, "Horná", ElisStop.TypeRegionalSeat | ElisStop.TypeJunction), Stop(4, "Dolná", ElisStop.TypeJunction),
            Stop(7, "Krajská", ElisStop.TypeRegionalSeat), Stop(5, "Prejazd", ElisStop.TypeDistrictSeat), Stop(6, "Cieľ")]));
        var client = Client();

        var review = ElisImportService.BuildReview(Import(client, data), client.Convert(data));

        CollectionAssert.AreEquivalent(new[] { "1", "4" }, review.Stations.Where(c => c.Short).Select(c => c.Station.ID).ToArray());
        Assert.IsTrue(review.Stations.Where(c => c.Short).All(c => c.Source == ElisShortSource.ElisType));
    }

    [TestMethod]
    public void Kontrola_GrafikonBezMedzistanicVKratkomHlaseniNieJeVzor()
    {
        // napr. grafikon z predchadzajuceho importu - prevzatie by zopakovalo prazdne stlpce Smer a Zo smeru
        var data = Result(Train("1", [], [Stop(1, "Horná", ElisStop.TypeRegionalSeat | ElisStop.TypeJunction), Stop(6, "Cieľ")]));
        var client = Client();
        var conversion = client.Convert(data);
        var same = new Train { Number = "1", Name = "", Type = Os, Routing = Routing.Vychadzajuci, DateLimitText = "" };
        same.StaniceDoSmeru.AddRange([new Station("1", "Horná", false, true), new Station("6", "Cieľ", true, true)]);

        var review = ElisImportService.BuildReview(Import(client, data, [same]), conversion);

        Assert.IsEmpty(review.Same);
        Assert.AreEqual((ElisShortSource.ElisType, true), (review.Stations.Single().Source, review.Stations.Single().Short));
    }

    [TestMethod]
    public void Pouzitie_KratkeHlasenieDodatkyARadenia()
    {
        var data = Result(Train("1", [], [Stop(1, "Horná"), Stop(4, "Dolná"), Stop(6, "Cieľ")], "reštauračný alebo bistro vozeň"));
        var client = Client();
        var conversion = client.Convert(data);
        var import = Import(client, data);
        var review = ElisImportService.BuildReview(import, conversion);
        review.Stations.Single(c => c.Station.ID == "4").Short = true;
        review.Remarks.Single().Action = new ElisRemarkAction(ElisTrainFlag.None, "1003", 0);

        var odchod = new ReportType("O", "Odchod", "O", baseTrain: true, passThrough: false, terminateTrain: false);
        var prichod = new ReportType("P", "Príchod", "P", baseTrain: false, passThrough: true, terminateTrain: true);
        var variant = new ReportVariant(1, "Dlhé hlásenie");
        var radenie = new Radenie { CisloVlaku = "1", Text = "radenie" };
        var document = new GrafikonDocument { ReportTypes = [odchod, prichod], ReportVariants = [variant], Radenia = [radenie] };
        var group = new FyzGroup(new FyzLanguage("SK", "Slovensky") { IsBasic = true }, "DODATKY", "Dodatky", "D\\");
        var sounds = new[] { new FyzSound(group, "D1003", "D1003", "D1003.WAV", "", "jedálny vozeň", 0) };

        ElisImportService.Apply(import, conversion, review, document, sounds);

        var train = conversion.Trains.Single();
        CollectionAssert.AreEqual(new[] { "4", "6" }, train.StaniceDoSmeru.Where(s => s.IsInShortReport).Select(s => s.ID).ToArray());
        var dodatok = train.Doplnky.Single();
        Assert.AreEqual("1003", dodatok.Name);
        Assert.AreSame(sounds[0], dodatok.Sound);
        Assert.AreEqual(odchod, dodatok.ChosenReports.Single().Type, "vychadzajuci vlak - len typy pre vychadzajuce vlaky");
        CollectionAssert.AreEqual(new[] { variant }, dodatok.ChosenReports.Single().Variants);
        Assert.AreSame(radenie, train.Radenia.Single());
    }

    [TestMethod]
    public void Pouzitie_RovnakyVlakSiPonechaKratkeHlasenie()
    {
        var data = Result(Train("1", [], [Stop(1, "Horná", ElisStop.TypeRegionalSeat), Stop(6, "Cieľ")]));
        var client = Client();
        var conversion = client.Convert(data);
        var same = new Train { Number = "1", Name = "", Type = Os, Routing = Routing.Vychadzajuci, DateLimitText = "", ZaciatokPlatnosti = Gvd.StartValidTimeTable,
            KoniecPlatnosti = Gvd.EndValidTimeTable };
        same.StaniceDoSmeru.AddRange([new Station("1", "Horná", false, true), new Station("6", "Cieľ", true, true)]);
        var other = new Train { Number = "2", Name = "", Type = Os, Routing = Routing.Vychadzajuci, DateLimitText = "" };
        other.StaniceDoSmeru.AddRange([new Station("4", "Dolná", true, true), new Station("6", "Cieľ", true, true), new Station("7", "Krajská", true, true)]);
        var import = Import(client, data, [same, other]);
        var review = ElisImportService.BuildReview(import, conversion);

        Assert.AreSame(same, review.Same[conversion.Trains[0]]);
        review.Stations.Single().Short = true;

        review.KeepTrainShort = true;
        ElisImportService.Apply(import, conversion, review, new GrafikonDocument(), []);
        Assert.IsFalse(conversion.Trains[0].StaniceDoSmeru[0].IsInShortReport);

        review.KeepTrainShort = false;
        ElisImportService.Apply(import, conversion, review, new GrafikonDocument(), []);
        Assert.IsTrue(conversion.Trains[0].StaniceDoSmeru[0].IsInShortReport);
    }
}
