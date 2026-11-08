using GVDEditor.Config;

namespace GVDEditor.Integration;

/// <summary>
/// Prepinac INISSu so zaskrtavacim polom v konfiguracii spustania.
/// </summary>
/// <param name="Name">nazov bez prefixu, ako sa zapise na prikazovy riadok</param>
/// <param name="Get">hodnota v konfiguracii</param>
/// <param name="Set">zmena hodnoty v konfiguracii</param>
internal sealed record InissSwitch(string Name, Func<RunConfiguration, bool> Get, Action<RunConfiguration, bool> Set);
