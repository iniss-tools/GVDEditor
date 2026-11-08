using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Konfiguracie zdielane v datach instalacie (<c>.gvdeditor\RunConfigurations.xml</c>).
/// </summary>
[XmlRoot("RunConfigurations")]
public sealed class SharedRunConfigurations
{
    /// <summary>Konfiguracie.</summary>
    [XmlElement("RunConfiguration")]
    public List<RunConfiguration> Items { get; set; } = [];
}
