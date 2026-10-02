using System.Diagnostics.CodeAnalysis;
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
    private static StateDgmEvent Apply(StateDgmEvent e)
    {
        using var font = new Font("Consolas", 9);
        using var form = new Form { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new Point(-2000, -2000) };
        var editor = new SdEventEditor(new SdEditorContext(null, ["Přijíždí", "Odjede"], font));
        form.Controls.Add(editor);
        editor.Bind(e, null, ["Odjede"], [], 1);
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
    public void Apply_TriedaBezHlasenia_TypHlaseniaSaZmaze()
    {
        var e = Apply(new StateDgmEvent { Key = "#Kolej", Class = "SDEventWithDialog", Dialog = "SDDlgKolej", ReportKey = "Odjede" });

        Assert.IsNull(e.ReportKey);
    }
}
