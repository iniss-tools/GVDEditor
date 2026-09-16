namespace GVDEditor.Entities;

/// <summary>
///     Trieda reprezentujuca kolaj v stanici.
/// </summary>
public sealed record Track()
{
    /// <summary>
    ///     Neznama kolaj - prvy riadok Pozice_A.TXT s klucom N. INISS na nu posadi vlak, ktoremu sa kolaj nepodarilo
    ///     urcit, preto sa do suboru vzdy zapisuje ako prva.
    /// </summary>
    public static readonly Track None = new("N", "-", "Neznáma", Platform.None, "", "");

    /// <summary>
    ///     Vytvori novu instanciu triedy typu <see cref="Track"/> so zadanymi vlastnostami.
    /// </summary>
    public Track(string key, string name, string fullname, Platform platform, string sound, string trackName) : this()
    {
        Key = key;
        Name = name;
        FullName = fullname;
        Platform = platform;
        SoundName = sound;
        TrackName = trackName;
    }

    /// <summary>
    ///     Identifikátor kolaje.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    ///     Názov kolaje.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Cely názov kolaje.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    ///     Nastupiste kolaje.
    /// </summary>
    public Platform Platform { get; set; } = null!;

    /// <summary>
    ///     Názov kolaje.
    /// </summary>
    public string TrackName { get; set; } = null!;

    /// <summary>
    ///     Nazov zvuku kolaje.
    /// </summary>
    public string SoundName { get; set; } = null!;

    /// <summary>
    ///     Logicke tabule nachadzajuce sa na tejto kolaji.
    /// </summary>
    public List<TableLogical> Tables { get; } = new();

    /// <summary>
    ///     Priority logickych tabul podla kluca tabule (Pozice_A.TXT, polia za klucmi tabul). Tabula bez zaznamu ma
    ///     prioritu 0. INISS hodnotu nacita, GVDEditor ju len zachovava.
    /// </summary>
    public Dictionary<string, int> TablePriorities { get; } = new();

    /// <summary>
    ///     Spojeny text nastupista a kolaje pre tabulove funkcie NastKolejPrijezd/NastKolejOdjezd (FILL_SECTION 30/31)
    ///     - prve nepovinne pole za prioritami v Pozice_A.TXT.
    /// </summary>
    public string PlatformTrackText { get; set; } = "";

    /// <summary>
    ///     Alternativny text kolaje pre tabulove funkcie KolejAltPrijezd/KolejAltOdjezd (FILL_SECTION 32/33)
    ///     - druhe nepovinne pole za prioritami v Pozice_A.TXT.
    /// </summary>
    public string AltTrackText { get; set; } = "";

    /// <summary>
    ///     Odkaz na seba, pouzite pre DataSource.
    /// </summary>
    public Track This => this;

    /// <summary>
    ///     Vyhlada kolaj z listu podla zadaneho identifikatora kolaje.
    /// </summary>
    /// <param name="tracks"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Track? GetFromID(IEnumerable<Track> tracks, string id) => tracks.FirstOrDefault(kolaj => kolaj.Key == id);

    /// <summary>
    ///     Porovna identifikatory kolaji
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EqualsKeys(Track other) => other.Key == Key;

    /// <inheritdoc />
    public override string ToString() => Key;
}