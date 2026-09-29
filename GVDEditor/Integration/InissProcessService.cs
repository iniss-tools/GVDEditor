using GVDEditor.Config;
using GVDEditor.Properties;

namespace GVDEditor.Integration;

/// <summary>
/// Proces INISS spusteny z GVDEditora: spustenie, riadne a nutene ukoncenie, restart. Zmenu stavu (spustenie,
/// ukoncenie procesu, zaciatok a koniec restartu) hlasi udalost <see cref="StateChanged" />.
/// </summary>
internal interface IInissProcess : IDisposable
{
    /// <summary>
    /// Cesta k naposledy spustenemu programu (pre restart).
    /// </summary>
    string? LastStartPath { get; }

    /// <summary>
    /// Prebieha restart.
    /// </summary>
    bool IsRestarting { get; }

    /// <summary>
    /// INISS bezi.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Zmena stavu - hlasi sa vo vlakne okna, ktore INISS spustilo.
    /// </summary>
    event EventHandler? StateChanged;

    /// <summary>
    /// Spusti program <paramref name="path" /> s nastaveniami spustania INISSu.
    /// </summary>
    void Start(string path, StartupINISS options);

    /// <summary>
    /// Nutene ukoncenie.
    /// </summary>
    void Kill();

    /// <summary>
    /// Riadne ukoncenie.
    /// </summary>
    void ShutDown();

    /// <summary>
    /// Restart; ak sa INISS riadne neukonci, opyta sa <paramref name="confirmKill" />, ci ho ukoncit nasilu.
    /// </summary>
    Task RestartAsync(StartupINISS options, Func<bool> confirmKill);
}

/// <summary>
/// Proces INISS cez <see cref="Process" />. Zmenu stavu hlasi vo vlakne, z ktoreho bol INISS spusteny
/// (hlavne okno) - sluzba moze vzniknut skor nez okno (composition root v <see cref="Program" />).
/// </summary>
internal sealed class InissProcessService : IInissProcess
{
    private SynchronizationContext? _context = SynchronizationContext.Current;
    private Process? _process;

    /// <summary>
    /// Ako dlho restart caka na riadne ukoncenie INISSu, kym ponukne nutene ukoncenie.
    /// </summary>
    public TimeSpan RestartTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Program spusteny naposledy (restart ho spusti znova).
    /// </summary>
    public string? LastStartPath { get; private set; }

    /// <summary>
    /// Ci prave prebieha restart (caka sa na ukoncenie INISSu).
    /// </summary>
    public bool IsRestarting { get; private set; }

    /// <summary>
    /// Ci bezi INISS spusteny z GVDEditora.
    /// </summary>
    public bool IsRunning
    {
        get
        {
            try
            {
                return _process is { HasExited: false };
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// INISS sa spustil alebo ukoncil, restart zacal alebo skoncil.
    /// </summary>
    public event EventHandler? StateChanged;

    /// <summary>
    /// Spusti program <paramref name="path" /> s nastaveniami spustania INISSu.
    /// </summary>
    /// <exception cref="InvalidOperationException">Program sa nepodarilo spustit (napr. zamietnute spustenie ako
    /// administrator); sprava je urcena pouzivatelovi.</exception>
    public void Start(string path, StartupINISS options)
    {
        _context = SynchronizationContext.Current ?? _context;
        var process = new Process { StartInfo = { FileName = path, UseShellExecute = true, Arguments = options.CmdArgs } };
        if (options.RunAsAdmin) process.StartInfo.Verb = "runas";
        process.EnableRaisingEvents = true;
        process.Exited += OnExited;
        try
        {
            process.Start();
        }
        catch (Exception e) when (e is Win32Exception or InvalidOperationException or FileNotFoundException)
        {
            // proces nebezi - nesmie ostat ako "beziaci"
            process.Dispose();
            throw new InvalidOperationException(string.Format(Resources.FMain_Nepodarilo_sa_spustiť_vybraný_program, e.Message), e);
        }

        _process = process;
        LastStartPath = path;
        OnStateChanged();
    }

    /// <summary>
    /// Nutene ukonci INISS.
    /// </summary>
    /// <exception cref="InvalidOperationException">INISS sa ukoncit neda (napr. bezi ako administrator a GVDEditor bez
    /// opravneni); sprava je urcena pouzivatelovi.</exception>
    public void Kill()
    {
        try
        {
            _process?.Kill();
        }
        catch (Exception e) when (e is Win32Exception or NotSupportedException or InvalidOperationException)
        {
            if (!IsRunning)
                return;
            throw new InvalidOperationException(string.Format(Resources.FMain_INISS_neda_ukoncit, e.Message), e);
        }
    }

    /// <summary>
    /// Zavrie okno INISSu rovnako ako krizik - INISS sa ukonci riadne (a moze sa opytat na potvrdenie).
    /// </summary>
    public void ShutDown()
    {
        try
        {
            if (IsRunning)
                _process!.CloseMainWindow();
        }
        catch (InvalidOperationException)
        {
        }
    }

    /// <summary>
    /// Riadne ukonci INISS a spusti naposledy spusteny program znova. Ak sa INISS do <see cref="RestartTimeout" />
    /// neukonci (napr. caka na potvrdenie), opyta sa <paramref name="confirmKill" />, ci ho ukoncit nasilu.
    /// </summary>
    /// <exception cref="InvalidOperationException">INISS sa nepodarilo ukoncit alebo znova spustit.</exception>
    public async Task RestartAsync(StartupINISS options, Func<bool> confirmKill)
    {
        var process = _process;
        var path = LastStartPath;
        if (IsRestarting || process == null || path == null)
            return;

        IsRestarting = true;
        OnStateChanged();
        try
        {
            ShutDown();
            using (var timeout = new CancellationTokenSource(RestartTimeout))
            {
                try
                {
                    await process.WaitForExitAsync(timeout.Token);
                }
                catch (OperationCanceledException)
                {
                    if (!confirmKill())
                        return;

                    Kill();
                    await WaitForExitAsync(process);
                }
                catch (InvalidOperationException)
                {
                }
            }

            if (!IsRunning)
                Start(path, options);
        }
        finally
        {
            IsRestarting = false;
            OnStateChanged();
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

    private void OnExited(object? sender, EventArgs e)
    {
        // pri restarte uz moze bezat novy proces - ukoncenie stareho ho nesmie prestat sledovat
        if (!ReferenceEquals(sender, _process))
        {
            (sender as Process)?.Dispose();
            return;
        }

        Post(() =>
        {
            if (!ReferenceEquals(sender, _process))
            {
                (sender as Process)?.Dispose();
                return;
            }

            _process?.Dispose();
            _process = null;
            OnStateChanged();
        });
    }

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
        if (_process is not null)
            _process.Exited -= OnExited;
        _process?.Dispose();
        _process = null;
    }
}
