using System.Diagnostics.CodeAnalysis;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms;

namespace GVDEditor.Tests;

/// <summary>
///     Import dat (Subor → Importovat → Data…): oddelovac buniek, trasa vlaku a stanice v kratkom a dlhom hlaseni.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ImportDataTests
{
    private const string Home = "9900100";

    private static readonly Station Velka = new("9900200", "Veľká Ves");
    private static readonly Station Hranicna = new("9900300", "Hraničná");
    private static readonly Station Sklene = new("9900400", "Sklené Pole");
    private static readonly Station Dolne = new(Home, "Dolné Mesto");

    [TestInitialize]
    public void Init()
    {
        GlobData.Stations = [Velka, Hranicna, Sklene, Dolne];
        GlobData.CustomStations = new ExBindingList<Station>();
    }

    [TestMethod]
    [DataRow("Číslo\tTyp\tPríchod\n521\tEx\t09:10", '\t')]
    [DataRow("Číslo;Typ;Trasy\n521;Ex;9900200,9900100", ';')]
    [DataRow("Číslo,Typ,Príchod\n521,Ex,09:10", ',')]
    [DataRow("521", ';')]
    public void Oddelovac_PodlaPrvehoRiadku(string text, char expected)
    {
        Assert.AreEqual(expected, FImportData.DetectSeparator(text));
    }

    [TestMethod]
    [DataRow("Koľaj", "Koľaj")]
    [DataRow("KOLAJ", "Koľaj")]
    [DataRow("Linka - Príchod", "Linka (príchod)")]
    [DataRow("Dátumové obmedzenie", "Dátum. obm. (text)")]
    [DataRow("Všetky stanice", "Všetky stanice (ID stanice)")]
    [DataRow("Příjezd", "Príchod")]
    [DataRow("Poznámka", "-")]
    public void Hlavicka_RozpoznaTypStlpca(string header, string expected)
    {
        Assert.AreEqual(expected, ImportTrainColumnType.ParseColumnName(header).Name);
    }

    [TestMethod]
    public void NazvyStanic_SMedzerou_SaNajdu()
    {
        var stations = Station.GetStationsFromNameListString("Veľká Ves, Dolné Mesto ,Sklené Pole");

        CollectionAssert.AreEqual(new[] { Velka.ID, Home, Sklene.ID }, stations.Select(s => s.ID).ToArray());
    }

    [TestMethod]
    public void Trasa_RozdeliSaPodlaTejtoStanice()
    {
        var (zo, @do) = FImportData.BuildRoute([Velka, Dolne, Hranicna, Sklene], Home, null, null);

        CollectionAssert.AreEqual(new[] { Velka.ID }, zo.Select(s => s.ID).ToArray());
        CollectionAssert.AreEqual(new[] { Hranicna.ID, Sklene.ID }, @do.Select(s => s.ID).ToArray());
    }

    [TestMethod]
    public void Trasa_BezStlpcaDlhehoHlasenia_VsetkyStaniceVDlhom()
    {
        var (zo, @do) = FImportData.BuildRoute([Velka, Dolne, Hranicna], Home, null, null);

        Assert.IsTrue(zo.Concat(@do).All(s => s.IsInLongReport && !s.IsInShortReport));
    }

    [TestMethod]
    public void Trasa_StanicaVKratkomAjDlhom_MaObePriznaky()
    {
        var (_, @do) = FImportData.BuildRoute([Dolne, Hranicna, Sklene], Home,
            shortStations: [Sklene], longStations: [Hranicna, Sklene]);

        Assert.IsFalse(@do[0].IsInShortReport);
        Assert.IsTrue(@do[0].IsInLongReport);
        Assert.IsTrue(@do[1].IsInShortReport);
        Assert.IsTrue(@do[1].IsInLongReport);
    }

    [TestMethod]
    public void Trasa_NemeniStaniceZoZoznamu()
    {
        FImportData.BuildRoute([Velka, Dolne], Home, [Velka], [Velka]);

        Assert.IsFalse(Velka.IsInShortReport);
        Assert.IsFalse(Velka.IsInLongReport);
    }
}
