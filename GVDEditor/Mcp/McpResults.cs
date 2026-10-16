namespace GVDEditor.Mcp;

/// <summary>Otvoreny grafikon (get_grafikon).</summary>
internal sealed record GrafikonSummary(
    [property: Description("INISS installation folder; empty = none chosen.")] string InissFolder,
    [property: Description("Folder of the open timetable; null = no timetable open.")] string? Folder,
    [property: Description("Station of the timetable.")] string? Station,
    [property: Description("First day the timetable is valid.")] DateOnly? ValidFrom,
    [property: Description("Last day the timetable is valid.")] DateOnly? ValidTo,
    [property: Description("Number of trains (with variants).")] int Trains);

/// <summary>Grafikon instalacie (list_grafikons).</summary>
internal sealed record GrafikonFolder(
    [property: Description("Timetable folder name.")] string Folder,
    [property: Description("Station; null = the folder has no readable timetable header.")] string? Station,
    [property: Description("First day the timetable is valid.")] DateOnly? ValidFrom,
    [property: Description("Last day the timetable is valid.")] DateOnly? ValidTo,
    [property: Description("Number of trains in the saved timetable.")] int? Trains,
    [property: Description("The timetable is open in GVDEditor.")] bool Open);

/// <summary>Grafikony instalacie.</summary>
internal sealed record GrafikonList(
    [property: Description("INISS installation folder; empty = none chosen.")] string InissFolder,
    IReadOnlyList<GrafikonFolder> Grafikons);

/// <summary>Koľaj stanice.</summary>
internal sealed record TrackInfo(
    [property: Description("Track key used in train records.")] string Key,
    [property: Description("Track name.")] string Name,
    [property: Description("Platform key.")] string? Platform,
    [property: Description("Logical tables (keys) showing trains on this track.")] IReadOnlyList<string> Tables);

/// <summary>Stanica otvoreneho grafikonu (get_station).</summary>
internal sealed record StationInfo(
    [property: Description("Station of the timetable.")] string? Station,
    IReadOnlyList<TrackInfo> Tracks,
    [property: Description("Platforms: key - name.")] IReadOnlyList<string> Platforms,
    [property: Description("Train operators: id - name.")] IReadOnlyList<string> Operators,
    [property: Description("Train types: key - text on boards.")] IReadOnlyList<string> TrainTypes,
    [property: Description("Stations added by this timetable (not in the sound bank).")] IReadOnlyList<string> CustomStations,
    [property: Description("Languages of announcements in this timetable.")] IReadOnlyList<string> Languages);

/// <summary>Vlak v zozname (list_trains).</summary>
internal sealed record TrainRow(
    [property: Description("Train id used by get_train.")] int Id,
    [property: Description("Train number.")] string Number,
    [property: Description("Variant number; 0 = the only or base variant.")] int Variant,
    [property: Description("Train type key, e.g. Os, R, Ex.")] string? Type,
    [property: Description("Train name.")] string Name,
    [property: Description("Routing: originates, passes through or terminates here.")] string? Routing,
    [property: Description("Origin station.")] string? From,
    [property: Description("Destination station.")] string? To,
    [property: Description("Arrival time; null = originates here.")] TimeOnly? Arrival,
    [property: Description("Departure time; null = terminates here.")] TimeOnly? Departure,
    [property: Description("Track key.")] string? Track,
    [property: Description("Operator name.")] string? Operator,
    [property: Description("Calendar restriction text; empty = runs daily in its validity.")] string Validity);

/// <summary>Vysledok list_trains.</summary>
internal sealed record TrainList(
    [property: Description("Number of trains matching the filter.")] int Total,
    IReadOnlyList<TrainRow> Trains);

/// <summary>Hlasenie (dodatok) vlaku.</summary>
internal sealed record TrainSupplement(
    [property: Description("Supplement name.")] string Name,
    [property: Description("Sound of the supplement.")] string? Sound,
    [property: Description("Announcement types it is used in.")] IReadOnlyList<string> Reports);

