namespace GVDEditor.Mcp.Results;

/// <summary>Hodnoty tabul a pisiem (table_options).</summary>
internal sealed record TableOptions(
    [property: Description("Manufacturers (protocols) of catalog boards; note = description and address range.")] IReadOnlyList<OptionInfo> Manufacturers,
    [property: Description("Column contents (FILL_SECTION).")] IReadOnlyList<OptionInfo> Contents,
    [property: Description("Column alignments.")] IReadOnlyList<OptionInfo> Aligns,
    [property: Description("Column DIVTYPE - how TAB1/TAB2 apply.")] IReadOnlyList<OptionInfo> DivTypes,
    [property: Description("View types of boards.")] IReadOnlyList<OptionInfo> ViewTypes,
    [property: Description("View modes; every view type of a catalog board lists columns for all of them.")] IReadOnlyList<OptionInfo> ViewModes,
    [property: Description("Font types.")] IReadOnlyList<OptionInfo> FontTypes);
