using GVDEditor.Config;

namespace GVDEditor.Integration;

/// <summary>
/// Co o instalacii a pocitaci treba vediet na kontrolu konfiguracie (testy ho stavaju priamo).
/// </summary>
internal sealed record RunEnvironment
{
    /// <summary>Programy INISS instalacie.</summary>
    public IReadOnlyList<string> Programs { get; init; } = [];

    /// <summary>Ci subor programu v instalacii existuje.</summary>
    public Func<string, bool> ProgramExists { get; init; } = _ => true;

    /// <summary>V datach je znacka davkoveho importu (INISS sa spusti ako s /Import /ImportDat).</summary>
    public bool ImportMarker { get; init; }

    /// <summary>Existujuce vetvy registra pod CHAPS (null = nezistuje sa).</summary>
    public IReadOnlyCollection<string>? RegistryBranches { get; init; }

    /// <summary>Grafikony podla aktivatora /N.</summary>
    public IReadOnlyDictionary<int, List<string>> ActivatorTargets { get; init; } = new Dictionary<int, List<string>>();

    /// <summary>Ostatne konfiguracie instalacie.</summary>
    public IReadOnlyList<RunConfiguration> Others { get; init; } = [];

    /// <summary>Hodnoty, ktore INISS konfiguracie bez prav spravcu cita z kopie vo VirtualStore odlisnej od HKLM.</summary>
    public Func<RunConfiguration, IReadOnlyList<string>> VirtualStoreShadows { get; init; } = _ => [];

    /// <summary>Bezi INISS bez /Multiuse (drzi zamok jedinej instancie) - zistuje sa len pri spusteni.</summary>
    public bool SingleInstanceHeld { get; init; }
}
