using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Integration;
using GVDEditor.Properties;
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

/// <summary>
/// Zistenie kontroly nastaveni INISSu voci grafikonu.
/// </summary>
/// <param name="Key">identita zistenia (na overenie po oprave)</param>
/// <param name="Type">zavaznost</param>
/// <param name="Text">text</param>
/// <param name="Solution">navrh riesenia</param>
/// <param name="Section">sekcia okna Nastavenia INISSu, ktora sa ma otvorit</param>
internal sealed record InissFinding(string Key, ProblemType Type, string Text, string Solution, string? Section);

/// <summary>
/// Kontroly nastaveni INISSu (register, .INI) voci grafikonu: ci INISS tabuliam grafikonu nieco posle, ci su
/// zapnute, ci ich linky nie su presmerovane na simulator, ci v sekcii Tables nie su nastavenia tabul, ktore v datach
/// nie su, a ci INISS cita subory, do ktorych GVDEditor zapisuje.
/// </summary>
internal static class InissChecks
{
    /// <summary>
    /// Nazvy suborov dat v <c>PathNames</c> a subor, do ktoreho GVDEditor zapisuje (INISS ich porovnava bez ohladu na
    /// velkost pismen).
    /// </summary>
    private static readonly (string Key, string File)[] DataFiles =
    [
        ("StationsTxtFile", GvdFileConsts.FileStanice), ("PoziceTxtFile", GvdFileConsts.FilePozice),
        ("StateDgm", GvdFileConsts.FileStatedgmDat), ("StateDgmTxt", GvdFileConsts.FileStatedgm),
        ("CategoriTxt", GvdFileConsts.FileCategori), ("Zpozdeni", GvdFileConsts.FileZpozdeniDat), ("ZpozdeniTxt", GvdFileConsts.FileZpozdeni),
        ("GVGrafikon", GvdFileConsts.FileGrafikon), ("GVVlaky", GvdFileConsts.FileVlaky), ("GVDoplnky", GvdFileConsts.FileDoplnky),
        ("GVRazeni", GvdFileConsts.FileRazeni), ("GVVyluka", GvdFileConsts.FileVyluka), ("GVVzory", GvdFileConsts.FileVzory),
        ("GVForeign", GvdFileConsts.FileForeign), ("GVMOS", GvdFileConsts.FileMos), ("GVTabTexts", GvdFileConsts.FileTtexts),
        ("GVStaHlasB", GvdFileConsts.FileStahlasb), ("GVStaHlasC", GvdFileConsts.FileStahlasc),
        ("GVExport3A", GvdFileConsts.FileExport3A), ("GVExport3B", GvdFileConsts.FileExport3B), ("GVExport3C", GvdFileConsts.FileExport3C),
        ("Razeni1.TXT", GvdFileConsts.FileRazeni1), ("DirList.TXT", GvdFileConsts.FileDirlist)
    ];

    /// <summary>
    /// Problemy grafikonu <paramref name="gvd" /> podla nastaveni INISSu (ak ich analyza ma - <see cref="AnalysisScope.InissLoader" />).
    /// </summary>
    public static IEnumerable<IProblem> Problems(GVDDirectory gvd, AnalysisScope scope)
    {
        if (scope.InissLoader?.Invoke() is not { } view) return [];
        var grafikon = GrafikonName(gvd);
        return Evaluate(grafikon, view).Select(f => new InissProblem(f, grafikon, scope));
    }

    /// <summary>Nazov grafikonu v zozname tabul INISSu (priecinok, pri datach priamo v DATA <c>DATA</c>).</summary>
    public static string GrafikonName(GVDDirectory gvd) => gvd.Dir.IsDataRoot ? "DATA" : gvd.Dir.DirName;

    /// <summary>Zistenia pre grafikon <paramref name="grafikon" /> - bez pristupu k registru.</summary>
    public static List<InissFinding> Evaluate(string grafikon, InissRegistryView view)
    {
        var findings = new List<InissFinding>();
        void Add(string key, ProblemType type, string text, string solution, string? section) =>
            findings.Add(new InissFinding(key, type, text, solution, section));

        if (!view.Exists)
        {
            Add("missing", ProblemType.Hint, string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Missing, view.ConfigName),
                Resources.InissCheck_Missing_Fix, null);
            return findings;
        }

        var config = view.Config;

