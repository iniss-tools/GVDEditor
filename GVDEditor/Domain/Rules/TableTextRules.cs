using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Kontrola textu na tabuliach (typ textu s realizaciami a textami vlakov).
/// </summary>
internal static class TableTextRules
{
    /// <summary>
    /// Pole textu, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Name,
        Key,
        Realization
    }

    /// <summary>
    /// Vsetky chyby textu na pozicii <paramref name="index" />; pri realizacii aj jej poradie v zozname.
    /// </summary>
    public static List<(Field Field, int Row, string Message)> Check(IReadOnlyList<TableText> texts, int index,
        IReadOnlyCollection<TableCatalog> catalogs)
    {
        var text = texts[index];
        var problems = new List<(Field, int, string)>();

        if (TableRules.CheckName(text.Name) is { } name)
            problems.Add((Field.Name, -1, name));
        if (TableRules.CheckKey(texts.Select(t => t.Key).ToList(), index) is { } key)
            problems.Add((Field.Key, -1, key));

        for (var i = 0; i < text.Realizations.Count; i++)
            if (CheckRealization(text.Realizations[i], catalogs) is { } realization)
                problems.Add((Field.Realization, i, string.Format(CultureInfo.CurrentCulture, Resources.TableTextRules_Realizacia,
                    i + 1, realization)));

        return problems;
    }

    /// <summary>
    /// Realizacia musi ukazovat na existujucu katalogovu tabulu a jej stlpec - inak by sa grafikon neotvoril.
    /// </summary>
    public static string? CheckRealization(TableTextRealization realization, IReadOnlyCollection<TableCatalog> catalogs)
    {
        if (realization.Table is not { } table || realization.Item is not { } item)
            return Resources.TableTextRules_Nevybrana;
        if (!catalogs.Contains(table))
            return string.Format(CultureInfo.CurrentCulture, Resources.TableTextRules_Bez_tabule, table.Name);
        if (!table.Items.Contains(item))
            return string.Format(CultureInfo.CurrentCulture, Resources.TableTextRules_Bez_stlpca, item.Name, table.Name);
        return null;
    }
}
