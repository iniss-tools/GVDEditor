namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Poznamka pod radenim, napr. <c>č. 21, č. 22 - ide do 15.IX.</c>.
/// </summary>
public sealed class PlanNote
{
    /// <summary>Precitany text poznamky (⑤ = den v tyzdni, ⚒ = pracovne dni).</summary>
    public string Text { get; set; } = "";

    public PlanNoteKind Kind { get; set; }

    /// <summary>Cisla vozni, ktorych sa poznamka tyka (<c>#n</c> = poradove cislo vozna bez cisla).</summary>
    public List<string> Cars { get; set; } = [];

    /// <summary>Obmedzenie dnami (pri <see cref="PlanNoteKind.Restriction" /> a <see cref="PlanNoteKind.Joined" />).</summary>
    public string Rest { get; set; } = "";

    /// <summary>Stanica: ciel vozni alebo stanica, v ktorej sa vozne pridavaju.</summary>
    public string Station { get; set; } = "";

    /// <summary>
    /// Zhoda nazvu stanice so zoznamom stanic (priemerna odchylka od pisma na znak); 0 = nazov nie je zo zoznamu.
    /// </summary>
    public double StationScore { get; set; }

    /// <summary>
    /// Najvacsia priemerna odchylka riadku od vzorov pisma na znak - nad 0,35 je text neisty.
    /// </summary>
    public double Score { get; set; }

    /// <summary>Oblast poznamky na strane (x,y sirka x vyska) - na zobrazenie pri kontrole.</summary>
    public string Area { get; set; } = "";

    public List<string> Warnings { get; set; } = [];
}
