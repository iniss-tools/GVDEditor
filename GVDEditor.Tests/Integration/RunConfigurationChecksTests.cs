using System.Diagnostics.CodeAnalysis;
using GVDEditor.Config;
using GVDEditor.Integration;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Kontrola konfiguracie spustania podla spravania INISSu (mutex, davkovy import, vetva registra, aktivatory).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class RunConfigurationChecksTests
{
    private static readonly RunEnvironment Env = new()
    {
        Programs = ["INISS.exe"],
        ProgramExists = p => p is "INISS.exe" or "Ine.exe",
        RegistryBranches = ["INISS"],
        ActivatorTargets = new Dictionary<int, List<string>> { [2] = ["Vedlajsi"] }
    };

    private static RunConfiguration Config() => new() { Name = "A", Program = "INISS.exe", Activators = "2" };

    private static RunCheckSeverity[] Severities(List<RunCheck> checks) => checks.Select(c => c.Severity).ToArray();

    [TestMethod]
    public void BeznaKonfiguracia_BezZisteni() => Assert.IsEmpty(RunConfigurationChecks.Check(Config(), Env));

    [TestMethod]
    public void ChybajuciProgram_Chyba()
    {
        var checks = RunConfigurationChecks.Check(Config() with { Program = "Nie.exe" }, Env);

        Assert.AreEqual(RunCheckSeverity.Error, checks[0].Severity);
    }

    [TestMethod]
    public void ProgramMimoINISS_Varovanie()
    {
        var checks = RunConfigurationChecks.Check(Config() with { Program = "Ine.exe" }, Env with { RegistryBranches = null });

        CollectionAssert.AreEqual(new[] { RunCheckSeverity.Warning }, Severities(checks));
    }

    [TestMethod]
    public void BeziaciBezMultiuse_VarovanieLenBezMultiuse()
    {
        var env = Env with { SingleInstanceHeld = true };

        Assert.HasCount(1, RunConfigurationChecks.Check(Config(), env));
        Assert.IsEmpty(RunConfigurationChecks.Check(Config() with { Multiuse = true }, env));
    }

    [TestMethod]
    public void DalsiaInstanciaBezMultiuse_Varovanie() =>
        CollectionAssert.AreEqual(new[] { RunCheckSeverity.Warning },
            Severities(RunConfigurationChecks.Check(Config() with { WhenRunning = WhenAlreadyRunning.NewInstance }, Env)));

    [TestMethod]
    public void ZnackaImportu_Varovanie() =>
        CollectionAssert.AreEqual(new[] { RunCheckSeverity.Warning }, Severities(RunConfigurationChecks.Check(Config(), Env with { ImportMarker = true })));

    [TestMethod]
    public void NeexistujucaVetva_Varovanie()
    {
        var checks = RunConfigurationChecks.Check(Config() with { Registry = "Nova" }, Env);

        Assert.AreEqual(RunCheckSeverity.Warning, checks.Single().Severity);
        StringAssert.Contains(checks[0].Text, "Nova");
    }

    [TestMethod]
    public void Aktivatory_NepouzityVarujeChybajuciInformuje()
    {
        var checks = RunConfigurationChecks.Check(Config() with { Activators = "7" }, Env);

        // /7 nezapne nic (varovanie), bez /2 ostane grafikon Vedlajsi neaktivny (informacia)
        CollectionAssert.AreEqual(new[] { RunCheckSeverity.Warning, RunCheckSeverity.Info }, Severities(checks));
        StringAssert.Contains(checks[1].Text, "Vedlajsi");
    }

    [TestMethod]
    public void KopiaVoVirtualStore_InformaciaLenBezSpravcu()
    {
        var env = Env with { VirtualStoreShadows = _ => [@"PathNames\LogPath", @"Grafikon\MinStay [m]"] };

        var checks = RunConfigurationChecks.Check(Config(), env);

        Assert.AreEqual(RunCheckSeverity.Info, checks.Single().Severity);
        StringAssert.Contains(checks[0].Text, @"PathNames\LogPath");
        Assert.IsEmpty(RunConfigurationChecks.Check(Config() with { RunAsAdmin = true }, env));
    }

    [TestMethod]
    public void DavkoveRezimyARovnakaVetva_Informacie()
    {
        var other = new RunConfiguration { Name = "B", Program = "INISS.exe" };
        var config = Config() with { ImportDat = true, ExportHlas = true };

        var checks = RunConfigurationChecks.Check(config, Env with { Others = [config, other] });

        CollectionAssert.AreEqual(new[] { RunCheckSeverity.Info, RunCheckSeverity.Info, RunCheckSeverity.Info }, Severities(checks));
        StringAssert.Contains(checks[2].Text, "B");
    }
}
