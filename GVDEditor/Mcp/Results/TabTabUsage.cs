namespace GVDEditor.Mcp.Results;

/// <summary>Stlpec katalogovej tabule, ktory sekciu pouziva.</summary>
internal sealed record TabTabUsage(
    [property: Description("Catalog board key.")] string Catalog,
    [property: Description("Column (item) name.")] string Column,
    [property: Description("TAB1 or TAB2.")] string Table,
    [property: Description("DIVTYPE 0-4 of the column - how the section is applied (see tabtab_reference); null = not set.")] int? DivType,
    [property: Description("Physical boards (keys) of this catalog type.")] IReadOnlyList<string> Boards);
