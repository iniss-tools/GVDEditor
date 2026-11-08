using System.Globalization;
using TableSimulator.Contracts.Boards;
using TableSimulator.Contracts.Faces;
using TableSimulator.Contracts.Station;

namespace GVDEditor.Integration;

/// <summary>Linky a tabule stanice pre simulator tabul (kontrakt <see cref="StationImportRequest" />).</summary>
internal static class SimulatorStation
{
    /// <summary>Vyrobca spinacej jednotky zosilnovaca v simulatore.</summary>
    private const string AmplifierManufacturer = "ELSVO";

    /// <summary>
    /// Linky presmerovane na simulator a tabule, ktore im INISS priradi; k tomu priecinok logov INISSu a tabule
    /// s exportom do XML pre virtualne tabule simulatora.
    /// </summary>
    /// <param name="map">linky konfiguracie s priradenymi tabulami</param>
    /// <param name="ports">sekcia Driver* → port linky na simulatore</param>
    /// <param name="tables">vsetky fyzicke tabule instalacie (export do XML maju aj tabule bez linky)</param>
    /// <param name="logFolder">priecinok logov INISSu (PathNames\LogPath), kam INISS zapisuje XML tabul</param>
    /// <param name="amplifiers">spinacie jednotky zosilnovacov ELSVO zo zvukovych okruhov - patria linke podla cisla</param>
    public static StationImportRequest From(DriverLineMap map, IReadOnlyDictionary<string, int> ports, IEnumerable<InissTable>? tables = null,
        string? logFolder = null, IReadOnlyList<ElsvoUnit>? amplifiers = null)
    {
        var lines = new List<StationLineDto>();
        var boards = new List<StationBoardDto>();
        foreach (var line in map.Lines)
        {
            if (line.Line is not { } number || !ports.TryGetValue(line.Section, out var port)) continue;
            lines.Add(new StationLineDto(number, line.Class, port, line.Section));
            foreach (var table in line.Tables)
                if (table.Table.Table.TableCatalog is { Manufacturer: { } manufacturer } catalog)
                    boards.Add(Board(number, table.Table, catalog, manufacturer));
            foreach (var unit in (amplifiers ?? []).Where(u => u.Line == number))
                boards.Add(new StationBoardDto(number, unit.Address, AmplifierManufacturer, unit.Name));
        }

        // INISS zapisuje XML vsetkych grafikonov z DirList - rovnake SAVE_XML zapisuje do jedneho suboru
        var xml = (tables ?? []).Where(t => !string.IsNullOrWhiteSpace(t.Table.SaveXML))
            .DistinctBy(t => t.Table.SaveXML.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(t => new StationXmlTableDto(t.Table.Key, t.Table.SaveXML.Trim()))
            .ToList();
        // popisy na celach tabul v jazyku GVDEditora
        var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "cs" ? StationFaceLanguage.Cs : StationFaceLanguage.Sk;
        return new StationImportRequest(lines, boards) { LogFolder = logFolder, XmlTables = xml, FaceLanguage = language };
    }

    /// <summary>
    /// Tabula s datami pre celo (druh, stlpce, kolaje); listova (ERS, FERS) aj s modulmi a zoznamami listov z predlohy
    /// a jej TabTab.
    /// </summary>
    private static StationBoardDto Board(int line, InissTable inissTable, TableCatalog catalog, TableManufacturer manufacturer)
    {
        var table = inissTable.Table;
        var board = new StationBoardDto(line, table.ID, manufacturer.Name, table.Key) { Face = Face(inissTable.Face) };
        if (!FlapLayouts.IsFlapBoard(manufacturer))
            return board;
        var layout = FlapLayouts.Build(catalog);
        return board with
        {
            Flaps = new FlapProfileDto
            {
                LinesPerRecord = layout.LinesPerRecord,
                Modules = layout.Modules.Select(m => new FlapModuleDto(m.Line, m.Position, m.Span, m.List)).ToArray(),
                Lists = layout.Lists
            },
            FlapRecords = layout.Records
        };
    }

    /// <summary>Co o cele tabule vedia data stanice (rovnako ako nacitanie instalacie INISSu v simulatore).</summary>
    private static StationFaceDto? Face(TableFaceInfo? info)
    {
        if (info is null)
            return null;
        var type = TableViewType.Parse(info.ViewType);
        var kind = type == TableViewType.Odchodova ? StationBoardKind.Departures
            : type == TableViewType.Prichodova ? StationBoardKind.Arrivals
            : type == TableViewType.Nastupistna ? StationBoardKind.Platform
            : type == TableViewType.Podchodova ? StationBoardKind.Underpass
            : StationBoardKind.Other;
        var columns = info.Columns.Select(c => new StationFaceColumnDto(c.Line, c.Start, c.End, c.Align switch
        {
            1 => FaceAlign.Right,
            2 => FaceAlign.Center,
            _ => FaceAlign.Left
        }, c.Content)).ToList();
        return new StationFaceDto(kind, info.LinesPerRecord, info.Records, info.Width, info.RowHeight, columns, info.Tracks, info.Platforms, info.Station);
    }
}
