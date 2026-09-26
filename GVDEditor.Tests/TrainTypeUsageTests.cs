using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Pouzitie typu vlaku v grafikone (Export3A.TXT) - pred odstranenim alebo premenovanim typu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TrainTypeUsageTests
{
    private string _file = null!;

    [TestInitialize]
    public void Init()
    {
        _file = Path.Combine(Path.GetTempPath(), "TrainTypeUsageTests_" + Guid.NewGuid().ToString("N") + ".txt");
        File.WriteAllLines(_file,
        [
            ";EXPORT3A.txt",
            "/9900100",
            "1,\"521\",\"Lipovan, expres\",\"Ex\",-1,\"P\"",
            "2,\"3601\",\"\",\"Os\",-1,\"V\""
        ], Encodings.Win1250);
    }

    [TestCleanup]
    public void Cleanup() => File.Delete(_file);

    [TestMethod]
    [DataRow("Ex", true)]
    [DataRow("Os", true)]
    [DataRow("R", false)]
    [DataRow("expres", false)]
    public void TypVlaku_PodlaStlpcaDruh(string key, bool used)
    {
        Assert.AreEqual(used, TrainTypeUsage.UsesKey(_file, key));
    }

    [TestMethod]
    public void TypVlaku_ChybajuciSubor_NieJePouzity()
    {
        Assert.IsFalse(TrainTypeUsage.UsesKey(_file + ".chyba", "Ex"));
    }
}
