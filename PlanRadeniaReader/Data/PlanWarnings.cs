namespace Iniss.PlanRadenia.Data;

/// <summary>
/// Upozornenia citania (<see cref="PlanTrain.Warnings" />, <see cref="PlanNote.Warnings" />): kod a za medzerami udaje.
/// Texty pre pouzivatela ma GVDEditor.
/// </summary>
public static class PlanWarnings
{
    /// <summary>Na konci prveho riadku radenia nie je rusen - vozne su zoradene zlava.</summary>
    public const string NoLocomotive = "NoLocomotive";

    /// <summary>Cislo vozna z poznamky (prvy udaj) opravene na cislo vozna v radeni (druhy udaj).</summary>
    public const string CarFixed = "CarFixed";

    /// <summary>Vozen z poznamky (udaj) v radeni nie je.</summary>
    public const string CarMissing = "CarMissing";
}
