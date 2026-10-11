using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;
using Iniss.Elis;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Poznamky vlakov z ELIS - ktore platia v stanici grafikonu a co z nich import nastavi.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ElisRemarksTests
{
    private static ElisTrain Vlak(string[] before, string[] after, params string[] remarks) => new()
    {
        StationsBefore = before.Select(name => new ElisStop { Name = name }).ToList(),
        StationsAfter = after.Select(name => new ElisStop { Name = name }).ToList(),
        Remarks = [.. remarks]
    };

    [TestMethod]
    public void Usek_PlatiLenVStaniciNaUseku()
    {
        var train = Vlak(["Púchov", "Lúky p.Makytou"], ["Horní Lideč", "Vsetín", "Praha hl.n."],
            "náhradná autobusová doprava (Púchov->Vsetín)",
            "zastávky NAD: Horní Lideč aut.zast.u žel.st. (Horní Lideč->Praha hl.n.)",
            "povinne miestenkový vlak (Český Těšín (Czeski Cieszyn)->Praha hl.n.)");

        CollectionAssert.AreEqual(new[] { "náhradná autobusová doprava" }, ElisRemarks.AtStation(train, "Žilina"));
    }

    [TestMethod]
    public void Usek_DvojjazycneNazvyAStanicaNaKoncoch()
    {
        var train = Vlak(["Návsí (Nawsie)", "Český Těšín (Czeski Cieszyn)"], ["Čadca"],
            "povinne miestenkový vlak (Český Těšín (Czeski Cieszyn)->Žilina)",
            "do označených vozňov možnosť zakúpenia miestenky (Žilina->Čadca)");

        CollectionAssert.AreEqual(new[] { "povinne miestenkový vlak", "do označených vozňov možnosť zakúpenia miestenky" },
            ElisRemarks.AtStation(train, "Žilina"));
    }

    [TestMethod]
    public void Zatvorka_StanicaAleboCastTextu()
    {
        var train = Vlak(["Vrútky"], ["Čadca"],
            "vlak čaká na prípoj 3322 (Žilina)",
            "vlak čaká na prípoj 1 (Čadca)",
            "dámske kupé (kupé pre samostatne cestujúce ženy)",
            "vlak nečaká na prípoje",
            "vlak nečaká na prípoje");

        CollectionAssert.AreEqual(new[] { "vlak čaká na prípoj 3322", "dámske kupé (kupé pre samostatne cestujúce ženy)", "vlak nečaká na prípoje" },
            ElisRemarks.AtStation(train, "Žilina"));
    }

    [TestMethod]
    public void Informacie_OPrestupochAHraniciachSaVynechaju()
    {
        var train = Vlak(["Prešov"], ["Čadca"],
            "nie je prípoj medzi vlakmi 7801 a 6402",
            "zo stanice Prešov (odch. 15:17) vlak Os 1960",
            "zo stanice Košice (odch. 21:36) č.vlaku 1020",
            "pohraničný prechodový bod [SK/CZ]: Čadca Gr.",
            "v úseku Lipany – Prešov je vlak vedený ako Os",
            "není přípoj mezi vlaky 1 a 2",
            "zo stanice odchádza vlak náhradnej dopravy",
            "reštauračný alebo bistro vozeň");

        CollectionAssert.AreEqual(new[] { "zo stanice odchádza vlak náhradnej dopravy", "reštauračný alebo bistro vozeň" },
            ElisRemarks.AtStation(train, "Žilina"));
    }

    [TestMethod]
    [DataRow("povinne miestenkový vlak, cestujúci je povinný zakúpiť si cestovný lístok s miestom", ElisTrainFlag.Reservation)]
    [DataRow("povinně místenkový vlak", ElisTrainFlag.Reservation)]
    [DataRow("povinná rezervace míst - nutno zakoupit místenku", ElisTrainFlag.Reservation)]
    [DataRow("povinne miestenkový vozeň 1. triedy", ElisTrainFlag.None)]
    [DataRow("vo vlaku je radený bezbariérový vozeň so zníženou podlahou a nástupnou rampou", ElisTrainFlag.LowFloor)]
    [DataRow("ve vlaku je řazen vůz se sníženou podlahou", ElisTrainFlag.LowFloor)]
    [DataRow("vlak vezie len lôžkové a ležadlové vozne", ElisTrainFlag.SleeperOnly)]
    [DataRow("reštauračný alebo bistro vozeň", ElisTrainFlag.None)]
    public void Navrh_PriznakPodlaTextu(string remark, ElisTrainFlag flag)
    {
        var action = ElisRemarks.Suggest(remark);

        Assert.AreEqual(flag, action.Flag);
        Assert.AreEqual("", action.Addition);
        Assert.AreEqual(0, action.Lockout);
    }

    [TestMethod]
    public void Pouzitie_PriznakyDodatokRazAVylukaLenAkZiadnaNie()
    {
        var train = new Train { Number = "1", LockoutNumber = 0 };
        var actions = new Dictionary<string, ElisRemarkAction>
        {
            ["a"] = new(ElisTrainFlag.Reservation, "1003", 1000002),
            ["b"] = new(ElisTrainFlag.LowFloor, "1003", 1000003),
            ["c"] = new(ElisTrainFlag.SleeperOnly, "9999", 0)
        };
        var created = new List<string>();

        ElisRemarks.Apply(train, ["a", "b", "c", "neznama"], actions, (_, code) =>
        {
            created.Add(code);
            return code == "9999" ? null : new Dodatok { Name = code, ChosenReports = [] };
        });

        Assert.IsTrue(train.IsMiestenkovy);
        Assert.IsTrue(train.IsNizkopodlazny);
        Assert.IsTrue(train.IsIbaLozkovy);
        Assert.AreEqual(1000002, train.LockoutNumber);
        CollectionAssert.AreEqual(new[] { "1003" }, train.Doplnky.Select(d => d.Name).ToArray());
        CollectionAssert.AreEqual(new[] { "1003", "9999" }, created, "dodatok, ktory vlak uz ma, sa nevytvara znova");
    }
}
