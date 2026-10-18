using System.Diagnostics;
using System.Globalization;
using GVDEditor.Properties;
using Iniss.PlanRadenia;

namespace GVDEditor.Integration;

/// <summary>
/// Spustenie pomocneho programu PlanRadeniaReader, ktory cita plan radenia vlakov ZSSK (PDF s obrazkami stran) -
/// vykreslenie PDF a rozpoznavanie textu su rozhrania Windows, preto bezi v samostatnom procese.
/// </summary>
internal static class PlanRadeniaReaderClient
{
    private const string ExeName = "PlanRadeniaReader.exe";
    private const int ExitStationNotFound = 2;
    private const int ExitOcrMissing = 3;

    /// <summary>
    /// Stanice planu radenia: cislo strany a nazov stanice v hlavicke strany.
    /// </summary>
    /// <param name="pdf">Subor planu.</param>
    /// <param name="cancel">Zrusenie - ukonci aj pomocny program.</param>
    /// <exception cref="InvalidOperationException">program zlyhal; sprava je urcena pouzivatelovi</exception>
    public static async Task<List<(int Page, string Station)>> ListStationsAsync(string pdf, CancellationToken cancel)
    {
        var output = await RunAsync(["--pdf", pdf, "--list-stations"], null, cancel).ConfigureAwait(false);
        var stations = new List<(int, string)>();
        foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = line.Split('\t', 2);
            if (parts.Length == 2 && int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var page))
                stations.Add((page, parts[1]));
        }

        return stations;
    }

    /// <summary>
    /// Precita vlaky na stranach planu.
    /// </summary>
    /// <param name="pdf">Subor planu.</param>
    /// <param name="pages">Strany (od 1).</param>
    /// <param name="stations">Nazvy stanic, z ktorych sa vybera stanica v poznamkach pod radenim.</param>
    /// <param name="progress">Precitana strana.</param>
    /// <param name="cancel">Zrusenie - ukonci aj pomocny program.</param>
    /// <exception cref="InvalidOperationException">program zlyhal; sprava je urcena pouzivatelovi</exception>
    public static async Task<PlanResult> ReadAsync(string pdf, IReadOnlyList<int> pages, IEnumerable<string> stations, IProgress<int>? progress,
        CancellationToken cancel)
    {
        var folder = Path.Combine(Path.GetTempPath(), "GVDEditor-plan-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            var stationFile = Path.Combine(folder, "stanice.txt");
            var result = Path.Combine(folder, "plan.xml");
            await File.WriteAllLinesAsync(stationFile, stations.Distinct(StringComparer.Ordinal), cancel).ConfigureAwait(false);
            var merged = new PlanResult();
            // strany jednej stanice idu za sebou - jeden beh na suvisly usek
            foreach (var range in Ranges(pages))
            {
                await RunAsync(["--pdf", pdf, "--pages", $"{range.From}-{range.To}", "--stations", stationFile, "--out", result], progress, cancel)
                    .ConfigureAwait(false);
                var part = PlanResult.Load(result);
                if (merged.ValidFrom.Length == 0)
                    (merged.ValidFrom, merged.ValidTo) = (part.ValidFrom, part.ValidTo);
                foreach (var station in part.Stations)
                {
                    var existing = merged.Stations.FirstOrDefault(s => s.Name == station.Name);
                    if (existing is null)
                        merged.Stations.Add(station);
                    else
                    {
                        existing.Pages.AddRange(station.Pages);
                        existing.Trains.AddRange(station.Trains);
                    }
                }
            }

            return merged;
        }
        finally
        {
            try
            {
                Directory.Delete(folder, true);
            }
            catch (IOException)
            {
                // docasny priecinok ostane - nevadi
            }
        }
    }

    private static IEnumerable<(int From, int To)> Ranges(IReadOnlyList<int> pages)
    {
        var sorted = pages.Distinct().Order().ToList();
        for (var i = 0; i < sorted.Count;)
        {
            var j = i;
            while (j + 1 < sorted.Count && sorted[j + 1] == sorted[j] + 1)
                j++;
            yield return (sorted[i], sorted[j]);
            i = j + 1;
        }
    }

    /// <summary>Spusti program a vrati jeho standardny vystup; priebeh cita zo standardneho chyboveho vystupu.</summary>
    private static async Task<string> RunAsync(string[] arguments, IProgress<int>? progress, CancellationToken cancel)
    {
        var exe = Path.Combine(AppContext.BaseDirectory, ExeName);
        if (!File.Exists(exe))
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_ProgramMissing, ExeName));

        var info = new ProcessStartInfo(exe)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8
        };
        foreach (var argument in arguments)
            info.ArgumentList.Add(argument);

        using var process = Process.Start(info)
                            ?? throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_ProgramMissing, ExeName));
        var errors = new System.Text.StringBuilder();
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null)
                return;
            // "strana 12: Kuty, vlakov 8"
            if (e.Data.StartsWith("strana ", StringComparison.Ordinal) &&
                int.TryParse(e.Data.AsSpan(7, Math.Max(0, e.Data.IndexOf(':', StringComparison.Ordinal) - 7)), NumberStyles.Integer, CultureInfo.InvariantCulture,
                    out var page))
                progress?.Report(page);
            else if (!e.Data.StartsWith("  ", StringComparison.Ordinal))
                lock (errors)
                    errors.AppendLine(e.Data);
        };
        process.BeginErrorReadLine();
        string output;
        try
        {
            output = await process.StandardOutput.ReadToEndAsync(cancel).ConfigureAwait(false);
            await process.WaitForExitAsync(cancel).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            try
            {
                process.Kill(true);
            }
            catch (InvalidOperationException)
            {
                // uz skoncil
            }

            throw;
        }

        string detail;
        lock (errors)
            detail = errors.ToString().Trim();
        return process.ExitCode switch
        {
            0 => output,
            ExitOcrMissing => throw new InvalidOperationException(Resources.PlanRadenia_OcrMissing),
            ExitStationNotFound => throw new InvalidOperationException(Resources.PlanRadenia_StationNotFound),
            _ => throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.PlanRadenia_ReaderFailed, detail))
        };
    }
}
