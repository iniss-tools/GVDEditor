namespace GVDEditor.Domain.Analysis;

/// <summary>
/// Miesto, odkial INISS posle tabuli cislo pisma.
/// </summary>
internal enum ElenFontSource
{
    /// <summary>Zoznam pisiem - INISS ich ponuka pri vkladani informacneho textu.</summary>
    FontList,

    /// <summary>Stlpec katalogovej tabule.</summary>
    Column,

    /// <summary>Text vlaku na tabuli.</summary>
    TrainText,

    /// <summary>Lava strana pravidla TabTab (koncove <c>{n}</c>).</summary>
    TabTab
}
