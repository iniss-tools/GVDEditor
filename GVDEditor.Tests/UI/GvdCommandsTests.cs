using System.Xml;
using System.Xml.Serialization;
using GVDEditor.Config;
using GVDEditor.UI;

namespace GVDEditor.Tests.UI;

/// <summary>
/// Prikazy hlavneho okna a ich skratky v config.xml.
/// </summary>
[TestClass]
public class GvdCommandsTests
{
    [TestMethod]
    public void All_IdentifikatoryAjPredvoleneSkratky_SuJedinecne()
    {
        var all = GvdCommands.All;

        Assert.HasCount(all.Count, all.Select(c => c.Id).Distinct().ToList());
        var shortcuts = all.Where(c => c.DefaultKeys != Keys.None).Select(c => c.DefaultKeys).ToList();
        Assert.HasCount(shortcuts.Count, shortcuts.Distinct().ToList());
        Assert.IsTrue(all.All(c => !string.IsNullOrWhiteSpace(c.Text)));
    }

    [TestMethod]
    public void Settings_DalsieOkna_JedinecneIdentifikatoryAPredvoleneSkratkyVKazdomOkne()
    {
        var all = GvdCommands.Settings;

        Assert.HasCount(all.Count, all.Select(c => c.Id).Distinct().ToList());
        foreach (var window in all.GroupBy(c => c.Window))
        {
            var shortcuts = window.Where(c => c.DefaultKeys != Keys.None).Select(c => c.DefaultKeys).ToList();
            Assert.HasCount(shortcuts.Count, shortcuts.Distinct().ToList(), window.Key ?? "hlavne okno");
        }

        Assert.IsTrue(GvdCommands.TabTabAll.All(c => c.Window is not null && c.Id.StartsWith("TabTab.", StringComparison.Ordinal)));
    }

    /// <summary>
    /// Nazvy prvkov skratiek v config.xml z predchadzajucich verzii - skratky nastavene pouzivatelom sa nesmu stratit.
    /// </summary>
    [TestMethod]
    public void All_ObsahujeVsetkyPrvkyZoStarsichVerzii()
    {
        string[] legacy =
        [
            "NewGVD", "OpenGVD", "ImportGVD", "ImportData", "Save", "Analyze", "AddTrain", "EditTrain", "DeleteTrains",
            "DuplicateTrain", "LSettings", "GSettings", "AppSettings", "GSGrafikony", "GSLangs", "GSMeskania", "GSTrainTypes",
            "GSAudio", "LSGrafikon", "LSJazyky", "LSStanice", "LSDopravcovia", "LSPlatforms", "LSKolaje", "LSTPhysicals",
            "LSTLogicals", "LSTCatalogs", "LSTabTab", "LSTTexts", "LSTFonts", "LSTabTabEditor", "RunINISS", "ShutdownINISS",
            "KillINISS", "RestartINISS", "InfoApp", "UpdateNotes", "DatObm"
        ];

        CollectionAssert.IsSubsetOf(legacy, GvdCommands.All.Select(c => c.Id).ToList());
    }

    [TestMethod]
    public void Config_StarySuborNastaveni_NacitaZmeneneSkratky()
    {
        const string xml = """
            <CONFIG>
              <Shortcuts>
                <OpenGVD sc="CtrlShiftO" />
                <Save sc="None" />
                <DatObm sc="F7" />
              </Shortcuts>
              <AutoTableText>true</AutoTableText>
            </CONFIG>
            """;

        using var reader = new StringReader(xml);
        var config = (GVDEditorConfig)new XmlSerializer(typeof(GVDEditorConfig)).Deserialize(XmlReader.Create(reader))!;
        var copy = config with { };

        Assert.AreEqual((Keys)Shortcut.CtrlShiftO, config.Shortcuts.Get(GvdCommands.Open));
        Assert.AreEqual((Keys)Shortcut.None, config.Shortcuts.Get(GvdCommands.Save));
        Assert.AreEqual((Keys)Shortcut.CtrlS, new GVDEditorConfig().Shortcuts.Get(GvdCommands.Save));
        Assert.IsTrue(config.AutoTableText);

        // kopia nastaveni (okno Nastavenia programu) nesmie menit povodne skratky
        copy.Shortcuts.Set(GvdCommands.Open.Id, Keys.F2);
        Assert.AreEqual((Keys)Shortcut.CtrlShiftO, config.Shortcuts.Get(GvdCommands.Open));
    }
}
