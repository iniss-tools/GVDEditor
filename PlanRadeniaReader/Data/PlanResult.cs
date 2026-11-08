using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Iniss.PlanRadenia.Data;

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
