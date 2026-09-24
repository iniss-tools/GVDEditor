using GVDEditor.Entities;

namespace GVDEditor.DocScreenshots;

/// <summary>
///     Informačné tabule ukážkového grafikonu: odchodová tabuľa v hale a dve nástupištné.
///     Hodnoty zodpovedajú pravidlám zo špecifikácie (TKatalog.txt, TPhysic.txt, TLogical.txt, TabTab.txt):
///     stĺpce zoradené podľa pozície, pri LCD1 hranice v násobkoch 8, v každom type zobrazenia všetkých šesť režimov.
/// </summary>
internal sealed class DemoTables
{
    public const string FontDir = @"C:\INISS\DATA\FONTS\";

    public List<TableFont> Fonts { get; } =
    [
        Font(16, "Tenké", TableFontType.None, 10, 8, "LCD8.fnt"),
        Font(17, "Tučné", TableFontType.Bold, 10, 8, "LCD8B.fnt"),
        Font(18, "Veľké", TableFontType.None, 16, 10, "LCD16.fnt"),
    ];

    public List<TableTabTab> TabTabs { get; } =
    [
        new() { Key = "Druh", Text = "R{17}=R\r\nREX{17}=REX\r\nEx{17}=Ex\r\nIC{17}=IC\r\nOs=Os" },
        new() { Key = "Smer", Text = "Odklon, \"ODKLON\" = #SWITCH" },
        new() { Key = "Meskanie", Text = "zpozdeni,\"+@ min\"=#SWITCH" },
    ];

    public List<TableCatalog> Catalogs { get; } = [];
    public List<TablePhysical> Physicals { get; } = [];
    public List<TableLogical> Logicals { get; } = [];
    public List<TableText> Texts { get; } = [];

    public DemoTables(IList<Train> trains, IList<Track> tracks)
    {
        var druh = TabTabs[0];
        var smer = TabTabs[1];
        var meskanie = TabTabs[2];

        // odchodová tabuľa LCD1: jeden riadok na vlak, šírka 528 bodov, stĺpce na hraniciach po 8 bodoch
        var departures = Catalog("Odchodová LCD1", "Odchodová tabuľa (8 vlakov)", TableManufacturer.LCD1, maxRecords: 8, lines: 8, width: 528);
        var dNothing = Item("Nic", "Prázdny riadok", TableFillSection.Free, 0, 0, 528, 16);
        var dText = Item("Text", "Text", TableFillSection.TextLine1, 0, 0, 528, 16);
        var dTime = Item("Cas", "Čas odchodu", TableFillSection.CasOdchodu, 0, 0, 40, 17);
        var dType = Item("Druh", "Druh vlaku", TableFillSection.TypVlaku, 0, 48, 80, 17, divType: TableDivType.Translate, tab1: druh);
        var dNumber = Item("Cislo", "Číslo vlaku", TableFillSection.CisloVlaku, 0, 88, 128, 16);
        var dName = Item("Nazov", "Názov vlaku", TableFillSection.NazovVlaku, 0, 136, 216, 16);
        var dTarget = Item("Smer", "Smer", TableFillSection.CielovaStanica, 0, 224, 384, 16, divType: TableDivType.Translate, tab1: smer);
        var dTrack = Item("Kolaj", "Koľaj", TableFillSection.KolajOdchod, 0, 392, 416, 17, TableAlign.Center);
        var dDelay = Item("Meskanie", "Meškanie", TableFillSection.MeskanieOdchod, 0, 424, 528, 16, divType: TableDivType.Translate, tab1: meskanie);
        departures.Items.AddRange([dNothing, dText, dTime, dType, dNumber, dName, dTarget, dTrack, dDelay]);
        departures.ViewTypeTabs.Add(ViewTab(TableViewType.Odchodova, 1, [dNothing], [dText],
            [dTime, dType, dNumber, dName, dTarget, dTrack], [dTime, dType, dNumber, dName, dTarget, dTrack, dDelay]));

        // nástupištná tabuľa ELEN16: dva riadky na vlak - vlak a cieľ, pod tým stanice na trase
        var platform = Catalog("Nástupištná ELEN16", "Nástupištná tabuľa", TableManufacturer.ELEN16, maxRecords: 1, lines: 2, width: 320);
        var pNothing = Item("Nic", "Prázdny riadok", TableFillSection.Free, 0, 0, 320, 16);
        var pText = Item("Text", "Text", TableFillSection.TextLine1, 0, 0, 320, 16);
        var pType = Item("Druh", "Druh vlaku", TableFillSection.TypVlaku, 0, 0, 36, 17, divType: TableDivType.Translate, tab1: druh);
        var pNumber = Item("Cislo", "Číslo vlaku", TableFillSection.CisloVlaku, 0, 40, 84, 16);
        var pTime = Item("Cas", "Čas odchodu", TableFillSection.CasOdchodu, 0, 88, 124, 17);
        var pTarget = Item("Ciel", "Cieľová stanica", TableFillSection.CielovaStanicaNastupiste, 0, 128, 320, 18);
        var pVia = Item("Cez", "Stanice na trase", TableFillSection.StaniceDoSmeruNastupiste, 1, 0, 320, 16);
        platform.Items.AddRange([pNothing, pText, pType, pNumber, pTime, pTarget, pVia]);
        platform.ViewTypeTabs.Add(ViewTab(TableViewType.Nastupistna, 2, [pNothing], [pText],
            [pType, pNumber, pTime, pTarget, pVia], [pType, pNumber, pTime, pTarget, pVia]));

        Catalogs.AddRange([departures, platform]);

        var hall = Physical("ODCH_HALA", "Odchodová tabuľa – hala", 1, 8, departures);
        var p1 = Physical("NAST_1", "Nástupište 1", 10, 1, platform);
        var p2 = Physical("NAST_2", "Nástupište 2", 11, 1, platform);
        Physicals.AddRange([hall, p1, p2]);

        var departuresLogical = Logical("Odchody", "Odchody – hala", TableViewType.Odchodova);
        for (var i = 0; i < hall.RecCount; i++)
            departuresLogical.Records.Add(Record(i, TableViewType.Odchodova, hall));
        var platform1 = Logical("Nastupiste1", "Nástupište 1", TableViewType.Nastupistna);
        platform1.Records.Add(Record(0, TableViewType.Nastupistna, p1));
        var platform2 = Logical("Nastupiste2", "Nástupište 2", TableViewType.Nastupistna);
        platform2.Records.Add(Record(0, TableViewType.Nastupistna, p2));
        Logicals.AddRange([departuresLogical, platform1, platform2]);

        // nástupištné tabule nad koľajami (Pozice_A.TXT)
        foreach (var track in tracks.Where(t => t != Track.None))
        {
            var table = track.Platform.Key == "1" ? platform1 : platform2;
            track.Tables.Add(table);
            track.TablePriorities[table.Key] = 0;
        }

        // vlastný text cieľovej stanice pre dva vlaky - ukážka okna Texty na tabuliach
        var target = new TableText { Key = "Ciel", Name = "Cieľová stanica", Comment = "" };
        target.Realizations.Add(new TableTextRealization { Table = departures, Item = dTarget });
        target.Realizations.Add(new TableTextRealization { Table = platform, Item = pTarget });
        target.Trains.Add(new TableTrain { Train = trains.First(t => t.Number == "3601"), Text = "N. Záhorie", FontID = -1 });
        target.Trains.Add(new TableTrain { Train = trains.First(t => t.Number == "521"), Text = "Hraničná – št. hranica", FontID = -1 });
        Texts.Add(target);
    }

