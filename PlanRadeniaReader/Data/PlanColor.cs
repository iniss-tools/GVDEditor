namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Farba karoserie vozna a jej podiel.
/// </summary>
public sealed class PlanColor
{
    /// <summary>Nazov farby: green, yellow, red, navy, lightblue, gray, white.</summary>
    public string Name { get; set; } = "";

    /// <summary>Podiel karoserie (0-1).</summary>
    public double Share { get; set; }
}
