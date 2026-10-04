using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Properties;
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

/// <summary>
/// INISSy spustene z GVDEditora: spustenie, riadne a nutene ukoncenie, restart. Moze ich bezat viac naraz (rozne
/// konfiguracie alebo dalsia instancia s /Multiuse). Zmenu stavu hlasi udalost <see cref="StateChanged" />.
/// </summary>
internal interface IInissProcess : IDisposable
{
    /// <summary>
    /// Beziace INISSy v poradi spustenia.
    /// </summary>
    IReadOnlyList<InissInstance> Instances { get; }

    /// <summary>
    /// Bezi aspon jeden INISS.
    /// </summary>
    bool IsRunning => Instances.Count > 0;

    /// <summary>
    /// Zmena stavu - hlasi sa vo vlakne okna, ktore INISS spustilo.
    /// </summary>
    event EventHandler? StateChanged;

    /// <summary>
    /// INISS skoncil (aj pri restarte) - hlasi sa vo vlakne okna, ktore ho spustilo, pred <see cref="StateChanged" />.
    /// </summary>
    event EventHandler<InissExitedEventArgs>? Exited;

    /// <summary>
    /// Spusti INISS.
    /// </summary>
    InissInstance Start(InissLaunch launch);

    /// <summary>
    /// Nutene ukoncenie.
    /// </summary>
    void Kill(InissInstance instance);

    /// <summary>
    /// Riadne ukoncenie.
    /// </summary>
    void ShutDown(InissInstance instance);

    /// <summary>
    /// Restart; ak sa INISS riadne neukonci, opyta sa <paramref name="confirmKill" />, ci ho ukoncit nasilu. Znova sa
    /// spusti podla <paramref name="launch" /> (konfiguracia sa medzitym mohla zmenit).
    /// </summary>
    Task RestartAsync(InissInstance instance, InissLaunch launch, Func<bool> confirmKill);
}

/// <summary>
/// INISSy cez <see cref="Process" />. Zmenu stavu hlasi vo vlakne, z ktoreho bol INISS spusteny
/// (hlavne okno) - sluzba moze vzniknut skor nez okno (composition root v <see cref="Program" />).
/// </summary>
internal sealed class InissProcessService : IInissProcess
{
    private SynchronizationContext? _context = SynchronizationContext.Current;
    private readonly List<InissInstance> _instances = [];

    /// <summary>
    /// Ako dlho restart caka na riadne ukoncenie INISSu, kym ponukne nutene ukoncenie.
    /// </summary>
    public TimeSpan RestartTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <inheritdoc />
    public IReadOnlyList<InissInstance> Instances => _instances;

    /// <inheritdoc />
    public event EventHandler? StateChanged;

    /// <inheritdoc />
    public event EventHandler<InissExitedEventArgs>? Exited;

    /// <summary>
    /// Spusti INISS podla <paramref name="launch" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">Program sa nepodarilo spustit (napr. zamietnute spustenie ako
    /// administrator); sprava je urcena pouzivatelovi.</exception>
    public InissInstance Start(InissLaunch launch)
    {
        _context = SynchronizationContext.Current ?? _context;
        var process = new Process
        {
            StartInfo =
            {
                FileName = launch.Path, UseShellExecute = true, Arguments = launch.Arguments,
                WorkingDirectory = Path.GetDirectoryName(launch.Path) ?? ""
            }
        };
        if (launch.Configuration.RunAsAdmin) process.StartInfo.Verb = "runas";
        process.EnableRaisingEvents = true;
        process.Exited += OnExited;
        try
        {
            process.Start();
        }
        catch (Exception e) when (e is Win32Exception or InvalidOperationException or FileNotFoundException)
        {
            // proces nebezi - nesmie ostat ako "beziaci"
            process.Exited -= OnExited;
            process.Dispose();
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.FMain_Nepodarilo_sa_spustiť_vybraný_program, e.Message), e);
        }

        var instance = new InissInstance(launch, process);
        _instances.Add(instance);
        OnStateChanged();
        return instance;
    }

    /// <summary>
    /// Nutene ukonci INISS.
    /// </summary>
    /// <exception cref="InvalidOperationException">INISS sa ukoncit neda (napr. bezi ako administrator a GVDEditor bez
    /// opravneni); sprava je urcena pouzivatelovi.</exception>
    public void Kill(InissInstance instance)
    {
        try
        {
            instance.Process.Kill();
        }
        catch (Exception e) when (e is Win32Exception or NotSupportedException or InvalidOperationException)
        {
            if (HasExited(instance.Process))
                return;
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.FMain_INISS_neda_ukoncit, e.Message), e);
        }
    }

    /// <summary>
    /// Zavrie okno INISSu rovnako ako krizik - INISS sa ukonci riadne (a moze sa opytat na potvrdenie).
    /// </summary>
    public void ShutDown(InissInstance instance)
    {
        try
        {
            if (!HasExited(instance.Process))
                instance.Process.CloseMainWindow();
        }
        catch (InvalidOperationException)
        {
        }
    }

    /// <summary>
    /// Riadne ukonci INISS a spusti ho znova podla <paramref name="launch" />. Ak sa INISS do
    /// <see cref="RestartTimeout" /> neukonci (napr. caka na potvrdenie), opyta sa <paramref name="confirmKill" />, ci ho
    /// ukoncit nasilu.
    /// </summary>
    /// <exception cref="InvalidOperationException">INISS sa nepodarilo ukoncit alebo znova spustit.</exception>
    public async Task RestartAsync(InissInstance instance, InissLaunch launch, Func<bool> confirmKill)
    {
        if (instance.IsRestarting || !_instances.Contains(instance))
            return;

        instance.IsRestarting = true;
        OnStateChanged();
        try
        {
            ShutDown(instance);
            using (var timeout = new CancellationTokenSource(RestartTimeout))
            {
                try
                {
                    await instance.Process.WaitForExitAsync(timeout.Token);
                }
                catch (OperationCanceledException)
                {
                    if (!confirmKill())
                        return;

                    Kill(instance);
                    await WaitForExitAsync(instance.Process);
                }
                catch (InvalidOperationException)
                {
                }
            }

            Start(launch);
        }
        finally
        {
            instance.IsRestarting = false;
            OnStateChanged();
        }
    }

    private static bool HasExited(Process process)
    {
        try
        {
            return process.HasExited;
        }
        catch (InvalidOperationException)
        {
            return true;
        }
    }

    private static int? ExitCodeOf(Process process)
    {
        try
        {
            return process.ExitCode;
        }
        catch (Exception e) when (e is InvalidOperationException or Win32Exception or NotSupportedException)
        {
            return null;
        }
    }

    private static async Task WaitForExitAsync(Process process)
    {
        try
        {
            await process.WaitForExitAsync();
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void OnExited(object? sender, EventArgs e) => Post(() =>
    {
        var instance = _instances.Find(i => ReferenceEquals(i.Process, sender));
        if (instance is null)
        {
            (sender as Process)?.Dispose();
            return;
        }

        _instances.Remove(instance);
        var code = ExitCodeOf(instance.Process);
        instance.Process.Exited -= OnExited;
        instance.Process.Dispose();
        Exited?.Invoke(this, new InissExitedEventArgs(instance, code));
        OnStateChanged();
    });

    private void OnStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);

    private void Post(Action action)
    {
        if (_context is null)
        {
            action();
            return;
        }

        try
        {
            _context.Post(_ => action(), null);
        }
        catch (InvalidOperationException)
        {
            // hlavne okno uz neexistuje
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            instance.Process.Exited -= OnExited;
            instance.Process.Dispose();
        }

        _instances.Clear();
    }
}
