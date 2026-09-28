using ExControls;
using GVDEditor.Domain.Documents;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using GVDEditor.Services;
using ToolsCore.Entities;
using ToolsCore.Tools;

namespace GVDEditor.Tests.Services;

/// <summary>
/// Operacie nad grafikonmi instalacie bez okien - novy grafikon, import a ulozenie globalnych nastaveni.
/// </summary>
[TestClass]
public class GrafikonServiceTests
{
    private static readonly FyzLanguage Sk = new("SK", "Slovenčina", "SK\\") { IsBasic = true };

    private string _root = null!;
    private string _data = null!;

    [TestInitialize]
    public void Init()
    {
        _root = Directory.CreateTempSubdirectory("gvdservice").FullName;
        _data = Directory.CreateDirectory(Path.Combine(_root, "INISS", "DATA")).FullName;
    }

    [TestCleanup]
    public void Cleanup()
    {
        foreach (var file in Directory.GetFiles(_root, "*", SearchOption.AllDirectories))
            File.SetAttributes(file, FileAttributes.Normal);
        Directory.Delete(_root, true);
    }

    private InissWorkspace Workspace()
    {
        DirListFile.Write(_data, []);
        return new InissWorkspace
        {
            DataDir = _data,
            GVDDirs = DirListFile.Read(_data),
            Languages = [Sk],
            TrainsTypes = new ExBindingList<TrainType>(TrainType.GetDefaultValues()),
            Delays = ["5", "10"],
            Audios = []
        };
    }

    private static GVDInfo Gvd(string station) => new()
    {
        ThisStation = new Station("5000", station),
        StartValidTimeTable = new DateOnly(2026, 12, 13), EndValidTimeTable = new DateOnly(2027, 12, 11),
        StartValidData = new DateOnly(2026, 12, 13), EndValidData = new DateOnly(2027, 12, 11),
        CreateData = new DateOnly(2026, 11, 1)
    };

    [TestMethod]
    public void Register_NovyGrafikon_ZapiseDirListAHlavicku()
    {
        var workspace = Workspace();
        var dir = new DirList { DirName = "Dolne.2026", FullPath = Path.Combine(_data, "Dolne.2026") };

        var created = GrafikonService.Register(workspace, dir, Gvd("Dolné Mesto"));

        Assert.AreSame(dir, created.Dir);
        CollectionAssert.AreEqual(new[] { "Dolne.2026" }, DirListFile.Read(_data).Select(d => d.DirName).ToList());
        Assert.HasCount(1, workspace.GVDDirs);
        Assert.AreEqual("Dolné Mesto", InfoGvdFile.Read(dir.FullPath).ThisStation.Name);
    }

    [TestMethod]
    public void Import_PlatnyGrafikon_SkopirujeDoDataAZaregistruje()
    {
        var workspace = Workspace();
        var source = Directory.CreateDirectory(Path.Combine(_root, "Zaloha", "Horna.2026")).FullName;
        InfoGvdFile.Write(source, Gvd("Horné Mesto"));

        var imported = GrafikonService.Import(workspace, source, []);

        Assert.AreEqual(Path.Combine(_data, "Horna.2026"), imported.Dir.FullPath);
        Assert.IsTrue(File.Exists(Path.Combine(imported.Dir.FullPath, GvdFileConsts.FILE_GRAFIKON)));
        Assert.AreEqual("Horné Mesto", imported.GVD.ThisStation.Name);
        CollectionAssert.AreEqual(new[] { "Horna.2026" }, DirListFile.Read(_data).Select(d => d.DirName).ToList());
        Assert.HasCount(1, workspace.GVDDirs);
    }

    [TestMethod]
    public void Import_PriecinokBezGrafikonu_VynimkaANicSaNezmeni()
    {
        var workspace = Workspace();
        var source = Directory.CreateDirectory(Path.Combine(_root, "Prazdny")).FullName;

        Assert.ThrowsExactly<InvalidOperationException>(() => GrafikonService.Import(workspace, source, []));

        Assert.IsEmpty(DirListFile.Read(_data));
        Assert.IsEmpty(workspace.GVDDirs);
        Assert.IsFalse(Directory.Exists(Path.Combine(_data, "Prazdny")));
    }

    [TestMethod]
    public void SaveGlobalSettings_UlozenieZapiseSuboryAJazykyGrafikonu()
    {
        var workspace = Workspace();
        var gb = new FyzLanguage("GB", "Angličtina", "GB\\");
        var document = GrafikonDocument.CreateNew([Sk, gb]);
        var train = new Train { Languages = [Sk, gb] };
        document.Trains.Add(train);
        var dirs = new List<DirList> { new() { DirName = "Dolne.2026", FullPath = Path.Combine(_data, "Dolne.2026") } };

        GrafikonService.SaveGlobalSettings(workspace, dirs, document);

        Assert.AreSame(dirs, workspace.GVDDirs);
        CollectionAssert.AreEqual(new[] { "Dolne.2026" }, DirListFile.Read(_data).Select(d => d.DirName).ToList());
        CollectionAssert.AreEqual(new[] { "5", "10" }, ZpozdeniFile.Read(_data));
        // odstraneny jazyk vypadne z grafikonu aj z vlakov
        CollectionAssert.AreEqual(new[] { Sk }, document.LocalLanguages);
        CollectionAssert.AreEqual(new[] { Sk }, train.Languages);
    }

    [TestMethod]
    public void SaveGlobalSettings_ChybaZapisu_VratiSuboryDoPovodnehoStavu()
    {
        var workspace = Workspace();
        ZpozdeniFile.Write(_data, ["15"]);
        var cache = Path.Combine(_data, GvdFileConsts.FILE_ZPOZDENI_DAT);
        File.WriteAllText(cache, "cache");
        var audio = Path.Combine(_data, GvdFileConsts.FILE_AUDIO);
        File.WriteAllText(audio, "");
        File.SetAttributes(audio, FileAttributes.ReadOnly);
        var dirs = new List<DirList> { new() { DirName = "Dolne.2026", FullPath = Path.Combine(_data, "Dolne.2026") } };
        var original = workspace.GVDDirs;

        var e = Assert.ThrowsExactly<InvalidOperationException>(() =>
            GrafikonService.SaveGlobalSettings(workspace, dirs, GrafikonDocument.CreateNew([Sk])));

        StringAssert.Contains(e.Message, "DATA");
        Assert.AreSame(original, workspace.GVDDirs);
        Assert.IsEmpty(DirListFile.Read(_data));
        CollectionAssert.AreEqual(new[] { "15" }, ZpozdeniFile.Read(_data));
        Assert.AreEqual("cache", File.ReadAllText(cache), "vyrovnavacia pamat meskani sa musi vratit");
    }
}
