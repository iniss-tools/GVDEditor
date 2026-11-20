using System.Diagnostics.CodeAnalysis;
using GVDEditor.Config;
using GVDEditor.UI.Controls;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Tests.UI.Controls;

/// <summary>
/// Nahlad pisma tabule: text pismom zo suboru pisiem ELEN, ktore cislo vyberie, inak pismom Windows.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class LedPreviewTests
{
    private DirectoryInfo _dir = null!;

    [TestInitialize]
    public void Init() => _dir = Directory.CreateTempSubdirectory("gvd-ledpreview-");

    [TestCleanup]
    public void Cleanup() => _dir.Delete(true);

    /// <summary>Pismo vysky 16, kazdy znak sirky <paramref name="width" /> s rozsvietenym lavym stlpcom.</summary>
    private static ElenFont Font(string name, int width)
    {
        var glyphs = Enumerable.Range(0, ElenFont.GlyphCount)
            .Select(_ => new ElenGlyph(width, 16, 1, Enumerable.Repeat((byte)0x80, 16).ToArray()))
            .ToArray();
        return new ElenFont(name, 8, 16, false, glyphs);
    }

    private string SaveFonts(params ElenFont[] fonts)
    {
        var path = Path.Combine(_dir.FullName, "PISMA.bin");
        new ElenFontFile("PISMA", fonts).Save(path);
        return path;
    }

    [STATestMethod]
    public void Nahlad_PismoZoSuboru_KresliSaJehoZnakmi()
    {
        using var led = new LedPreview();
        led.Fonts = ElenFontFile.Load(SaveFonts(Font("A", 5), Font("B", 7)));

        // rez 1 (tenke) = pismo 1 suboru; "R 612 Košice" ma 12 znakov
        led.FontId = 0x52;

        Assert.IsTrue(led.UsesFontFile);
        Assert.AreEqual(12 * 7, led.Raster!.Width);
        Assert.AreEqual(2, led.DotColor);
    }

    [STATestMethod]
    public void Nahlad_PismoVSubereNie_PismoWindows()
    {
        using var led = new LedPreview();
        led.Fonts = ElenFontFile.Load(SaveFonts(Font("A", 5)));

        led.FontId = 0x62;

        Assert.IsFalse(led.UsesFontFile);
        Assert.IsEmpty(led.Raster!.Glyphs);
    }

    [TestMethod]
    public void SuborPisiem_Poskodeny_ChybaANahladBezSuboru()
    {
        var path = Path.Combine(_dir.FullName, "ZLY.bin");
        File.WriteAllBytes(path, [1, 2, 3]);
        var config = new GVDEditorConfig { ElenPreviewFonts = path };
        var saved = 0;
        var fonts = new ElenPreviewFonts(config, null, () => saved++);

        Assert.IsNull(fonts.File);
        Assert.IsNotNull(fonts.Error);

        fonts.Set(SaveFonts(Font("A", 5)));

        Assert.IsNotNull(fonts.File);
        Assert.IsNull(fonts.Error);
        Assert.AreEqual(1, saved);
        Assert.AreEqual(Path.Combine(_dir.FullName, "PISMA.bin"), config.ElenPreviewFonts);
    }
}
