namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Vlak a jeho radenie v stanici.
/// </summary>
public sealed class PlanTrain
{
    /// <summary>Druh vlaku tak, ako ho precital OCR (R, Ex, EC, rj...).</summary>
    public string Type { get; set; } = "";

    /// <summary>Cislo vlaku.</summary>
    public string Number { get; set; } = "";

    /// <summary>Nazov vlaku (moze byt prazdny).</summary>
    public string Name { get; set; } = "";

    /// <summary>Prichod a odchod tak, ako su na strane (napr. 6:58 - 6:59).</summary>
    public string Times { get; set; } = "";

    /// <summary>Vychodiskova stanica vlaku.</summary>
    public string From { get; set; } = "";

    /// <summary>Cielova stanica vlaku.</summary>
    public string To { get; set; } = "";

    /// <summary>Strana (od 1).</summary>
    public int Page { get; set; }

    /// <summary>
    /// Kde je rusen: <c>left</c>, <c>right</c>, alebo prazdne (bez rusna, napr. jednotka). Vozne su v
    /// <see cref="Vehicles" /> vzdy v poradi od rusna.
    /// </summary>
    public string Locomotive { get; set; } = "";

    /// <summary>Vozidla v poradi od rusna (rusne su v zozname tiez, s <see cref="PlanVehicle.IsLocomotive" />).</summary>
    public List<PlanVehicle> Vehicles { get; set; } = [];

    /// <summary>Poznamky pod radenim.</summary>
    public List<PlanNote> Notes { get; set; } = [];

    /// <summary>Co sa nepodarilo precitat spolahlivo.</summary>
    public List<string> Warnings { get; set; } = [];

    /// <summary>Oblast vlaku na strane (x,y,sirka,vyska) - na vyrez v kontrole importu.</summary>
    public string Area { get; set; } = "";
}
