using GVDEditor.Annotations;
using GVDEditor.Entities;
using ToolsCore;
using ToolsCore.Tools;

namespace GVDEditor.Tools;

internal static class RawBankReader
{
    /// <summary>
    ///     Precita a vrati zoznam fyzickych zvukov zo zvukovej banky.
    /// </summary>
    /// <param name="pathToBank">Cesta k zvukovej banke.</param>
    /// <param name="language">Jazyk, z ktoreho sa maju precitat zvuky.</param>
    /// <returns>zoznam fyzickych zvukov zo zvukovej banky.</returns>
    public static List<FyzZvuk> ReadFyzZvukFile([NotNull] string pathToBank, [NotNull] Language language)
    {
        if (string.IsNullOrEmpty(pathToBank))
            throw new ArgumentNullException(nameof(pathToBank));
        if (language == null)
            throw new ArgumentNullException(nameof(language));

        if (language.RelativePath == null || language.FileFyzZvuk == null)
            throw new FormatException(
                $"Nie je možné načítať zvukovú banku jazyka, pretože jazyk {language.Name} definovaný v {FileConsts.FILE_CATEGORI} sa nenachádza v zvukovej banke.");

        var file = pathToBank + Path.DirectorySeparatorChar + language.RelativePath + language.FileFyzZvuk;

        if (File.Exists(file))
        {
            using var reader = new BinaryReader(File.Open(file, FileMode.Open), Encodings.Win1250);
            var countDirs = reader.ReadInt32();
            var sounds = new List<FyzZvuk>();

            for (var i = 0; i < countDirs; i++)
            {
                int dirSkupinaZvukovLength = reader.ReadByte();
                var skupinaZvukovName = reader.ReadBytes(dirSkupinaZvukovLength).ANSItoUTF();

                int dirNameLength = reader.ReadByte();
                var dirName = reader.ReadBytes(dirNameLength).ANSItoUTF();

                int dirRelativnaCestaLength = reader.ReadByte();
                var dirRelativnaCesta = reader.ReadBytes(dirRelativnaCestaLength).ANSItoUTF();

                var countFiles = reader.ReadInt32();

                var fyzZvukDir = new FyzZvukDir(skupinaZvukovName, dirName, dirRelativnaCesta, countFiles);

                for (var j = 0; j < countFiles; j++)
                {
                    int textHlaseniaLength = reader.ReadByte();
                    string textHlasenia;
                    if (textHlaseniaLength == 0xff)
                    {
                        int textHlasenia2Length = reader.ReadInt16();
                        textHlasenia = reader.ReadBytes(textHlasenia2Length).ANSItoUTF();
                    }
                    else
                    {
                        textHlasenia = reader.ReadBytes(textHlaseniaLength).ANSItoUTF();
                    }

                    int zvukLength = reader.ReadByte();
                    var zvuk = reader.ReadBytes(zvukLength).ANSItoUTF();

                    int nazevZvukuProLength = reader.ReadByte();
                    var nazevZvukuPro = reader.ReadBytes(nazevZvukuProLength).ANSItoUTF();

                    int nameFileLength = reader.ReadByte();
                    var nameFile = reader.ReadBytes(nameFileLength).ANSItoUTF();

                    sounds.Add(new FyzZvuk(fyzZvukDir, zvuk, nazevZvukuPro, nameFile, textHlasenia, language));

                    reader.ReadBytes(4);
                }
            }

            return sounds;
        }

        throw new FileNotFoundException($"Súbor s definíciou zvukov sa na zvolenej ceste nenašiel: {file}");
    }

    /// <summary>
    ///     Precita data o zvukovej banke a vrati zoznam jazykov.
    /// </summary>
    /// <param name="pathToBank">Cesta k zvukovej banke.</param>
    /// <param name="maxLangs">Maximalny pocet jazkov, ktore sa mozu nachadzat v globalnych a lokalnych CATEGORI.TXT.</param>
    /// <returns></returns>
    public static List<Language> ReadFyzBankFile([NotNull] string pathToBank, out int maxLangs)
    {
        if (string.IsNullOrEmpty(pathToBank))
            throw new ArgumentNullException(nameof(pathToBank));

        var file = pathToBank + Path.DirectorySeparatorChar + FileConsts.FILE_FYZBANK;

        if (File.Exists(file))
        {
            using var reader = new BinaryReader(File.Open(file, FileMode.Open), Encodings.Win1250);
            var countLangs = reader.ReadInt32();
            maxLangs = countLangs;
            var languages = new List<Language>(countLangs);

            for (var i = 0; i < countLangs; i++)
            {
                int fileFyzZvukLength = reader.ReadByte();
                var fileFyzZvukName = reader.ReadBytes(fileFyzZvukLength).ANSItoUTF();

                int langDirRelativePathLength = reader.ReadByte();
                var langDirRelativePathName = reader.ReadBytes(langDirRelativePathLength).ANSItoUTF();

                int langKeyLength = reader.ReadByte();
                var langKey = reader.ReadBytes(langKeyLength).ANSItoUTF();

                int langNameLength = reader.ReadByte();
                var langName = reader.ReadBytes(langNameLength).ANSItoUTF();

                reader.ReadBytes(4);

                languages.Add(new Language
                {
                    Key = langKey,
                    FileFyzZvuk = fileFyzZvukName,
                    FyzBankName = langName,
                    RelativePath = langDirRelativePathName
                });
            }

            return languages;
        }

        throw new FileNotFoundException($"Súbor s definíciou priečinkov zvukov sa na zvolenej ceste nenašiel: {file}");
    }
}