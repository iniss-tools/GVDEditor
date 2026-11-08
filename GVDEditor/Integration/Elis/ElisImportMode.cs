namespace GVDEditor.Integration.Elis;

/// <summary>
/// Co sa pri importe z ELIS stane s vlakmi, ktore grafikon uz ma.
/// </summary>
internal enum ElisImportMode
{
    /// <summary>Naimportovane vlaky sa pridaju k povodnym.</summary>
    Add,

    /// <summary>
    /// Povodne vlaky sa nahradia naimportovanymi, ale rovnaky vlak si ponecha udaje, ktore ELIS nepozna (kolaje,
    /// doplnky, jazyky...). Povodne vlaky, ktore v ELIS nie su, sa odstrania.
    /// </summary>
    Update,

    /// <summary>Povodne vlaky sa odstrania bez nahrady.</summary>
    Replace
}
