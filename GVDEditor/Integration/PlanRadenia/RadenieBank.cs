using ToolsCore.Iniss.Entities;

namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Zvuky banky v jednom jazyku grafikonu.
/// </summary>
internal sealed record RadenieBank(FyzLanguage Language, IReadOnlyList<FyzSound> Sounds);
