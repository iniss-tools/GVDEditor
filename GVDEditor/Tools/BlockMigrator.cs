using System.Globalization;
using System.Text.RegularExpressions;
using GVDEditor.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Jeden blok grafikonu uvedeny riadkom <c>/&lt;cislo stanice&gt;</c> v CSV suboroch priecinka GVD.
/// </summary>
/// <param name="Index">Poradie bloku v subore (od 1).</param>
/// <param name="StationId">Cislo stanice z hlavicky bloku.</param>
/// <param name="StationName">Nazov stanice - z Grafikon.txt, ak je to stanica grafikonu, inak zo zvukovej banky.</param>
/// <param name="TrainCount">Pocet datovych riadkov bloku v Export3A.</param>
/// <param name="StartValid">Najskorsi zaciatok platnosti vlaku v bloku (Export3B).</param>
/// <param name="EndValid">Najneskorsi koniec platnosti vlaku v bloku (Export3B).</param>
internal sealed record GvdBlock(int Index, int StationId, string StationName, int TrainCount, DateTime StartValid, DateTime EndValid)
{
    /// <summary>
    ///     Nazov noveho priecinka, do ktoreho sa blok presunie (predvyplneny, pouzivatel ho moze zmenit).
    /// </summary>
    public string DirName { get; set; } = "";
}

/// <summary>
///     Migracia starsieho zapisu grafikonu, v ktorom jeden priecinok obsahoval viac grafikonov za sebou
///     (kazdy uvedeny riadkom <c>/&lt;cislo stanice&gt;</c>), na dnesne rozlozenie - jeden priecinok na grafikon
///     zapisany v DirList.TXT.
/// </summary>
/// <remarks>
///     INISS pri hlavicke bloku zacne cislovat vlaky znova od 1 a vlaky bloku vedie pod stanicou z hlavicky;
///     GVDEditor bloky nerozlisuje, takze by druhy blok prepisal prvy. Migracia rozdeli kazdy subor s hlavickami
///     podla blokov, subory bez hlavicky (tabule, StateDgm, Categori...) skopiruje do kazdeho noveho priecinka
///     a pre kazdy blok vytvori Grafikon.txt s platnostou podla najskorsieho a najneskorsieho datumu v Export3B.
/// </remarks>
internal static class BlockMigrator
{
    /// <summary>Subory a priecinky, ktore si INISS vytvara sam - do novych priecinkov sa nekopiruju.</summary>
    private static readonly string[] RuntimeExtensions = { ".dat", ".log", ".bak", ".err", ".hed" };
    private static readonly string[] RuntimeDirectories = { "_TrStat" };

    private static readonly Regex YearSuffix = new(@"\.\d{4}$", RegexOptions.Compiled);

    /// <summary>
    ///     Zisti, ci priecinok GVD obsahuje bloky, ktore treba rozdelit: viac hlaviciek <c>/</c> v Export3A,
    ///     alebo jedinu hlavicku s inou stanicou, nez je IDSTATION grafikonu.
    /// </summary>
    /// <param name="gvdPath">Priecinok grafikonu.</param>
    /// <param name="gvd">Grafikon.txt priecinka.</param>
    /// <param name="dirName">Nazov priecinka v DirList (zaklad pre nazvy novych priecinkov); prazdny pre grafikon priamo v DATA.</param>
    /// <param name="always">Vratit bloky aj pri jedinom bloku bez hlavicky - grafikon priamo v DATA sa presuva vzdy.</param>
    /// <returns>Zoznam blokov s predvyplnenymi nazvami priecinkov, alebo prazdny zoznam, ak migracia nie je potrebna.</returns>
    public static List<GvdBlock> Analyze(string gvdPath, GVDInfo gvd, string dirName, bool always = false)
    {
        var export3A = Utils.CombinePath(gvdPath, FileConsts.FILE_EXPORT3A)!;
        var export3B = Utils.CombinePath(gvdPath, FileConsts.FILE_EXPORT3B)!;
        if (!File.Exists(export3A))
            return new List<GvdBlock>();

        var blocksA = SplitBlocks(ReadLines(export3A));
        var headers = blocksA.Count(b => b.StationId.HasValue);

        var idStation = int.TryParse(gvd.ThisStation.ID, out var id) ? id : 0;
        if (!always && (headers == 0 || (blocksA.Count == 1 && blocksA[0].StationId == idStation)))
            return new List<GvdBlock>();

        var blocksB = File.Exists(export3B) ? SplitBlocks(ReadLines(export3B)) : new List<RawBlock>();

        var result = new List<GvdBlock>();
        for (var i = 0; i < blocksA.Count; i++)
        {
            var a = blocksA[i];
            var dataRows = a.Lines.Count(IsDataLine);

            var (start, end) = ValidityOf(i < blocksB.Count ? blocksB[i].Lines : new List<string>());
            if (start == DateTime.MinValue)
            {
                start = gvd.StartValidTimeTable;
                end = gvd.EndValidTimeTable;
            }

            var stationId = a.StationId ?? idStation;
            var stationName = stationId == idStation
                ? gvd.ThisStation.Name
                : Station.GetFromID(stationId.ToString(CultureInfo.InvariantCulture)).Name;
            result.Add(new GvdBlock(result.Count + 1, stationId, stationName, dataRows, start, end));
        }

        var baseName = YearSuffix.Replace(dirName, "");
        if (string.IsNullOrWhiteSpace(baseName))
            baseName = SanitizeName(gvd.ThisStation.Name);

        var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var block in result)
        {
            var name = $"{baseName}.{block.EndValid.Year}";
            var candidate = name;
            var n = 2;
            while (used.Contains(candidate) || Directory.Exists(Utils.CombinePath(GlobData.DataDir, candidate)!))
                candidate = $"{name}_{n++}";
            used.Add(candidate);
            block.DirName = candidate;
        }

