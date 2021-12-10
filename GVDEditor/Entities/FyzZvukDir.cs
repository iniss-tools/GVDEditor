#pragma warning disable 1591

namespace GVDEditor.Entities;

/// <summary>
///     Trieda reprezentujúca priečinok s fyzickými zvukmi vo zvukovej banke.
/// </summary>
public sealed class FyzZvukDir
{
    /// <summary>
    ///     Konstruktor
    /// </summary>
    /// <param name="soundGroup">názov skupiny zvukov (napr. Linka)</param>
    /// <param name="name">názov priečinka (napr. Linka)</param>
    /// <param name="relativePath">relatívna cesta k priečinku (napr. \Linka)</param>
    /// <param name="countFilesInDir">počet súborov v tomto priečinku, ktoré sú deklarované v súbore FYZZVUK.DAT</param>
    public FyzZvukDir(string soundGroup, string name, string relativePath, int countFilesInDir)
    {
        SoundGroup = soundGroup;
        Name = name;
        RelativePath = relativePath;
        CountFilesInDir = countFilesInDir;
        DirType = FyzZvukDirType.Parse(name);
    }

    /// <summary>
    ///     Názov skupiny so zvukmi.
    /// </summary>
    public string SoundGroup { get; }

    /// <summary>
    ///     Názov priečinka.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Relatívna cesta k priečinku (formát: \(nazov)).
    /// </summary>
    public string RelativePath { get; }

    /// <summary>
    ///     Počet súborov v priečinku.
    /// </summary>
    public int CountFilesInDir { get; }

    /// <summary>
    ///     Typ priečinka podľa zvukov, ktoré obsahuje.
    /// </summary>
    public FyzZvukDirType DirType { get; }

    /// <summary>
    ///     Porovnava priecinky s fyzickymi zvukmi.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        return obj is FyzZvukDir fyz && string.Equals(fyz.Name, Name) &&
               string.Equals(fyz.RelativePath, RelativePath);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SoundGroup != null ? SoundGroup.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (RelativePath != null ? RelativePath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ CountFilesInDir;
            hashCode = (hashCode * 397) ^ (DirType != null ? DirType.GetHashCode() : 0);
            return hashCode;
        }
    }
}