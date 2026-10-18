namespace Iniss.PlanRadenia;

/// <summary>
/// Poznamky pod radenim vlaku: riadky textu pod poslednym riadkom vozidiel. Kazda poznamka zacina odrazkou,
/// riadok bez odrazky pokracuje predoslou poznamkou.
/// </summary>
internal sealed class NoteReader(NoteFont font, NoteDecoder decoder)
{
    /// <summary>Najvyssi pas s jednym riadkom textu; vyssi pas su dva riadky bez medzery medzi nimi.</summary>
    private const int MaxLineHeight = 13;

    /// <summary>Vyska riadku poznamky pri deleni vyssieho pasu.</summary>
    private const int LineHeight = 12;

    /// <summary>Priemerna odchylka od vzorov na znak, od ktorej je riadok neisty (ukaze sa na kontrolu).</summary>
    public const double UncertainCost = 0.35;

    /// <summary>Roztiahnutia useku s nazvom stanice pri porovnani s kandidatmi.</summary>
    private static readonly double[] StretchFactors = [1.0, 1.05, 1.1, 1.15];

    /// <summary>
    /// Najvacsia odchylka na znak, pri ktorej sa nazov stanice este prevezme z kandidatov (spravne nazvy maju do 0,55,
    /// nespravne od 0,7).
    /// </summary>
    private const double StationCost = 0.62;

    /// <summary>
    /// To iste pri stanici, v ktorej sa vozne pridavaju - veta je vysadzana vacsim pismom, ktoremu vzory sedia horsie.
    /// </summary>
    private const double JoinedStationCost = 0.5;

    /// <summary>Nazvy stanic, z ktorych sa vybera ciel vozna (podla pisma, nie podla precitaneho textu).</summary>
    public List<string> Stations { get; } = [];

    /// <summary>
    /// Ci sa k nazvom stanic pridavaju vychodiskove a cielove stanice citanych vlakov - len bez zoznamu stanic
    /// (nazvy z hlaviciek vlakov su z OCR a obcas skomolene).
    /// </summary>
    public bool AddTrainStations { get; set; }

    public List<PlanNote> Read(Raster page, IEnumerable<Box> bands, PlanTrain train)
    {
        var notes = new List<(PlanNote Note, List<NoteLine> Lines, List<NoteReading> Readings)>();
        foreach (var band in bands.Where(b => !IsWheels(page, b)).SelectMany(b => Split(page, b)))
        {
            if (NoteLine.Create(page, band, font) is not { } line)
                continue;
            var reading = decoder.Read(line);
            if (reading.Cells.Count == 0)
                continue;
            if (reading.Cells[0].Char != '•' && notes.Count > 0)
            {
                notes[^1].Lines.Add(line);
                notes[^1].Readings.Add(reading);
            }
            else
                notes.Add((new PlanNote(), [line], [reading]));
        }

        var result = new List<PlanNote>();
        foreach (var (note, lines, readings) in notes)
        {
            var texts = new List<string>();
            for (var i = 0; i < lines.Count; i++)
                texts.Add(WithStation(lines[i], readings[i], note));
            var text = string.Join(" ", texts).Replace('%', '⚒');
            var parsed = NoteParser.Parse(text);
            note.Text = parsed.Text;
            note.Kind = parsed.Kind;
            note.Cars = parsed.Cars;
            note.Rest = parsed.Rest;
            note.Station = parsed.Station;
            note.Score = Math.Round(readings.Max(r => r.Cost), 3);
            var area = lines.Select(l => l.Band).Aggregate((a, b) => Box.FromEdges(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Max(a.Right, b.Right), Math.Max(a.Bottom, b.Bottom)));
            note.Area = area.ToString();
            SnapCars(note, train);
            result.Add(note);
        }

        return result;
    }

    /// <summary>Pas kolies pod vozidlami: podvozky su siroke tmave behy, pismena su tenke.</summary>
    private static bool IsWheels(Raster page, Box band)
    {
        for (var y = band.Y; y < band.Bottom; y++)
        {
            var run = 0;
            for (var x = band.X; x < band.Right; x++)
            {
                run = NoteLine.Darkness(page[x, y]) > 155 ? run + 1 : 0;
                if (run > 12)
                    return true;
            }
        }

        return false;
    }

    /// <summary>Vyssi pas (dva riadky tesne pod sebou) rozdeli v najsvetlejsich riadkoch.</summary>
    private static IEnumerable<Box> Split(Raster page, Box band)
    {
        if (band.Height <= MaxLineHeight)
        {
            yield return band;
            yield break;
        }

        var ink = new int[band.Height];
        for (var y = band.Y; y < band.Bottom; y++)
            for (var x = band.X; x < band.Right; x++)
                if (NoteLine.Darkness(page[x, y]) > 55)
                    ink[y - band.Y]++;
        var count = (int)Math.Round(band.Height / (double)LineHeight);
        var cuts = new List<int> { 0 };
        for (var k = 1; k < count; k++)
        {
            var center = (int)Math.Round(k * band.Height / (double)count);
            var best = Enumerable.Range(center - 3, 7).Where(r => r > 0 && r < band.Height).MinBy(r => ink[r]);
            cuts.Add(best);
        }

        cuts.Add(band.Height);
        for (var i = 0; i + 1 < cuts.Count; i++)
            yield return new Box(band.X, band.Y + cuts[i], band.Width, cuts[i + 1] - cuts[i]);
    }

