using GVDEditor.Config;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Co sa spusta: konfiguracia (snimka v case spustenia), cesta k programu a argumenty.
/// </summary>
/// <param name="Configuration">konfiguracia spustania</param>
/// <param name="Path">cela cesta k programu</param>
/// <param name="Arguments">argumenty prikazoveho riadka</param>
internal sealed record InissLaunch(RunConfiguration Configuration, string Path, string Arguments)
{
    /// <summary>
    /// Spustenie konfiguracie <paramref name="configuration" /> z instalacie <paramref name="installationDir" />.
    /// </summary>
    public static InissLaunch Create(RunConfiguration configuration, string installationDir) =>
        new(configuration with { }, PathUtils.CombinePath(installationDir, configuration.Program)!, RunConfigurations.Arguments(configuration));
}
