namespace Iniss.PlanRadenia;

/// <summary>
/// Integralny obraz oblasti strany (sucty zloziek farieb) - priemer farieb lubovolneho obdlznika styrmi pristupmi.
/// </summary>
internal sealed class SummedImage
{
    private readonly Box _area;
    private readonly long[] _sums;

    public SummedImage(Raster page, Box area)
    {
        _area = area;
        int w = area.Width + 1, h = area.Height + 1;
        _sums = new long[w * h * 3];
        for (var y = 1; y < h; y++)
            for (var x = 1; x < w; x++)
            {
                var p = page[area.X + x - 1, area.Y + y - 1];
                for (var c = 0; c < 3; c++)
                {
                    long value = c == 0 ? p.R : c == 1 ? p.G : p.B;
                    _sums[(y * w + x) * 3 + c] = value + _sums[((y - 1) * w + x) * 3 + c] + _sums[(y * w + x - 1) * 3 + c]
                                                 - _sums[((y - 1) * w + x - 1) * 3 + c];
                }
            }
    }

    public bool Contains(Box box) => box.X >= _area.X && box.Y >= _area.Y && box.Right <= _area.Right && box.Bottom <= _area.Bottom;

    /// <summary>Priemerne farby obdlznika v mriezke grid x grid (R, G, B za sebou).</summary>
    public float[] Sample(Box box, int grid)
    {
        var result = new float[grid * grid * 3];
        var w = _area.Width + 1;
        for (var gy = 0; gy < grid; gy++)
            for (var gx = 0; gx < grid; gx++)
            {
                int x0 = box.X - _area.X + gx * box.Width / grid, x1 = Math.Max(x0 + 1, box.X - _area.X + (gx + 1) * box.Width / grid);
                int y0 = box.Y - _area.Y + gy * box.Height / grid, y1 = Math.Max(y0 + 1, box.Y - _area.Y + (gy + 1) * box.Height / grid);
                var n = (float)((x1 - x0) * (y1 - y0));
                for (var c = 0; c < 3; c++)
                {
                    var sum = _sums[(y1 * w + x1) * 3 + c] - _sums[(y0 * w + x1) * 3 + c] - _sums[(y1 * w + x0) * 3 + c] + _sums[(y0 * w + x0) * 3 + c];
                    result[(gy * grid + gx) * 3 + c] = sum / n;
                }
            }

        return result;
    }
}
