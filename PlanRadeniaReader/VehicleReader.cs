namespace Iniss.PlanRadenia;

/// <summary>
/// Cita radenie v pravej casti riadku vlaku: riadky vozidiel (pokracovanie je pod prvym riadkom), rusen, vozne,
/// ich cisla, farby karoserie, piktogramy a poznamky pod radenim.
/// </summary>
internal sealed class VehicleReader(DigitReader digits, NoteReader notes)
{
    /// <summary>Najmensia sirka vozidla - uzsie behy su narazniky a spriahla.</summary>
    private const int MinVehicleWidth = 40;

    /// <summary>
    /// Vyska oblasti cisla nad strechou - cislice maju nad strechou 9-10 bodov; vyssie su uz kolesa riadku nad
    /// pokracovanim radenia.
    /// </summary>
    private const int NumberHeight = 12;

    /// <summary>Riadky obrysu strechy pod jej hornym okrajom.</summary>
    private const int RoofRows = 3;

    /// <summary>Najmensia vyska riadku vozidiel; nizsie pasy su riadky poznamok.</summary>
    private const int MinLineHeight = 30;

    /// <summary>Volitelny ladiaci obrazok strany - vyznacia sa vozidla a piktogramy (--debug).</summary>
    public Raster? Overlay { get; set; }

    /// <summary>Volitelny zber znakov cisel na ucenie vzorov (--dump-glyphs).</summary>
    public List<(string Where, Glyph Glyph)>? GlyphSink { get; set; }

    /// <summary>Volitelny zber riadkov poznamok na ucenie vzorov pisma (--dump-notes).</summary>
    public List<(string Where, Box Band)>? NoteSink { get; set; }

    public void Read(Raster page, Box area, PlanTrain train, Legend legend)
    {
        var bands = Bands(page, area);
        var lines = bands.Where(band => band.Height >= MinLineHeight).ToList();
        var lineVehicles = new List<List<Vehicle>>();
        foreach (var line in lines)
            lineVehicles.Add(ReadLine(page, line, legend, train));

        Order(train, lineVehicles);

        // poznamky su pod poslednym riadkom vozidiel (prvy pas pod nim su kolesa)
        var notesTop = lines.Count > 0 ? lines[^1].Bottom + 1 : area.Y;
        var noteBands = bands.Where(band => band.Height < MinLineHeight && band.Y >= notesTop).ToList();
        if (NoteSink is { } sink)
            lock (sink)
                sink.AddRange(noteBands.Select(band => ($"{train.Page}	{train.Type} {train.Number}", band)));
        // ciel vozna byva vychodiskova alebo cielova stanica niektoreho vlaku - nazvy z planu su napisane rovnako
        if (notes.AddTrainStations)
            foreach (var name in new[] { train.From, train.To })
                if (name.Length > 2 && !notes.Stations.Contains(name))
                    notes.Stations.Add(name);
        train.Notes.AddRange(notes.Read(page, noteBands, train));
    }

    /// <summary>
    /// Vodorovne pasy s obsahom: riadky vozidiel (aj s cislami nad nimi) a riadky poznamok.
    /// </summary>
    private static List<Box> Bands(Raster page, Box area)
    {
        var bands = new List<Box>();
        int? start = null;
        for (var y = area.Y; y <= area.Bottom; y++)
        {
            var ink = 0;
            if (y < area.Bottom)
                for (var x = area.X; x < area.Right; x++)
                    if (!page[x, y].IsWhite)
                        ink++;
            if (ink > 3)
                start ??= y;
            else if (start is { } s)
            {
                bands.Add(Box.FromEdges(area.X, s, area.Right, y));
                start = null;
            }
        }

        return bands;
    }

    private sealed record Vehicle(PlanVehicle Data, int X);

