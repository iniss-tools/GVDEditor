namespace GVDEditor.Mcp.Results;

/// <summary>Hodnota na vyber.</summary>
internal sealed record OptionInfo(
    [property: Description("Value to use in the tools.")] string Value,
    [property: Description("Name in the editor.")] string Name,
    [property: Description("More information; null = none.")] string? Note);
