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
