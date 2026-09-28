using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using ToolsCore.Entities;

namespace GVDEditor.Tests.Domain.Editing;

/// <summary>
/// Koncept vlaku v okne vlaku: udaje prejdu do vlaku bez straty, okno meni len kopie a kopia vlaku
/// nezdiela zoznamy so zdrojovym vlakom.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainDraftTests
{
    private static readonly TrainType Ex = new("Ex");
    private static readonly Operator Zssk = new(1, "ZSSK");
    private static readonly Track Track1 = new("1", "1", "Koľaj 1", Platform.None, "", "1");
    private static readonly Track Track2 = new("2", "2", "Koľaj 2", Platform.None, "", "2");
    private static readonly FyzLanguage Basic = new("SK", "Slovenčina") { IsBasic = true };
    private static readonly FyzLanguage English = new("EN", "Angličtina");
    private static readonly ReportType Prichod = new("P", "Príchod", "P");
    private static readonly List<ReportVariant> Variants = ReportVariant.GetDefaultValues();

    private static Train FullTrain()
    {
        var train = new Train
        {
            Number = "521",
            Type = Ex,
            Name = "Lipovan",
            Operator = Zssk,
            Arrival = new TimeOnly(10, 12),
            Departure = new TimeOnly(10, 15),
            Routing = Routing.Prechadzajuci,
            Track = Track1,
            TrackDeparture = Track2,
            LineArrival = "S20",
            LineDeparture = "R1",
            DateLimitText = "ide v 1-5",
            ZaciatokPlatnosti = new DateOnly(2026, 12, 13),
            KoniecPlatnosti = new DateOnly(2027, 12, 11),
            Variant = 2,
            LockoutNumber = 57,
            IsMedzistatny = true,
            IsMiestenkovy = true,
            IsNizkopodlazny = true,
            IsMotorovy = true,
            IsPriznakO = true,
            Languages = [Basic, English],
            Doplnky =
            [
                new Dodatok
                {
                    Sound = new FyzSound { Name = "D12" }, Name = "12",
                    ChosenReports = [new ChosenReportType { Type = Prichod, Variants = [Variants[0]] }]
                }
            ]
        };
        train.StaniceZoSmeru.AddRange([new Station("1", "Veľká Ves", IsInLongReport: true), new Station("2", "Horné Lúky")]);
        train.StaniceDoSmeru.Add(new Station("3", "Brezová Dolina", true, true));
        return train;
    }

    [TestMethod]
    public void Koncept_RoundTrip_ZachovaVsetkyUdaje()
    {
        var source = FullTrain();
        var copy = new Train();

        TrainDraft.From(source).ApplyTo(copy);
        copy.Variant = source.Variant;

        Assert.AreEqual(source.Number, copy.Number);
        Assert.AreSame(source.Type, copy.Type);
        Assert.AreEqual(source.Name, copy.Name);
        Assert.AreEqual(source.Operator, copy.Operator);
        Assert.AreEqual(source.Arrival, copy.Arrival);
        Assert.AreEqual(source.Departure, copy.Departure);
        Assert.AreSame(Routing.Prechadzajuci, copy.Routing);
        Assert.AreEqual(Track1, copy.Track);
        Assert.AreEqual(Track2, copy.TrackDeparture);
        Assert.AreEqual("S20", copy.LineArrival);
        Assert.AreEqual("R1", copy.LineDeparture);
        Assert.AreEqual(source.DateLimitText, copy.DateLimitText);
        Assert.AreEqual(source.ZaciatokPlatnosti, copy.ZaciatokPlatnosti);
        Assert.AreEqual(source.KoniecPlatnosti, copy.KoniecPlatnosti);
        Assert.AreEqual(57, copy.LockoutNumber);
        Assert.IsTrue(copy is { IsMedzistatny: true, IsMiestenkovy: true, IsNizkopodlazny: true, IsMotorovy: true, IsPriznakO: true });
        Assert.IsTrue(copy is { IsMimoriadny: false, IsDialkovy: false, IsPrestupovy: false, IsIbaLozkovy: false });
        CollectionAssert.AreEqual(source.StaniceZoSmeru, copy.StaniceZoSmeru);
        CollectionAssert.AreEqual(source.StaniceDoSmeru, copy.StaniceDoSmeru);
        Assert.AreEqual("Veľká Ves", copy.StartingStation!.Name);
        Assert.AreEqual("Brezová Dolina", copy.EndingStation!.Name);
        CollectionAssert.AreEqual(new[] { English }, copy.Languages);
        Assert.HasCount(1, copy.Doplnky);
        Assert.AreEqual("12", copy.Doplnky[0].Name);
        CollectionAssert.AreEqual(new[] { Variants[0] }, copy.Doplnky[0].ChosenReports[0].Variants);
    }

    [TestMethod]
    public void Koncept_ZmenyVKoncepte_NemeniaVlak()
    {
        var train = FullTrain();
        var draft = TrainDraft.From(train);

        draft.RouteFrom[1].IsInShortReport = true;
        draft.RouteTo.Clear();
        draft.Doplnky[0].ChosenReports[0].Variants.Add(Variants[1]);
        draft.Languages.Clear();
        draft.Number = "999";

        Assert.IsFalse(train.StaniceZoSmeru[1].IsInShortReport);
        Assert.HasCount(1, train.StaniceDoSmeru);
        CollectionAssert.AreEqual(new[] { Variants[0] }, train.Doplnky[0].ChosenReports[0].Variants);
        Assert.HasCount(2, train.Languages);
        Assert.AreEqual("521", train.Number);
    }

    [TestMethod]
    public void Koncept_KopiaVlaku_NezdielaObjektySoZdrojom()
    {
        var source = FullTrain();
        var copy = new Train();
        TrainDraft.From(source).ApplyTo(copy);

        Assert.AreNotSame(source.StaniceZoSmeru[0], copy.StaniceZoSmeru[0]);
        Assert.AreNotSame(source.Doplnky[0], copy.Doplnky[0]);
        Assert.AreNotSame(source.Languages, copy.Languages);

        copy.StaniceZoSmeru[0].IsInShortReport = true;
        Assert.IsFalse(source.StaniceZoSmeru[0].IsInShortReport);
    }

    [TestMethod]
    public void Koncept_KolajOdchoduRovnakaAkoPrichodu_NezapiseSa()
    {
        var train = FullTrain();
        train.TrackDeparture = null;

        var draft = TrainDraft.From(train);
        Assert.AreEqual(Track1, draft.TrackDeparture, "ponuka ukazuje kolaj prichodu");

        draft.TrackDeparture = Track1;
        draft.ApplyTo(train);
        Assert.IsNull(train.TrackDeparture);
    }

    [TestMethod]
    public void Koncept_CasBezTrasy_SaNezapise()
    {
        var train = FullTrain();
        var draft = TrainDraft.From(train);
        draft.RouteTo.Clear();

        draft.ApplyTo(train);

        Assert.IsNull(train.Departure);
        Assert.IsNull(train.EndingStation);
        Assert.AreSame(Routing.Konciaci, train.Routing);
    }

    [TestMethod]
    public void Koncept_PrazdnaLinka_JeBezLinky()
    {
        var train = FullTrain();
        train.LineArrival = null;

        var draft = TrainDraft.From(train);
        Assert.AreEqual("", draft.LineArrival);

        draft.ApplyTo(train);
        Assert.AreEqual("", train.LineArrival);
    }

    [TestMethod]
    public void Koncept_NovyVlak_MaPlatnostGrafikonu()
    {
        var draft = TrainDraft.New(new DateOnly(2026, 12, 13), new DateOnly(2027, 12, 11));

        Assert.AreEqual(new DateOnly(2026, 12, 13), draft.ValidFrom);
        Assert.AreEqual(new DateOnly(2027, 12, 11), draft.ValidTo);
        Assert.AreEqual(-1, draft.Variant);
        Assert.IsNull(draft.Routing);
    }

    [TestMethod]
    public void Koncept_BezTrasy_SaNedaZapisat()
    {
        var draft = TrainDraft.From(FullTrain());
        draft.RouteFrom.Clear();
        draft.RouteTo.Clear();

        Assert.ThrowsExactly<InvalidOperationException>(() => draft.ApplyTo(new Train()));
    }
}
