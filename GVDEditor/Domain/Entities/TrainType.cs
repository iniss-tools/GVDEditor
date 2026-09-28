using System.Text.RegularExpressions;

namespace GVDEditor.Domain.Entities;

/// <summary>
/// Reprezentuje typ vlaku.
/// </summary>
/// <remarks>
/// Entita s identitou - vlaky sa odkazuju na instanciu zo zoznamu typov a typ sa upravuje na mieste,
/// preto sa porovnava referenciou.
/// </remarks>
public sealed partial class TrainType
{
    /// <summary>
    /// Konstruktor pre definovanie predvoleneho typu vlaku.
    /// </summary>
    /// <param name="key">Kluc typu vlaku.</param>
    public TrainType(string key)
    {
        Key = key;
        CategoryTrain = key;
        TextInTable = key;
    }

    /// <summary>
    /// Konstruktor pre definovanie pouzivatelom definovaneho typu vlaku.
    /// </summary>
    /// <param name="categoryTrain"></param>
    /// <param name="key"></param>
    /// <param name="textInTable"></param>
    public TrainType(string categoryTrain, string key, string textInTable)
    {
        Key = key;
        CategoryTrain = categoryTrain;
        TextInTable = textInTable;
    }

    /// <summary>
    /// Kluc typu vlaku.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Ci je pouzivatelom definovany - ma kategoriu z volnych miest INISSu (Os1-Os9, R1-R9, X1-X9, Sl1-Sl9).
    /// Odvodzuje sa z kategorie, aby typ mohol na mieste zmenit zabudovany druh na vlastny a naopak
    /// (vlaky otvoreneho grafikonu sa odkazuju na tento objekt).
    /// </summary>
    public bool IsCustom => IsCustomCategory(CategoryTrain);

    /// <summary>
    /// Ci je kategoria volne miesto pre vlastny typ (napr. R3).
    /// </summary>
    public static bool IsCustomCategory(string? category) =>
        category is not null && CustomTrainType().IsMatch(category);

    /// <summary>
    /// Vrati text na tabuli, ktory sa ma zobrazovat na mieste typu vlaku.
    /// </summary>
    public string TextInTable { get; set; }

    /// <summary>
    /// Kategoria typu vlaku.
    /// </summary>
    public string CategoryTrain { get; set; }

    /// <summary>
    /// This.
    /// </summary>
    public TrainType This => this;

    /// <inheritdoc />
    public override string ToString() => Key;

    /// <summary>
    /// Zisti, ci zadany retazec reprezentuje predvoleny typ vlaku.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Validate(string type)
    {
        switch (type)
        {
            case "Os":
            case "MOs":
            case "Sp":
            case "R":
            case "Ex":
            case "EC":
            case "EN":
            case "IC":
            case "SC":
            case "Bus":
            case "Rn":
            case "Rp":
            case "Zr":
            case "REX":
            case "ER":
            case "Nákl":
            case "NZ":
            case "ICE":
            case "Sl":
            case "SPR":
            case "Loď":
            case "Lan":
            case "TGV":
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Vrati vsetky zabudovane typy vlakov (tabulka INISSu bez volnych miest X1-X9, R1-R9, Os1-Os9, Sl1-Sl9).
    /// </summary>
    /// <returns></returns>
    public static List<TrainType> GetDefaultValues()
    {
        var types = new List<TrainType>
        {
            new("Os"),
            new("MOs"),
            new("Sp"),
            new("Zr"),
            new("SPR"),
            new("R"),
            new("Ex"),
            new("REX"),
            new("ER"),
            new("EC"),
            new("IC"),
            new("SC"),
            new("ICE"),
            new("EN"),
            new("NZ"),
            new("TGV"),
            new("Bus"),
            new("Loď"),
            new("Lan"),
            new("Nákl"),
            new("Sl"),
            new("Rn"),
            new("Rp"),
        };

        return types;
    }

    [GeneratedRegex("^(Os|R|X|Sl)[1-9]$")]
    private static partial Regex CustomTrainType();
}