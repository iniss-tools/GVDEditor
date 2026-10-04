using System.Globalization;
using GVDEditor.Config;
using GVDEditor.Domain.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Integration;

/// <summary>
/// Prepinac INISSu so zaskrtavacim polom v konfiguracii spustania.
/// </summary>
/// <param name="Name">nazov bez prefixu, ako sa zapise na prikazovy riadok</param>
/// <param name="Get">hodnota v konfiguracii</param>
/// <param name="Set">zmena hodnoty v konfiguracii</param>
internal sealed record InissSwitch(string Name, Func<RunConfiguration, bool> Get, Action<RunConfiguration, bool> Set);

/// <summary>
/// Konfiguracie spustania INISSu bez UI: prikazovy riadok, prenos povodneho spolocneho nastavenia, predvolene
/// konfiguracie instalacie a nazvy.
/// </summary>
internal static class RunConfigurations
{
    /// <summary>
    /// Prepinace v poradi, v akom sa zapisu na prikazovy riadok.
    /// </summary>
    public static IReadOnlyList<InissSwitch> Switches { get; } =
    [
        new("Minimize", c => c.Minimize, (c, v) => c.Minimize = v),
        new("Multiuse", c => c.Multiuse, (c, v) => c.Multiuse = v),
        new("Remote", c => c.Remote, (c, v) => c.Remote = v),
        new("Export", c => c.Export, (c, v) => c.Export = v),
        new("ExportHlas", c => c.ExportHlas, (c, v) => c.ExportHlas = v),
        new("Import", c => c.Import, (c, v) => c.Import = v),
        new("ImportDat", c => c.ImportDat, (c, v) => c.ImportDat = v),
        new("NoRestore", c => c.NoRestore, (c, v) => c.NoRestore = v)
    ];

    /// <summary>
    /// Argumenty prikazoveho riadka (bez programu).
    /// </summary>
    public static string Arguments(RunConfiguration config)
    {
        var args = Switches.Where(s => s.Get(config)).Select(s => "/" + s.Name).ToList();
        if (!string.IsNullOrWhiteSpace(config.Registry))
            args.Add(Quote("/Reg:" + config.Registry.Trim()));
        args.AddRange(config.ActivatorList.Select(n => "/" + n.ToString(CultureInfo.InvariantCulture)));
        if (!string.IsNullOrWhiteSpace(config.ExtraArguments))
            args.Add(config.ExtraArguments.Trim());
        return string.Join(" ", args);
    }

    /// <summary>
    /// Cely prikazovy riadok na zobrazenie (program v uvodzovkach a argumenty).
    /// </summary>
    public static string CommandLine(RunConfiguration config)
    {
        var args = Arguments(config);
        var program = "\"" + config.Program + "\"";
        return args.Length == 0 ? program : program + " " + args;
    }

    /// <summary>
    /// Vetva registra, z ktorej INISS tejto konfiguracie cita nastavenia (<c>/Reg:</c>, inak meno programu).
    /// </summary>
    public static string AppName(RunConfiguration config) => InissRegistry.AppNameFor(config.Program, config.Registry.Trim());

    /// <summary>
    /// Konfiguracia z textu argumentov (povodne nastavenie spustania): zname prepinace, <c>/Reg:</c> a aktivatory
    /// sa rozlozia, ostatne ostane v dalsich parametroch.
    /// </summary>
    public static RunConfiguration FromArguments(string name, string program, bool runAsAdmin, string? arguments)
    {
        var config = new RunConfiguration { Name = name, Program = program, RunAsAdmin = runAsAdmin };
        var extra = new List<string>();
        var activators = new List<int>();
        foreach (var token in INISSArgs.Tokens(arguments))
        {
            var known = token.Length > 1 && token[0] is '/' or '-' ? Switches.FirstOrDefault(s => string.Equals(token[1..], s.Name, StringComparison.OrdinalIgnoreCase)) : null;
            if (known is not null)
                known.Set(config, true);
            else if (INISSArgs.Registry(Quote(token)) is { } reg)
                config.Registry = reg;
            else if (token.Length == 2 && token[0] is '/' or '-' && token[1] is >= '1' and <= '9')
                activators.Add(token[1] - '0');
            else
                extra.Add(Quote(token));
        }

        config.ActivatorList = activators;
        config.ExtraArguments = string.Join(" ", extra);
        return config;
    }

    /// <summary>
    /// Predvolene konfiguracie instalacie bez ulozenych konfiguracii: jedna na kazdy program INISS, s povodnym
    /// spolocnym nastavenim spustania (spravca, argumenty).
    /// </summary>
    public static List<RunConfiguration> Defaults(IEnumerable<string> programs, StartupINISS legacy)
    {
        var result = new List<RunConfiguration>();
        foreach (var program in programs)
        {
            var name = UniqueName(DefaultName(program), result.Select(r => r.Name));
            result.Add(FromArguments(name, program, legacy.RunAsAdmin, legacy.CmdArgs));
        }

        return result;
    }

    /// <summary>
    /// Nazov konfiguracie podla programu: meno suboru bez pripony a bez uvodneho <c>INISS -</c> (nie <c>INISSView</c>).
    /// </summary>
    public static string DefaultName(string program)
    {
        var name = Path.GetFileNameWithoutExtension(program).Trim();
        if (name.Length > 5 && name.StartsWith("INISS", StringComparison.OrdinalIgnoreCase) && name[5] is ' ' or '-' or '_' or '\u2013')
        {
            var rest = name[5..].TrimStart(' ', '-', '_', '\u2013').Trim();
            if (rest.Length > 0)
                return rest;
        }

        return name;
    }

    /// <summary>
    /// <paramref name="name" />, pripadne s poradovym cislom, aby sa nezhodoval s ziadnym z <paramref name="existing" />.
    /// </summary>
    public static string UniqueName(string name, IEnumerable<string> existing)
    {
        var used = new HashSet<string>(existing, StringComparer.CurrentCultureIgnoreCase);
        if (!used.Contains(name))
            return name;

        for (var i = 2; ; i++)
        {
            var candidate = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", name, i);
            if (!used.Contains(candidate))
                return candidate;
        }
    }

    /// <summary>
    /// Grafikony, ktore zapina aktivator <c>/N</c> (cislica vo 4. stlpci DirList), podla cislice.
    /// </summary>
    public static IReadOnlyDictionary<int, List<string>> ActivatorTargets(IEnumerable<DirList> dirs)
    {
        var result = new SortedDictionary<int, List<string>>();
        foreach (var dir in dirs)
        {
            if (DirListFlags.Parse(dir.Flags).Switch is not { } n)
                continue;
            if (!result.TryGetValue(n, out var names))
                result[n] = names = [];
            names.Add(dir.IsDataRoot ? Path.GetFileName(dir.FullPath.TrimEnd('\\', '/')) : dir.DirName);
        }

        return result;
    }

    private static string Quote(string token) => token.Contains(' ', StringComparison.Ordinal) ? "\"" + token + "\"" : token;
}
