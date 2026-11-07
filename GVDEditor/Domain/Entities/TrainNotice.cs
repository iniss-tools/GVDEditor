namespace GVDEditor.Domain.Entities;

/// <summary>
/// Vyluka alebo odklon, ktore obsluha INISS 3.39 priradila vlaku na obdobie (riadok Vyluky1.txt alebo Odklony1.txt).
/// </summary>
/// <param name="Number">cislo vlaku</param>
/// <param name="Station">cislo stanice; 0 = bez stanice</param>
/// <param name="Code">kod textu; kluc V&lt;kod&gt; alebo O&lt;kod&gt; v logickej zvukovej banke</param>
/// <param name="From">prvy den obdobia</param>
/// <param name="To">posledny den obdobia</param>
/// <param name="Days">obmedzenie dni v obdobi; prazdne = vsetky dni</param>
public sealed record TrainNotice(string Number, int Station, int Code, DateOnly From, DateOnly To, string Days);
