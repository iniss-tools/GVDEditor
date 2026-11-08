namespace GVDEditor.Integration;

/// <summary>Ako GVDEditor so simulatorom pracovat moze.</summary>
internal enum SimulatorAccess
{
    /// <summary>Simulator na adrese neodpoveda.</summary>
    Offline,

    /// <summary>Simulator vyzaduje prihlasenie a API kluc chyba alebo ho neprijal.</summary>
    Unauthorized,

    /// <summary>Kluc smie len pozerat - linky a tabule nezalozi.</summary>
    ReadOnly,

    /// <summary>Linky a tabule sa daju zalozit.</summary>
    Ok,

    /// <summary>Simulator zo siete prijima len HTTPS - adresa sa ma zmenit na <see cref="SimulatorProbe.HttpsUrl" />.</summary>
    HttpsRequired,

    /// <summary>Certifikatu simulatora tento pocitac nedoveruje (certifikacna autorita simulatora nie je nainstalovana).</summary>
    UntrustedCertificate
}
