using GVDEditor.Integration.Iniss2;
using ToolsCore.Iniss.Expressions;

namespace GVDEditor.Tests.Integration.Iniss2;

/// <summary>
/// Preklad vyrazov INISS 3.39 do VB.NET servera INISS2.
/// </summary>
[TestClass]
public class Iniss2ExpressionTranspilerTests
{
    private static readonly Dictionary<string, int> TrainTypes = new(StringComparer.Ordinal)
    {
        ["Os"] = 0, ["Sp"] = 2, ["R"] = 26, ["Ex"] = 27, ["RJ"] = 39, ["IC"] = 49, ["Nákl"] = 74
    };

    private static Iniss2ExpressionTranspiler Transpiler(ExprEvalSite site = ExprEvalSite.Default) =>
        new(TrainTypes, name => name is "1" or "2a" ? "K" + name : null, site);

    [TestMethod]
    [DataRow("1", "True")]
    [DataRow("0", "False")]
    [DataRow("Typ(Typ_R)", ".Druh = \"R\"")]
    [DataRow("Typ(Typ_R1)", ".Druh = \"RJ\"")]
    [DataRow("Typ(Typ_R) | Typ(Typ_Ex)", ".Druh = \"R\" Or .Druh = \"Ex\"")]
    [DataRow("(Typ(Typ_R) || Typ(Typ_Sp)) && !ODKLON", "(.Druh = \"R\" Or .Druh = \"Sp\") And Not .JeOdklonen")]
    [DataRow("Typ(Typ_R) && ZPOZDENI > 5 || ODKLON",
        ".Druh = \"R\" And (Math.Max(.ZpozdeniSkutecnePrijezd \\ 60, .ZpozdeniSkutecneOdjezd \\ 60) > 5 Or .JeOdklonen)")]
    [DataRow("PRIZNAK(Prizn_M)", "(.PriznakyVlaku And VlajkyVarianty.M) <> 0")]
    [DataRow("PRIZNAK(Prizn_Vyl)", ".VylukaPrij Or .VylukaOdj")]
    [DataRow("PRIZNAK(Prizn_Pre)", "(.PriznakyStanice And VlajkyStanice.PR) <> 0")]
    [DataRow("PRIZNAK(Prizn_M) == 1", "If((.PriznakyVlaku And VlajkyVarianty.M) <> 0, 4096, 0) = 1")]
    [DataRow("PRIZNAK(Prizn_M) & 4096", "(If((.PriznakyVlaku And VlajkyVarianty.M) <> 0, 4096, 0) And 4096) <> 0")]
    [DataRow("STAV(Stav_Stoji)", "(.PriznakyStavu And NODEFLAGS.STOJI) <> 0")]
    [DataRow("STAV(Stav_Stoji | Stav_Odbaven)", "(.PriznakyStavu And NODEFLAGS.ODBAVEN) <> 0 And (.PriznakyStavu And NODEFLAGS.STOJI) <> 0")]
    [DataRow("POZICE(Poz_V)", ".JeVychozi")]
    [DataRow("POZICE == Poz_K && ZPOZDENIPRIJ >= 10",
        "If(.JeVychozi, 1, If(.JeKoncici, 3, 2)) = 3 And .ZpozdeniSkutecnePrijezd \\ 60 >= 10")]
    [DataRow("NAKLTYP", ".Druh = \"Nákl\"")]
    [DataRow("MISTNI == 0", "Not (.JeVychozi Or .JeKoncici)")]
    [DataRow("KOLEJODJ(\"1\")", ".KlicPoziceOdjSkut = \"K1\"")]
    [DataRow("KOLEJPRIJ(\"\")", ".KlicPozicePrijSkut = \"\"")]
    [DataRow("OPERATOR(\"ZSSK\")", "StrComp(.Dopravce, \"ZSSK\", CompareMethod.Text) = 0")]
    [DataRow("NAZVLAKU(\"Tatran\")", "StrComp(.NazVlaku, \"Tatran\", CompareMethod.Text) = 0")]
    [DataRow("LINKAODJ(\"S20\")", ".LinkaIDS(False) = \"S20\"")]
    [DataRow("ZMENALINKY", ".LinkaIDS(True) <> .LinkaIDS(False)")]
    [DataRow("DOSMERU(5612345)", ".DoSmeru(5612345)")]
    [DataRow("CILSTANICE(5612345)", ".CilStanice = 5612345")]
    [DataRow("CVLAKU(\"1234\")", ".CVlaku = \"1234\"")]
    [DataRow("CVLAKU(1234)", "CInt(Val(.CVlaku)) = 1234")]
    [DataRow("Typ(Typ_R) || Typ(Typ_Ex) || ODKLON", ".Druh = \"R\" Or .Druh = \"Ex\" Or .JeOdklonen")]
    [DataRow("(Typ(Typ_R) && ODKLON) || MISTNI", "(.Druh = \"R\" And .JeOdklonen) Or .JeVychozi Or .JeKoncici")]
    [DataRow("!(ZPOZDENIPRIJ > 5)", "Not (.ZpozdeniSkutecnePrijezd \\ 60 > 5)")]
    public void Podmienka_PrelozenaDoVb(string text, string expected)
    {
        var transpiler = Transpiler();

        Assert.AreEqual(expected, transpiler.Condition(text));
        Assert.IsEmpty(transpiler.Problems);
    }

