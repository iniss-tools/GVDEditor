using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tests.Domain.Entities;

/// <summary>
/// Vyznam cisla pisma pri tabuliach ELEN (Lokalne nastavenia → Pisma, ModeTabs.TXT [FONT]).
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
    public void PismoElen_SkladanieZCastiVratiRovnakeCislo()
    {
        // vyber pisma sklada cislo z casti - kazde cislo musi prejst tam a spat bez zmeny (vratane bitov bez ovladaca)
        for (var id = -1; id <= 0xFFFF; id++)
        {
            var code = new ElenFontCode(id);
            if (id < 0)
                continue;

            var composed = ElenFontCode.Compose(code.KeptBits, code.Face, code.Color, code.Blinks, code.TallDigits, code.ExtendedFont);
            Assert.AreEqual(id, composed.Id, $"cislo {id} (0x{id:X})");
        }
    }

    [TestMethod]
    [DataRow(1, 1, false, false, 0, 81)] // tenke cervene
    [DataRow(2, 1, true, false, 0, 101)] // tucne cervene blikajuce
    [DataRow(1, 2, false, true, 0, 90)] // tenke zelene s vysokymi cislicami
    [DataRow(3, 3, false, false, 0, 115)] // len cislice zlte
    [DataRow(0, 2, false, false, 6, 34370)] // ELEN16 rozsirene pismo 6 zelene
    public void PismoElen_NovePismoMaBit0x40(int face, int color, bool blinks, bool tall, int extended, int expected)
    {
        Assert.AreEqual(expected,
            ElenFontCode.Compose(ElenFontCode.DefaultKeptBits, face, color, blinks, tall, extended).Id);
    }

    [TestMethod]
    public void PismoElen_ZmenaRezuZachovaBityBezOvladaca()
    {
        // 0x215: bity nad dolnym bajtom bez 0x8000 - tabuli sa neposielaju, ale v cisle ostanu
        var code = new ElenFontCode(0x215);
        var changed = ElenFontCode.Compose(code.KeptBits, 2, code.Color, code.Blinks, code.TallDigits, 0);

        Assert.AreEqual(0x225, changed.Id);
    }

    [TestMethod]
    public void PismoElen_RozsirenePismaPodlaVyrobcu()
    {
        Assert.AreEqual(9, ElenFontCode.MaxExtendedFont(null));
        Assert.AreEqual(9, ElenFontCode.MaxExtendedFont(TableManufacturer.Elen16));
        Assert.AreEqual(4, ElenFontCode.MaxExtendedFont(TableManufacturer.Elen10));
        Assert.AreEqual(0, ElenFontCode.MaxExtendedFont(TableManufacturer.Elen));
    }

    [TestMethod]
    public void PismoElen_NazovPodlaVzhladu()
    {
        var old = Resources.Culture;
        Resources.Culture = new CultureInfo("sk-SK");
        try
        {
            Assert.AreEqual("Tučné červené blikajúce", new ElenFontCode(101).SuggestedName());
            Assert.AreEqual("Tenké zelené s vysokými číslicami", new ElenFontCode(90).SuggestedName());
            Assert.AreEqual("Rozšírené písmo 6 zelené", new ElenFontCode(34370).SuggestedName());
        }
        finally
        {
            Resources.Culture = old;
        }
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
        Assert.IsTrue(ElenFontCode.AppliesTo(TableManufacturer.Elen16));
        Assert.IsTrue(ElenFontCode.AppliesTo(TableManufacturer.Elekon));
        Assert.IsFalse(ElenFontCode.AppliesTo(TableManufacturer.Lcd1));
        Assert.IsFalse(ElenFontCode.AppliesTo(TableManufacturer.Apel));
    }
}
