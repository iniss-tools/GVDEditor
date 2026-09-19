namespace GVDEditor.Entities;

/// <summary>
///     Predloha stavoveho diagramu (StateDgm.txt), ktora sa zapise do noveho grafikonu.
/// </summary>
public enum StateDgmTemplate
{
    /// <summary>Slovenske nazvy pre obsluhu, poloautomat pri prichode a odchode.</summary>
    Slovak,

    /// <summary>Ceske nazvy pre obsluhu, inak ako <see cref="Slovak" />.</summary>
    Czech,

    /// <summary>Slovenska predloha s automatikou riadenou udalostami ILTIS (VVC, OVC, Vj, Odj).</summary>
    SlovakIltis
}
