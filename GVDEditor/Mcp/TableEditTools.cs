using System.Globalization;
using GVDEditor.Domain.Analysis;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Properties;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using ToolsCore.Iniss.Elen;

namespace GVDEditor.Mcp;

/// <summary>
/// Nastroje MCP na zmeny tabul a pisiem otvoreneho grafikonu - rovnake kontroly ako stranky Lokalnych nastaveni.
/// Zmena nesmie pridat chybu, ktoru polozka este nemala; pouzitu tabulu ani pismo nezmaze.
/// </summary>
[McpServerToolType]
internal static class TableEditTools
{
    [McpServerTool(Name = "set_font", Title = "Set board font", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes a font of the board font list, or adds one when no font has the number (unset values then follow the number like in "
                 + "GVDEditor). Only the given values change. newId renumbers the font and the catalog board columns and train texts using it; TabTab "
                 + "sections keep their {n} and are listed in notes.")]
    public static async Task<TableChangeResult> SetFont(
        EditorAccess editor,
        [Description("Font number.")] int id,
        [Description("New number of an existing font.")] int? newId = null,
        [Description("Font name.")] string? name = null,
        [Description("regular, bold, italic, boldItalic or special.")] string? type = null,
        [Description("Height in points.")] int? size = null,
        [Description("Character width in points.")] int? width = null,
        [Description("Proportional font.")] bool? proportional = null,
        [Description("Has letters with diacritics.")] bool? diacritics = null,
        [Description("Has lowercase letters.")] bool? lowercase = null,
        [Description("Has uppercase letters.")] bool? uppercase = null,
        [Description("Has digits.")] bool? digits = null,
        [Description("Has special characters.")] bool? specialChars = null,
        [Description("Special characters have their own assignment.")] bool? specialAssignment = null,
        [Description("Font file; empty = none.")] string? file = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        List<string> notes = [];
        var (table, change) = await editor.WriteAsync(
            context => string.Format(CultureInfo.CurrentCulture, TableMapper.FindFont(context.Document, id) is null ? Resources.Agent_NewFont : Resources.Agent_SetFont,
                id),
            dryRun, context =>
            {
                var document = context.Document;
                var font = TableMapper.FindFont(document, id);
                List<string> before = [];
                if (font is null)
                {
                    if (newId is not null)
                        throw new McpException($"font.notFound: There is no font {id} to renumber; use list_tables.");
                    font = NewFont(id);
                    document.TableFonts.Add(font);
                }
                else
                {
                    before = TableMapper.Problems(document, font);
                }

                if (newId is { } number && number != font.FontID)
                {
                    // stlpce a texty idu s pismom ako na stranke Pisma; {n} v TabTab moze byt aj kod znaku
                    TableFontUsage.Replace(font.FontID, number, document.TableCatalogs, document.TableTexts);
                    var sections = TableFontUsage.Find(font.FontID, [], [], document.TabTabs).TabTabSections;
                    if (sections.Count > 0)
                        notes.Add($"TabTab sections {string.Join(", ", sections)} still contain {{{font.FontID}}} - it may be this font or a character code, "
                                  + "change them only when it is the font.");
                    font.FontID = number;
                }

                if (name is not null)
                    font.Name = name;
                if (type is not null)
                    font.Type = TableMapper.FontType(type);
                if (size is { } s)
                    font.Size = s;
                if (width is { } w)
                    font.Width = w;
                font.IsProportional = proportional ?? font.IsProportional;
                font.IsDia = diacritics ?? font.IsDia;
                font.IsLower = lowercase ?? font.IsLower;
                font.IsUpper = uppercase ?? font.IsUpper;
                font.IsNumber = digits ?? font.IsNumber;
                font.IsSpecChars = specialChars ?? font.IsSpecChars;
                font.IsSpecAssigment = specialAssignment ?? font.IsSpecAssigment;
                if (file is not null)
                    font.FileName = file;

                TableMapper.ThrowNewProblems(before, TableMapper.Problems(document, font));
                return TableMapper.Detail(document, font);
            }).ConfigureAwait(false);
        return new TableChangeResult(change, table, notes);
    }

    [McpServerTool(Name = "set_catalog_table", Title = "Set catalog board", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes a catalog board (board type: manufacturer, columns and which columns each view mode shows), or creates one when the key "
                 + "is new - empty or as a copy (copyFrom). Only the given values change; rows, columns and columnOrder replace the whole list. Read "
                 + "the board with get_table and the values with table_options first.")]
    public static async Task<TableChangeResult> SetCatalogTable(
        EditorAccess editor,
        [Description("Catalog board key; a new board is created when none has it.")] string key,
        [Description("New key (rename); physical boards and board texts follow it.")] string? newKey = null,
        [Description("Key of the catalog board a new one starts as a copy of; only when creating.")] string? copyFrom = null,
        [Description("Name for the operator.")] string? name = null,
        [Description("Comment.")] string? comment = null,
        [Description("Manufacturer key - protocol of the board (table_options).")] string? manufacturer = null,
        [Description("Most records the board can show; rows follow it unless rows are given.")] int? maxRecords = null,
        [Description("Minimal row height in points.")] int? minHeight = null,
        [Description("Number of segments (NUM_SEGMENTS), usually 1.")] int? segments = null,
        [Description("All row sizes, replacing the current ones; INISS does not use them.")] IReadOnlyList<CatalogRowInput>? rows = null,
        [Description("All columns, replacing the current ones. A column shown by a board text cannot be removed - keep it, or rename it with "
                     + "previousKey; renamed and removed keys follow in the column order.")]
        IReadOnlyList<CatalogColumnInput>? columns = null,
        [Description("Column order of all view types, replacing the current one.")] IReadOnlyList<ColumnOrderInput>? columnOrder = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        CheckRange(maxRecords, 1, nameof(maxRecords));
        CheckRange(minHeight, 0, nameof(minHeight));
        CheckRange(segments, 1, nameof(segments));
        var (table, change) = await editor.WriteAsync(context => TableLabel(TableMapper.FindCatalog(context.Document, key) is null, key), dryRun, context =>
        {
            var document = context.Document;
            var catalog = TableMapper.FindCatalog(document, key);
            List<string> before = [];
            if (catalog is null)
            {
                if (copyFrom is not null)
                {
                    var source = TableMapper.CatalogOrThrow(document, copyFrom);
                    catalog = TableCatalogEditing.Clone(source);
                    catalog.Key = key.Trim();
                    catalog.Name = TableRules.Unique(document.TableCatalogs.Select(t => t.Name), source.Name);
                    document.TableCatalogs.Insert(document.TableCatalogs.IndexOf(source) + 1, catalog);
                }
                else
                {
                    // ako tlacidlo Pridat na stranke Katalogove tabule
                    catalog = new TableCatalog
                    {
                        Key = key.Trim(), Name = key.Trim(), Comment = "", Manufacturer = TableManufacturer.Lcd1, MaxRecCount = 1, MinHeight = 10, NumSegments = 1
                    };
                    TableCatalogEditing.ResizeRows(catalog.Segments, 1, () => new TableSegment { Height = 10 });
                    document.TableCatalogs.Add(catalog);
                }
            }
            else if (copyFrom is not null)
                throw Exists(TableMapper.Catalog, catalog.Key);
            else
                before = TableMapper.Problems(document, catalog);

            if (newKey is not null)
                catalog.Key = newKey;
            if (name is not null)
                catalog.Name = name;
            if (comment is not null)
                catalog.Comment = comment;
            if (manufacturer is not null)
                catalog.Manufacturer = TableMapper.Manufacturer(manufacturer);
            if (minHeight is { } height)
                catalog.MinHeight = height;
            if (segments is { } count)
                catalog.NumSegments = count;
            if (maxRecords is { } max)
            {
                catalog.MaxRecCount = max;
                // pocet riadkov nasleduje pocet zaznamov, nove riadky maju minimalnu vysku - ako na stranke
                if (rows is null)
                    TableCatalogEditing.ResizeRows(catalog.Segments, max,
                        () => new TableSegment { Height = catalog.MinHeight, Width = catalog.Segments.LastOrDefault()?.Width ?? 0, Size = 0 });
            }

            if (rows is not null)
            {
                catalog.Segments.Clear();
                catalog.Segments.AddRange(rows.Select(r => new TableSegment { Height = r.Height, Width = r.Width, Size = r.Size }));
            }

            if (columns is not null)
                ReplaceColumns(document, catalog, columns);
            if (columnOrder is not null)
            {
                var tabs = columnOrder.Select(ViewTypeTab).ToList();
                catalog.ViewTypeTabs.Clear();
                catalog.ViewTypeTabs.AddRange(tabs);
            }

            TableMapper.ThrowNewProblems(before, TableMapper.Problems(document, catalog));
            return TableMapper.Detail(document, catalog);
        }).ConfigureAwait(false);
        return new TableChangeResult(change, table, []);
    }

    [McpServerTool(Name = "set_physical_table", Title = "Set physical board", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes a physical board (a board at the station: its catalog board, communication line and address), or creates one when the key "
                 + "is new - with catalog, or as a copy (copyFrom). Only the given values change.")]
    public static async Task<TableChangeResult> SetPhysicalTable(
        EditorAccess editor,
        [Description("Physical board key; a new board is created when none has it.")] string key,
        [Description("New key (rename); logical boards follow it.")] string? newKey = null,
        [Description("Key of the physical board a new one starts as a copy of; only when creating.")] string? copyFrom = null,
        [Description("Name for the operator.")] string? name = null,
        [Description("Comment.")] string? comment = null,
        [Description("Catalog board key - the type of the board; required for a new board without copyFrom.")] string? catalog = null,
        [Description("Address on the communication line within the range of the manufacturer (table_options); -1 = none.")] int? address = null,
        [Description("Communication line number; 0 = assigned automatically.")] int? line = null,
        [Description("Number of records the board shows.")] int? records = null,
        [Description("Records with reversed arrows; usually empty.")] string? reverseArrows = null,
        [Description("File name of the XML export; empty = no export.")] string? xmlFile = null,
        [Description("Note for the operator (REM).")] string? note = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        CheckRange(line, 0, nameof(line));
        CheckRange(records, 0, nameof(records));
        var (table, change) = await editor.WriteAsync(context => TableLabel(TableMapper.FindPhysical(context.Document, key) is null, key), dryRun, context =>
        {
            var document = context.Document;
            var physical = TableMapper.FindPhysical(document, key);
            List<string> before = [];
            if (physical is null)
            {
                if (copyFrom is not null)
                {
                    var source = TableMapper.PhysicalOrThrow(document, copyFrom);
                    physical = new TablePhysical
                    {
                        Key = key.Trim(), Name = TableRules.Unique(document.TablePhysicals.Select(t => t.Name), source.Name), TableCatalog = source.TableCatalog,
                        ID = source.ID, CommunicationPort = source.CommunicationPort, RecCount = source.RecCount, SaveXML = source.SaveXML,
                        ReverseArrows = source.ReverseArrows, Rem = source.Rem, Comment = source.Comment
                    };
                    document.TablePhysicals.Insert(document.TablePhysicals.IndexOf(source) + 1, physical);
                }
                else
                {
                    // ako tlacidlo Pridat na stranke Fyzicke tabule
                    var type = TableMapper.CatalogOrThrow(document,
                        catalog ?? throw new McpException("table.invalidValue: A new physical board needs catalog (or copyFrom)."));
                    physical = new TablePhysical
                    {
                        Key = key.Trim(), Name = key.Trim(), TableCatalog = type,
                        ID = type.Manufacturer is { IsKnownToIniss: true, MinAddress: >= 1 } manufacturer ? manufacturer.MinAddress : 1,
                        CommunicationPort = document.TablePhysicals.LastOrDefault()?.CommunicationPort ?? 1, RecCount = type.MaxRecCount,
                        SaveXML = "", ReverseArrows = "", Rem = "", Comment = ""
                    };
                    document.TablePhysicals.Add(physical);
                }
            }
            else if (copyFrom is not null)
                throw Exists(TableMapper.Physical, physical.Key);
            else
                before = TableMapper.Problems(document, physical);

            if (newKey is not null)
                physical.Key = newKey;
            if (name is not null)
                physical.Name = name;
            if (comment is not null)
                physical.Comment = comment;
            if (catalog is not null)
                physical.TableCatalog = TableMapper.CatalogOrThrow(document, catalog);
            if (address is { } id)
                physical.ID = id;
            if (line is { } port)
                physical.CommunicationPort = port;
            if (records is { } count)
                physical.RecCount = count;
            if (reverseArrows is not null)
                physical.ReverseArrows = reverseArrows;
            if (xmlFile is not null)
                physical.SaveXML = xmlFile;
            if (note is not null)
                physical.Rem = note;

            TableMapper.ThrowNewProblems(before, TableMapper.Problems(document, physical));
            return TableMapper.Detail(document, physical);
        }).ConfigureAwait(false);
        return new TableChangeResult(change, table, []);
    }

    [McpServerTool(Name = "set_logical_table", Title = "Set logical board", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Changes a logical board (how many records - trains - it has and which rows of which physical boards show them), or creates one when "
                 + "the key is new - empty or as a copy (copyFrom). Only the given values change; layout replaces all rows. Tracks point to logical "
                 + "boards (set_track_tables).")]
    public static async Task<TableChangeResult> SetLogicalTable(
        EditorAccess editor,
        [Description("Logical board key; a new board is created when none has it.")] string key,
        [Description("New key (rename); tracks follow it.")] string? newKey = null,
        [Description("Key of the logical board a new one starts as a copy of; only when creating.")] string? copyFrom = null,
        [Description("Name for the operator.")] string? name = null,
        [Description("Comment.")] string? comment = null,
        [Description("View type key (table_options); layout rows with the old type follow it where their physical board supports it.")] string? viewType = null,
        [Description("Number of records (trains) the board shows; layout rows ending at the last record follow it.")] int? records = null,
        [Description("Station number (IDSTATION); 0 = none.")] int? station = null,
        [Description("All layout rows, replacing the current ones.")] IReadOnlyList<LayoutRowInput>? layout = null,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        CheckRange(records, 1, nameof(records));
        CheckRange(station, 0, nameof(station));
        var (table, change) = await editor.WriteAsync(context => TableLabel(TableMapper.FindLogical(context.Document, key) is null, key), dryRun, context =>
        {
            var document = context.Document;
            var logical = TableMapper.FindLogical(document, key);
            List<string> before = [];
            if (logical is null)
            {
                var source = copyFrom is null ? null : TableMapper.LogicalOrThrow(document, copyFrom);
                logical = new TableLogical
                {
                    Key = key.Trim(),
                    Name = source is null ? key.Trim() : TableRules.Unique(document.TableLogicals.Select(t => t.Name), source.Name),
                    ViewType = source?.ViewType ?? TableViewType.Odchodova,
                    TypeViewFlags = source?.TypeViewFlags ?? "",
                    IdStation = source?.IdStation ?? 0,
                    Comment = source?.Comment ?? "",
                    Records = source is null ? TableLogicalLayout.ToRecords([], 1) : TableLogicalLayout.CloneRecords(source.Records)
                };
                document.TableLogicals.Insert(source is null ? document.TableLogicals.Count : document.TableLogicals.IndexOf(source) + 1, logical);
            }
            else if (copyFrom is not null)
                throw Exists(TableMapper.Logical, logical.Key);
            else
                before = TableMapper.Problems(document, logical);

            if (newKey is not null && newKey != logical.Key)
            {
                // priority tabul pri kolajach su podla kluca
                foreach (var track in document.Tracks)
                    if (track.TablePriorities.Remove(logical.Key, out var priority))
                        track.TablePriorities[newKey] = priority;
                logical.Key = newKey;
            }

            if (name is not null)
                logical.Name = name;
            if (comment is not null)
                logical.Comment = comment;
            if (station is { } id)
                logical.IdStation = id;

            var oldType = logical.ViewType;
            if (viewType is not null)
                logical.ViewType = TableMapper.ViewType(viewType);
            var count = records ?? logical.Records.Count;
            if (layout is not null)
                logical.Records = TableLogicalLayout.ToRecords(Layout(document, layout, logical.ViewType, count), count);
            else if (records is not null || viewType is not null)
            {
                // ako na stranke: zostava sa prisposobi poctu zaznamov a typu
                var segments = TableMapper.Layout(logical);
                if (segments is null && count != logical.Records.Count)
                    throw new McpException("table.layout: The placement of this board cannot be written as layout rows; give the whole layout with records.");
                if (segments is not null)
                {
                    TableLogicalLayout.ChangeViewType(segments, oldType, logical.ViewType);
                    TableLogicalLayout.Resize(segments, logical.Records.Count, count);
                    logical.Records = TableLogicalLayout.ToRecords(segments, count);
                }
            }

            TableMapper.ThrowNewProblems(before, TableMapper.Problems(document, logical));
            return TableMapper.Detail(document, logical);
        }).ConfigureAwait(false);
        return new TableChangeResult(change, table, []);
    }

    [McpServerTool(Name = "set_track_tables", Title = "Boards of a track", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Sets the logical boards that show the trains of a track; the list replaces the current one.")]
    public static async Task<TrackChangeResult> SetTrackTables(
        EditorAccess editor,
        [Description("Track key from get_station.")] string track,
        [Description("Logical board keys; empty = none.")] IReadOnlyList<string> tables,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var (result, change) = await editor.WriteAsync(_ => string.Format(CultureInfo.CurrentCulture, Resources.Agent_TrackTables, track), dryRun, context =>
        {
            var document = context.Document;
            var found = TableMapper.Find(document.Tracks.Where(t => t != Track.None), t => t.Key, track)
                        ?? throw new McpException($"track.notFound: Track '{track}' does not exist; use get_station.");
            var logicals = tables.Select(key => TableMapper.LogicalOrThrow(document, key)).Distinct().ToList();
            found.Tables.Clear();
            found.Tables.AddRange(logicals);
            return TableMapper.Track(found);
        }).ConfigureAwait(false);
        return new TrackChangeResult(change, result);
    }

    [McpServerTool(Name = "delete_table", Title = "Delete board or font", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false,
        UseStructuredContent = true)]
    [Description("Deletes a board or font nothing uses. A used one is refused with the list of usages (get_table shows them) - change those first.")]
    public static async Task<TableChangeResult> DeleteTable(
        EditorAccess editor,
        [Description("catalog, physical, logical or font.")] string kind,
        [Description("Key from list_tables; for a font its number.")] string key,
        [Description(EditorAccess.DryRunDescription)] bool dryRun = false)
    {
        var resource = TableMapper.Kind(kind) == TableMapper.Font ? Resources.Agent_DeleteFont : Resources.Agent_DeleteTable;
        var (_, change) = await editor.WriteAsync(_ => string.Format(CultureInfo.CurrentCulture, resource, key), dryRun, context =>
        {
            var document = context.Document;
            var item = TableMapper.Item(document, kind, key);
            if (TableMapper.UsedBy(document, item) is { Count: > 0 } usage)
                throw new McpException($"table.inUse: {kind} '{key}' is used by {string.Join(", ", usage)}; change those first.");
            var removed = item switch
            {
                TableCatalog catalog => document.TableCatalogs.Remove(catalog),
                TablePhysical physical => document.TablePhysicals.Remove(physical),
                TableLogical logical => document.TableLogicals.Remove(logical),
                TableFont font => document.TableFonts.Remove(font),
                _ => false
            };
            return removed;
        }).ConfigureAwait(false);
        return new TableChangeResult(change, null, []);
    }

    // ---------------------------------------------------------------- pomocne

    /// <summary>Nove pismo podla cisla - ako tlacidlo Pridat na stranke Pisma.</summary>
    private static TableFont NewFont(int id)
    {
        var code = new ElenFontCode(id);
        return new TableFont
        {
            Name = code.SuggestedName(),
            FontID = id,
            Type = code.SuggestedType,
            Width = code.SuggestedWidth,
            IsProportional = code.SuggestedProportional,
            Size = 7,
            FileName = "",
            IsDia = true,
            IsLower = true,
            IsUpper = true,
            IsNumber = true
        };
    }

    /// <summary>Stlpce z poziadavky - doterajsie podla kluca (alebo previousKey) zachovaju instanciu, na ktoru odkazuju texty.</summary>
    private static void ReplaceColumns(GrafikonDocument document, TableCatalog catalog, IReadOnlyList<CatalogColumnInput> columns)
    {
        var items = columns.Select(c => (c.PreviousKey ?? c.Key, new TableItem
        {
            Key = c.Key,
            Name = c.Name,
            FillSection = TableMapper.Content(c.Content),
            Line = c.Line,
            Start = c.Start,
            End = c.End,
            FontIdx = c.Font,
            Align = TableMapper.Align(c.Align),
            DivType = TableMapper.DivType(c.DivType),
            Tab1 = TableMapper.TabTab(document, c.Tab1),
            Tab2 = TableMapper.TabTab(document, c.Tab2)
        })).ToList();
        var removed = TableCatalogEditing.ReplaceColumns(catalog, items);
        var used = removed.SelectMany(item => TableCatalogEditing.TextsUsing(item, document.TableTexts).Select(text => $"{item.Key} (board text {text.Key})")).ToList();
        if (used.Count > 0)
            throw new McpException($"table.inUse: Columns shown by board texts cannot be removed: {string.Join(", ", used)}; keep them or rename them with previousKey.");
    }

    /// <summary>Poradie stlpcov typu zobrazenia - rezimy bez vlastnych stlpcov dostanu spolocne.</summary>
    private static TableViewTypeTab ViewTypeTab(ColumnOrderInput order)
    {
        var own = new Dictionary<TableViewMode, IReadOnlyList<string>>();
        foreach (var mode in order.Modes ?? [])
            own[TableMapper.ViewMode(mode.Mode)] = mode.Columns;
        var tab = new TableViewTypeTab
        {
            ViewType = TableMapper.ViewType(order.ViewType), CountLinesRecord = order.LinesPerRecord.ToString(CultureInfo.InvariantCulture)
        };
        foreach (var mode in TableViewMode.GetValues())
            tab.TypeModeItems.Add(new TableTypeModeItem
                { ViewMode = mode, ItemsKeys = [..(own.TryGetValue(mode, out var keys) ? keys : order.Columns ?? [])] });
        return tab;
    }

    /// <summary>Riadky zostavy z poziadavky; neplatny riadok (mimo zaznamov, riadok od 1) = chyba.</summary>
    private static List<TableLogicalSegment> Layout(GrafikonDocument document, IReadOnlyList<LayoutRowInput> rows, TableViewType viewType, int records)
    {
        var segments = new List<TableLogicalSegment>();
        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var segment = new TableLogicalSegment
            {
                Table = TableMapper.PhysicalOrThrow(document, row.Physical),
                FirstRecord = row.FirstRecord,
                LastRecord = row.LastRecord,
                StartRow = row.StartRow,
                TypeView = row.ViewType is null ? viewType : TableMapper.ViewType(row.ViewType)
            };
            if (!TableLogicalRules.IsValid(segment, records))
                throw new McpException($"table.invalid: Layout row {i + 1} ({row.Physical}): records must be within 1-{records} with first <= last and the "
                                       + "start row from 1.");
            segments.Add(segment);
        }

        return segments;
    }

    private static string TableLabel(bool created, string key) =>
        string.Format(CultureInfo.CurrentCulture, created ? Resources.Agent_NewTable : Resources.Agent_SetTable, key);

    private static McpException Exists(string kind, string key) =>
        new($"table.exists: The {kind} board '{key}' already exists; copyFrom only creates a new board.");

    private static void CheckRange(int? value, int min, string name)
    {
        if (value < min)
            throw new McpException($"table.invalidValue: {name} must be at least {min}.");
    }
}
