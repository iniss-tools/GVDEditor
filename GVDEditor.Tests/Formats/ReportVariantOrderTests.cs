using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Poradie variantov hlasenia: INISS berie variant podla poradia sekcii VARIANT_nn - prvy (velke pismeno typu,
/// prvy stlpec mapy dodatku) je dlhe hlasenie, druhy (male pismeno) kratke. Nazvy variantov INISS necita.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ReportVariantOrderTests
{
    private readonly LoadWarnings _warnings = new();

    [TestMethod]
    public void VariantyHlasenia_Predvolene_PrvyJeDlhy()
    {
        var variants = ReportVariant.GetDefaultValues();

        Assert.HasCount(2, variants);
        Assert.AreEqual(0, variants[0].Key);
        Assert.AreEqual("Dlhé hlásenie", variants[0].Name);
        Assert.AreEqual(1, variants[1].Key);
        Assert.AreEqual("Krátke hlásenie", variants[1].Name);
    }

    [TestMethod]
    public void VariantyHlasenia_PrehodeneNazvyGVDEditora_SaOpravia()
    {
        var variants = new List<ReportVariant>
        {
            new(0, "Krátke hlásenie"),
            new(1, "Dlhé hlásenie")
        };

        Assert.IsTrue(ReportVariant.FixSwappedDefaultNames(variants));
        CollectionAssert.AreEqual(ReportVariant.GetDefaultValues(), variants);
        // uz opravene sa druhy raz nemenia
        Assert.IsFalse(ReportVariant.FixSwappedDefaultNames(variants));
    }

    [TestMethod]
    public void VariantyHlasenia_VlastneNazvy_SaNemenia()
    {
        var variants = new List<ReportVariant>
        {
            new(0, "Dlouhé hlášení"),
            new(1, "Krátké hlášení")
        };

        Assert.IsFalse(ReportVariant.FixSwappedDefaultNames(variants));
        Assert.AreEqual("Dlouhé hlášení", variants[0].Name);
        Assert.AreEqual("Krátké hlášení", variants[1].Name);
    }

    [TestMethod]
    public void VariantyHlasenia_CategoriStarehoGVDEditora_SaNacitaOpraveneAZapiseSpravne()
    {
        var dir = Directory.CreateTempSubdirectory("gvdcategori");
        try
        {
            var swapped = new List<ReportVariant>
            {
                new(0, "Krátke hlásenie"),
                new(1, "Dlhé hlásenie")
            };
            CategoriFile.WriteLocal(dir.FullName, swapped, ReportType.GetDefaultValuesSk(), []);
            _warnings.Clear();

            var (variants, types, _) = CategoriFile.ReadLocal(dir.FullName, [], _warnings);

            CollectionAssert.AreEqual(ReportVariant.GetDefaultValues(), variants);
            Assert.HasCount(5, types);
            Assert.HasCount(1, _warnings.Items);
            _warnings.Clear();

            CategoriFile.WriteLocal(dir.FullName, variants, types, []);
            var file = new TxtPropsAreasFields(Path.Combine(dir.FullName, GvdFileConsts.FileCategori));
            Assert.AreEqual("Dlhé hlásenie", file.Get("VARIANT_01", "NAME").AnsiToUTF());
            Assert.AreEqual("0", file.Get("VARIANT_01", "KEY"));
            Assert.AreEqual("Krátke hlásenie", file.Get("VARIANT_02", "NAME").AnsiToUTF());
        }
        finally
        {
            dir.Delete(true);
        }
    }

    [TestMethod]
    public void VariantyHlasenia_PismenaRadenia_VelkeJeDlheMaleKratke()
    {
        var dir = Directory.CreateTempSubdirectory("gvdrazeni");
        try
        {
            var prichadza = new ReportType("Prijizdi", "Přijíždí", "P");
            var zastavil = new ReportType("Zastavil", "Zastavil", "L");
            var context = new GrafikonContext(new InissWorkspace { Stations = [] },
                new GrafikonDocument { ReportTypes = [prichadza, zastavil], ReportVariants = ReportVariant.GetDefaultValues() }, AppLanguage.Slovak);
            var file = Path.Combine(dir.FullName, GvdFileConsts.FileRazeni1);
            File.WriteAllText(file, "#721,Pl,,,\r\n", Encodings.Win1250);

            var radenie = RazeniFile.Read(dir.FullName, [], context).Single();

            Assert.HasCount(2, radenie.ChosenReports);
            CollectionAssert.AreEqual(new[] { ReportVariant.DlheHlasenie }, radenie.ChosenReports.Single(r => r.Type == prichadza).Variants);
            CollectionAssert.AreEqual(new[] { ReportVariant.KratkeHlasenie }, radenie.ChosenReports.Single(r => r.Type == zastavil).Variants);

            RazeniFile.Write(dir.FullName, [radenie], [], context.Document.ReportVariants);
            var header = File.ReadAllLines(file, Encodings.Win1250).Single(line => line.StartsWith('#'));

            Assert.AreEqual("#721,Pl,,,", header);
        }
        finally
        {
            dir.Delete(true);
        }
    }
}
