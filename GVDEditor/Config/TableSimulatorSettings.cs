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

    /// <summary>Kopia.</summary>
    public TableSimulatorSettings Clone() => new() { Host = Host, BasePort = BasePort, WebUrl = WebUrl, Prepare = Prepare };
}
