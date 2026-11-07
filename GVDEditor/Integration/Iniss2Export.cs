using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using Serilog;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.StateDgm;
using ToolsCore.Iniss.Tools;
using ToolsCore.Iniss2.Grafikon;
using ToolsCore.Iniss2.Tables;
using Iniss2TablesFile = ToolsCore.Iniss2.Tables.TablesFile;

namespace GVDEditor.Integration;

/// <summary>
/// Co sa pri exporte do INISS2 nedalo preniest presne alebo co zastavilo export.
/// </summary>
internal enum Iniss2ExportIssue
{
    /// <summary>Stanica nema ciselne oznacenie; v trase vlaku chyba.</summary>
    StationWithoutNumber,

    /// <summary>Trasa vlaku neobsahuje stanicu grafikonu, ostane len vychodzia a konecna stanica.</summary>
    RouteWithoutStation,

    /// <summary>Datumova poznamka sa neda rozvinut; vlak v INISS2 nejde ziaden den.</summary>
    BadValidity,

    /// <summary>Obdobie vlaku ma viac ako 400 dni; server poznamku nerozvinie.</summary>
    PeriodTooLong,

    /// <summary>Vlak ma priznak D, server s nim nesparuje spravy GTN.</summary>
    LongDistanceFlag,

    /// <summary>Varianty vlaku idu v rovnaky den; server vezme jednu, v spolocne dni ostava ta s menej dnami.</summary>
    VariantOverlap,

    /// <summary>Vlaky s rovnakym cislom maju v jeden den rozne kolaje; server pozna pre cislo a den len jeden zaznam.</summary>
    SharedNumberTracks,

    /// <summary>Radenie ma cislo vlaku, ktory v stanici nejde; server ho zahodi.</summary>
    CompositionWithoutTrain,

    /// <summary>Radenie neplati v ziadny den vlaku, nikdy by sa neprehralo.</summary>
    CompositionOutsidePeriod,

    /// <summary>Vyluka alebo odklon obsluhy neplati v ziadny den vlaku v stanici.</summary>
    NoticeOutsidePeriod,

    /// <summary>V cielovej instalacii nie je LogText.vb, kluce hlaseni sa nedali overit.</summary>
    LogTextMissing,

    /// <summary>LogText nema hlasenie, na ktore kluc ukazuje; dodatok, vyluka alebo odklon sa neprehra.</summary>
    LogTextFunctionMissing,

    /// <summary>Vlak s rovnakym cislom v inej stanici sa nespojil - trasy si odporuju alebo sa dni nestretli.</summary>
    TrainNotMerged,

    /// <summary>Vlak ide v stanici v den mimo obdobia, ktore server stanici urci; v ten den neplati koliaj ani dodatky.</summary>
    StationDayOutsidePeriod,

    /// <summary>Stanica je medzi exportovanymi grafikonmi dvakrat.</summary>
    StationTwice,

    /// <summary>Druh tabule INISS2 nepozna; tabula sa neprevedie.</summary>
    TableUnsupported,

    /// <summary>Logicka tabula nema druh zobrazenia, ktory INISS2 pozna; jej cast neukazuje vlaky.</summary>
    TableViewUnknown,

    /// <summary>Obsah udaja tabule INISS2 nepozna; udaj ostane prazdny.</summary>
    ContentUnsupported,

    /// <summary>Udaj tabule sa prekryva s inym v tom istom riadku; server by zapis odmietol, udaj sa vynecha.</summary>
    ColumnOverlap,

    /// <summary>Tabulka TabTab ma pravidla, ktore sa nedaju previest; su ako komentar vo vyraze a treba ich prepisat vo VB.NET.</summary>
    TabTabNotConverted,

    /// <summary>Udaj odkazuje na tabulku TabTab, ktora v grafikone nie je; udaj sa neprekoduje.</summary>
    TabTabMissing,

    /// <summary>Tabulka TabTab stanice ma rovnaky nazov ako spolocna tabulka v cielovej instalacii; server sa nespusti.</summary>
    TabTabGlobalDuplicate,

    /// <summary>Vlaky s rovnakym cislom maju v jeden den rozne texty; server pozna pre cislo a den len jeden.</summary>
    TableTextConflict,

    /// <summary>Tabula so zariadenim nema linku; server vezme linku stanice alebo prvu s vhodnym protokolom, inak sa nespusti.</summary>
    TableWithoutLine,

