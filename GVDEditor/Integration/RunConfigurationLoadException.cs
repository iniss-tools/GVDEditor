namespace GVDEditor.Integration;

/// <summary>
/// Subor zdielanych konfiguracii sa nepodarilo precitat; <see cref="Partial" /> obsahuje konfiguracie tohto pocitaca.
/// </summary>
internal sealed class RunConfigurationLoadException(RunConfigurationSet partial, Exception inner) : InvalidOperationException(inner.Message, inner)
{
    /// <summary>Konfiguracie, ktore sa nacitat dali.</summary>
    public RunConfigurationSet Partial { get; } = partial;
}