        return result;
    }

    /// <summary>
    ///     Rozdeli priecinok GVD podla blokov do novych priecinkov v DATA a zapise ich do DirList.TXT
    ///     namiesto povodneho zaznamu. Povodny priecinok ostava na disku nedotknuty.
    /// </summary>
    /// <param name="gvdPath">Priecinok so starym zapisom.</param>
    /// <param name="sourceDir">Zaznam DirList povodneho priecinka (porty a priznaky sa prenesu na nove zaznamy); null, ak v DirList nebol.</param>
    /// <param name="gvd">Grafikon.txt povodneho priecinka.</param>
    /// <param name="blocks">Bloky z <see cref="Analyze"/> s nazvami cielovych priecinkov.</param>
    /// <returns>Nove zaznamy DirList v poradi blokov.</returns>
    public static List<DirList> Migrate(string gvdPath, DirList? sourceDir, GVDInfo gvd, IReadOnlyList<GvdBlock> blocks)
    {
        ValidateNames(blocks);

        var targets = blocks.Select(b => Utils.CombinePath(GlobData.DataDir, b.DirName)!).ToList();
        var created = new List<string>();

        try
        {
            foreach (var target in targets)
            {
                Directory.CreateDirectory(target);
                created.Add(target);
            }

            foreach (var file in Directory.GetFiles(gvdPath))
            {
                var name = Path.GetFileName(file);
                if (IsRuntimeFile(name)
                    || name.Equals(FileConsts.FILE_GRAFIKON, StringComparison.OrdinalIgnoreCase)
                    || name.Equals(FileConsts.FILE_AUDIO, StringComparison.OrdinalIgnoreCase)
                    || name.Equals(FileConsts.FILE_DIRLIST, StringComparison.OrdinalIgnoreCase))
                    continue;

                var lines = ReadLines(file);

                // TTexts.TXT hlavicky nema, ale TRAIN_nnn_ID je index vlaku v celom subore (cez vsetky bloky)
                if (name.Equals(FileConsts.FILE_TTEXTS, StringComparison.OrdinalIgnoreCase))
                {
                    var perBlock = SplitTTexts(lines, blocks);
                    for (var i = 0; i < blocks.Count; i++)
                        WriteLines(Utils.CombinePath(targets[i], name)!, perBlock[i]);
                    continue;
                }

                var raw = SplitBlocks(lines);
                var headerCount = raw.Count(b => b.StationId.HasValue);

                if (headerCount == 0)
                {
                    foreach (var target in targets)
                        File.Copy(file, Utils.CombinePath(target, name)!, true);
                    continue;
                }

                if (raw.Count != blocks.Count)
                    throw new InvalidDataException(string.Format(Properties.Resources.BlockMigrator_Pocet_blokov_nesedi, name, raw.Count, blocks.Count));

                for (var i = 0; i < blocks.Count; i++)
                    WriteLines(Utils.CombinePath(targets[i], name)!, raw[i].Lines);
            }

            // podpriecinky (pisma tabul) - okrem runtime dat, inych grafikonov a prave vytvorenych cielov
            // (pri grafikone priamo v DATA lezia ciele v tom istom priecinku)
            foreach (var dir in Directory.GetDirectories(gvdPath))
            {
                var name = Path.GetFileName(dir);
                if (RuntimeDirectories.Contains(name, StringComparer.OrdinalIgnoreCase)
                    || targets.Any(t => string.Equals(Path.GetFullPath(t), Path.GetFullPath(dir), StringComparison.OrdinalIgnoreCase))
                    || File.Exists(Utils.CombinePath(dir, FileConsts.FILE_GRAFIKON)))
                    continue;
                foreach (var target in targets)
                    Utils.CopyDirectory(dir, Utils.CombinePath(target, name)!);
            }

            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                var info = gvd with
                {
                    ThisStation = new Station(block.StationId.ToString(CultureInfo.InvariantCulture), block.StationName),
                    TrainCount = block.TrainCount,
                    StartValidTimeTable = block.StartValid,
                    EndValidTimeTable = block.EndValid,
                    StartValidData = block.StartValid,
                    EndValidData = block.EndValid,
                    CreateData = block.StartValid
                };
                TxtParser.WriteInfoGVD(targets[i], info);
            }
        }
        catch
        {
            foreach (var dir in created)
                try { Directory.Delete(dir, true); }
                catch (Exception e) { Log.Exception(e); }
            throw;
        }

        var newDirs = blocks.Select(b => new DirList
        {
            DirName = b.DirName,
            FullPath = Utils.CombinePath(GlobData.DataDir, b.DirName)!,
            TablePort = sourceDir?.TablePort,
            ReportPort = sourceDir?.ReportPort,
            Flags = sourceDir?.Flags,
            BackColor = sourceDir?.BackColor
        }).ToList();

        var dirList = File.Exists(Utils.CombinePath(GlobData.DataDir, FileConsts.FILE_DIRLIST)) ? TxtParser.ReadDirList() : new List<DirList>();
        var position = sourceDir is null ? -1 : dirList.FindIndex(d => d.DirName.Equals(sourceDir.DirName, StringComparison.OrdinalIgnoreCase));
        if (position >= 0)
            dirList.RemoveAt(position);
        else
            position = dirList.Count;
        dirList.InsertRange(position, newDirs);
        TxtParser.WriteDirList(dirList);

        Log.Info($"Grafikon {gvdPath} rozdelený na {blocks.Count} priečinkov: {string.Join(", ", blocks.Select(b => b.DirName))}");
        return newDirs;
    }

    private static void ValidateNames(IReadOnlyList<GvdBlock> blocks)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var block in blocks)
        {
            var name = block.DirName.Trim();
            if (name.Length == 0 || name.IndexOfAny(invalid) >= 0 || name is "." or "..")
                throw new ArgumentException(string.Format(Properties.Resources.BlockMigrator_Neplatny_nazov_priecinka, block.DirName));
            if (!seen.Add(name))
                throw new ArgumentException(string.Format(Properties.Resources.BlockMigrator_Duplicitny_nazov_priecinka, name));
            if (Directory.Exists(Utils.CombinePath(GlobData.DataDir, name)!))
                throw new ArgumentException($"{name}: {Properties.Resources.Priečinok_s_týmto_názvom_už_existuje__Zmeňte_jeho_názov}");
            block.DirName = name;
        }
    }

    private sealed record RawBlock(int? StationId, List<string> Lines);

    /// <summary>
    ///     Rozdeli riadky suboru podla hlaviciek <c>/N</c>; hlavicka sama sa do riadkov bloku nezapisuje.
    ///     Riadky pred prvou hlavickou tvoria blok bez stanice (INISS ich vedie pod IDSTATION); ak su to len
    ///     komentare a prazdne riadky, pripoja sa k prvemu bloku s hlavickou.
    /// </summary>
    private static List<RawBlock> SplitBlocks(IEnumerable<string> lines)
    {
        var blocks = new List<RawBlock>();
        var current = new RawBlock(null, new List<string>());

        foreach (var line in lines)
        {
            if (line.StartsWith('/'))
            {
                blocks.Add(current);
                int.TryParse(line.AsSpan(1).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var station);
                current = new RawBlock(station, new List<string>());
                continue;
            }

            current.Lines.Add(line);
        }

        blocks.Add(current);

        if (blocks.Count > 1 && !blocks[0].StationId.HasValue && !blocks[0].Lines.Any(IsDataLine))
        {
            blocks[1].Lines.InsertRange(0, blocks[0].Lines);
            blocks.RemoveAt(0);
        }

        return blocks;
    }

    private static readonly Regex TrainEntry = new(@"^TRAIN_(\d+)_(ID|TEXT|IDX_FONT)\s*=\s*(.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    ///     Rozdeli TTexts.TXT podla blokov: v kazdej sekcii [TEXT_nnn] ostanu len vlaky daneho bloku,
    ///     ich index sa zmensi o zakladnu bloku a precisluju sa od TRAIN_001; COUNT sekcie sa prepocita.
    /// </summary>
    private static List<List<string>> SplitTTexts(List<string> lines, IReadOnlyList<GvdBlock> blocks)
    {
        var bases = new int[blocks.Count];
        for (var i = 1; i < blocks.Count; i++)
            bases[i] = bases[i - 1] + blocks[i - 1].TrainCount;

        var outputs = blocks.Select(_ => new List<string>()).ToList();
        var section = new List<string>();
        var inText = false;

        foreach (var line in lines.Append("[END]"))
        {
            if (line.TrimStart().StartsWith('['))
            {
                if (inText)
                    FlushTextSection(section, blocks, bases, outputs);
                else
                    foreach (var output in outputs)
                        output.AddRange(section);

                section = new List<string>();
                inText = line.Trim().StartsWith("[TEXT_", StringComparison.OrdinalIgnoreCase);
                if (line != "[END]")
                    section.Add(line);
                continue;
            }

            section.Add(line);
        }

        return outputs;
    }

    private static void FlushTextSection(List<string> section, IReadOnlyList<GvdBlock> blocks, int[] bases, List<List<string>> outputs)
    {
        var others = new List<string>();
        var trains = new SortedDictionary<int, List<(string key, string value)>>();
        var countLine = -1;

        foreach (var line in section)
        {
            var m = TrainEntry.Match(line.Trim());
            if (m.Success)
            {
                var n = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                if (!trains.TryGetValue(n, out var entry))
                    trains[n] = entry = new List<(string, string)>();
                entry.Add((m.Groups[2].Value.ToUpperInvariant(), m.Groups[3].Value));
                continue;
            }

            if (line.TrimStart().StartsWith("COUNT=", StringComparison.OrdinalIgnoreCase) && countLine < 0)
                countLine = others.Count;
            others.Add(line);
        }

        while (others.Count > 0 && string.IsNullOrWhiteSpace(others[^1]))
            others.RemoveAt(others.Count - 1);

        for (var b = 0; b < blocks.Count; b++)
        {
            var from = bases[b] + 1;
            var to = bases[b] + blocks[b].TrainCount;
            var selected = trains.Values
                .Where(e => e.Any(kv => kv.key == "ID" && int.TryParse(kv.value.Trim(), out var id) && id >= from && id <= to))
                .ToList();

            var output = outputs[b];
            for (var i = 0; i < others.Count; i++)
                output.Add(i == countLine ? $"COUNT={selected.Count}" : others[i]);
            if (countLine < 0)
                output.Add($"COUNT={selected.Count}");

            for (var i = 0; i < selected.Count; i++)
                foreach (var (key, value) in selected[i])
                {
                    var v = key == "ID" ? (int.Parse(value.Trim(), CultureInfo.InvariantCulture) - bases[b]).ToString(CultureInfo.InvariantCulture) : value;
                    output.Add($"TRAIN_{i + 1:000}_{key}={v}");
                }

            output.Add("");
        }
    }

    private static (DateTime start, DateTime end) ValidityOf(IEnumerable<string> export3BLines)
    {
        var start = DateTime.MaxValue;
        var end = DateTime.MinValue;

        foreach (var line in export3BLines.Where(IsDataLine))
        {
            var cols = line.Split(',');
            if (cols.Length < 3)
                continue;
            try
            {
                var s = Utils.ParseDateAlts(cols[1].Trim());
                var e = Utils.ParseDateAlts(cols[2].Trim());
                if (s < start) start = s;
                if (e > end) end = e;
            }
            catch (FormatException)
            {
                // riadok bez datumu - platnost urcia ostatne
            }
        }

        return start == DateTime.MaxValue ? (DateTime.MinValue, DateTime.MinValue) : (start, end);
    }

    private static bool IsDataLine(string line)
        => !string.IsNullOrWhiteSpace(line) && !line.StartsWith(';') && !line.StartsWith('/');

    private static bool IsRuntimeFile(string name)
        => RuntimeExtensions.Contains(Path.GetExtension(name), StringComparer.OrdinalIgnoreCase);

    private static string SanitizeName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var chars = name.Where(c => !invalid.Contains(c) && !char.IsWhiteSpace(c)).ToArray();
        return chars.Length == 0 ? "GVD" : new string(chars);
    }

    private static List<string> ReadLines(string file) => File.ReadAllLines(file, Encodings.Win1250).ToList();

    private static void WriteLines(string file, IEnumerable<string> lines) => File.WriteAllLines(file, lines, Encodings.Win1250);
}
