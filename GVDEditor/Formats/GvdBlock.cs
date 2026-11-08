namespace GVDEditor.Formats;

/// <summary>
/// Jeden blok grafikonu uvedeny riadkom <c>/&lt;cislo stanice&gt;</c> v CSV suboroch priecinka GVD.
/// </summary>
/// <param name="Index">Poradie bloku v subore (od 1).</param>
/// <param name="StationId">Cislo stanice z hlavicky bloku.</param>
/// <param name="StationName">Nazov stanice - z Grafikon.txt, ak je to stanica grafikonu, inak zo zvukovej banky.</param>
/// <param name="TrainCount">Pocet datovych riadkov bloku v Export3A.</param>
/// <param name="StartValid">Najskorsi zaciatok platnosti vlaku v bloku (Export3B).</param>
/// <param name="EndValid">Najneskorsi koniec platnosti vlaku v bloku (Export3B).</param>
internal sealed record GvdBlock(int Index, int StationId, string StationName, int TrainCount, DateTime StartValid, DateTime EndValid)
{
    /// <summary>
    /// Nazov noveho priecinka, do ktoreho sa blok presunie (predvyplneny, pouzivatel ho moze zmenit).
    /// </summary>
    public string DirName { get; set; } = "";
}
