using System.Diagnostics.CodeAnalysis;
using GVDEditor.Tools;

namespace GVDEditor.Tests;

/// <summary>
///     Argumenty spustania INISSu (Nastavenia programu → Spustanie INISS).
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class INISSArgsTests
{
    [TestMethod]
    [DataRow("/multiuse /Minimize", "Multiuse", true)]
    [DataRow("-MINIMIZE", "Minimize", true)]
    [DataRow("/ExportHlas", "Export", false)]
    [DataRow("/ExportHlas", "ExportHlas", true)]
    [DataRow("/Export /ExportHlas", "Export", true)]
    [DataRow("Minimize", "Minimize", false)]
    [DataRow("", "Minimize", false)]
    public void Argumenty_ParameterPodlaMena(string args, string name, bool expected)
    {
        Assert.AreEqual(expected, INISSArgs.Has(args, name));
    }

    [TestMethod]
    [DataRow("/reg:\"INISS Bardejov\" /Minimize", "INISS Bardejov")]
    [DataRow("/Minimize /Reg:CHAPS", "CHAPS")]
    [DataRow("/Minimize", null)]
    public void Argumenty_Register(string args, string? expected)
    {
        Assert.AreEqual(expected, INISSArgs.Registry(args));
    }
}
