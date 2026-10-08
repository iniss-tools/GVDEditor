using GVDEditor.Config;
using GVDEditor.Domain.Analysis;
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
    public static ResolvedConfig Resolve(RunConfiguration config, string installationDir, IReadOnlyDictionary<int, RegTableInfo>? tables = null)
    {
        var exe = PathUtils.CombinePath(installationDir, config.Program)!;
        var mode = config.RunAsAdmin ? InissRunMode.Elevated : InissRunMode.Normal;
        return RegResolver.Resolve(InissRegistry.LoadSource(RunConfigurations.AppName(config), exe, mode, tables));
    }

    /// <summary>
    /// Nastavenia INISSu vybranej konfiguracie spustania pre analyzu grafikonu (tabule instalacie v poradi INISSu);
    /// null, ak instalacia nema konfiguraciu s programom.
    /// </summary>
    public static InissRegistryView? AnalysisView(RunConfiguration? config, InissWorkspace workspace)
    {
        if (config is null || config.Program.Length == 0) return null;
        var tables = InissTableMap.Build(workspace.GVDDirs);
        var resolved = Resolve(config, workspace.INISSDir, tables.ToDictionary(t => t.Index, t => t.ToInfo()));
        var src = resolved.Source;
        var exists = src.Machine.Exists || src.User.Exists || src.VirtualStore.Exists || src.Ini is not null;
        return new InissRegistryView(config.Name, resolved, tables, exists);
    }

    /// <summary>
    /// Priecinok logov INISSu tejto konfiguracie - <c>PathNames\LogPath</c> z nastaveni, ktore INISS nacita
    /// (relativne k priecinku programu), a odkial hodnota pochadza.
    /// </summary>
    public static (string Path, ResolvedSetting? Setting) LogDirectory(ResolvedConfig resolved, string installationDir)
    {
        return (InissPaths.LogDirectory(resolved, installationDir), resolved.Find("PathNames", "LogPath"));
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
