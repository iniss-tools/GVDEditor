using ExControls;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using ToolsCore.Iniss.Entities;
using ToolsCore.Iniss.Tools;

namespace GVDEditor.Formats;

/// <summary>
/// Nacitanie instalacie INISS: zoznam grafikonov, zvukova banka a globalne nastavenia stanice.
/// </summary>
internal static class WorkspaceRepository
{
    /// <summary>
    /// Nacita instalaciu INISS z priecinka <paramref name="inissDir" />.
    /// </summary>
    public static InissWorkspace Load(string inissDir)
    {
        var dataDir = PathUtils.CombinePath(inissDir, GvdFileConsts.DIR_DATA)!;
        var rawBankDir = PathUtils.CombinePath(inissDir, GvdFileConsts.DIR_RAWBANK)!;
        var langs = RawBankParser.ReadFyzBankFile(rawBankDir, out var maxLangs);
        var languages = new ExBindingList<FyzLanguage>(CategoriFile.ReadGlobal(dataDir, langs, maxLangs));
        var sounds = RawBankParser.ReadFyzZvukFile(rawBankDir, FyzLanguage.GetBasicLanguage(languages)!);
        // zoznamy stanic v oknach su podla nazvu
        var stations = Station.GetStations(sounds);
        stations.Sort();

        var workspace = new InissWorkspace
        {
            INISSDir = inissDir,
            DataDir = dataDir,
            RawBankDir = rawBankDir,
            GVDDirs = DirListFile.Read(dataDir),
            INISSExeFiles = new DirectoryInfo(inissDir).GetFiles("*.exe").Select(file => file.Name).ToList(),
            Languages = languages,
            Sounds = sounds,
            LogZvukTexts = LogZvukParser.ReadLogZvukUsr(rawBankDir),
            TrainNames = Train.GetTrainNames(sounds),
            Stations = stations,
            Delays = new ExBindingList<string>(ZpozdeniFile.Read(dataDir)),
            TrainsTypes = [],
            Audios = []
        };

        try
        {
            workspace.TrainsTypes = new ExBindingList<TrainType>(TrTypesFile.Read(dataDir));
        }
        catch (FileNotFoundException)
        {
        }

        try
        {
            workspace.Audios = new ExBindingList<Audio>(AudioFile.Read(dataDir, stations));
        }
        catch (FileNotFoundException)
        {
        }

        return workspace;
    }
}
