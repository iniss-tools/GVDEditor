namespace GVDEditor.Integration.PlanRadenia;

/// <summary>
/// Upozornenie k navrhu radenia.
/// </summary>
internal sealed record PlanRadeniaIssue(PlanRadeniaIssueKind Kind, string Text = "");
