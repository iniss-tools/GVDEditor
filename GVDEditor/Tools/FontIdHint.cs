using System.Globalization;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Tools;

/// <summary>
///     Napoveda pri poli s cislom pisma tabule: podfarbenie podla farby ELEN a popis vyznamu cisla.
/// </summary>
internal static class FontIdHint
{
    /// <summary>
    ///     Podfarbi pole a nastavi mu popis vyznamu cisla pisma pre daneho vyrobcu.
    /// </summary>
    /// <param name="nud">Pole s cislom pisma.</param>
    /// <param name="tip">Popisok, do ktoreho sa zapise vyznam.</param>
    /// <param name="defaultBorderColor">Povodna farba okraja pola.</param>
    /// <param name="manufacturer">Vyrobca tabule; <see langword="null" /> = nie je znamy, predpoklada sa ELEN.</param>
    public static void Apply(ExNumericUpDown nud, ToolTip tip, Color defaultBorderColor, TableManufacturer? manufacturer = null)
    {
        var code = new ElenFontCode(decimal.ToInt32(nud.Value));
        var isElen = ElenFontCode.AppliesTo(manufacturer);

        ApplyColor(nud, isElen ? code.Color : 0, defaultBorderColor);

        var text = isElen
            ? string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Popis, code.Describe())
            : string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_InyVyrobca, manufacturer!.Name);
        tip.SetToolTip(nud, text);
        // mys nad textovym polom alebo sipkami je nad vnorenym prvkom, nie nad samotnym polom
        foreach (Control child in nud.Controls)
            tip.SetToolTip(child, text);
    }

    /// <summary>
    ///     Podfarbi pole farbou pisma: 1 cervena, 2 zelena, 3 zlta, inak neutralne.
    /// </summary>
    public static void ApplyColor(ExNumericUpDown nud, int color, Color defaultBorderColor)
    {
        switch (color)
        {
            case 1:
                nud.BorderColor = defaultBorderColor;
                nud.ArrowsColor = Color.White;
                nud.BackColor = Color.Red;
                nud.ForeColor = Color.White;
                break;
            case 2:
                nud.BorderColor = defaultBorderColor;
                nud.ArrowsColor = Color.White;
                nud.BackColor = Color.Green;
                nud.ForeColor = Color.White;
                break;
            case 3:
                nud.BorderColor = defaultBorderColor;
                nud.ArrowsColor = Color.Black;
                nud.BackColor = Color.Yellow;
                nud.ForeColor = Color.Black;
                break;
            default:
                nud.BorderColor = Color.DimGray;
                nud.ArrowsColor = Color.Black;
                nud.BackColor = Color.White;
                nud.ForeColor = Color.Black;
                break;
        }
    }
}
