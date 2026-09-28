using GVDEditor.Config;
using GVDEditor.Integration;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// Proces INISS spusteny z GVDEditora - stavy bez skutocneho INISSu.
/// </summary>
[TestClass]
public class InissProcessServiceTests
{
    private static readonly StartupINISS Options = new() { CmdArgs = "", RunAsAdmin = false };

    [TestMethod]
    public void Start_NeexistujuciProgram_VynimkaAINISSNebezi()
    {
        using var service = new InissProcessService();
        var changed = 0;
        service.StateChanged += (_, _) => changed++;

        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "INISS.exe");
        Assert.ThrowsExactly<InvalidOperationException>(() => service.Start(path, Options));

        Assert.IsFalse(service.IsRunning);
        Assert.IsNull(service.LastStartPath);
        Assert.AreEqual(0, changed);
    }

    [TestMethod]
    public async Task Restart_BezSpustenehoINISSu_NicNespravi()
    {
        using var service = new InissProcessService();
        var changed = 0;
        service.StateChanged += (_, _) => changed++;

        await service.RestartAsync(Options, () =>
        {
            Assert.Fail("bez procesu sa nema na co pytat");
            return false;
        });

        Assert.IsFalse(service.IsRestarting);
        Assert.AreEqual(0, changed);
    }

    [TestMethod]
    public void KillAShutDown_BezSpustenehoINISSu_BezChyby()
    {
        using var service = new InissProcessService();

        service.Kill();
        service.ShutDown();

        Assert.IsFalse(service.IsRunning);
    }
}
