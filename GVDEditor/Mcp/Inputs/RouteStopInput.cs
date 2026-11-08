namespace GVDEditor.Mcp.Inputs;

/// <summary>Stanica trasy vlaku (set_train).</summary>
internal sealed record RouteStopInput(
    [property: Description("Station name or number (the number when more stations have the name).")] string Station,
    [property: Description("Announced in the short announcement.")] bool Short = false,
    [property: Description("Announced in the long announcement.")] bool Long = true);
