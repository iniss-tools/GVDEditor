namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Co sa pri preklade vyrazu INISS 3.39 do VB.NET nedalo preniest presne.
/// </summary>
internal enum Iniss2ExprIssue
{
    /// <summary>Vyraz sa neda prelozit ani v INISS 3.39; detail je text chyby.</summary>
    ParseError,

    /// <summary>Stav vlaku, ktory INISS2 nepozna; podmienka neplati nikdy.</summary>
    UnknownState,

    /// <summary>Udalost GTN nie je konstanta; automatika na udalost necaka.</summary>
    WaitNotConstant
}
