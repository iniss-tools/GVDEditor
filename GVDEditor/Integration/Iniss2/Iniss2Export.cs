using System.Globalization;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using Serilog;
using ToolsCore.Iniss.Tools;
using ToolsCore.Iniss2.Grafikon;
using ToolsCore.Iniss2.Tables;
using Iniss2TablesFile = ToolsCore.Iniss2.Tables.TablesFile;

namespace GVDEditor.Integration.Iniss2;

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

    /// <summary>Stavovy diagram stanice; prevedie sa zo StateDgm.txt, inak sa kopiruje zo sablony.</summary>
    public const string StateDiagramFile = ToolsCore.Iniss2.Grafikon.StateDiagramFile.FileName;

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
            source.TemplateStation)
        {
            DefaultStations = DefaultStations(builders.SelectMany(builder => builder.ReportedToStations()), state.Stations),
            AdditionFunctions = [.. state.AdditionFunctions.Values]
        };
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

        if (LogTextFile.ReadFunctions(dataDir) is not { } functions)
            return [new Iniss2ExportWarning(Iniss2ExportIssue.LogTextMissing, LogTextFile.FileName)];

        var warnings = new List<Iniss2ExportWarning>();

        void Require(string name, string attribute)
        {
            if (functions.TryGetValue(name, out var attributes) && attributes.Contains(attribute))
                return;

            var warning = new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionMissing, name);
            if (!warnings.Contains(warning))
                warnings.Add(warning);
        }

        // chybajucu funkciu dodatku zapis doplni; funkciu s rovnakym nazvom bez oznacenia nie
        foreach (var addition in data.Stations.SelectMany(station => station.Additions).Where(addition => !IsReportNumber(addition.Key)))
        {
            var added = new Iniss2ExportWarning(Iniss2ExportIssue.LogTextFunctionAdded, addition.Key);
            if (!functions.ContainsKey(addition.Key) && data.AdditionFunctions.Any(function => function.Name == addition.Key))
            {
                if (!warnings.Contains(added))
                    warnings.Add(added);
            }
            else
            {
                Require(addition.Key, "Dodatek");
            }
        }
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
    /// Overi, ze tabulky TabTab stanic sa nevolaju rovnako ako spolocne tabulky cielovej instalacie. Server pouzije prvy
    /// TabTabGlobal.xml v poradi grafikonov a pri zhode nazvu sa nespusti.
    /// </summary>
    /// <param name="dataDir">priecinok dat servera</param>
    /// <param name="data">prevedeny grafikon</param>
    public static List<Iniss2ExportWarning> CheckTabTabGlobal(string dataDir, Iniss2ExportData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var names = TabTabXml.ReadGlobal(dataDir).Select(table => table.Name).ToHashSet(StringComparer.Ordinal);
        return data.Stations.SelectMany(station => station.TabTabs).Where(table => names.Contains(table.Name))
            .Select(table => new Iniss2ExportWarning(Iniss2ExportIssue.TabTabGlobalDuplicate, table.Name)).Distinct().ToList();
    }

    private static bool IsReportNumber(string key) => int.TryParse(key, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _);

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
        var logText = Path.Combine(dataDir, LogTextFile.FileName);

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
            Path.Combine(groupDir, DefaultStationsFile.FileName),
            Path.Combine(dataDir, ConstReportsFile.FileName),
            Path.Combine(dataDir, CommLinesXml.FileName),
            logText
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
                Path.Combine(stationDir, TrainReportStationsFile.FileName),
                Path.Combine(stationDir, ReportPatternsFile.ShortFileName),
                Path.Combine(stationDir, ReportPatternsFile.LongFileName),
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
            if (data.DefaultStations.Count > 0)
                DefaultStationsFile.Write(groupDir, data.DefaultStations);
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

            if (File.Exists(logText) && data.AdditionFunctions.Count > 0)
            {
                var added = AddAdditionFunctions(logText, data.AdditionFunctions);
                if (added.Count > 0)
                    Log.Information("Export do INISS2: do {File} doplnene funkcie dodatkov {Functions}", logText, added);
            }
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or FormatException)
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
        TrainReportStationsFile.Write(stationDir, station.ReportStations);
        if (station.ShortReportPatterns.Count > 0)
            ReportPatternsFile.Write(stationDir, ReportPatternsFile.ShortFileName, station.ShortReportPatterns);
        if (station.LongReportPatterns.Count > 0)
            ReportPatternsFile.Write(stationDir, ReportPatternsFile.LongFileName, station.LongReportPatterns);
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

        /// <summary>Funkcie dodatkov pre LogText.vb podla kluca dodatku.</summary>
        public Dictionary<string, LogTextAddition> AdditionFunctions { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
