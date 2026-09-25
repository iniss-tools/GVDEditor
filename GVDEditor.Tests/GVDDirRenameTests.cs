using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Premenovanie priecinka grafikonu (Lokalne nastavenia → Grafikon → Zmenit, vykona sa az tlacidlom Ulozit).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GVDDirRenameTests
{
    private string _dataDir = null!;

    [TestInitialize]
    public void Init()
    {
        _dataDir = Path.Combine(Path.GetTempPath(), "GVDDirRenameTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(_dataDir, "Horna.2024"));
        Directory.CreateDirectory(Path.Combine(_dataDir, "Dolna.2024"));
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dataDir, true);

    private string PathOf(string name) => _dataDir + Path.DirectorySeparatorChar + name;

    [TestMethod]
    public void Overenie_PlatnyNazovVratiNovuCestu()
    {
        Assert.IsNull(GVDDirRename.Validate("Horna.2025", PathOf("Horna.2024"), _dataDir, out var fullPath));
        Assert.AreEqual(PathOf("Horna.2025"), fullPath);
    }

    [TestMethod]
    public void Overenie_OdmietnePrazdnyNepovolenyZnakABodkuNaKonci()
    {
        Assert.IsNotNull(GVDDirRename.Validate("", PathOf("Horna.2024"), _dataDir, out _));
        Assert.IsNotNull(GVDDirRename.Validate("Hor|na", PathOf("Horna.2024"), _dataDir, out _));
        Assert.IsNotNull(GVDDirRename.Validate("Horna.", PathOf("Horna.2024"), _dataDir, out _));
    }

    [TestMethod]
    public void Overenie_ExistujuciPriecinokOdmietneZmenuVelkostiPismenPovoli()
    {
        Assert.IsNotNull(GVDDirRename.Validate("Dolna.2024", PathOf("Horna.2024"), _dataDir, out _));
        Assert.IsNull(GVDDirRename.Validate("HORNA.2024", PathOf("Horna.2024"), _dataDir, out _));
        Assert.IsNull(GVDDirRename.Validate("Horna.2024", PathOf("Horna.2024"), _dataDir, out _));
    }

    [TestMethod]
    public void PrepisZaznamov_UpraviAjInuInstanciuAOstatneNecha()
    {
        var dir = new DirList { DirName = "Horna.2024", FullPath = PathOf("Horna.2024"), Flags = "K" };
        var kopia = new DirList { DirName = "Horna.2024", FullPath = PathOf("horna.2024"), Flags = "K" };
        var ina = new DirList { DirName = "Dolna.2024", FullPath = PathOf("Dolna.2024") };
        List<DirList> all = [ina, kopia];

        GVDDirRename.UpdateEntries(dir, all, "Horna.2025", PathOf("Horna.2025"));

        Assert.AreEqual("Horna.2025", dir.DirName);
        Assert.AreEqual(PathOf("Horna.2025"), dir.FullPath);
        Assert.AreEqual("Horna.2025", kopia.DirName);
        Assert.AreEqual(PathOf("Horna.2025"), kopia.FullPath);
        Assert.AreEqual("K", kopia.Flags);
        Assert.AreEqual("Dolna.2024", ina.DirName);
        Assert.AreEqual(2, all.Count);
    }
}
