using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GVDEditor.Entities;
using GVDEditor.Properties;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Vyznam cisla pisma pri tabuliach ELEN (Lokalne nastavenia → Pisma, ModeTabs.TXT [FONT]).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ElenFontCodeTests
{
    [TestMethod]
    [DataRow(82, 2, false, false, 1, 0)] // tenke
    [DataRow(81, 1, false, false, 1, 0)] // tenke cervene
    [DataRow(98, 2, false, false, 2, 0)] // tucne
    [DataRow(102, 2, true, false, 2, 0)] // tucne blikajuce
    [DataRow(90, 2, false, true, 1, 0)] // vysoke tenke cislice
    [DataRow(119, 3, true, false, 3, 0)] // len cislice zlte blikajuce
    [DataRow(34370, 2, false, false, 0, 6)] // ELEN16 rozsirene pismo 6 zelene
    public void PismoElen_BityPodlaINISS(int id, int color, bool blinks, bool tallDigits, int face, int extended)
    {
        var code = new ElenFontCode(id);
        Assert.AreEqual(color, code.Color);
        Assert.AreEqual(blinks, code.Blinks);
        Assert.AreEqual(tallDigits, code.TallDigits);
        Assert.AreEqual(face, code.Face);
        Assert.AreEqual(extended, code.ExtendedFont);
    }

    [TestMethod]
    public void PismoElen_BityNad255BezRozsirenia()
    {
        Assert.IsTrue(new ElenFontCode(533).HasIgnoredHighBits);
        Assert.IsFalse(new ElenFontCode(34370).HasIgnoredHighBits);
        Assert.IsFalse(new ElenFontCode(255).HasIgnoredHighBits);
    }

    [TestMethod]
    public void PismoElen_PopisPreObsluhu()
    {
        var old = Resources.Culture;
        Resources.Culture = new CultureInfo("sk-SK");
        try
        {
            Assert.AreEqual("tučné, červené, bliká", new ElenFontCode(101).Describe());
            Assert.AreEqual("tenké, zelené, vysoké číslice", new ElenFontCode(90).Describe());
            Assert.AreEqual("rozšírené písmo 6, žlté", new ElenFontCode(34371).Describe());
            Assert.AreEqual("písmo stĺpca", new ElenFontCode(-1).Describe());
        }
        finally
        {
            Resources.Culture = old;
        }
    }

    [TestMethod]
    [DataRow(82, "", true, 5)]
    [DataRow(98, "Tučný", true, 7)]
    [DataRow(114, "Špeciálny", true, 6)]
    [DataRow(66, "", false, 6)]
    public void PismoElen_DoplnenieVlastnosti(int id, string typeKey, bool proportional, int width)
    {
        var code = new ElenFontCode(id);
        Assert.AreEqual(typeKey, code.SuggestedType.Key);
        Assert.AreEqual(proportional, code.SuggestedProportional);
        Assert.AreEqual(width, code.SuggestedWidth);
    }

    [TestMethod]
    public void PismoElen_LenProtokolElen()
    {
        Assert.IsTrue(ElenFontCode.AppliesTo(null));
        Assert.IsTrue(ElenFontCode.AppliesTo(TableManufacturer.ELEN16));
        Assert.IsTrue(ElenFontCode.AppliesTo(TableManufacturer.ELEKON));
        Assert.IsFalse(ElenFontCode.AppliesTo(TableManufacturer.LCD1));
        Assert.IsFalse(ElenFontCode.AppliesTo(TableManufacturer.APEL));
    }
}