        // subory, do ktorych GVDEditor zapisuje, INISS cita pod inym nazvom
        foreach (var (key, file) in DataFiles)
        {
            if (config.Find("PathNames", key) is not { Value: string value } setting || !RegTools.IsExplicit(setting.Source)) continue;
            if (string.Equals(value.Trim(), file, StringComparison.OrdinalIgnoreCase)) continue;
            Add("file:" + key, ProblemType.Warning, string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_FileName, key, value),
                Resources.InissCheck_FileName_Fix, "PathNames");
        }

        // tabule grafikonu
        var mine = view.Tables.Where(t => string.Equals(t.Grafikon, grafikon, StringComparison.OrdinalIgnoreCase)).ToList();
        if (mine.Count > 0)
        {
            if (config.Find("Environment", "OutToTableDriver")?.Value is 0)
            {
                Add("output", ProblemType.Warning, Resources.InissCheck_OutputOff, Resources.InissCheck_OutputOff_Fix, "Environment");
            }
            else
            {
                var lines = DriverLines.Build(config, view.Tables);
                foreach (var unserved in lines.Unserved.Where(u => mine.Contains(u.Table)))
                    Add("unserved:" + unserved.Table.Index, ProblemType.Warning,
                        string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Unserved, unserved.Table.Table.Key, unserved.Reason),
                        Resources.InissCheck_Unserved_Fix, "Tables");
                foreach (var line in lines.Lines)
                foreach (var (severity, text) in line.Problems.Where(p => p.Severity != RegSeverity.Info))
                    Add("line:" + line.Section + ":" + text, severity == RegSeverity.Error ? ProblemType.Error : ProblemType.Warning,
                        string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Line, line.Section, text), Resources.InissCheck_Line_Fix, line.Section);

                // skuska so simulatorom tabul, na ktoru sa lahko zabudne - skutocne tabule potom nedostanu nic
                var redirected = SimulatorRedirect.Find(config.Source.Ini).Select(r => r.Section).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var toSimulator = lines.Lines.Where(l => redirected.Contains(l.Section) && l.Tables.Any(t => mine.Contains(t.Table))).Select(l => l.Section).ToList();
                if (toSimulator.Count > 0)
                    Add("redirected", ProblemType.Warning, string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Redirected, string.Join(", ", toSimulator)),
                        Resources.InissCheck_Redirected_Fix, toSimulator[0]);
            }

            foreach (var table in mine)
                if (config.Find("Tables", "Enabled" + table.Index.ToString(CultureInfo.InvariantCulture))?.Value is 0)
                    Add("disabled:" + table.Index, ProblemType.Hint, string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Disabled, table.Table.Key),
                        Resources.InissCheck_Disabled_Fix, "Tables");
        }

        // nastavenia tabul, ktore v datach nie su (po pridani ci odstraneni tabule sa poradie posunie)
        var orphans = config.FindSection("Tables")?.Settings
            .Where(s => s.Setting.Kind == RegNameKind.Indexed && s.Table is null && RegTools.IsExplicit(s.Source))
            .Select(s => IndexOf(s.Name)).OfType<int>().Distinct().Order().ToList() ?? [];
        if (orphans.Count > 0)
            Add("orphans", ProblemType.Hint, string.Format(CultureInfo.CurrentCulture, Resources.InissCheck_Orphans, Ranges(orphans), view.Tables.Count),
                Resources.InissCheck_Orphans_Fix, "Tables");

        return findings;
    }

    /// <summary>Zoradene cisla ako rozsahy: <c>1, 4–7, 9</c>.</summary>
    public static string Ranges(IReadOnlyList<int> numbers)
    {
        var parts = new List<string>();
        for (var i = 0; i < numbers.Count; i++)
        {
            var start = numbers[i];
            while (i + 1 < numbers.Count && numbers[i + 1] == numbers[i] + 1) i++;
            parts.Add(start == numbers[i]
                ? start.ToString(CultureInfo.CurrentCulture)
                : start.ToString(CultureInfo.CurrentCulture) + (numbers[i] == start + 1 ? ", " : "–") + numbers[i].ToString(CultureInfo.CurrentCulture));
        }

        return string.Join(", ", parts);
    }

    private static int? IndexOf(string name)
    {
        var digits = name.Length - name.Reverse().TakeWhile(char.IsAsciiDigit).Count();
        return digits < name.Length && int.TryParse(name.AsSpan(digits), NumberStyles.None, CultureInfo.InvariantCulture, out var index) ? index : null;
    }
}

/// <summary>
/// Problem z kontroly nastaveni INISSu - opravi sa v okne Nastavenia INISSu, potom sa kontrola zopakuje.
/// </summary>
internal sealed class InissProblem(InissFinding finding, string grafikon, AnalysisScope scope) : IProblem
{
    public string Text => finding.Text;

    public string Solution => finding.Solution;

    public ProblemType ProblemType => finding.Type;

    public FixType FixType => FixType.Manual;

    // nastavenia INISSu sa zapisuju hned v ich okne, nie s grafikonom
    public bool ChangesGrafikon => false;

    public FixResult FixProblem()
    {
        scope.Host.ShowInissSettings(finding.Section);
        if (scope.InissLoader?.Invoke() is not { } view) return FixResult.Done;
        return InissChecks.Evaluate(grafikon, view).Any(f => f.Key == finding.Key) ? FixResult.NotSolved : FixResult.Done;
    }
}
