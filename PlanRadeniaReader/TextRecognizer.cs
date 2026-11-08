using Windows.Globalization;
using Windows.Media.Ocr;

namespace Iniss.PlanRadenia;

/// <summary>
/// Rozpoznavanie textu rozhranim Windows (Windows.Media.Ocr) so slovencinou.
/// </summary>
internal sealed class TextRecognizer
{
    /// <summary>Biely okraj okolo citanej oblasti v bodoch strany.</summary>
    private const int Padding = 16;

    private readonly OcrEngine _engine;

    private TextRecognizer(OcrEngine engine) => _engine = engine;

    /// <summary>
    /// Vytvori rozpoznavanie slovenciny.
    /// </summary>
    /// <exception cref="InvalidOperationException">Windows nema nainstalovane OCR pre slovencinu</exception>
    public static TextRecognizer Create()
    {
        var language = new Language("sk");
        if (!OcrEngine.IsLanguageSupported(language) || OcrEngine.TryCreateFromLanguage(language) is not { } engine)
            throw new InvalidOperationException("OcrMissing");
        return new TextRecognizer(engine);
    }

    /// <summary>
    /// Precita text v oblasti strany. Oblast sa pred rozpoznanim zvacsi - drobny text OCR cita az zvacseny.
    /// </summary>
    /// <param name="words">Vratit jednotlive slova namiesto riadkov.</param>
    /// <returns>Riadky (alebo slova) textu so suradnicami na strane.</returns>
    public async Task<List<TextLine>> ReadAsync(Raster page, Box region, int scale, bool words = false)
    {
        region = region.Clip(page.Width, page.Height);
        if (region.Width < 4 || region.Height < 4)
            return [];

        // OCR casto necita text, ktory sa dotyka okraja obrazka - pridava sa biely okraj
        var image = page.Crop(region).Pad(Padding);
        if (scale > 1)
            image = image.Scale(scale);

        using var bitmap = WinImaging.ToSoftwareBitmap(image);
        var result = await _engine.RecognizeAsync(bitmap);
        var lines = new List<TextLine>();
        int x0 = region.X - Padding, y0 = region.Y - Padding;
        if (words)
        {
            foreach (var word in result.Lines.SelectMany(line => line.Words))
            {
                var r = word.BoundingRect;
                lines.Add(new TextLine(word.Text, Box.FromEdges(x0 + (int)(r.X / scale), y0 + (int)(r.Y / scale),
                    x0 + (int)Math.Ceiling((r.X + r.Width) / scale), y0 + (int)Math.Ceiling((r.Y + r.Height) / scale))));
            }

            return lines;
        }

        foreach (var line in result.Lines)
        {
            double left = double.MaxValue, top = double.MaxValue, right = 0, bottom = 0;
            foreach (var word in line.Words)
            {
                left = Math.Min(left, word.BoundingRect.X);
                top = Math.Min(top, word.BoundingRect.Y);
                right = Math.Max(right, word.BoundingRect.X + word.BoundingRect.Width);
                bottom = Math.Max(bottom, word.BoundingRect.Y + word.BoundingRect.Height);
            }

            lines.Add(new TextLine(line.Text, Box.FromEdges(x0 + (int)(left / scale), y0 + (int)(top / scale),
                x0 + (int)Math.Ceiling(right / scale), y0 + (int)Math.Ceiling(bottom / scale))));
        }

        return lines;
    }
}
