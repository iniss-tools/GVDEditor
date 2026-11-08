namespace GVDEditor.Domain.Editing;

/// <summary>
/// Pocet vlakov, v ktorych sa stanica hlasi medzi koncovymi stanicami, a kolko z nich ju ma v kratkom hlaseni.
/// </summary>
/// <param name="Trains">Pocet vlakov, ktore stanicou idu a hlasia ju v dlhom hlaseni.</param>
/// <param name="Short">Pocet z nich, ktore ju hlasia aj v kratkom hlaseni.</param>
public readonly record struct ShortReportVote(int Trains, int Short)
{
    /// <summary>
    /// Ci ju vlaky grafikonu prevazne hlasia v kratkom hlaseni (aspon polovica).
    /// </summary>
    public bool IsShort => Trains > 0 && Short * 2 >= Trains;
}
