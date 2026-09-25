using System.Diagnostics.CodeAnalysis;
using GVDEditor.Entities;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Generovanie textov na tabuliach (TTexts.txt) – spolocne pre okno FTableText aj automaticke generovanie pri
///     ukladani grafikonu (FMain).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TableTextGeneratingTests
{
    private static readonly Station ThisStation = new("100", "Stredná");

    private static Station St(string name, bool shortReport = true) => new(name, name, shortReport);

    private static Train Vlak(int id, Routing routing, IEnumerable<Station>? zo = null, IEnumerable<Station>? doSmeru = null)
    {
        var train = new Train { ID = id, Number = id.ToString(System.Globalization.CultureInfo.InvariantCulture), Routing = routing };
        train.StaniceZoSmeru.AddRange(zo ?? []);
        train.StaniceDoSmeru.AddRange(doSmeru ?? []);
        return train;
    }

    private static TableItem Stlpec(TableFillSection fill) => new() { Key = fill.Name, Name = fill.Name, FillSection = fill };

    private static TableTextRealization Realizacia(TableFillSection fill) =>
        new() { Table = new TableCatalog(), Item = Stlpec(fill) };

    private static readonly Train Prechadzajuci = Vlak(1, Routing.Prechadzajuci,
        [St("Východ"), St("Malá", false), St("Horná")], [St("Dolná"), St("Nízka", false), St("Brezová"), St("Cieľ")]);

    private static readonly Train Vychadzajuci = Vlak(2, Routing.Vychadzajuci, doSmeru: [St("Dolná"), St("Cieľ")]);

    private static readonly Train Konciaci = Vlak(3, Routing.Konciaci, zo: [St("Východ"), St("Horná")]);

    [TestMethod]
    public void Generovanie_CielovaStanica()
    {
        Assert.AreEqual("Cieľ", TableTextGenerating.GenerateText(Prechadzajuci, TableFillSection.CielovaStanica, ThisStation));
        Assert.AreEqual("Cieľ", TableTextGenerating.GenerateText(Vychadzajuci, TableFillSection.CielovaStanicaPodchod, ThisStation));
        Assert.AreEqual("Stredná", TableTextGenerating.GenerateText(Konciaci, TableFillSection.CielovaStanicaNastupiste, ThisStation));
    }

    [TestMethod]
    public void Generovanie_VychadzajucaStanica()
    {
        Assert.AreEqual("Východ", TableTextGenerating.GenerateText(Prechadzajuci, TableFillSection.VychadzajucaStanica, ThisStation));
        Assert.AreEqual("Stredná", TableTextGenerating.GenerateText(Vychadzajuci, TableFillSection.VychadzajucaStanica, ThisStation));
    }

    [TestMethod]
    public void Generovanie_StaniceDoSmeruBezPoslednejAMimoKratsiehoHlasenia()
    {
        Assert.AreEqual("Dolná#Brezová", TableTextGenerating.GenerateText(Prechadzajuci, TableFillSection.StaniceDoSmeru, ThisStation));
        Assert.AreEqual("Dolná", TableTextGenerating.GenerateText(Vychadzajuci, TableFillSection.StaniceDoSmeruNastupiste, ThisStation));
        Assert.AreEqual("", TableTextGenerating.GenerateText(Konciaci, TableFillSection.StaniceDoSmeru, ThisStation));
    }

    [TestMethod]
    public void Generovanie_StaniceZoSmeruBezPrvej()
    {
        Assert.AreEqual("Horná", TableTextGenerating.GenerateText(Prechadzajuci, TableFillSection.StaniceZoSmeru, ThisStation));
        Assert.AreEqual("Horná", TableTextGenerating.GenerateText(Konciaci, TableFillSection.StaniceZoSmeru, ThisStation));
        Assert.AreEqual("", TableTextGenerating.GenerateText(Vychadzajuci, TableFillSection.StaniceZoSmeru, ThisStation));
    }

    [TestMethod]
    public void Generovanie_PrazdneTrasyNezhodiaGenerovanie()
    {
        //predtym pri ukladani RemoveAt(-1) na prazdnom zozname a Last() na prazdnej trase
        var bezStanic = Vlak(4, Routing.Prechadzajuci);
        var bezKratsiehoHlasenia = Vlak(5, Routing.Prechadzajuci, doSmeru: [St("Dolná", false)]);

        foreach (var fill in TableFillSection.GetValues().Where(TableTextGenerating.IsSupported))
        {
            Assert.AreEqual("", TableTextGenerating.GenerateText(bezStanic, fill, ThisStation), fill.Name);
            _ = TableTextGenerating.GenerateText(bezKratsiehoHlasenia, fill, ThisStation);
        }

        Assert.AreEqual("", TableTextGenerating.GenerateText(bezKratsiehoHlasenia, TableFillSection.StaniceDoSmeru, ThisStation));
    }

    [TestMethod]
    public void Generovanie_NepodporovanyStlpecVyhodiVynimku()
    {
        Assert.IsFalse(TableTextGenerating.IsSupported(TableFillSection.CasOdchodu));
        Assert.IsFalse(TableTextGenerating.IsSupported(null));
        Assert.ThrowsExactly<ArgumentException>(() =>
            TableTextGenerating.GenerateText(Prechadzajuci, TableFillSection.CasOdchodu, ThisStation));
    }

    [TestMethod]
    public void Generovanie_VsetkyVlakyVPoradiSPredvolenymPismom()
    {
        var texts = TableTextGenerating.Generate([Prechadzajuci, Vychadzajuci, Konciaci], TableFillSection.CielovaStanica, ThisStation);

        CollectionAssert.AreEqual(new[] { 1, 2, 3 }, texts.Select(t => t.Train.ID).ToArray());
        CollectionAssert.AreEqual(new[] { "Cieľ", "Cieľ", "Stredná" }, texts.Select(t => t.Text).ToArray());
        Assert.IsTrue(texts.All(t => t.FontID == -1));
    }

    [TestMethod]
    public void Ukladanie_ViacRealizaciiGenerujePodlaPrvejPodporovanej()
    {
        //predtym vysledok zavisel od poslednej realizacie
        var text = new TableText
        {
            Key = "Ciel", Name = "Ciel", Comment = "",
            Realizations = [Realizacia(TableFillSection.CasOdchodu), Realizacia(TableFillSection.CielovaStanica), Realizacia(TableFillSection.StaniceDoSmeru)]
        };

        Assert.AreSame(text.Realizations[1], TableTextGenerating.FindGeneratingRealization(text));

        var count = TableTextGenerating.RegenerateAll([text], [Prechadzajuci, Konciaci], ThisStation);

        Assert.AreEqual(1, count);
        CollectionAssert.AreEqual(new[] { "Cieľ", "Stredná" }, text.Trains.Select(t => t.Text).ToArray());
    }

    [TestMethod]
    public void Ukladanie_TextBezPodporovanejRealizacieSaNemeni()
    {
        //predtym nepodporovany stlpec zmazal texty a ukoncil generovanie aj pre dalsie typy textov
        var rucny = new TableTrain { Train = Prechadzajuci, Text = "Ručný", FontID = 3 };
        var nepodporovany = new TableText { Key = "Cas", Name = "Cas", Comment = "", Realizations = [Realizacia(TableFillSection.CasOdchodu)], Trains = [rucny] };
        var bezRealizacie = new TableText { Key = "Bez", Name = "Bez", Comment = "", Trains = [rucny] };
        var ciel = new TableText { Key = "Ciel", Name = "Ciel", Comment = "", Realizations = [Realizacia(TableFillSection.CielovaStanica)] };

        var count = TableTextGenerating.RegenerateAll([nepodporovany, bezRealizacie, ciel], [Prechadzajuci], ThisStation);

        Assert.AreEqual(1, count);
        Assert.AreSame(rucny, nepodporovany.Trains.Single());
        Assert.AreSame(rucny, bezRealizacie.Trains.Single());
        Assert.AreEqual("Cieľ", ciel.Trains.Single().Text);
    }

    [TestMethod]
    public void PridanieVlaku_PonukaLenVlakyBezTextuPodlaInstancie()
    {
        //Train je record – kopia s rovnakymi hodnotami je iny vlak
        var kopia = Prechadzajuci with { };
        var existing = new[] { new TableTrain { Train = Prechadzajuci, Text = "", FontID = -1 } };

        var without = TableTextGenerating.TrainsWithoutText([Prechadzajuci, Vychadzajuci, kopia, Konciaci], existing);

        Assert.HasCount(3, without);
        Assert.AreSame(Vychadzajuci, without[0]);
        Assert.AreSame(kopia, without[1]);
        Assert.AreSame(Konciaci, without[2]);
    }

    [TestMethod]
    public void PridanieVlaku_TextPodlaStlpcaAleboPrazdny()
    {
        Assert.AreEqual("Cieľ", TableTextGenerating.CreateFor(Prechadzajuci, TableFillSection.CielovaStanica, ThisStation).Text);
        Assert.AreEqual("", TableTextGenerating.CreateFor(Prechadzajuci, TableFillSection.CasOdchodu, ThisStation).Text);
        Assert.AreEqual("", TableTextGenerating.CreateFor(Prechadzajuci, null, ThisStation).Text);
        Assert.AreEqual(-1, TableTextGenerating.CreateFor(Prechadzajuci, null, ThisStation).FontID);
    }

    [TestMethod]
    public void Kopie_ZmenaKopieNemeniPovodny()
    {
        var original = new TableTrain { Train = Prechadzajuci, Text = "A", FontID = 1 };
        var copy = TableTextGenerating.Clone(original);
        copy.Text = "B";
        copy.FontID = 2;

        Assert.AreEqual("A", original.Text);
        Assert.AreEqual(1, original.FontID);
        Assert.AreSame(original.Train, copy.Train);

        var realization = Realizacia(TableFillSection.CielovaStanica);
        var realizationCopy = TableTextGenerating.Clone(realization);
        realizationCopy.Item = Stlpec(TableFillSection.StaniceDoSmeru);

        Assert.AreEqual(TableFillSection.CielovaStanica, realization.Item.FillSection);
        Assert.AreSame(realization.Table, realizationCopy.Table);
    }
}
