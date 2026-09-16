namespace GVDEditor.Entities;

/// <summary>
///     Trieda obsahujuca informacie o audio linke (zvukovom okruhu) - jeden riadok Audio.txt.
/// </summary>
public sealed class Audio
{
    /// <summary>
    ///     Vrati alebo nastavi stanicu audio linky (použije sa len identifikátor stanice). Stlpec 1.
    /// </summary>
    public Station Station { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi názov audio linky pre operatora. Stlpec 2.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi protokolovy nazov (meno okruhu pre nadradeny system, znacka LINE). Stlpec 3.
    /// </summary>
    public string ShortName { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi názov prehravacej fronty. Stlpec 4.
    /// </summary>
    public string QueueName { get; set; } = null!;

    /// <summary>
    ///     Stlpec 5 - INISS ho nacita, ale nepouziva. Zachovava sa pre kompatibilitu.
    /// </summary>
    public string Mixer { get; set; } = null!;

    /// <summary>
    ///     Vrati alebo nastavi zvukove zariadenie, volitelne s cielovou linkou mixera (<c>zariadenie:linka</c>). Stlpec 6.
    /// </summary>
    public string SoundCard { get; set; } = null!;

    /// <summary>
    ///     Vstupna linka mixera - cislo alebo nazov. Stlpec 7.
    /// </summary>
    public string InputLine { get; set; } = "";

    /// <summary>
    ///     Spinanie zosilnovaca: cislo vystupu ustredne TORNZ (0-63), <c>E&lt;n&gt;</c> port ELSVO, pripadne <c>EE…</c> / <c>Z…</c>.
    ///     Stlpec 8.
    /// </summary>
    public string AmplifierPort { get; set; } = "";

    /// <summary>
    ///     Parameter, ktory INISS posiela ovladacu TORNZ pri zopnuti vystupu. Stlpec 9.
    /// </summary>
    public string ExchangeParameter { get; set; } = "";

    /// <summary>
    ///     Cislo uzla (pocitaca), ktory okruh realne prehrava; prazdne alebo 0 = tento pocitac. Moze byt aj <c>COM3</c>. Stlpec 10.
    /// </summary>
    public string Node { get; set; } = "";

    /// <inheritdoc />
    public override string ToString() => Name;
}