    /// <summary>
    /// Text riadku; nazov stanice (za pomlckou pri cieli vozna, za „ŽST“ pri vozni pridanom v stanici) sa vyberie
    /// z <see cref="Stations" /> podla zhody s obrazkom - volne citanie nazvov je nespolahlive.
    /// </summary>
    private string WithStation(NoteLine line, NoteReading reading, PlanNote note)
    {
        if (Stations.Count == 0 || StationStart(reading) is not { } start)
            return reading.Text;

        var cells = reading.Cells;
        // nazov stanice siaha po koniec riadku; bodka na konci moze patrit nazvu (Praha hl.n.)
        var end = cells.Count;
        int from = cells[start].X, to = cells[end - 1].X + cells[end - 1].Width;
        // nazvy stanic su v plane vysadzane o nieco hustejsie nez zvysok poznamky - porovnava sa aj s roztiahnutym usekom
        var slice = line.Slice(from - 2, to + 3);
        var best = (Name: (string?)null, Cost: double.MaxValue);
        foreach (var factor in StretchFactors)
        {
            var stretched = factor == 1 ? slice : slice.Stretch(factor);
            var cache = new Dictionary<char, List<(int Width, float[] Costs)>>();
            foreach (var name in Stations)
            {
                var expected = ExpectedWidth(name);
                if (expected < stretched.Width * 0.7 || expected > stretched.Width * 1.4)
                    continue;
                var cost = decoder.Match(stretched, name, cache);
                if (cost < best.Cost)
                    best = (name, cost);
            }
        }

        var joined = cells.Take(start).Any(c => c.Char == 'Ž');
        if (best.Name is null || best.Cost > (joined ? JoinedStationCost : StationCost))
            return reading.Text;

        note.Station = best.Name;
        note.StationScore = Math.Round(best.Cost, 3);
        // text pred nazvom stanice z citania, nazov z kandidata; veta o pridani vozni konci bodkou
        var prefix = NoteDecoder.TextOf(cells.Take(start));
        var suffix = prefix.Contains('Ž', StringComparison.Ordinal) && !best.Name.EndsWith('.') ? "." : "";
        return $"{prefix} {best.Name}{suffix}";
    }

    /// <summary>Index prveho znaku nazvu stanice v riadku, alebo <see langword="null" />.</summary>
    private static int? StationStart(NoteReading reading)
    {
        var cells = reading.Cells;
        var text = new string(cells.Select(c => c.Char).ToArray());
        // pridanie vozna v stanici: … vo vlaku od ŽST Humenné. - nazov je prvy token za „ŽST“
        var zst = text.LastIndexOf('Ž');
        if (text.StartsWith("•Voz", StringComparison.Ordinal) || text.StartsWith("Voz", StringComparison.Ordinal))
        {
            if (zst < 0)
                return null;
            // „ŽST“ ma okolo 18 bodov - nazov zacina prvym tokenom za nim (T sa obcas precita ako 7)
            var after = cells[zst].X + 16;
            var i = zst + 1;
            while (i < cells.Count && (!cells[i].SpaceBefore || cells[i].X < after))
                i++;
            return i < cells.Count ? i : null;
        }

        // ciel vozna: č. 344 - Krakow Glowny (za pomlckou nie je ide/nejde ani cislo)
        var dash = text.IndexOf('-');
        if (dash < 0 || dash + 1 >= cells.Count)
            return null;
        var rest = text[(dash + 1)..];
        if (rest.StartsWith("ide", StringComparison.Ordinal) || rest.StartsWith("nejde", StringComparison.Ordinal) ||
            rest.StartsWith("okrem", StringComparison.Ordinal) || char.IsDigit(rest[0]) || NoteGrammar.Symbols.Contains(rest[0]))
            return null;
        return dash + 1;
    }

    /// <summary>Ocakavana sirka textu v bodoch podla vzorov (medzera 3 body).</summary>
    private double ExpectedWidth(string text)
    {
        double width = 0;
        foreach (var c in text)
        {
            if (c == ' ')
                width += 3;
            else if (font.ByChar.TryGetValue(c, out var glyphs) || font.ByChar.TryGetValue(Names.WithoutDiacritics(c.ToString())[0], out glyphs))
                width += glyphs.Average(g => g.Width);
            else
                width += 6;
        }

        return width;
    }

    /// <summary>
    /// Cislo vozna z poznamky, ktore v radeni nie je, sa nahradi cislom vozna, od ktoreho sa lisi jednou cislicou
    /// (pri drobnom pisme sa najcastejsie zamenia 0/6/8/9 a 3/8) - ak je taky prave jeden.
    /// </summary>
    private static void SnapCars(PlanNote note, PlanTrain train)
    {
        var numbers = train.Vehicles.Where(v => v.Number.Length > 0).Select(v => v.Number).ToHashSet();
        if (numbers.Count == 0)
            return;
        for (var i = 0; i < note.Cars.Count; i++)
        {
            var car = note.Cars[i];
            if (car.StartsWith('#') || numbers.Contains(car))
                continue;
            var close = numbers.Where(n => n.Length == car.Length && n.Zip(car).Count(p => p.First != p.Second) == 1).ToList();
            if (close.Count == 1)
            {
                note.Warnings.Add($"{PlanWarnings.CarFixed} {car} {close[0]}");
                note.Cars[i] = close[0];
            }
            else
                note.Warnings.Add($"{PlanWarnings.CarMissing} {car}");
        }
    }
}
