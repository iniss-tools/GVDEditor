namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Problem prekladu vyrazu.
/// </summary>
/// <param name="Issue">druh</param>
/// <param name="Detail">vyraz, stav alebo text chyby</param>
internal sealed record Iniss2ExprProblem(Iniss2ExprIssue Issue, string Detail);
