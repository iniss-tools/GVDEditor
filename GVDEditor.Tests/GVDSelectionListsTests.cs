using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Comboboxy Stanica a Obdobie v hlavnom okne po zmene stanice alebo obdobia grafikonu v lokalnych nastaveniach
///     (Lokalne nastavenia → Grafikon).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GVDSelectionListsTests
{
    private static GVDDirectory NewDir(string station, int year) => new(
        new DirList { DirName = station + year, FullPath = @"C:\INISS\DATA\" + station + year },
        new GVDInfo
        {
            ThisStation = new Station("1", station),
            StartValidTimeTable = new DateTime(year, 12, 10),
            EndValidTimeTable = new DateTime(year + 1, 12, 9),
        });

    [TestMethod]
    public void Obdobie_PocitaSaZAktualnychDatumov()
    {
        var dir = NewDir("Horna", 2024);
        Assert.AreEqual("2024/2025", dir.Period);

        dir.GVD.StartValidTimeTable = new DateTime(2025, 12, 14);
        dir.GVD.EndValidTimeTable = new DateTime(2026, 12, 12);

        Assert.AreEqual("2025/2026", dir.Period);
        Assert.AreEqual("2025/2026", dir.ToString());
        Assert.AreEqual("Horna 2025/2026", dir.PeriodFormatted);
    }

    [TestMethod]
    public void PremenovanieStanice_NepouzivanyNazovNahradiNaRovnakomMieste()
    {
        var dir = NewDir("Horna", 2024);
        List<GVDDirectory> dirs = [NewDir("Dolna", 2024), dir];
        List<string> stanice = ["Dolna", "Horna", "Stredna"];

        dir.GVD.ThisStation = new Station("2", "Nova");
        GVDSelectionLists.RenameStation(stanice, dirs, "Horna", "Nova");

        CollectionAssert.AreEqual(new[] { "Dolna", "Nova", "Stredna" }, stanice);
    }

    [TestMethod]
    public void PremenovanieStanice_PouzivanyNazovPonechaANovyPrida()
    {
        var dir = NewDir("Horna", 2024);
        List<GVDDirectory> dirs = [NewDir("Horna", 2023), dir];
        List<string> stanice = ["Horna"];

        dir.GVD.ThisStation = new Station("2", "Nova");
        GVDSelectionLists.RenameStation(stanice, dirs, "Horna", "Nova");

        CollectionAssert.AreEqual(new[] { "Horna", "Nova" }, stanice);
        CollectionAssert.AreEqual(new[] { dir }, GVDSelectionLists.PeriodsOf(dirs, "Nova").ToList());
        Assert.AreEqual(1, GVDSelectionLists.PeriodsOf(dirs, "Horna").Count());
    }

    [TestMethod]
    public void PremenovanieStanice_NaExistujucuNazovNezdvoji()
    {
        var dir = NewDir("Horna", 2024);
        var dolna = NewDir("Dolna", 2023);
        List<GVDDirectory> dirs = [dolna, dir];
        List<string> stanice = ["Dolna", "Horna"];

        dir.GVD.ThisStation = new Station("3", "Dolna");
        GVDSelectionLists.RenameStation(stanice, dirs, "Horna", "Dolna");

        CollectionAssert.AreEqual(new[] { "Dolna" }, stanice);
        CollectionAssert.AreEqual(new[] { dolna, dir }, GVDSelectionLists.PeriodsOf(dirs, "Dolna").ToList());
    }

    [TestMethod]
    public void PremenovanieStanice_RovnakyNazovNemeniZoznam()
    {
        List<string> stanice = ["Horna"];

        GVDSelectionLists.RenameStation(stanice, [NewDir("Horna", 2024)], "Horna", "Horna");

        CollectionAssert.AreEqual(new[] { "Horna" }, stanice);
    }
}
