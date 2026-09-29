using ExControls;
using GVDEditor.Domain.Analysis;
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
    /// <param name="inissDir">priecinok instalacie INISS</param>
    /// <param name="warnings">sem sa pridaju varovania z globalnych suborov</param>
    public static InissWorkspace Load(string inissDir, LoadWarnings warnings)
    {
        var dataDir = PathUtils.CombinePath(inissDir, GvdFileConsts.DirData)!;
        var rawBankDir = PathUtils.CombinePath(inissDir, GvdFileConsts.DirRawbank)!;
        var langs = RawBankParser.ReadFyzBankFile(rawBankDir, out var maxLangs);
        var languages = new ExBindingList<FyzLanguage>(CategoriFile.ReadGlobal(dataDir, langs, maxLangs, warnings));
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
            workspace.TrainsTypes = new ExBindingList<TrainType>(TrTypesFile.Read(dataDir, warnings));
        }
        catch (FileNotFoundException)
        {
        }

        try
        {
            workspace.Audios = new ExBindingList<Audio>(AudioFile.Read(dataDir, stations, out var trailer));
            workspace.AudioTrailer = trailer;
        }
        catch (FileNotFoundException)
        {
        }

        return workspace;
    }
}