    private static TableFont Font(int id, string name, TableFontType type, int size, int width, string file) => new()
    {
        FontID = id, Name = name, Type = type, Size = size, Width = width, FileName = file,
        IsDia = true, IsLower = true, IsUpper = true, IsNumber = true, IsSpecChars = true,
    };

    private static TableCatalog Catalog(string key, string name, TableManufacturer manufacturer, int maxRecords, int lines, int width)
    {
        var catalog = new TableCatalog
        {
            Key = key, Name = name, Comment = "", Manufacturer = manufacturer,
            MaxRecCount = maxRecords, MinHeight = 10, NumSegments = 1,
        };
        for (var i = 0; i < lines; i++)
            catalog.Segments.Add(new TableSegment { Height = 10, Width = width, Size = 15 });
        return catalog;
    }

    private static TableItem Item(string key, string name, TableFillSection fill, int line, int start, int end, int font,
        TableAlign? align = null, TableDivType? divType = null, TableTabTab? tab1 = null) => new()
    {
        Key = key, Name = name, FillSection = fill, Line = line, Start = start, End = end, FontIDX = font,
        Align = align ?? TableAlign.Left, DivType = divType ?? TableDivType.Free,
        Tab1 = tab1 ?? TableTabTab.Empty, Tab2 = TableTabTab.Empty,
    };

    /// <summary>
    ///     Typ zobrazenia so všetkými šiestimi režimami; meškajúci vlak na odchode ukazuje aj stĺpec meškania.
    /// </summary>
    private static TableViewTypeTab ViewTab(TableViewType type, int linesPerRecord, TableItem[] nothing, TableItem[] text,
        TableItem[] train, TableItem[] delayed)
    {
        var tab = new TableViewTypeTab { ViewType = type, CountLinesRecord = linesPerRecord.ToString(System.Globalization.CultureInfo.InvariantCulture) };
        (TableViewMode Mode, TableItem[] Items)[] modes =
        [
            (TableViewMode.Nothing, nothing),
            (TableViewMode.Vlak, train),
            (TableViewMode.VlakZmeskanyPrichod, train),
            (TableViewMode.VlakZmeskanyOdchod, delayed),
            (TableViewMode.VlakZmeskany, delayed),
            (TableViewMode.Text, text),
        ];
        foreach (var (mode, items) in modes)
            tab.TypeModeItems.Add(new TableTypeModeItem { ViewMode = mode, ItemsKeys = items.Select(i => i.Key).ToList() });
        return tab;
    }

    private static TablePhysical Physical(string key, string name, int id, int records, TableCatalog catalog) => new()
    {
        Key = key, Name = name, ID = id, CommunicationPort = 2, RecCount = records, TableCatalog = catalog,
        Comment = "", Rem = "", ReverseArrows = "", SaveXML = "",
    };

    private static TableLogical Logical(string key, string name, TableViewType type) => new()
    {
        Key = key, Name = name, ViewType = type, TypeViewFlags = "", IdStation = 0, Comment = "",
    };

    private static TableRecord Record(int position, TableViewType type, TablePhysical table)
    {
        var record = new TableRecord();
        record.Positions.Add(new TablePosition { Position = position, TypeView = type, Table = table });
        return record;
    }
}
