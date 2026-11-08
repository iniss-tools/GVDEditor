namespace GVDEditor.Mcp.Results;

/// <summary>Tabule otvoreneho grafikonu (list_tables).</summary>
internal sealed record TableList(
    IReadOnlyList<PhysicalTableInfo> Physical,
    IReadOnlyList<LogicalTableInfo> Logical,
    IReadOnlyList<CatalogTableInfo> Catalog,
    IReadOnlyList<TableTextInfo> Texts,
    [property: Description("Board fonts: id - name.")] IReadOnlyList<string> Fonts,
    [property: Description("TabTab sections (keys); text by get_tabtab.")] IReadOnlyList<string> TabTabs);
