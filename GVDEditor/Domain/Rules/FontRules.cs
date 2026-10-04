using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Pravidla zoznamu pisiem tabul (ModeTabs.txt [FONT]): povinny nazov a jedinecne cislo - stlpce a texty
/// sa na pismo odkazuju cislom.
/// </summary>
internal static class FontRules
{
    /// <summary>
    /// Chyba nazvu pisma, alebo <see langword="null" />.
    /// </summary>
    public static string? CheckName(string name) =>
        string.IsNullOrWhiteSpace(name) ? Resources.FLocalSettings_Nezadaný_názov_písma : null;

    /// <summary>
    /// Chyba cisla pisma na pozicii <paramref name="index" /> - rovnake cislo ma aj ine pismo.
    /// </summary>
    public static string? CheckId(IReadOnlyList<TableFont> fonts, int index)
    {
        var font = fonts[index];
        for (var i = 0; i < fonts.Count; i++)
            if (i != index && fonts[i].FontID == font.FontID)
                return string.Format(CultureInfo.CurrentCulture, Resources.FontRules_Cislo_existuje, font.FontID, fonts[i].Name);

        return null;
    }

    /// <summary>
    /// Cislo pre nove pismo - prvy volny vzhlad (tenke, tucne, neproporcionalne, len cislice; bez farby, cervene,
    /// zelene, zlte) s bitom 0x40 ako v datach INISSu.
    /// </summary>
    public static int SuggestId(IEnumerable<int> used)
    {
        var taken = used.ToHashSet();
        foreach (var face in new[] { 1, 2, 0, 3 })
            for (var color = 0; color < 4; color++)
            {
                var id = ElenFontCode.Compose(ElenFontCode.DefaultKeptBits, face, color, false, false, 0).Id;
                if (!taken.Contains(id))
                    return id;
            }

        return ElenFontCode.DefaultKeptBits | 0x10;
    }
}
