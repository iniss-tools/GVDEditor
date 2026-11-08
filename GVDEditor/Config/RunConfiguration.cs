using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Konfiguracia spustania INISSu - vlastny nazov, program instalacie, parametre prikazoveho riadka, vetva registra
/// a ci sa spusta ako spravca. Uklada sa do config.xml (pre tento pocitac) alebo do dat instalacie
/// (<see cref="Shared" />).
/// </summary>
[XmlType("RunConfiguration")]
public sealed record RunConfiguration
{
    /// <summary>Stabilny identifikator (vyber, beziace instancie).</summary>
    [XmlAttribute("Id")]
    public string Id { get; set; } = NewId();

    /// <summary>Nazov zobrazovany v ponukach.</summary>
    [XmlElement("Name")]
    public string Name { get; set; } = "";

    /// <summary>Meno suboru programu v priecinku instalacie (napr. <c>INISS - Bardejov.exe</c>).</summary>
    [XmlElement("Program")]
    public string Program { get; set; } = "";

    /// <summary>Hodnota <c>/Reg:</c>; prazdna = vetva podla mena programu.</summary>
    [XmlElement("Registry"), DefaultValue("")]
    public string Registry { get; set; } = "";

    /// <summary>Spustit ako spravca.</summary>
    [XmlElement("RunAsAdmin"), DefaultValue(false)]
    public bool RunAsAdmin { get; set; }

    /// <summary><c>/Minimize</c>.</summary>
    [XmlElement("Minimize"), DefaultValue(false)]
    public bool Minimize { get; set; }

    /// <summary><c>/Multiuse</c>.</summary>
    [XmlElement("Multiuse"), DefaultValue(false)]
    public bool Multiuse { get; set; }

    /// <summary><c>/Remote</c>.</summary>
    [XmlElement("Remote"), DefaultValue(false)]
    public bool Remote { get; set; }

    /// <summary><c>/Export</c>.</summary>
    [XmlElement("Export"), DefaultValue(false)]
    public bool Export { get; set; }

    /// <summary><c>/ExportHlas</c>.</summary>
    [XmlElement("ExportHlas"), DefaultValue(false)]
    public bool ExportHlas { get; set; }

    /// <summary><c>/Import</c>.</summary>
    [XmlElement("Import"), DefaultValue(false)]
    public bool Import { get; set; }

    /// <summary><c>/ImportDat</c>.</summary>
    [XmlElement("ImportDat"), DefaultValue(false)]
    public bool ImportDat { get; set; }

    /// <summary><c>/NoRestore</c>.</summary>
    [XmlElement("NoRestore"), DefaultValue(false)]
    public bool NoRestore { get; set; }

    /// <summary>Aktivatory DirList <c>/1</c>-<c>/9</c> ako cislice (napr. <c>13</c>).</summary>
    [XmlElement("Activators"), DefaultValue("")]
    public string Activators { get; set; } = "";

    /// <summary>Dalsie parametre pripojene na koniec prikazoveho riadka.</summary>
    [XmlElement("ExtraArgs"), DefaultValue("")]
    public string ExtraArguments { get; set; } = "";

    /// <summary>Ulozenie zmien grafikonu pred spustenim.</summary>
    [XmlElement("SaveBefore"), DefaultValue(SaveBeforeRun.Ask)]
    public SaveBeforeRun SaveBefore { get; set; } = SaveBeforeRun.Ask;

    /// <summary>Pred spustenim skontrolovat otvoreny grafikon analyzou.</summary>
    [XmlElement("AnalyzeBefore"), DefaultValue(false)]
    public bool AnalyzeBefore { get; set; }

    /// <summary>Spravanie, ked INISS tejto konfiguracie uz bezi.</summary>
    [XmlElement("WhenRunning"), DefaultValue(WhenAlreadyRunning.Ask)]
    public WhenAlreadyRunning WhenRunning { get; set; } = WhenAlreadyRunning.Ask;

    /// <summary>Konfiguracia je ulozena v datach instalacie (nie v config.xml).</summary>
    [XmlIgnore]
    public bool Shared { get; set; }

    /// <summary>Novy identifikator.</summary>
    public static string NewId() => Guid.NewGuid().ToString("N");

    /// <summary>Cislice aktivatorov ako zoznam (bez duplicit, zoradene).</summary>
    [XmlIgnore]
    public IReadOnlyList<int> ActivatorList
    {
        get => Activators.Where(c => c is >= '1' and <= '9').Select(c => c - '0').Distinct().Order().ToList();
        set => Activators = string.Concat(value.Where(n => n is >= 1 and <= 9).Distinct().Order());
    }
}
