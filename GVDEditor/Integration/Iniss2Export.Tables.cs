using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Entities;
using GVDEditor.TabTabEditor;
using ToolsCore.Iniss.Elen;
using ToolsCore.Iniss.TabTab;
using ToolsCore.Iniss2.Tables;

namespace GVDEditor.Integration;

internal static partial class Iniss2Export
{
    // %stlpec% - text ineho stlpca v pravidle INISS 3.39
    [GeneratedRegex("%[^%]+%")]
    private static partial Regex ColumnReference();

    /// <summary>
    /// Tabule stanice: fyzicka tabula 3.39 je tabula INISS2, zaznamy logickych tabul na nej su casti. Logicka tabula zobrazena
    /// na viacerych tabuliach rovnako (zrkadlo) dostane na kazdej vlastnu cast s rovnakym druhom a kolajami - server ich naplni
    /// rovnako. Pokracovanie logickej tabule na dalsej tabuli je cast s predchadzajucou tabulou.
    /// </summary>
    private sealed partial class StationBuilder
    {
        // druhy zariadenia s polohou udajov v znakoch; INISS 3.39 ju ma v bodoch po 8 na znak
        private static readonly HashSet<TableManufacturer> CharacterClasses = [TableManufacturer.Lcd, TableManufacturer.Lcd1, TableManufacturer.Erp, TableManufacturer.Fers];

        private static readonly Dictionary<TableManufacturer, string> Classes = new()
        {
            [TableManufacturer.Lcd] = "LCD",
            [TableManufacturer.Lcd1] = "LCD1",
            [TableManufacturer.Erp] = "ERP",
            [TableManufacturer.Fers] = "FERS",
            [TableManufacturer.Elen] = "ELEN",
            [TableManufacturer.Elenold] = "ELENOLD",
            [TableManufacturer.Elen10] = "ELEN10",
            [TableManufacturer.Elen16] = "ELEN16",
            [TableManufacturer.Elen16Kam] = "ELEN16KAM",
            [TableManufacturer.Elekon] = "ELEKON"
        };

        private const int PointsPerCharacter = 8;

        // riadok fiktivneho stlpca predlohy - server ho nezobrazi, text ma len pre ine stlpce
        private const int FictiveRow = -1;

        // druhy tabul, ktore nevedia striedat casti textu - #MERGE sa na nich sprava ako #MERGE2
        private static readonly HashSet<TableManufacturer> SinglePartClasses =
            [TableManufacturer.AdonBuse, TableManufacturer.Lcd, TableManufacturer.Ers, TableManufacturer.Fers];

        // druhy, ktorym server sirku pisma berie z binarnej banky; bez nej sa nespusti
        private static readonly HashSet<string> BankClasses = new(StringComparer.Ordinal) { "ELEN", "ELENOLD", "ELEN10", "ELEN16", "ELEN16KAM" };

        private readonly List<(TableCatalog Source, string Variant, CatalogTable Catalog)> _catalogs = [];

        private (List<TableDefinition> Tables, List<CatalogTable> Catalogs) ToTables()
        {
            var placements = Placements();
            var tables = new List<TableDefinition>();

            foreach (var physical in document.TablePhysicals)
            {
                var internet = physical.ID == TableDefinition.NoAddress;
                if (!internet && !Classes.ContainsKey(physical.TableCatalog.Manufacturer))
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TableUnsupported, physical.Key));
                    continue;
                }

