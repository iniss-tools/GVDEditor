namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok zmeny tabule alebo pisma.</summary>
internal sealed record TableChangeResult(
    ChangeInfo Change,
    [property: Description("The item after the change; null = deleted.")] TableDetail? Table,
    [property: Description("What the change did besides, or left to do (e.g. TabTab sections still using an old font number).")] IReadOnlyList<string> Notes);
