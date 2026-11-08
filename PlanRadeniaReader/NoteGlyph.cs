namespace Iniss.PlanRadenia;

/// <summary>
/// Vzor znaku pisma poznamok: tmavost bodov bunky znaku (<see cref="NoteFont.Rows" /> riadkov x <see cref="Width" />
/// stlpcov, 0-1) po riadkoch. Bunka siaha od zaciatku znaku po zaciatok dalsieho.
/// </summary>
/// <param name="Learned">
/// Vzor je nauceny zo skutocnych poznamok; nahradny (vykresleny pismom) sluzi len na porovnanie s nazvami stanic.
/// </param>
internal sealed record NoteGlyph(char Char, int Width, bool Learned, float[] Ink);
