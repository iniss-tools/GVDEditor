using System.Xml.Linq;
using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Farebna schema GVDEditora: ovladacie prvky, druhy vlakov v tabulke vlakov a editor TabTab.
/// </summary>
public class GVDEditorStyle : Style
{
    private static readonly IReadOnlyList<ColorKey> AllKeys = [.. UiColors.Keys, .. GvdColors.Keys];

    /// <summary>
    /// Farby druhov vlakov v stlpci Typ vlaku.
    /// </summary>
    public TrainTypeColumnScheme TrainTypeColumnScheme { get; } = new();

    /// <summary>
    /// Farby editora TabTab.
    /// </summary>
    public TabTabEditorScheme TabTabEditorScheme { get; } = new();

    /// <summary>
    /// Pismo editora zo stareho suboru stylov (dnes v nastaveniach pisiem programu); <see langword="null" /> = styl
    /// ho neobsahoval.
    /// </summary>
    public FontChoice? LegacyEditorFont { get; private set; }

    /// <inheritdoc />
    public override IReadOnlyList<ColorKey> ColorKeys => AllKeys;

    /// <inheritdoc />
    protected override Style CreateEmpty() => new GVDEditorStyle();

    /// <inheritdoc />
    protected override void ReadLegacy(XElement element)
    {
        if (element.Element("TabTabEditor")?.Element("Font") is not { } font)
            return;

        var family = (string?)font.Attribute("family");
        var size = (float?)font.Attribute("size");
        if (!string.IsNullOrEmpty(family) && size is > 0)
            LegacyEditorFont = new FontChoice(family, size.Value);
    }
}
