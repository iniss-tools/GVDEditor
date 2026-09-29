using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Entities;

namespace GVDEditor.Tests.Domain.Documents;

/// <summary>
/// Otvoreny grafikon: vyprazdnenie, novy grafikon a odvodene typy hlaseni.
/// </summary>
[TestClass]
public class GrafikonDocumentTests
{
    private GrafikonDocument _original = null!;

    [TestInitialize]
    public void Init() => _original = GlobData.Document;

    [TestCleanup]
    public void Cleanup() => GlobData.OpenDocument(_original);

    [TestMethod]
    public void Vyprazdnenie_ZachovaZoznamVlakov()
    {
        var open = new GrafikonDocument { Tracks = [Track.None] };
        open.Trains.Add(new Train { Number = "601" });
        GlobData.OpenDocument(open);
        var trains = GlobData.Trains;

        GlobData.ClearGrafikonData();

        Assert.AreSame(trains, GlobData.Trains, "tabulka vlakov v hlavnom okne ostava naviazana na ten isty zoznam");
        Assert.IsEmpty(GlobData.Trains);
        Assert.IsEmpty(GlobData.Tracks);
    }

    [TestMethod]
    public void NovyGrafikon_PredvoleneUdajeAVsetkyJazyky()
    {
        List<FyzLanguage> languages = [new("SK", "Slovenčina"), new("GB", "Angličtina")];

        var document = GrafikonDocument.CreateNew(languages);

        CollectionAssert.AreEqual(new[] { Track.None }, document.Tracks.ToList());
        CollectionAssert.AreEqual(new[] { Platform.None }, document.Platforms.ToList());
        CollectionAssert.AreEqual(new[] { Operator.None }, document.Operators.ToList());
        CollectionAssert.AreEqual(languages, document.LocalLanguages);
        Assert.AreNotSame(languages, document.LocalLanguages);
        Assert.AreEqual("", document.TableFontDir, "priecinok pisiem sa nepreberie z predchadzajuceho grafikonu");
    }

    [TestMethod]
    public void TypyHlaseni_PodlaSmerovania_SaPocitajuZTypov()
    {
        var document = new GrafikonDocument { ReportTypes = ReportType.GetDefaultValuesSK() };

        CollectionAssert.AreEqual(document.ReportTypes, document.ReportTypesV);
        CollectionAssert.AreEqual(document.ReportTypes, document.ReportTypesP);
        CollectionAssert.AreEqual(document.ReportTypes.Where(t => t.TerminateTrain).ToList(), document.ReportTypesK);
        CollectionAssert.DoesNotContain(document.ReportTypesK, ReportType.Stoji);
    }
}
