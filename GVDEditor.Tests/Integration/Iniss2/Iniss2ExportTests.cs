using System.Collections;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration.Iniss2;
using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss2;
using ToolsCore.Iniss2.Grafikon;
using ToolsCore.Iniss2.Tables;

namespace GVDEditor.Tests.Integration.Iniss2;

/// <summary>
/// Prevod grafikonu INISS 3.39 do suborov INISS2 na fiktivnej stanici.
/// </summary>
[TestClass]
public class Iniss2ExportTests
{
    private const int Lipovec = 5400010;
    private const int Horany = 5400020;
    private const int Javor = 5400030;

    private static readonly Station Home = new("5400010", "Lipovec");
    private static readonly Station Hills = new("5400020", "Horany");
    private static readonly Station Oak = new("5400030", "Javorina");

    private static readonly DateOnly From = new(2030, 1, 7);
    private static readonly DateOnly To = new(2030, 1, 20);
    private static readonly GVDInfo Gvd = new() { ThisStation = Home, StartValidTimeTable = From, EndValidTimeTable = To };

    private static readonly List<TrainType> Types = TrainType.GetDefaultValues();
    private static readonly Platform First = new("I", "Nástupište 1", "N1");
    private static readonly Track One = new("1", "1", "Koľaj 1", First, "K1", "1") { PlatformTrackText = "I/1", AltTrackText = "1A" };
    private static readonly Track Two = new("2", "2", "Koľaj 2", First, "K2", "2");
    private static readonly Operator Rail = new(1, "Fiktívne dráhy");

    private static readonly FyzLanguage Sk = new("SK", "Slovenčina", "SK\\") { IsBasic = true };
    private static readonly FyzLanguage Gb = new("GB", "Angličtina", "GB\\");
    private static readonly FyzLanguage De = new("D", "Nemčina", "D\\");
    private static readonly List<FyzLanguage> Languages = [Sk, Gb, De];
    private static readonly FyzGroup Additions = new(Sk, "DODATKY", "Dodatkové hlásenia", "DODATKY\\");
    private static readonly FyzGroup Cars = new(Sk, "VOZY1", "Vozne", "VOZY1\\");
    private static readonly FyzSound Buffet = new(Additions, "D1006", "D1006", "D1006.WAV", "", "Vlak má bufet", 0);
    private static readonly FyzSound Car = new(Cars, "vlak", "vlak", "vlak.WAV", "", "vlak", 0);

    private string _dataDir = null!;

