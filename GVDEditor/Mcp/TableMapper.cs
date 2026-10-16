using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using ModelContextProtocol;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Mcp;

/// <summary>Tabule a pisma pre nastroje MCP: vyhladanie podla kluca, zaznamy vysledkov, kontrola a pouzitie.</summary>
internal static class TableMapper
{
    public const string Catalog = "catalog";
    public const string Physical = "physical";
    public const string Logical = "logical";
    public const string Font = "font";

    /// <summary>Typy pisma pre agenta (kluce v ModeTabs.txt su slovenske).</summary>
    private static readonly (string Value, TableFontType Type)[] FontTypes =
    [
        ("regular", TableFontType.None), ("bold", TableFontType.Bold), ("italic", TableFontType.Italics), ("boldItalic", TableFontType.BoldItalics),
        ("special", TableFontType.Special)
    ];

    // ---------------------------------------------------------------- vyhladanie

    /// <summary>Polozka s klucom <paramref name="key" /> - presne, inak jedina bez ohladu na velkost pismen; null = nie je.</summary>
    public static T? Find<T>(IEnumerable<T> items, Func<T, string?> keyOf, string key) where T : class
    {
        var list = items.ToList();
        return list.FirstOrDefault(i => keyOf(i) == key)
               ?? (list.Where(i => string.Equals(keyOf(i), key, StringComparison.OrdinalIgnoreCase)).ToList() is [var single] ? single : null);
    }

    public static TableCatalog? FindCatalog(GrafikonDocument document, string key) => Find(document.TableCatalogs, c => c.Key, key);

    public static TablePhysical? FindPhysical(GrafikonDocument document, string key) => Find(document.TablePhysicals, p => p.Key, key);

    public static TableLogical? FindLogical(GrafikonDocument document, string key) => Find(document.TableLogicals, l => l.Key, key);

    public static TableCatalog CatalogOrThrow(GrafikonDocument document, string key) =>
        FindCatalog(document, key) ?? throw NotFound(Catalog, key);

    public static TablePhysical PhysicalOrThrow(GrafikonDocument document, string key) =>
        FindPhysical(document, key) ?? throw NotFound(Physical, key);

    public static TableLogical LogicalOrThrow(GrafikonDocument document, string key) =>
        FindLogical(document, key) ?? throw NotFound(Logical, key);

    /// <summary>Pismo s cislom <paramref name="id" />; null = nie je. Viac pisiem s tym istym cislom = chyba.</summary>
    public static TableFont? FindFont(GrafikonDocument document, int id)
    {
        var fonts = document.TableFonts.Where(f => f.FontID == id).ToList();
        return fonts.Count > 1
            ? throw new McpException($"font.ambiguous: {fonts.Count} fonts have number {id}; the user has to fix the font list in GVDEditor first.")
            : fonts.FirstOrDefault();
    }

    public static TableFont FontOrThrow(GrafikonDocument document, string key) =>
        (int.TryParse(key, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? FindFont(document, id) : null) ?? throw NotFound(Font, key);

    /// <summary>Druh tabule alebo pismo: catalog, physical, logical, font.</summary>
    public static string Kind(string kind) =>
        new[] { Catalog, Physical, Logical, Font }.FirstOrDefault(k => string.Equals(k, kind, StringComparison.OrdinalIgnoreCase))
        ?? throw new McpException($"table.invalidValue: Unknown kind '{kind}'; use catalog, physical, logical or font.");

    /// <summary>Tabula alebo pismo podla druhu a kluca (pri pisme cisla).</summary>
    public static object Item(GrafikonDocument document, string kind, string key) =>
        Kind(kind) switch
        {
            Catalog => CatalogOrThrow(document, key),
            Physical => PhysicalOrThrow(document, key),
            Logical => LogicalOrThrow(document, key),
            _ => FontOrThrow(document, key)
        };

    public static McpException NotFound(string kind, string key) =>
        new($"table.notFound: There is no {kind} {(kind == Font ? "" : "board ")}'{key}'; use list_tables.");

    /// <summary>Sekcia TabTab podla kluca; null alebo prazdny kluc = ziadna.</summary>
    public static TableTabTab TabTab(GrafikonDocument document, string? key) =>
        string.IsNullOrWhiteSpace(key)
            ? TableTabTab.Empty
            : Find(document.TabTabs.Where(t => t != TableTabTab.Empty), t => t.Key, key)
              ?? throw new McpException($"tabtab.notFound: TabTab section '{key}' does not exist; use list_tables.");

    /// <summary>Hodnota enumeracie INISSu; neznama = chyba so zoznamom platnych hodnot.</summary>
    public static T Option<T>(IReadOnlyList<T> values, Func<T, string> valueOf, string value, string what)
    {
        foreach (var item in values)
            if (string.Equals(valueOf(item), value, StringComparison.OrdinalIgnoreCase))
                return item;
        throw new McpException($"table.invalidValue: Unknown {what} '{value}'; use one of {string.Join(", ", values.Select(valueOf))} (table_options).");
    }

    public static TableManufacturer Manufacturer(string value) => Option(TableManufacturer.GetValues(), m => m.Name, value, "manufacturer");

    public static TableViewType ViewType(string value) => Option(TableViewType.GetValues(), v => v.Key!, value, "view type");

    public static TableViewMode ViewMode(string value) => Option(TableViewMode.GetValues(), v => v.Key!, value, "view mode");

    public static TableFillSection Content(int value) => Option(TableFillSection.GetValues(), v => Number(v.Id), Number(value), "content");

    public static TableAlign Align(int value) => Option(TableAlign.GetValues(), v => Number(v.Id), Number(value), "alignment");

    public static TableDivType DivType(int value) => Option(TableDivType.GetValues(), v => Number(v.Id), Number(value), "DIVTYPE");

    public static TableFontType FontType(string value) => Option(FontTypes, t => t.Value, value, "font type").Type;

    public static string? FontTypeName(TableFontType? type) => FontTypes.FirstOrDefault(t => t.Type == type).Value;

    public static IEnumerable<(string Value, TableFontType Type)> AllFontTypes => FontTypes;

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);

