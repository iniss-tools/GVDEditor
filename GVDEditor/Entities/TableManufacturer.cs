using ToolsCore.Tools;

namespace GVDEditor.Entities;

/// <summary>
///     Vyrobca tabule, ktory sa definuje v katalogovej tabuli
/// </summary>
public sealed class TableManufacturer : Enumeration<TableManufacturer>
{
    private TableManufacturer(int id, string name, string description, int minAddress, int maxAddress) : base(id, name)
    {
        Description = description;
        MinAddress = minAddress;
        MaxAddress = maxAddress;
    }

    /// <summary>
    ///     Odkaz na seba, pouzivane pre DataSource.
    /// </summary>
    public TableManufacturer This => this;

    /// <summary>
    ///     Popis vyrobcu tak, ako ho ma zabudovany INISS.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Najmensia povolena adresa (ID) fyzickej tabule; <c>-1</c> = INISS rozsah nekontroluje.
    /// </summary>
    public int MinAddress { get; }

    /// <summary>
    ///     Najvacsia povolena adresa (ID) fyzickej tabule; <c>-1</c> = INISS rozsah nekontroluje.
    /// </summary>
    public int MaxAddress { get; }

    /// <summary>
    ///     Ci INISS pozna tohto vyrobcu. Vyrobcovia, ktorych nepozna, ostavaju len kvoli starsim datam z GVDEditora.
    /// </summary>
    public bool IsKnownToIniss => MinAddress != int.MinValue;

    /// <summary>
    ///     Ci je adresa tabule v rozsahu, ktory INISS pre tohto vyrobcu prijme. Adresa <c>-1</c> (bez adresy) prejde vzdy.
    /// </summary>
    /// <param name="address">Adresa tabule (kluc ID v TPhysic.TXT).</param>
    public bool IsAddressValid(int address) => address == -1 || MinAddress < 0 || (address >= MinAddress && address <= MaxAddress);

    /// <summary>
    ///     Konvertuje textove vyjadrenie nazvu typu tabule na objekt. V pripade, ak sa to nepodari, metoda vrati
    ///     <see langword="null" />.
    /// </summary>
    /// <param name="s">Vstupny retazec.</param>
    /// <returns></returns>
    public new static TableManufacturer? Parse(string s) => GetValues().FirstOrDefault(manufacturer => manufacturer.Name == s);

    #region VALUES

    // Kluce, popisy a rozsahy adries su zabudovane v INISSe 3.39 (viď dokumentaciu TPhysic.TXT).
#pragma warning disable 1591
    public static readonly TableManufacturer AdonBuse = new(0, "Adon/BUSE", "terčíkové", -1, -1);
    public static readonly TableManufacturer LCD = new(1, "LCD", "pôv. Elektročas do r. 2001", 0, 255);
    public static readonly TableManufacturer ERS = new(2, "ERS", "listové PT / stará Čihařova ERS", 0, 255);
    public static readonly TableManufacturer FERS = new(3, "FERS", "listové PT / nová Fialova ERS", 0, 255);
    public static readonly TableManufacturer ELEN = new(4, "ELEN", "do r. 2005 – 10-linkové, kód Kamenických", 1, 127);
    public static readonly TableManufacturer LCD1 = new(5, "LCD1", "Elektročas od r. 2002 + blikanie, fonty", 0, 255);
    public static readonly TableManufacturer ELENOLD = new(6, "ELENOLD", "historické – znakovo orientované – Petržalka", 1, 127);
    public static readonly TableManufacturer ELEN10 = new(7, "ELEN10", "od 2006 – 10-linkové, kód Windows", 1, 127);
    public static readonly TableManufacturer ELEN16 = new(8, "ELEN16", "od 2006 – 16-linkové, kód Windows", 1, 127);
    public static readonly TableManufacturer ELEN16Kam = new(9, "ELEN16Kam", "ako ELEN16, kód Kamenických", 1, 127);
    public static readonly TableManufacturer APEL = new(10, "APEL", "jednoriadková (nást.) s proporc. fontmi", 129, 191);
    public static readonly TableManufacturer APEL_AN = new(11, "APEL_AN", "viacriadková (odj.) s neproporc. fontmi", 129, 191);
    public static readonly TableManufacturer ERP = new(12, "ERP", "slepecké hlásiče Elektročas", 0, 255);
    public static readonly TableManufacturer ELEKON = new(13, "ELEKON", "ELEKON (Mobatime) s protokolom ELEN", 1, 127);

    // INISS tieto kluce nepozna; ostavaju len kvoli datam ulozenym starsimi verziami GVDEditora.
    public static readonly TableManufacturer ELEKTROCAS = new(100, "Elektrocas", "(INISS nepozná)", int.MinValue, int.MinValue);
    public static readonly TableManufacturer Pragotron = new(101, "Pragotron", "(INISS nepozná)", int.MinValue, int.MinValue);
#pragma warning restore 1591

    #endregion
}