    /// <summary>Linka INISS 3.39 nema v INISS2 obdobu (druh tabul alebo port); jej tabule ostanu bez linky.</summary>
    LineUnsupported,

    /// <summary>Predloha ELEN nema banku pisem; server sa bez nej nespusti.</summary>
    FontBankMissing,

    /// <summary>Stanica nema stavovy diagram ani sablonu; server sa bez neho nespusti.</summary>
    StateDiagramMissing,

    /// <summary>StateDgm.txt sa neda nacitat; stanica dostane diagram sablony.</summary>
    StateDiagramUnreadable,

    /// <summary>Vyraz stavoveho diagramu sa nedal prelozit; jeho cast sa vynecha.</summary>
    StateExpressionNotConverted,

    /// <summary>Cast stavoveho diagramu (akcia, starter, casovy bod) INISS2 nevie vyjadrit; vynecha sa.</summary>
    StatePartNotConverted,

    /// <summary>Akcie, ktore vedu do stavu, maju rozne hlasenia; uzol INISS2 ma jedno.</summary>
    StateReportConflict,

    /// <summary>Typ hlasenia 3.39 nema hlasenie v cieli; stav ostane bez hlasenia.</summary>
    StateReportMissing,

    /// <summary>Server INISS2 na tomto pocitaci bezi; subory by pri dalsej uprave prepisal.</summary>
    ServerRunning,

    /// <summary>Grafikon v cielovej instalacii uz ma vlaky.</summary>
    GroupExists
}

/// <summary>
/// Upozornenie exportu.
/// </summary>
/// <param name="Issue">druh</param>
/// <param name="Detail">cislo vlaku alebo nazov stanice</param>
internal sealed record Iniss2ExportWarning(Iniss2ExportIssue Issue, string Detail);

/// <summary>
/// Export sa nedal vykonat.
/// </summary>
/// <param name="issue">dovod</param>
/// <param name="detail">nazov stanice alebo grafikonu</param>
internal sealed class Iniss2ExportException(Iniss2ExportIssue issue, string detail) : Exception(detail)
{
    /// <summary>
    /// Dovod.
    /// </summary>
    public Iniss2ExportIssue Issue { get; } = issue;
}

/// <summary>
/// Subory jednej stanice v tvare INISS2.
/// </summary>
/// <param name="Station">cislo stanice</param>
/// <param name="Tracks">kolaje stanice</param>
/// <param name="Positions">kolaje vlakov</param>
/// <param name="MotorTrains">motorove vlaky</param>
/// <param name="Additions">dodatky vlakov</param>
/// <param name="Compositions">radenie vlakov</param>
/// <param name="Languages">jazyky hlaseni vlakov</param>
/// <param name="ReportKeys">hlasenia stanice s pismenami</param>
/// <param name="Tables">tabule stanice</param>
/// <param name="Catalogs">katalogove predlohy tabul</param>
/// <param name="TabTabs">tabulky TabTab stanice</param>
/// <param name="Texts">texty vlakov pre tabule</param>
internal sealed record Iniss2StationData(
    int Station,
    IReadOnlyList<TrackDefinition> Tracks,
    IReadOnlyList<TrainPosition> Positions,
    IReadOnlyList<MosTrain> MotorTrains,
    IReadOnlyList<TrainAddition> Additions,
    IReadOnlyList<TrainComposition> Compositions,
    IReadOnlyList<TrainLanguages> Languages,
    IReadOnlyList<ReportKey> ReportKeys,
    IReadOnlyList<TableDefinition> Tables,
    IReadOnlyList<CatalogTable> Catalogs,
    IReadOnlyList<TabTabTable> TabTabs,
    IReadOnlyList<TrainTextSet> Texts)
{
    /// <summary>Stavovy diagram prevedeny zo StateDgm.txt; null = grafikon ho nema, stanica dostane diagram sablony.</summary>
    public StateDiagram? StateDiagram { get; init; }
}

