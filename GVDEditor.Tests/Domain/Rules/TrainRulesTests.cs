using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Domain.Rules;

/// <summary>
/// Kontrola vlaku v okne vlaku (TrainRules) a varianty vlaku (TrainVariants).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainRulesTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly TrainType R = new("R");
    private static readonly Operator Zssk = new(1, "ZSSK");
    private static readonly Track Track1 = new("1", "1", "Koľaj 1", Platform.None, "", "1");
    private static readonly DateOnly From = new(2026, 12, 13);
    private static readonly DateOnly To = new(2027, 12, 11);

    private static TrainContext Context(IReadOnlyList<Train>? trains = null, int row = -1) =>
        new(trains ?? [], row < 0 ? trains?.Count ?? 0 : row);

    // platny koncept prechadzajuceho vlaku Os 3001
    private static TrainDraft Draft()
    {
        var draft = TrainDraft.New(From, To);
        draft.Number = "3001";
        draft.Type = Os;
        draft.Operator = Zssk;
        draft.Track = Track1;
        draft.TrackDeparture = Track1;
        draft.ArrivalText = "10:12";
        draft.DepartureText = "10:15";
        draft.DateLimitText = "ide v 1-5";
        draft.RouteFrom.Add(new Station("1", "Veľká Ves"));
        draft.RouteTo.Add(new Station("2", "Brezová Dolina"));
        return draft;
    }

    private static Train Variant(int variant, string dateLimit, string number = "3001", TrainType? type = null) => new()
    {
        Number = number,
        Name = "",
        Type = type ?? Os,
        Variant = variant,
        DateLimitText = dateLimit,
        ZaciatokPlatnosti = From,
        KoniecPlatnosti = To
    };

    private static List<TrainRules.Field> Errors(TrainDraft draft, TrainContext? context = null) =>
        TrainRules.Check(draft, context ?? Context()).Where(p => !p.IsWarning).Select(p => p.Field).ToList();

    private static List<TrainRules.Field> Warnings(TrainDraft draft, TrainContext? context = null) =>
        TrainRules.Check(draft, context ?? Context()).Where(p => p.IsWarning).Select(p => p.Field).ToList();

    [TestMethod]
    public void Vlak_PlatnyKoncept_NemaChyby()
    {
        Assert.IsEmpty(TrainRules.Check(Draft(), Context()));
    }

    [TestMethod]
    [DataRow("3001", true)]
    [DataRow("R521a", true)]
    [DataRow("", false)]
    [DataRow("30 01", false)]
    [DataRow("30;01", false)]
    [DataRow("30\t01", false)]
    public void CisloVlaku_PrazdneAleboSoZnakomRiadku_JeChyba(string number, bool valid)
    {
        Assert.AreEqual(valid, TrainRules.CheckNumber(number) is null);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("S20", true)]
    [DataRow("abcdefghij0123456789", true)]
    [DataRow("abcdefghij0123456789X", false)]
    [DataRow("S-20", false)]
    [DataRow("Ž1", false)]
    [DataRow("S 20", false)]
    public void Linka_NajviacDvadsatPismenACislic(string line, bool valid)
    {
        Assert.AreEqual(valid, TrainRules.CheckLine(line) is null);
    }

    [TestMethod]
    [DataRow("", true)]
    [DataRow("ide v 1-5", true)]
    [DataRow("ide 24.XII.2030", false)]
    [DataRow("ide 20.XII.-2.I.", false)]
    public void DatumoveObmedzenie_CitatelnostVObdobiPlatnosti(string text, bool valid)
    {
        Assert.AreEqual(valid, TrainRules.CheckDateLimit(text, From, To) is null);
    }

    [TestMethod]
    [DataRow(true, false, "K")]
    [DataRow(false, true, "V")]
    [DataRow(true, true, "P")]
    [DataRow(false, false, null)]
    public void Smerovanie_PodlaVyplnenychCastiTrasy(bool hasFrom, bool hasTo, string? symbol)
    {
        Assert.AreSame(symbol is null ? null : Routing.Parse(symbol), TrainRules.RoutingOf(hasFrom, hasTo));
    }

    [TestMethod]
    public void Vlak_BezTrasy_JeChyba()
    {
        var draft = Draft();
        draft.RouteFrom.Clear();
        draft.RouteTo.Clear();

        CollectionAssert.AreEqual(new[] { TrainRules.Field.Route }, Errors(draft));
    }

    [TestMethod]
    public void CasPrichodu_ZlyFormat_JeChyba_BezTrasyZoSmeruSaNekontroluje()
    {
        var draft = Draft();
        draft.ArrivalText = "25:99";
        CollectionAssert.AreEqual(new[] { TrainRules.Field.Arrival }, Errors(draft));

        draft.RouteFrom.Clear();
        Assert.IsEmpty(Errors(draft));
    }

    [TestMethod]
    public void CasOdchodu_NevyplnenaMaska_JeChyba()
    {
        var draft = Draft();
        draft.DepartureText = "  :";

        CollectionAssert.AreEqual(new[] { TrainRules.Field.Departure }, Errors(draft));
    }

    [TestMethod]
    public void Radenia_ChybyRadeniaSuChybamiVlakuSPoradim()
    {
        var draft = Draft();
        draft.Radenia.Items.Add(new Radenie { Validity = new ValidityPeriod(From, To), DatObm = "", Text = "A", Sounds = [new FyzSound()] });
        draft.Radenia.Items.Add(new Radenie { Validity = new ValidityPeriod(From, To), DatObm = "", Text = "" });

        var problem = TrainRules.Check(draft, Context()).Single();

        Assert.IsTrue(problem is { Field: TrainRules.Field.Radenie, IsWarning: false, Row: 1 });
        StringAssert.StartsWith(problem.Message, "2.");
    }

    [TestMethod]
    public void Trasa_SoStanicouGrafikonu_JeUpozornenie()
    {
        var draft = Draft();
        draft.RouteTo.Add(new Station("7", "Dolné Mesto"));

        Assert.IsEmpty(TrainRules.Check(draft, Context()), "bez znamej stanice grafikonu sa nekontroluje");

        var problem = TrainRules.Check(draft, Context() with { HomeStationId = "7" }).Single();
        Assert.IsTrue(problem is { Field: TrainRules.Field.Route, IsWarning: true });
    }

    [TestMethod]
    public void OdchodSkorAkoPrichod_JeLenUpozornenie()
    {
        var draft = Draft();
        draft.ArrivalText = "23:50";
        draft.DepartureText = "0:10";

        Assert.IsEmpty(Errors(draft));
        CollectionAssert.AreEqual(new[] { TrainRules.Field.Departure }, Warnings(draft));
    }

    [TestMethod]
    public void Vlak_BezKolajeTypuADopravcu_SuChyby()
    {
        var draft = Draft();
        draft.Track = null;
        draft.Type = null;
        draft.Operator = null;

        CollectionAssert.AreEqual(new[] { TrainRules.Field.Type, TrainRules.Field.Operator, TrainRules.Field.Track }, Errors(draft));
    }

    [TestMethod]
    public void Platnost_ZaciatokPoKonci_JeChyba_ObmedzenieSaUzNekontroluje()
    {
        var draft = Draft();
        draft.ValidFrom = To;
        draft.ValidTo = From;
        draft.DateLimitText = "ide 20.XII.-2.I.";

        CollectionAssert.AreEqual(new[] { TrainRules.Field.Validity }, Errors(draft));
    }

    [TestMethod]
    public void DatumoveObmedzenie_Necitatelne_JeChyba()
    {
        var draft = Draft();
        draft.DateLimitText = "ide 24.XII.2030";

        CollectionAssert.AreEqual(new[] { TrainRules.Field.DateLimit }, Errors(draft));
    }

    [TestMethod]
    public void Dodatok_BezHlaseni_JeUpozornenieSPoradim()
    {
        var draft = Draft();
        var prichod = new ReportType("P", "Príchod", "P");
        draft.Doplnky.Add(new Dodatok
        {
            Name = "1",
            ChosenReports = [new ChosenReportType { Type = prichod, Variants = [ReportVariant.GetDefaultValues()[0]] }]
        });
        draft.Doplnky.Add(new Dodatok { Name = "2", ChosenReports = [new ChosenReportType { Type = prichod }] });

        var problem = TrainRules.Check(draft, Context()).Single();

        Assert.AreEqual(TrainRules.Field.Dodatok, problem.Field);
        Assert.IsTrue(problem.IsWarning);
        Assert.AreEqual(1, problem.Row);
    }

    [TestMethod]
    public void Varianty_OstatneVlakySRovnakymCislomNazvomATypom_BezUpravovanehoRiadku()
    {
        var edited = Variant(1, "ide v 1-5");
        var other = Variant(2, "ide v 6,7");
        List<Train> trains = [edited, Variant(1, "", "3002"), Variant(1, "", type: R), other];

        CollectionAssert.AreEqual(new[] { other }, TrainVariants.Others(Draft(), Context(trains, row: 0)));
        CollectionAssert.AreEqual(new[] { edited, other }, TrainVariants.Others(Draft(), Context(trains)), "novy vlak a kopia");
    }

    [TestMethod]
    public void Varianty_PrekrytieObmedzenia_JeUpozornenieSoSpolocnymiDnami()
    {
        var draft = Draft();
        draft.Variant = 2;
        draft.DateLimitText = "ide v 1-6";
        List<Train> trains = [Variant(1, "ide v 6,7"), Variant(3, "ide v 7")];

        var overlaps = TrainVariants.Overlaps(draft, trains);
        var problems = TrainRules.Check(draft, Context(trains));

        Assert.HasCount(1, overlaps);
        Assert.AreSame(trains[0], overlaps[0].Train);
        Assert.AreEqual("ide v 6", overlaps[0].Days);
        Assert.IsTrue(problems.Single() is { Field: TrainRules.Field.DateLimit, IsWarning: true });
        StringAssert.Contains(problems[0].Message, "Os 3001");
        StringAssert.Contains(problems[0].Message, "1/3", "poradie varianty v skupine, nie jej cislo");
    }

    [TestMethod]
    public void Varianty_InaPlatnostAleboNecitatelneObmedzenie_NieJePrekrytie()
    {
        var otherPeriod = Variant(1, "ide v 1-5");
        otherPeriod.KoniecPlatnosti = To.AddDays(-1);

        Assert.IsEmpty(TrainVariants.Overlaps(Draft(), [otherPeriod, Variant(2, "ide 20.XII.-2.I.")]));
    }

    [TestMethod]
    public void Varianty_ObmedzenieInejVariantyZmeneneVOkne_PlatiPrePrekrytieAZapiseSaAzPoOK()
    {
        var draft = Draft();
        draft.Variant = 2;
        draft.DateLimitText = "ide v 1-6";
        var other = Variant(1, "ide v 6,7");

        var proposal = TrainVariants.WithoutCommonDays(draft, other);
        Assert.AreEqual("ide v 7", proposal);

        draft.VariantLimits[other] = proposal!;
        Assert.IsEmpty(TrainVariants.Overlaps(draft, [other]));
        Assert.AreEqual("ide v 6,7", other.DateLimitText, "vlak sa meni az pri ulozeni");

        draft.ApplyVariantLimits();
        Assert.AreEqual("ide v 7", other.DateLimitText);
    }

    [TestMethod]
    public void TypyHlaseni_PodlaSmerovania_ZmenaOdstraniNeplatneZDodatkov()
    {
        var both = new ReportType("A", "A", "A");
        var baseOnly = new ReportType("B", "B", "B", true, false, false);
        var endOnly = new ReportType("C", "C", "C", false, false, true);
        List<ReportType> all = [both, baseOnly, endOnly];

        CollectionAssert.AreEqual(new[] { both, baseOnly }, TrainRules.ReportTypesFor(Routing.Vychadzajuci, all));
        CollectionAssert.AreEqual(new[] { both }, TrainRules.ReportTypesFor(Routing.Prechadzajuci, all));
        CollectionAssert.AreEqual(new[] { both, endOnly }, TrainRules.ReportTypesFor(Routing.Konciaci, all));
        Assert.IsEmpty(TrainRules.ReportTypesFor(null, all));

        var dodatok = new Dodatok
        {
            ChosenReports = [new ChosenReportType { Type = both }, new ChosenReportType { Type = baseOnly }, new ChosenReportType { Type = endOnly }]
        };
        TrainRules.PruneReports([dodatok], TrainRules.ReportTypesFor(Routing.Konciaci, all));

        CollectionAssert.AreEqual(new[] { both, endOnly }, dodatok.ChosenReports.Select(c => c.Type).ToArray());
    }

    [TestMethod]
    public void Chyby_PoradiePodlaPoliOkna()
    {
        var draft = Draft();
        draft.Number = "";
        draft.ArrivalText = "";
        draft.Track = null;

        var problems = TrainRules.Check(draft, Context());

        CollectionAssert.AreEqual(new[] { TrainRules.Field.Number, TrainRules.Field.Arrival, TrainRules.Field.Track },
            problems.Select(p => p.Field).ToArray());
        Assert.AreEqual(Resources.FEditTrain_bSave_Click_Zadajte_číslo_vlaku, problems[0].Message);
    }
}
