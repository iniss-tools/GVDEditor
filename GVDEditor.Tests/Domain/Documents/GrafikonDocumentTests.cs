using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Entities;

namespace GVDEditor.Tests.Domain.Documents;

/// <summary>
/// Otvoreny grafikon: nacitanie do noveho dokumentu a jeho vymena naraz (hlavne okno pocas nacitania vidi povodny).
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
    public void Nacitanie_ZapisujeDoNovehoDokumentu_OtvorenyOstavaNezmeneny()
    {
        var open = new GrafikonDocument { Operators = [Operator.None] };
        GlobData.OpenDocument(open);
        var loaded = GlobData.LoadDocument(() =>
        {
            GlobData.Operators = [Operator.None, new Operator(1, "ZSSK")];
            // parser pocas nacitania cita uz nove data (napr. dopravcov pre vlaky)
            Assert.HasCount(2, GlobData.Operators);
        });

        Assert.AreSame(open, GlobData.Document);
        Assert.HasCount(1, GlobData.Operators);
        Assert.HasCount(2, loaded.Operators);

        GlobData.OpenDocument(loaded);
        Assert.AreEqual("ZSSK", GlobData.Operators[1].Name);
    }

    [TestMethod]
    public void Nacitanie_InyVlakno_VidiOtvorenyDokument()
    {
        var open = new GrafikonDocument();
        GlobData.OpenDocument(open);
        GrafikonDocument? seenFromOtherThread = null;

        GlobData.LoadDocument(() =>
        {
            var thread = new Thread(() => seenFromOtherThread = GlobData.Document);
            thread.Start();
            thread.Join();
        });

        Assert.AreSame(open, seenFromOtherThread);
    }

    [TestMethod]
    public void Nacitanie_Chyba_OtvorenyDokumentOstane()
    {
        var open = new GrafikonDocument();
        GlobData.OpenDocument(open);

        Assert.ThrowsExactly<FormatException>(() => GlobData.LoadDocument(() =>
        {
            GlobData.Tracks = [Track.None];
            throw new FormatException("chyba v polovici nacitania");
        }));

        Assert.AreSame(open, GlobData.Document);
        Assert.IsEmpty(GlobData.Tracks);
    }

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
