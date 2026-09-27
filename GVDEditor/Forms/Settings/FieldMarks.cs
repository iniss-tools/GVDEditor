using ExControls;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Oznacenie chybnych poli farbou okraja. Povodne farby (podla temy) sa zapamataju pri
///     <see cref="Capture" />, preto ju volat az po nastaveni temy okna.
/// </summary>
internal sealed class FieldMarks
{
    private readonly Dictionary<Control, Color> _normal = [];

    /// <summary>
    ///     Zapamata povodnu farbu okraja poli.
    /// </summary>
    public void Capture(params Control[] fields)
    {
        foreach (var field in fields)
            if (Border(field) is { } color)
                _normal[field] = color;
    }

    /// <summary>
    ///     Zafarbi okraj poli <paramref name="bad" />, ostatnym vrati povodnu farbu.
    /// </summary>
    public void Mark(IEnumerable<Control> bad)
    {
        var set = bad.ToHashSet();
        foreach (var (field, normal) in _normal)
            SetBorder(field, set.Contains(field) ? SettingsWindow.ProblemColor(field.Parent ?? field) : normal);
    }

    private static Color? Border(Control field) => field switch
    {
        ExTextBox tb => tb.BorderColor,
        ExNumericUpDown nud => nud.BorderColor,
        ExMaskedTextBox mtb => mtb.BorderColor,
        _ => null
    };

    private static void SetBorder(Control field, Color color)
    {
        switch (field)
        {
            case ExTextBox tb:
                tb.BorderColor = color;
                break;
            case ExNumericUpDown nud:
                nud.BorderColor = color;
                break;
            case ExMaskedTextBox mtb:
                mtb.BorderColor = color;
                break;
        }
    }
}
