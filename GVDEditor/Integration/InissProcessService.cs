using System.Globalization;
using GVDEditor.Properties;
using Serilog;

namespace GVDEditor.Integration;

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
            Log.Error(e, "INISS {Config} sa nepodarilo spustit: {Path} {Arguments}", launch.Configuration.Name, launch.Path, launch.Arguments);
            // proces nebezi - nesmie ostat ako "beziaci"
            process.Exited -= OnExited;
            process.Dispose();
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.FMain_Nepodarilo_sa_spustiť_vybraný_program, e.Message), e);
        }

        Log.Information("Spusteny INISS {Config}: {Path} {Arguments} (PID {Pid}, spravca {RunAsAdmin})", launch.Configuration.Name, launch.Path,
            launch.Arguments, process.Id, launch.Configuration.RunAsAdmin);
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
            Log.Information("INISS {Config} nasilne ukonceny", instance.Launch.Configuration.Name);
        }
        catch (Exception e) when (e is Win32Exception or NotSupportedException or InvalidOperationException)
        {
            if (HasExited(instance.Process))
                return;
            Log.Warning(e, "INISS {Config} sa nepodarilo ukoncit", instance.Launch.Configuration.Name);
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
            {
                Log.Information("INISS {Config} sa ukoncuje riadne", instance.Launch.Configuration.Name);
                instance.Process.CloseMainWindow();
            }
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
                    Log.Warning("INISS {Config} sa neukoncil do {Timeout}", instance.Launch.Configuration.Name, RestartTimeout);
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
        if (code == 0)
            Log.Information("INISS {Config} skoncil", instance.Launch.Configuration.Name);
        else
            Log.Warning("INISS {Config} skoncil s kodom {ExitCode}", instance.Launch.Configuration.Name, code);
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