/// <summary>Radenie vlaku.</summary>
internal sealed record TrainFormation(
    [property: Description("Text of the formation.")] string Text,
    [property: Description("Destination station of the coaches.")] string? Destination,
    [property: Description("Train number of the coaches.")] string Number,
    [property: Description("Validity of the formation; null = whole validity of the train.")] string? Validity,
    [property: Description("Announcement types it is used in.")] IReadOnlyList<string> Reports);

/// <summary>Vlak so vsetkymi udajmi (get_train).</summary>
internal sealed record TrainDetail(
    TrainRow Train,
    [property: Description("First day of validity.")] DateOnly ValidFrom,
    [property: Description("Last day of validity.")] DateOnly ValidTo,
    [property: Description("Stations before this station (where the train comes from).")] IReadOnlyList<string> StationsBefore,
    [property: Description("Stations after this station (where the train goes).")] IReadOnlyList<string> StationsAfter,
    [property: Description("Departure track when different from the arrival track.")] string? TrackDeparture,
    [property: Description("Line of arrival.")] string? LineArrival,
    [property: Description("Line of departure.")] string? LineDeparture,
    [property: Description("Languages of announcements for this train.")] IReadOnlyList<string> Languages,
    [property: Description("Set flags: international, longDistance, extraordinary, reservation, sleeperOnly, lowFloor, transfer, flagO, diesel.")]
    IReadOnlyList<string> Flags,
    [property: Description("Lockout number; 0 = none.")] int Lockout,
    IReadOnlyList<TrainSupplement> Supplements,
    IReadOnlyList<TrainFormation> Formations);

/// <summary>Rozsah dni.</summary>
internal sealed record DayRange(DateOnly From, DateOnly To);

/// <summary>Vyklad textu platnosti (explain_validity).</summary>
internal sealed record ValidityDays(
    [property: Description("The text as GVDEditor writes it.")] string Normalized,
    [property: Description("Number of days the train runs.")] int Days,
    [property: Description("Days the train runs as continuous ranges.")] IReadOnlyList<DayRange> Ranges);

/// <summary>Fyzicka tabula.</summary>
internal sealed record PhysicalTableInfo(int Id, string Key, string Name,
    [property: Description("Communication port (line) of the board.")] int Port,
    [property: Description("Catalog board (type) key.")] string? Catalog);

/// <summary>Logicka tabula.</summary>
internal sealed record LogicalTableInfo(string Key, string Name,
    [property: Description("View type.")] string? View,
    [property: Description("Physical boards (keys) by record and position.")] IReadOnlyList<string> Physicals);

/// <summary>Katalogova tabula.</summary>
internal sealed record CatalogTableInfo(string Key, string Name,
    [property: Description("Manufacturer.")] string? Manufacturer,
    [property: Description("Number of segments.")] int Segments);

/// <summary>Text do tabule.</summary>
internal sealed record TableTextInfo(string Key, string Name,
    [property: Description("Number of realizations on catalog boards.")] int Realizations,
    [property: Description("Number of trains with their own text.")] int Trains);

/// <summary>Tabule otvoreneho grafikonu (list_tables).</summary>
internal sealed record TableList(
    IReadOnlyList<PhysicalTableInfo> Physical,
    IReadOnlyList<LogicalTableInfo> Logical,
    IReadOnlyList<CatalogTableInfo> Catalog,
    IReadOnlyList<TableTextInfo> Texts,
    [property: Description("Board fonts: id - name.")] IReadOnlyList<string> Fonts,
    [property: Description("TabTab sections (keys); text by get_tabtab.")] IReadOnlyList<string> TabTabs);

/// <summary>Hlasenie kontroly sekcie TabTab.</summary>
internal sealed record TabTabDiagnosticInfo(
    [property: Description("error (INISS rejects the rule or logs it), warning or info.")] string Severity,
    [property: Description("Line in the section, from 1.")] int Line,
    [property: Description("Check code.")] string Code,
    string Message,
    [property: Description("Suggested fix; null = none.")] string? Suggestion);

/// <summary>Cislo pisma pouzite v sekcii.</summary>
internal sealed record FontCodeInfo(
    [property: Description("Font number as written in {n}.")] int Code,
    [property: Description("Meaning on ELEN boards: colour, blinking, tall digits, face, extended font.")] string Elen,
    [property: Description("Font of the timetable font list with this number (other manufacturers); null = none.")] string? FontList);

