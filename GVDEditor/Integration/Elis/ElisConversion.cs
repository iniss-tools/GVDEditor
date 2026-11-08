using Iniss.Elis.Data;
using GVDEditor.Domain.Entities;

namespace GVDEditor.Integration.Elis;

/// <summary>
/// Vlaky prevedene z dat ELIS a udaje k nim, ktore sa pouziju az po volbach obsluhy (kratke hlasenie, poznamky).
/// </summary>
public sealed class ElisConversion
{
    /// <summary>Prevedene vlaky.</summary>
    public required List<Train> Trains { get; init; }

    /// <summary>Poznamky platne v stanici grafikonu (kluce bez useku) pre kazdy vlak.</summary>
    public required Dictionary<Train, List<string>> Remarks { get; init; }

    /// <summary>Druh stanice z ELIS (<see cref="ElisStop.StationType" />) podla ID stanice grafikonu.</summary>
    public required Dictionary<string, int> StationTypes { get; init; }
}
