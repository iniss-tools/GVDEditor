using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Volby importu z ELIS ulozene v grafikone (ElisKrat.txt, ElisPozn.txt).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ElisChoicesFileTests
{
    private static readonly GVDInfo Gvd = new()
    {
        ThisStation = new Station("5613600", "Žilina"),
        StartValidTimeTable = new DateOnly(2025, 12, 14),
        EndValidTimeTable = new DateOnly(2026, 12, 12)
    };

    private string _dir = null!;

    [TestInitialize]
    public void Init() => _dir = Directory.CreateTempSubdirectory("eliskrat").FullName;

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, true);

    [TestMethod]
    public void BezSuborov_PrazdneVolby()
    {
        Assert.IsEmpty(ElisChoicesFile.ReadShort(_dir));
        Assert.IsEmpty(ElisChoicesFile.ReadRemarks(_dir));
    }

    [TestMethod]
    public void KratkeHlasenie_ZapisANacitanie()
    {
        var stations = new StationDirectory([new Station("5617915", "Čadca"), new Station("5613800", "Vrútky")], []);
        var choices = new Dictionary<string, bool> { ["5617915"] = true, ["5613800"] = false, ["9000001"] = true };

        ElisChoicesFile.WriteShort(_dir, choices, stations, Gvd, AppLanguage.Slovak);
        var read = ElisChoicesFile.ReadShort(_dir);

        CollectionAssert.AreEquivalent(choices, read);
        StringAssert.Contains(File.ReadAllText(Path.Combine(_dir, "ELISKRAT.txt"), Encodings.Win1250), "\"Čadca\"");
    }

    [TestMethod]
    public void Poznamky_ZapisANacitanieVratanePoznamokBezUcinku()
    {
        var remarks = new Dictionary<string, ElisRemarkAction>
        {
            ["povinne miestenkový vlak, cestujúci je povinný zakúpiť si cestovný lístok s miestom"] = new(ElisTrainFlag.Reservation, "", 0),
            ["reštauračný alebo bistro vozeň"] = new(ElisTrainFlag.None, "1003", 0),
            ["náhradná autobusová doprava"] = new(ElisTrainFlag.None, "", 1000002),
            ["bezbariérový vozeň so zníženou podlahou, \"rampa\""] = new(ElisTrainFlag.LowFloor, "", 0),
            ["lôžkový vozeň"] = new(ElisTrainFlag.SleeperOnly, "2001", 0),
            ["tichý oddiel"] = ElisRemarkAction.Nothing
        };

        ElisChoicesFile.WriteRemarks(_dir, remarks, Gvd, AppLanguage.Czech);
        var read = ElisChoicesFile.ReadRemarks(_dir);

        Assert.HasCount(remarks.Count, read);
        foreach (var (text, action) in remarks)
            Assert.AreEqual(action, read[text], text);
    }
}
