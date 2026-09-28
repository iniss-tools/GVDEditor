using System.Diagnostics.CodeAnalysis;
using GVDEditor.Domain.Entities;
using GVDEditor.Formats;
using ToolsCore.Tools;

namespace GVDEditor.Tests.Formats;

/// <summary>
/// DirList.TXT: grafikon priamo v DATA (bez DirList.TXT) nesmie zapis zoznamu skryt pred INISSom - existujuci
/// prazdny subor INISS berie ako prazdny zoznam a nenacita ziadny grafikon.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class DirListFileTests
{
    // "~" = grafikon priamo v DATA, "|" oddeluje polozky aj riadky, null = subor neexistuje
    private const string DataRoot = "~";

    private string _dir = null!;

    [TestInitialize]
    public void Init()
    {
        _dir = Path.Combine(Path.GetTempPath(), "DirListFileTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, true);

    private string File_ => Path.Combine(_dir, GvdFileConsts.FILE_DIRLIST);

    private List<DirList> Dirs(string names) =>
        names.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(n => n == DataRoot
                ? new DirList { DirName = "", FullPath = _dir }
                : new DirList { DirName = n, FullPath = Path.Combine(_dir, n) })
            .ToList();

    [TestMethod]
    [DataRow(null, DataRoot, false, null, DisplayName = "len grafikon v DATA, subor chyba - nezalozi sa")]
    [DataRow("A.2019,,,,", DataRoot, false, "A.2019,,,,", DisplayName = "len grafikon v DATA, subor existuje - neprepise sa")]
    [DataRow(null, "", false, null, DisplayName = "prazdny zoznam, subor chyba - nezalozi sa")]
    [DataRow("A.2019,,,,", "", true, "", DisplayName = "vsetky grafikony odstranene - subor sa vyprazdni")]
    [DataRow(null, DataRoot + "|B.2020", true, "B.2020,,,,", DisplayName = "novy grafikon vedla DATA - zapise sa len novy")]
    [DataRow("A.2019,,,,", "A.2019|B.2020", true, "A.2019,,,,|B.2020,,,,", DisplayName = "bezne priecinky - zapisu sa")]
    public void DirList_ZapisBezRiadkov_NezaloziAniNeprepiseSubor(string? before, string names, bool expectedWritten, string? expectedAfter)
    {
        if (before != null)
            File.WriteAllLines(File_, before.Split('|'), Encodings.Win1250);

        var written = DirListFile.WriteFile(File_, Dirs(names));

        Assert.AreEqual(expectedWritten, written);
        if (expectedAfter == null)
        {
            Assert.IsFalse(File.Exists(File_));
            return;
        }

        var after = File.ReadAllLines(File_, Encodings.Win1250);
        CollectionAssert.AreEqual(expectedAfter.Split('|', StringSplitOptions.RemoveEmptyEntries), after);
    }
}
