namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Druh vozna, ako sa hlasi v radeni.
/// </summary>
internal enum CarKind
{
    /// <summary>Druh sa nepodarilo urcit.</summary>
    Unknown,
    First,
    Second,

    /// <summary>Vozen s oddielmi prvej aj druhej triedy.</summary>
    FirstSecond,
    Sleeper,
    Couchette,
    Restaurant,
    Bistro,

    /// <summary>Autovozen - cestujuci v nom necestuju, nehlasi sa.</summary>
    CarCarrier
}
