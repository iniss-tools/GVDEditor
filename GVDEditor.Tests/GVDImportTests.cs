using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Import grafikonu (Subor → Importovat → Grafikon…) - kontroly pred skopirovanim priecinka do DATA.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GVDImportTests
{
    private string _root = null!;
    private string _data = null!;

    [TestInitialize]
    public void Init()
    {
        _root = Path.Combine(Path.GetTempPath(), "GVDImportTests_" + Guid.NewGuid().ToString("N"));
        _data = Path.Combine(_root, "INISS", "DATA");
        Directory.CreateDirectory(Path.Combine(_data, "Dolne.2026"));
        Directory.CreateDirectory(Path.Combine(_root, "Zaloha", "Horna.2026"));
        Directory.CreateDirectory(Path.Combine(_root, "Zaloha", "Dolne.2026"));
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_root, true);

    private static GVDInfo Gvd(string station, int year) => new()
    {
        ThisStation = new Station("99", station),
        StartValidTimeTable = new DateTime(year, 12, 13),
        EndValidTimeTable = new DateTime(year + 1, 12, 11)
    };

    private GVDDirectory[] Existing() =>
    [
        new(new DirList { DirName = "Dolne.2026", FullPath = Path.Combine(_data, "Dolne.2026") }, Gvd("Dolné Mesto", 2026))
    ];

    [TestMethod]
    public void Import_NovyGrafikon_CielVData()
    {
        var error = GVDImport.Check(Path.Combine(_root, "Zaloha", "Horna.2026"), _data, Gvd("Horné Mesto", 2026), Existing(), out var target);

        Assert.IsNull(error);
        Assert.AreEqual(Path.Combine(_data, "Horna.2026"), target);
    }

    [TestMethod]
    public void Import_PriecinokUzVData_SaLenZapise()
    {
        var inData = Path.Combine(_data, "Horna.2026");
        Directory.CreateDirectory(inData);

        Assert.IsNull(GVDImport.Check(inData, _data, Gvd("Horné Mesto", 2026), Existing(), out var target));
        Assert.AreEqual(inData, target);
    }

    [TestMethod]
    public void Import_RovnakyNazovPriecinka_JeChyba()
    {
        Assert.IsNotNull(GVDImport.Check(Path.Combine(_root, "Zaloha", "Dolne.2026"), _data, Gvd("Horné Mesto", 2030), Existing(), out _));
    }

    [TestMethod]
    public void Import_PrekryteObdobieTejIstejStanice_JeChyba()
    {
        Assert.IsNotNull(GVDImport.Check(Path.Combine(_root, "Zaloha", "Horna.2026"), _data, Gvd("Dolné Mesto", 2026), Existing(), out _));
    }

    [TestMethod]
    public void Import_DalsieObdobieTejIstejStanice_Prejde()
    {
        Assert.IsNull(GVDImport.Check(Path.Combine(_root, "Zaloha", "Horna.2026"), _data, Gvd("Dolné Mesto", 2027), Existing(), out _));
    }

    [TestMethod]
    public void Import_PriecinokObsahujuciData_JeChyba()
    {
        Assert.IsNotNull(GVDImport.Check(Path.Combine(_root, "INISS"), Path.Combine(_root, "INISS", "INISS", "DATA"),
            Gvd("Horné Mesto", 2026), Existing(), out _));
    }
}
