using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Ci sa pred spustenim INISSu ulozia neulozene zmeny grafikonu.
/// </summary>
public enum SaveBeforeRun
{
    /// <summary>Opytat sa.</summary>
    [XmlEnum("Ask")] Ask,

    /// <summary>Ulozit bez pytania.</summary>
    [XmlEnum("Always")] Always,

    /// <summary>Neukladat - INISS nacita data z disku.</summary>
    [XmlEnum("Never")] Never
}