    // ---------------------------------------------------------------- vysledky

    public static TableDetail Detail(GrafikonDocument document, object item) =>
        item switch
        {
            TableCatalog catalog => Detail(document, catalog),
            TablePhysical physical => Detail(document, physical),
            TableLogical logical => Detail(document, logical),
            TableFont font => Detail(document, font),
            _ => throw new ArgumentException("Unknown table item.", nameof(item))
        };

    public static TableDetail Detail(GrafikonDocument document, TableCatalog catalog) =>
        new(Catalog, new CatalogTableDetail(catalog.Key, catalog.Name, catalog.Comment ?? "", catalog.Manufacturer?.Name, catalog.MaxRecCount, catalog.MinHeight,
                catalog.NumSegments, catalog.Segments.Select(s => new RowInfo(s.Height, s.Width, s.Size)).ToList(),
                catalog.Items.Select(i => new ColumnInfo(i.Key, i.Name, i.FillSection?.Id ?? 0, i.FillSection?.Name ?? "", i.Line, i.Start, i.End, i.FontIdx,
                    i.Align?.Id ?? 0, i.DivType?.Id ?? 0, TabTabKey(i.Tab1), TabTabKey(i.Tab2),
                    TableCatalogEditing.TextsUsing(i, document.TableTexts).Select(t => t.Key).ToList())).ToList(),
                catalog.ViewTypeTabs.Select(t => new ColumnOrderInfo(t.ViewType?.Key ?? "",
                    int.TryParse(t.CountLinesRecord, NumberStyles.Integer, CultureInfo.InvariantCulture, out var lines) ? lines : 0,
                    t.TypeModeItems.Select(m => new ViewModeInfo(m.ViewMode?.Key ?? "", m.ItemsKeys.ToList())).ToList())).ToList()),
            null, null, null, UsedBy(document, catalog), Problems(document, catalog),
            TableCatalogRules.Warnings(catalog).Select(w => w.Message).ToList());

    public static TableDetail Detail(GrafikonDocument document, TablePhysical physical) =>
        new(Physical, null, new PhysicalTableDetail(physical.Key, physical.Name, physical.Comment ?? "", physical.TableCatalog?.Key, physical.ID,
                physical.CommunicationPort, physical.RecCount, physical.ReverseArrows ?? "", physical.SaveXML ?? "", physical.Rem ?? ""),
            null, null, UsedBy(document, physical), Problems(document, physical), []);

    public static TableDetail Detail(GrafikonDocument document, TableLogical logical)
    {
        var layout = Layout(logical);
        return new TableDetail(Logical, null, null, new LogicalTableDetail(logical.Key, logical.Name, logical.Comment ?? "", logical.ViewType?.Key,
                logical.Records.Count, logical.IdStation,
                layout?.Select(s => new LayoutRowInfo(s.Table.Key, s.FirstRecord, s.LastRecord, s.StartRow, s.TypeView?.Key)).ToList(),
                layout is null
                    ? logical.Records.Select((r, i) => $"{i + 1}: " + string.Join("; ", r.Positions.Select(p => $"{p.Table?.Key} row {p.Position} {p.TypeView?.Key}")))
                        .ToList()
                    : null),
            null, UsedBy(document, logical), Problems(document, logical),
            layout is null ? [] : TableLogicalRules.Warnings(layout).Select(w => w.Message).ToList());
    }

