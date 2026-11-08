namespace Iniss.Elis;

/// <summary>
/// Stanica aj s jej cislom v ciselniku dopravcu (pre zeleznicu kod SR70).
/// </summary>
/// <param name="Code">Cislo stanice, napr. 5613600.</param>
/// <param name="Name">Nazov stanice tak, ako ho uvadza ELIS.</param>
internal readonly record struct ElisStationCode(int Code, string Name);
