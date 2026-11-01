using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Farby editora TabTab (polozky <see cref="GvdColors" />).
/// </summary>
public sealed class TabTabEditorScheme
{
    /// <summary>Cislo.</summary>
    public ColorSetting Number { get; } = new();

    /// <summary>Retazec.</summary>
    public ColorSetting String { get; } = new();

    /// <summary>Komentar.</summary>
    public ColorSetting Comment { get; } = new();

    /// <summary>Pokracovanie na dalsom riadku.</summary>
    public ColorSetting OnNewLine { get; } = new();

    /// <summary>Operator.</summary>
    public ColorSetting Operator { get; } = new();

    /// <summary>Konstanta.</summary>
    public ColorSetting Constant { get; } = new();

    /// <summary>Bezny text.</summary>
    public ColorSetting Default { get; } = new();

    /// <summary>Premenna.</summary>
    public ColorSetting Var { get; } = new();

    /// <summary>Udalost.</summary>
    public ColorSetting Event { get; } = new();

    /// <summary>Funkcia.</summary>
    public ColorSetting Function { get; } = new();

    /// <summary>Identifikator.</summary>
    public ColorSetting Identifier { get; } = new();

    /// <summary>Aktivne zatvorky.</summary>
    public ColorSetting SelBraces { get; } = new();

    /// <summary>Zatvorka bez paru.</summary>
    public ColorSetting SelBraceBad { get; } = new();
}
