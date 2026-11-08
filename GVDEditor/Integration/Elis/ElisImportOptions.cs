using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.Elis;

/// <summary>
/// Volby importu vlakov z ELIS zadane pouzivatelom.
/// </summary>
internal sealed class ElisImportOptions
{
    /// <summary>
    /// Predvolene umiestnenie aplikacie Cestovne poriadky.
    /// </summary>
    public const string DefaultElisDirectory = @"C:\Program Files (x86)\Cestovné poriadky";

    public string AppDirectory { get; init; } = null!;
    public bool OmitPassingTrains { get; init; }
    public bool ReorderTrains { get; init; }

    /// <summary>
    /// Co sa stane s povodnymi vlakmi grafikonu.
    /// </summary>
    public ElisImportMode Mode { get; init; }

    /// <summary>
    /// Ci sa maju naimportovanym vlakom vygenerovat texty na tabuliach.
    /// </summary>
    public bool GenerateTableTexts { get; init; }

    /// <summary>
    /// Grafikon, z ktoreho vlakov sa prevezme kratke hlasenie a hlasenia doplnkov (otvoreny alebo ine obdobie
    /// stanice); <see langword="null" /> = nepreberat.
    /// </summary>
    public GVDDirectory? LearnFrom { get; init; }

    /// <summary>Plan radenia vlakov ZSSK (PDF), z ktoreho sa vlakom navrhne radenie; <see langword="null" /> = bez radenia.</summary>
    public string? PlanPdf { get; init; }
}
