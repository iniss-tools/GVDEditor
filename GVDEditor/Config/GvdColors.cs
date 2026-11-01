using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using ToolsCore.XML;

namespace GVDEditor.Config;

/// <summary>
/// Polozky farebnej schemy GVDEditora: druhy vlakov v tabulke vlakov a editor TabTab.
/// </summary>
public static class GvdColors
{
    /// <summary>
    /// Kategoria Stlpec typ vlaku.
    /// </summary>
    public static ColorCategory TrainTypes { get; } = new("TrainType", () => Resources.Scheme_TrainTypeColumn, () => new TrainTypeColorPreview());

    /// <summary>
    /// Kategoria Editor TabTab.
    /// </summary>
    public static ColorCategory TabTab { get; } = new("TabTab", () => Resources.Scheme_TabTabEditor, () => new TabTabColorPreview());

    private const ColorParts TypeParts = ColorParts.Fore | ColorParts.Back | ColorParts.Bold;
    private const ColorParts TextParts = ColorParts.Fore | ColorParts.Bold;

    private static ColorKey TrainType(string id, Color light, Color dark, bool bold, Func<TrainTypeColumnScheme, ColorSetting> setting) =>
        new("TrainType." + id, TrainTypes, () => id, new ColorValue(light, Color.Transparent, bold), new ColorValue(dark, Color.Transparent, bold),
            TypeParts, s => setting(((GVDEditorStyle)s).TrainTypeColumnScheme), "TrainType/" + id, UiColors.Box.Id);

    private static ColorKey Text(string id, Func<string> name, Color light, Color dark, Func<TabTabEditorScheme, ColorSetting> setting) =>
        new("TabTab." + id, TabTab, name, new ColorValue(light, Color.Empty), new ColorValue(dark, Color.Empty),
            TextParts, s => setting(((GVDEditorStyle)s).TabTabEditorScheme), "TabTabEditor/" + id, UiColors.Box.Id);

    private static ColorKey Brace(string id, Func<string> name, ColorValue value, Func<TabTabEditorScheme, ColorSetting> setting) =>
        new("TabTab." + id, TabTab, name, value, value, TypeParts, s => setting(((GVDEditorStyle)s).TabTabEditorScheme), "TabTabEditor/" + id);

    /// <summary>
    /// Polozky v poradi stromu na stranke schemy.
    /// </summary>
    public static IReadOnlyList<ColorKey> Keys { get; } =
    [
        TrainType("Os", Color.Black, Color.White, false, t => t.Os),
        TrainType("R", Color.Red, Color.Red, true, t => t.R),
        TrainType("X", Color.Green, Color.Green, true, t => t.X),
        TrainType("Sl", Color.OrangeRed, Color.OrangeRed, false, t => t.Sl),
        Text("Default", () => Resources.Scheme_Default, Color.Black, Color.White, t => t.Default),
        Text("Number", () => Resources.Scheme_Number, Color.Purple, Color.PaleGoldenrod, t => t.Number),
        Text("String", () => Resources.Scheme_String, Color.Red, Color.Coral, t => t.String),
        Text("Comment", () => Resources.Scheme_Comment, Color.Green, Color.Green, t => t.Comment),
        Text("OnNewLine", () => Resources.Scheme_OnNewLine, Color.OrangeRed, Color.OrangeRed, t => t.OnNewLine),
        Text("Operator", () => Resources.Scheme_Operator, Color.Black, Color.White, t => t.Operator),
        Text("Constant", () => Resources.Scheme_Constant, Color.DimGray, Color.LightSlateGray, t => t.Constant),
        Text("Var", () => Resources.Scheme_Var, Color.DarkSlateGray, Color.Aquamarine, t => t.Var),
        Text("Event", () => Resources.Scheme_Event, Color.SaddleBrown, Color.Goldenrod, t => t.Event),
        Text("Function", () => Resources.Scheme_Function, Color.Blue, Color.DodgerBlue, t => t.Function),
        Text("Identifier", () => Resources.Scheme_Identifier, Color.Teal, Color.LightGreen, t => t.Identifier),
        Brace("SelBraces", () => Resources.Scheme_SelBraces, new ColorValue(Color.LightGray, Color.BlueViolet), t => t.SelBraces),
        Brace("SelBraceBad", () => Resources.Scheme_SelBraceBad, new ColorValue(Color.Red, Color.LightGray), t => t.SelBraceBad)
    ];
}
