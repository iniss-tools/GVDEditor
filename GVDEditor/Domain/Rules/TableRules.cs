using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

/// <summary>
/// Spolocne pravidla zoznamov tabul a textov: nazov a jednoznacny kluc, nazov kopie.
/// </summary>
internal static partial class TableRules
{
    /// <summary>
    /// Chyba nazvu; <see langword="null" />, ak je v poriadku.
    /// </summary>
    public static string? CheckName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? Resources.TableRules_Nazov : null;

    /// <summary>
    /// Chyba kluca polozky na pozicii <paramref name="index" /> - prazdny alebo rovnaky ako pri inej polozke.
    /// Na velkosti pismen zalezi (INISS kluce porovnava presne).
    /// </summary>
    public static string? CheckKey(IReadOnlyList<string?> keys, int index)
    {
        var key = keys[index]?.Trim() ?? "";
        if (key.Length == 0)
            return Resources.TableRules_Kluc;

        for (var i = 0; i < keys.Count; i++)
            if (i != index && (keys[i]?.Trim() ?? "") == key)
                return string.Format(CultureInfo.CurrentCulture, Resources.TableRules_Kluc_Existuje, key);

        return null;
    }

    /// <summary>
    /// Text, ktory v zozname este nie je: <paramref name="name" />, potom „name 2“, „name 3“… Cislo na konci sa
    /// neopakuje - kopia „name 2“ dostane „name 3“, nie „name 2 2“.
    /// </summary>
    public static string Unique(IEnumerable<string?> used, string name)
    {
        var set = used.Select(u => u?.Trim() ?? "").ToHashSet();
        name = name.Trim();
        if (!set.Contains(name))
            return name;

        var numbered = NumberedName().Match(name);
        var baseName = numbered.Success ? numbered.Groups[1].Value : name;
        for (var i = 2; ; i++)
            if (!set.Contains($"{baseName} {i}"))
                return $"{baseName} {i}";
    }

    [GeneratedRegex(@"^(.*\S) \d+$")]
    private static partial Regex NumberedName();
}

/// <summary>
/// Kontrola fyzickej tabule tak, ako ju INISS nacita.
/// </summary>
internal static class TablePhysicalRules
{
    /// <summary>
    /// Pole fyzickej tabule, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Name,
        Key,
        Catalog,
        Id
    }

    /// <summary>
    /// Vsetky chyby tabule na pozicii <paramref name="index" />.
    /// </summary>
    /// <param name="tables">fyzicke tabule</param>
    /// <param name="index">kontrolovana tabula</param>
    /// <param name="catalogs">katalogove tabule, ktore este existuju</param>
    public static List<(Field Field, string Message)> Check(IReadOnlyList<TablePhysical> tables, int index,
        IReadOnlyCollection<TableCatalog> catalogs)
    {
        var table = tables[index];
        var problems = new List<(Field, string)>();

        if (TableRules.CheckName(table.Name) is { } name)
            problems.Add((Field.Name, name));
        if (TableRules.CheckKey(tables.Select(t => t.Key).ToList(), index) is { } key)
            problems.Add((Field.Key, key));

        if (table.TableCatalog is not { } catalog || !catalogs.Contains(catalog))
            problems.Add((Field.Catalog, Resources.TablePhysicalRules_Katalog));
        else if (CheckId(table.ID, catalog.Manufacturer) is { } id)
            problems.Add((Field.Id, id));

        return problems;
    }

    /// <summary>
    /// Adresa tabule mimo rozsahu vyrobcu - INISS taku tabulu nezalozi. -1 (bez adresy) je vzdy v poriadku.
    /// </summary>
    public static string? CheckId(int id, TableManufacturer? manufacturer)
    {
        if (manufacturer is null || !manufacturer.IsKnownToIniss || manufacturer.IsAddressValid(id))
            return null;

        return string.Format(CultureInfo.CurrentCulture, Resources.TablePhysicalRules_Adresa, id, manufacturer.Name,
            manufacturer.MinAddress, manufacturer.MaxAddress);
    }
}

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

/// <summary>
/// Kontrola logickej tabule a jej zostavy. Chyby INISS neprijme (tabula bez zaznamov, neplatny riadok zostavy),
/// upozornenia znesie (zapise ich do logu) - tie ulozenie neblokuju.
/// </summary>
internal static class TableLogicalRules
{
    /// <summary>
    /// Pole logickej tabule, ku ktoremu sa chyba viaze.
    /// </summary>
    public enum Field
    {
        Name,
        Key,
        Count,
        Station,
        Segment
    }

