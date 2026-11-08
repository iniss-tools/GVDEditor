namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok zmeny sekcie TabTab.</summary>
internal sealed record TabTabChangeResult(
    ChangeInfo Change,
    [property: Description("The section after the change with its check findings; null = deleted.")] TabTabInfo? TabTab);