                var parts = Parts(physical, placements, internet);
                var address = internet ? TableDefinition.NoAddress : physical.ID;
                var save = string.IsNullOrEmpty(physical.SaveXML) ? "" : physical.SaveXML + ".xml";
                tables.Add(new TableDefinition(physical.Key, physical.Name, LineOf(physical, internet), address, save, parts)
                {
                    ReverseArrows = physical.ReverseArrows ?? ""
                });
            }

            return (tables, [.. _catalogs.Select(catalog => catalog.Catalog)]);
        }

        // linka tabule zo zistenia INISSu 3.39; tabula bez zariadenia linku nema
        private string LineOf(TablePhysical physical, bool internet)
        {
            if (internet)
                return "";

            var line = source.TableLines.FirstOrDefault(line => line.TableKey == physical.Key && string.Equals(line.Grafikon, grafikon, StringComparison.OrdinalIgnoreCase));
            if (line is null)
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TableWithoutLine, physical.Key));
                return "";
            }

            state.Lines.TryAdd(line.Line.Key, line.Line);
            return line.Line.Key;
        }

        // co je na ktorej pozicii fyzickej tabule: logicka tabula, jej zaznam a druh zobrazenia
        private Dictionary<TablePhysical, Dictionary<int, (TableLogical Logical, int Record, TableViewType? View)>> Placements()
        {
            var placements = new Dictionary<TablePhysical, Dictionary<int, (TableLogical, int, TableViewType?)>>();

            foreach (var logical in document.TableLogicals)
            {
                for (var record = 0; record < logical.Records.Count; record++)
                {
                    foreach (var position in logical.Records[record].Positions.Where(position => position.Table is not null))
                    {
                        if (!placements.TryGetValue(position.Table, out var table))
                            placements[position.Table] = table = [];

                        table.TryAdd(position.Position, (logical, record, position.TypeView ?? logical.ViewType));
                    }
                }
            }

            return placements;
        }

        private List<TablePart> Parts(TablePhysical physical, Dictionary<TablePhysical, Dictionary<int, (TableLogical Logical, int Record, TableViewType? View)>> placements,
            bool internet)
        {
            var parts = new List<TablePart>();
            var table = placements.GetValueOrDefault(physical) ?? [];
            var position = 0;

            // niektore data maju REC_COUNT 0, hoci logicke tabule zaznamy na tabulu kladu
            var count = Math.Max(physical.RecCount, table.Count == 0 ? 0 : table.Keys.Max() + 1);

            while (position < count)
            {
                var start = position;
                if (!table.TryGetValue(position, out var first))
                {
                    while (position < count && !table.ContainsKey(position))
                        position++;

                    parts.Add(new TablePart("", CatalogKey(physical.TableCatalog, null, internet), TablePart.NotDefined, position - start));
                    continue;
                }

                position++;
                while (position < count && table.TryGetValue(position, out var next) && ReferenceEquals(next.Logical, first.Logical)
                       && next.View == first.View && next.Record == first.Record + position - start)
                    position++;

                parts.Add(Part(physical, first.Logical, first.Record, first.View, position - start, placements, internet));
            }

            return parts;
        }

        private TablePart Part(TablePhysical physical, TableLogical logical, int record, TableViewType? view, int records,
            Dictionary<TablePhysical, Dictionary<int, (TableLogical Logical, int Record, TableViewType? View)>> placements, bool internet)
        {
            var kind = Kind(view);
            if (kind == TablePart.NotDefined)
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TableViewUnknown, logical.Key));

            var tracks = document.Tracks.Where(track => track.Tables.Contains(logical)).ToList();

            // zaznamy pred tymto su na inej tabuli - cast na nu nadvazuje
            var previous = record == 0
                ? ""
                : placements.Where(other => !ReferenceEquals(other.Key, physical)
                                            && other.Value.Values.Any(placed => ReferenceEquals(placed.Logical, logical) && placed.Record == record - 1))
                    .Select(other => other.Key.Key).FirstOrDefault("");

            var stations = logical.IdStation != 0 && logical.IdStation != Station ? logical.IdStation.ToString(CultureInfo.InvariantCulture) : "";

            return new TablePart(logical.Name ?? "", CatalogKey(physical.TableCatalog, view, internet), kind, records)
            {
                Tracks = [.. tracks.Select(track => track.Key)],
                TrackPriorities = tracks.Where(track => track.TablePriorities.GetValueOrDefault(logical.Key) != 0)
                    .ToDictionary(track => track.Key, track => track.TablePriorities[logical.Key]),
                Stations = stations,
                Previous = previous
            };
        }

        private static string Kind(TableViewType? view)
        {
            if (view == TableViewType.Odchodova)
                return "DEPARTURE";
            if (view == TableViewType.Prichodova)
                return "ARRIVAL";
            if (view == TableViewType.Nastupistna)
                return "PLATFORM";

            return view == TableViewType.Podchodova ? "SUBWAY" : TablePart.NotDefined;
        }

        // predloha pre druh zobrazenia; rovnaky obsah pre iny druh pouzije tu istu predlohu
        private string CatalogKey(TableCatalog source, TableViewType? view, bool internet)
        {
            var catalog = Catalog(source, view, internet);
            var known = _catalogs.Find(item => ReferenceEquals(item.Source, source) && Same(item.Catalog, catalog));
            if (known.Catalog is not null)
                return known.Catalog.Key;

            var variants = _catalogs.Count(item => ReferenceEquals(item.Source, source));
            var key = variants == 0 ? source.Key : $"{source.Key} ({variants + 1})";
            _catalogs.Add((source, key, catalog with { Key = key }));
            return key;
        }

        private static bool Same(CatalogTable a, CatalogTable b) =>
            a.Class == b.Class && a.FontBank == b.FontBank && a.Columns.SequenceEqual(b.Columns);

        private CatalogTable Catalog(TableCatalog source, TableViewType? view, bool internet)
        {
            var tab = source.ViewTypeTabs.FirstOrDefault(tab => tab.ViewType == view) ?? source.ViewTypeTabs.FirstOrDefault();
            var lines = int.TryParse(tab?.CountLinesRecord?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var count) && count > 0 ? count : 1;
            var characters = CharacterClasses.Contains(source.Manufacturer);

            var columns = new List<CatalogColumn>();
            foreach (var item in ShownItems(source, tab).OrderBy(item => item.Line).ThenBy(item => item.Start))
            {
                // udaje jedneho riadka musia ist zlava doprava bez prekryvu, inak server zapis na tabulu odmietne
                if (columns.Exists(column => column.Row == item.Line && Unit(item.Start, characters) < column.End && column.Start < Unit(item.End, characters)))
                {
                    state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.ColumnOverlap, $"{source.Key}: {item.Key}"));
                    continue;
                }

                columns.Add(Column(source, item, characters));
            }

            // stlpce, na ktore odkazuje %meno% v pravidlach TabTab zobrazenych stlpcov - server ich text berie z predlohy, preto idu
            // do predlohy ako fiktivne (riadok -1, nezobrazia sa)
            for (var i = 0; i < columns.Count; i++)
            {
                foreach (var name in PeerNames(columns[i].Table1))
                {
                    var item = source.Items.FirstOrDefault(other => string.Equals(other.Name, name, StringComparison.OrdinalIgnoreCase)
                                                                    || string.Equals(other.Key, name, StringComparison.OrdinalIgnoreCase));
                    if (item is not null && !columns.Exists(column => column.Key == item.Key))
                        columns.Add(Column(source, item, characters) with { Row = FictiveRow, Start = 0, End = 0 });
                }
            }

            // posledny riadok zaznamu bez udaja by server nepocital
            if (columns.Count == 0 || columns.Max(column => column.Row) < lines - 1)
                columns.Add(new CatalogColumn($"Riadok {lines}", CatalogColumn.NoContent, lines - 1, 0, 0, "LEFT"));

            var cls = internet ? CatalogTable.Internet : Classes[source.Manufacturer];
            return new CatalogTable(source.Key, source.Name, cls) { Columns = columns, FontBank = BankClasses.Contains(cls) ? FontBank(source) : "" };
        }

        // nazov banky je ulozeny v subore .bin a musi sa zhodovat s menom suboru v Data\TableFonts
        private string FontBank(TableCatalog catalog)
        {
            if (catalog.Key is null || !source.FontBanks.TryGetValue(catalog.Key, out var file))
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.FontBankMissing, catalog.Key ?? ""));
                return "";
            }

            try
            {
                var name = ElenFontFile.Load(file).Name;
                state.FontFiles.TryAdd(name, file);
                return name;
            }
            catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException or FormatException)
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.FontBankMissing, $"{catalog.Key}: {file}"));
                return "";
            }
        }

        // udaje vypisane pri vlaku; udaje rezimov s meskanim sa pridaju, ak nezaberu miesto inym
        private static IEnumerable<TableItem> ShownItems(TableCatalog source, TableViewTypeTab? tab)
        {
            var train = tab?.TypeModeItems.FirstOrDefault(mode => mode.ViewMode == TableViewMode.Vlak);
            if (train is null)
                return source.Items.Where(item => item.FillSection != TableFillSection.NotDefined);

            var keys = train.ItemsKeys.ToList();
            foreach (var mode in tab!.TypeModeItems.Where(mode => mode.ViewMode == TableViewMode.VlakZmeskany || mode.ViewMode == TableViewMode.VlakZmeskanyOdchod
                                                                  || mode.ViewMode == TableViewMode.VlakZmeskanyPrichod))
            {
                foreach (var key in mode.ItemsKeys.Where(key => !keys.Contains(key)))
                {
                    var item = source.Items.Find(item => item.Key == key);
                    var overlaps = item is null || keys.Select(shown => source.Items.Find(other => other.Key == shown))
                        .Any(other => other is not null && other.Line == item.Line && item.Start < other.End && other.Start < item.End);
                    if (!overlaps)
                        keys.Add(key);
                }
            }

            return keys.Select(key => source.Items.Find(item => item.Key == key)).OfType<TableItem>();
        }

        private CatalogColumn Column(TableCatalog source, TableItem item, bool characters)
        {
            var content = CatalogColumn.ContentFor(item.FillSection);
            if (content is null)
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.ContentUnsupported, $"{source.Key}: {item.Key}"));
                content = CatalogColumn.NoContent;
            }

            var (translation, table1, table2) = Translation(source, item);
            return new CatalogColumn(item.Key, content, item.Line, Unit(item.Start, characters), Unit(item.End, characters), Align(item.Align))
            {
                Name = item.Name ?? "",
                Font = item.FontIdx,
                Translation = translation,
                Table1 = table1,
                Table2 = table2
            };
        }

        // texty vlakov 3.39 pre stlpce predlohy; pre cislo a den plati prvy zaznam, preto maju zaznamy rozne dni a zaznam pre vsetky dni
        // ide posledny
        private List<TrainTextSet> ToTrainTexts(Dictionary<string, NumberDays> numbers)
        {
            var sets = new List<TrainTextSet>();

            foreach (var text in document.TableTexts)
            {
                var targets = text.Realizations
                    .SelectMany(realization => _catalogs.Where(catalog => ReferenceEquals(catalog.Source, realization.Table)
                                                                          && catalog.Catalog.Columns.Any(column => column.Key == realization.Item.Key))
                        .Select(catalog => new TextTarget(catalog.Catalog.Key, realization.Item.Key)))
                    .Distinct()
                    .ToList();
                if (targets.Count == 0)
                    continue;

                var trains = new List<TrainText>();
                foreach (var (number, data) in numbers)
                {
                    var records = new List<(string Text, int Font, BitArray Days)>();
                    var conflict = false;

                    for (var day = 0; day < data.All.Length; day++)
                    {
                        var texts = data.Running(day).SelectMany(train => text.Trains.Where(item => ReferenceEquals(item.Train, train)))
                            .Select(item => (Text: item.Text ?? "", Font: item.FontID)).Distinct().ToList();
                        if (texts.Count == 0)
                            continue;

                        conflict |= texts.Count > 1;
                        var index = records.FindIndex(record => record.Text == texts[0].Text && record.Font == texts[0].Font);
                        if (index < 0)
                        {
                            index = records.Count;
                            records.Add((texts[0].Text, texts[0].Font, new BitArray(data.All.Length)));
                        }

                        records[index].Days[day] = true;
                    }

                    if (conflict)
                        state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TableTextConflict, $"{text.Key}: {number}"));

                    trains.AddRange(records.Select(record => new TrainText(number, ValidityOf(record.Days, data), record.Text, record.Font))
                        .OrderBy(record => record.Validity.Length == 0));
                }

                sets.Add(new TrainTextSet(text.Key) { Targets = targets, Trains = trains });
            }

            return sets;
        }

        // tabulky TabTab, na ktore odkazuju prevedene predlohy; jednoduche pravidla idu do slovnika, udalosti (#SWITCH…) do vyrazu,
        // jednoduche pravidla s @ alebo %stlpec% ako komentar do vyrazu
        private List<TabTabTable> ToTabTabs(List<CatalogTable> catalogs)
        {
            var names = catalogs.SelectMany(catalog => catalog.Columns).SelectMany(column => new[] { column.Table1, column.Table2 })
                .Where(name => name.Length > 0).ToHashSet(StringComparer.Ordinal);
            var tracks = TrackKeys();
            var transpiler = new Iniss2TabTabTranspiler(source.TrainTypeKeys, name => tracks.GetValueOrDefault(name));

            return document.TabTabs.Where(table => names.Contains(table.Key)).Select(table => ToTabTab(table, transpiler)).ToList();
        }

        private TabTabTable ToTabTab(TableTabTab table, Iniss2TabTabTranspiler transpiler)
        {
            var section = TabTabSection.Parse(table.Text);
            var comparer = section.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var entries = new List<TabTabEntry>();
            var manual = new List<string>();

            foreach (var rule in section.Rules)
            {
                if (rule.Event != TabTabEventKind.None)
                    continue;

                var left = TabTabText.Decode(rule.Left);
                var right = TabTabText.Decode(rule.Right);
                if (Special(left.Text) || Special(right.Text))
                {
                    manual.Add($"{rule.Left.Trim()} = {rule.Right.Trim()}");
                    continue;
                }

                // INISS 3.39 hlada podla textu INISSu bez pisma a pri opakovani plati posledne pravidlo
                entries.RemoveAll(entry => comparer.Equals(entry.KeyText, right.Text));
                entries.Add(new TabTabEntry(right.Text, TabTabTable.AnyFont, left.Text, left.Font ?? TabTabTable.AnyFont));
            }

            // predlohy, ktore tabulku pouzivaju ako prvu - podla nich miesto vyhodnotenia a striedanie casti
            var uses = _catalogs.Where(catalog => catalog.Catalog.Columns.Any(column => column.Table1 == table.Key))
                .Select(catalog => new Iniss2TabTabUse(catalog.Catalog.Key, BoardColumnComposer.SiteOf(catalog.Source),
                    !SinglePartClasses.Contains(catalog.Source.Manufacturer)))
                .ToList();
            var problems = transpiler.Problems.Count;
            var expression = transpiler.Expression(section, uses);
            if (manual.Count > 0 || transpiler.Problems.Count > problems)
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TabTabNotConverted, table.Key));

            if (manual.Count > 0)
            {
                var lines = manual.SelectMany(rule => rule.Split('\n')).Select(line => "' " + line.TrimEnd('\r'));
                expression = string.Join('\n', lines) + "\n" + (expression.Length > 0 ? expression : "Return sOrig");
            }

            return new TabTabTable(table.Key) { IgnoreCase = section.IgnoreCase, ViewValues = section.ViewValues, Expression = expression, Entries = entries };
        }

        private IEnumerable<string> PeerNames(string table) =>
            table.Length > 0 && document.TabTabs.FirstOrDefault(tab => tab.Key == table) is { } tabTab
                ? Iniss2TabTabTranspiler.PeerNames(TabTabSection.Parse(tabTab.Text))
                : [];

        // povodny text @, text ineho stlpca a riadiace znaky sa do slovnika nedaju zapisat
        private static bool Special(string text) => text.Contains('@', StringComparison.Ordinal) || ColumnReference().IsMatch(text) || text.Any(char.IsControl);

        private static int Unit(int points, bool characters) => characters ? points / PointsPerCharacter : points;

        private static string Align(TableAlign? align)
        {
            if (align == TableAlign.Right)
                return "RIGHT";

            return align == TableAlign.Center ? "CENTER" : "LEFT";
        }

        // sposob prekladu a tabulky; server odmietne tabulku navyse aj chybajucu, INISS 3.39 nepotrebne ignoruje
        private (string Translation, string Table1, string Table2) Translation(TableCatalog source, TableItem item)
        {
            var table1 = TabTabKey(item.Tab1);
            var table2 = TabTabKey(item.Tab2);

            // tabulka, ktora v grafikone nie je, by zastavila start servera
            foreach (var name in new[] { table1, table2 }.Where(name => name.Length > 0 && !document.TabTabs.Any(table => table.Key == name)))
            {
                state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.TabTabMissing, $"{source.Key}: {item.Key}: {name}"));
                return (CatalogColumn.FreeTranslation, "", "");
            }

            if (item.DivType == TableDivType.TableTime && table1.Length > 0 && table2.Length > 0)
                return ("TABLETIME", table1, table2);
            if (item.DivType == TableDivType.Table && table1.Length > 0)
                return ("TABLE", table1, "");
            if (item.DivType == TableDivType.Translate && table1.Length > 0)
                return ("TRANSLATE", table1, "");
            if (item.DivType == TableDivType.Char && table1.Length > 0)
                return ("CHAR", table1, "");

            return (CatalogColumn.FreeTranslation, "", "");
        }

        private static string TabTabKey(TableTabTab? table) => table is null || table == TableTabTab.Empty ? "" : table.Key;
    }
}
