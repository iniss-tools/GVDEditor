using System.Diagnostics.CodeAnalysis;
using GVDEditor.Forms;

namespace GVDEditor.Tests;

/// <summary>
///     Nazov priecinka noveho grafikonu (Subor → Novy…) - zapisuje sa do DirList.TXT, ktory ciarky neuzatvara do uvodzoviek.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class NewGrafikonDirNameTests
{
    private static readonly string[] Existing = ["Horna.2024", "Dolna.2025"];

    [TestMethod]
    [DataRow("Horna.2025")]
    [DataRow("Brezová Dolina.2026")]
    public void NazovPriecinka_Platny_Prejde(string name)
    {
        Assert.IsNull(FNewGrafikon.CheckDirName(name, Existing));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(" Horna.2026")]
    [DataRow("Horna/2026")]
    [DataRow("Horna:2026")]
    [DataRow("Horna,2026")]
    [DataRow("Horna.")]
    public void NazovPriecinka_Neplatny_JeChyba(string name)
    {
        Assert.IsNotNull(FNewGrafikon.CheckDirName(name, Existing));
    }

    [TestMethod]
    public void NazovPriecinka_UzVDirList_JeChybaBezOhladuNaVelkostPismen()
    {
        Assert.IsNotNull(FNewGrafikon.CheckDirName("HORNA.2024", Existing));
    }
}
