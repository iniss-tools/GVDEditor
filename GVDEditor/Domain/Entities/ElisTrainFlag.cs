namespace GVDEditor.Domain.Entities;

/// <summary>
/// Priznak vlaku, ktory import z ELIS nastavi podla poznamky vlaku.
/// </summary>
public enum ElisTrainFlag
{
    /// <summary>Ziadny.</summary>
    None,

    /// <summary><see cref="Train.IsMiestenkovy" />.</summary>
    Reservation,

    /// <summary><see cref="Train.IsNizkopodlazny" />.</summary>
    LowFloor,

    /// <summary><see cref="Train.IsIbaLozkovy" />.</summary>
    SleeperOnly
}