/// <summary>Stlpec katalogovej tabule, ktory sekciu pouziva.</summary>
internal sealed record TabTabUsage(
    [property: Description("Catalog board key.")] string Catalog,
    [property: Description("Column (item) name.")] string Column,
    [property: Description("TAB1 or TAB2.")] string Table,
    [property: Description("DIVTYPE 0-4 of the column - how the section is applied (see tabtab_reference); null = not set.")] int? DivType,
    [property: Description("Physical boards (keys) of this catalog type.")] IReadOnlyList<string> Boards);

/// <summary>Sekcia TabTab (get_tabtab).</summary>
internal sealed record TabTabInfo(string Key,
    [property: Description("Program text of the section (INISS board language, see tabtab_reference).")] string Text,
    [property: Description("All findings of the GVDEditor check with line numbers (the same check as the TabTab editor and the analysis).")]
    IReadOnlyList<TabTabDiagnosticInfo> Diagnostics,
    [property: Description("Font numbers {n} used in the section, decoded.")] IReadOnlyList<FontCodeInfo> Fonts,
    [property: Description("Board columns using this section; empty = no catalog board refers to it.")] IReadOnlyList<TabTabUsage> UsedBy);

/// <summary>Problem z analyzy.</summary>
internal sealed record ProblemInfo(
    [property: Description("hint, warning or error.")] string Severity,
    [property: Description("What is wrong.")] string Text,
    [property: Description("How to fix it.")] string Solution,
    [property: Description("auto, semiAuto or manual - how the Analysis window in GVDEditor can fix it.")] string Fix);

/// <summary>Vysledok analyzy (analyze_grafikon).</summary>
internal sealed record AnalysisResult(
    [property: Description("Number of errors.")] int Errors,
    [property: Description("Number of warnings.")] int Warnings,
    IReadOnlyList<ProblemInfo> Problems);

/// <summary>Krok skladania textu stlpca.</summary>
internal sealed record ComposeStepInfo(
    [property: Description("Source in the fixed order (#VYLUKA … #SWITCH, #MERGE, TTexts, own value, DIVTYPE rules).")] string Source,
    string Text,
    [property: Description("Font number; null = unchanged.")] int? Font,
    [property: Description("Which item applied, whether the text was kept for @ …")] string Note);

/// <summary>Vysledny text stlpca tabule pre vlak (preview_board_text).</summary>
internal sealed record BoardColumnText(
    [property: Description("Catalog board key.")] string Catalog,
    [property: Description("Column (item) name.")] string Column,
    [property: Description("DIVTYPE 0-4.")] int DivType,
    [property: Description("TAB1 section key; null = none.")] string? Tab1,
    [property: Description("TAB2 section key; null = none.")] string? Tab2,
    [property: Description("The value the column computes itself.")] string Own,
    [property: Description("Text sent to the board; # separates alternating parts.")] string Text,
    [property: Description("Font number of the result; null = column font.")] int? Font,
    [property: Description("Evaluation error (e.g. division by zero); null = none.")] string? Error,
    IReadOnlyList<ComposeStepInfo> Steps);

/// <summary>Vysledok preview_board_text.</summary>
internal sealed record BoardPreview(
    [property: Description("The train the texts are composed for.")] TrainRow Train,
    IReadOnlyList<BoardColumnText> Columns);

/// <summary>Zmena grafikonu agentom - spolocna cast vysledku nastrojov so zmenami.</summary>
internal sealed record ChangeInfo(
    [property: Description("Dry run only - the timetable stays as it was.")] bool DryRun,
    [property: Description("The timetable is (or would be) different; false = it already was in that state.")] bool Changed,
    [property: Description("Undo step the user sees in GVDEditor (Edit > Undo); null = no step. The change is not saved - the user saves the timetable.")]
    string? UndoStep);

/// <summary>Zmena textu vlaku na tabuli.</summary>
internal sealed record TrainTextChange(
    [property: Description("Train id.")] int Train,
    [property: Description("Train type, number and name.")] string Label,
    [property: Description("Text before; null = the train had no text.")] string? Before,
    [property: Description("Text after (# = line break); null = the text was removed.")] string? After);