    /// <summary>
    /// Vozidla jedneho riadku. Vozidla su spojene podvozkom a medzi niektorymi je v strede karoserie nakresleny
    /// prechodovy mostik - oddeluju sa podla strechy: obrys strechy ma kazde vozidlo vlastny a medzi vozidlami
    /// je v riadkoch strechy papier.
    /// </summary>
    private List<Vehicle> ReadLine(Raster page, Box line, Legend legend, PlanTrain train)
    {
        // karoseria = riadky s vacsinou obsahu pasu (cisla nad voznami su riedke)
        var counts = new int[line.Height];
        for (var y = line.Y; y < line.Bottom; y++)
            for (var x = line.X; x < line.Right; x++)
                if (!page[x, y].IsWhite)
                    counts[y - line.Y]++;
        var max = counts.Max();
        var bodyTop = line.Y + Array.FindIndex(counts, c => c > max * 6 / 10);
        var bodyBottom = line.Y + Array.FindLastIndex(counts, c => c > max * 6 / 10);

        var vehicles = new List<Vehicle>();
        var bodies = new List<Box>();
        int? start = null;
        var gap = 0;
        for (var x = line.X; x <= line.Right; x++)
        {
            var ink = false;
            for (var y = bodyTop; y <= bodyTop + RoofRows && x < line.Right; y++)
                ink |= !page[x, y].IsWhite;
            if (ink)
            {
                start ??= x;
                gap = 0;
                continue;
            }

            if (start is not { } s || ++gap < 2)
                continue;

            var right = x - gap + 1;
            if (right - s >= MinVehicleWidth)
                bodies.Add(Box.FromEdges(s, bodyTop, right, bodyBottom + 1));

            start = null;
            gap = 0;
        }

        // vozidla su nezavisle - piktogramy (najpomalsia cast) sa hladaju paralelne
        var read = new PlanVehicle[bodies.Count];
        Parallel.For(0, bodies.Count, i => read[i] = ReadVehicle(page, bodies[i], line, legend, train));
        for (var i = 0; i < bodies.Count; i++)
            vehicles.Add(new Vehicle(read[i], bodies[i].X));
        return vehicles;
    }

    private PlanVehicle ReadVehicle(Raster page, Box body, Box line, Legend legend, PlanTrain train)
    {
        var vehicle = new PlanVehicle { Area = body.ToString() };
        if (Overlay is { } frame)
            lock (frame)
                frame.DrawRect(body.Inflate(1, 1), new Rgb(255, 0, 0));
        vehicle.IsLocomotive = BodyColors.IsLocomotive(page, body);
        if (!vehicle.IsLocomotive)
        {
            vehicle.Colors = BodyColors.Read(page, body);
            var cells = legend.Cells(page, body);
            vehicle.Icons = cells.Select(c => c.Label).ToList();
            if (Overlay is { } overlay)
                lock (overlay)
                    foreach (var cell in cells)
                        overlay.DrawRect(cell.Box, new Rgb(255, 0, 255));
        }

        // cislo vozna je cierne nad strechou a spodok cislic splyva so strechou - cita sa cast nad strechou
        var roof = vehicle.IsLocomotive ? -1 : Glyph.RoofTop(page, body);
        var glyphs = roof < 0 ? [] : Glyph.Extract(page, Box.FromEdges(body.X - 6, Math.Max(line.Y, roof - NumberHeight), body.Right + 6, roof));
        if (glyphs.Count > 0)
        {
            if (GlyphSink is { } sink)
                lock (sink)
                    foreach (var glyph in glyphs)
                        sink.Add(($"{train.Page}:{train.Type} {train.Number}:{body.X}:{glyph.X}", glyph));
            (vehicle.Number, vehicle.NumberScore) = digits.Read(glyphs);
        }

        return vehicle;
    }

    /// <summary>
    /// Zoradi vozidla od rusna. Pokracovanie radenia v dalsom riadku zacina na strane rusna.
    /// </summary>
    private static void Order(PlanTrain train, List<List<Vehicle>> lines)
    {
        if (lines.Count == 0)
            return;

        var first = lines[0];
        var locoRight = first.Count > 0 && first[^1].Data.IsLocomotive && !first[0].Data.IsLocomotive;
        var locoLeft = first.Count > 0 && first[0].Data.IsLocomotive;
        train.Locomotive = locoLeft ? "left" : locoRight ? "right" : "";
        if (!locoLeft && !locoRight)
            train.Warnings.Add(PlanWarnings.NoLocomotive);

        for (var i = 0; i < lines.Count; i++)
        {
            var ordered = locoRight ? lines[i].OrderByDescending(v => v.X) : lines[i].OrderBy(v => v.X);
            foreach (var vehicle in ordered)
            {
                vehicle.Data.Line = i + 1;
                train.Vehicles.Add(vehicle.Data);
            }
        }
    }
}
