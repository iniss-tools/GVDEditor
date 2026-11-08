using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;
using TableSimulator.Contracts.Auth;
using TableSimulator.Contracts.State;
using TableSimulator.Contracts.Station;

namespace GVDEditor.Integration;

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
