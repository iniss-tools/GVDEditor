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

/// <summary>
/// Co import z ELIS urobi s vlakom, ktory ma danu poznamku.
/// </summary>
/// <param name="Flag">Priznak vlaku, ktory sa zapne.</param>
/// <param name="Addition">Kod doplnku (kluc zvuku bez predpony D), ktory sa vlaku prida; prazdny = ziadny.</param>
/// <param name="Lockout">Cislo vyluky, ktora sa vlaku priradi; 0 = ziadna.</param>
public sealed record ElisRemarkAction(ElisTrainFlag Flag, string Addition, int Lockout)
{
    /// <summary>
    /// Poznamka bez ucinku.
    /// </summary>
    public static ElisRemarkAction Nothing { get; } = new(ElisTrainFlag.None, "", 0);

    /// <summary>
    /// Ci poznamka nic nenastavuje.
    /// </summary>
    public bool IsEmpty => Flag == ElisTrainFlag.None && Addition.Length == 0 && Lockout == 0;
}
