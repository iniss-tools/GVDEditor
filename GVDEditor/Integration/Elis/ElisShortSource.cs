namespace GVDEditor.Integration.Elis;

/// <summary>
/// Odkial je navrh kratkeho hlasenia pre stanicu.
/// </summary>
internal enum ElisShortSource
{
    /// <summary>Bez navrhu.</summary>
    None,

    /// <summary>Volba z predchadzajuceho importu.</summary>
    Saved,

    /// <summary>Ako ju hlasia vlaky grafikonu.</summary>
    Grafikon,

    /// <summary>Podla druhu stanice v ELIS (uzlova alebo koncova stanica).</summary>
    ElisType
}
