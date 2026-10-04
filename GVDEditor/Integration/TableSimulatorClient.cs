using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GVDEditor.Integration;

/// <summary>Linka stanice pre simulator tabul.</summary>
/// <param name="Number">cislo linky</param>
/// <param name="TableClass">trieda linky</param>
/// <param name="Port">port TCP, na ktory INISS po presmerovani posiela</param>
/// <param name="Name">nazov pre obsluhu (sekcia Driver*)</param>
internal sealed record SimulatorStationLine(
    [property: JsonPropertyName("number")] int Number,
    [property: JsonPropertyName("tableClass")] int TableClass,
    [property: JsonPropertyName("port")] int Port,
    [property: JsonPropertyName("name")] string Name);

/// <summary>Fyzicka tabula stanice pre simulator.</summary>
/// <param name="Line">cislo linky</param>
/// <param name="Address">adresa (ID z TPhysic)</param>
/// <param name="Manufacturer">kluc vyrobcu z katalogovej predlohy</param>
/// <param name="Name">nazov tabule (KEY)</param>
internal sealed record SimulatorStationBoard(
    [property: JsonPropertyName("line")] int Line,
    [property: JsonPropertyName("address")] int Address,
    [property: JsonPropertyName("manufacturer")] string Manufacturer,
    [property: JsonPropertyName("name")] string Name);

/// <summary>Linky a tabule stanice, ktore ma simulator mat (POST /api/station).</summary>
internal sealed record SimulatorStation(
    [property: JsonPropertyName("lines")] IReadOnlyList<SimulatorStationLine> Lines,
    [property: JsonPropertyName("boards")] IReadOnlyList<SimulatorStationBoard> Boards)
{
    /// <summary>
    /// Linky presmerovane na simulator a tabule, ktore im INISS priradi.
    /// </summary>
    /// <param name="map">linky konfiguracie s priradenymi tabulami</param>
    /// <param name="ports">sekcia Driver* → port linky na simulatore</param>
    public static SimulatorStation From(DriverLineMap map, IReadOnlyDictionary<string, int> ports)
    {
        var lines = new List<SimulatorStationLine>();
        var boards = new List<SimulatorStationBoard>();
        foreach (var line in map.Lines)
        {
            if (line.Line is not { } number || !ports.TryGetValue(line.Section, out var port)) continue;
            lines.Add(new SimulatorStationLine(number, line.Class, port, line.Section));
            foreach (var table in line.Tables)
                if (table.Table.Table.TableCatalog?.Manufacturer is { } manufacturer)
                    boards.Add(new SimulatorStationBoard(number, table.Table.Table.ID, manufacturer.Name, table.Table.Table.Key));
        }

        return new SimulatorStation(lines, boards);
    }
}

/// <summary>Co simulator pri zalozeni stanice zmenil.</summary>
internal sealed record SimulatorStationResult(
    [property: JsonPropertyName("linesAdded")] int LinesAdded,
    [property: JsonPropertyName("linesUpdated")] int LinesUpdated,
    [property: JsonPropertyName("boardsAdded")] int BoardsAdded,
    [property: JsonPropertyName("boardsKept")] int BoardsKept,
    [property: JsonPropertyName("unsupportedLines")] IReadOnlyList<int> UnsupportedLines,
    [property: JsonPropertyName("skippedBoards")] IReadOnlyList<string> SkippedBoards);

/// <summary>Stav simulatora - z neho len triedy liniek, ktore pozna.</summary>
internal sealed record SimulatorState([property: JsonPropertyName("supportedClasses")] IReadOnlyList<int> SupportedClasses);

/// <summary>
/// Volania weboveho API simulatora tabul (TableSimulator): ci bezi a ake triedy liniek pozna, zalozenie liniek a tabul
/// stanice.
/// </summary>
internal static class TableSimulatorClient
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>Webova adresa simulatora z textu (<c>http://host:port</c>); null pri neplatnej.</summary>
    public static Uri? ParseUrl(string text) =>
        Uri.TryCreate(text.Trim(), UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) ? uri : null;

    /// <summary>Stav simulatora; null, ak na adrese neodpoveda.</summary>
    public static async Task<SimulatorState?> StateAsync(Uri baseUri, TimeSpan timeout, CancellationToken cancel)
    {
        using var limit = CancellationTokenSource.CreateLinkedTokenSource(cancel);
        limit.CancelAfter(timeout);
        try
        {
            return await Http.GetFromJsonAsync<SimulatorState>(new Uri(baseUri, "api/state"), limit.Token);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException or OperationCanceledException && !cancel.IsCancellationRequested)
        {
            return null;
        }
    }

    /// <summary>Zalozi v simulatore linky a tabule stanice.</summary>
    /// <exception cref="HttpRequestException">simulator neodpoveda alebo poziadavku odmietol</exception>
    public static async Task<SimulatorStationResult> ImportAsync(Uri baseUri, SimulatorStation station, CancellationToken cancel)
    {
        using var response = await Http.PostAsJsonAsync(new Uri(baseUri, "api/station"), station, cancel);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SimulatorStationResult>(cancel)
               ?? throw new HttpRequestException("Empty response.");
    }
}
