using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Kam GVDEditor presmerovava linky INISSu pri skuske so simulatorom tabul (TableSimulator).
/// </summary>
public sealed class TableSimulatorSettings
{
    /// <summary>Predvoleny prvy port - linka N pocuva na porte 47000 + N (ako v simulatore).</summary>
    public const int DefaultBasePort = 47000;

    /// <summary>Adresa pocitaca so simulatorom, ako ju pouzije INISS (TablePort <c>N=TCP://adresa:port</c>).</summary>
    [XmlAttribute("Host")]
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>Prvy port - port linky = prvy port + cislo linky.</summary>
    [XmlAttribute("BasePort")]
    public int BasePort { get; set; } = DefaultBasePort;

    /// <summary>Webova adresa simulatora (jeho API).</summary>
    [XmlAttribute("WebUrl")]
    public string WebUrl { get; set; } = "http://localhost:5470";

    /// <summary>Pri presmerovani zalozit v simulatore linky a tabule stanice.</summary>
    [XmlAttribute("Prepare")]
    public bool Prepare { get; set; } = true;

    /// <summary>API kluc simulatora sifrovany pre aktualneho pouzivatela Windows (DPAPI) - v configu nie je citatelny.</summary>
    [XmlAttribute("ApiKey")]
    public string ProtectedApiKey { get; set; } = "";

    /// <summary>
    /// API kluc simulatora (<c>Authorization: Bearer</c>), ak simulator vyzaduje prihlasenie; prazdny = bez kluca.
    /// Kluc ulozeny inym pouzivatelom alebo na inom pocitaci sa neda precitat - je prazdny.
    /// </summary>
    [XmlIgnore]
    public string ApiKey
    {
        get => Unprotect(ProtectedApiKey);
        set => ProtectedApiKey = Protect(value);
    }

    /// <summary>Kopia.</summary>
    public TableSimulatorSettings Clone() =>
        new() { Host = Host, BasePort = BasePort, WebUrl = WebUrl, Prepare = Prepare, ProtectedApiKey = ProtectedApiKey };

    private static readonly byte[] Entropy = "GVDEditor.TableSimulator.ApiKey"u8.ToArray();

    private static string Protect(string? key) =>
        string.IsNullOrWhiteSpace(key)
            ? ""
            : Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(key.Trim()), Entropy, DataProtectionScope.CurrentUser));

    private static string Unprotect(string protectedKey)
    {
        if (protectedKey.Length == 0) return "";
        try
        {
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(protectedKey), Entropy, DataProtectionScope.CurrentUser));
        }
        catch (Exception e) when (e is CryptographicException or FormatException)
        {
            return "";
        }
    }
}
