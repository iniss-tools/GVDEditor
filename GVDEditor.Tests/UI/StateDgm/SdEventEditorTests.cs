using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.UI.Controls;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.Tests.UI.StateDgm;

/// <summary>
/// Editor akcie stavoveho diagramu: zapis hodnot po zatvoreni dialogu (okno uz nie je zobrazene).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class SdEventEditorTests
{
    // typ Ukončit nástup ma zameneny kluc a nazov (nazov je standardny kluc Odjede)
    private static readonly ReportType[] Types = [new("Přijíždí", "Prichádza", "P"), new("Odjede", "Odchádza", "O"), new("Ukončit nástup", "Odjede", "U")];

    private static StateDgmEvent Apply(StateDgmEvent e, bool useSuggestion = false)
    {
        using var font = new Font("Consolas", 9);
        using var form = new Form { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new Point(-2000, -2000) };
        var editor = new SdEventEditor(new SdEditorContext(null, Types, font));
        form.Controls.Add(editor);
        editor.Bind(e, null, ["Odjede"], [], 1);
        if (useSuggestion) editor.UseSuggestedReport();
        form.Show();
        // hlavne okno vola Apply az po ShowDialog - dialog je vtedy skryty a jeho prvky maju Visible = false
        form.Hide();
        editor.Apply(e, null);
        return e;
    }

    [TestMethod]
    [DataRow("SDEventUniPos")]
    [DataRow("SDEventReportAboutState")]
    [DataRow("SDEventVlakAttr")]
    public void Apply_BezZmeny_ZachovaTypHlasenia(string cls)
    {
        var e = Apply(new StateDgmEvent { Key = "#GoToOdíde", Class = cls, NextState = "Odjede", ReportKey = "Odjede" });

        Assert.AreEqual("Odjede", e.ReportKey);
    }

    [TestMethod]
    public void Apply_HodnotaMimoZoznamu_ZostaneNezmenena()
    {
        var e = Apply(new StateDgmEvent { Key = "#GoToOdíde", Class = "SDEventUniPos", ReportKey = "Ukončit nástup" });

        Assert.AreEqual("Ukončit nástup", e.ReportKey);
    }

    [TestMethod]
    [DataRow("Odjede", "Odjede")]
    [DataRow("Ukončit nástup", "Ukončit nástup")]
    [DataRow("přijíždí", "Přijíždí")]
    [DataRow("Příde", "Příde")]
    public void UseSuggestedReport_NeznamyTyp_NavrhnePodlaNazvuAleboKluca(string key, string expected)
    {
        // "Odjede" je platny kluc, nemeni sa; "přijíždí" sa lisi len velkostou pismen; "Příde" nema navrh
        var e = Apply(new StateDgmEvent { Key = "#GoToOdíde", Class = "SDEventUniPos", ReportKey = key }, true);

        Assert.AreEqual(expected, e.ReportKey);
    }

    [TestMethod]
    public void SuggestReport_NazovTypu_VratiKlucTypu()
    {
        using var font = new Font("Consolas", 9);
        var ctx = new SdEditorContext(null, [new ReportType("Přijíždí", "Prichádza", "P"), new ReportType("Ukončit nástup", "Odjede", "U")], font);

        Assert.IsTrue(ctx.IsUnknownReport("Odjede"));
        Assert.AreEqual("Ukončit nástup", ctx.SuggestReport("Odjede")?.Key);
        Assert.IsFalse(ctx.IsUnknownReport("Ukončit nástup"));
    }

    [TestMethod]
    public void IsUnknownReport_BezTypovHlaseni_NekontrolujeSa()
    {
        using var font = new Font("Consolas", 9);
        var ctx = new SdEditorContext(null, [], font);

        Assert.IsFalse(ctx.IsUnknownReport("Odjede"));
    }

    [TestMethod]
    public void Apply_TriedaBezHlasenia_TypHlaseniaSaZmaze()
    {
        var e = Apply(new StateDgmEvent { Key = "#Kolej", Class = "SDEventWithDialog", Dialog = "SDDlgKolej", ReportKey = "Odjede" });

        Assert.IsNull(e.ReportKey);
    }
}
