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
