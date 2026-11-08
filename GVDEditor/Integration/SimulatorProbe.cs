using TableSimulator.Contracts.State;

namespace GVDEditor.Integration;

/// <summary>Zistenie stavu simulatora.</summary>
/// <param name="Access">co smie GVDEditor robit</param>
/// <param name="State">stav simulatora (triedy liniek); null, ak sa neda zistit</param>
/// <param name="HttpsUrl">adresa HTTPS simulatora, ak pristup cez HTTP presmeroval</param>
internal sealed record SimulatorProbe(SimulatorAccess Access, StateDto? State, Uri? HttpsUrl = null);
