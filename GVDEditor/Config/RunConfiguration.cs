using System.Xml.Serialization;

namespace GVDEditor.Config;

/// <summary>
/// Ci sa pred spustenim INISSu ulozia neulozene zmeny grafikonu.
/// </summary>
public enum SaveBeforeRun
{
    /// <summary>Opytat sa.</summary>
    [XmlEnum("Ask")] Ask,

    /// <summary>Ulozit bez pytania.</summary>
    [XmlEnum("Always")] Always,

    /// <summary>Neukladat - INISS nacita data z disku.</summary>
    [XmlEnum("Never")] Never
}

/// <summary>
/// Co spravit, ked INISS tejto konfiguracie uz bezi.
/// </summary>
public enum WhenAlreadyRunning
{
    /// <summary>Opytat sa (restart alebo dalsia instancia).</summary>
    [XmlEnum("Ask")] Ask,

    /// <summary>Beziaci INISS riadne ukoncit a spustit znova.</summary>
    [XmlEnum("Restart")] Restart,

    /// <summary>Spustit dalsiu instanciu (INISS ju pusti len s /Multiuse).</summary>
    [XmlEnum("NewInstance")] NewInstance
}

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

/// <summary>
/// Konfiguracie spustania jednej instalacie ulozene v config.xml (tento pocitac) a vybrana konfiguracia.
/// </summary>
public sealed record InstallationRunConfigurations
{
    /// <summary>Priecinok instalacie INISS.</summary>
    [XmlAttribute("Dir")]
    public string Dir { get; set; } = "";

    /// <summary>Identifikator vybranej konfiguracie (aj zdielanej).</summary>
    [XmlAttribute("Selected"), DefaultValue("")]
    public string Selected { get; set; } = "";

    /// <summary>Konfiguracie ulozene len na tomto pocitaci.</summary>
    [XmlElement("RunConfiguration")]
    public List<RunConfiguration> Items { get; set; } = [];

    /// <summary>Hlboka kopia.</summary>
    public InstallationRunConfigurations DeepCopy() => this with { Items = Items.Select(i => i with { }).ToList() };
}

/// <summary>
/// Konfiguracie zdielane v datach instalacie (<c>.gvdeditor\RunConfigurations.xml</c>).
/// </summary>
[XmlRoot("RunConfigurations")]
public sealed class SharedRunConfigurations
{
    /// <summary>Konfiguracie.</summary>
    [XmlElement("RunConfiguration")]
    public List<RunConfiguration> Items { get; set; } = [];
}
