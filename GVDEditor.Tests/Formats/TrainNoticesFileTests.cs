using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Vyluky a odklony obsluhy INISS 3.39 (Vyluky1.txt, Odklony1.txt).
/// </summary>
[TestClass]
public class TrainNoticesFileTests
{
    private string _dataDir = null!;

    [TestInitialize]
    public void Init() => _dataDir = Directory.CreateTempSubdirectory("gvdnotices").FullName;

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dataDir, true);

    [TestMethod]
    public void Citanie_StanicaObdobieADni()
    {
        File.WriteAllText(Path.Combine(_dataDir, TrainNoticesFile.FileLockouts), """
            ; vyluky obsluhy
            "603",5400010,1000004,27.7.2030,27.7.2030
            "111",,1,05.08.2030,6.8.2030,"ide v 6,7"
            """, Encodings.Win1250);

        var notices = TrainNoticesFile.Read(_dataDir, TrainNoticesFile.FileLockouts);

        Assert.AreEqual(new TrainNotice("603", 5400010, 1000004, new DateOnly(2030, 7, 27), new DateOnly(2030, 7, 27), ""), notices[0]);
        Assert.AreEqual(new TrainNotice("111", 0, 1, new DateOnly(2030, 8, 5), new DateOnly(2030, 8, 6), "ide v 6,7"), notices[1]);
    }

    [TestMethod]
    public void Citanie_ChybnyRiadokAObdobieOpacneSaPreskocia()
    {
        File.WriteAllText(Path.Combine(_dataDir, TrainNoticesFile.FileDiversions), """
            "603",X,1,27.7.2030,27.7.2030
            "604",,1,28.7.2030,27.7.2030
            "605",,1,27.7.2030,28.7.2030
            """, Encodings.Win1250);

        var notices = TrainNoticesFile.Read(_dataDir, TrainNoticesFile.FileDiversions);

        Assert.AreEqual("605", notices.Single().Number);
    }

    [TestMethod]
    public void Citanie_ChybajuciSuborJePrazdny()
    {
        Assert.IsEmpty(TrainNoticesFile.Read(_dataDir, TrainNoticesFile.FileLockouts));
    }
}
