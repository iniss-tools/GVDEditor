namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Funkcia dodatku, ktoru export doplni do skriptu hlaseni cielovej instalacie.
/// </summary>
/// <param name="Name">nazov funkcie - kluc dodatku <c>D&lt;kod&gt;</c></param>
/// <param name="Description">popis pre obsluhu - text nahravky z banky INISS 3.39</param>
/// <param name="Sound">nahravka v tvare <c>skupina/zvuk</c>; jazyk doplni server podla hlaseneho jazyka</param>
internal sealed record LogTextAddition(string Name, string Description, string Sound);
