namespace GVDEditor.Mcp.Results;

/// <summary>Linka stanice v simulatore tabul.</summary>
internal sealed record SimulatorLineInfo(
    [property: Description("Line number.")] int Line,
    [property: Description("Driver section of the INISS settings (Driver, Driver0...).")] string Section,
    [property: Description("Table class of the line (protocol family).")] int Class,
    [property: Description("Port of the line on the simulator.")] int Port,
    [property: Description("Boards and amplifier units on the line.")] int Boards,
    [property: Description("The .INI next to the INISS program already redirects the line to the simulator (the port is taken from it).")] bool Redirected);
