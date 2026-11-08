namespace GVDEditor.Mcp.Results;

/// <summary>Fyzicka tabula - konkretna tabula na stanici (get_table).</summary>
internal sealed record PhysicalTableDetail(string Key, string Name, string Comment,
    [property: Description("Catalog board key - the type of the board.")] string? Catalog,
    [property: Description("Address on the communication line; -1 = none.")] int Address,
    [property: Description("Communication line number; 0 = assigned automatically.")] int Line,
    [property: Description("Number of records the board shows.")] int Records,
    [property: Description("Records with reversed arrows; usually empty.")] string ReverseArrows,
    [property: Description("File name of the XML export; empty = no export.")] string XmlFile,
    [property: Description("Note for the operator (REM).")] string Note);
