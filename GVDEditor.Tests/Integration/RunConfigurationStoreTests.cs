using System.Diagnostics.CodeAnalysis;
using GVDEditor.Config;
using GVDEditor.Integration;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Ulozenie konfiguracii spustania - config.xml (tento pocitac) a data instalacie (zdielane).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class RunConfigurationStoreTests
{
    private string _dir = null!;

    [TestInitialize]
    public void Init()
    {
        _dir = Path.Combine(Path.GetTempPath(), "gvd-run-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, true);
    }

    [TestMethod]
    public void BezUlozenych_PredvoleneZProgramovANicSaNezapise()
    {
        var config = new GVDEditorConfig { StartupINISSConfig = new StartupINISS { CmdArgs = "/Multiuse" } };

        var set = RunConfigurationStore.Load(config, _dir, ["INISS - A.exe"]);

        Assert.IsTrue(set.IsDefault);
        Assert.AreEqual("A", set.Selected!.Name);
        Assert.IsTrue(set.Selected.Multiuse);
        Assert.IsEmpty(config.RunConfigurations);
        Assert.IsFalse(Directory.Exists(Path.Combine(_dir, RunConfigurationStore.SharedDirName)));
    }

    [TestMethod]
    public void Ulozenie_LokalneDoKonfiguracieZdielaneDoDat()
    {
        var config = new GVDEditorConfig();
        var local = new RunConfiguration { Name = "Lokalna", Program = "INISS.exe" };
        var shared = new RunConfiguration { Name = "Zdielana", Program = "INISS.exe", Registry = "Test", Shared = true };
        var set = new RunConfigurationSet(_dir, [local, shared], shared.Id);

        RunConfigurationStore.Save(config, set);
        var back = RunConfigurationStore.Load(config, _dir + Path.DirectorySeparatorChar, []);

        Assert.IsTrue(File.Exists(RunConfigurationStore.SharedPath(_dir)));
        Assert.AreEqual("Lokalna", config.RunConfigurations.Single().Items.Single().Name);
        Assert.IsFalse(back.IsDefault);
        CollectionAssert.AreEqual(new[] { "Lokalna", "Zdielana" }, back.Items.Select(i => i.Name).ToArray());
        Assert.AreEqual(shared.Id, back.Selected!.Id);
        Assert.IsTrue(back.Selected.Shared);
        Assert.AreEqual("Test", back.Selected.Registry);
        Assert.IsFalse(back.Find(local.Id)!.Shared);
    }

    [TestMethod]
    public void BezZdielanych_SuborAPriecinokSaOdstrania()
    {
        var config = new GVDEditorConfig();
        var shared = new RunConfiguration { Name = "Z", Program = "INISS.exe", Shared = true };
        var set = new RunConfigurationSet(_dir, [shared], null);
        RunConfigurationStore.Save(config, set);

        shared.Shared = false;
        RunConfigurationStore.Save(config, set);

        Assert.IsFalse(Directory.Exists(Path.Combine(_dir, RunConfigurationStore.SharedDirName)));
        Assert.AreEqual("Z", RunConfigurationStore.Load(config, _dir, []).Items.Single().Name);
    }

    [TestMethod]
    public void PoskodenyZdielanySubor_ChybaSLokalnymiKonfiguraciami()
    {
        var config = new GVDEditorConfig();
        RunConfigurationStore.Save(config, new RunConfigurationSet(_dir, [new RunConfiguration { Name = "L", Program = "INISS.exe" }], null));
        Directory.CreateDirectory(Path.Combine(_dir, RunConfigurationStore.SharedDirName));
        File.WriteAllText(RunConfigurationStore.SharedPath(_dir), "<RunConfigurations><nie-je-xml");

        var e = Assert.ThrowsExactly<RunConfigurationLoadException>(() => RunConfigurationStore.Load(config, _dir, []));

        Assert.AreEqual("L", e.Partial.Items.Single().Name);
    }

    [TestMethod]
    public void Vyber_ZapiseSaBezZdielanehoSuboru()
    {
        var config = new GVDEditorConfig();
        var a = new RunConfiguration { Name = "A", Program = "A.exe" };
        var b = new RunConfiguration { Name = "B", Program = "B.exe" };
        var set = new RunConfigurationSet(_dir, [b, a], a.Id) { IsDefault = true };

        set.SelectedId = b.Id;
        RunConfigurationStore.SaveSelection(config, set);

        var back = RunConfigurationStore.Load(config, _dir, []);
        Assert.AreEqual("B", back.Selected!.Name);
        CollectionAssert.AreEqual(new[] { "A", "B" }, back.Items.Select(i => i.Name).ToArray());
        Assert.IsFalse(File.Exists(RunConfigurationStore.SharedPath(_dir)));
    }
}
