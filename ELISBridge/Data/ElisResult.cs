using System.Text;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace Iniss.Elis.Data;

/// <summary>
/// Vysledok jedneho vycitania dat z programu ELIS (Cestovne poriadky, INPROP).
/// Tento subor je zdielany medzi projektom ELISBridge (x86 host nad TT.dll)
/// a GVDEditorom, ktory ho linkuje - preto nesmie zavisiet na niecom z GVDEditora.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class ElisResult
{
    /// <summary>Priecinok s datami (.tt subormi), z ktoreho sa citalo.</summary>
    public string DataPath { get; set; } = null!;

    /// <summary>Nazov stanice tak, ako ho pozna ELIS.</summary>
    public string StationName { get; set; } = null!;

    /// <summary>Cislo stanice (SR70) podla ELIS; 0, ak ho nema.</summary>
    public int StationCode { get; set; }

    /// <summary>Zaciatok platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidFrom { get; set; } = null!;

    /// <summary>Koniec platnosti cestovneho poriadku (yyyy-MM-dd).</summary>
    public string ValidTo { get; set; } = null!;

    /// <summary>Pocet dni platnosti - dlzka retazca <see cref="ElisTrain.RunsBits" />.</summary>
    public int TotalDays { get; set; }

    /// <summary>Vlaky prechadzajuce zadanou stanicou.</summary>
    public List<ElisTrain> Trains { get; set; } = [];

    /// <summary>Zaciatok platnosti ako <see cref="DateTime" />.</summary>
    public DateTime ValidFromDate => DateTime.ParseExact(ValidFrom, "yyyy-MM-dd", null);

    /// <summary>Koniec platnosti ako <see cref="DateTime" />.</summary>
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
        return (ElisResult)serializer.Deserialize(XmlReader.Create(reader))!;
    }
}
