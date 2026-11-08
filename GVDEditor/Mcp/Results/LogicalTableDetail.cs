namespace GVDEditor.Mcp.Results;

/// <summary>Logicka tabula - co sa posiela na ktore fyzicke tabule (get_table).</summary>
internal sealed record LogicalTableDetail(string Key, string Name, string Comment,
    [property: Description("View type key.")] string? ViewType,
    [property: Description("Number of records (trains) the board shows.")] int Records,
    [property: Description("Station number (IDSTATION); 0 = none.")] int Station,
    [property: Description("Where the records go, as rows of physical boards; null = the placement cannot be written as rows (see placements).")]
    IReadOnlyList<LayoutRowInfo>? Layout,
    [property: Description("Placement of each record when layout is null: physical board, row from 0 and view type.")] IReadOnlyList<string>? Placements);