    /// <summary>
    /// Vsetky chyby tabule na pozicii <paramref name="index" />; pri riadku zostavy aj jeho poradie.
    /// </summary>
    /// <param name="tables">logicke tabule</param>
    /// <param name="index">kontrolovana tabula</param>
    /// <param name="segments">zostava tabule, ak sa da zobrazit a pouzivatel ju upravuje; inak <see langword="null" /></param>
    public static List<(Field Field, int Row, string Message)> Check(IReadOnlyList<TableLogical> tables, int index,
        IReadOnlyList<TableLogicalSegment>? segments)
    {
        var table = tables[index];
        var problems = new List<(Field, int, string)>();

        if (TableRules.CheckName(table.Name) is { } name)
            problems.Add((Field.Name, -1, name));
        if (TableRules.CheckKey(tables.Select(t => t.Key).ToList(), index) is { } key)
            problems.Add((Field.Key, -1, key));
        if (table.Records.Count < 1)
            problems.Add((Field.Count, -1, Resources.FTableLogical_Bez_záznamov));

        if (segments is not null)
            for (var i = 0; i < segments.Count; i++)
                if (!IsValid(segments[i], table.Records.Count))
                    problems.Add((Field.Segment, i, string.Format(CultureInfo.CurrentCulture, Resources.FTableLogical_Neplatný_riadok_zostavy,
                        i + 1, segments[i].Table, table.Records.Count)));

        return problems;
    }

    /// <summary>
    /// Riadok zostavy: zaznamy v rozsahu 1 az pocet zaznamov (od ≤ do), riadok fyzickej tabule aspon 1, typ vybrany.
    /// </summary>
    public static bool IsValid(TableLogicalSegment segment, int recordCount) =>
        segment.FirstRecord >= 1 && segment.LastRecord <= recordCount && segment.FirstRecord <= segment.LastRecord &&
        segment.StartRow >= 1 && segment.TypeView != null;

    /// <summary>
    /// Nedostatky zostavy, ktore INISS znesie: typ, ktory katalog fyzickej tabule nepodporuje, riadky za poctom
    /// zaznamov fyzickej tabule, viac zaznamov na jednom riadku. Riadok zostavy je -1, ak sa tyka viacerych riadkov.
    /// </summary>
    public static List<(int Row, string Message)> Warnings(IReadOnlyList<TableLogicalSegment> segments)
    {
        var warnings = new List<(int, string)>();
        for (var i = 0; i < segments.Count; i++)
        {
            var s = segments[i];
            var supported = TableLogicalLayout.SupportedViewTypes(s.Table);
            if (s.TypeView != null && supported.Count != 0 && !supported.Contains(s.TypeView))
                warnings.Add((i, string.Format(CultureInfo.CurrentCulture, Resources.FTableLogical_Typ_nepodporovaný, s.Table, s.TypeView.Name)));
            if (s.Table.RecCount > 0 && s.EndRow > s.Table.RecCount)
                warnings.Add((i, string.Format(CultureInfo.CurrentCulture, Resources.FTableLogical_Mimo_tabule, s.Table, s.StartRow, s.EndRow,
                    s.Table.RecCount)));
        }

        // rovnaky riadok tej istej fyzickej tabule pre rozne zaznamy
        var rows = new Dictionary<(TablePhysical, int), SortedSet<int>>();
        foreach (var s in segments)
            for (var record = s.FirstRecord; record <= s.LastRecord; record++)
            {
                var key = (s.Table, s.StartRow + record - s.FirstRecord);
                if (!rows.TryGetValue(key, out var set))
                    rows[key] = set = [];
                set.Add(record);
            }

        foreach (var ((physical, row), set) in rows)
            if (set.Count > 1)
                warnings.Add((-1, string.Format(CultureInfo.CurrentCulture, Resources.FTableLogical_Kolízia_riadku, physical, row,
                    string.Join(", ", set))));

        return warnings;
    }

    /// <summary>
    /// Precita cislo stanice (IDSTATION) z pola - cislo, pripadne v tvare „5613600 – Nazov“ z ponuky.
    /// </summary>
    /// <returns>Cislo stanice, 0 pri prazdnom poli, alebo <see langword="null" /> pri neplatnom zadani.</returns>
    public static int? ParseStation(string? text)
    {
        var value = (text ?? "").Trim();
        if (value.Length == 0)
            return 0;

        var digits = new string(value.TakeWhile(char.IsDigit).ToArray());
        var rest = value[digits.Length..].TrimStart();
        if (digits.Length == 0 || !int.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out var id))
            return null;

        // za cislom moze byt len nazov z ponuky („ – Nazov“)
        return rest.Length == 0 || rest.StartsWith('–') || rest.StartsWith('-') ? id : null;
    }
}

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
