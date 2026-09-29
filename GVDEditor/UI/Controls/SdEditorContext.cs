using GVDEditor.Properties;
using ToolsCore.Expressions;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Spolocny kontext editorov stavoveho diagramu - symboly pre kontrolu vyrazov a typy hlaseni z Categori.txt.
/// </summary>
internal static class SdEditorContext
{
    /// <summary>Symboly grafikonu pre validator vyrazov (druhy vlakov, stanice…).</summary>
    public static IExprSymbolProvider? Symbols { get; set; }

    /// <summary>Kluce typov hlaseni z lokalneho Categori.txt.</summary>
    public static IReadOnlyList<string> ReportKeys { get; set; } = [];

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
    public static (ExprSeverity Severity, string Message)? Check(string text, ExprContext context, bool isCondition)
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
        return abs % 60 == 0 ? string.Format(Resources.FStateDgm_Min, abs / 60) : string.Format(Resources.FStateDgm_Sek, abs);
    }
}
