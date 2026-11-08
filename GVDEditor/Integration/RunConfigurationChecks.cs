using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Properties;

namespace GVDEditor.Integration;

/// <summary>
/// Kontrola konfiguracie spustania podla spravania INISSu 3.39 (parametre prikazoveho riadka, DirList, register).
/// </summary>
internal static class RunConfigurationChecks
{
    /// <summary>
    /// Zistenia pre konfiguraciu <paramref name="config" /> - najzavaznejsie prve.
    /// </summary>
    public static List<RunCheck> Check(RunConfiguration config, RunEnvironment env)
    {
        var checks = new List<RunCheck>();
        void Add(RunCheckSeverity severity, string format, params object[] args) =>
            checks.Add(new RunCheck(severity, string.Format(CultureInfo.CurrentCulture, format, args)));

        if (string.IsNullOrWhiteSpace(config.Program))
            Add(RunCheckSeverity.Error, Resources.Run_Check_NoProgram);
        else if (!env.ProgramExists(config.Program))
            Add(RunCheckSeverity.Error, Resources.Run_Check_ProgramMissing, config.Program);
        else if (!env.Programs.Contains(config.Program, StringComparer.OrdinalIgnoreCase))
            Add(RunCheckSeverity.Warning, Resources.Run_Check_NotIniss, config.Program);

        if (env.SingleInstanceHeld && !config.Multiuse)
            Add(RunCheckSeverity.Warning, Resources.Run_Check_SingleInstance);
        if (config.WhenRunning == WhenAlreadyRunning.NewInstance && !config.Multiuse)
            Add(RunCheckSeverity.Warning, Resources.Run_Check_NewInstanceNoMultiuse);
        if (env.ImportMarker)
            Add(RunCheckSeverity.Warning, Resources.Run_Check_ImportMarker);

        var app = config.Program.Length > 0 ? RunConfigurations.AppName(config) : config.Registry.Trim();
        if (app.Length > 0 && env.RegistryBranches is { } branches && !branches.Contains(app, StringComparer.OrdinalIgnoreCase))
            Add(RunCheckSeverity.Warning, Resources.Run_Check_NoBranch, app);

        foreach (var n in config.ActivatorList.Where(n => !env.ActivatorTargets.ContainsKey(n)))
            Add(RunCheckSeverity.Warning, Resources.Run_Check_UnusedActivator, n);

        if (!config.RunAsAdmin && config.Program.Length > 0 && env.VirtualStoreShadows(config) is { Count: > 0 } shadows)
            Add(RunCheckSeverity.Info, Resources.Run_Check_VirtualStore, shadows.Count,
                string.Join(", ", shadows.Take(3)) + (shadows.Count > 3 ? ", …" : ""));
        if (config.ImportDat)
            Add(RunCheckSeverity.Info, Resources.Run_Check_ImportDat);
        if (config.ExportHlas)
            Add(RunCheckSeverity.Info, Resources.Run_Check_ExportHlas);
        foreach (var (n, names) in env.ActivatorTargets.Where(t => !config.ActivatorList.Contains(t.Key)))
            Add(RunCheckSeverity.Info, Resources.Run_Check_Sleeping, n, string.Join(", ", names));

        if (app.Length > 0)
        {
            var same = env.Others.Where(o => o.Id != config.Id && o.Program.Length > 0
                                             && string.Equals(RunConfigurations.AppName(o), app, StringComparison.OrdinalIgnoreCase)).Select(o => o.Name).ToList();
            if (same.Count > 0)
                Add(RunCheckSeverity.Info, Resources.Run_Check_SameBranch, app, string.Join(", ", same));
        }

        return checks.OrderByDescending(c => c.Severity).ToList();
    }
}
