using ToolsCore.Iniss.Registry;

namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Nastavenia INISSu vybranej konfiguracie spustania pre kontroly analyzy.
/// </summary>
/// <param name="ConfigName">nazov konfiguracie spustania</param>
/// <param name="Config">nastavenia tak, ako ich INISS nacita</param>
/// <param name="Tables">fyzicke tabule instalacie v poradi INISSu</param>
/// <param name="Exists">konfiguracia je v registri alebo v .INI (inak INISS bezi s predvolenymi)</param>
internal sealed record InissRegistryView(string ConfigName, ResolvedConfig Config, IReadOnlyList<InissTable> Tables, bool Exists);
