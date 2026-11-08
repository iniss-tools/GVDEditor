using GVDEditor.Integration.Iniss2;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.TabTab;

namespace GVDEditor.Tests.Integration.Iniss2;

/// <summary>
/// Prevod pravidiel TabTab INISS 3.39 na vyraz VB.NET tabulky INISS2.
/// </summary>
[TestClass]
public class Iniss2TabTabTranspilerTests
{
    private static readonly Dictionary<string, int> TrainTypes = new(StringComparer.Ordinal) { ["Os"] = 0, ["R"] = 26 };

    private static Iniss2TabTabTranspiler Transpiler() => new(TrainTypes, name => name == "1" ? "K1" : null);

    private static readonly Iniss2TabTabUse Departure = new("Odchody", ExprEvalSite.Default, true);

    [TestMethod]
    public void BezUdalosti_ZiadnyVyraz()
    {
        Assert.AreEqual("", Transpiler().Expression(TabTabSection.Parse("R{65} = R"), [Departure]));
        Assert.AreEqual("", Transpiler().Expression(TabTabSection.Parse("Bus = #VYLUKA"), []), "tabulku nepouziva ziadna predloha");
    }

    [TestMethod]
    public void Udalosti_VPoradiInissu()
    {
        var code = Transpiler().Expression(TabTabSection.Parse("Odklon =#ODKLON\nBus =#VYLUKA\nKolaj 1{70} =#POZODJ_1\nX =#POZODJ_9"), [Departure]);

        var lines = code.Split('\n');
        var lockout = Array.FindIndex(lines, line => line.StartsWith("If (.VylukaPrij Or .VylukaOdj) AndAlso accept(New TabString(\"Bus\", -1))", StringComparison.Ordinal));
        var deflection = Array.FindIndex(lines, line => line.StartsWith("If .JeOdklonen AndAlso accept(New TabString(\"Odklon\", -1))", StringComparison.Ordinal));
        var track = Array.FindIndex(lines, line => line == "If .KlicPoziceOdjSkut = \"K1\" AndAlso accept(New TabString(\"Kolaj 1\", 70)) Then Return r");
        Assert.IsTrue(lockout >= 0 && lockout < deflection && deflection < track, code);
        Assert.IsFalse(code.Contains("POZODJ_9", StringComparison.Ordinal) || code.Contains("\"X\"", StringComparison.Ordinal), "kolaj mimo grafikonu vlak mat nemoze");
        Assert.EndsWith("If(p.iFont <> -1, p.iFont, If(sOrig Is Nothing, -1, sOrig.iFont)))", code);
    }

    [TestMethod]
    public void Switch_PodmienkyATextSoZavinacom()
    {
        var code = Transpiler().Expression(TabTabSection.Parse("Typ(Typ_R), \"@\"{93}, \\\n1, \"-\" =#SWITCH"), [Departure]);

        StringAssert.Contains(code, "If (.Druh = \"R\") Then\n    v = New TabString(\"@\", 93)\nElseIf True Then\n    v = New TabString(\"-\", -1)\nElse\n    v = Nothing\nEnd If\nIf v IsNot Nothing AndAlso accept(v) Then Return r", code);
        StringAssert.Contains(code, "If t.sString.Contains(\"@\") Then", "text s @ caka na dalsi zdroj");
    }

    [TestMethod]
    public void Merge_TextInehoStlpcaAStriedanieCasti()
    {
        var section = TabTabSection.Parse("(Typ(Typ_R)),,\"@\"{81},\\\n(!LinkaOdj(\"\")),\"#\",%Linka% = #MERGE");

        var code = Transpiler().Expression(section, [Departure]);

        StringAssert.Contains(code, "If (.Druh = \"R\") Then v = merge(v, \"\", New TabString(\"@\", 81), True)", code);
        StringAssert.Contains(code, "If (Not (.LinkaIDS(False) = \"\")) Then v = merge(v, \"#\", peer(\"Linka\", -1), True)", code);
        StringAssert.Contains(code, "Dim peer = Function(name As String, font As Integer) As TabString", code);
        StringAssert.Contains(code, "Dim merge = Function(acc As TabString, sep As String, t As TabString, byParts As Boolean) As TabString", code);
    }

    [TestMethod]
    public void Predlohy_RozneMiestoVetviPodlaKlucaPredlohy()
    {
        var code = Transpiler().Expression(TabTabSection.Parse("Bus =#VYLUKA"),
            [Departure, new Iniss2TabTabUse("Prichody", ExprEvalSite.ArrivalTable, true), new Iniss2TabTabUse("Nastupiste", ExprEvalSite.Default, false)]);

        StringAssert.Contains(code, "Select Case oDiv.oTable.sKey\n    Case \"Prichody\"\n        If .VylukaPrij AndAlso", code);
        StringAssert.Contains(code, "    Case Else\n        If (.VylukaPrij Or .VylukaOdj) AndAlso", code);
        Assert.IsFalse(code.Contains("Case \"Nastupiste\"", StringComparison.Ordinal), "striedanie casti bez #MERGE vyraz nemeni");
    }

    [TestMethod]
    public void Merge_TabulaBezStriedaniaSpajaLenZretazenim()
    {
        var code = Transpiler().Expression(TabTabSection.Parse("1,\" \",\"A#B\" = #MERGE"),
            [Departure, new Iniss2TabTabUse("Lcd", ExprEvalSite.Default, false)]);

        StringAssert.Contains(code, "Select Case oDiv.oTable.sKey", code);
        StringAssert.Contains(code, "' 1,\" \",\"A#B\" = #MERGE", code);
        StringAssert.Contains(code, "If True Then v = merge(v, \" \", New TabString(\"A#B\", -1), False)", code);
        StringAssert.Contains(code, "If True Then v = merge(v, \" \", New TabString(\"A#B\", -1), True)", code);
    }

    [TestMethod]
    public void Text_RiadiaceZnakyCezChrW()
    {
        var code = Transpiler().Expression(TabTabSection.Parse("\\u007Fx =#VYLUKA".Replace("\\u007F", "\u007F", StringComparison.Ordinal)), [Departure]);

        StringAssert.Contains(code, "accept(New TabString((ChrW(127) & \"x\"), -1))", code);
    }
}
