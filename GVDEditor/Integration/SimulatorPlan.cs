using TableSimulator.Contracts.Station;

namespace GVDEditor.Integration;

/// <summary>Co sa v simulatore zalozi: poziadavka a linky, z ktorych vznikla.</summary>
/// <param name="Request">poziadavka pre simulator</param>
/// <param name="Lines">pripravovane linky</param>
/// <param name="Missing">vybrane cisla liniek, ktore konfiguracia nema alebo nie su aktivne (vtedy sa nic nepripravi)</param>
internal sealed record SimulatorPlan(StationImportRequest Request, IReadOnlyList<SimulatorLinePlan> Lines, IReadOnlyList<int> Missing);
