using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Ciselnik dopravcov grafikonu (Lokalne nastavenia → Dopravcovia, Vlastnik.txt).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class OperatorRulesTests
{
    [TestMethod]
    public void Dopravca_RozneNazvy_SuVPoriadku()
    {
        string[] names = ["ZSSK", "RegioJet", "Leo Express"];

        for (var i = 0; i < names.Length; i++)
            Assert.IsNull(OperatorRules.CheckName(names, i));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Dopravca_PrazdnyNazov_JeChyba(string name)
    {
        Assert.IsNotNull(OperatorRules.CheckName(["ZSSK", name], 1));
    }

    [TestMethod]
    public void Dopravca_NazovSUvodzovkami_JeChyba()
    {
        // nazov sa zapisuje do uvodzoviek bez escapovania - vnutorne uvodzovky by riadok rozbili
        Assert.IsNotNull(OperatorRules.CheckName(["Dopravca \"Sever\""], 0));
    }

    [TestMethod]
    [DataRow("ZSSK", "ZSSK")]
    [DataRow("ZSSK", "zssk ")]
    public void Dopravca_RovnakyNazov_JeChybaPriObochDopravcoch(string first, string second)
    {
        // meno nahravky je meno suboru - velkost pismen ani medzery na kraji ich nerozlisia
        string[] names = [first, second];

        Assert.IsNotNull(OperatorRules.CheckName(names, 0));
        Assert.IsNotNull(OperatorRules.CheckName(names, 1));
    }

    [TestMethod]
    [DataRow(new[] { -1, 1, 2 }, 3)]
    [DataRow(new[] { -1, 1, 7 }, 8)]
    [DataRow(new[] { -1 }, 1)]
    [DataRow(new int[0], 1)]
    public void Dopravca_NoveCislo_JeZaNajvyssim(int[] ids, int expected)
    {
        Assert.AreEqual(expected, OperatorRules.NextId(ids));
    }
}
