using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Farby druhov vlakov v stlpci Typ vlaku tabulky vlakov (polozky <see cref="GvdColors" />).
/// </summary>
public sealed class TrainTypeColumnScheme
{
    /// <summary>Osobny vlak.</summary>
    public ColorSetting Os { get; } = new();

    /// <summary>Rychlik.</summary>
    public ColorSetting R { get; } = new();

    /// <summary>Vlak vyssej kvality.</summary>
    public ColorSetting X { get; } = new();

    /// <summary>Sluzobny vlak.</summary>
    public ColorSetting Sl { get; } = new();
}
