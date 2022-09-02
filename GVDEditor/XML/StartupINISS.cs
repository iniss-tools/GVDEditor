using System.Xml.Serialization;

namespace GVDEditor.XML;

/// <summary>
///     Konfiguracia spustania programu INISS pomocou tohto programu.
/// </summary>
public record StartupINISS
{
    /// <summary>
    ///     Ci ma program zapnut ako Administrator.
    /// </summary>
    [XmlElement("RunAsAdmin"), DefaultValue(false)]
    public bool RunAsAdmin;

    /// <summary>
    ///     Argumenty prikazoveho riadka ako vstup pre program.
    /// </summary>
    [XmlElement("CmdArgs"), DefaultValue("")]
    public string CmdArgs;
}