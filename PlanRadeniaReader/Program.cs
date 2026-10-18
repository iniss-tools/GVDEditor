using System.Globalization;
using System.Text;

namespace Iniss.PlanRadenia;

internal static class Program
{
    private const string Usage =
        """
        PlanRadeniaReader - citanie planu radenia vlakov ZSSK (PDF s obrazkami stran)

          PlanRadeniaReader.exe --pdf <subor> --station <nazov> [--out <subor.xml>]
          PlanRadeniaReader.exe --pdf <subor> --list-stations
          PlanRadeniaReader.exe --pdf <subor> --pages <od-do> [--out <subor.xml>] [--dump-glyphs <subor>]

        --station      nazov stanice (bez diakritiky, bodiek a medzier sa porovnava volne)
        --pages        citat strany v rozsahu (od 1), napr. 5-8
        --out          XML s vysledkom (bez neho ide prehlad na standardny vystup)
        --dump-glyphs  zapise vsetky znaky cisel vozni (na ucenie vzorov cislic)
        --debug        priecinok na obrazky stran a vypis rozlozenia (ladenie)
        --dump-icons   zapise piktogramy legend (na zostavenie katalogu piktogramov)
        --dump-notes   zapise polohy riadkov poznamok (na ucenie vzorov pisma poznamok)
        --stations     subor s nazvami stanic (riadok = nazov) - z nich sa vybera stanica v poznamkach

        Navratove kody: 0 = ok, 1 = chyba, 2 = stanica sa v plane nenachadza, 3 = chyba OCR pre slovencinu
        """;

