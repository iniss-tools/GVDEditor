namespace Iniss.PlanRadenia;

/// <summary>
/// Precitany riadok poznamky.
/// </summary>
/// <param name="Text">Text s medzerami podla gramatiky (pred kazdym tokenom).</param>
/// <param name="Cells">Znaky (bez medzier).</param>
/// <param name="Cost">Priemerna odchylka obrazka od vzorov na znak - mera neistoty.</param>
internal sealed record NoteReading(string Text, List<NoteCell> Cells, double Cost);
