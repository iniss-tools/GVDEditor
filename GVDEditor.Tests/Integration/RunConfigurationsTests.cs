using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using GVDEditor.Config;
using GVDEditor.Domain.Entities;
using GVDEditor.Integration;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Konfiguracie spustania INISSu - prikazovy riadok, prenos povodneho nastavenia, nazvy, aktivatory DirList.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class RunConfigurationsTests
{
    [TestMethod]
    public void Argumenty_PrepinaceVetvaAktivatoryADalsie()
    {
        var config = new RunConfiguration
        {
            Program = "INISS.exe", Multiuse = true, NoRestore = true, Registry = " INISSTest ", Activators = "92", ExtraArguments = " /Foo "
        };

        Assert.AreEqual("/Multiuse /NoRestore /Reg:INISSTest /2 /9 /Foo", RunConfigurations.Arguments(config));
        Assert.AreEqual("\"INISS.exe\" /Multiuse /NoRestore /Reg:INISSTest /2 /9 /Foo", RunConfigurations.CommandLine(config));
        Assert.AreEqual("INISSTest", RunConfigurations.AppName(config));
    }

    [TestMethod]
    public void BezParametrov_LenProgramAVetvaPodlaMena()
    {
        var config = new RunConfiguration { Program = "INISS - Bardejov.exe" };

        Assert.AreEqual("", RunConfigurations.Arguments(config));
        Assert.AreEqual("\"INISS - Bardejov.exe\"", RunConfigurations.CommandLine(config));
        Assert.AreEqual("INISS - Bardejov", RunConfigurations.AppName(config));
    }

    [TestMethod]
    public void ZArgumentov_RozlozeneZnamePrepinaceVetvaAktivatory()
    {
        var config = RunConfigurations.FromArguments("A", "INISS.exe", true, "-minimize /EXPORTHLAS /Reg:\"INISS Test\" /3 /1 /Neznamy \"/X Y\"");

        Assert.IsTrue(config.RunAsAdmin);
        Assert.IsTrue(config.Minimize);
        Assert.IsTrue(config.ExportHlas);
        Assert.IsFalse(config.Export, "/ExportHlas nie je /Export");
        Assert.AreEqual("INISS Test", config.Registry);
        CollectionAssert.AreEqual(new[] { 1, 3 }, config.ActivatorList.ToArray());
        Assert.AreEqual("/Neznamy \"/X Y\"", config.ExtraArguments);
        Assert.AreEqual("/Minimize /ExportHlas \"/Reg:INISS Test\" /1 /3 /Neznamy \"/X Y\"", RunConfigurations.Arguments(config));
    }

    [TestMethod]
    public void Predvolene_JednaNaProgramSPovodnymNastavenim()
    {
        var legacy = new StartupINISS { RunAsAdmin = true, CmdArgs = "/Minimize" };

        var configs = RunConfigurations.Defaults(["INISS - Bardejov.exe", "INISS.exe", "INISS-Bardejov.exe"], legacy);

        CollectionAssert.AreEqual(new[] { "Bardejov", "INISS", "Bardejov (2)" }, configs.Select(c => c.Name).ToArray());
        Assert.IsTrue(configs.TrueForAll(c => c.RunAsAdmin && c.Minimize));
        Assert.HasCount(3, configs.Select(c => c.Id).Distinct());
    }

    [TestMethod]
    [DataRow("INISS - Bardejov.exe", "Bardejov")]
    [DataRow("INISS_Test.exe", "Test")]
    [DataRow("INISS.exe", "INISS")]
    [DataRow("INISSView.exe", "INISSView")]
    [DataRow("Stanica.exe", "Stanica")]
    public void NazovPodlaProgramu(string program, string expected) => Assert.AreEqual(expected, RunConfigurations.DefaultName(program));

    [TestMethod]
    public void Aktivatory_GrafikonyPodlaCisliceVPriznakoch()
    {
        DirList Dir(string name, string? flags) => new() { DirName = name, FullPath = @"C:\INISS\DATA\" + name, Flags = flags };

        var targets = RunConfigurations.ActivatorTargets([Dir("A", "K2"), Dir("B", null), Dir("C", "z2"), Dir("D", "5")]);

        CollectionAssert.AreEqual(new[] { 2, 5 }, targets.Keys.ToArray());
        CollectionAssert.AreEqual(new[] { "A", "C" }, targets[2]);
    }

    [TestMethod]
    public void KonfiguraciaProgramu_XmlRoundTrip()
    {
        var config = new GVDEditorConfig();
        config.RunConfigurations.Add(new InstallationRunConfigurations
        {
            Dir = @"C:\INISS", Selected = "x",
            Items = [new RunConfiguration { Id = "x", Name = "A", Program = "INISS.exe", Remote = true, Activators = "4", SaveBefore = SaveBeforeRun.Never, WhenRunning = WhenAlreadyRunning.NewInstance }]
        });

        var serializer = new XmlSerializer(typeof(GVDEditorConfig));
        using var writer = new StringWriter();
        serializer.Serialize(writer, config);
        using var reader = System.Xml.XmlReader.Create(new StringReader(writer.ToString()));
        var back = (GVDEditorConfig)serializer.Deserialize(reader)!;

        var item = back.RunConfigurations.Single().Items.Single();
        Assert.AreEqual("x", back.RunConfigurations[0].Selected);
        Assert.AreEqual("A", item.Name);
        Assert.IsTrue(item.Remote);
        Assert.AreEqual("4", item.Activators);
        Assert.AreEqual(SaveBeforeRun.Never, item.SaveBefore);
        Assert.AreEqual(WhenAlreadyRunning.NewInstance, item.WhenRunning);
        StringAssert.Contains(writer.ToString(), "<WhenRunning>NewInstance</WhenRunning>");
    }
}
