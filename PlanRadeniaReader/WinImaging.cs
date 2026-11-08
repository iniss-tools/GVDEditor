using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace Iniss.PlanRadenia;

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
