namespace GVDEditor.Integration.Iniss2;

/// <summary>
/// Banka pisem ELEN na skopirovanie.
/// </summary>
/// <param name="Name">nazov banky ulozeny v subore; je to aj meno suboru v Data\TableFonts</param>
/// <param name="Source">subor .bin</param>
internal sealed record Iniss2FontFile(string Name, string Source);
