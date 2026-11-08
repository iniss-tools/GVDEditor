namespace GVDEditor.Integration;

/// <summary>Linka stanice pripravovana v simulatore tabul.</summary>
/// <param name="Line">linka konfiguracie s tabulami, ktore jej INISS priradi</param>
/// <param name="Port">port linky na simulatore</param>
/// <param name="Redirected">.INI vedla programu linku uz presmerovava na simulator (port je z neho)</param>
internal sealed record SimulatorLinePlan(DriverLine Line, int Port, bool Redirected);
