using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Tests.Domain.Analysis;

/// <summary>
/// Analyza grafikonu: pravidla okna vlaku a stranok nastaveni nad celym grafikonom, hlasene stanice bez nahravky
/// a stavovy diagram.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GrafikonChecksTests
{
    private static readonly TrainType Os = new("Os");
    private static readonly Operator Zssk = new(1, "ZSSK");
    private static readonly Track Track1 = new("1", "1", "Koľaj 1", Platform.None, "", "1");
    private static readonly DateOnly From = new(2026, 12, 13);
    private static readonly DateOnly To = new(2027, 12, 11);

    private string _dir = null!;

    [TestInitialize]
    public void Init()
    {
        _dir = Path.Combine(Path.GetTempPath(), "GrafikonChecksTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, true);

    // prechadzajuci vlak Os 3001 Velka Ves - Brezova Dolina, obe stanice sa hlasia
    private static Train Train(string number = "3001", string arrival = "10:12", string departure = "10:15", Track? track = null)
    {
        var train = new Train
        {
            Number = number,
            Name = "",
            Type = Os,
            Operator = Zssk,
            Track = track ?? Track1,
            Variant = -1,
            Arrival = TimeOnly.Parse(arrival, System.Globalization.CultureInfo.InvariantCulture),
            Departure = TimeOnly.Parse(departure, System.Globalization.CultureInfo.InvariantCulture),
            ZaciatokPlatnosti = From,
            KoniecPlatnosti = To,
            Routing = Routing.Prechadzajuci
        };
        train.StaniceZoSmeru.Add(new Station("1", "Veľká Ves", IsInLongReport: true));
        train.StaniceDoSmeru.Add(new Station("2", "Brezová Dolina", IsInShortReport: true, IsInLongReport: true));
        return train;
    }

    private static AnalysisScope Scope(GrafikonDocument document, params Station[] bank) =>
        new(document, new InissWorkspace { Stations = [.. bank] }, new Host());

    private GVDDirectory Gvd() => new(new DirList { DirName = "G", FullPath = _dir },
        new GVDInfo { ThisStation = new Station("99", "Dolné Mesto"), StartValidTimeTable = From, EndValidTimeTable = To });

    [TestMethod]
    public void Vlak_PlatnyANocnyPobyt_BezProblemov()
    {
        // odchod skor ako prichod = pobyt cez polnoc - okno na to upozornuje, analyza nie
        List<Train> trains = [Train(), Train("3003", "23:50", "00:10")];

        Assert.IsEmpty(GrafikonChecks.CheckTrain(trains, 0, "99"));
        Assert.IsEmpty(GrafikonChecks.CheckTrain(trains, 1, "99"));
    }

    [TestMethod]
    public void Vlak_BezKolajeJeChybaAPrekrytieVariantUpozornenie()
    {
        var withoutTrack = Train("3005");
        withoutTrack.Track = null!;
        // dva vlaky s rovnakym klucom bez datumoveho obmedzenia idu v rovnake dni
        List<Train> trains = [withoutTrack, Train(), Train()];

        var missing = GrafikonChecks.CheckTrain(trains, 0, "99");
        Assert.IsTrue(missing.Any(p => p.Field == TrainRules.Field.Track && !p.IsWarning));

        var overlap = GrafikonChecks.CheckTrain(trains, 1, "99");
        Assert.IsTrue(overlap.Any(p => p.Field == TrainRules.Field.DateLimit && p.IsWarning));
    }

    [TestMethod]
    public void Vlaky_ProblemLenPriVlakochSChybou()
    {
        var document = new GrafikonDocument();
        var bad = Train("3005");
        bad.Track = null!;
        document.Trains.Add(Train());
        document.Trains.Add(bad);

        var problems = GrafikonChecks.Trains(Gvd(), Scope(document)).ToList();

        Assert.HasCount(1, problems);
        Assert.AreEqual(ProblemType.Error, problems[0].ProblemType);
        StringAssert.Contains(problems[0].Text, "3005");
    }

    [TestMethod]
    public void Stanice_HlasenaBezNahravky_ZoskupenaPodlaStanice()
    {
        var other = Train("3003");
        // stanica 1 sa vo vlaku 3003 nehlasi - nepocita sa
        other.StaniceZoSmeru[0] = new Station("1", "Veľká Ves");
        List<Train> trains = [Train(), other];

        var result = GrafikonChecks.StationsWithoutRecording(trains, [new Station("2", "Brezová Dolina")]);

        Assert.HasCount(1, result);
        Assert.AreEqual("1", result[0].Station.ID);
        Assert.HasCount(1, result[0].Trains);
        Assert.AreEqual("3001", result[0].Trains[0].Number);
    }

    [TestMethod]
    public void Stanice_VsetkyVBanke_BezProblemov()
    {
        var result = GrafikonChecks.StationsWithoutRecording([Train(), Train("3003")],
            [new Station("1", "Veľká Ves"), new Station("2", "Brezová Dolina")]);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void Nastavenia_DuplicitnyKlucTabuleALogickaBezZaznamov()
    {
        var document = new GrafikonDocument();
        var catalog = new TableCatalog { Key = "KAT", Name = "Katalóg", Manufacturer = TableManufacturer.Elen };
        document.TableCatalogs.Add(catalog);
        document.TablePhysicals.Add(new TablePhysical { Key = "T1", Name = "Tabuľa 1", TableCatalog = catalog, ID = 1 });
        document.TablePhysicals.Add(new TablePhysical { Key = "T1", Name = "Tabuľa 2", TableCatalog = catalog, ID = 2 });
        document.TableLogicals.Add(new TableLogical { Key = "L1", Name = "Odchody" });

        var problems = GrafikonChecks.Settings(Scope(document)).Where(p => p.ProblemType == ProblemType.Error).ToList();

        Assert.HasCount(2, problems.Where(p => p.Text.StartsWith("Fyzická tabuľa T1", StringComparison.Ordinal)).ToList());
        Assert.IsTrue(problems.Any(p => p.Text.StartsWith("Logická tabuľa L1", StringComparison.Ordinal)));
    }

    [TestMethod]
    public void Nastavenia_DopravcaZiadnySaNekontroluje()
    {
        var document = new GrafikonDocument();
        document.Operators.Add(Operator.None);
        document.Operators.Add(Zssk);

        Assert.IsEmpty(GrafikonChecks.Settings(Scope(document)));
    }

    [TestMethod]
    public void StavovyDiagram_ChybajuciJeChyba()
    {
        var problems = GrafikonChecks.StateDgm(Gvd(), Scope(new GrafikonDocument())).ToList();

        Assert.HasCount(1, problems);
        Assert.IsInstanceOfType<StateDgmMissing>(problems[0]);
        Assert.AreEqual(ProblemType.Error, problems[0].ProblemType);
    }

    [TestMethod]
    public void StavovyDiagram_PoskodenyJeChyba()
    {
        File.WriteAllText(StateDgmFile.PathOf(_dir), "[CATEGORY\nKEY=\"", Encodings.Win1250);

        var problems = GrafikonChecks.StateDgm(Gvd(), Scope(new GrafikonDocument())).ToList();

        Assert.HasCount(1, problems);
        Assert.AreEqual(ProblemType.Error, problems[0].ProblemType);
    }

    private sealed class Host : IAnalyzerHost
    {
        public bool ShowLocalSettings(LocalSettingsPage page = LocalSettingsPage.Grafikon, LocalSettingsAction action = LocalSettingsAction.None,
            object? select = null) => false;

        public void EditTabTab(TableTabTab tabTab)
        {
        }

        public bool EditTrain(Train train) => false;
    }
}
