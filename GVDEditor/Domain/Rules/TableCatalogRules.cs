using System.Globalization;
using GVDEditor.Domain.Editing;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Kontrola katalogovej tabule a jej stlpcov tak, ako ich INISS nacita. Chyby INISS neprijme (stlpec s nulovou
/// sirkou, chybajuca TAB1/TAB2, pocet rezimov...), upozornenia znesie - tie ulozenie neblokuju. Prekryv stlpcov
/// na riadku chybou nie je (alternativy pre rozne rezimy), ani poradie stlpcov podla pozicie.
/// </summary>
internal static class TableCatalogRules
{
    /// <summary>
    /// Cast katalogovej tabule, ku ktorej sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Name,
        Key,
        Manufacturer,
        Column,
        Order
    }

    /// <summary>
    /// Najvacsia pozicia stlpca na tabuli ELEN (v bodoch).
    /// </summary>
    public const int ElenMaxPosition = 512;

    /// <summary>
    /// Vsetky chyby tabule na pozicii <paramref name="index" />; pri stlpci aj jeho poradie.
    /// </summary>
    public static List<(Field Field, int Column, string Message)> Check(IReadOnlyList<TableCatalog> tables, int index)
    {
        var table = tables[index];
        var problems = new List<(Field, int, string)>();

        if (TableRules.CheckName(table.Name) is { } name)
            problems.Add((Field.Name, -1, name));
        if (TableRules.CheckKey(tables.Select(t => t.Key).ToList(), index) is { } key)
            problems.Add((Field.Key, -1, key));
        if (table.Manufacturer is null)
            problems.Add((Field.Manufacturer, -1, Resources.TableCatalogRules_Vyrobca));

        for (var i = 0; i < table.Items.Count; i++)
            if (CheckColumn(table, i) is { } column)
                problems.Add((Field.Column, i, string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Stlpec,
                    ColumnLabel(table.Items[i], i), column)));

        var modesCount = TableViewMode.GetValues().Count;
        foreach (var tab in table.ViewTypeTabs)
            if (tab.TypeModeItems.Count != modesCount)
                problems.Add((Field.Order, -1, string.Format(CultureInfo.CurrentCulture, Resources.FTableCatalog_bSave_NespravnyPocetModov,
                    tab.ViewType, tab.TypeModeItems.Count, modesCount)));

        // pri stlpci s neplatnym klucom by odkaz v poradi hlasil tu istu chybu druhykrat
        if (!problems.Any(p => p.Item1 == Field.Column) && TableCatalogEditing.FindUnknownKey(table.ViewTypeTabs, table.Items) is { } unknown)
            problems.Add((Field.Order, -1, string.Format(CultureInfo.CurrentCulture, Resources.FTableCatalog_bSave_NeznamyStlpec,
                unknown.Tab.ViewType, unknown.Mode.ViewMode, unknown.Key)));

        return problems;
    }

    /// <summary>
    /// Prva chyba stlpca na pozicii <paramref name="index" />; <see langword="null" />, ak je v poriadku.
    /// </summary>
    public static string? CheckColumn(TableCatalog table, int index)
    {
        var item = table.Items[index];
        if (string.IsNullOrWhiteSpace(item.Name))
            return Resources.TableCatalogRules_Stlpec_Nazov;

        var key = item.Key?.Trim() ?? "";
        if (key.Length == 0)
            return Resources.TableCatalogRules_Stlpec_Kluc;
        for (var i = 0; i < table.Items.Count; i++)
            if (i != index && (table.Items[i].Key?.Trim() ?? "") == key)
                return string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Stlpec_Kluc_Existuje, key);

        if (item.End <= item.Start)
            return string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Stlpec_Sirka, item.Start, item.End);

        if (item.DivType is { } div && div != TableDivType.Free && IsEmpty(item.Tab1))
            return string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Stlpec_Tab1, div.Id);
        if (item.DivType == TableDivType.TableTime && IsEmpty(item.Tab2))
            return Resources.TableCatalogRules_Stlpec_Tab2;

        if (table.Manufacturer == TableManufacturer.Elen && item.End > ElenMaxPosition)
            return string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Stlpec_ELEN, ElenMaxPosition, item.End);

        return null;
    }

    /// <summary>
    /// Nedostatky stlpcov, ktore INISS znesie: pozicia mimo hranic znakov tabule, TAB1/TAB2, ktore sa pri spôsobe
    /// plnenia nepouziju.
    /// </summary>
    public static List<(int Column, string Message)> Warnings(TableCatalog table)
    {
        var warnings = new List<(int, string)>();
        var cell = CellWidth(table.Manufacturer);
        for (var i = 0; i < table.Items.Count; i++)
        {
            var item = table.Items[i];
            var label = ColumnLabel(item, i);
            if (cell > 1 && (item.Start % cell != 0 || item.End % cell != 0))
                warnings.Add((i, string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Nasobok, label, cell,
                    table.Manufacturer!.Name)));

            if (item.DivType == TableDivType.Free && !IsEmpty(item.Tab1))
                warnings.Add((i, string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Tab_Navyse, label, "TAB1", 0)));
            if (item.DivType is { } div && div != TableDivType.TableTime && !IsEmpty(item.Tab2))
                warnings.Add((i, string.Format(CultureInfo.CurrentCulture, Resources.TableCatalogRules_Tab_Navyse, label, "TAB2", div.Id)));
        }

        return warnings;
    }

    /// <summary>
    /// Sirka znakovej bunky tabule - START a END stlpcov maju byt jej nasobkom; 1 = bez obmedzenia.
    /// </summary>
    public static int CellWidth(TableManufacturer? manufacturer)
    {
        if (manufacturer == TableManufacturer.Elenold)
            return 6;
        if (manufacturer == TableManufacturer.Lcd || manufacturer == TableManufacturer.Ers || manufacturer == TableManufacturer.Fers ||
            manufacturer == TableManufacturer.Lcd1 || manufacturer == TableManufacturer.Erp)
            return 8;
        return 1;
    }

    private static bool IsEmpty(TableTabTab? tab) => tab is null || tab == TableTabTab.Empty || string.IsNullOrEmpty(tab.Key);

    private static string ColumnLabel(TableItem item, int index) =>
        string.IsNullOrWhiteSpace(item.Name) ? (index + 1).ToString(CultureInfo.CurrentCulture) : $"„{item.Name}“";
}
