using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Razeni1.txt: zaznam s prazdnymi datumami plati bez obmedzenia a po ulozeni musi ostat bez datumov -
/// inak by INISS radenie hlasil len v nezmyselnom obdobi (01.01.0001), teda nikdy.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class Razeni1ValidityTests
{
    private static GrafikonContext Context(List<Station>? stations = null) =>
        new(new InissWorkspace { Stations = stations ?? [] },
            new GrafikonDocument { ReportTypes = [new ReportType("Prijizdi", "Přijíždí", "P")], ReportVariants = ReportVariant.GetDefaultValues() },
            AppLanguage.Slovak);

    [TestMethod]
    public void Razeni1_PrazdneDatumy_SaNacitajuAZapisuBezObmedzenia()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        var context = Context();
        try
        {
            var file = Path.Combine(dir.FullName, GvdFileConsts.FILE_RAZENI1);
            File.WriteAllText(file, "#721,P,,,\r\n#722,P,01.01.2026,03.01.2026,101\r\n", Encodings.Win1250);

            var radenia = RazeniFile.Read(dir.FullName, [], context);

            Assert.HasCount(2, radenia);
            Assert.IsFalse(radenia[0].HasValidity);
            Assert.AreEqual("", radenia[0].DatObm);
            Assert.IsTrue(radenia[1].HasValidity);

            RazeniFile.Write(dir.FullName, radenia, [], context.Document.ReportVariants);
            var headers = File.ReadAllLines(file, Encodings.Win1250).Where(line => line.StartsWith('#')).ToList();

            CollectionAssert.AreEqual(new[] { "#721,P,,,", "#722,P,01.01.2026,03.01.2026,101" }, headers);
        }
        finally
        {
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Razeni1_CielovaStanica_SaZachova()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        var context = Context([new Station("9900140", "Hraničná")]);
        try
        {
            var file = Path.Combine(dir.FullName, GvdFileConsts.FILE_RAZENI1);
            // znama stanica, neznama stanica (ostane pod cislom) a bez obmedzenia
            File.WriteAllLines(file, ["#521:9900140,P,,,", "#521:9912345,P,,,", "#521,P,,,"], Encodings.Win1250);

            var radenia = RazeniFile.Read(dir.FullName, [], context);

            Assert.AreEqual("Hraničná", radenia[0].DestStation.Name);
            Assert.AreEqual("9912345", radenia[1].DestStation.ID);
            Assert.IsNull(radenia[2].DestStation);

            RazeniFile.Write(dir.FullName, radenia, [], context.Document.ReportVariants);
            var headers = File.ReadAllLines(file, Encodings.Win1250).Where(line => line.StartsWith('#')).ToList();

            CollectionAssert.AreEqual(new[] { "#521:9900140,P,,,", "#521:9912345,P,,,", "#521,P,,," }, headers);
        }
        finally
        {
            dir.Delete(true);
        }
    }
}