/// <summary>Texty jedneho typu textu na tabuliach po generovani.</summary>
internal sealed record BoardTextResult(
    [property: Description("Board text type key.")] string Key,
    [property: Description("Board text type name.")] string Name,
    [property: Description("Catalog board and column the texts are generated for (catalog/column); null = no realization in a column showing the destination "
                           + "or stations on the way - the type was skipped.")] string? Column,
    [property: Description("Trains that got a text.")] int Added,
    [property: Description("Trains whose text or font changed (the font is reset to the default).")] int Changed,
    [property: Description("Texts removed (the train is no longer in the timetable).")] int Removed,
    [property: Description("First changes in train order (at most 20).")] IReadOnlyList<TrainTextChange> Changes);

/// <summary>Vysledok regenerate_board_texts.</summary>
internal sealed record RegenerateTextsResult(
    ChangeInfo Change,
    IReadOnlyList<BoardTextResult> Texts);

/// <summary>Stlpec katalogovej tabule (get_table).</summary>
internal sealed record ColumnInfo(
    [property: Description("Column key; board texts and the column order refer to it.")] string Key,
    string Name,
    [property: Description("Content number (FILL_SECTION, see table_options).")] int Content,
    [property: Description("Content name.")] string ContentName,
    [property: Description("Row of the board, from 0.")] int Line,
    [property: Description("Start in points.")] int Start,
    [property: Description("End in points.")] int End,
    [property: Description("Font number.")] int Font,
    [property: Description("Alignment number: 0 left, 1 right, 2 centre.")] int Align,
    [property: Description("DIVTYPE 0-4 - how TAB1/TAB2 apply.")] int DivType,
    [property: Description("TAB1 section key; null = none.")] string? Tab1,
    [property: Description("TAB2 section key; null = none.")] string? Tab2,
    [property: Description("Board texts (keys) shown in this column; such a column cannot be removed.")] IReadOnlyList<string> Texts);

/// <summary>Rezim typu zobrazenia.</summary>
internal sealed record ViewModeInfo(
    [property: Description("View mode key.")] string Mode,
    [property: Description("Column keys shown in this mode, in order.")] IReadOnlyList<string> Columns);

/// <summary>Poradie stlpcov pre typ zobrazenia.</summary>
internal sealed record ColumnOrderInfo(
    [property: Description("View type key.")] string ViewType,
    [property: Description("Board rows per record.")] int LinesPerRecord,
    IReadOnlyList<ViewModeInfo> Modes);

/// <summary>Riadok katalogovej tabule.</summary>
internal sealed record RowInfo(int Height, int Width, int Size);

/// <summary>Katalogova tabula - predloha vzhladu tabule (get_table).</summary>
internal sealed record CatalogTableDetail(string Key, string Name, string Comment,
    [property: Description("Manufacturer key - protocol of the board.")] string? Manufacturer,
    [property: Description("Most records the board can show.")] int MaxRecords,
    [property: Description("Minimal row height in points.")] int MinHeight,
    [property: Description("Number of segments (NUM_SEGMENTS).")] int Segments,
    [property: Description("Row sizes; INISS reads but does not use them.")] IReadOnlyList<RowInfo> Rows,
    IReadOnlyList<ColumnInfo> Columns,
    [property: Description("Columns shown per view type and mode.")] IReadOnlyList<ColumnOrderInfo> ColumnOrder);

/// <summary>Fyzicka tabula - konkretna tabula na stanici (get_table).</summary>
internal sealed record PhysicalTableDetail(string Key, string Name, string Comment,
    [property: Description("Catalog board key - the type of the board.")] string? Catalog,
    [property: Description("Address on the communication line; -1 = none.")] int Address,
    [property: Description("Communication line number; 0 = assigned automatically.")] int Line,
    [property: Description("Number of records the board shows.")] int Records,
    [property: Description("Records with reversed arrows; usually empty.")] string ReverseArrows,
    [property: Description("File name of the XML export; empty = no export.")] string XmlFile,
    [property: Description("Note for the operator (REM).")] string Note);

