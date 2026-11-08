namespace GVDEditor.Integration;

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
