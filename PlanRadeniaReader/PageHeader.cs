namespace Iniss.PlanRadenia;

/// <summary>
/// Hlavicka strany: nazov stanice a platnost planu.
/// </summary>
internal sealed record PageHeader(string Station, string ValidFrom, string ValidTo);
