namespace Iniss.PlanRadenia;

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