/// <summary>
/// Grafikon INISS2 s jednou alebo viacerymi stanicami.
/// </summary>
/// <param name="Timetable">vlaky s trasami a casmi vo vsetkych staniciach</param>
/// <param name="RouteStations">stanice tras</param>
/// <param name="Stations">subory stanic v poradi exportovanych grafikonov</param>
/// <param name="Lockouts">vyluky vlakov</param>
/// <param name="Diversions">odklony vlakov</param>
/// <param name="UserReports">texty vyluk a odklonov zalozene obsluhou; zaporne cisla sa pri zapise zmenia na cisla v ConstReports.xml</param>
/// <param name="Lines">linky tabul na doplnenie do CommLines.xml</param>
/// <param name="FontFiles">banky pisem ELEN na skopirovanie do Data\TableFonts</param>
/// <param name="Warnings">co sa nedalo preniest presne</param>
/// <param name="TemplateStation">stanica cielovej instalacie, z ktorej sa skopiruje statedgm.xml staniciam bez vlastneho diagramu; prazdne = bez kopie</param>
internal sealed record Iniss2ExportData(
    GvFile Timetable,
    IReadOnlyList<GvStation> RouteStations,
    IReadOnlyList<Iniss2StationData> Stations,
    IReadOnlyList<TrainLockout> Lockouts,
    IReadOnlyList<TrainDiversion> Diversions,
    IReadOnlyList<ConstReport> UserReports,
    IReadOnlyList<CommLineDefinition> Lines,
    IReadOnlyList<Iniss2FontFile> FontFiles,
    IReadOnlyList<Iniss2ExportWarning> Warnings,
    string TemplateStation = "");

/// <summary>
/// Grafikon jednej stanice INISS 3.39 na export.
/// </summary>
/// <param name="Document">grafikon</param>
/// <param name="Gvd">hlavicka grafikonu</param>
internal sealed record Iniss2StationInput(GrafikonDocument Document, GVDInfo Gvd)
{
    /// <summary>Priecinok grafikonu v DirList.TXT - podla neho sa hladaju linky tabul.</summary>
    public string Grafikon { get; init; } = "";

    /// <summary>Stavovy diagram grafikonu; null = grafikon ho nema.</summary>
    public StateDgmDiagram? StateDgm { get; init; }
}

/// <summary>
/// Linka fyzickej tabule INISS 3.39 v tvare INISS2.
/// </summary>
/// <param name="Grafikon">priecinok grafikonu v DirList.TXT</param>
/// <param name="TableKey">kluc fyzickej tabule</param>
/// <param name="Line">linka</param>
internal sealed record Iniss2TableLine(string Grafikon, string TableKey, CommLineDefinition Line);

/// <summary>
/// Banka pisem ELEN na skopirovanie.
/// </summary>
/// <param name="Name">nazov banky ulozeny v subore; je to aj meno suboru v Data\TableFonts</param>
/// <param name="Source">subor .bin</param>
internal sealed record Iniss2FontFile(string Name, string Source);

/// <summary>
/// Udaje instalacie INISS 3.39 mimo grafikonu, ktore export potrebuje.
/// </summary>
/// <param name="Languages">jazyky instalacie; vlak so vsetkymi sa hlasi vo vsetkych jazykoch banky</param>
/// <param name="UserTexts">texty zalozene obsluhou (LogZvuk.usr)</param>
/// <param name="Lockouts">vyluky obsluhy (Vyluky1.txt)</param>
/// <param name="Diversions">odklony obsluhy (Odklony1.txt)</param>
internal sealed record Iniss2ExportSource(
    IReadOnlyList<FyzLanguage> Languages,
    IReadOnlyList<LogZvukText> UserTexts,
    IReadOnlyList<TrainNotice> Lockouts,
    IReadOnlyList<TrainNotice> Diversions)
{
    /// <summary>Linky tabul zo zistenia INISSu 3.39, pozri <see cref="Iniss2Export.TableLines" />.</summary>
    public IReadOnlyList<Iniss2TableLine> TableLines { get; init; } = [];

    /// <summary>Banky pisem ELEN: kluc katalogovej tabule 3.39 → subor .bin.</summary>
    public IReadOnlyDictionary<string, string> FontBanks { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Stanica cielovej instalacie, z ktorej sa prevezmu hlasenia ReportKeys.xml a statedgm.xml pre stanice bez StateDgm.txt;
    /// prazdne = hlasenia z typov 3.39.
    /// </summary>
    public string TemplateStation { get; init; } = "";

    /// <summary>Hlasenia sablony; null = hlasenia z typov hlaseni 3.39.</summary>
    public IReadOnlyList<ReportKey>? ReportTemplate { get; init; }

    /// <summary>
    /// Pismeno hlasenia INISS2 pre typ hlasenia 3.39 podla kluca typu; typ, ktory tu nie je, ma svoj znak, prazdne pismeno = dodatky
    /// a radenie sa pri nom neprehraju.
    /// </summary>
    public IReadOnlyDictionary<string, string> ReportLetters { get; init; } = new Dictionary<string, string>();

    /// <summary>Kluce druhov vlakov z TrTypes s indexom zabudovaneho druhu INISSu - pre preklad podmienok TabTab.</summary>
    public IReadOnlyDictionary<string, int> TrainTypeKeys { get; init; } = new Dictionary<string, int>();

    /// <summary>
    /// Udaje nacitanej instalacie; vyluky a odklony obsluhy sa citaju zo suborov.
    /// </summary>
    /// <param name="workspace">instalacia</param>
    public static Iniss2ExportSource FromWorkspace(InissWorkspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);

        var keys = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var type in workspace.TrainsTypes ?? [])
        {
            var index = ExprTrainTypes.IndexOf(type.CategoryTrain);
            if (index >= 0 && !string.IsNullOrEmpty(type.Key))
                keys.TryAdd(type.Key, index);
        }

        return new Iniss2ExportSource(workspace.Languages, workspace.LogZvukTexts,
            Formats.TrainNoticesFile.Read(workspace.DataDir, Formats.TrainNoticesFile.FileLockouts),
            Formats.TrainNoticesFile.Read(workspace.DataDir, Formats.TrainNoticesFile.FileDiversions)) { TrainTypeKeys = keys };
    }
}

