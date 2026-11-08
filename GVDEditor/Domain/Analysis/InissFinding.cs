namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Zistenie kontroly nastaveni INISSu voci grafikonu.
/// </summary>
/// <param name="Key">identita zistenia (na overenie po oprave)</param>
/// <param name="Type">zavaznost</param>
/// <param name="Text">text</param>
/// <param name="Solution">navrh riesenia</param>
/// <param name="Section">sekcia okna Nastavenia INISSu, ktora sa ma otvorit</param>
internal sealed record InissFinding(string Key, ProblemType Type, string Text, string Solution, string? Section);
