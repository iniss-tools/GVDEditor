namespace GVDEditor.Mcp.Results;

/// <summary>Krok skladania textu stlpca.</summary>
internal sealed record ComposeStepInfo(
    [property: Description("Source in the fixed order (#VYLUKA … #SWITCH, #MERGE, TTexts, own value, DIVTYPE rules).")] string Source,
    string Text,
    [property: Description("Font number; null = unchanged.")] int? Font,
    [property: Description("Which item applied, whether the text was kept for @ …")] string Note);