    public static TableDetail Detail(GrafikonDocument document, TableFont font) =>
        new(Font, null, null, null, new FontDetail(font.FontID, font.Name, FontTypeName(font.Type), font.Size, font.Width, font.IsProportional, font.IsDia,
                font.IsLower, font.IsUpper, font.IsNumber, font.IsSpecChars, font.IsSpecAssigment, font.FileName ?? "", TableTools.DescribeElen(new ElenFontCode(font.FontID))),
            UsedBy(document, font), Problems(document, font), []);

    public static TrackInfo Track(Track track) =>
        new(track.Key, track.Name, track.Platform?.Key, track.Tables.Select(t => t.Key).ToList());

    /// <summary>Zostava logickej tabule; null = umiestnenia sa zostavou vyjadrit nedaju.</summary>
    public static List<TableLogicalSegment>? Layout(TableLogical logical)
    {
        var segments = TableLogicalLayout.FromRecords(logical.Records);
        return TableLogicalLayout.IsExpressible(logical.Records, segments) ? segments : null;
    }

    private static string? TabTabKey(TableTabTab? tab) => tab is null || tab == TableTabTab.Empty ? null : tab.Key;

    // ---------------------------------------------------------------- kontrola

    /// <summary>Chyby polozky podla kontroly okna Lokalne nastavenia (v jazyku obsluhy).</summary>
    public static List<string> Problems(GrafikonDocument document, object item)
    {
        switch (item)
        {
            case TableCatalog catalog:
                return TableCatalogRules.Check(document.TableCatalogs, document.TableCatalogs.IndexOf(catalog)).Select(p => p.Message).ToList();
            case TablePhysical physical:
                return TablePhysicalRules.Check(document.TablePhysicals, document.TablePhysicals.IndexOf(physical), document.TableCatalogs)
                    .Select(p => p.Message).ToList();
            case TableLogical logical:
                return TableLogicalRules.Check(document.TableLogicals, document.TableLogicals.IndexOf(logical), Layout(logical)).Select(p => p.Message).ToList();
            case TableFont font:
                var fonts = document.TableFonts.ToList();
                return new[] { FontRules.CheckName(font.Name), FontRules.CheckId(fonts, fonts.IndexOf(font)) }.OfType<string>().ToList();
            default:
                return [];
        }
    }

    /// <summary>
    /// Zmena nesmie pridat chybu, ktoru polozka predtym nemala - chyba z doby pred zmenou (napr. z importu) zmenu nebrani.
    /// </summary>
    public static void ThrowNewProblems(IReadOnlyCollection<string> before, IEnumerable<string> after)
    {
        var added = after.Where(p => !before.Contains(p)).ToList();
        if (added.Count > 0)
            throw new McpException("table.invalid: " + string.Join(" ", added));
    }

    // ---------------------------------------------------------------- pouzitie

    public static List<string> UsedBy(GrafikonDocument document, object item) =>
        item switch
        {
            TableCatalog catalog => TableUsage.CatalogUsage(catalog, document.TablePhysicals, document.TableTexts) is var (physicals, texts)
                ? physicals.Select(p => $"physical board {p.Key}").Concat(texts.Select(t => $"board text {t.Text.Key} (column {t.Column?.Key})")).ToList()
                : [],
            TablePhysical physical => TableUsage.LogicalPositions(physical, document.TableLogicals)
                .Select(u => $"logical board {u.Logical.Key} (row {u.Position + 1})").Distinct().ToList(),
            TableLogical logical => TableUsage.Tracks(logical, document.Tracks).Select(t => $"track {t.Key}").ToList(),
            TableFont font => FontUsage(document, font.FontID),
            _ => []
        };

    private static List<string> FontUsage(GrafikonDocument document, int id)
    {
        var usage = document.TableCatalogs.SelectMany(c => c.Items.Where(i => i.FontIdx == id).Select(i => $"catalog board {c.Key} column {i.Key}")).ToList();
        usage.AddRange(document.TableTexts.Select(t => (t.Key, Count: t.Trains.Count(train => train.FontID == id)))
            .Where(t => t.Count > 0).Select(t => $"board text {t.Key} ({t.Count} trains)"));
        usage.AddRange(TableFontUsage.Find(id, [], [], document.TabTabs).TabTabSections.Select(s => $"TabTab section {s} ({{{id}}})"));
        return usage;
    }
}
