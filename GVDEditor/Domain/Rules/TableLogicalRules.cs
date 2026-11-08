using System.Globalization;
using GVDEditor.Domain.Editing;
using GVDEditor.Properties;

namespace GVDEditor.Domain.Rules;

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
