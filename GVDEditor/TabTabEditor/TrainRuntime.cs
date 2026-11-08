namespace GVDEditor.TabTabEditor;

/// <summary>
/// Prevadzkovy stav vlaku, ktory grafikon nepozna a nahlad si ho musi nasimulovat: meskania,
/// stav, vyluky, odklon a "teraz".
/// </summary>
internal sealed record TrainRuntime
{
    public int ArrivalDelayMinutes { get; init; }
    public int DepartureDelayMinutes { get; init; }
    public bool IsStanding { get; init; }
    public bool IsDispatched { get; init; }
    public bool LockoutArrival { get; init; }
    public bool LockoutDeparture { get; init; }
    public bool IsDeflected { get; init; }
    public DateTime Now { get; init; } = DateTime.Now;
}
