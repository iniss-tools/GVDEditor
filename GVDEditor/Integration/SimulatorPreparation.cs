using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Config;
using TableSimulator.Contracts.Station;
using ToolsCore.Iniss.Registry;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Integration;

/// <summary>Linka stanice pripravovana v simulatore tabul.</summary>
/// <param name="Line">linka konfiguracie s tabulami, ktore jej INISS priradi</param>
/// <param name="Port">port linky na simulatore</param>
/// <param name="Redirected">.INI vedla programu linku uz presmerovava na simulator (port je z neho)</param>
internal sealed record SimulatorLinePlan(DriverLine Line, int Port, bool Redirected);

/// <summary>Co sa v simulatore zalozi: poziadavka a linky, z ktorych vznikla.</summary>
/// <param name="Request">poziadavka pre simulator</param>
/// <param name="Lines">pripravovane linky</param>
/// <param name="Missing">vybrane cisla liniek, ktore konfiguracia nema alebo nie su aktivne (vtedy sa nic nepripravi)</param>
internal sealed record SimulatorPlan(StationImportRequest Request, IReadOnlyList<SimulatorLinePlan> Lines, IReadOnlyList<int> Missing);

/// <summary>
/// Linky a tabule stanice pre simulator tabul podla konfiguracie spustania - ako Pripravit v simulatore v okne
/// presmerovania, ale bez zmeny .INI: presmerovana linka ide na port z .INI, ostatne na prvy port + cislo linky.
/// </summary>
internal static partial class SimulatorPreparation
{
    /// <summary>
    /// Zostavi poziadavku pre simulator. Bez vyberu liniek sa vezmu linky, ktore .INI presmerovava, a ak ziadne nie su,
    /// vsetky aktivne linky s cislom - ako predvoleny vyber okna presmerovania.
    /// </summary>
    /// <param name="config">konfiguracia spustania s programom INISSu</param>
    /// <param name="installationDir">priecinok instalacie INISSu</param>
    /// <param name="grafikons">grafikony instalacie (tabule sa beru zo vsetkych ako v INISSe)</param>
    /// <param name="circuits">zvukove okruhy instalacie (spinacie jednotky zosilnovacov ELSVO)</param>
    /// <param name="settings">simulator z nastaveni GVDEditora</param>
    /// <param name="lines">cisla liniek; null = predvoleny vyber</param>
    public static SimulatorPlan Plan(RunConfiguration config, string installationDir, IReadOnlyList<DirList> grafikons,
        IEnumerable<(string Name, string? AmplifierPort, string? ExchangeParameter)> circuits, TableSimulatorSettings settings,
        IReadOnlyCollection<int>? lines)
    {
        var tables = InissTableMap.Build(grafikons);
        var resolved = InissEnvironment.Resolve(config, installationDir, tables.ToDictionary(t => t.Index, t => t.ToInfo()));
        var exe = PathUtils.CombinePath(installationDir, config.Program)!;
        return Plan(resolved, InissIniFile.Load(InissRegistryReader.IniPathFor(exe)), tables, InissEnvironment.LogDirectory(resolved, installationDir).Path,
            ElsvoUnits.FromCircuits(circuits), settings.BasePort, lines);
    }

    /// <inheritdoc cref="Plan(RunConfiguration, string, IReadOnlyList{DirList}, IEnumerable{ValueTuple{string, string, string}}, TableSimulatorSettings, IReadOnlyCollection{int})" />
    /// <param name="resolved">nastavenia INISSu konfiguracie</param>
    /// <param name="ini">subor .INI vedla programu; null = nie je</param>
    /// <param name="tables">fyzicke tabule instalacie v poradi INISSu</param>
    /// <param name="logFolder">priecinok logov INISSu</param>
    /// <param name="amplifiers">spinacie jednotky zosilnovacov ELSVO</param>
    /// <param name="basePort">prvy port - linka N, ktoru .INI nepresmerovava, ide na prvy port + N</param>
    /// <param name="lines">cisla liniek; null = predvoleny vyber</param>
    internal static SimulatorPlan Plan(ResolvedConfig resolved, InissIniFile? ini, IReadOnlyList<InissTable> tables, string? logFolder,
        IReadOnlyList<ElsvoUnit> amplifiers, int basePort, IReadOnlyCollection<int>? lines)
    {
        var map = DriverLines.Build(resolved, tables);
        var redirected = SimulatorRedirect.Find(ini).Select(r => r.Section).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var selectable = map.Lines.Where(l => l is { Active: true, Line: not null }).ToList();
        List<DriverLine> chosen;
        if (lines is null)
            chosen = selectable.Any(l => redirected.Contains(l.Section)) ? selectable.Where(l => redirected.Contains(l.Section)).ToList() : selectable;
        else
        {
            var missing = lines.Where(n => selectable.All(l => l.Line != n)).Distinct().ToList();
            if (missing.Count > 0)
                return new SimulatorPlan(new StationImportRequest([], []), [], missing);
            chosen = selectable.Where(l => lines.Contains(l.Line!.Value)).ToList();
        }

        var plans = chosen.Select(l =>
        {
            var tcp = TcpPort().Match(l.Port);
            var isRedirected = redirected.Contains(l.Section) && tcp.Success;
            var port = isRedirected ? int.Parse(tcp.Groups[1].Value, CultureInfo.InvariantCulture) : basePort + l.Line!.Value;
            return new SimulatorLinePlan(l, port, isRedirected);
        }).ToList();
        var ports = plans.ToDictionary(p => p.Line.Section, p => p.Port, StringComparer.OrdinalIgnoreCase);
        return new SimulatorPlan(SimulatorStation.From(map, ports, tables, logFolder, amplifiers), plans, []);
    }

    /// <summary>Port v TablePort presmerovanom na simulator: <c>N=TCP://host:port</c>.</summary>
    [GeneratedRegex(@"TCP://[^:/\s]+:(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex TcpPort();
}