/// <summary>Riadok zostavy logickej tabule.</summary>
internal sealed record LayoutRowInfo(
    [property: Description("Physical board key.")] string Physical,
    [property: Description("First record shown there, from 1.")] int FirstRecord,
    [property: Description("Last record shown there.")] int LastRecord,
    [property: Description("Row of the physical board for the first record, from 1.")] int StartRow,
    [property: Description("View type key on that board.")] string? ViewType);

/// <summary>Logicka tabula - co sa posiela na ktore fyzicke tabule (get_table).</summary>
internal sealed record LogicalTableDetail(string Key, string Name, string Comment,
    [property: Description("View type key.")] string? ViewType,
    [property: Description("Number of records (trains) the board shows.")] int Records,
    [property: Description("Station number (IDSTATION); 0 = none.")] int Station,
    [property: Description("Where the records go, as rows of physical boards; null = the placement cannot be written as rows (see placements).")]
    IReadOnlyList<LayoutRowInfo>? Layout,
    [property: Description("Placement of each record when layout is null: physical board, row from 0 and view type.")] IReadOnlyList<string>? Placements);

/// <summary>Pismo tabul (get_table).</summary>
internal sealed record FontDetail(
    [property: Description("Font number; columns, texts and TabTab {n} refer to it.")] int Id,
    string Name,
    [property: Description("regular, bold, italic, boldItalic or special; null = not set.")] string? Type,
    [property: Description("Height in points.")] int Size,
    [property: Description("Character width in points.")] int Width,
    bool Proportional,
    bool Diacritics,
    bool Lowercase,
    bool Uppercase,
    bool Digits,
    bool SpecialChars,
    bool SpecialAssignment,
    [property: Description("Font file; empty = none.")] string File,
    [property: Description("Meaning of the number on ELEN boards: colour, face, blinking.")] string Elen);

/// <summary>Tabula alebo pismo s pouzitim a kontrolou (get_table).</summary>
internal sealed record TableDetail(
    [property: Description("catalog, physical, logical or font - the one filled below.")] string Kind,
    CatalogTableDetail? Catalog,
    PhysicalTableDetail? Physical,
    LogicalTableDetail? Logical,
    FontDetail? Font,
    [property: Description("Where it is used; a used item cannot be deleted.")] IReadOnlyList<string> UsedBy,
    [property: Description("Errors of the GVDEditor check (INISS would reject them), in the language of the editor.")] IReadOnlyList<string> Problems,
    [property: Description("Warnings INISS tolerates, in the language of the editor.")] IReadOnlyList<string> Warnings);

/// <summary>Vysledok zmeny tabule alebo pisma.</summary>
internal sealed record TableChangeResult(
    ChangeInfo Change,
    [property: Description("The item after the change; null = deleted.")] TableDetail? Table,
    [property: Description("What the change did besides, or left to do (e.g. TabTab sections still using an old font number).")] IReadOnlyList<string> Notes);

/// <summary>Vysledok set_track_tables.</summary>
internal sealed record TrackChangeResult(ChangeInfo Change, TrackInfo Track);

/// <summary>Hodnota na vyber.</summary>
internal sealed record OptionInfo(
    [property: Description("Value to use in the tools.")] string Value,
    [property: Description("Name in the editor.")] string Name,
    [property: Description("More information; null = none.")] string? Note);

/// <summary>Hodnoty tabul a pisiem (table_options).</summary>
internal sealed record TableOptions(
    [property: Description("Manufacturers (protocols) of catalog boards; note = description and address range.")] IReadOnlyList<OptionInfo> Manufacturers,
    [property: Description("Column contents (FILL_SECTION).")] IReadOnlyList<OptionInfo> Contents,
    [property: Description("Column alignments.")] IReadOnlyList<OptionInfo> Aligns,
    [property: Description("Column DIVTYPE - how TAB1/TAB2 apply.")] IReadOnlyList<OptionInfo> DivTypes,
    [property: Description("View types of boards.")] IReadOnlyList<OptionInfo> ViewTypes,
    [property: Description("View modes; every view type of a catalog board lists columns for all of them.")] IReadOnlyList<OptionInfo> ViewModes,
    [property: Description("Font types.")] IReadOnlyList<OptionInfo> FontTypes);
