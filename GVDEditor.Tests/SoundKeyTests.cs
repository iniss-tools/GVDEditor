using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Odkazy na zvuky banky: INISS hlada skupinu aj zvuk podla klucov (nie nazvov), bez ohladu na velkost pismen.
///     Testovacia banka ma nazvy odlisne od klucov, aby sa zamena prejavila.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class SoundKeyTests
{
    private static readonly FyzLanguage Sk = new("SK", "Slovenčina", "SK\\") { IsBasic = true };
    private static readonly FyzLanguage Cz = new("CZ", "Čeština", "CZ\\");

    private static readonly FyzGroup SkVlakNum = new(Sk, "VlakNum", "Číslovky", "VlakNum\\");
    private static readonly FyzGroup SkPoz7 = new(Sk, "Poz7", "Poznámky", "Poz7\\");
    private static readonly FyzGroup CzPoz7 = new(Cz, "Poz7", "Poznámky CZ", "Poz7\\");
    private static readonly FyzGroup SkR1 = new(Sk, "R1", "Stanice", "R1\\");
    private static readonly FyzGroup SkV8 = new(Sk, "V8", "Názvy vlaků", "V8\\");
    private static readonly FyzGroup SkDodatky = new(Sk, "DODATKY", "Dodatkové hlásenia", "DODATKY\\");

    private static readonly FyzSound Nmen = new(SkVlakNum, "NMEN", "NFMEN", "NMEN.WAV", "", "menší", 0);
    // nazov tohto zvuku je zhodny s klucom ineho - odkaz podla nazvu by nasiel zly zvuk
    private static readonly FyzSound Decoy = new(SkVlakNum, "NFMEN", "NMEN", "NFMEN.WAV", "", "iný", 0);
    private static readonly FyzSound SkZalok = new(SkPoz7, "zalok", "Za lokomotívou", "ZALOK.WAV", "", "Za rušňom", 0);
    private static readonly FyzSound CzZalok = new(CzPoz7, "zalok", "Za lokomotivou", "ZALOK.WAV", "", "Za lokomotivou", 0);
    private static readonly FyzSound StationSound = new(SkR1, "5693001", "Zastavka A", "5693001.WAV", "", "Abda,", 0);
    private static readonly FyzSound TrainName = new(SkV8, "Pendolino", "Názov Pendolino", "PEND.WAV", "", "Pendolino", 0);
    private static readonly FyzSound Dodatok1003 = new(SkDodatky, "D1003", "Výluka", "D1003.WAV", "", "Výluka", 0);

    private static readonly List<FyzSound> Sounds = [Nmen, Decoy, SkZalok, CzZalok, StationSound, TrainName, Dodatok1003];

    [TestMethod]
    public void Razeni1_OdkazyPodlaKlucov_SaNacitajuAZapisuSKlucmi()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        var (oldTypes, oldVariants, oldLangs) = (GlobData.ReportTypes, GlobData.ReportVariants, GlobData.LocalLanguages);
        try
        {
            GlobData.ReportTypes = [new ReportType("Prijizdi", "Přijíždí", "P")];
            GlobData.ReportVariants = ReportVariant.GetDefaultValues();
            GlobData.LocalLanguages = [Sk, Cz];
            LoadWarnings.Clear();
            var file = Path.Combine(dir.FullName, FileConsts.FILE_RAZENI1);
            // male pismena, dvojdielny zapis a odkaz podla nazvov (ten INISS nenajde)
            File.WriteAllLines(file, ["#721,P,,,", "sk/vlaknum/nmen", "CZ/POZ7/ZALOK", "Poz7/zalok", "SK/Číslovky/NFMEN"], Encodings.Win1250);

            var radenia = TxtParser.ReadRazeni1(dir.FullName, Sounds);

            Assert.HasCount(1, radenia);
            CollectionAssert.AreEqual(new[] { Nmen, CzZalok, SkZalok }, radenia[0].Sounds);
            Assert.HasCount(1, LoadWarnings.Items);
            StringAssert.Contains(LoadWarnings.Items[0], "SK/Číslovky/NFMEN");

            TxtParser.WriteRazeni1(dir.FullName, radenia, [Sk, Cz]);
            var refs = File.ReadAllLines(file, Encodings.Win1250).Where(line => !line.StartsWith('#') && !line.StartsWith(';')).ToList();

            CollectionAssert.AreEqual(new[] { "SK/VlakNum/NMEN", "CZ/Poz7/zalok", "SK/Poz7/zalok" }, refs);

            var again = TxtParser.ReadRazeni1(dir.FullName, Sounds);
            CollectionAssert.AreEqual(radenia[0].Sounds, again[0].Sounds);
        }
        finally
        {
            (GlobData.ReportTypes, GlobData.ReportVariants, GlobData.LocalLanguages) = (oldTypes, oldVariants, oldLangs);
            LoadWarnings.Clear();
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Banka_StaniceANazvyVlakov_SuKluceZvukov()
    {
        var property = typeof(GlobData).GetProperty(nameof(GlobData.Sounds))!;
        var old = GlobData.Sounds;
        try
        {
            property.SetValue(null, Sounds);

            var station = Station.GetStations().Single();
            Assert.AreEqual("5693001", station.ID);
            Assert.AreEqual("Abda", station.Name);

            CollectionAssert.AreEqual(new[] { new Entities.TrainName("Pendolino", "Názov Pendolino") }, Train.GetTrainNames());
        }
        finally
        {
            property.SetValue(null, old);
        }
    }

    private static readonly List<Entities.TrainName> TrainNames =
        [new("SLOVAKIA", "Slovakia"), new("Vojtíh Lanna", "Vojtěch Lanna"), new("Ostavan", "Ostravan"), new("Ostravan", "Iny")];

    [TestMethod]
    [DataRow("SLOVAKIA", "Slovakia")]
    [DataRow("slovakia", "Slovakia")]
    [DataRow("Vojtíh Lanna", "Vojtěch Lanna")]
    [DataRow("SLOVAKIA /443", "SLOVAKIA /443")]
    public void NazovVlaku_KlucZGrafikonuSaZobraziAkoNazov(string stored, string display) =>
        Assert.AreEqual(display, Entities.TrainName.ToDisplay(TrainNames, stored));

    [TestMethod]
    [DataRow("Slovakia", "SLOVAKIA")]
    [DataRow("Vojtěch Lanna", "Vojtíh Lanna")]
    [DataRow("SLOVAKIA", "SLOVAKIA")]
    // zobrazeny nazov ma prednost pred klucom ineho zvuku
    [DataRow("Ostravan", "Ostavan")]
    [DataRow("REGIOJET /1020", "REGIOJET /1020")]
    [DataRow("", "")]
    public void NazovVlaku_ZoznamZapiseKluc(string text, string stored) =>
        Assert.AreEqual(stored, Entities.TrainName.ToStored(TrainNames, text));

    [TestMethod]
    public void NazovVlaku_TabulkaTriediPodlaZobrazenehoNazvu()
    {
        // kluce su v opacnom poradi ako nazvy: "Zeta" = Alfa, "Alfa" = Zeta
        List<Entities.TrainName> names = [new("Zeta", "Alfa"), new("Alfa", "Zeta")];
        var list = new TrainBindingList([new Train { Name = "Alfa" }, new Train { Name = "SLOVAKIA /443" }, new Train { Name = "Zeta" }])
        {
            TrainNames = names
        };

        ((System.ComponentModel.IBindingList)list).ApplySort(
            System.ComponentModel.TypeDescriptor.GetProperties(typeof(Train))[nameof(Train.Name)]!, System.ComponentModel.ListSortDirection.Ascending);

        CollectionAssert.AreEqual(new[] { "Zeta", "SLOVAKIA /443", "Alfa" }, list.Select(t => t.Name).ToList());
    }

    [TestMethod]
    [DataRow("D1003", "1003")]
    [DataRow("d1003", "1003")]
    [DataRow("1003", "1003")]
    [DataRow("DD12", "D12")]
    public void Dodatok_KodZKlucaZvuku(string key, string code) => Assert.AreEqual(code, Dodatok.CodeFromKey(key));

    [TestMethod]
    public void Dodatok_KodSaBerieZKlucaNieZNazvu()
    {
        var dodatok = Dodatok.NumsToDodatok(Dodatok1003, "10", [new ReportType("Odjede", "Odjede", "O")], ReportVariant.GetDefaultValues(), Routing.Vychadzajuci);

        Assert.AreEqual("1003", dodatok.Name);
    }

    [TestMethod]
    public void FyzGroup_TypPodlaKluca() => Assert.AreEqual(FyzGroupType.VLAKNUM, SkVlakNum.Type);
}
