namespace GVDEditor.Mcp.Results;

/// <summary>Tabula alebo pismo s pouzitim a kontrolou (get_table).</summary>
internal sealed record TableDetail(
    [property: Description("catalog, physical, logical or font - the one filled below.")] string Kind,
    CatalogTableDetail? Catalog,
    PhysicalTableDetail? Physical,
    LogicalTableDetail? Logical,
    FontDetail? Font,
    [property: Description("Where it is used; a used item cannot be deleted.")] IReadOnlyList<string> UsedBy,
    [property: Description("Errors of the GVDEditor check (INISS would reject them), in the language of the editor.")] IReadOnlyList<string> Problems,
    [property: Description("Warnings INISS tolerates, in the language of the editor.")] IReadOnlyList<string> Warnings);
