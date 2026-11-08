namespace GVDEditor.Mcp.Results;

/// <summary>Vysledok zmeny vlaku.</summary>
internal sealed record TrainChangeResult(
    ChangeInfo Change,
    [property: Description("The train after the change; null = deleted.")] TrainDetail? Train,
    [property: Description("Warnings of the train check (the Edit train window shows them too), in the language of the editor.")] IReadOnlyList<string> Warnings);
