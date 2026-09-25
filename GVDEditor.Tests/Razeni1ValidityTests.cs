using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Razeni1.txt: zaznam s prazdnymi datumami plati bez obmedzenia a po ulozeni musi ostat bez datumov -
///     inak by INISS radenie hlasil len v nezmyselnom obdobi (01.01.0001), teda nikdy.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class Razeni1ValidityTests
{
    [TestMethod]
    public void Razeni1_PrazdneDatumy_SaNacitajuAZapisuBezObmedzenia()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        var oldTypes = GlobData.ReportTypes;
        var oldVariants = GlobData.ReportVariants;
        try
        {
            GlobData.ReportTypes = [new ReportType("Prijizdi", "Přijíždí", "P")];
            GlobData.ReportVariants = ReportVariant.GetDefaultValues();
            var file = Path.Combine(dir.FullName, FileConsts.FILE_RAZENI1);
            File.WriteAllText(file, "#721,P,,,\r\n#722,P,01.01.2026,03.01.2026,101\r\n", Encodings.Win1250);

            var radenia = TxtParser.ReadRazeni1(dir.FullName, []);

            Assert.HasCount(2, radenia);
            Assert.IsFalse(radenia[0].HasValidity);
            Assert.AreEqual("", radenia[0].DatObm);
            Assert.IsTrue(radenia[1].HasValidity);

            TxtParser.WriteRazeni1(dir.FullName, radenia, []);
            var headers = File.ReadAllLines(file, Encodings.Win1250).Where(line => line.StartsWith('#')).ToList();

            CollectionAssert.AreEqual(new[] { "#721,P,,,", "#722,P,01.01.2026,03.01.2026,101" }, headers);
        }
        finally
        {
            GlobData.ReportTypes = oldTypes;
            GlobData.ReportVariants = oldVariants;
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void Razeni1_CielovaStanica_SaZachova()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        var (oldTypes, oldVariants, oldStations, oldCustom) = (GlobData.ReportTypes, GlobData.ReportVariants, GlobData.Stations, GlobData.CustomStations);
        try
        {
            GlobData.ReportTypes = [new ReportType("Prijizdi", "Přijíždí", "P")];
            GlobData.ReportVariants = ReportVariant.GetDefaultValues();
            GlobData.Stations = [new Station("9900140", "Hraničná")];
            GlobData.CustomStations = [];
            var file = Path.Combine(dir.FullName, FileConsts.FILE_RAZENI1);
            // znama stanica, neznama stanica (ostane pod cislom) a bez obmedzenia
            File.WriteAllLines(file, ["#521:9900140,P,,,", "#521:9912345,P,,,", "#521,P,,,"], Encodings.Win1250);

            var radenia = TxtParser.ReadRazeni1(dir.FullName, []);

            Assert.AreEqual("Hraničná", radenia[0].DestStation.Name);
            Assert.AreEqual("9912345", radenia[1].DestStation.ID);
            Assert.IsNull(radenia[2].DestStation);

            TxtParser.WriteRazeni1(dir.FullName, radenia, []);
            var headers = File.ReadAllLines(file, Encodings.Win1250).Where(line => line.StartsWith('#')).ToList();

            CollectionAssert.AreEqual(new[] { "#521:9900140,P,,,", "#521:9912345,P,,,", "#521,P,,," }, headers);
        }
        finally
        {
            (GlobData.ReportTypes, GlobData.ReportVariants, GlobData.Stations, GlobData.CustomStations) = (oldTypes, oldVariants, oldStations, oldCustom);
            dir.Delete(true);
        }
    }
}
