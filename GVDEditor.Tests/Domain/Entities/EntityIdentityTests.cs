using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Tests.Domain.Entities;

/// <summary>
/// Entity grafikonu (vlak, kolaj, nastupiste, dopravca) sa porovnavaju referenciou, hodnotove objekty hodnotou.
/// </summary>
[TestClass]
public class EntityIdentityTests
{
    private static Train Vlak() => new() { ID = 1, Number = "601", Name = "", Variant = -1 };

    [TestMethod]
    public void Vlak_KopiaSRovnakymiUdajmi_JeInyVlak()
    {
        var original = Vlak();
        var kopia = Vlak();
        List<Train> trains = [original, kopia];

        Assert.AreNotEqual(original, kopia);
        trains.Remove(kopia);
        Assert.AreSame(original, trains.Single(), "odstranit sa musi prave ten vlak, nie prvy s rovnakymi udajmi");
    }

    [TestMethod]
    public void Vlak_CisloAVarianta_SaZobrazujuSpolu()
    {
        var train = Vlak();
        Assert.AreEqual("601", train.NumberVariant.ToString());

        train.Variant = 2;
        Assert.AreEqual(new NumberVariant("601", 2), train.NumberVariant);
        Assert.AreEqual("601 v2", train.NumberVariant.ToString());
    }

    [TestMethod]
    [DataRow("99", -1, "100", -1)]
    [DataRow("601", 1, "601", 2)]
    [DataRow("Ex 5", -1, "Ex 6", -1)]
    public void CisloAVarianta_TriediPodlaCislaPotomVarianty(string number1, int variant1, string number2, int variant2)
    {
        var first = new NumberVariant(number1, variant1);
        var second = new NumberVariant(number2, variant2);

        Assert.IsLessThan(0, first.CompareTo(second));
        Assert.IsTrue(first < second);
    }

    [TestMethod]
    public void Stanica_None_JeZakazdymNovaInstancia()
    {
        var none = Station.None;
        none.IsInShortReport = true;

        Assert.IsFalse(Station.None.IsInShortReport, "uprava stanice v trase nesmie zmenit predvolenu stanicu");
        Assert.AreEqual(Station.None, new Station("0000000", "None"));
    }

    [TestMethod]
    public void Dopravcovia_RovnakyRiadokDvakrat_SaNacitaRaz()
    {
        var dir = Directory.CreateTempSubdirectory("gvdoperators");
        try
        {
            File.WriteAllText(Path.Combine(dir.FullName, GvdFileConsts.FILE_VLASTNIK),
                "-1,\"Žiadny\"\r\n1,\"ZSSK\"\r\n1,\"ZSSK\"\r\n2,\"RegioJet\"\r\n", Encodings.Win1250);

            var operators = OperatorsFile.Read(dir.FullName);

            CollectionAssert.AreEqual(new[] { "Žiadny", "ZSSK", "RegioJet" }, operators.Select(o => o.Name).ToList());
            Assert.AreSame(Operator.None, operators[0]);
        }
        finally
        {
            dir.Delete(true);
        }
    }
}
