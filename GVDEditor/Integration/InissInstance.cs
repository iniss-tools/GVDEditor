using GVDEditor.Config;

namespace GVDEditor.Integration;

/// <summary>
/// INISS spusteny z GVDEditora.
/// </summary>
internal sealed class InissInstance
{
    internal InissInstance(InissLaunch launch, Process process)
    {
        Launch = launch;
        Process = process;
        Started = DateTime.Now;
    }

    /// <summary>Ako bol spusteny.</summary>
    public InissLaunch Launch { get; }

    /// <summary>Konfiguracia v case spustenia.</summary>
    public RunConfiguration Configuration => Launch.Configuration;

    /// <summary>Cas spustenia.</summary>
    public DateTime Started { get; }

    /// <summary>Prave sa restartuje (caka sa na ukoncenie).</summary>
    public bool IsRestarting { get; internal set; }

    internal Process Process { get; }
}
