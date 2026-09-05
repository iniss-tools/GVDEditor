using System.Text;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Iniss.Elis;

/// <summary>
///     Vysledok jedneho vycitania dat z programu ELIS (Cestovne poriadky, CHAPS).
///     Tento subor je zdielany medzi projektom ELISBridge (x86 host nad TT.dll)
///     a GVDEditorom, ktory ho linkuje - preto nesmie zavisiet na niecom z GVDEditora.
/// </summary>
public sealed class ElisResult
{
    /// <summary>Priecinok s datami (.tt subormi), z ktoreho sa citalo.</summary>
    public string DataPath { get; set; }

    /// <summary>Nazov stanice tak, ako ho pozna ELIS.</summary>
    public string StationName { get; set; }

    /// <summary>Zaciatok platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidFrom { get; set; }

    /// <summary>Koniec platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidTo { get; set; }

    /// <summary>Pocet dni platnosti - dlzka retazca <see cref="ElisTrain.RunsBits" />.</summary>
    [UsedImplicitly]
    public int TotalDays { get; set; }

    /// <summary>Vlaky prechadzajuce zadanou stanicou.</summary>
    public List<ElisTrain> Trains { get; set; } = new();

    /// <summary>Zaciatok platnosti ako <see cref="DateTime" />.</summary>
    [UsedImplicitly]
    public DateTime ValidFromDate => DateTime.ParseExact(ValidFrom, "yyyy-MM-dd", null);

    /// <summary>Koniec platnosti ako <see cref="DateTime" />.</summary>
    [UsedImplicitly]
    public DateTime ValidToDate => DateTime.ParseExact(ValidTo, "yyyy-MM-dd", null);

    /// <summary>Zapise vysledok do suboru ako XML v kodovani UTF-8.</summary>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ElisResult));
        using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
        serializer.Serialize(writer, this);
    }

    /// <summary>Nacita vysledok zo suboru zapisaneho metodou <see cref="Save" />.</summary>
    public static ElisResult Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ElisResult));
        using var reader = new StreamReader(path, Encoding.UTF8);
        return (ElisResult)serializer.Deserialize(reader);
    }
}

/// <summary>
///     Jeden vlak tak, ako ho vracia TT.dll - este bez naviazania na entity GVDEditora.
/// </summary>
public sealed class ElisTrain
{
    /// <summary>Hodnota <see cref="ArrivalMinutes" />/<see cref="DepartureMinutes" />, ked cas nie je uvedeny.</summary>
    public const int NoTime = -1;

    /// <summary>Typ (kategoria) vlaku, napr. "EC", "Os".</summary>
    public string Type { get; set; }

    /// <summary>Cislo vlaku.</summary>
    public string Number { get; set; }

    /// <summary>Nazov vlaku (moze byt prazdny).</summary>
    public string Name { get; set; }

    /// <summary>Prichod do stanice v minutach od polnoci, alebo <see cref="NoTime" />.</summary>
    public int ArrivalMinutes { get; set; } = NoTime;

    /// <summary>Odchod zo stanice v minutach od polnoci, alebo <see cref="NoTime" />.</summary>
    public int DepartureMinutes { get; set; } = NoTime;

    /// <summary>Stanice pred domovskou stanicou, v poradi jazdy.</summary>
    public List<string> StationsBefore { get; set; } = new();

    /// <summary>Stanice za domovskou stanicou, v poradi jazdy.</summary>
    public List<string> StationsAfter { get; set; } = new();

    /// <summary>Nazov dopravcu (pole ON), alebo prazdne.</summary>
    public string OperatorName { get; set; }

    /// <summary>Cislo dopravcu (pole ONo) - stabilnejsi kluc nez nazov.</summary>
    public string OperatorNumber { get; set; }

    /// <summary>Cislo linky, ak ho dataset obsahuje (inak prazdne).</summary>
    public string Line { get; set; }

    /// <summary>
    ///     Datumove obmedzenie ako retazec '0'/'1' dlzky <see cref="ElisResult.TotalDays" />,
    ///     kde index 0 zodpoveda <see cref="ElisResult.ValidFrom" />.
    /// </summary>
    public string RunsBits { get; set; }
}