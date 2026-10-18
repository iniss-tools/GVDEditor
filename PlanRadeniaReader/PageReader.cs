namespace Iniss.PlanRadenia;

/// <summary>
/// Hlavicka strany: nazov stanice a platnost planu.
/// </summary>
internal sealed record PageHeader(string Station, string ValidFrom, string ValidTo);

/// <summary>
/// Precitana strana planu radenia.
/// </summary>
internal sealed class PageContent
{
    public required PageHeader Header { get; init; }

    public required PageLayout Layout { get; init; }

    public List<PlanTrain> Trains { get; } = [];

    /// <summary>Riadky tabulky bez druhu a cisla vlaku (hlavicka tabulky alebo neprecitany vlak).</summary>
    public List<(Box Row, string Text)> Skipped { get; } = [];
}

/// <summary>
/// Cita jednu stranu planu radenia: hlavicku, riadky vlakov a v nich radenie.
/// </summary>
internal sealed class PageReader(TextRecognizer ocr, VehicleReader vehicles, Legend icons)
{
    private static readonly int[] HeaderScales = [3, 2, 4];

    /// <summary>
    /// Precita len hlavicku strany (nazov stanice a platnost) - na vyber stran stanice.
    /// </summary>
    /// <returns><see langword="null" />, ak strana nema tabulku planu radenia.</returns>
    public async Task<(PageHeader Header, PageLayout Layout)?> ReadHeaderAsync(Raster page)
    {
        if (PageLayout.Find(page) is not { } layout)
            return null;

        var lines = await ocr.ReadAsync(page, Box.FromEdges(0, 0, page.Width, layout.Rules[0] - 2), 2);
        var (from, to) = TrainHeader.Validity(lines);

        // nazov stanice je druhy riadok nadpisu v strede strany (prvy je PLAN RADENIA VLAKOV / TRAIN ...)
        var station = lines
            .Where(line => Math.Abs(line.Box.CenterX - page.Width / 2) < page.Width / 6)
            .Where(line => !line.Text.Contains("TRAIN", StringComparison.OrdinalIgnoreCase) && !line.Text.Contains("RADENIA", StringComparison.OrdinalIgnoreCase))
            .OrderBy(line => line.Box.Y)
            .Select(line => line.Text.Trim())
            .FirstOrDefault() ?? "";

        return (new PageHeader(station, from, to), layout);
    }

    /// <summary>Precita celu stranu.</summary>
    public async Task<PageContent?> ReadAsync(Raster page, int pageNumber)
    {
        if (await ReadHeaderAsync(page) is not { } header)
            return null;


        var content = new PageContent { Header = header.Header, Layout = header.Layout };
        foreach (var row in header.Layout.TrainRows())
        {
            var left = Box.FromEdges(row.X + 2, row.Y, header.Layout.Column - 2, row.Bottom);
            var train = new PlanTrain { Page = pageNumber, Area = row.ToString() };
            // OCR drobneho textu zavisi od zvacsenia - skusa sa viac, kym sa neprecita druh a cislo vlaku
            var lines = new List<TextLine>();
            var found = false;
            foreach (var scale in HeaderScales)
            {
                lines = await ocr.ReadAsync(page, left, scale);
                if (found = TrainHeader.Fill(train, lines))
                    break;
            }

            if (!found)
            {
                content.Skipped.Add((row, string.Join(" | ", lines.Select(l => l.Text))));
                continue;
            }

            var right = Box.FromEdges(header.Layout.Column + 3, row.Y, row.Right, row.Bottom);
            vehicles.Read(page, right, train, icons);
            content.Trains.Add(train);
        }

        return content;
    }
}
