using System.Globalization;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Editing;
using GVDEditor.Domain.Entities;
using GVDEditor.Domain.Rules;
using GVDEditor.Formats;
using GVDEditor.Properties;
using ToolsCore.Tools;
using static GVDEditor.Formats.GvdFileConsts;

namespace GVDEditor.Services;

/// <summary>
/// Operacie nad grafikonmi instalacie bez okien: nacitanie, ulozenie, novy grafikon, import a ulozenie globalnych
/// nastaveni. Otazky a hlasenia pouzivatelovi ostavaju v hlavnom okne.
/// </summary>
internal static class GrafikonService
{
    /// <summary>
    /// Nacita grafikon na pozadi do noveho dokumentu - otvoreny grafikon sa medzitym nemeni.
    /// </summary>
    public static Task<GrafikonDocument> LoadAsync(GVDDirectory dir, InissWorkspace workspace) =>
        Task.Run(() => GrafikonRepository.Load(dir.Dir.FullPath, dir.GVD, workspace));

    /// <summary>
    /// Ulozi grafikon (transakcne, pozri <see cref="GrafikonRepository.Save" />). Pred ulozenim podla nastavenia
    /// pregeneruje texty tabul a do hlavicky zapise pocet vlakov a datum upravy.
    /// </summary>
    /// <exception cref="InvalidOperationException">ulozenie zlyhalo; sprava je urcena pouzivatelovi</exception>
    public static void Save(GVDDirectory dir, GrafikonContext context, bool autoTableText)
    {
        var document = context.Document;
        if (autoTableText)
            TableTextGenerating.RegenerateAll(document.TableTexts, document.Trains, dir.GVD.ThisStation);

        dir.GVD.TrainCount = document.Trains.Count;
        dir.GVD.CreateData = DateOnly.FromDateTime(DateTime.Today);

        GrafikonRepository.Save(dir.Dir.FullPath, dir.GVD, context);
    }

    /// <summary>
    /// Zaregistruje novy grafikon: prida ho do DirList.TXT, vytvori jeho priecinok a hlavicku. Subory grafikonu
    /// vytvori az <see cref="GrafikonRepository.CreateNew" /> pri jeho otvoreni.
    /// </summary>
    public static GVDDirectory Register(InissWorkspace workspace, DirList dir, GVDInfo gvd)
    {
        var dirs = DirListFile.Read(workspace.DataDir);
        dirs.Add(dir);
        DirListFile.Write(workspace.DataDir, dirs);
        workspace.GVDDirs.Add(dir);

        Directory.CreateDirectory(dir.FullPath);
        InfoGvdFile.Write(dir.FullPath, gvd);
        return new GVDDirectory(dir, gvd);
    }

    /// <summary>
    /// Importuje grafikon z priecinka <paramref name="sourcePath" />: skontroluje ho, skopiruje do DATA (ak uz
    /// v nom nie je) a zaregistruje v DirList.TXT. Grafikon sa dalej upravuje v kopii, nie v povodnom priecinku.
    /// </summary>
    /// <exception cref="InvalidOperationException">priecinok neobsahuje platny grafikon alebo ho nemozno importovat;
    /// sprava je urcena pouzivatelovi</exception>
    public static GVDDirectory Import(InissWorkspace workspace, string sourcePath, IReadOnlyCollection<GVDDirectory> existing)
    {
        // hlavicka sa cita zo zdroja - neplatny grafikon sa do DATA vobec neskopiruje
        GVDInfo gvd;
        try
        {
            gvd = InfoGvdFile.Read(sourcePath);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"{Resources.FMain_Priečinok_neobsahuje_všetky_potrebné_dáta} {e.Message}", e);
        }

        var error = GVDImport.Check(sourcePath, workspace.DataDir, gvd, existing, out var newDirPath);
        if (error != null)
            throw new InvalidOperationException(error);

        if (!string.Equals(Path.GetFullPath(sourcePath).TrimEnd(Path.DirectorySeparatorChar), Path.GetFullPath(newDirPath),
                StringComparison.OrdinalIgnoreCase))
            Utils.CopyDirectory(sourcePath, newDirPath);

        var dirList = new DirList { DirName = Path.GetFileName(newDirPath), FullPath = newDirPath };
        var dirs = DirListFile.Read(workspace.DataDir);
        dirs.Add(dirList);
        DirListFile.Write(workspace.DataDir, dirs);
        workspace.GVDDirs.Add(dirList);

        return new GVDDirectory(dirList, gvd);
    }

    /// <summary>
    /// Zapise globalne nastavenia instalacie (zoznam grafikonov, typy vlakov, meskania, audio, jazyky) transakcne -
    /// pri chybe sa subory v DATA vratia do povodneho stavu. Otvoreny grafikon prevezme zmenu jazykov.
    /// </summary>
    /// <exception cref="InvalidOperationException">ulozenie zlyhalo; sprava je urcena pouzivatelovi</exception>
    public static void SaveGlobalSettings(InissWorkspace workspace, List<DirList> dirs, GrafikonDocument document)
    {
        var dataDir = workspace.DataDir;
        var transaction = new FileTransaction(new[] { FILE_DIRLIST, FILE_TRTYPES, FILE_ZPOZDENI, FILE_ZPOZDENI_DAT, FILE_AUDIO, FILE_CATEGORI }
            .Select(file => Path.Combine(dataDir, file)));
        try
        {
            DirListFile.Write(dataDir, dirs);
            TrTypesFile.Write(dataDir, workspace.TrainsTypes);
            ZpozdeniFile.Write(dataDir, workspace.Delays);
            AudioFile.Write(dataDir, workspace.Audios);
            CategoriFile.WriteGlobal(dataDir, workspace.Languages.ToList());
        }
        catch (Exception exception)
        {
            Log.Exception(exception);

            var message = transaction.TryRollback()
                ? string.Format(CultureInfo.CurrentCulture, Resources.GlobalSettings_Ulozenie_zlyhalo_vratene, exception.Message)
                : string.Format(CultureInfo.CurrentCulture, Resources.GlobalSettings_Ulozenie_zlyhalo_neobnovene, exception.Message,
                    transaction.BackupPath);
            throw new InvalidOperationException(message, exception);
        }

        transaction.Commit();
        workspace.GVDDirs = dirs;

        // jazyky grafikonu si ponechavaju vlastny vyber - zmazany jazyk z neho vypadne, novy si zapne pouzivatel
        document.LocalLanguages = GrafikonLanguageRules.Sync(document.LocalLanguages, workspace.Languages);

        // odstraneny jazyk nesmie ostat pri vlakoch - zapisal by sa do Foreign.txt
        foreach (var train in document.Trains)
            train.Languages.RemoveAll(l => !workspace.Languages.Contains(l));
    }
}
