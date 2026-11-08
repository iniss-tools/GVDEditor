namespace GVDEditor.Integration;

/// <summary>Simulator poziadavku presmeroval na HTTPS - zo siete neprijima HTTP.</summary>
/// <param name="httpsUrl">adresa simulatora cez HTTPS (bez cesty)</param>
internal sealed class SimulatorHttpsRequiredException(Uri httpsUrl)
    : HttpRequestException($"The simulator accepts only HTTPS from the network: {httpsUrl}")
{
    public Uri HttpsUrl { get; } = httpsUrl;
}
