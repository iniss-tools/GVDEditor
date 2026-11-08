namespace Iniss.PlanRadenia;

/// <summary>
/// Znak precitaneho riadku: poloha bunky v stlpcoch riadku.
/// </summary>
/// <param name="SpaceBefore">Pred znakom je v texte medzera (znak zacina novy token).</param>
internal readonly record struct NoteCell(char Char, int X, int Width, bool SpaceBefore);
