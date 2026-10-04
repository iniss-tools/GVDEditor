using GVDEditor.Config;
using GVDEditor.Domain.Documents;
using GVDEditor.Formats;
using ToolsCore.Iniss.Registry;
using ToolsCore.Iniss.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Okolie spustenia INISSu na tomto pocitaci - programy a data instalacie, vetvy registra, zamok jedinej instancie.
/// </summary>
internal static class InissEnvironment
{
    /// <summary>
    /// Mutex, ktorym INISS bez /Multiuse odmietne druhu instanciu (skonci s kodom -4).
    /// </summary>
    private const string SingleInstanceMutex = "INISS Operator";

    /// <summary>
    /// Ci bezi INISS bez /Multiuse - aj spusteny mimo GVDEditora.
    /// </summary>
    public static bool SingleInstanceHeld()
    {
        try
        {
            if (!Mutex.TryOpenExisting(SingleInstanceMutex, out var mutex))
                return false;
            mutex.Dispose();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            // mutex existuje, ale patri INISSu spustenemu ako spravca
            return true;
        }
        catch (Exception e) when (e is IOException or WaitHandleCannotBeOpenedException)
        {
            return false;
        }
    }

    /// <summary>
    /// Nastavenia INISSu tejto konfiguracie tak, ako ich INISS pri starte nacita (vetva, rezim spravcu).
    /// </summary>
    public static ResolvedConfig Resolve(RunConfiguration config, string installationDir)
    {
        var exe = PathUtils.CombinePath(installationDir, config.Program)!;
        var mode = config.RunAsAdmin ? InissRunMode.Elevated : InissRunMode.Normal;
        return RegResolver.Resolve(InissRegistry.LoadSource(RunConfigurations.AppName(config), exe, mode));
    }

    /// <summary>
    /// Priecinok logov INISSu tejto konfiguracie - <c>PathNames\LogPath</c> z nastaveni, ktore INISS nacita
    /// (relativne k priecinku programu), a odkial hodnota pochadza.
    /// </summary>
    public static (string Path, ResolvedSetting? Setting) LogDirectory(ResolvedConfig resolved, string installationDir)
    {
        var setting = resolved.Find("PathNames", "LogPath");
        var value = setting?.Value as string;
        return (Path.GetFullPath(Path.Combine(installationDir, string.IsNullOrWhiteSpace(value) ? "DATA" : value.Trim())), setting);
    }

    /// <summary>
    /// Hodnoty, ktore INISS bez prav spravcu cita z kopie vo VirtualStore a ktore sa lisia od HKLM (okrem tych, ktore
    /// si INISS prepisuje sam, napr. rozlozenie okna) - ako <c>Sekcia\Nazov</c>.
    /// </summary>
    public static IReadOnlyList<string> VirtualStoreShadows(ResolvedConfig resolved) =>
        resolved.Diagnostics.Where(d => d.Code == RegDiagnosticCode.ShadowedByVirtualStore && d.Severity == RegSeverity.Warning)
            .Select(d => d.Name is null ? d.Section : d.Section + "\\" + d.Name).Distinct().ToList();

    /// <summary>
    /// Okolie pre kontrolu konfiguracie instalacie <paramref name="workspace" />.
    /// </summary>
    /// <param name="workspace">otvorena instalacia</param>
    /// <param name="all">vsetky konfiguracie instalacie</param>
    /// <param name="atLaunch">pri spusteni - zisti aj beziaci INISS bez /Multiuse</param>
    public static RunEnvironment Create(InissWorkspace workspace, IReadOnlyList<RunConfiguration> all, bool atLaunch) => new()
    {
        Programs = workspace.INISSExeFiles,
        ProgramExists = program => File.Exists(PathUtils.CombinePath(workspace.INISSDir, program)),
        ImportMarker = File.Exists(PathUtils.CombinePath(workspace.DataDir, GvdFileConsts.FileAktAkt)),
        RegistryBranches = InissRegistry.AppNames(),
        ActivatorTargets = RunConfigurations.ActivatorTargets(workspace.GVDDirs),
        VirtualStoreShadows = config => config.RunAsAdmin || config.Program.Length == 0 ? [] : VirtualStoreShadows(Resolve(config, workspace.INISSDir)),
        Others = all,
        SingleInstanceHeld = atLaunch && SingleInstanceHeld()
    };
}
