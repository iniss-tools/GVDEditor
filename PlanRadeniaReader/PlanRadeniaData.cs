using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Iniss.PlanRadenia;

/// <summary>
/// Vysledok citania planu radenia vlakov ZSSK (PDF). Subor zdiela pomocny program PlanRadeniaReader a GVDEditor,
/// ktory ho linkuje - preto nesmie zavisiet na nicom z GVDEditora.
/// </summary>
public sealed class PlanResult
{
    /// <summary>Platnost planu od (yyyy-MM-dd), prazdne ak sa neprecitala.</summary>
    public string ValidFrom { get; set; } = "";

    /// <summary>Platnost planu do (yyyy-MM-dd), prazdne ak sa neprecitala.</summary>
    public string ValidTo { get; set; } = "";

    /// <summary>Precitane stanice (len pozadovane, alebo vsetky pri vypise).</summary>
    public List<PlanStation> Stations { get; set; } = [];

    /// <summary>Zapise vysledok do suboru ako XML v kodovani UTF-8.</summary>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(PlanResult));
        using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
        serializer.Serialize(writer, this);
    }

    /// <summary>Nacita vysledok zo suboru zapisaneho metodou <see cref="Save" />.</summary>
    public static PlanResult Load(string path)
    {
        var serializer = new XmlSerializer(typeof(PlanResult));
        using var reader = new StreamReader(path, Encoding.UTF8);
        return (PlanResult)serializer.Deserialize(XmlReader.Create(reader))!;
    }
}

/// <summary>
/// Stanica planu radenia - jedna alebo viac stran s vlakmi.
/// </summary>
public sealed class PlanStation
{
    /// <summary>Nazov stanice v hlavicke strany.</summary>
    public string Name { get; set; } = "";

    /// <summary>Cisla stran (od 1).</summary>
    public List<int> Pages { get; set; } = [];

    public List<PlanTrain> Trains { get; set; } = [];
}

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

/// <summary>
/// Upozornenia citania (<see cref="PlanTrain.Warnings" />, <see cref="PlanNote.Warnings" />): kod a za medzerami udaje.
/// Texty pre pouzivatela ma GVDEditor.
/// </summary>
public static class PlanWarnings
{
    /// <summary>Na konci prveho riadku radenia nie je rusen - vozne su zoradene zlava.</summary>
    public const string NoLocomotive = "NoLocomotive";

    /// <summary>Cislo vozna z poznamky (prvy udaj) opravene na cislo vozna v radeni (druhy udaj).</summary>
    public const string CarFixed = "CarFixed";

    /// <summary>Vozen z poznamky (udaj) v radeni nie je.</summary>
    public const string CarMissing = "CarMissing";
}

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

/// <summary>
/// Farba karoserie vozna a jej podiel.
/// </summary>
public sealed class PlanColor
{
    /// <summary>Nazov farby: green, yellow, red, navy, lightblue, gray, white.</summary>
    public string Name { get; set; } = "";

    /// <summary>Podiel karoserie (0-1).</summary>
    public double Share { get; set; }
}

/// <summary>
/// Druh poznamky pod radenim.
/// </summary>
public enum PlanNoteKind
{
    /// <summary>Text sa nepodarilo zaradit.</summary>
    Other,

    /// <summary>Kedy vozne idu: <c>č. 21, č. 22 - ide do 15.IX.</c></summary>
    Restriction,

    /// <summary>Kam vozne idu: <c>č. 344 - Krakow Glowny</c></summary>
    Destination,

    /// <summary>Vozne pridane v stanici: <c>Vozne č. 1, 2 budú zaradené do 31.VIII. vo vlaku od ŽST Banská Bystrica.</c></summary>
    Joined,
}

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
