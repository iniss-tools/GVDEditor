namespace GVDEditor.Mcp.Inputs;

/// <summary>Riadok katalogovej tabule (set_catalog_table).</summary>
internal sealed record CatalogRowInput(
    [property: Description("Row height in points (usually the minimal row height).")] int Height,
    [property: Description("Row width in points (width of the board).")] int Width,
    [property: Description("Size kept for the original configuration tool (typically 15); INISS ignores it.")] int Size = 15);
