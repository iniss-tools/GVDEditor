using System.Globalization;
using GVDEditor.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Spravovanie sekcii TabTab.txt: kontrola nazvu sekcie a hladanie jej pouzitia v katalogovych tabuliach.
/// </summary>
internal static class TabTabSections
{
    /// <summary>
    ///     Skontroluje nazov sekcie (pri pridani alebo premenovani).
    /// </summary>
    /// <param name="name">Zadany nazov; okolite medzery sa odrezu.</param>
    /// <param name="otherNames">Nazvy ostatnych sekcii (bez premenovavanej).</param>
    /// <param name="normalized">Nazov bez okolitych medzier.</param>
    /// <returns>Text chyby, alebo <see langword="null"/>, ak je nazov platny.</returns>
    public static string? ValidateName(string? name, IEnumerable<string> otherNames, out string normalized)
    {
        var trimmed = normalized = (name ?? "").Trim();

        // TxtParser.ReadTables prazdny nazov odmietne a grafikon sa nenacita
        if (normalized.Length == 0)
            return Resources.TabTab_Nazov_prazdny;

        // hlavicka sekcie [NAZOV] konci prvou ']'; koniec riadka by ju rozdelil
        if (normalized.Any(c => c == ']' || char.IsControl(c)))
            return Resources.TabTab_Nazov_neplatny_znak;

        // subor je v CP1250 - iny znak by sa zapisal ako '?' a odkaz z TKatalog.txt by nesedel
        if (Encodings.Win1250.GetString(Encodings.Win1250.GetBytes(normalized)) != normalized)
            return Resources.TabTab_Nazov_kodovanie;

        // polozka Ziadny v katalogovej tabuli znamena "bez TabTab" a do suboru sa nezapisuje
        if (string.Equals(normalized, TableTabTab.Empty.Key, StringComparison.OrdinalIgnoreCase))
            return string.Format(CultureInfo.CurrentCulture, Resources.TabTab_Nazov_rezervovany, TableTabTab.Empty.Key);

        // dve sekcie s rovnakym menom by sa pri zapise (TxtPropsAreas.Set) zlucili do jednej
        var existing = otherNames.FirstOrDefault(other => string.Equals(other, trimmed, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
            return string.Format(CultureInfo.CurrentCulture, Resources.TabTab_Nazov_existuje, existing);

        return null;
    }

    /// <summary>
    ///     Najde stlpce katalogovych tabul, ktore sekciu pouzivaju ako TAB1 alebo TAB2.
    /// </summary>
    /// <returns>Popis kazdeho pouzitia (prazdny zoznam, ak sa sekcia nepouziva).</returns>
    public static List<string> FindUsage(TableTabTab tab, IEnumerable<TableCatalog> catalogs)
    {
        var usage = new List<string>();
        if (tab == TableTabTab.Empty)
            return usage;

        foreach (var catalog in catalogs)
        foreach (var item in catalog.Items)
        {
            if (item.Tab1 == tab)
                usage.Add(string.Format(CultureInfo.CurrentCulture, Resources.TabTab_Pouzitie, catalog.Name, item.Name, "TAB1"));
            if (item.Tab2 == tab)
                usage.Add(string.Format(CultureInfo.CurrentCulture, Resources.TabTab_Pouzitie, catalog.Name, item.Name, "TAB2"));
        }

        return usage;
    }

    /// <summary>
    ///     Ak sa sekcia pouziva, zobrazi chybu so zoznamom pouziti a vrati <see langword="false"/>.
    /// </summary>
    public static bool CheckCanRemove(TableTabTab tab, IEnumerable<TableCatalog> catalogs)
    {
        var usage = FindUsage(tab, catalogs);
        if (usage.Count == 0)
            return true;

        Utils.ShowError(Resources.SelectedItemRemoveCancel + Environment.NewLine + string.Join(Environment.NewLine, usage.Select(u => "– " + u)));
        return false;
    }
}
