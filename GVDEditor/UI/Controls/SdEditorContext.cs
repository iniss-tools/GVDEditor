using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Expressions;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Spolocny kontext editorov stavoveho diagramu - symboly pre kontrolu vyrazov, typy hlaseni z Categori.txt
/// a pismo vyrazov. Vytvara ho okno stavoveho diagramu a dostava ho kazdy editor.
/// </summary>
/// <param name="symbols">symboly grafikonu pre validator vyrazov (druhy vlakov, stanice…)</param>
/// <param name="reportTypes">typy hlaseni z lokalneho Categori.txt</param>
/// <param name="exprFont">pismo poli s vyrazom (ako v editore TabTab)</param>
internal sealed class SdEditorContext(IExprSymbolProvider? symbols, IReadOnlyList<ReportType> reportTypes, Font exprFont)
{
    /// <summary>Symboly grafikonu pre validator vyrazov (druhy vlakov, stanice…).</summary>
    public IExprSymbolProvider? Symbols { get; } = symbols;

    /// <summary>Typy hlaseni z lokalneho Categori.txt.</summary>
    public IReadOnlyList<ReportType> ReportTypes { get; } = reportTypes;

    /// <summary>Kluce typov hlaseni z lokalneho Categori.txt.</summary>
    public IReadOnlyList<string> ReportKeys { get; } = reportTypes.Select(r => r.Key).ToList();

    /// <summary>
    /// Kluc hlasenia, ktory INISS v Categori.txt nenajde (porovnava presne s KEY). Bez typov hlaseni sa nekontroluje.
    /// </summary>
    public bool IsUnknownReport(string key) => ReportKeys.Count > 0 && key.Length > 0 && !ReportKeys.Contains(key, StringComparer.Ordinal);

    /// <summary>
    /// Typ hlasenia, ktory zrejme patri k neznamemu klucu: s rovnakym nazvom (NAME - napr. v Categori.txt so zamenenym
    /// KEY a NAME), inak s klucom, ktory sa lisi len velkostou pismen. Null, ak sa ziadny nehodi.
    /// </summary>
    public ReportType? SuggestReport(string key) =>
        ReportTypes.FirstOrDefault(r => string.Equals(r.Name.Trim(), key, StringComparison.OrdinalIgnoreCase))
        ?? ReportTypes.FirstOrDefault(r => string.Equals(r.Key, key, StringComparison.OrdinalIgnoreCase));

    /// <summary>Pismo poli s vyrazom.</summary>
    public Font ExprFont { get; } = exprFont;

    /// <summary>Polozka comboboxu s hodnotou.</summary>
    public sealed record Item(string Text, object? Value)
    {
        /// <inheritdoc />
        public override string ToString() => Text;
    }

    /// <summary>Vyberie polozku podla hodnoty (alebo prvu, ak sa nenajde).</summary>
    public static void Select(ComboBox cb, object? value)
    {
        for (var i = 0; i < cb.Items.Count; i++)
            if (cb.Items[i] is Item it && Equals(it.Value, value))
            {
                cb.SelectedIndex = i;
                return;
            }

        cb.SelectedIndex = cb.Items.Count > 0 ? 0 : -1;
    }

    /// <summary>Hodnota vybranej polozky.</summary>
    public static object? Value(ComboBox cb) => (cb.SelectedItem as Item)?.Value;

    /// <summary>Skontroluje vyraz; vrati text chyby/varovania alebo null.</summary>
    public (ExprSeverity Severity, string Message)? Check(string text, ExprContext context, bool isCondition)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var r = ExprValidator.Validate(text, new ExprValidationOptions { Context = context, Symbols = Symbols, IsCondition = isCondition, ReportContextDependent = false });
        var d = r.Diagnostics.OrderBy(x => x.Severity == ExprSeverity.Error ? 0 : x.Severity == ExprSeverity.Warning ? 1 : 2).FirstOrDefault();
        if (d == null) return null;
        var msg = d.Message + (d.Suggestion != null ? " – " + d.Suggestion : "");
        return (d.Severity, msg);
    }

    /// <summary>Citatelny posun v sekundach (<c>-20 min</c>, <c>+90 s</c>).</summary>
    public static string Seconds(int s)
    {
        var abs = Math.Abs(s);
        return abs % 60 == 0 ? string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_Min, abs / 60) : string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_Sek, abs);
    }
}