/// <summary>
/// Jednorazovy prevod grafikonov INISS 3.39 do suborov INISS2. Grafikon 3.39 pozna vlak len z pohladu jednej stanice, casy ma
/// preto len ona, ostatne body trasy su bez casu. Server cas hlada len v zaujmovych bodoch, inde mu nechyba. Grafikony viacerych
/// stanic idu do jedneho grafikonu INISS2 a vlak s rovnakym cislom sa v nich spoji do jedneho vlaku s casmi vo vsetkych staniciach.
/// </summary>
internal static partial class Iniss2Export
{
    private const string MotorType = "MOs";
    private const string PassengerType = "Os";

    // kluc dodatku je nazov hlasenia v LogText; INISS2 ho ma ako INISS 3.39 nahravku D<kod> v skupine DODATKY
    private const string AdditionPrefix = "D";

    // typy hlasenia, pri ktorych sa dodatok ani radenie neprehra
    private const string NoReports = "-";

    private const string LogTextFile = "LogText.vb";

    /// <summary>Stavovy diagram stanice; prevedie sa zo StateDgm.txt, inak sa kopiruje zo sablony.</summary>
    public const string StateDiagramFile = ToolsCore.Iniss2.Grafikon.StateDiagramFile.FileName;

    // mutex, ktorym server brani druhemu spusteniu
    private const string ServerMutex = "INISRV_MUTEX";
    private const string FontsDirectory = "TableFonts";
    private const char LockoutKind = 'V';
    private const char DiversionKind = 'O';

    /// <summary>
    /// Prevedie grafikon jednej stanice. Nic nezapisuje.
    /// </summary>
    /// <param name="document">grafikon</param>
    /// <param name="gvd">hlavicka grafikonu</param>
    /// <param name="source">udaje instalacie mimo grafikonu</param>
    /// <exception cref="Iniss2ExportException">stanica grafikonu nema cislo</exception>
    public static Iniss2ExportData Build(GrafikonDocument document, GVDInfo gvd, Iniss2ExportSource source) =>
        Build([new Iniss2StationInput(document, gvd)], source);

