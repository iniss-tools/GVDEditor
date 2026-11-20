using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// Predlohy stavoveho diagramu, ktore sa ponukaju pri novom grafikone a v editore diagramu.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class StateDgmFileTests
{
    [TestMethod]
    public void TemplateText_KazdaPredlohaMaVlastnyText()
    {
        var texts = Enum.GetValues<StateDgmTemplate>().ToDictionary(t => t, StateDgmFile.TemplateText);

        // chybajuca vetva v TemplateText by vratila slovensku predlohu
        Assert.AreEqual(texts.Count, texts.Values.Distinct().Count());
        foreach (var (template, text) in texts)
        {
            var diagram = StateDgmDiagram.Parse(text);
            Assert.AreEqual(0, diagram.Warnings.Count, $"{template}: {string.Join("; ", diagram.Warnings)}");
        }
    }

    [TestMethod]
    public void TemplateText_CeskaGtnMaCeskeNazvyAAutomatikuGtn()
    {
        var czech = StateDgmDiagram.Parse(StateDgmFile.TemplateText(StateDgmTemplate.Czech));
        var slovakGtn = StateDgmDiagram.Parse(StateDgmFile.TemplateText(StateDgmTemplate.SlovakGtn));
        var czechGtn = StateDgmDiagram.Parse(StateDgmFile.TemplateText(StateDgmTemplate.CzechGtn));

        CollectionAssert.AreEqual(czech.Designs.Select(d => d.Key).ToList(), czechGtn.Designs.Select(d => d.Key).ToList());
        CollectionAssert.AreEqual(States(czech).Select(s => s.Name).ToList(), States(czechGtn).Select(s => s.Name).ToList());
        CollectionAssert.AreEqual(States(slovakGtn).Select(Automation).ToList(), States(czechGtn).Select(Automation).ToList());
    }

    private static IEnumerable<StateDgmState> States(StateDgmDiagram diagram) => diagram.Categories.SelectMany(c => c.States);

    private static string Automation(StateDgmState state) =>
        $"{state.AutoMode?.Text}|{state.AutoTimePoint?.Text}|{state.AutoTimePointAdd?.Text}|{state.Wait?.Text}";
}