    [TestInitialize]
    public void Init() => _dataDir = Directory.CreateTempSubdirectory("gvdiniss2").FullName;

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dataDir, true);

    private static Train Train(string number, Routing routing, string type = "Os", int variant = -1, string limit = "", Track? track = null)
    {
        var train = new Train
        {
            ID = 1, Number = number, Name = "", Type = Types.First(t => t.Key == type), Variant = variant, Routing = routing, Track = track ?? One,
            Operator = Rail, DateLimitText = limit, ZaciatokPlatnosti = From, KoniecPlatnosti = To
        };

        if (routing != Routing.Vychadzajuci)
        {
            train.Arrival = new TimeOnly(8, 15);
            train.StaniceZoSmeru.Add(Hills);
        }

        if (routing != Routing.Konciaci)
        {
            train.Departure = new TimeOnly(8, 17);
            train.StaniceDoSmeru.Add(Oak);
        }

        return train;
    }

    private static Iniss2ExportData Build(params Train[] trains) => Build(Document(trains));

    private static Iniss2ExportData Build(GrafikonDocument document) => Iniss2Export.Build(document, Gvd, Source());

    private static Iniss2ExportSource Source(IReadOnlyList<TrainNotice>? lockouts = null, IReadOnlyList<TrainNotice>? diversions = null) =>
        new(Languages, [UserLockout, UserDiversion], lockouts ?? [], diversions ?? []);

    // texty obsluhy z LogZvuk.usr: predloha so stanicami
    private static readonly LogZvukText UserLockout = new("V1000004", "Výluka Lipovec - Horany", "", LogZvukText.TypeLockoutDynamic, "Výluka ve stanici",
        ["5400010", "5400020"]);

    private static readonly Station Dub = new("5400040", "Dubová");
    private const int Dubova = 5400040;
    private static readonly GVDInfo GvdOak = new() { ThisStation = Oak, StartValidTimeTable = From, EndValidTimeTable = To };

    // vlak v susednej stanici Javorina: prichadza z Horan cez Lipovec, pokracuje do Dubovej
    private static Train Next(string number, string limit = "", TimeOnly? arrival = null, TimeOnly? departure = null)
    {
        var train = Train(number, Routing.Prechadzajuci, limit: limit);
        train.StaniceZoSmeru.Clear();
        train.StaniceZoSmeru.AddRange([Hills, Home]);
        train.StaniceDoSmeru.Clear();
        train.StaniceDoSmeru.Add(Dub);
        train.Arrival = arrival ?? new TimeOnly(8, 40);
        train.Departure = departure ?? new TimeOnly(8, 42);
        return train;
    }

    private static Iniss2ExportData BuildPair(Train[] home, Train[] next, GVDInfo? nextGvd = null) =>
        Iniss2Export.Build([new Iniss2StationInput(Document(home), Gvd), new Iniss2StationInput(Document(next), nextGvd ?? GvdOak)], Source());

    private static readonly LogZvukText UserDiversion = new("O3001", "Odklon cez Javorinu", "", LogZvukText.TypeDiversion, "Odklon", ["5400030"]);

    private static GrafikonDocument Document(params Train[] trains)
    {
        var document = GrafikonDocument.CreateNew([Sk]);
        document.Trains = new TrainBindingList(trains.ToList());
        document.Tracks = [Track.None, One, Two];
        return document;
    }

    // dodatok s vybranymi typmi hlasenia: velke pismeno dlhe, male kratke hlasenie
    private static Dodatok Addition(Routing routing, params string[] letters)
    {
        var addition = new Dodatok { Sound = Buffet, Name = "1006" };
        var types = ReportType.GetDefaultValuesSk();
        var applicable = types.Where(type => routing == Routing.Vychadzajuci ? type.BaseTrain : routing == Routing.Prechadzajuci ? type.PassThrough : type.TerminateTrain);
        foreach (var type in applicable)
        {
            addition.ChosenReports.Add(new ChosenReportType
            {
                Type = type,
                Variants = letters.Where(letter => letter.Equals(type.Char, StringComparison.OrdinalIgnoreCase))
                    .Select(letter => letter == type.Char ? ReportVariant.DlheHlasenie : ReportVariant.KratkeHlasenie).ToList()
            });
        }

        return addition;
    }

    private static List<ChosenReportType> Chosen(params string[] letters) =>
        ReportType.GetDefaultValuesSk()
            .Where(type => letters.Any(letter => letter.Equals(type.Char, StringComparison.OrdinalIgnoreCase)))
            .Select(type => new ChosenReportType
            {
                Type = type,
                Variants = letters.Where(letter => letter.Equals(type.Char, StringComparison.OrdinalIgnoreCase))
                    .Select(letter => letter == type.Char ? ReportVariant.DlheHlasenie : ReportVariant.KratkeHlasenie).ToList()
            })
            .ToList();

    private static string Bits(BitArray days) => string.Concat(days.Cast<bool>().Select(day => day ? '1' : '0'));

    private static BitArray Days(string bits) => new(bits.Select(bit => bit == '1').ToArray());

    [TestMethod]
    public void Trasa_PrechadzajuciVlakMaCasyLenVStanici()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Name = "Javor";

        var stops = Build(train).Timetable.Trains.Single().Variants.Single().Stops;

        CollectionAssert.AreEqual(new[] { Horany, Lipovec, Javor }, stops.Select(s => s.Station).ToArray());
        Assert.AreEqual("1001", stops[0].Number, "cislo a druh su v prvom bode");
        Assert.AreEqual("Os", stops[0].TrainType);
        Assert.AreEqual("Javor", stops[0].TrainName);
        Assert.IsFalse(stops[0].HasTime);
        Assert.AreEqual(8 * 60 + 15, stops[1].Arrival);
        Assert.AreEqual(8 * 60 + 17, stops[1].Departure);
        Assert.AreEqual(GvStopFlags.Interest, stops[1].Flags);
        Assert.AreEqual("", stops[1].Number, "cislo sa v stanici nemeni");
    }

    [TestMethod]
    public void Trasa_VychadzajuciVlakZacinaVStanici()
    {
        var stops = Build(Train("1001", Routing.Vychadzajuci)).Timetable.Trains.Single().Variants.Single().Stops;

        Assert.AreEqual(Lipovec, stops[0].Station);
        Assert.AreEqual("1001", stops[0].Number);
        Assert.IsNull(stops[0].Arrival);
        Assert.AreEqual(Javor, stops[1].Station);
    }

    [TestMethod]
    public void Trasa_KonciaciVlakKonciVStanici()
    {
        var stops = Build(Train("1001", Routing.Konciaci)).Timetable.Trains.Single().Variants.Single().Stops;

        Assert.AreEqual(Lipovec, stops[^1].Station);
        Assert.IsNull(stops[^1].Departure);
        Assert.AreEqual("1001", stops[0].Number);
    }

    [TestMethod]
    public void Trasa_BezStanicPouzijeVychodziuAKonecnuStanicu()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.StaniceZoSmeru.Clear();
        train.StaniceDoSmeru.Clear();
        train.StartingStation = Hills;
        train.EndingStation = Oak;

        var stops = Build(train).Timetable.Trains.Single().Variants.Single().Stops;

        CollectionAssert.AreEqual(new[] { Horany, Lipovec, Javor }, stops.Select(s => s.Station).ToArray());
    }

    [TestMethod]
    public void Trasa_BezStaniceGrafikonuUpozorni()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.StaniceZoSmeru.Clear();
        train.StaniceZoSmeru.Add(Oak);

        var data = Build(train);

        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.RouteWithoutStation, "1001"));
        Assert.AreEqual(Lipovec, data.Timetable.Trains.Single().Variants.Single().Stops.Single().Station);
    }

    [TestMethod]
    public void Trasa_StanicaBezCislaVypadneSUpozornenim()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.StaniceDoSmeru.Add(new Station("X1", "Vlastná"));

        var data = Build(train);

        Assert.HasCount(3, data.Timetable.Trains.Single().Variants.Single().Stops);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.StationWithoutNumber, "Vlastná"));
    }

    [TestMethod]
    public void Stanica_BezCislaZastaviExport()
    {
        var gvd = new GVDInfo { ThisStation = new Station("X1", "Vlastná") };

        var error = Assert.Throws<Iniss2ExportException>(() => Iniss2Export.Build(GrafikonDocument.CreateNew([Sk]), gvd, Source()));

        Assert.AreEqual(Iniss2ExportIssue.StationWithoutNumber, error.Issue);
    }

    [TestMethod]
    public void Cas_OdchodPoPolnociJeNasledujuciDen()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Arrival = new TimeOnly(23, 58);
        train.Departure = new TimeOnly(0, 2);

        var stop = Build(train).Timetable.Trains.Single().Variants.Single().Stops[1];

        Assert.AreEqual(0, stop.ArrivalDay);
        Assert.AreEqual(1, stop.DepartureDay);
    }

    [TestMethod]
    public void Linka_InaPriOdchodeJeVStanici()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.LineArrival = "S1";
        train.LineDeparture = "S2";

        var stops = Build(train).Timetable.Trains.Single().Variants.Single().Stops;

        Assert.AreEqual("S1", stops[0].Line);
        Assert.AreEqual("S2", stops[1].Line);
        Assert.AreEqual("1001", stops[1].Number, "linka plati len v bode s cislom vlaku");
    }

    [TestMethod]
    public void Linka_RovnakaLenVPrvomBode()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.LineArrival = "S1";
        train.LineDeparture = "S1";

        var stops = Build(train).Timetable.Trains.Single().Variants.Single().Stops;

        Assert.AreEqual("S1", stops[0].Line);
        Assert.AreEqual("", stops[1].Number);
    }

    [TestMethod]
    public void Priznaky_PrenasajuSaNaVariantuAPrestupNaBod()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.IsMedzistatny = true;
        train.IsNizkopodlazny = true;
        train.IsMimoriadny = true;
        train.IsPrestupovy = true;

        var variant = Build(train).Timetable.Trains.Single().Variants.Single();

        Assert.AreEqual(GvVariantFlags.M | GvVariantFlags.N | GvVariantFlags.X, variant.Flags);
        Assert.AreEqual(GvStopFlags.Interest | GvStopFlags.Pr, variant.Stops[1].Flags);
        Assert.AreEqual("Fiktívne dráhy", variant.Operator);
    }

    [TestMethod]
    public void Priznaky_DialkovyVlakUpozorni()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.IsDialkovy = true;

        CollectionAssert.Contains(Build(train).Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.LongDistanceFlag, "1001"));
    }

    [TestMethod]
    public void Varianty_RovnakyVlakJeJedenVlakVPoradiVariant()
    {
        var second = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");
        var first = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");

        var train = Build(second, first).Timetable.Trains.Single();

        Assert.HasCount(2, train.Variants);
        Assert.AreEqual("11111001111100", Bits(train.DaysOf(train.Variants[0])));
        Assert.AreEqual("00000110000011", Bits(train.DaysOf(train.Variants[1])));
    }

    [TestMethod]
    public void Varianty_PrekrytieMaVSpolocneDniKratsiuVariantu()
    {
        var daily = Train("1001", Routing.Vychadzajuci, variant: 1);
        var saturday = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6", track: Two);

        var data = Build(daily, saturday);
        var train = data.Timetable.Trains.Single();

        Assert.AreEqual("11111011111101", Bits(train.DaysOf(train.Variants[0])));
        Assert.AreEqual("00000100000010", Bits(train.DaysOf(train.Variants[1])));
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.VariantOverlap, "1001"));
    }

    [TestMethod]
    public void Varianty_RovnakeDniOstanuPrvej()
    {
        var train = Build(Train("1001", Routing.Prechadzajuci, variant: 1), Train("1001", Routing.Prechadzajuci, variant: 2)).Timetable.Trains.Single();

        Assert.AreEqual("", train.Variants[0].Validity);
        Assert.AreEqual("-", train.Variants[1].Validity);
    }

    [TestMethod]
    public void Poznamka_JeCeskyAMaRovnakeDni()
    {
        var variant = Build(Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5")).Timetable.Trains.Single().Variants.Single();

        StringAssert.StartsWith(variant.Validity, "jede");
    }

    [TestMethod]
    public void Poznamka_DenneJePrazdna()
    {
        var variant = Build(Train("1001", Routing.Prechadzajuci, limit: "ide denne")).Timetable.Trains.Single().Variants.Single();

        Assert.AreEqual("", variant.Validity);
    }

    [TestMethod]
    public void Poznamka_ChybnaZnamenaZiadnyDenSUpozornenim()
    {
        var data = Build(Train("1001", Routing.Prechadzajuci, limit: "toto nie je poznamka"));

        Assert.AreEqual("-", data.Timetable.Trains.Single().Variants.Single().Validity);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.BadValidity, "1001"));
    }

    [TestMethod]
    public void Poznamka_DlheObdobieUpozorni()
    {
        var train = Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5");
        train.KoniecPlatnosti = From.AddDays(500);

        CollectionAssert.Contains(Build(train).Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.PeriodTooLong, "1001"));
    }

    [TestMethod]
    public void Kolaje_PrenesuTextyAZvukyBezNeznamejKolaje()
    {
        var tracks = Build().Stations[0].Tracks;

        Assert.HasCount(2, tracks);
        Assert.AreEqual(new TrackDefinition
        {
            Key = "1", Name = "1", TrackText = "1", PlatformText = "I", PlatformTrackText = "I/1", AltTrackText = "1A", TrackSound = "K1", PlatformSound = "N1"
        }, tracks[0]);
    }

    [TestMethod]
    public void Kolaje_VlakJedinejKolajeBezPoznamky()
    {
        var train = Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5");
        train.TrackDeparture = Two;

        Assert.AreEqual(new TrainPosition("1001", "1", "2", ""), Build(train).Stations[0].Positions.Single());
    }

    [TestMethod]
    public void Kolaje_VychadzajuciAjKonciaciMajuKolajPrichoduAjOdchodu()
    {
        // Pozice.txt INISSu 3.39 dava jednu kolaj prichodu aj odchodu kazdemu vlaku - vyrazy KOLEJPRIJ/KOLEJODJ ju citaju
        var positions = Build(Train("1001", Routing.Vychadzajuci), Train("1002", Routing.Konciaci)).Stations[0].Positions;

        Assert.AreEqual(new TrainPosition("1001", "1", "1", ""), positions[0]);
        Assert.AreEqual(new TrainPosition("1002", "1", "1", ""), positions[1]);
    }

    [TestMethod]
    public void Kolaje_RozneVDniMajuPoznamku()
    {
        var first = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        var second = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7", track: Two);

        var positions = Build(first, second).Stations[0].Positions;

        Assert.HasCount(2, positions);
        Assert.AreEqual("11111001111100", Bits(GvValidity.Days(From, To, positions[0].Validity)));
        Assert.AreEqual("00000110000011", Bits(GvValidity.Days(From, To, positions[1].Validity)));
    }

    [TestMethod]
    public void Kolaje_DvaVlakyJednehoCislaSpojiPrichodSOdchodomVychadzajuceho()
    {
        var starting = Train("1001", Routing.Vychadzajuci, track: Two);
        var through = Train("1001", Routing.Prechadzajuci);
        through.Name = "Javor";

        var data = Build(starting, through);

        Assert.HasCount(2, data.Timetable.Trains);
        Assert.AreEqual(new TrainPosition("1001", "1", "2", ""), data.Stations[0].Positions.Single());
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.SharedNumberTracks, "1001"));
    }

    [TestMethod]
    public void Kolaje_DvaVlakyJednehoCislaSpojiOdchodSPrichodomKonciaceho()
    {
        var ending = Train("1001", Routing.Konciaci, track: Two);
        var through = Train("1001", Routing.Prechadzajuci);
        through.Name = "Javor";

        Assert.AreEqual(new TrainPosition("1001", "2", "1", ""), Build(through, ending).Stations[0].Positions.Single());
    }

    [TestMethod]
    public void Kolaje_VlakyJednehoCislaLenCastDniMajuDniOddelene()
    {
        var starting = Train("1001", Routing.Vychadzajuci, track: Two);
        var through = Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5");
        through.Name = "Javor";

        var positions = Build(starting, through).Stations[0].Positions;

        Assert.HasCount(2, positions);
        Assert.AreEqual(new TrainPosition("1001", "1", "2", GvValidity.Text(From, To, Days("11111001111100"))), positions[0]);
        Assert.AreEqual("00000110000011", Bits(GvValidity.Days(From, To, positions[1].Validity)));
        Assert.AreEqual("2", positions[1].ArrivalTrack, "vychadzajuci vlak ma kolaj prichodu z kolaje vlaku");
        Assert.AreEqual("2", positions[1].DepartureTrack);
    }

    [TestMethod]
    public void Kolaje_RovnakaKolajVlakovJednehoCislaNeupozorni()
    {
        var starting = Train("1001", Routing.Vychadzajuci);
        var through = Train("1001", Routing.Prechadzajuci);
        through.Name = "Javor";

        Assert.IsEmpty(Build(starting, through).Warnings);
    }

    [TestMethod]
    public void Kolaje_NeznamaKolajNemaZaznam()
    {
        Assert.IsEmpty(Build(Train("1001", Routing.Prechadzajuci, track: Track.None)).Stations[0].Positions);
    }

    [TestMethod]
    public void Motorovy_VlakVsetkyDniBezPoznamky()
    {
        var train = Train("1001", Routing.Prechadzajuci, type: "R");
        train.IsMotorovy = true;

        var data = Build(train);

        Assert.AreEqual(new MosTrain("1001", ""), data.Stations[0].MotorTrains.Single());
        Assert.AreEqual("Os", data.Timetable.Trains.Single().Variants.Single().Stops[0].TrainType, "server meni na MOs len osobne vlaky");
    }

    [TestMethod]
    public void Motorovy_DruhMOsSaPrenesieCezMos()
    {
        var data = Build(Train("1001", Routing.Prechadzajuci, type: "MOs"));

        Assert.AreEqual(new MosTrain("1001", ""), data.Stations[0].MotorTrains.Single());
        Assert.AreEqual("Os", data.Timetable.Trains.Single().Variants.Single().Stops[0].TrainType);
    }

    [TestMethod]
    public void Motorovy_CastDniMaPoznamku()
    {
        var first = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        var second = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");
        second.IsMotorovy = true;

        Assert.AreEqual("00000110000011", Bits(GvValidity.Days(From, To, Build(first, second).Stations[0].MotorTrains.Single().Validity)));
    }

    [TestMethod]
    public void Stanice_ObsahujuTrasuBezOpakovania()
    {
        var data = Build(Train("1001", Routing.Prechadzajuci), Train("1002", Routing.Prechadzajuci));

        CollectionAssert.AreEquivalent(new[] { Lipovec, Horany, Javor }, data.RouteStations.Select(s => s.Number).ToArray());
    }

    [TestMethod]
    public void Zapis_ServerovyCitacDostaneRovnakeVlakyAKolaje()
    {
        var first = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        var second = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7", track: Two);
        second.IsMotorovy = true;

        Iniss2Export.Write(Build(first, second), _dataDir, "2030", "01 Lipovec");

        var group = DirListXml.Read(_dataDir).Single();
        var item = group.Items.Single();
        Assert.AreEqual(Lipovec, item.Station);

        var timetable = StationTimetable.Read(_dataDir, group, item);
        Assert.HasCount(2, timetable.Calls);
        var train = timetable.Calls[0].Train;
        Assert.AreEqual("Os", train.Variants[0].Stops[0].TrainType);
        Assert.AreEqual("MOs", train.Variants[1].Stops[0].TrainType);

        var monday = timetable.Calls[0];
        var saturday = timetable.Calls[1];
        Assert.AreEqual("1", timetable.PositionFor(monday, From)!.ArrivalTrack);
        Assert.AreEqual("2", timetable.PositionFor(saturday, From.AddDays(5))!.ArrivalTrack);
        Assert.HasCount(2, timetable.Tracks);
        Assert.HasCount(3, GvStationsFile.Read(Path.Combine(_dataDir, "2030")));
    }

    [TestMethod]
    public void Zapis_DodatkyRadenieJazykyAHlasenia()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Doplnky.Add(Addition(Routing.Prechadzajuci, "P", "p"));
        train.Languages.Add(Gb);
        var document = Document(train);
        document.Radenia.Add(new Radenie { CisloVlaku = "1001", DatObm = "", Text = "vlak", Sounds = [Car], ChosenReports = Chosen("P") });

        Iniss2Export.Write(Build(document), _dataDir, "2030", "01 Lipovec");

        var dir = Path.Combine(_dataDir, "2030", "01 Lipovec");
        Assert.AreEqual(new TrainAddition("1001", "D1006", "Pp", ""), DodatkyFile.Read(dir).Single());
        CollectionAssert.AreEqual(new[] { "SK/VOZY1/vlak" }, RazeniFile.Read(dir).Single().Sounds.ToArray());
        CollectionAssert.AreEqual(new[] { "GB" }, ToolsCore.Iniss2.Grafikon.ForeignFile.Read(dir).Single().Languages.ToArray());
        Assert.HasCount(5, ReportKeysFile.Read(dir));
    }

    [TestMethod]
    public void Dodatky_KlucZoZvukuAPismenaTypov()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Doplnky.Add(Addition(Routing.Prechadzajuci, "P", "p", "O"));

        Assert.AreEqual(new TrainAddition("1001", "D1006", "PpO", ""), Build(train).Stations[0].Additions.Single());
    }

    [TestMethod]
    public void Dodatky_VsetkyTypyVlakuSuPrazdneZiadnyJePomlcka()
    {
        var all = Train("1001", Routing.Konciaci);
        all.Doplnky.Add(Addition(Routing.Konciaci, "P", "p", "I", "i", "L", "l"));
        var none = Train("1002", Routing.Prechadzajuci);
        none.Doplnky.Add(Addition(Routing.Prechadzajuci));

        var additions = Build(all, none).Stations[0].Additions;

        Assert.AreEqual("", additions[0].Reports, "konciaci vlak nepozna typy Pobytove a Odjede");
        Assert.AreEqual("-", additions[1].Reports);
    }

    [TestMethod]
    public void Dodatky_PlatiaLenVDniVlakuSDodatkom()
    {
        var weekday = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        weekday.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));
        var weekend = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");

        var addition = Build(weekday, weekend).Stations[0].Additions.Single();

        Assert.AreEqual("11111001111100", Bits(GvValidity.Days(From, To, addition.Validity)));
    }

    [TestMethod]
    public void Dodatky_RovnakyDodatokVlakovJednehoCislaSaSpoji()
    {
        var weekday = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        weekday.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));
        var weekend = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");
        weekend.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));

        Assert.AreEqual(new TrainAddition("1001", "D1006", "P", ""), Build(weekday, weekend).Stations[0].Additions.Single());
    }

    [TestMethod]
    public void Radenie_ZvukyCielAPismena()
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.Radenia.Add(new Radenie { CisloVlaku = "1001", DestStation = Oak, DatObm = "", Text = "vlak", Sounds = [Car], ChosenReports = Chosen("P", "p", "L") });

        var composition = Build(document).Stations[0].Compositions.Single();

        Assert.AreEqual(("1001", Javor, "PpL", ""), (composition.Number, composition.EndStation, composition.Reports, composition.Validity));
        CollectionAssert.AreEqual(new[] { "SK/VOZY1/vlak" }, composition.Sounds.ToArray());
    }

    [TestMethod]
    public void Radenie_VsetkyTypySuPrazdne()
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.Radenia.Add(new Radenie { CisloVlaku = "1001", DatObm = "", Text = "", ChosenReports = Chosen("P", "p", "I", "i", "L", "l", "N", "n", "O", "o") });

        Assert.AreEqual("", Build(document).Stations[0].Compositions.Single().Reports);
    }

    [TestMethod]
    public void Radenie_ObdobieSaPrevedieNaPoznamku()
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.Radenia.Add(new Radenie
        {
            CisloVlaku = "1001", Validity = new ValidityPeriod(From, From.AddDays(6)), DatObm = "ide v 6,7", Text = "", ChosenReports = Chosen("P")
        });

        var composition = Build(document).Stations[0].Compositions.Single();

        Assert.AreEqual("00000110000000", Bits(GvValidity.Days(From, To, composition.Validity)));
    }

    [TestMethod]
    public void Radenie_BezVlakuVypadneSUpozornenim()
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.Radenia.Add(new Radenie { CisloVlaku = "9999", DatObm = "", Text = "", ChosenReports = Chosen("P") });

        var data = Build(document);

        Assert.IsEmpty(data.Stations[0].Compositions);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.CompositionWithoutTrain, "9999"));
    }

    [TestMethod]
    public void Radenie_MimoDniVlakuVypadne()
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.Radenia.Add(new Radenie
        {
            CisloVlaku = "1001", Validity = new ValidityPeriod(From.AddYears(-1), To.AddYears(-1)), DatObm = "", Text = "", ChosenReports = Chosen("P")
        });

        var data = Build(document);

        Assert.IsEmpty(data.Stations[0].Compositions);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.CompositionOutsidePeriod, "1001"));
    }

    [TestMethod]
    public void Jazyky_VsetkyJazykyJeJednotka()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Languages.AddRange(Languages);

        CollectionAssert.AreEqual(new[] { "1" }, Build(train).Stations[0].Languages.Single().Languages.ToArray());
    }

    [TestMethod]
    public void Jazyky_LenDalsieJazykyBezZakladneho()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Languages.AddRange([Sk, De]);

        CollectionAssert.AreEqual(new[] { "D" }, Build(train).Stations[0].Languages.Single().Languages.ToArray());
    }

    [TestMethod]
    public void Jazyky_BezDalsichJazykovBezZaznamu()
    {
        Assert.IsEmpty(Build(Train("1001", Routing.Prechadzajuci)).Stations[0].Languages);
    }

    [TestMethod]
    public void Jazyky_ZaznamyVlakovJednehoCislaMajuOddeleneDni()
    {
        var weekday = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        weekday.Languages.Add(Gb);
        var weekend = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");
        weekend.Languages.Add(De);

        var languages = Build(weekday, weekend).Stations[0].Languages;

        Assert.HasCount(2, languages);
        Assert.AreEqual("11111001111100", Bits(GvValidity.Days(From, To, languages[0].Validity)));
        Assert.AreEqual("00000110000011", Bits(GvValidity.Days(From, To, languages[1].Validity)));
    }

    // vlak s vyberom 3.39: zo smeru Horany v oboch hlaseniach, do smeru Javorina len v dlhom, Dubova v oboch
    private static Train Announced(string number, int variant = -1, string limit = "", bool javorShort = false)
    {
        var train = Train(number, Routing.Prechadzajuci, variant: variant, limit: limit);
        train.StaniceZoSmeru.Clear();
        train.StaniceZoSmeru.Add(new Station(Hills.ID, Hills.Name, IsInShortReport: true, IsInLongReport: true));
        train.StaniceDoSmeru.Clear();
        train.StaniceDoSmeru.AddRange([new Station(Oak.ID, Oak.Name, IsInShortReport: javorShort, IsInLongReport: true),
            new Station(Dub.ID, Dub.Name, IsInShortReport: true, IsInLongReport: true)]);
        return train;
    }

    [TestMethod]
    public void StaniceHlasenia_VyberPriVlaku()
    {
        var data = Build(Announced("1001"));

        var record = data.Stations[0].ReportStations.Single();
        Assert.AreEqual(("1001", ""), (record.Number, record.Validity));
        CollectionAssert.AreEqual(new[] { Horany }, record.ShortFrom.ToArray());
        CollectionAssert.AreEqual(new[] { Horany }, record.LongFrom.ToArray());
        CollectionAssert.AreEqual(new[] { Dubova }, record.ShortTo.ToArray());
        CollectionAssert.AreEqual(new[] { Javor, Dubova }, record.LongTo.ToArray());
        Assert.IsEmpty(data.Stations[0].ShortReportPatterns);
        Assert.IsEmpty(data.Stations[0].LongReportPatterns);
        CollectionAssert.AreEqual(new[] { new DefaultStation(Javor, "Javorina") { Short = false }, new DefaultStation(Dubova, "Dubová") },
            data.DefaultStations.ToArray());
    }

    [TestMethod]
    public void StaniceHlasenia_VlakyJednehoCislaMajuOddeleneDni()
    {
        var records = Build(Announced("1001", 1, "ide v 1-5"), Announced("1001", 2, "ide v 6,7", javorShort: true)).Stations[0].ReportStations;

        Assert.HasCount(2, records);
        Assert.AreEqual("11111001111100", Bits(GvValidity.Days(From, To, records[0].Validity)));
        CollectionAssert.AreEqual(new[] { Javor, Dubova }, records[1].ShortTo.ToArray());
    }

    [TestMethod]
    public void StaniceHlasenia_PrazdnyVyberJeVzorBezStanic()
    {
        var data = Build(Train("1002", Routing.Vychadzajuci));

        Assert.IsEmpty(data.Stations[0].ReportStations);
        CollectionAssert.AreEqual(new[] { new ReportPatternStop(Lipovec, false) }, data.Stations[0].ShortReportPatterns[0].ToArray(),
            "vychadzajuci vlak nema stanice zo smeru");
        CollectionAssert.AreEqual(new[] { new ReportPatternStop(Lipovec, false), new ReportPatternStop(Javor, false) },
            data.Stations[0].LongReportPatterns[1].ToArray());
        Assert.IsEmpty(data.DefaultStations);
    }

    [TestMethod]
    public void StaniceHlasenia_JedenVariantHlaseniaBerieDlhyVyber()
    {
        var document = Document(Announced("1001"));
        document.ReportVariants.RemoveRange(1, document.ReportVariants.Count - 1);

        var record = Build(document).Stations[0].ReportStations.Single();

        CollectionAssert.AreEqual(new[] { Javor, Dubova }, record.ShortTo.ToArray());
    }

    [TestMethod]
    public void StaniceHlasenia_ZapisSubory()
    {
        var data = Build(Announced("1001"), Train("1002", Routing.Vychadzajuci));

        Iniss2Export.Write(data, _dataDir, "2030", "01 Lipovec");

        var dir = Path.Combine(_dataDir, "2030", "01 Lipovec");
        Assert.AreEqual("1001", TrainReportStationsFile.Read(dir).Single().Number);
        Assert.HasCount(2, ReportPatternsFile.Read(dir, ReportPatternsFile.ShortFileName));
        Assert.HasCount(2, DefaultStationsFile.Read(Path.Combine(_dataDir, "2030")));
    }

    [TestMethod]
    public void Vyluky_ZVlakuZabudovanaJeFunkciaLogText()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.LockoutNumber = 1;

        var data = Build(train);

        Assert.AreEqual(new TrainLockout("1001", "V1", From, To, ""), data.Lockouts.Single());
        Assert.IsEmpty(data.UserReports);
    }

    [TestMethod]
    public void Vyluky_ZVlakuPlatiaLenVJehoDni()
    {
        var weekday = Train("1001", Routing.Prechadzajuci, variant: 1, limit: "ide v 1-5");
        weekday.LockoutNumber = 1;
        var weekend = Train("1001", Routing.Prechadzajuci, variant: 2, limit: "ide v 6,7");

        var lockout = Build(weekday, weekend).Lockouts.Single();

        Assert.AreEqual("11111001111100", Bits(GvValidity.Days(From, To, lockout.Validity)));
    }

    [TestMethod]
    public void Vyluky_TextObsluhyJeVzorSoStanicami()
    {
        var first = Train("1001", Routing.Prechadzajuci);
        first.LockoutNumber = 1000004;
        var second = Train("1002", Routing.Prechadzajuci);
        second.LockoutNumber = 1000004;

        var data = Build(first, second);

        Assert.AreEqual("-1", data.Lockouts[0].Key);
        Assert.AreEqual("-1", data.Lockouts[1].Key, "rovnaky text je jedno hlasenie");
        var report = data.UserReports.Single();
        Assert.AreEqual((-1, "Výluka Lipovec - Horany", "Výluka_ve_stanici"), (report.Key, report.Name, report.Function));
        CollectionAssert.AreEqual(new[] { Lipovec, Horany }, report.Stations.ToArray());
    }

    [TestMethod]
    public void Vyluky_ObsluhyLenPreStanicuAVlakyGrafikonu()
    {
        var data = Iniss2Export.Build(Document(Train("1001", Routing.Prechadzajuci)), Gvd, Source([
            new TrainNotice("1001", 0, 1, From, From.AddDays(1), ""),
            new TrainNotice("1001", Horany, 1, From, From, ""),
            new TrainNotice("9999", 0, 1, From, From, ""),
            new TrainNotice("1001", Lipovec, 1000004, From.AddDays(2), From.AddDays(8), "ide v 6,7")
        ]));

        Assert.HasCount(2, data.Lockouts);
        Assert.AreEqual(new TrainLockout("1001", "V1", From, From.AddDays(1), ""), data.Lockouts[0]);
        Assert.AreEqual(("-1", From.AddDays(2), From.AddDays(8)), (data.Lockouts[1].Key, data.Lockouts[1].From, data.Lockouts[1].To));
        Assert.AreEqual("0001100", Bits(GvValidity.Days(From.AddDays(2), From.AddDays(8), data.Lockouts[1].Validity)));
    }

    [TestMethod]
    public void Vyluky_ObsluhyMimoDniVlakuUpozorni()
    {
        var data = Iniss2Export.Build(Document(Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5")), Gvd, Source([
            new TrainNotice("1001", 0, 1, From.AddYears(-1), From.AddYears(-1), ""),
            new TrainNotice("1001", 0, 1, From.AddDays(5), From.AddDays(6), "")
        ]));

        Assert.IsEmpty(data.Lockouts);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.NoticeOutsidePeriod, "1001"));
    }

    [TestMethod]
    public void Odklony_ObsluhyDostanuStanicuGrafikonu()
    {
        var data = Iniss2Export.Build(Document(Train("1001", Routing.Prechadzajuci)), Gvd, Source(diversions: [
            new TrainNotice("1001", 0, 1, From, To, ""),
            new TrainNotice("1001", Lipovec, 3001, From, From, "")
        ]));

        Assert.AreEqual(new TrainDiversion("1001", Lipovec, "O1", From, To, ""), data.Diversions[0]);
        Assert.AreEqual("-1", data.Diversions[1].Key);
        Assert.AreEqual("Odklon", data.UserReports.Single().Function);
    }

    [TestMethod]
    public void Zapis_TextyObsluhyDostanuCislaZaExistujucimi()
    {
        ConstReportsFile.Write(_dataDir, [new ConstReport(7, "Prestup", "Přestup_v_zadané_stanici", [Horany])]);
        var train = Train("1001", Routing.Prechadzajuci);
        train.LockoutNumber = 1000004;

        Iniss2Export.Write(Build(train), _dataDir, "2030", "01 Lipovec");

        Assert.AreEqual("8", VylukyFile.Read(Path.Combine(_dataDir, "2030")).Single().Key);
        CollectionAssert.AreEqual(new[] { 7, 8 }, ConstReportsFile.Read(_dataDir).Select(report => report.Key).ToArray());
    }

    [TestMethod]
    public void Zapis_RovnakyTextObsluhyPouzijeExistujuceCislo()
    {
        ConstReportsFile.Write(_dataDir, [new ConstReport(7, "Výluka", "Výluka_ve_stanici", [Lipovec, Horany])]);
        var train = Train("1001", Routing.Prechadzajuci);
        train.LockoutNumber = 1000004;

        Iniss2Export.Write(Build(train), _dataDir, "2030", "01 Lipovec");

        Assert.AreEqual("7", VylukyFile.Read(Path.Combine(_dataDir, "2030")).Single().Key);
        Assert.HasCount(1, ConstReportsFile.Read(_dataDir));
    }

    [TestMethod]
    public void Zapis_BezTextovObsluhyConstReportsNevznikne()
    {
        Iniss2Export.Write(Build(Train("1001", Routing.Prechadzajuci)), _dataDir, "2030", "01 Lipovec");

        Assert.IsFalse(File.Exists(Path.Combine(_dataDir, ConstReportsFile.FileName)));
    }

    [TestMethod]
    public void LogText_ChybajuceHlaseniaUpozorni()
    {
        File.WriteAllText(Path.Combine(_dataDir, "LogText.vb"), """
            Public Class LogText
                <Dodatek("Vlak má bufet")> _
                Public Sub D1006()
                End Sub ' D1006
                '<Vyluka("Obecná výluka ve stanici", TYPVYLUKY.AUTO)> _
                Public Sub V1_aby_to_INISS_nevidel()
                End Sub
                <VzorVyluky("Výluka", TYPVYLUKY.AUTO)> _
                Public Sub Výluka_ve_stanici(ByVal aiStanice As Integer())
                End Sub
                <Odklon("Odklon (obecný)")> _
                Public Sub O2()
                End Sub
            End Class
            """, ToolsCore.Iniss.Tools.Encodings.Win1250);
        var builtin = Train("1001", Routing.Prechadzajuci);
        builtin.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));
        builtin.LockoutNumber = 1;
        var user = Train("1002", Routing.Prechadzajuci);
        user.LockoutNumber = 1000004;
        var data = Iniss2Export.Build(Document(builtin, user), Gvd, Source(diversions: [new TrainNotice("1001", 0, 1, From, To, "")]));

        var warnings = Iniss2Export.CheckLogText(_dataDir, data);

        CollectionAssert.AreEqual(new[]
        {
            new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, "V1"),
            new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, "O1")
        }, warnings);
    }

    private const string Script = "Imports IniSrv\r\n\r\nPublic Class LogText\r\n    Inherits LogTextBase\r\n\r\n    Private Class Pomocna\r\n    End Class\r\n\r\n"
                                  + "    ' End Class v komentari\r\n    <Dodatek(\"Bufet\")> _\r\n    Public Sub D1()\r\n        Zvuk(\"DODATKY/D1\")\r\n    End Sub\r\nEnd Class\r\n";

    [TestMethod]
    public void LogText_FunkcieDodatkovPredKoncomTriedy()
    {
        var text = Iniss2Export.InsertIntoLogText(Script, [new LogTextAddition("D1006", "Vlak má \"bufet\"", "DODATKY/D1006")]);

        StringAssert.EndsWith(text, "    <Dodatek(\"Vlak má \"\"bufet\"\"\")> _\r\n    Public Sub D1006()\r\n        Zvuk(\"DODATKY/D1006\")\r\n    End Sub ' D1006\r\n\r\nEnd Class\r\n");
        StringAssert.Contains(text, "    Private Class Pomocna\r\n    End Class\r\n", "vnorena trieda ostane");
        Assert.DoesNotContain("\n\n", text.Replace("\r\n", "\r", StringComparison.Ordinal), "konce riadkov podla suboru");
    }

    [TestMethod]
    public void LogText_ExistujucaFunkciaSaNezdvoji()
    {
        var file = Path.Combine(_dataDir, "LogText.vb");
        File.WriteAllText(file, Script, ToolsCore.Iniss.Tools.Encodings.Win1250);

        var added = Iniss2Export.AddAdditionFunctions(file, [new LogTextAddition("D1", "Bufet", "DODATKY/D1"), new LogTextAddition("D2", "Dva", "DODATKY/D2")]);

        CollectionAssert.AreEqual(new[] { "D2" }, added);
        var text = File.ReadAllText(file, ToolsCore.Iniss.Tools.Encodings.Win1250);
        Assert.AreEqual(1, text.Split("Public Sub D1()").Length - 1);
        Assert.IsEmpty(Iniss2Export.AddAdditionFunctions(file, [new LogTextAddition("D2", "Dva", "DODATKY/D2")]));
    }

    [TestMethod]
    public void LogText_ZapisDoplniChybajuciDodatok()
    {
        File.WriteAllText(Path.Combine(_dataDir, "LogText.vb"), Script, ToolsCore.Iniss.Tools.Encodings.Win1250);
        var train = Train("1001", Routing.Prechadzajuci);
        train.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));
        var data = Build(train);

        var warnings = Iniss2Export.CheckLogText(_dataDir, data);
        Iniss2Export.Write(data, _dataDir, "2030", "01 Lipovec");

        Assert.AreEqual(new LogTextAddition("D1006", "Vlak má bufet", "DODATKY/D1006"), data.AdditionFunctions.Single());
        Assert.AreEqual(new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionAdded, "D1006"), warnings.Single());
        StringAssert.Contains(File.ReadAllText(Path.Combine(_dataDir, "LogText.vb"), ToolsCore.Iniss.Tools.Encodings.Win1250), "<Dodatek(\"Vlak má bufet\")> _");
    }

    [TestMethod]
    public void LogText_BezSuboruUpozorni()
    {
        var warnings = Iniss2Export.CheckLogText(_dataDir, Build(Train("1001", Routing.Prechadzajuci)));

        Assert.AreEqual(Iniss2ExportIssue.LogTextMissing, warnings.Single().Issue);
    }

    [TestMethod]
    public void Spajanie_VlakMaCasyVOboch()
    {
        var data = BuildPair([Train("1001", Routing.Prechadzajuci)], [Next("1001")]);

        var stops = data.Timetable.Trains.Single().Variants.Single().Stops;

        CollectionAssert.AreEqual(new[] { Horany, Lipovec, Javor, Dubova }, stops.Select(stop => stop.Station).ToArray());
        Assert.AreEqual((8 * 60 + 15, 8 * 60 + 17), (stops[1].Arrival, stops[1].Departure));
        Assert.AreEqual((8 * 60 + 40, 8 * 60 + 42), (stops[2].Arrival, stops[2].Departure));
        Assert.AreEqual(GvStopFlags.Interest, stops[2].Flags);
        Assert.AreEqual("1001", stops[0].Number);
        Assert.AreEqual("", stops[2].Number, "cislo, druh a nazov sa nemenia");
        CollectionAssert.AreEqual(new[] { Lipovec, Javor }, data.Timetable.Stations.ToArray());
        Assert.IsEmpty(data.Warnings);
    }

    [TestMethod]
    public void Spajanie_PolnocPosunieDenDruhejStanice()
    {
        var home = Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5");
        home.Arrival = new TimeOnly(23, 45);
        home.Departure = new TimeOnly(23, 50);

        var data = BuildPair([home], [Next("1001", "ide v 2-6", new TimeOnly(0, 20), new TimeOnly(0, 22))]);

        var train = data.Timetable.Trains.Single();
        var variant = train.Variants.Single();
        Assert.AreEqual((1, 1), (variant.Stops[2].ArrivalDay, variant.Stops[2].DepartureDay));
        Assert.AreEqual("11111001111100", Bits(train.DaysOf(variant)));
    }

    [TestMethod]
    public void Spajanie_RozneDniDajuVarianty()
    {
        var data = BuildPair([Train("1001", Routing.Prechadzajuci)], [Next("1001", "ide v 1-5")]);

        var train = data.Timetable.Trains.Single();
        Assert.HasCount(2, train.Variants);
        Assert.AreEqual("11111001111100", Bits(train.DaysOf(train.Variants[0])));
        Assert.IsTrue(train.Variants[0].Stops[2].HasTime);
        Assert.AreEqual("00000110000011", Bits(train.DaysOf(train.Variants[1])));
        Assert.IsFalse(train.Variants[1].Stops[2].HasTime, "cez Javorinu ide bez zastavenia v stanici grafikonu");
    }

    [TestMethod]
    public void Spajanie_DniLenVDruhejStaniciSuVariantouToistehoVlaku()
    {
        var data = BuildPair([Train("1001", Routing.Prechadzajuci, limit: "ide v 1-5")], [Next("1001")]);

        var train = data.Timetable.Trains.Single();
        Assert.HasCount(2, train.Variants);
        Assert.AreEqual("00000110000011", Bits(train.DaysOf(train.Variants[1])));
        Assert.AreEqual(Javor, train.Variants[1].Stops.First(stop => stop.HasTime).Station);
        Assert.AreEqual("1001", train.Variants[1].Stops[0].Number);
    }

    [TestMethod]
    public void Spajanie_OpacnySmerSaNespoji()
    {
        var next = Next("1001");
        next.StaniceZoSmeru.Clear();
        next.StaniceZoSmeru.Add(Dub);
        next.StaniceDoSmeru.Clear();
        next.StaniceDoSmeru.AddRange([Home, Hills]);

        var data = BuildPair([Train("1001", Routing.Prechadzajuci)], [next]);

        Assert.HasCount(2, data.Timetable.Trains);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.TrainNotMerged, "1001"));
    }

    [TestMethod]
    public void Spajanie_KonciaciVlakSaNespojiSVychadzajucim()
    {
        var data = BuildPair([Train("1001", Routing.Konciaci)], [Train("1001", Routing.Vychadzajuci)]);

        Assert.HasCount(2, data.Timetable.Trains);
        Assert.IsEmpty(data.Warnings);
    }

    [TestMethod]
    public void Spajanie_InaLinkaZaStanicouSaZapise()
    {
        var home = Train("1001", Routing.Prechadzajuci);
        home.LineArrival = "S1";
        home.LineDeparture = "S1";
        var next = Next("1001");
        next.LineArrival = "S1";
        next.LineDeparture = "S2";

        var stops = BuildPair([home], [next]).Timetable.Trains.Single().Variants.Single().Stops;

        Assert.AreEqual("S1", stops[0].Line);
        Assert.AreEqual(("1001", "S2"), (stops[2].Number, stops[2].Line));
    }

    [TestMethod]
    public void Spajanie_DniDruhejStaniceMimoObdobiaUpozornia()
    {
        var home = Train("1001", Routing.Prechadzajuci);
        home.Departure = new TimeOnly(23, 50);
        home.Arrival = new TimeOnly(23, 45);
        var next = Next("1001", arrival: new TimeOnly(0, 20), departure: new TimeOnly(0, 22));
        next.ZaciatokPlatnosti = From.AddDays(1);
        next.KoniecPlatnosti = To.AddDays(1);

        var data = BuildPair([home], [next], new GVDInfo { ThisStation = Oak, StartValidTimeTable = From.AddDays(1), EndValidTimeTable = To.AddDays(1) });

        Assert.HasCount(1, data.Timetable.Trains.Single().Variants);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.StationDayOutsidePeriod, "1001"));
    }

    [TestMethod]
    public void Spajanie_RovnakaStanicaDvakratZastaviExport()
    {
        var error = Assert.Throws<Iniss2ExportException>(() =>
            Iniss2Export.Build([new Iniss2StationInput(Document(), Gvd), new Iniss2StationInput(Document(), Gvd)], Source()));

        Assert.AreEqual(Iniss2ExportIssue.StationTwice, error.Issue);
    }

    [TestMethod]
    public void Spajanie_ZapisDvochStanicServerovymCitacom()
    {
        var home = Train("1001", Routing.Prechadzajuci);
        home.Doplnky.Add(Addition(Routing.Prechadzajuci, "P"));
        var next = Next("1001", "ide v 1-5");
        next.Track = Two;

        Iniss2Export.Write(BuildPair([home], [next]), _dataDir, "2030", ["01 Lipovec", "02 Javorina"]);

        var group = DirListXml.Read(_dataDir).Single();
        CollectionAssert.AreEqual(new[] { Lipovec, Javor }, group.Items.Select(item => item.Station).ToArray());
        var oak = StationTimetable.Read(_dataDir, group, group.Items[1]);
        var call = oak.Calls.Single();
        Assert.AreEqual("2", oak.PositionFor(call, From)!.ArrivalTrack);
        Assert.HasCount(1, DodatkyFile.Read(Path.Combine(_dataDir, "2030", "01 Lipovec")));
        Assert.IsEmpty(DodatkyFile.Read(Path.Combine(_dataDir, "2030", "02 Javorina")));
    }

    // predloha ELEN16 s casom odchodu (preklad tabulkou) a cielom; zobrazenie odchodovej tabule v rezime Vlak
    private static TableCatalog Catalog(GrafikonDocument document, TableManufacturer manufacturer, string key = "Odchody")
    {
        var time = new TableItem
        {
            Key = "Cas", Name = "Čas", FillSection = TableFillSection.CasOdchodu, Line = 0, Start = 0, End = 32, FontIdx = 74, Align = TableAlign.Right,
            DivType = TableDivType.Translate, Tab1 = document.TabTabs.FirstOrDefault() ?? TableTabTab.Empty, Tab2 = TableTabTab.Empty
        };
        var target = new TableItem
        {
            Key = "Ciel", Name = "Cieľ", FillSection = TableFillSection.CielovaStanica, Line = 0, Start = 40, End = 200, FontIdx = 66, Align = TableAlign.Left,
            DivType = TableDivType.Free, Tab1 = TableTabTab.Empty, Tab2 = TableTabTab.Empty
        };
        var delay = new TableItem
        {
            Key = "Meskanie", Name = "Meškanie", FillSection = TableFillSection.MeskanieOdchod, Line = 0, Start = 200, End = 240, Align = TableAlign.Right,
            DivType = TableDivType.Free, Tab1 = TableTabTab.Empty, Tab2 = TableTabTab.Empty
        };
        var catalog = new TableCatalog { Key = key, Name = key, Manufacturer = manufacturer };
        catalog.Items.AddRange([time, target, delay]);
        var tab = new TableViewTypeTab { ViewType = TableViewType.Odchodova, CountLinesRecord = "1" };
        tab.TypeModeItems.Add(new TableTypeModeItem { ViewMode = TableViewMode.Vlak, ItemsKeys = ["Cas", "Ciel"] });
        tab.TypeModeItems.Add(new TableTypeModeItem { ViewMode = TableViewMode.VlakZmeskany, ItemsKeys = ["Cas", "Ciel", "Meskanie"] });
        catalog.ViewTypeTabs.Add(tab);
        document.TableCatalogs.Add(catalog);
        return catalog;
    }

    private static TablePhysical Physical(GrafikonDocument document, string key, int id, int records, TableCatalog catalog)
    {
        var physical = new TablePhysical { Key = key, Name = key, ID = id, RecCount = records, TableCatalog = catalog, ReverseArrows = "", SaveXML = "" };
        document.TablePhysicals.Add(physical);
        return physical;
    }

    // logicka tabula so zaznamami na poziciach fyzickych tabul: (tabula, pozicia) pre kazdy zaznam
    private static TableLogical Logical(GrafikonDocument document, string key, TableViewType view, params (TablePhysical Table, int Position)[] records)
    {
        var logical = new TableLogical { Key = key, Name = key, ViewType = view };
        foreach (var (table, position) in records)
        {
            var record = new TableRecord();
            record.Positions.Add(new TablePosition { Table = table, Position = position, TypeView = view });
            logical.Records.Add(record);
        }

        document.TableLogicals.Add(logical);
        return logical;
    }

    private static GrafikonDocument TableDocument(TableManufacturer manufacturer, out TablePhysical physical, out TableLogical logical)
    {
        var document = Document(Train("1001", Routing.Prechadzajuci));
        document.TabTabs.Add(new TableTabTab { Key = "Odjezd", Text = "8:15{74} = 8:15\nBus = #VYLUKA\nR{65} = R\nR{66} = R" });
        var catalog = Catalog(document, manufacturer);
        physical = Physical(document, "Hala", 1, 3, catalog);
        logical = Logical(document, "Odchodova", TableViewType.Odchodova, (physical, 0), (physical, 1));
        One.Tables.Clear();
        One.Tables.Add(logical);
        One.TablePriorities.Clear();
        One.TablePriorities[logical.Key] = 2;
        return document;
    }

    [TestMethod]
    public void Tabule_FyzickaTabulaMaCastiLogickychTabul()
    {
        var data = Build(TableDocument(TableManufacturer.Elen16, out _, out _));

        var table = data.Stations[0].Tables.Single();
        Assert.AreEqual(("Hala", 1, ""), (table.Key, table.Address, table.Line));
        Assert.HasCount(2, table.Parts);
        Assert.AreEqual(("Odchody", "DEPARTURE", 2), (table.Parts[0].CatalogKey, table.Parts[0].Kind, table.Parts[0].Records));
        CollectionAssert.AreEqual(new[] { "1" }, table.Parts[0].Tracks.ToArray());
        Assert.AreEqual(2, table.Parts[0].TrackPriorities["1"]);
        Assert.AreEqual((TablePart.NotDefined, 1), (table.Parts[1].Kind, table.Parts[1].Records), "volna pozicia");
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.TableWithoutLine, "Hala"));
    }

    [TestMethod]
    public void Tabule_PredlohaVBodochSPrekladomARezimomMeskania()
    {
        var data = Build(TableDocument(TableManufacturer.Elen16, out _, out _));

        var catalog = data.Stations[0].Catalogs.Single();
        Assert.AreEqual(("Odchody", "ELEN16", ""), (catalog.Key, catalog.Class, catalog.FontBank));
        CollectionAssert.AreEqual(new[] { "Cas", "Ciel", "Meskanie" }, catalog.Columns.Select(column => column.Key).ToArray(), "meskanie z rezimu s meskanim");
        var time = catalog.Columns[0];
        Assert.AreEqual(("CasOdjezdu", 0, 32, "RIGHT", 74), (time.Content, time.Start, time.End, time.Align, time.Font));
        Assert.AreEqual(("TRANSLATE", "Odjezd", ""), (time.Translation, time.Table1, time.Table2));
        Assert.AreEqual((CatalogColumn.FreeTranslation, ""), (catalog.Columns[1].Translation, catalog.Columns[1].Table1));
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.FontBankMissing, "Odchody"));
    }

    [TestMethod]
    public void Tabule_ZnakoveZariadenieMaStlpceVZnakoch()
    {
        var catalog = Build(TableDocument(TableManufacturer.Lcd1, out _, out _)).Stations[0].Catalogs.Single();

        Assert.AreEqual(("LCD1", 0, 4, 5, 25), (catalog.Class, catalog.Columns[0].Start, catalog.Columns[0].End, catalog.Columns[1].Start, catalog.Columns[1].End));
    }

    [TestMethod]
    public void Tabule_BezAdresyJeInternetSoSuborom()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var physical, out _);
        physical.ID = -1;
        physical.SaveXML = "5400010O";

        var data = Build(document);

        var table = data.Stations[0].Tables.Single();
        Assert.AreEqual((-1, "5400010O.xml", ""), (table.Address, table.SaveFile, table.Line));
        Assert.AreEqual(CatalogTable.Internet, data.Stations[0].Catalogs.Single().Class);
        Assert.IsFalse(data.Warnings.Any(warning => warning.Issue is Iniss2ExportIssue.TableWithoutLine or Iniss2ExportIssue.FontBankMissing));
    }

    [TestMethod]
    public void Tabule_PokracovanieLogickejTabuleNaDalsej()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var hall, out var logical);
        var second = Physical(document, "Hala 2", 2, 2, hall.TableCatalog);
        foreach (var position in new[] { 0, 1 })
        {
            var record = new TableRecord();
            record.Positions.Add(new TablePosition { Table = second, Position = position, TypeView = TableViewType.Odchodova });
            logical.Records.Add(record);
        }

        var tables = Build(document).Stations[0].Tables;

        Assert.AreEqual("", tables[0].Parts[0].Previous);
        Assert.AreEqual("Hala", tables[1].Parts.Single().Previous);
    }

    [TestMethod]
    public void Tabule_ZrkadloMaNaKazdejTabuliVlastnuCast()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var hall, out var logical);
        var mirror = Physical(document, "Podchod", 2, 2, hall.TableCatalog);
        for (var record = 0; record < 2; record++)
            logical.Records[record].Positions.Add(new TablePosition { Table = mirror, Position = record, TypeView = TableViewType.Odchodova });

        var part = Build(document).Stations[0].Tables[1].Parts.Single();

        Assert.AreEqual(("DEPARTURE", 2, ""), (part.Kind, part.Records, part.Previous));
        CollectionAssert.AreEqual(new[] { "1" }, part.Tracks.ToArray());
    }

    [TestMethod]
    public void Tabule_NepodporovanyDruhSaVynecha()
    {
        var data = Build(TableDocument(TableManufacturer.Ers, out _, out _));

        Assert.IsEmpty(data.Stations[0].Tables);
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.TableUnsupported, "Hala"));
    }

    [TestMethod]
    public void TabTab_JednoduchePravidlaDoSlovnikaUdalostiDoVyrazu()
    {
        var data = Build(TableDocument(TableManufacturer.Elen16, out _, out _));

        var table = data.Stations[0].TabTabs.Single();
        Assert.AreEqual("Odjezd", table.Name);
        CollectionAssert.AreEqual(new[]
        {
            new TabTabEntry("8:15", TabTabTable.AnyFont, "8:15", 74), new TabTabEntry("R", TabTabTable.AnyFont, "R", 66)
        }, table.Entries.ToArray(), "kluc bez pisma, posledne pravidlo vyhrava");
        StringAssert.Contains(table.Expression, "If (.VylukaPrij Or .VylukaOdj) AndAlso accept(New TabString(\"Bus\", -1)) Then Return r");
        StringAssert.EndsWith(table.Expression, "sOrig.iFont)))");
        Assert.IsFalse(data.Warnings.Any(warning => warning.Issue == Iniss2ExportIssue.TabTabNotConverted));
    }

    [TestMethod]
    public void TabTab_PravidloSInymStlpcomOstaneKomentarom()
    {
        var document = TableDocument(TableManufacturer.Elen16, out _, out _);
        document.TabTabs[0].Text += "\nL = %Linka%";

        var data = Build(document);

        var table = data.Stations[0].TabTabs.Single();
        Assert.StartsWith("' L = %Linka%\n", table.Expression);
        StringAssert.Contains(table.Expression, "accept(New TabString(\"Bus\", -1))");
        CollectionAssert.Contains(data.Warnings.ToArray(), new Iniss2ExportWarning(Iniss2ExportIssue.TabTabNotConverted, "Odjezd"));
    }

    [TestMethod]
    public void TabTab_ChybajucaTabulkaZnamenaBezPrekladu()
    {
        var document = TableDocument(TableManufacturer.Elen16, out _, out _);
        document.TabTabs.Clear();

        var data = Build(document);

        Assert.AreEqual(CatalogColumn.FreeTranslation, data.Stations[0].Catalogs.Single().Columns[0].Translation);
        Assert.IsEmpty(data.Stations[0].TabTabs);
        Assert.IsTrue(data.Warnings.Any(warning => warning.Issue == Iniss2ExportIssue.TabTabMissing));
    }

    [TestMethod]
    public void Texty_TextVlakuPreStlpecPredlohy()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var physical, out _);
        var text = new TableText { Key = "Ciel", Name = "Cieľ" };
        text.Realizations.Add(new TableTextRealization { Table = physical.TableCatalog, Item = physical.TableCatalog.Items[1] });
        text.Trains.Add(new TableTrain { Train = document.Trains[0], Text = "Horná Lehota", FontID = 2 });
        document.TableTexts.Add(text);

        var set = Build(document).Stations[0].Texts.Single();

        CollectionAssert.AreEqual(new[] { new TextTarget("Odchody", "Ciel") }, set.Targets.ToArray());
        CollectionAssert.AreEqual(new[] { new TrainText("1001", "", "Horná Lehota", 2) }, set.Trains.ToArray());
    }

    [TestMethod]
    public void Linky_ZoZisteniaINISSuDostaneTabula()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var physical, out _);
        var map = new DriverLineMap([
            new DriverLine("Driver", 1, 4, "COM2", true, [new LineTable(new InissTable(0, "Lipovec.2030", physical), false)], []),
            new DriverLine("Driver0", 2, 130, "2=TCP://10.0.0.1:4001", true, [new LineTable(new InissTable(1, "Lipovec.2030", physical), false)], [])
        ], []);

        var (lines, warnings) = Iniss2Export.TableLines(map, section => section == "Driver" ? "9600,n,8,1" : null, "Lipovec");
        var data = Iniss2Export.Build([new Iniss2StationInput(document, Gvd) { Grafikon = "Lipovec.2030" }], Source() with { TableLines = lines });

        var line = data.Lines.Single();
        Assert.AreEqual(("Lipovec - linka 1", CommLineType.Serial, "ELEN", "COM2", "9600,n,8,1"), (line.Key, line.Type, line.Protocol, line.Address, line.Parameters));
        Assert.AreEqual(line.Key, data.Stations[0].Tables.Single().Line);
        CollectionAssert.AreEqual(new[] { new Iniss2ExportWarning(Iniss2ExportIssue.LineUnsupported, "Driver0: 2=TCP://10.0.0.1:4001") }, warnings,
            "ITP funguje len cez UDP");
    }

    [TestMethod]
    public void Pisma_BankaZoSuboruSaSkopiruje()
    {
        var bank = Path.Combine(_dataDir, "banka.bin");
        new ElenFontFile("16_test", [Font()]).Save(bank);
        var data = Iniss2Export.Build(TableDocument(TableManufacturer.Elen16, out _, out _), Gvd,
            Source() with { FontBanks = new Dictionary<string, string> { ["Odchody"] = bank } });

        Iniss2Export.Write(data, _dataDir, "2030", "01 Lipovec");

        Assert.AreEqual("16_test", data.Stations[0].Catalogs.Single().FontBank);
        Assert.IsTrue(File.Exists(Path.Combine(_dataDir, "TableFonts", "16_test.bin")));
    }

    private static ElenFont Font()
    {
        var glyphs = new ElenGlyph[ElenFont.GlyphCount];
        for (var i = 0; i < glyphs.Length; i++)
            glyphs[i] = new ElenGlyph(6, 16, 1, new byte[16]);
        return new ElenFont("FIX_16", 6, 16, false, glyphs);
    }

    [TestMethod]
    public void Zapis_TabuleKatalogPrekladyATextySaDajuPrecitat()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var physical, out _);
        var text = new TableText { Key = "Ciel", Name = "Cieľ" };
        text.Realizations.Add(new TableTextRealization { Table = physical.TableCatalog, Item = physical.TableCatalog.Items[1] });
        text.Trains.Add(new TableTrain { Train = document.Trains[0], Text = "Horná Lehota", FontID = -1 });
        document.TableTexts.Add(text);

        Iniss2Export.Write(Build(document), _dataDir, "2030", "01 Lipovec");

        var dir = Path.Combine(_dataDir, "2030", "01 Lipovec");
        Assert.AreEqual("Hala", ToolsCore.Iniss2.Tables.TablesFile.Read(dir).Single().Key);
        Assert.HasCount(3, TableCatalogFile.Read(dir).Single().Columns);
        Assert.AreEqual("Odjezd", TabTabXml.Read(Path.Combine(dir, TabTabXml.StationFileName)).Single().Name);
        Assert.AreEqual("Horná Lehota", TrainTextsFile.Read(dir).Single().Trains.Single().Text);
    }

    // stanica cielovej instalacie so stavovym diagramom a hlaseniami INISS2
    private string TemplateStation()
    {
        var dir = Path.Combine(_dataDir, "2020", "01 Horany");
        DirListXml.AddStation(_dataDir, "2020", new DirListItem("01 Horany", Horany, "", "", DirListItemFlags.None));
        DirListXml.WriteStationNumber(dir, Horany);
        File.WriteAllText(Path.Combine(dir, Iniss2Export.StateDiagramFile), "<STATEDGM />", ToolsCore.Iniss.Tools.Encodings.Win1250);
        ReportKeysFile.Write(dir, [new ReportKey("Přijede", "P", "V P K"), new ReportKey("Vjíždí", "V", "P K"), new ReportKey("Odjede", "O", "V P")]);
        return dir;
    }

    [TestMethod]
    public void Sablona_PismenaDodatkovPodlaSablony()
    {
        var train = Train("1001", Routing.Prechadzajuci);
        train.Doplnky.Add(Addition(Routing.Prechadzajuci, "P", "p", "I", "L"));
        var letters = new Dictionary<string, string> { ["Přijíždí"] = "P", ["Vjíždí"] = "V", ["Zastavil"] = "" };

        var data = Iniss2Export.Build(Document(train), Gvd, Source() with { ReportLetters = letters, ReportTemplate = [new ReportKey("Přijede", "P", "V P K")] });

        Assert.AreEqual("PpV", data.Stations[0].Additions.Single().Reports, "Zastavil sablona nema");
        Assert.AreEqual(new ReportKey("Přijede", "P", "V P K"), data.Stations[0].ReportKeys.Single());
    }

    [TestMethod]
    public void Sablona_ZapisSkopirujeStavovyDiagram()
    {
        var template = TemplateStation();
        var data = Iniss2Export.Build(Document(Train("1001", Routing.Prechadzajuci)), Gvd, Source() with { TemplateStation = template });

        Iniss2Export.Write(data, _dataDir, "2030", "01 Lipovec");

        Assert.AreEqual("<STATEDGM />", File.ReadAllText(Path.Combine(_dataDir, "2030", "01 Lipovec", Iniss2Export.StateDiagramFile)));
    }

    // cielova instalacia s jednou stanicou bez stavoveho diagramu
    private void Installation()
    {
        DirListXml.AddStation(_dataDir, "2020", new DirListItem("01 Horany", Horany, "", "", DirListItemFlags.None));
        DirListXml.WriteStationNumber(Path.Combine(_dataDir, "2020", "01 Horany"), Horany);
    }

    // stavovy diagram 3.39 s jednou kategoriou: tlacidlo Prichadza vedie do stavu s hlasenim Přijíždí
    private static ToolsCore.Iniss.StateDgm.StateDgmDiagram StateDgm()
    {
        var category = new ToolsCore.Iniss.StateDgm.StateDgmCategory { Key = "#Projíždějící vlak" };
        foreach (var key in new[] { "#Start", "Přijíždí" })
        {
            var state = new ToolsCore.Iniss.StateDgm.StateDgmState { Key = key };
            state.Events.Add(new ToolsCore.Iniss.StateDgm.StateDgmEvent { Key = "#GoToPrijde", NextState = "Přijíždí", ReportKey = "Přijíždí" });
            state.Controls.Add(new ToolsCore.Iniss.StateDgm.StateDgmControl { CtrlId = 4, DesignKey = "Príde3", EventKey = "#GoToPrijde" });
            category.States.Add(state);
        }

        var diagram = new ToolsCore.Iniss.StateDgm.StateDgmDiagram();
        diagram.Categories.Add(category);
        return diagram;
    }

    [TestMethod]
    public void StavovyDiagram_PrevedenyMaPrednostPredSablonou()
    {
        var template = TemplateStation();
        var letters = new Dictionary<string, string> { ["Přijíždí"] = "P" };
        var data = Iniss2Export.Build([new Iniss2StationInput(Document(Train("1001", Routing.Prechadzajuci)), Gvd) { StateDgm = StateDgm() }],
            Source() with { TemplateStation = template, ReportTemplate = ReportKeysFile.Read(template), ReportLetters = letters });

        Iniss2Export.Write(data, _dataDir, "2030", "01 Lipovec");

        var nodes = StateDiagramFile.Read(Path.Combine(_dataDir, "2030", "01 Lipovec"))!.Graphs.Single().Nodes;
        Assert.AreEqual(("Přijíždí", "Přijede", new StateButton("Príde", 4)), (nodes[1].Key, nodes[1].Report, nodes[1].Button),
            "hlasenie sablony s pismenom typu");
        Assert.IsEmpty(data.Warnings);
    }

    [TestMethod]
    public void StavovyDiagram_BezSablonyHlasenieJeKlucTypu()
    {
        Installation();
        var data = Iniss2Export.Build([new Iniss2StationInput(Document(Train("1001", Routing.Prechadzajuci)), Gvd) { StateDgm = StateDgm() }], Source());

        Assert.AreEqual("Přijíždí", data.Stations[0].StateDiagram!.Graphs[0].Nodes[1].Report);
        Assert.IsFalse(Iniss2ExportService.Check(_dataDir, data).Any(warning => warning.Issue == Iniss2ExportIssue.StateDiagramMissing));
    }

    [TestMethod]
    public void StavovyDiagram_BezDiagramuASablonyUpozorni()
    {
        Installation();
        var warnings = Iniss2ExportService.Check(_dataDir, Build(Document(Train("1001", Routing.Prechadzajuci))));

        Assert.AreEqual(new Iniss2ExportWarning(Iniss2ExportIssue.StateDiagramMissing, "5400010"),
            warnings.Single(warning => warning.Issue == Iniss2ExportIssue.StateDiagramMissing));
    }

    [TestMethod]
    public void LogText_HlasenieStavuMusiBytMetoda()
    {
        File.WriteAllText(Path.Combine(_dataDir, "LogText.vb"), """
            Public Class LogText
                <ZakladniHlaseni("Príde")> _
                Public Sub Odjede()
                End Sub
            End Class
            """, ToolsCore.Iniss.Tools.Encodings.Win1250);
        var data = Iniss2Export.Build([new Iniss2StationInput(Document(Train("1001", Routing.Prechadzajuci)), Gvd) { StateDgm = StateDgm() }], Source());

        var warnings = Iniss2Export.CheckLogText(_dataDir, data);

        Assert.AreEqual(new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, "Přijíždí"), warnings.Single());
    }

    [TestMethod]
    public void Sluzba_PredvolenyGrafikonAStanica()
    {
        Assert.AreEqual("2030", Iniss2ExportService.DefaultGroup(Gvd));
        Assert.AreEqual("01 Lipovec", Iniss2ExportService.DefaultStationDirectory(0, "Lipovec"));
        Assert.AreEqual("12 Horná Lehota", Iniss2ExportService.DefaultStationDirectory(11, "Horná: Lehota"));
    }

    [TestMethod]
    public void Sluzba_SablonyACielovyPriecinok()
    {
        var template = TemplateStation();
        DirListXml.WriteStationNumber(Path.Combine(_dataDir, "2020", "02 Prazdna"), Javor);
        DirListXml.AddStation(_dataDir, "2020", new DirListItem("02 Prazdna", Javor, "", "", DirListItemFlags.None));

        var stations = Iniss2ExportService.TemplateStations(_dataDir);

        Assert.AreEqual(template, stations.Single().Path, "stanica bez stavoveho diagramu sablonou nie je");
        Assert.HasCount(3, stations[0].Reports);
        Assert.AreEqual(_dataDir, Iniss2Installation.DataDirectory(_dataDir));
        Assert.IsNull(Iniss2Installation.DataDirectory(template));
        Assert.IsFalse(Iniss2ExportService.GroupExists(_dataDir, "2020"));
    }

    [TestMethod]
    public void Sluzba_PredvolenePismenaPodlaZnaku()
    {
        var letters = Iniss2ExportService.DefaultLetters(ReportType.GetDefaultValuesSk(),
            [new ReportKey("Přijede", "P", "V P K"), new ReportKey("Odjede", "O", "V P")]);

        Assert.AreEqual("P", letters["Přijíždí"]);
        Assert.AreEqual("O", letters["Odjede"]);
        Assert.AreEqual("", letters["Vjíždí"], "znak I sablona nema");
    }

    [TestMethod]
    public void Sluzba_KatalogyElenPotrebujuBanku()
    {
        var document = TableDocument(TableManufacturer.Elen16, out var physical, out _);
        var internet = Catalog(document, TableManufacturer.Elen10, "Internet");
        Physical(document, "Web", -1, 1, internet);

        CollectionAssert.AreEqual(new[] { physical.TableCatalog }, Iniss2ExportService.BankCatalogs([document]), "tabula bez adresy banku nepotrebuje");
    }

    [TestMethod]
    public void Hlasenia_KlucPismenoAKategorieZTypov()
    {
        var reports = Build().Stations[0].ReportKeys;

        Assert.AreEqual(new ReportKey("Přijíždí", "P", "V P K"), reports[0]);
        Assert.AreEqual(new ReportKey("Pobytové", "N", "V P"), reports[3]);
    }

    [TestMethod]
    public void Zapis_DoExistujucehoGrafikonuOdmietne()
    {
        Iniss2Export.Write(Build(Train("1001", Routing.Prechadzajuci)), _dataDir, "2030", "01 Lipovec");

        var error = Assert.Throws<Iniss2ExportException>(() => Iniss2Export.Write(Build(), _dataDir, "2030", "02 Horany"));

        Assert.AreEqual(Iniss2ExportIssue.GroupExists, error.Issue);
    }

    [TestMethod]
    public void Zapis_PridaGrafikonKExistujucim()
    {
        Iniss2Export.Write(Build(Train("1001", Routing.Prechadzajuci)), _dataDir, "2030", "01 Lipovec");
        Iniss2Export.Write(Build(Train("1002", Routing.Prechadzajuci)), _dataDir, "2031", "01 Lipovec");

        CollectionAssert.AreEqual(new[] { "2030", "2031" }, DirListXml.Read(_dataDir).Select(g => g.Name).ToArray());
    }
}