    /// <summary>
    /// Prevedie grafikony viacerych stanic do jedneho grafikonu. Nic nezapisuje.
    /// </summary>
    /// <param name="stations">grafikony stanic</param>
    /// <param name="source">udaje instalacie mimo grafikonu</param>
    /// <exception cref="Iniss2ExportException">stanica nema cislo alebo je dvakrat</exception>
    public static Iniss2ExportData Build(IReadOnlyList<Iniss2StationInput> stations, Iniss2ExportSource source)
    {
        ArgumentNullException.ThrowIfNull(stations);
        ArgumentNullException.ThrowIfNull(source);

        var state = new ExportState();
        var builders = stations.Select(input => new StationBuilder(input.Document, input.Gvd, input.Grafikon, input.StateDgm, source, state)).ToList();
        var rows = builders.SelectMany(builder => builder.Prepare()).ToList();

        var twice = builders.GroupBy(builder => builder.Station).FirstOrDefault(group => group.Count() > 1);
        if (twice is not null)
            throw new Iniss2ExportException(Iniss2ExportIssue.StationTwice, twice.Key.ToString(CultureInfo.InvariantCulture));

        var timetable = new GvFile([.. builders.Select(builder => builder.Station)], new TrainMerger(state).Merge(rows));
        var files = builders.Select(builder => builder.Files(timetable)).ToList();

        // vyluka alebo odklon obsluhy mimo dni vlaku v jednej stanici moze platit v inej
        var exported = builders.SelectMany(builder => builder.ExportedNotices).ToHashSet();
        foreach (var notice in builders.SelectMany(builder => builder.OutsideNotices).Distinct().Where(notice => !exported.Contains(notice)))
            state.Warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.NoticeOutsidePeriod, notice.Number));

        return new Iniss2ExportData(timetable, [.. state.Stations.Values], files, [.. builders.SelectMany(builder => builder.Lockouts).Distinct()],
            [.. builders.SelectMany(builder => builder.Diversions).Distinct()], [.. state.UserReports.Select(user => user.Report)],
            [.. state.Lines.Values], [.. state.FontFiles.Select(font => new Iniss2FontFile(font.Key, font.Value))], [.. state.Warnings.Distinct()],
            source.TemplateStation);
    }

    /// <summary>
    /// Linky tabul INISS 3.39 v tvare INISS2 podla nastavenia INISSu (Driver, TablePort, TablePortParam).
    /// </summary>
    /// <param name="map">tabule priradene linkam</param>
    /// <param name="serialParameters">parametre serioveho portu podla sekcie Driver (TablePortParam); null = predvolene</param>
    /// <param name="stationName">nazov stanice - cast kluca linky</param>
    public static (List<Iniss2TableLine> Lines, List<Iniss2ExportWarning> Warnings) TableLines(DriverLineMap map, Func<string, string?> serialParameters,
        string stationName)
    {
        ArgumentNullException.ThrowIfNull(map);
        ArgumentNullException.ThrowIfNull(serialParameters);

        var lines = new List<Iniss2TableLine>();
        var warnings = new List<Iniss2ExportWarning>();

        foreach (var line in map.Lines.Where(line => line.Active && line.Tables.Count > 0))
        {
            var definition = LineDefinition(serialParameters, line, stationName);
            if (definition is null)
            {
                warnings.Add(new Iniss2ExportWarning(Iniss2ExportIssue.LineUnsupported, $"{line.Section}: {line.Port}"));
                continue;
            }

            lines.AddRange(line.Tables.Select(table => new Iniss2TableLine(table.Table.Grafikon, table.Table.Table.Key, definition)));
        }

        return (lines, warnings);
    }

    // protokol podla triedy linky a spojenie podla TablePort; ITP funguje v INISS2 len cez UDP
    private static CommLineDefinition? LineDefinition(Func<string, string?> serialParameters, DriverLine line, string stationName)
    {
        var protocol = line.Class switch
        {
            1 or 3 or 5 => "LCD",
            4 => "ELEN",
            130 => "ITP",
            _ => null
        };

        var port = line.Port.Contains('=', StringComparison.Ordinal) ? line.Port[(line.Port.IndexOf('=', StringComparison.Ordinal) + 1)..] : line.Port;
        var (type, address) = port.ToUpperInvariant() switch
        {
            var value when value.StartsWith("COM", StringComparison.Ordinal) => (CommLineType.Serial, port),
            var value when value.StartsWith("TCP://", StringComparison.Ordinal) => (CommLineType.TcpClient, port[6..]),
            var value when value.StartsWith("UDP://", StringComparison.Ordinal) => (CommLineType.UdpClient, port[6..]),
            _ => (CommLineType.None, "")
        };

        if (protocol is null || type == CommLineType.None || protocol == "ITP" && type != CommLineType.UdpClient)
            return null;

        var parameters = type == CommLineType.Serial ? serialParameters(line.Section) ?? "" : "";
        var number = (line.Line ?? 0).ToString(CultureInfo.InvariantCulture);
        return new CommLineDefinition($"{stationName} - linka {number}", type, protocol, address) { Parameters = parameters };
    }

    /// <summary>
    /// Overi, ze LogText cielovej instalacie ma hlasenia, na ktore ukazuju textove kluce dodatkov, vyluk a odklonov, a vzory
    /// textov obsluhy. Hlasenie bez spravneho oznacenia server nepouzije.
    /// </summary>
    /// <param name="dataDir">priecinok dat servera</param>
    /// <param name="data">prevedeny grafikon</param>
    public static List<Iniss2ExportWarning> CheckLogText(string dataDir, Iniss2ExportData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var file = Path.Combine(dataDir, LogTextFile);
        if (!File.Exists(file))
            return [new Iniss2ExportWarning(Iniss2ExportIssue.LogTextMissing, LogTextFile)];

        var functions = LogTextFunctions(File.ReadAllLines(file, Encodings.Win1250));
        var warnings = new List<Iniss2ExportWarning>();

        void Require(string name, string attribute)
        {
            if (functions.TryGetValue(name, out var attributes) && attributes.Contains(attribute))
                return;

            var warning = new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, name);
            if (!warnings.Contains(warning))
                warnings.Add(warning);
        }

        foreach (var addition in data.Stations.SelectMany(station => station.Additions).Where(addition => !IsReportNumber(addition.Key)))
            Require(addition.Key, "Dodatek");
        foreach (var lockout in data.Lockouts.Where(lockout => !IsReportNumber(lockout.Key)))
            Require(lockout.Key, "Vyluka");
        foreach (var diversion in data.Diversions.Where(diversion => !IsReportNumber(diversion.Key)))
            Require(diversion.Key, "Odklon");

        // hlasenie stavu je metoda LogText s rovnakym nazvom; druh hlasenia urcuje atribut, preto sa overuje len nazov
        foreach (var report in data.Stations.Where(station => station.StateDiagram is not null)
                     .SelectMany(station => station.StateDiagram!.Graphs.SelectMany(graph => graph.Nodes)).Select(node => node.Report)
                     .Where(report => report.Length > 0).Distinct(StringComparer.Ordinal))
        {
            var warning = new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, report);
            if (!functions.ContainsKey(report) && !warnings.Contains(warning))
                warnings.Add(warning);
        }

        foreach (var report in data.UserReports)
        {
            var key = report.Key.ToString(CultureInfo.InvariantCulture);
            if (data.Lockouts.Any(lockout => lockout.Key == key))
                Require(report.Function, "VzorVyluky");
            if (data.Diversions.Any(diversion => diversion.Key == key))
                Require(report.Function, "VzorOdklonu");
        }

        return warnings;
    }

    /// <summary>
    /// Server INISS2 na tomto pocitaci bezi. Pri zapise by subory pri dalsej uprave prepisal.
    /// </summary>
    public static bool IsServerRunning()
    {
        if (!Mutex.TryOpenExisting(ServerMutex, out var mutex))
            return false;

        mutex.Dispose();
        return true;
    }

    /// <summary>
    /// Overi, ze tabulky TabTab stanic sa nevolaju rovnako ako spolocne tabulky cielovej instalacie. Server pouzije prvy
    /// TabTabGlobal.xml v poradi grafikonov a pri zhode nazvu sa nespusti.
    /// </summary>
    /// <param name="dataDir">priecinok dat servera</param>
    /// <param name="data">prevedeny grafikon</param>
    public static List<Iniss2ExportWarning> CheckTabTabGlobal(string dataDir, Iniss2ExportData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var global = DirListXml.Read(dataDir).Select(group => Path.Combine(dataDir, group.Name, TabTabXml.GlobalFileName)).FirstOrDefault(File.Exists);
        if (global is null)
            return [];

        var names = TabTabXml.Read(global).Select(table => table.Name).ToHashSet(StringComparer.Ordinal);
        return data.Stations.SelectMany(station => station.TabTabs).Where(table => names.Contains(table.Name))
            .Select(table => new Iniss2ExportWarning(Iniss2ExportIssue.TabTabGlobalDuplicate, table.Name)).Distinct().ToList();
    }

    private static bool IsReportNumber(string key) => int.TryParse(key, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _);

    // funkcie LogText a oznacenia nad nimi; zakomentovane riadky sa preskocia, VB nerozlisuje velkost pismen
    internal static Dictionary<string, HashSet<string>> LogTextFunctions(IEnumerable<string> lines)
    {
        var functions = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var attributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in lines.Select(line => line.Trim()))
        {
            if (line.Length == 0 || line.StartsWith('\''))
                continue;

            foreach (Match attribute in AttributePattern().Matches(line))
                attributes.Add(attribute.Groups[1].Value);

            var function = FunctionPattern().Match(line);
            if (!function.Success)
                continue;

            functions.TryAdd(function.Groups[1].Value, attributes);
            attributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        return functions;
    }

    [GeneratedRegex(@"<(\w+)\(")]
    private static partial Regex AttributePattern();

    [GeneratedRegex(@"\b(?:Sub|Function)\s+([\p{L}\p{N}_]+)\s*\(")]
    private static partial Regex FunctionPattern();

    /// <summary>
    /// Zapise grafikon jednej stanice - pozri <see cref="Write(Iniss2ExportData, string, string, IReadOnlyList{string})" />.
    /// </summary>
    public static void Write(Iniss2ExportData data, string dataDir, string group, string stationDirectory) =>
        Write(data, dataDir, group, [stationDirectory]);

    /// <summary>
    /// Zapise grafikon do instalacie INISS2 ako novy grafikon. Zapis je transakcny. Server musi byt zastaveny, inak
    /// subory pri dalsej uprave prepise.
    /// </summary>
    /// <param name="data">prevedeny grafikon</param>
    /// <param name="dataDir">priecinok dat servera</param>
    /// <param name="group">nazov priecinka grafikonu</param>
    /// <param name="stationDirectories">nazvy priecinkov stanic v poradi <see cref="Iniss2ExportData.Stations" /></param>
    /// <exception cref="Iniss2ExportException">grafikon uz ma vlaky</exception>
    public static void Write(Iniss2ExportData data, string dataDir, string group, IReadOnlyList<string> stationDirectories)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(stationDirectories);
        if (stationDirectories.Count != data.Stations.Count)
            throw new ArgumentException("Pocet priecinkov nezodpoveda poctu stanic.", nameof(stationDirectories));

        var groupDir = Path.Combine(dataDir, group);

        if (File.Exists(Path.Combine(groupDir, GvFile.FileName)))
            throw new Iniss2ExportException(Iniss2ExportIssue.GroupExists, group);

        // texty obsluhy dostanu cisla v spolocnom zozname instalacie; rovnaky text, aky uz v nom je, sa neprida
        List<ConstReport>? reports = null;
        var keys = new Dictionary<string, string>();
        if (data.UserReports.Count > 0)
        {
            (reports, var numbers) = ConstReportsFile.Merge(ConstReportsFile.Read(dataDir), data.UserReports);
            for (var i = 0; i < numbers.Count; i++)
                keys[data.UserReports[i].Key.ToString(CultureInfo.InvariantCulture)] = numbers[i].ToString(CultureInfo.InvariantCulture);
        }

        var lockouts = data.Lockouts.Select(lockout => keys.TryGetValue(lockout.Key, out var key) ? lockout with { Key = key } : lockout).ToList();
        var diversions = data.Diversions.Select(diversion => keys.TryGetValue(diversion.Key, out var key) ? diversion with { Key = key } : diversion).ToList();

        List<string> files =
        [
            Path.Combine(dataDir, DirListXml.FileName),
            Path.Combine(groupDir, GvFile.FileName),
            Path.Combine(groupDir, GvStationsFile.FileName),
            Path.Combine(groupDir, VylukyFile.FileName),
            Path.Combine(groupDir, OdklonyFile.FileName),
            Path.Combine(dataDir, ConstReportsFile.FileName),
            Path.Combine(dataDir, CommLinesXml.FileName)
        ];
        var fonts = data.FontFiles.Select(font => (Target: Path.Combine(dataDir, FontsDirectory, font.Name + ".bin"), font.Source))
            .Where(font => !File.Exists(font.Target)).ToList();
        files.AddRange(fonts.Select(font => font.Target));
        foreach (var directory in stationDirectories)
        {
            var stationDir = Path.Combine(groupDir, directory);
            files.AddRange([
                Path.Combine(stationDir, DirListXml.StationNumberFile),
                Path.Combine(stationDir, PositionsDefFile.FileName),
                Path.Combine(stationDir, PositionsFile.FileName),
                Path.Combine(stationDir, MosFile.FileName),
                Path.Combine(stationDir, DodatkyFile.FileName),
                Path.Combine(stationDir, RazeniFile.FileName),
                Path.Combine(stationDir, ForeignFile.FileName),
                Path.Combine(stationDir, ReportKeysFile.FileName),
                Path.Combine(stationDir, Iniss2TablesFile.FileName),
                Path.Combine(stationDir, TableCatalogFile.FileName),
                Path.Combine(stationDir, TabTabXml.StationFileName),
                Path.Combine(stationDir, TrainTextsFile.FileName),
                Path.Combine(stationDir, StateDiagramFile)
            ]);
        }

        var transaction = new FileTransaction(files);

        try
        {
            data.Timetable.Write(groupDir);
            GvStationsFile.Write(groupDir, data.RouteStations);
            VylukyFile.Write(groupDir, lockouts);
            OdklonyFile.Write(groupDir, diversions);
            if (reports is not null)
                ConstReportsFile.Write(dataDir, reports);
            CommLinesXml.Add(dataDir, data.Lines);
            foreach (var (target, fontSource) in fonts)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(fontSource, target);
            }

            for (var i = 0; i < data.Stations.Count; i++)
                WriteStation(data.Stations[i], dataDir, group, stationDirectories[i], data.TemplateStation);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            Log.Error(e, "Export do INISS2 {Directory} zlyhal - subory sa vracaju do povodneho stavu", groupDir);
            transaction.TryRollback();
            throw;
        }

        transaction.Commit();
        Log.Information("Export do INISS2: grafikon {Group}, stanic {Stations}, vlakov {Trains}", group, data.Stations.Count, data.Timetable.Trains.Count);
    }

    private static void WriteStation(Iniss2StationData station, string dataDir, string group, string directory, string template)
    {
        var stationDir = Path.Combine(dataDir, group, directory);
        var diagram = Path.Combine(template, StateDiagramFile);
        if (station.StateDiagram is not null)
        {
            Directory.CreateDirectory(stationDir);
            ToolsCore.Iniss2.Grafikon.StateDiagramFile.Write(stationDir, station.StateDiagram);
        }
        else if (template.Length > 0 && File.Exists(diagram))
        {
            Directory.CreateDirectory(stationDir);
            File.Copy(diagram, Path.Combine(stationDir, StateDiagramFile), true);
        }

        DirListXml.WriteStationNumber(stationDir, station.Station);
        PositionsDefFile.Write(stationDir, station.Tracks);
        PositionsFile.Write(stationDir, station.Positions);
        MosFile.Write(stationDir, station.MotorTrains);
        DodatkyFile.Write(stationDir, station.Additions);
        RazeniFile.Write(stationDir, station.Compositions);
        ForeignFile.Write(stationDir, station.Languages);
        ReportKeysFile.Write(stationDir, station.ReportKeys);
        Iniss2TablesFile.Write(stationDir, station.Tables);
        TableCatalogFile.Write(stationDir, station.Catalogs);
        TabTabXml.Write(Path.Combine(stationDir, TabTabXml.StationFileName), station.TabTabs);
        TrainTextsFile.Write(stationDir, station.Texts);
        DirListXml.AddStation(dataDir, group, new DirListItem(directory, station.Station, "", "", DirListItemFlags.None));
    }

    /// <summary>
    /// Spolocne udaje exportu vsetkych stanic.
    /// </summary>
    private sealed class ExportState
    {
        /// <summary>Texty obsluhy a ich hlasenia s docasnym zapornym cislom.</summary>
        public List<(LogZvukText Text, ConstReport Report)> UserReports { get; } = [];

        /// <summary>Stanice tras podla cisla.</summary>
        public Dictionary<int, GvStation> Stations { get; } = [];

        /// <summary>Upozornenia.</summary>
        public List<Iniss2ExportWarning> Warnings { get; } = [];

        /// <summary>Linky tabul podla kluca.</summary>
        public Dictionary<string, CommLineDefinition> Lines { get; } = [];

        /// <summary>Banky pisem ELEN: nazov → subor .bin.</summary>
        public Dictionary<string, string> FontFiles { get; } = [];
    }
}
