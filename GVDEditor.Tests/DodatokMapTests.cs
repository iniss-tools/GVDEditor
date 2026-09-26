using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using ToolsCore.Entities;

namespace GVDEditor.Tests;

/// <summary>
///     Mapa dodatku v Doplnky.txt: obsahuje len typy hlaseni platne pre smerovanie vlaku, v poradi zo zoznamu typov.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class DodatokMapTests
{
    // "Ukoncit nastup" plati len pre vychadzajuci vlak, "Prijizdi" len pre prechadzajuci a konciaci
    private static readonly ReportType Nastup = new("UkoncitNastup", "Ukončiť nástup", "U", baseTrain: true, passThrough: false, terminateTrain: false);
    private static readonly ReportType Prijizdi = new("Prijizdi", "Přijíždí", "P", baseTrain: false, passThrough: true, terminateTrain: true);
    private static readonly ReportType Odjede = new("Odjede", "Odjede", "O");

    private static readonly List<ReportType> Types = [Nastup, Prijizdi, Odjede];
    private static readonly List<ReportVariant> Variants = ReportVariant.GetDefaultValues();

    private static readonly FyzSound Sound = new() { Key = "D1001", Name = "D1001" };

    [TestMethod]
    public void Dodatok_MapaPrechadzajucehoVlaku_PriradiTypyPlatnePreSmerovanie()
    {
        // prechadzajuci vlak: Prijizdi (dlhe), Odjede (kratke)
        var dodatok = Dodatok.NumsToDodatok(Sound, "1001", Types, Variants, Routing.Prechadzajuci);

        Assert.HasCount(2, dodatok.ChosenReports);
        Assert.AreSame(Prijizdi, dodatok.ChosenReports[0].Type);
        CollectionAssert.AreEqual(new[] { Variants[0] }, dodatok.ChosenReports[0].Variants);
        Assert.AreSame(Odjede, dodatok.ChosenReports[1].Type);
        CollectionAssert.AreEqual(new[] { Variants[1] }, dodatok.ChosenReports[1].Variants);
    }

    [TestMethod]
    public void Dodatok_MapaNacitanaAZapisana_SaNezmeni()
    {
        foreach (var (routing, subset, map) in new[]
                 {
                     (Routing.Prechadzajuci, Types.Where(t => t.PassThrough).ToList(), "0110"),
                     (Routing.Konciaci, Types.Where(t => t.TerminateTrain).ToList(), "1011"),
                     (Routing.Vychadzajuci, Types.Where(t => t.BaseTrain).ToList(), "1101")
                 })
        {
            var dodatok = Dodatok.NumsToDodatok(Sound, map, Types, Variants, routing);

            Assert.AreEqual(map, Dodatok.DodatokToNums(dodatok, subset, Variants), routing.ToString());
        }
    }
}
