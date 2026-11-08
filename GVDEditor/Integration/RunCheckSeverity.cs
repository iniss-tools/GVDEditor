namespace GVDEditor.Integration;

/// <summary>
/// Zavaznost zistenia pri konfiguracii spustania.
/// </summary>
internal enum RunCheckSeverity
{
    /// <summary>Len na vedomie (okno konfiguracii), pri spusteni sa nepyta.</summary>
    Info,

    /// <summary>Pri spusteni sa opyta, ci aj tak spustit.</summary>
    Warning,

    /// <summary>INISS sa spustit neda.</summary>
    Error
}
