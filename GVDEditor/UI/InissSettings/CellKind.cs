namespace GVDEditor.UI.InissSettings;

/// <summary>Ako sa hodnota upravuje v tabulke.</summary>
internal enum CellKind
{
    /// <summary>Neupravuje sa (binarne, mimo katalogu).</summary>
    ReadOnly,

    /// <summary>Zaskrtavacie pole (nula / nenula).</summary>
    Check,

    /// <summary>Zoznam vymenovanych hodnot.</summary>
    Choice,

    /// <summary>Text (cislo, text, farba #RRGGBB).</summary>
    Text
}
