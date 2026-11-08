namespace GVDEditor.Mcp.Results;

/// <summary>Pismo tabul (get_table).</summary>
internal sealed record FontDetail(
    [property: Description("Font number; columns, texts and TabTab {n} refer to it.")] int Id,
    string Name,
    [property: Description("regular, bold, italic, boldItalic or special; null = not set.")] string? Type,
    [property: Description("Height in points.")] int Size,
    [property: Description("Character width in points.")] int Width,
    bool Proportional,
    bool Diacritics,
    bool Lowercase,
    bool Uppercase,
    bool Digits,
    bool SpecialChars,
    bool SpecialAssignment,
    [property: Description("Font file; empty = none.")] string File,
    [property: Description("Meaning of the number on ELEN boards: colour, face, blinking.")] string Elen);
