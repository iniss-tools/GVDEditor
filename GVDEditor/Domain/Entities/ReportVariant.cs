namespace GVDEditor.Domain.Entities;

/// <summary>
///     Definuje varianty reportu
/// </summary>
/// <param name="Key">Kluc varianty reportu.</param>
/// <param name="Name">Nazov varianty reportu.</param>
public sealed record ReportVariant(int Key, string Name)
{
    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    ///     Vrati predvolene hodnoty variant reportov. Na poradi zalezi: prvy variant (VARIANT_01, velke pismeno typu)
    ///     je v INISSe dlhe hlasenie, druhy (VARIANT_02, male pismeno) kratke.
    /// </summary>
    /// <returns></returns>
    public static List<ReportVariant> GetDefaultValues()
    {
        var variants = new List<ReportVariant> { DlheHlasenie, KratkeHlasenie };
        return variants;
    }

    /// <summary>
    ///     Starsie verzie GVDEditora zakladali grafikony s prehodenymi nazvami variantov (VARIANT_01 = Kratke hlasenie).
    ///     INISS nazvy nepouziva, takze sa opravia len popisky; pismena a mapy vlakov ostanu bezo zmeny.
    ///     Opravi sa len presne tato dvojica nazvov, vlastne nazvy pouzivatela sa nemenia.
    /// </summary>
    /// <param name="variants">varianty nacitane z Categori.txt (nie instancie z <see cref="GetDefaultValues" />)</param>
    /// <returns>true, ak sa nazvy opravili</returns>
    public static bool FixSwappedDefaultNames(IList<ReportVariant> variants)
    {
        if (variants.Count != 2 || variants[0].Name != KratkeHlasenie.Name || variants[1].Name != DlheHlasenie.Name)
            return false;

        // variant je nemenny - volat sa musi pred nacitanim vlakov, ktore sa na varianty odkazuju
        variants[0] = variants[0] with { Name = DlheHlasenie.Name };
        variants[1] = variants[1] with { Name = KratkeHlasenie.Name };
        return true;
    }

#pragma warning disable 1591
    public static readonly ReportVariant DlheHlasenie = new(0, "Dlhé hlásenie");
    public static readonly ReportVariant KratkeHlasenie = new(1, "Krátke hlásenie");
#pragma warning restore 1591
}