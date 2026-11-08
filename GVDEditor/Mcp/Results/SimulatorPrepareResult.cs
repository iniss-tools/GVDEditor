namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok prepare_simulator.</summary>
internal sealed record SimulatorPrepareResult(
    [property: Description("Dry run only - nothing was sent to the simulator.")] bool DryRun,
    [property: Description("Web address of the simulator.")] string Url,
    [property: Description("Run configuration the lines come from.")] string Configuration,
    IReadOnlyList<SimulatorLineInfo> Lines,
    [property: Description("Boards with XML export the simulator shows as virtual boards.")] int XmlTables,
    [property: Description("INISS log folder where INISS writes the XML of boards.")] string? LogFolder,
    [property: Description("What the simulator did; null = dry run.")] SimulatorImportInfo? Result,
    [property: Description("What is left to do, e.g. lines INISS does not send to the simulator yet.")] IReadOnlyList<string> Notes);
