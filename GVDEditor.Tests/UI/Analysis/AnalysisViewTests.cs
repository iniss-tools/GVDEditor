using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using GVDEditor.Domain.Analysis;
using GVDEditor.Properties;
using GVDEditor.UI.Analysis;

namespace GVDEditor.Tests.UI.Analysis;

/// <summary>
/// Panel Analyza grafikonu bez hlavneho okna: filtre zavaznosti s poctom, poradie problemov a zastaraly vysledok po zmene
/// grafikonu, pri ktorom sa oprava neda spustit.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class AnalysisViewTests
{
    private sealed record Problem(string Text, ProblemType ProblemType) : IProblem
    {
        public string Solution => "";

        public FixType FixType => FixType.Auto;

        public FixResult FixProblem() => FixResult.Done;
    }

    private static readonly List<IProblem> Problems =
    [
        new Problem("info", ProblemType.Hint), new Problem("upozornenie", ProblemType.Warning), new Problem("chyba", ProblemType.Error),
        new Problem("druha chyba", ProblemType.Error)
    ];

    private static AnalysisView View(List<IProblem>? problems)
    {
        var view = new AnalysisView(_ => problems is null ? null : Task.FromResult(problems), _ => { });
        view.CreateControl();
        return view;
    }

    private static T Field<T>(Control control, string name) =>
        (T)control.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(control)!;

    private static List<string> Texts(AnalysisView view) =>
        view.Grid.Rows.Cast<DataGridViewRow>().Select(row => ((IProblem)row.DataBoundItem!).Text).ToList();

    [TestMethod]
    public void Analyza_ChybyNaZaciatkuAFiltreSPoctom()
    {
        using var view = View(Problems);

        view.AnalyzeAsync().GetAwaiter().GetResult();

        CollectionAssert.AreEqual(new[] { "chyba", "druha chyba", "upozornenie", "info" }, Texts(view));
        var errors = Field<ToolStripButton>(view, "tsbErrors");
        Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, Resources.Analysis_Errors, 2), errors.Text);

        errors.Checked = false;

        CollectionAssert.AreEqual(new[] { "upozornenie", "info" }, Texts(view));
        Assert.AreEqual(2, view.Count(ProblemType.Error), "pocet ostava aj pre skryte");
    }

    [TestMethod]
    public void Analyza_ZmenaGrafikonuZablokujeOpravuDoNovejAnalyzy()
    {
        using var view = View(Problems);
        view.AnalyzeAsync().GetAwaiter().GetResult();
        view.Grid.CurrentCell = view.Grid.Rows[0].Cells[1];
        var fix = Field<ToolStripButton>(view, "tsbFix");
        Assert.IsTrue(fix.Enabled);

        view.MarkStale();

        Assert.IsFalse(fix.Enabled);
        Assert.AreEqual(Resources.Analysis_Stale, Field<ToolStripLabel>(view, "tslStatus").Text);

        view.AnalyzeAsync().GetAwaiter().GetResult();

        Assert.IsTrue(fix.Enabled, "vybrany problem ostal vybrany");
        Assert.AreEqual("", Field<ToolStripLabel>(view, "tslStatus").Text);
    }

    [TestMethod]
    public void Analyza_BezGrafikonuPrazdnyZoznamBezTextu()
    {
        using var view = View(null);

        view.AnalyzeAsync().GetAwaiter().GetResult();
        view.MarkStale();

        Assert.AreEqual(0, view.Grid.Rows.Count);
        Assert.AreEqual("", Field<ToolStripLabel>(view, "tslStatus").Text);
    }
}
