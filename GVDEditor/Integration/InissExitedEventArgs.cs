namespace GVDEditor.Integration;

/// <summary>
/// INISS skoncil.
/// </summary>
/// <param name="instance">ukonceny INISS</param>
/// <param name="exitCode">navratovy kod; <see langword="null" />, ak sa neda zistit</param>
internal sealed class InissExitedEventArgs(InissInstance instance, int? exitCode) : EventArgs
{
    /// <summary>Ukonceny INISS.</summary>
    public InissInstance Instance { get; } = instance;

    /// <summary>Navratovy kod; <see langword="null" />, ak sa neda zistit.</summary>
    public int? ExitCode { get; } = exitCode;
}
