using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.Tools;
using ToolsCore.Iniss2.Grafikon;

namespace GVDEditor.DocScreenshots;

/// <summary>
/// Ukážková inštalácia INISS2 pre snímky sprievodcu exportu: minuloročný grafikon so stanicou Horná Lehota, ktorá má
/// stavový diagram a hlásenia (šablóna), a banky písem ELEN.
/// </summary>
internal static class DemoIniss2
{
    private const string Group = "2026";
    private const string TemplateDirectory = "01 Horná Lehota";
    private const int TemplateStation = 9900500;

    /// <summary>
    /// Zostaví inštaláciu v <paramref name="root" /> (priečinok vytvorený harnessom sa najprv zmaže).
    /// </summary>
    /// <returns>priečinok inštalácie</returns>
    public static string Build(string root)
    {
        var marker = Path.Combine(root, ".docshots");
        if (Directory.Exists(root))
        {
            if (!File.Exists(marker))
                throw new InvalidOperationException($"Priečinok {root} existuje a nevytvoril ho harness – nezmaže sa.");
            Directory.Delete(root, true);
        }

        Directory.CreateDirectory(root);
        File.WriteAllText(marker, "Ukážková inštalácia INISS2 pre GVDEditor.DocScreenshots – pri ďalšom spustení sa zmaže.");

        // podľa Config.xml GVDEditor priečinok spozná ako inštaláciu INISS2
        var dataDir = Path.Combine(root, "Server", "Data");
        Directory.CreateDirectory(dataDir);
        File.WriteAllText(Path.Combine(dataDir, "Config.xml"), "<?xml version=\"1.0\" encoding=\"windows-1250\"?>\r\n<Config />\r\n", Encodings.Win1250);

        var station = Path.Combine(dataDir, Group, TemplateDirectory);
        Directory.CreateDirectory(station);
        DirListXml.AddStation(dataDir, Group, new DirListItem(TemplateDirectory, TemplateStation, "", "", DirListItemFlags.None));
        DirListXml.WriteStationNumber(station, TemplateStation);
        File.WriteAllText(Path.Combine(station, "statedgm.xml"), "<?xml version=\"1.0\" encoding=\"windows-1250\"?>\r\n<STATEDGM />\r\n",
            Encodings.Win1250);
        ReportKeysFile.Write(station,
        [
            new ReportKey("Přijede", "P", "V P K"),
            new ReportKey("Vjíždí", "V", "P K"),
            new ReportKey("Přijel", "L", "P K"),
            new ReportKey("Nástup", "N", "V P"),
            new ReportKey("Odjede", "O", "V P")
        ]);

        var fonts = Path.Combine(dataDir, "TableFonts");
        Directory.CreateDirectory(fonts);
        Bank("10_DEMO").Save(Path.Combine(fonts, "10_demo.bin"));
        Bank("16_DEMO").Save(Path.Combine(fonts, "16_demo.bin"));
        return root;
    }

    // banka s jedným prázdnym písmom - sprievodca z nej potrebuje len názov
    private static ElenFontFile Bank(string name)
    {
        const int height = 7;
        var glyphs = Enumerable.Range(0, ElenFont.GlyphCount).Select(_ => new ElenGlyph(5, height, 1, new byte[height])).ToList();
        return new ElenFontFile(name, [new ElenFont("Normal", 5, height, false, glyphs)]);
    }
}
