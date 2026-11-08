using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Konfiguracie spustania jednej instalacie ulozene v config.xml (tento pocitac) a vybrana konfiguracia.
/// </summary>
public sealed record InstallationRunConfigurations
{
    /// <summary>Priecinok instalacie INISS.</summary>
    [XmlAttribute("Dir")]
    public string Dir { get; set; } = "";

    /// <summary>Identifikator vybranej konfiguracie (aj zdielanej).</summary>
    [XmlAttribute("Selected"), DefaultValue("")]
    public string Selected { get; set; } = "";

    /// <summary>Konfiguracie ulozene len na tomto pocitaci.</summary>
    [XmlElement("RunConfiguration")]
    public List<RunConfiguration> Items { get; set; } = [];

    /// <summary>Hlboka kopia.</summary>
    public InstallationRunConfigurations DeepCopy() => this with { Items = Items.Select(i => i with { }).ToList() };
}
