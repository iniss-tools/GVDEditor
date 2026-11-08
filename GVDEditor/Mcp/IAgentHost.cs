using ModelContextProtocol;

namespace GVDEditor.Mcp;

/// <summary>Hlavne okno GVDEditora pri zmene grafikonu agentom.</summary>
internal interface IAgentHost
{
    /// <summary>
    /// Vykona zmenu <paramref name="change" /> na vlakne okna: ak sa grafikon teraz menit neda (otvorene okno, rozpisana
    /// bunka, nacitavanie), hodi <see cref="McpException" /> s kodom <c>editor.busy</c>. Pocas zmeny potlaci udalosti
    /// okna, potom obnovi zobrazenie a zapamata krok spat s popisom, ktory zmena vrati (null = grafikon sa nezmenil).
    /// </summary>
    void Change(Func<string?> change);
}
