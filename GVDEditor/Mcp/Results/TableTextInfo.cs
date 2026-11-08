namespace GVDEditor.Mcp.Results;

/// <summary>Text do tabule.</summary>
internal sealed record TableTextInfo(string Key, string Name,
    [property: Description("Number of realizations on catalog boards.")] int Realizations,
    [property: Description("Number of trains with their own text.")] int Trains);
