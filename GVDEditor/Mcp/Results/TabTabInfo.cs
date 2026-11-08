namespace GVDEditor.Mcp.Results;

/// <summary>Sekcia TabTab (get_tabtab).</summary>
internal sealed record TabTabInfo(string Key,
    [property: Description("Program text of the section (INISS board language, see tabtab_reference).")] string Text,
    [property: Description("All findings of the GVDEditor check with line numbers (the same check as the TabTab editor and the analysis).")]
    IReadOnlyList<TabTabDiagnosticInfo> Diagnostics,
    [property: Description("Font numbers {n} used in the section, decoded.")] IReadOnlyList<FontCodeInfo> Fonts,
    [property: Description("Board columns using this section; empty = no catalog board refers to it.")] IReadOnlyList<TabTabUsage> UsedBy);
