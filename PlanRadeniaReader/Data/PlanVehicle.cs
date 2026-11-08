namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Vozidlo v radeni.
/// </summary>
public sealed class PlanVehicle
{
    /// <summary>Ci je to rusen.</summary>
    public bool IsLocomotive { get; set; }

    /// <summary>Cislo vozna nad voznom, prazdne ak ziadne nie je alebo sa neprecitalo.</summary>
    public string Number { get; set; } = "";

    /// <summary>Zhoda cisla so vzormi cislic (0-1); spravne cisla maju aj okolo <c>0.7</c>, nizsiu treba skontrolovat.</summary>
    public double NumberScore { get; set; }

    /// <summary>Riadok radenia (1 = prvy, 2 = pokracovanie pod nim...).</summary>
    public int Line { get; set; } = 1;

    /// <summary>Farby karoserie s podielom - podla nich trieda a druh vozna.</summary>
    public List<PlanColor> Colors { get; set; } = [];

    /// <summary>Piktogramy vozna - popisy z legendy strany.</summary>
    public List<string> Icons { get; set; } = [];

    /// <summary>Oblast vozna na strane.</summary>
    public string Area { get; set; } = "";
}