    [TestMethod]
    [DataRow("ZPOZDENIPRIJ*60", "(.ZpozdeniSkutecnePrijezd \\ 60) * 60")]
    [DataRow("INDCAT8", ".IndCat8()")]
    [DataRow("Typ(Typ_R) ? 2 : 3", "If(.Druh = \"R\", 2, 3)")]
    [DataRow("-300", "-300")]
    [DataRow("7 / 2 + 7 % 2", "4")]
    [DataRow("ODD(CASODJ)", "(If(.CasOdj < 0, .CasPrij, .CasOdj) * 60) Mod 2")]
    [DataRow("Typ(Typ_R)", "If(.Druh = \"R\", 1, 0)")]
    [DataRow("ZPOZDENIPRIJ * 2 + 1", "(.ZpozdeniSkutecnePrijezd \\ 60) * 2 + 1")]
    [DataRow("CASPRIJ - (CASODJ - 60)", "If(.CasPrij < 0, .CasOdj, .CasPrij) * 60 - (If(.CasOdj < 0, .CasPrij, .CasOdj) * 60 - 60)")]
    [DataRow("DOBAPOBYTU + (0 - 5)", ".DobaPobytu + (-5)")]
    public void Cislo_PrelozeneDoVb(string text, string expected)
    {
        Assert.AreEqual(expected, Transpiler().Number(text));
    }

    [TestMethod]
    public void Spojenie_PodmienkaSoSpojkouOrMaVAndAlsoZatvorky()
    {
        var transpiler = Transpiler();

        Assert.AreEqual((".Druh = \"R\" Or .JeOdklonen", "(.Druh = \"R\" Or .JeOdklonen)"), transpiler.JoinableCondition("Typ(Typ_R) || ODKLON"));
        Assert.AreEqual((".Druh = \"R\" And .JeOdklonen", ".Druh = \"R\" And .JeOdklonen"), transpiler.JoinableCondition("Typ(Typ_R) && ODKLON"));
    }

    [TestMethod]
    [DataRow("POZICE", "If(.JeVychozi, 1, If(.JeKoncici, 3, 2)) = 2")]
    [DataRow("ZPOZDENIPRIJ & 3", "(.ZpozdeniSkutecnePrijezd \\ 60 And 3) = 2")]
    public void Porovnanie_CisloSOperatoromNizsejPriority(string text, string expected)
    {
        Assert.AreEqual(expected, Transpiler().NumberEquals(text, 2));
    }

    [TestMethod]
    public void Typ_IndexPodlaTrTypes()
    {
        var code = Transpiler().Number("TYP");

        Assert.AreEqual("If(.Druh = \"Os\", 0, If(.Druh = \"Sp\", 2, If(.Druh = \"R\", 26, If(.Druh = \"Ex\", 27, "
                        + "If(.Druh = \"RJ\", 39, If(.Druh = \"IC\", 49, If(.Druh = \"Nákl\", 74, 0)))))))", code);
    }

    [TestMethod]
    public void Miesto_PrichodovaTabulaBerieVylukuAMeskaniePrichodu()
    {
        var transpiler = Transpiler(ExprEvalSite.ArrivalTable);

        Assert.AreEqual(".VylukaPrij And .ZpozdeniSkutecnePrijezd \\ 60 > 0", transpiler.Condition("VYLUKAZDE && ZPOZDENI > 0"));
    }

    [TestMethod]
    public void Nikdy_NeznamaKolajADruhBezKlucaVlakMatNemoze()
    {
        var transpiler = Transpiler();

        Assert.AreEqual("False Or False", transpiler.Condition("KOLEJPRIJ(\"9\") || Typ(Typ_Sl)"));
        Assert.IsEmpty(transpiler.Problems);
    }

    [TestMethod]
    public void Problem_StavKtoryInissDvaNepozna()
    {
        var transpiler = Transpiler();

        Assert.AreEqual("False", transpiler.Condition("STAV(1)"));
        Assert.AreEqual(new Iniss2ExprProblem(Iniss2ExprIssue.UnknownState, "0x1"), transpiler.Problems.Single());
    }

    [TestMethod]
    public void Chyba_VyrazSaNedaPrelozit()
    {
        var transpiler = Transpiler();

        Assert.IsNull(transpiler.Condition("Typ("));
        Assert.AreEqual(Iniss2ExprIssue.ParseError, transpiler.Problems.Single().Issue);
    }

    [TestMethod]
    public void Cakanie_UdalostiGtn()
    {
        var transpiler = Transpiler();

        Assert.AreEqual("(.PriznakyGTN And StaniceVlaku.FLAGS.OVC) <> 0", transpiler.Wait("OVC"));
        Assert.AreEqual("(.PriznakyGTN And StaniceVlaku.FLAGS.VVC) <> 0 And (.PriznakyGTN And StaniceVlaku.FLAGS.Vj) <> 0",
            transpiler.Wait("VVC | Vj"));
        Assert.IsNull(transpiler.Wait("0"));
        Assert.AreEqual("(.PriznakyGTN And StaniceVlaku.FLAGS.VVC) <> 0", Iniss2ExpressionTranspiler.WaitPath);
    }

    [TestMethod]
    public void Telo_ReturnPriSloveVRetazci()
    {
        Assert.AreEqual(".JeOdklonen", Iniss2ExpressionTranspiler.Body(".JeOdklonen"));
        Assert.AreEqual("Return (.Dopravce = \"return x\")", Iniss2ExpressionTranspiler.Body("(.Dopravce = \"return x\")"));
    }
}
