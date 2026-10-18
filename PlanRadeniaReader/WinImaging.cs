using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Iniss.PlanRadenia;

/// <summary>
/// Strany PDF vykreslene rozhranim Windows (Windows.Data.Pdf).
/// </summary>
internal sealed class PdfSource
{
    /// <summary>
    /// Rozlisenie vykreslenia. Plan radenia ZSSK ma strany ako obrazok s rozlisenim okolo 200 dpi - vzory cislic
    /// a vsetky rozmery v bodoch su pri tomto rozliseni.
    /// </summary>
    public const int Dpi = 200;

    private readonly PdfDocument _document;

    private PdfSource(PdfDocument document) => _document = document;

    public int PageCount => (int)_document.PageCount;

    public static async Task<PdfSource> OpenAsync(string path)
    {
        var file = await StorageFile.GetFileFromPathAsync(Path.GetFullPath(path));
        return new PdfSource(await PdfDocument.LoadFromFileAsync(file));
    }

    /// <summary>Vykresli stranu (od 0) do obrazka s rozlisenim <see cref="Dpi" />.</summary>
    public async Task<Raster> RenderAsync(int index)
    {
        using var page = _document.GetPage((uint)index);
        using var stream = new InMemoryRandomAccessStream();
        // velkost strany je v DIP (1/96 palca)
        var options = new PdfPageRenderOptions { DestinationWidth = (uint)Math.Round(page.Size.Width / 96 * Dpi) };
        await page.RenderToStreamAsync(stream, options);
        var decoder = await BitmapDecoder.CreateAsync(stream);
        using var bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
        return WinImaging.ToRaster(bitmap);
    }
}

/// <summary>
/// Riadok textu z OCR.
/// </summary>
internal sealed record TextLine(string Text, Box Box);

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

/// <summary>
/// Prevody medzi <see cref="Raster" /> a obrazkami Windows.
/// </summary>
internal static class WinImaging
{
    public static Raster ToRaster(SoftwareBitmap bitmap)
    {
        var pixels = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];
        bitmap.CopyToBuffer(pixels.AsBuffer());
        return new Raster(bitmap.PixelWidth, bitmap.PixelHeight, pixels);
    }

    public static SoftwareBitmap ToSoftwareBitmap(Raster raster) =>
        SoftwareBitmap.CreateCopyFromBuffer(raster.Pixels.AsBuffer(), BitmapPixelFormat.Bgra8, raster.Width, raster.Height, BitmapAlphaMode.Ignore);

    /// <summary>Ulozi obrazok do PNG - len na ladenie (--debug).</summary>
    public static async Task SavePngAsync(Raster raster, string path)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(Path.GetFullPath(path)));
        var file = await folder.CreateFileAsync(Path.GetFileName(path), CreationCollisionOption.ReplaceExisting);
        using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        using var bitmap = ToSoftwareBitmap(raster);
        encoder.SetSoftwareBitmap(bitmap);
        await encoder.FlushAsync();
    }
}
