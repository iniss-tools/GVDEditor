using ToolsCore.Tools;

namespace GVDEditor.Tools;

/// <summary>
///     Zabezpečí, že sa obsah priečinka zmení buď celý, alebo vôbec.
/// </summary>
/// <remarks>
///     Ukladanie grafikonu zapisuje pätnásť súborov po sebe. Keď niektorý zápis zlyhá,
///     časť súborov je nová a časť pôvodná - grafikon sa potom pri otvorení hlási ako chybný.
///     Táto trieda si pred zápisom odloží kópiu pôvodných súborov a pri chybe ich vráti späť.
///     <para>
///         Zálohujú sa len súbory priamo v priečinku, nie podpriečinky (napr. písma tabúľ) -
///         do tých ukladanie nezasahuje.
///     </para>
/// </remarks>
internal sealed class FileTransaction
{
    private readonly string _path;
    private readonly HashSet<string> _originalFiles;

    /// <summary>
    ///     Odloží si kópiu súčasného obsahu priečinka.
    /// </summary>
    /// <param name="path">Priečinok, ktorého obsah sa bude meniť.</param>
    /// <exception cref="IOException">ak sa zálohu nepodarí vytvoriť - vtedy sa nesmie začať zapisovať</exception>
    public FileTransaction(string path)
    {
        _path = path;
        BackupPath = Utils.CombinePath(Path.GetTempPath(), "GVDEditor", "save-" + Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(BackupPath);

        var files = Directory.GetFiles(_path);
        _originalFiles = new HashSet<string>(files.Select(Path.GetFileName), StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
            File.Copy(file, Utils.CombinePath(BackupPath, Path.GetFileName(file)), true);
    }

    /// <summary>
    ///     Priečinok so zálohou pôvodných súborov.
    /// </summary>
    public string BackupPath { get; }

    /// <summary>
    ///     Potvrdí zmeny - záloha sa zahodí.
    /// </summary>
    public void Commit() => TryDeleteBackup();

    /// <summary>
    ///     Vráti priečinok do stavu spred zápisu.
    /// </summary>
    /// <remarks>
    ///     Volá sa z bloku catch, preto nikdy nevyhadzuje výnimku - keby to spravila,
    ///     nahradila by pôvodnú chybu a tá by sa k používateľovi nedostala.
    /// </remarks>
    /// <returns><see langword="false" />, ak sa obnovenie nepodarilo; záloha vtedy zostáva zachovaná.</returns>
    public bool TryRollback()
    {
        try
        {
            foreach (var backup in Directory.GetFiles(BackupPath))
                File.Copy(backup, Utils.CombinePath(_path, Path.GetFileName(backup)), true);

            //súbory, ktoré vznikli až počas neúspešného ukladania, tam pôvodne neboli
            foreach (var file in Directory.GetFiles(_path))
                if (!_originalFiles.Contains(Path.GetFileName(file)))
                    File.Delete(file);
        }
        catch (Exception e)
        {
            Log.Exception(e);
            return false;
        }

        TryDeleteBackup();
        return true;
    }

    private void TryDeleteBackup()
    {
        try
        {
            if (Directory.Exists(BackupPath))
                Directory.Delete(BackupPath, true);
        }
        catch (Exception e)
        {
            //neodstránená záloha nič nerozbíja, len zaberá miesto v TEMPe
            Log.Exception(e);
        }
    }
}
