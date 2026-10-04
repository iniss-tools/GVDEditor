using GVDEditor.Config;
using GVDEditor.Integration;

namespace GVDEditor.Tests.Integration;

/// <summary>
/// INISSy spustene z GVDEditora - stavy bez skutocneho INISSu.
/// </summary>
[TestClass]
public class InissProcessServiceTests
{
    private static InissLaunch Launch(string path) => new(new RunConfiguration { Name = "Test", Program = Path.GetFileName(path) }, path, "");

    [TestMethod]
    public void Start_NeexistujuciProgram_VynimkaAINISSNebezi()
    {
        using var service = new InissProcessService();
        var changed = 0;
        service.StateChanged += (_, _) => changed++;

        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "INISS.exe");
        Assert.ThrowsExactly<InvalidOperationException>(() => service.Start(Launch(path)));

        Assert.IsFalse(((IInissProcess)service).IsRunning);
        Assert.IsEmpty(service.Instances);
        Assert.AreEqual(0, changed);
    }

    [TestMethod]
    public void Spustenie_VytvoriPrikazovyRiadokZKonfiguracie()
    {
        var config = new RunConfiguration { Name = "A", Program = "INISS - A.exe", Minimize = true, Registry = "INISS Test", Activators = "31" };

        var launch = InissLaunch.Create(config, @"C:\INISS");

        Assert.AreEqual(@"C:\INISS\INISS - A.exe", launch.Path);
        Assert.AreEqual("/Minimize \"/Reg:INISS Test\" /1 /3", launch.Arguments);
        Assert.AreNotSame(config, launch.Configuration);
    }
}
