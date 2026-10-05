using System.Net.Http.Json;
using System.Text.Json;
using TableSimulator.Contracts.State;
using TableSimulator.Contracts.Station;

namespace GVDEditor.Integration;

/// <summary>Linky a tabule stanice pre simulator tabul (kontrakt <see cref="StationImportRequest" />).</summary>
internal static class SimulatorStation
{
    /// <summary>
    /// Linky presmerovane na simulator a tabule, ktore im INISS priradi.
    /// </summary>
    /// <param name="map">linky konfiguracie s priradenymi tabulami</param>
    /// <param name="ports">sekcia Driver* → port linky na simulatore</param>
    public static StationImportRequest From(DriverLineMap map, IReadOnlyDictionary<string, int> ports)
    {
        var lines = new List<StationLineDto>();
        var boards = new List<StationBoardDto>();
        foreach (var line in map.Lines)
        {
            if (line.Line is not { } number || !ports.TryGetValue(line.Section, out var port)) continue;
            lines.Add(new StationLineDto(number, line.Class, port, line.Section));
            foreach (var table in line.Tables)
                if (table.Table.Table.TableCatalog?.Manufacturer is { } manufacturer)
                    boards.Add(new StationBoardDto(number, table.Table.Table.ID, manufacturer.Name, table.Table.Table.Key));
        }

        return new StationImportRequest(lines, boards);
    }
}

/// <summary>
/// Volania weboveho API simulatora tabul (TableSimulator) s jeho kontraktmi: ci bezi a ake triedy liniek pozna,
/// zalozenie liniek a tabul stanice.
/// </summary>
internal static class TableSimulatorClient
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>Webova adresa simulatora z textu (<c>http://host:port</c>); null pri neplatnej.</summary>
    public static Uri? ParseUrl(string text) =>
        Uri.TryCreate(text.Trim(), UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) ? uri : null;

    /// <summary>Stav simulatora; null, ak na adrese neodpoveda.</summary>
    public static async Task<StateDto?> StateAsync(Uri baseUri, TimeSpan timeout, CancellationToken cancel)
    {
        using var limit = CancellationTokenSource.CreateLinkedTokenSource(cancel);
        limit.CancelAfter(timeout);
        try
        {
            return await Http.GetFromJsonAsync<StateDto>(new Uri(baseUri, "api/state"), limit.Token);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException or OperationCanceledException && !cancel.IsCancellationRequested)
        {
            return null;
        }
    }

    /// <summary>Zalozi v simulatore linky a tabule stanice.</summary>
    /// <exception cref="HttpRequestException">simulator neodpoveda alebo poziadavku odmietol</exception>
    public static async Task<StationImportedDto> ImportAsync(Uri baseUri, StationImportRequest station, CancellationToken cancel)
    {
        using var response = await Http.PostAsJsonAsync(new Uri(baseUri, "api/station"), station, cancel);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StationImportedDto>(cancel)
               ?? throw new HttpRequestException("Empty response.");
    }
}
