using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Co spravit, ked INISS tejto konfiguracie uz bezi.
/// </summary>
public enum WhenAlreadyRunning
{
    /// <summary>Opytat sa (restart alebo dalsia instancia).</summary>
    [XmlEnum("Ask")] Ask,

    /// <summary>Beziaci INISS riadne ukoncit a spustit znova.</summary>
    [XmlEnum("Restart")] Restart,

    /// <summary>Spustit dalsiu instanciu (INISS ju pusti len s /Multiuse).</summary>
    [XmlEnum("NewInstance")] NewInstance
}
