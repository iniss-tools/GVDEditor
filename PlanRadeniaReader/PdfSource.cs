using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
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
