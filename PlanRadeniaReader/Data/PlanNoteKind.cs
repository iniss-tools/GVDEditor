namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Druh poznamky pod radenim.
/// </summary>
public enum PlanNoteKind
{
    /// <summary>Text sa nepodarilo zaradit.</summary>
    Other,

    /// <summary>Kedy vozne idu: <c>č. 21, č. 22 - ide do 15.IX.</c></summary>
    Restriction,

    /// <summary>Kam vozne idu: <c>č. 344 - Krakow Glowny</c></summary>
    Destination,

    /// <summary>Vozne pridane v stanici: <c>Vozne č. 1, 2 budú zaradené do 31.VIII. vo vlaku od ŽST Banská Bystrica.</c></summary>
    Joined,
}
