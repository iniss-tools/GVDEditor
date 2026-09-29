using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Tools;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Poradie variantov hlasenia: INISS berie variant podla poradia sekcii VARIANT_nn - prvy (velke pismeno typu,
/// prvy stlpec mapy dodatku) je dlhe hlasenie, druhy (male pismeno) kratke. Nazvy variantov INISS necita.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ReportVariantOrderTests
{
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
            CategoriFile.WriteLocal(dir.FullName, swapped, ReportType.GetDefaultValuesSK(), []);
            LoadWarnings.Clear();

            var (variants, types, _) = CategoriFile.ReadLocal(dir.FullName, GlobData.Languages);

            CollectionAssert.AreEqual(ReportVariant.GetDefaultValues(), variants);
            Assert.HasCount(5, types);
            Assert.HasCount(1, LoadWarnings.Items);
            LoadWarnings.Clear();

            CategoriFile.WriteLocal(dir.FullName, variants, types, []);
            var file = new TxtPropsAreasFields(Path.Combine(dir.FullName, GvdFileConsts.FILE_CATEGORI));
            Assert.AreEqual("Dlhé hlásenie", file.Get("VARIANT_01", "NAME").ANSItoUTF());
            Assert.AreEqual("0", file.Get("VARIANT_01", "KEY"));
            Assert.AreEqual("Krátke hlásenie", file.Get("VARIANT_02", "NAME").ANSItoUTF());
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
        var (oldTypes, oldVariants) = (GlobData.ReportTypes, GlobData.ReportVariants);
        try
        {
            var prichadza = new ReportType("Prijizdi", "Přijíždí", "P");
            var zastavil = new ReportType("Zastavil", "Zastavil", "L");
            GlobData.ReportTypes = [prichadza, zastavil];
            GlobData.ReportVariants = ReportVariant.GetDefaultValues();
            var file = Path.Combine(dir.FullName, GvdFileConsts.FILE_RAZENI1);
            File.WriteAllText(file, "#721,Pl,,,\r\n", Encodings.Win1250);

            var radenie = RazeniFile.Read(dir.FullName, [], GrafikonContext.Current).Single();

            Assert.HasCount(2, radenie.ChosenReports);
            CollectionAssert.AreEqual(new[] { ReportVariant.DlheHlasenie }, radenie.ChosenReports.Single(r => r.Type == prichadza).Variants);
            CollectionAssert.AreEqual(new[] { ReportVariant.KratkeHlasenie }, radenie.ChosenReports.Single(r => r.Type == zastavil).Variants);

            RazeniFile.Write(dir.FullName, [radenie], [], GlobData.ReportVariants);
            var header = File.ReadAllLines(file, Encodings.Win1250).Single(line => line.StartsWith('#'));

            Assert.AreEqual("#721,Pl,,,", header);
        }
        finally
        {
            (GlobData.ReportTypes, GlobData.ReportVariants) = (oldTypes, oldVariants);
            dir.Delete(true);
        }
    }
}
