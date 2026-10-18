namespace Iniss.PlanRadenia;

/// <summary>
/// Farba bodu obrazka.
/// </summary>
internal readonly record struct Rgb(byte R, byte G, byte B)
{
    public int Max => Math.Max(R, Math.Max(G, B));

    public int Min => Math.Min(R, Math.Min(G, B));

    /// <summary>Biely alebo takmer biely papier.</summary>
    public bool IsWhite => Min > 225 && Max - Min < 25;

    /// <summary>Cierna farba textu a obrysov (cisla vozni su cierne, karoseria farebna).</summary>
    public bool IsBlack => Max < 110;

    /// <summary>
    /// Svetlosiva bez odtiena - deliace ciary tabulky. Ciara medzi dvoma riadkami bodov je po vykresleni len o malo
    /// tmavsia nez papier (papier ma vsetky zlozky nad 250).
    /// </summary>
    public bool IsRule => Max > 100 && Min < 247 && Max - Min < 30;

    /// <summary>Vzdialenost farieb (stvorec euklidovskej vzdialenosti).</summary>
    public int Distance(Rgb other)
    {
        int r = R - other.R, g = G - other.G, b = B - other.B;
        return r * r + g * g + b * b;
    }
}

/// <summary>
/// Obdlznik v bodoch obrazka.
/// </summary>
internal readonly record struct Box(int X, int Y, int Width, int Height)
{
    public int Right => X + Width;

    public int Bottom => Y + Height;

    public int CenterX => X + Width / 2;

    public int CenterY => Y + Height / 2;

    public static Box FromEdges(int left, int top, int right, int bottom) => new(left, top, right - left, bottom - top);

    public Box Inflate(int dx, int dy) => new(X - dx, Y - dy, Width + 2 * dx, Height + 2 * dy);

    public Box Clip(int width, int height) => FromEdges(Math.Max(0, X), Math.Max(0, Y), Math.Min(width, Right), Math.Min(height, Bottom));

    public override string ToString() => $"{X},{Y} {Width}x{Height}";
}

/// <summary>
/// Obrazok strany v pamati (BGRA, 8 bitov na zlozku).
/// </summary>
internal sealed class Raster(int width, int height, byte[] bgra)
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    /// <summary>Body v poradi B, G, R, A po riadkoch.</summary>
    public byte[] Pixels { get; } = bgra;

    public Rgb this[int x, int y]
    {
        get
        {
            var i = (y * Width + x) * 4;
            return new Rgb(Pixels[i + 2], Pixels[i + 1], Pixels[i]);
        }
    }

    public void Set(int x, int y, Rgb color)
    {
        var i = (y * Width + x) * 4;
        Pixels[i] = color.B;
        Pixels[i + 1] = color.G;
        Pixels[i + 2] = color.R;
        Pixels[i + 3] = 255;
    }

    /// <summary>Obrys obdlznika - len na ladiace obrazky.</summary>
    public void DrawRect(Box box, Rgb color)
    {
        box = box.Clip(Width, Height);
        for (var x = box.X; x < box.Right; x++)
        {
            Set(x, box.Y, color);
            Set(x, box.Bottom - 1, color);
        }

        for (var y = box.Y; y < box.Bottom; y++)
        {
            Set(box.X, y, color);
            Set(box.Right - 1, y, color);
        }
    }

    public Raster Crop(Box box)
    {
        box = box.Clip(Width, Height);
        var pixels = new byte[box.Width * box.Height * 4];
        for (var y = 0; y < box.Height; y++)
            Buffer.BlockCopy(Pixels, ((box.Y + y) * Width + box.X) * 4, pixels, y * box.Width * 4, box.Width * 4);
        return new Raster(box.Width, box.Height, pixels);
    }

    /// <summary>Obrazok s bielym okrajom.</summary>
    public Raster Pad(int margin)
    {
        int w = Width + 2 * margin, h = Height + 2 * margin;
        var pixels = new byte[w * h * 4];
        Array.Fill(pixels, (byte)255);
        for (var y = 0; y < Height; y++)
            Buffer.BlockCopy(Pixels, y * Width * 4, pixels, ((y + margin) * w + margin) * 4, Width * 4);
        return new Raster(w, h, pixels);
    }

    /// <summary>
    /// Zvacsi obrazok bilinearne - OCR cita drobny text az zvacseny.
    /// </summary>
    public Raster Scale(int factor)
    {
        int w = Width * factor, h = Height * factor;
        var pixels = new byte[w * h * 4];
        for (var y = 0; y < h; y++)
        {
            var sy = Math.Max(0, (y + 0.5) / factor - 0.5);
            var y0 = (int)sy;
            var y1 = Math.Min(Height - 1, y0 + 1);
            var fy = sy - y0;
            for (var x = 0; x < w; x++)
            {
                var sx = Math.Max(0, (x + 0.5) / factor - 0.5);
                var x0 = (int)sx;
                var x1 = Math.Min(Width - 1, x0 + 1);
                var fx = sx - x0;
                for (var c = 0; c < 4; c++)
                {
                    double a = Pixels[(y0 * Width + x0) * 4 + c], b = Pixels[(y0 * Width + x1) * 4 + c];
                    double d = Pixels[(y1 * Width + x0) * 4 + c], e = Pixels[(y1 * Width + x1) * 4 + c];
                    var top = a + (b - a) * fx;
                    var bottom = d + (e - d) * fx;
                    pixels[(y * w + x) * 4 + c] = (byte)Math.Round(top + (bottom - top) * fy);
                }
            }
        }

        return new Raster(w, h, pixels);
    }
}
