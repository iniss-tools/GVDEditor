using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.Integration;

/// <summary>Tabula priradena linke.</summary>
/// <param name="Table">tabula</param>
/// <param name="Automatic">priradena automaticky (COMUNICATION_PORT=0)</param>
internal sealed record LineTable(InissTable Table, bool Automatic);

/// <summary>Linka k tabuliam (sekcia Driver*) s tabulami, ktore jej INISS priradi, a zisteniami.</summary>
/// <param name="Section">sekcia (Driver, Driver0…)</param>
/// <param name="Line">cislo komunikacnej linky alebo null (INISS ju berie ako linku 0)</param>
/// <param name="Class">trieda linky</param>
/// <param name="Port">komunikacny kanal (TablePort)</param>
/// <param name="Active">INISS linku zalozi (cislo v rozsahu a nepouzite skorsou linkou)</param>
/// <param name="Tables">tabule, ktore INISS na linku posiela</param>
/// <param name="Problems">zistenia (zavaznost, text)</param>
internal sealed record DriverLine(
    string Section, int? Line, int Class, string Port, bool Active, IReadOnlyList<LineTable> Tables, IReadOnlyList<(RegSeverity Severity, string Text)> Problems)
{
    /// <summary>Kratky popis do stromu: <c>linka 3 · ELEN</c>.</summary>
    public string Summary => string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineSummary,
        Line?.ToString(CultureInfo.CurrentCulture) ?? "?", DriverClasses.Find(Class)?.Name ?? Class.ToString(CultureInfo.CurrentCulture));
}

/// <summary>Tabula, ktorej INISS nic neposle, a dovod.</summary>
/// <param name="Table">tabula</param>
/// <param name="Reason">dovod v jazyku UI</param>
internal sealed record UnservedTable(InissTable Table, string Reason);

/// <summary>Linky konfiguracie a priradenie fyzickych tabul k nim.</summary>
/// <param name="Lines">linky v poradi, v akom ich INISS zaklada</param>
/// <param name="Unserved">tabule s adresou, ktorym INISS nic neposle</param>
internal sealed record DriverLineMap(IReadOnlyList<DriverLine> Lines, IReadOnlyList<UnservedTable> Unserved);

/// <summary>
/// Priradenie fyzickych tabul k linkam ako v INISSe 3.39: linka dostane cislo z TablePort (COMn alebo predpona N=),
/// cislo musi byt 1-100 a pri duplicite INISS dalsiu linku ignoruje. Tabula s COMUNICATION_PORT=N ide na linku N,
/// ak jej protokol vyrobcu tabule prijima; tabula s 0 sa priradi automaticky linke s vlastnou rodinou protokolu
/// (prvej v poradi). Tabule s ID=-1 sa nikam neposielaju.
/// </summary>
internal static class DriverLines
{
    /// <summary>Linky a priradenie tabul.</summary>
    public static DriverLineMap Build(ResolvedConfig config, IReadOnlyList<InissTable> tables)
    {
        var drivers = config.Sections.Where(s => s.Definition.Name == "Driver")
            .Select(s => (s.Name, Port: s.Find("TablePort")?.Value as string ?? DriverDefaults.DefaultPort,
                Class: s.Find("TableClass")?.Value as int? ?? DriverDefaults.DefaultClass))
            .ToList();

        // linky, ktore INISS naozaj zalozi
        var active = new Dictionary<int, string>();
        var problems = drivers.ToDictionary(d => d.Name, _ => new List<(RegSeverity, string)>());
        foreach (var d in drivers)
        {
            var line = DriverClasses.LineNumber(d.Port);
            if (line is null or < 1 or > 100)
                problems[d.Name].Add((RegSeverity.Error, string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineOutOfRange, line ?? 0)));
            else if (active.TryGetValue(line.Value, out var first))
                problems[d.Name].Add((RegSeverity.Error, string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineDuplicate, line, first)));
            else
                active[line.Value] = d.Name;
        }

        var assigned = drivers.ToDictionary(d => d.Name, _ => new List<LineTable>());
        var unserved = new List<UnservedTable>();
        foreach (var t in tables.Where(t => t.Table.ID != -1))
        {
            var port = t.Table.CommunicationPort;
            var manufacturer = t.Manufacturer ?? -1;
            if (port == 0)
            {
                // automaticky: linka s najlepsou zhodou (vlastna rodina), pri rovnosti prva
                var best = drivers.Where(d => active.ContainsValue(d.Name) && DriverClasses.AcceptsAutomatically(d.Class, manufacturer))
                    .OrderByDescending(d => DriverClasses.Score(d.Class, manufacturer)).Select(d => d.Name).FirstOrDefault();
                if (best is null) unserved.Add(new UnservedTable(t, Resources.InissSettings_Unserved_NoAuto));
                else assigned[best].Add(new LineTable(t, true));
                continue;
            }

            if (!active.TryGetValue(port, out var driver))
            {
                unserved.Add(new UnservedTable(t, string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Unserved_NoDriver, port)));
                continue;
            }

            var cls = drivers.First(d => d.Name == driver).Class;
            if (DriverClasses.Accepts(cls, manufacturer))
            {
                assigned[driver].Add(new LineTable(t, false));
                continue;
            }

            var reason = string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_LineWrongFamily, t.Table.Key, t.Table.TableCatalog?.Manufacturer?.Name,
                DriverClasses.Find(cls)?.Name ?? cls.ToString(CultureInfo.CurrentCulture));
            problems[driver].Add((RegSeverity.Warning, reason));
            unserved.Add(new UnservedTable(t, reason));
        }

        var lines = drivers.Select(d =>
        {
            var list = problems[d.Name];
            var isActive = active.ContainsValue(d.Name);
            if (isActive && assigned[d.Name].Count == 0 && d.Class is not (128 or 129))
                list.Add((RegSeverity.Info, Resources.InissSettings_LineNoTables));
            return new DriverLine(d.Name, DriverClasses.LineNumber(d.Port), d.Class, d.Port, isActive, assigned[d.Name], list);
        }).ToList();
        return new DriverLineMap(lines, unserved);
    }

    /// <summary>Prve volne cislo linky (1-100): linka, ktoru tabule v datach pouzivaju a nema ovladac, inak najmensie volne.</summary>
    public static int FreeLine(DriverLineMap map, IReadOnlyList<InissTable> tables)
    {
        var used = map.Lines.Select(l => l.Line).OfType<int>().ToHashSet();
        foreach (var port in tables.Where(t => t.Table.ID != -1).Select(t => t.Table.CommunicationPort).Distinct().Order())
            if (port is >= 1 and <= 100 && !used.Contains(port))
                return port;
        for (var i = 1; i <= 100; i++)
            if (!used.Contains(i))
                return i;
        return 1;
    }
}
