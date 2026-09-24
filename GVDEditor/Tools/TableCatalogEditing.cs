using GVDEditor.Entities;

namespace GVDEditor.Tools;

/// <summary>
///     Logika okna katalogovej tabule nezavisla od GUI (FTableCatalog, FTableColumnOrder): pracovne kopie,
///     udrzanie klucov stlpcov v poradi stlpcov a pocet riadkov.
/// </summary>
public static class TableCatalogEditing
{
    /// <summary>
    ///     Vrati novy zoznam TabTab pre vyber v okne – na zaciatku <see cref="TableTabTab.Empty"/>, potom TabTab
    ///     zo zoznamu (bez pripadnej uz ulozenej polozky „Ziadny“). Odovzdany zoznam sa nemeni.
    /// </summary>
    /// <param name="tabTabs">TabTab grafikonu.</param>
    public static List<TableTabTab> WithEmptyTabTab(IEnumerable<TableTabTab> tabTabs)
    {
        var list = new List<TableTabTab> { TableTabTab.Empty };
        list.AddRange(tabTabs.Where(t => t != TableTabTab.Empty));
        return list;
    }

    /// <summary>
    ///     Kopia stlpca (TabTab su zdielane).
    /// </summary>
    public static TableItem Clone(TableItem item)
    {
        var copy = new TableItem();
        CopyTo(item, copy);
        return copy;
    }

    /// <summary>
    ///     Prepise vlastnosti stlpca <paramref name="target"/> hodnotami zo <paramref name="source"/> (instancia cielu
    ///     sa zachova – odkazuju na nu napr. realizacie textov na tabuli).
    /// </summary>
    public static void CopyTo(TableItem source, TableItem target)
    {
        target.Key = source.Key;
        target.Name = source.Name;
        target.FillSection = source.FillSection;
        target.Line = source.Line;
        target.Start = source.Start;
        target.End = source.End;
        target.FontIDX = source.FontIDX;
        target.Align = source.Align;
        target.DivType = source.DivType;
        target.Tab1 = source.Tab1;
        target.Tab2 = source.Tab2;
    }

    /// <summary>
    ///     Kopia riadku (segmentu).
    /// </summary>
    public static TableSegment Clone(TableSegment segment) => new() { Height = segment.Height, Width = segment.Width, Size = segment.Size };

    /// <summary>
    ///     Hlboka kopia typu zobrazenia vratane modov a zoznamov klucov.
    /// </summary>
    public static TableViewTypeTab Clone(TableViewTypeTab tab)
    {
        var copy = new TableViewTypeTab { ViewType = tab.ViewType, CountLinesRecord = tab.CountLinesRecord };
        foreach (var mode in tab.TypeModeItems)
            copy.TypeModeItems.Add(new TableTypeModeItem { ViewMode = mode.ViewMode, ItemsKeys = [..mode.ItemsKeys] });
        return copy;
    }

    /// <summary>
    ///     Vytvori typ zobrazenia, v ktorom maju vsetky mody (<see cref="TableViewMode.GetValues"/>) rovnake stlpce;
    ///     kazdy mod dostane vlastnu kopiu zoznamu klucov.
    /// </summary>
    /// <param name="tab">Typ zobrazenia, ktoremu sa mody nastavia.</param>
    /// <param name="keys">Kluce stlpcov v poradi.</param>
    public static void SetAllModes(TableViewTypeTab tab, IEnumerable<string> keys)
    {
        var list = keys.ToList();
        tab.TypeModeItems.Clear();
        foreach (var mode in TableViewMode.GetValues())
            tab.TypeModeItems.Add(new TableTypeModeItem { ViewMode = mode, ItemsKeys = [..list] });
    }

    /// <summary>
    ///     Pocet vyskytov kluca stlpca vo vsetkych typoch a modoch poradia stlpcov.
    /// </summary>
    public static int CountKeyUsages(IEnumerable<TableViewTypeTab> tabs, string key) =>
        tabs.SelectMany(t => t.TypeModeItems).Sum(m => m.ItemsKeys.Count(k => k == key));

    /// <summary>
    ///     Premenuje kluc stlpca vo vsetkych typoch a modoch poradia stlpcov.
    /// </summary>
    public static void RenameKey(IEnumerable<TableViewTypeTab> tabs, string oldKey, string newKey)
    {
        foreach (var mode in tabs.SelectMany(t => t.TypeModeItems))
            for (var i = 0; i < mode.ItemsKeys.Count; i++)
                if (mode.ItemsKeys[i] == oldKey)
                    mode.ItemsKeys[i] = newKey;
    }

    /// <summary>
    ///     Odoberie kluc stlpca zo vsetkych typov a modov poradia stlpcov.
    /// </summary>
    public static void RemoveKey(IEnumerable<TableViewTypeTab> tabs, string key)
    {
        foreach (var mode in tabs.SelectMany(t => t.TypeModeItems))
            mode.ItemsKeys.RemoveAll(k => k == key);
    }

    /// <summary>
    ///     Najde prvy kluc v poradi stlpcov, ku ktoremu neexistuje stlpec (ReadTables by taky subor odmietol).
    /// </summary>
    /// <returns>Typ, mod a kluc, alebo <c>null</c>, ak su vsetky kluce platne.</returns>
    public static (TableViewTypeTab Tab, TableTypeModeItem Mode, string Key)? FindUnknownKey(IEnumerable<TableViewTypeTab> tabs,
        IEnumerable<TableItem> items)
    {
        var keys = items.Select(i => i.Key).ToHashSet();
        foreach (var tab in tabs)
        foreach (var mode in tab.TypeModeItems)
        foreach (var key in mode.ItemsKeys)
            if (!keys.Contains(key))
                return (tab, mode, key);
        return null;
    }

    /// <summary>
    ///     Upravi pocet riadkov na presne <paramref name="count"/> – nadbytocne od konca odoberie, chybajuce doplni.
    /// </summary>
    /// <param name="rows">Riadky tabule.</param>
    /// <param name="count">Pozadovany pocet riadkov.</param>
    /// <param name="create">Vytvori novy riadok.</param>
    public static void ResizeRows(IList<TableSegment> rows, int count, Func<TableSegment> create)
    {
        while (rows.Count > count) rows.RemoveAt(rows.Count - 1);
        while (rows.Count < count) rows.Add(create());
    }
}
