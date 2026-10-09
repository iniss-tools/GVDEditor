using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;
using TableSimulator.Contracts.Auth;
using TableSimulator.Contracts.Boards;
using TableSimulator.Contracts.State;
using TableSimulator.Contracts.Station;

namespace GVDEditor.Integration;

/// <summary>Linky a tabule stanice pre simulator tabul (kontrakt <see cref="StationImportRequest" />).</summary>
internal static class SimulatorStation
{
    /// <summary>
    /// Linky presmerovane na simulator a tabule, ktore im INISS priradi; k tomu priecinok logov INISSu a tabule
    /// s exportom do XML pre virtualne tabule simulatora.
    /// </summary>
    /// <param name="map">linky konfiguracie s priradenymi tabulami</param>
    /// <param name="ports">sekcia Driver* → port linky na simulatore</param>
    /// <param name="tables">vsetky fyzicke tabule instalacie (export do XML maju aj tabule bez linky)</param>
    /// <param name="logFolder">priecinok logov INISSu (PathNames\LogPath), kam INISS zapisuje XML tabul</param>
    public static StationImportRequest From(DriverLineMap map, IReadOnlyDictionary<string, int> ports, IEnumerable<InissTable>? tables = null,
        string? logFolder = null)
    {
        var lines = new List<StationLineDto>();
        var boards = new List<StationBoardDto>();
        foreach (var line in map.Lines)
        {
            if (line.Line is not { } number || !ports.TryGetValue(line.Section, out var port)) continue;
            lines.Add(new StationLineDto(number, line.Class, port, line.Section));
            foreach (var table in line.Tables)
                if (table.Table.Table.TableCatalog is { Manufacturer: { } manufacturer } catalog)
                    boards.Add(Board(number, table.Table.Table, catalog, manufacturer));
        }

        // INISS zapisuje XML vsetkych grafikonov z DirList - rovnake SAVE_XML zapisuje do jedneho suboru
        var xml = (tables ?? []).Where(t => !string.IsNullOrWhiteSpace(t.Table.SaveXML))
            .DistinctBy(t => t.Table.SaveXML.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(t => new StationXmlTableDto(t.Table.Key, t.Table.SaveXML.Trim()))
            .ToList();
        return new StationImportRequest(lines, boards) { LogFolder = logFolder, XmlTables = xml };
    }

    /// <summary>Tabula; listova (ERS, FERS) aj s modulmi a zoznamami listov z predlohy a jej TabTab.</summary>
    private static StationBoardDto Board(int line, TablePhysical table, TableCatalog catalog, TableManufacturer manufacturer)
    {
        var board = new StationBoardDto(line, table.ID, manufacturer.Name, table.Key);
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
}

/// <summary>Ako GVDEditor so simulatorom pracovat moze.</summary>
internal enum SimulatorAccess
{
    /// <summary>Simulator na adrese neodpoveda.</summary>
    Offline,

    /// <summary>Simulator vyzaduje prihlasenie a API kluc chyba alebo ho neprijal.</summary>
    Unauthorized,

    /// <summary>Kluc smie len pozerat - linky a tabule nezalozi.</summary>
    ReadOnly,

    /// <summary>Linky a tabule sa daju zalozit.</summary>
    Ok,

    /// <summary>Simulator zo siete prijima len HTTPS - adresa sa ma zmenit na <see cref="SimulatorProbe.HttpsUrl" />.</summary>
    HttpsRequired,

    /// <summary>Certifikatu simulatora tento pocitac nedoveruje (certifikacna autorita simulatora nie je nainstalovana).</summary>
    UntrustedCertificate
}

/// <summary>Zistenie stavu simulatora.</summary>
/// <param name="Access">co smie GVDEditor robit</param>
/// <param name="State">stav simulatora (triedy liniek); null, ak sa neda zistit</param>
/// <param name="HttpsUrl">adresa HTTPS simulatora, ak pristup cez HTTP presmeroval</param>
internal sealed record SimulatorProbe(SimulatorAccess Access, StateDto? State, Uri? HttpsUrl = null);

/// <summary>Simulator poziadavku presmeroval na HTTPS - zo siete neprijima HTTP.</summary>
/// <param name="httpsUrl">adresa simulatora cez HTTPS (bez cesty)</param>
internal sealed class SimulatorHttpsRequiredException(Uri httpsUrl)
    : HttpRequestException($"The simulator accepts only HTTPS from the network: {httpsUrl}")
{
    public Uri HttpsUrl { get; } = httpsUrl;
}

/// <summary>
/// Volania weboveho API simulatora tabul (TableSimulator) s jeho kontraktmi: ci bezi a ake triedy liniek pozna,
/// zalozenie liniek a tabul stanice. Ak simulator vyzaduje prihlasenie, ide s API klucom (<c>Authorization: Bearer</c>).
/// </summary>
internal static class TableSimulatorClient
{
    // opravnenie simulatora na zmenu liniek a tabul (kluc z /api/auth/me)
    private const string EditPermission = "simulator.edit";

    private static readonly HttpClient Http = CreateHttp();

    /// <summary>
    /// Klient s hlavickou, bez ktorej server simulatora zmenu odmietne (ochrana proti CSRF). Presmerovanie nenasleduje -
    /// presmerovana poziadavka by prisla o API kluc; presmerovanie na HTTPS sa obsluhe ohlasi.
    /// </summary>
    private static HttpClient CreateHttp()
    {
        var http = new HttpClient(new SocketsHttpHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(10) };
        http.DefaultRequestHeaders.Add("X-Requested-With", "GVDEditor");
        return http;
    }

    /// <summary>Webova adresa simulatora z textu (<c>http://host:port</c>); null pri neplatnej.</summary>
    public static Uri? ParseUrl(string text) =>
        Uri.TryCreate(text.Trim(), UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) ? uri : null;

    /// <summary>Poziadavka na simulator - s API klucom, ak je zadany.</summary>
    internal static HttpRequestMessage Request(HttpMethod method, Uri uri, string? apiKey)
    {
        var request = new HttpRequestMessage(method, uri);
        if (!string.IsNullOrWhiteSpace(apiKey))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey.Trim());
        return request;
    }

    /// <summary>
    /// Zisti, ci simulator bezi, ake triedy liniek pozna a ci s klucom smie zakladat linky a tabule. Simulator bez
    /// prihlasovania (aj starsi bez <c>/api/auth/me</c>) dovoli vsetko.
    /// </summary>
    public static async Task<SimulatorProbe> ProbeAsync(Uri baseUri, string? apiKey, TimeSpan timeout, CancellationToken cancel)
    {
        using var limit = CancellationTokenSource.CreateLinkedTokenSource(cancel);
        limit.CancelAfter(timeout);
        try
        {
            var me = await GetAsync<MeDto>(baseUri, "api/auth/me", apiKey, limit.Token);
            if (me is { LoginRequired: true, Authenticated: false })
                return new SimulatorProbe(SimulatorAccess.Unauthorized, null);
            var state = await GetAsync<StateDto>(baseUri, "api/state", apiKey, limit.Token);
            if (state is null)
                return new SimulatorProbe(SimulatorAccess.Offline, null);
            var canEdit = me is null || !me.LoginRequired || me.Permissions.Contains(EditPermission);
            return new SimulatorProbe(canEdit ? SimulatorAccess.Ok : SimulatorAccess.ReadOnly, state);
        }
        catch (SimulatorHttpsRequiredException e)
        {
            return new SimulatorProbe(SimulatorAccess.HttpsRequired, null, e.HttpsUrl);
        }
        catch (HttpRequestException e) when (IsUntrustedCertificate(e))
        {
            return new SimulatorProbe(SimulatorAccess.UntrustedCertificate, null);
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            return new SimulatorProbe(SimulatorAccess.Unauthorized, null);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException or OperationCanceledException && !cancel.IsCancellationRequested)
        {
            return new SimulatorProbe(SimulatorAccess.Offline, null);
        }
    }

    /// <summary>Spojenie zlyhalo, lebo certifikatu simulatora tento pocitac nedoveruje.</summary>
    public static bool IsUntrustedCertificate(HttpRequestException e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return e.InnerException is AuthenticationException || e.HttpRequestError == HttpRequestError.SecureConnectionError;
    }

    /// <summary>Zalozi v simulatore linky a tabule stanice.</summary>
    /// <exception cref="SimulatorHttpsRequiredException">simulator zo siete prijima len HTTPS</exception>
    /// <exception cref="HttpRequestException">simulator neodpoveda alebo poziadavku odmietol (stav 401/403 - kluc)</exception>
    public static async Task<StationImportedDto> ImportAsync(Uri baseUri, string? apiKey, StationImportRequest station, CancellationToken cancel)
    {
        using var request = Request(HttpMethod.Post, new Uri(baseUri, "api/station"), apiKey);
        request.Content = JsonContent.Create(station);
        using var response = await Http.SendAsync(request, cancel);
        ThrowIfRedirectedToHttps(response);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StationImportedDto>(cancel)
               ?? throw new HttpRequestException("Empty response.");
    }

    /// <summary>Presmerovanie na HTTPS (simulator zo siete prijima len HTTPS) ako vynimka s adresou simulatora.</summary>
    private static void ThrowIfRedirectedToHttps(HttpResponseMessage response)
    {
        if ((int)response.StatusCode is >= 300 and < 400 && response.Headers.Location is { IsAbsoluteUri: true } location
                                                         && location.Scheme == Uri.UriSchemeHttps)
            throw new SimulatorHttpsRequiredException(new Uri(location.GetLeftPart(UriPartial.Authority) + "/"));
    }

    /// <summary>Odpoved GET ako kontrakt; null, ak server cestu nepozna (starsi simulator).</summary>
    private static async Task<T?> GetAsync<T>(Uri baseUri, string path, string? apiKey, CancellationToken cancel)
    {
        using var request = Request(HttpMethod.Get, new Uri(baseUri, path), apiKey);
        using var response = await Http.SendAsync(request, cancel);
        ThrowIfRedirectedToHttps(response);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(cancel);
    }
}