    private static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        string? pdf = null, station = null, output = null, pages = null, dump = null, debug = null, icons = null, notesDump = null, stations = null;
        var list = false;
        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "--pdf": pdf = Next(args, ref i); break;
                case "--station": station = Next(args, ref i); break;
                case "--out": output = Next(args, ref i); break;
                case "--pages": pages = Next(args, ref i); break;
                case "--dump-glyphs": dump = Next(args, ref i); break;
                case "--debug": debug = Next(args, ref i); break;
                case "--dump-icons": icons = Next(args, ref i); break;
                case "--dump-notes": notesDump = Next(args, ref i); break;
                case "--stations": stations = Next(args, ref i); break;
                case "--list-stations": list = true; break;
                case "--help" or "-h":
                    Console.WriteLine(Usage);
                    return 0;
                default:
                    Console.Error.WriteLine($"Neznámy prepínač: {args[i]}");
                    Console.Error.WriteLine(Usage);
                    return 1;
            }

        if (pdf is null || (!list && station is null && pages is null))
        {
            Console.Error.WriteLine(Usage);
            return 1;
        }

        TextRecognizer ocr;
        try
        {
            ocr = TextRecognizer.Create();
        }
        catch (InvalidOperationException)
        {
            Console.Error.WriteLine("Windows nemá nainštalované rozpoznávanie textu (OCR) pre slovenčinu.");
            return 3;
        }

        var source = await PdfSource.OpenAsync(pdf);
        var digits = DigitReader.Load();
        var noteFont = NoteFont.Load();
        var notes = new NoteReader(noteFont, new NoteDecoder(noteFont));
        if (stations is not null)
            notes.Stations.AddRange(File.ReadAllLines(stations).Select(l => l.Trim()).Where(l => l.Length > 0).Distinct());
        else
            notes.AddTrainStations = true;
        var vehicles = new VehicleReader(digits, notes);
        if (dump is not null)
            vehicles.GlyphSink = [];
        if (notesDump is not null)
            vehicles.NoteSink = [];
        var reader = new PageReader(ocr, vehicles, Legend.LoadCatalog());
        var result = new PlanResult();

        // strany stanice: najprv sa citaju len hlavicky
        var selected = new List<int>();
        if (pages is not null)
        {
            var parts = pages.Split('-');
            var from = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var to = parts.Length > 1 ? int.Parse(parts[1], CultureInfo.InvariantCulture) : from;
            for (var p = from; p <= Math.Min(to, source.PageCount); p++)
                selected.Add(p);
        }
        else
        {
            for (var p = 1; p <= source.PageCount; p++)
            {
                var page = await source.RenderAsync(p - 1);
                if (await reader.ReadHeaderAsync(page) is not { } header)
                    continue;
                if (list)
                    Console.WriteLine($"{p}\t{header.Header.Station}");
                else if (Names.Same(header.Header.Station, station!))
                    selected.Add(p);
            }

            if (list)
                return 0;
            if (selected.Count == 0)
            {
                Console.Error.WriteLine($"Stanica \"{station}\" sa v pláne radenia nenachádza.");
                return 2;
            }
        }

        foreach (var p in selected)
        {
            var page = await source.RenderAsync(p - 1);
            if (debug is not null)
            {
                await WinImaging.SavePngAsync(page, Path.Combine(debug, $"page{p}.png"));
                if (PageLayout.Find(page) is { } layout)
                    Console.Error.WriteLine($"strana {p}: ciary {string.Join(",", layout.Rules)} stlpec {layout.Column} spodok {layout.Bottom}");
            }

            if (icons is not null && PageLayout.Find(page) is { } iconLayout)
            {
                var legendArea = PageLayout.Legend(page, iconLayout.Bottom);
                var words = await ocr.ReadAsync(page, legendArea, 3, words: true);
                var boxes = Legend.IconBoxes(page, legendArea);
                await File.AppendAllLinesAsync(icons, boxes.Select(b =>
                    $"{p}	{b}	{string.Join(" ", words.Where(w => Math.Abs(w.Box.CenterY - b.CenterY) < b.Height / 2 && w.Box.X >= b.Right - 2 && w.Box.X < b.Right + 400).OrderBy(w => w.Box.X).Select(w => w.Text))}	{string.Join(",", Legend.Sample(page, b).Select(v => ((int)v).ToString(CultureInfo.InvariantCulture)))}"));
            }

            if (debug is not null)
                vehicles.Overlay = page.Crop(new Box(0, 0, page.Width, page.Height));
            if (await reader.ReadAsync(page, p) is not { } content)
                continue;
            if (vehicles.Overlay is { } overlay)
                await WinImaging.SavePngAsync(overlay, Path.Combine(debug!, $"overlay{p}.png"));

            if (result.ValidFrom.Length == 0)
                (result.ValidFrom, result.ValidTo) = (content.Header.ValidFrom, content.Header.ValidTo);
            var planStation = result.Stations.FirstOrDefault(s => s.Name == content.Header.Station);
            if (planStation is null)
            {
                planStation = new PlanStation { Name = content.Header.Station };
                result.Stations.Add(planStation);
            }

            planStation.Pages.Add(p);
            planStation.Trains.AddRange(content.Trains);
            Console.Error.WriteLine($"strana {p}: {content.Header.Station}, vlakov {content.Trains.Count}");
            foreach (var (row, text) in content.Skipped)
                Console.Error.WriteLine($"  bez vlaku {row}: {text}");
        }

        if (dump is not null)
            await File.WriteAllLinesAsync(dump, vehicles.GlyphSink!.Select(g => $"{g.Where}\t{g.Glyph.Pattern}"));

        if (notesDump is not null)
            await File.WriteAllLinesAsync(notesDump, vehicles.NoteSink!.Select(n => $"{n.Where}	{n.Band.X},{n.Band.Y},{n.Band.Width},{n.Band.Height}"));

        if (output is not null)
            result.Save(output);
        else
            WriteSummary(result);
        return 0;
    }

    private static void WriteSummary(PlanResult result)
    {
        Console.WriteLine($"platnosť {result.ValidFrom} - {result.ValidTo}");
        foreach (var station in result.Stations)
        {
            Console.WriteLine($"== {station.Name} (strany {string.Join(",", station.Pages)})");
            foreach (var train in station.Trains)
            {
                Console.WriteLine($"  {train.Type} {train.Number} {train.Name} | {train.Times} | {train.From} -> {train.To} | rušeň {train.Locomotive}");
                Console.WriteLine("    " + string.Join("  ", train.Vehicles.Select(v => v.IsLocomotive ? "[R]"
                    : $"{(v.Number.Length > 0 ? v.Number : "?")}{(v.Line > 1 ? "^" + v.Line : "")}({string.Join("+", v.Colors.Select(c => c.Name))}){(v.Icons.Count > 0 ? "[" + string.Join(";", v.Icons) + "]" : "")}")));
                foreach (var note in train.Notes)
                    Console.WriteLine($"    * [{string.Join(",", note.Cars)}] {note.Rest} | {note.Text}");
                foreach (var warning in train.Warnings)
                    Console.WriteLine($"    ! {warning}");
            }
        }
    }

    private static string Next(string[] args, ref int i) =>
        i + 1 < args.Length ? args[++i] : throw new ArgumentException($"Prepínač {args[i]} vyžaduje hodnotu.");
}

/// <summary>
/// Porovnanie nazvov stanic bez diakritiky, interpunkcie a medzier (OCR a nazvy v zdrojoch sa lisia).
/// </summary>
internal static class Names
{
    public static bool Same(string a, string b) => Canonical(a) == Canonical(b);

    public static string Canonical(string name)
    {
        var decomposed = name.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();
        foreach (var c in decomposed)
            if (char.IsLetterOrDigit(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        return builder.ToString();
    }

    /// <summary>Text bez diakritiky (ł a ß nemaju rozklad - nahradzaju sa zvlast).</summary>
    public static string WithoutDiacritics(string text)
    {
        var builder = new StringBuilder();
        foreach (var c in text.Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c switch { 'ł' => 'l', 'Ł' => 'L', 'ß' => 's', _ => c });
        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
