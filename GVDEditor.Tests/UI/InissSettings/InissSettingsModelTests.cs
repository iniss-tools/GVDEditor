using System.Diagnostics.CodeAnalysis;
using GVDEditor.UI.InissSettings;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.Tests.UI.InissSettings;

/// <summary>
/// Model okna Nastavenia INISSu - neulozene zmeny, ciel zapisu a texty hodnot.
/// </summary>
[TestClass]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class InissSettingsModelTests
{
    private static InissSettingsModel Model(RegBranch? machine = null, string? ini = null) =>
        new(RegResolver.Resolve(new InissConfigSource
        {
            AppName = "INISS - Test",
            Version = new RegVersion(3, 39),
            Machine = machine ?? new RegBranch(),
            Ini = ini is null ? null : InissIniFile.Parse(ini)
        }), @"C:\INISS\INISS - Test.INI", false);

    private static SettingRow Row(InissSettingsModel m, string section, string name) =>
        m.AllRows().Single(r => r.Section == section && r.Name == name);

    private static RegSetting FirstColor(InissSettingsModel m) =>
        m.AllRows().First(r => r.Setting?.Setting.Type == RegValueType.Color).Setting!.Setting;

    [TestMethod]
    public void Zmena_RovnakaHodnotaAkoUcinnaNieJeZmenou()
    {
        var m = Model(new RegBranch().Set("Loging", "TableLogMode", RegRawValue.Dword(15)));
        var row = Row(m, "Loging", "TableLogMode");

        m.SetPending(row, 3, RegWriteTarget.Registry, false);
        Assert.HasCount(1, m.Pending);

        m.SetPending(row, 15, RegWriteTarget.Registry, false);
        Assert.IsEmpty(m.Pending);
    }

    [TestMethod]
    public void Zmena_RovnakaHodnotaInyCielJeZmenou()
    {
        var m = Model(new RegBranch().Set("Loging", "TableLogMode", RegRawValue.Dword(15)));

        m.SetPending(Row(m, "Loging", "TableLogMode"), 15, RegWriteTarget.Ini, false);

        Assert.AreEqual(RegWriteTarget.Ini, m.Pending.Single().Target);
    }

    [TestMethod]
    public void Obnovenie_ZmenaBezHodnoty()
    {
        var m = Model(new RegBranch().Set("Loging", "TableLogMode", RegRawValue.Dword(15)));

        m.SetPending(Row(m, "Loging", "TableLogMode"), 15, RegWriteTarget.Registry, true);

        var change = m.Pending.Single().ToChange();
        Assert.IsNull(change.Value);
        Assert.AreEqual(RegValueType.Dword, change.Type);
    }

    [TestMethod]
    public void Ciel_PodlaZdrojaHodnoty()
    {
        var m = Model(ini: "[Loging]\r\nTableLogMode=15\r\n");

        Assert.AreEqual(RegWriteTarget.Ini, InissSettingsModel.DefaultTarget(Row(m, "Loging", "TableLogMode")));
        Assert.AreEqual(RegWriteTarget.Registry, InissSettingsModel.DefaultTarget(Row(m, "Loging", "ReportLogOn")));
    }

    [TestMethod]
    public void Riadky_HodnotaMimoKataloguSaDaLenZmazat()
    {
        var m = Model(new RegBranch().Set("Environment", "Loggin", RegRawValue.Dword(1)));

        var row = Row(m, "Environment", "Loggin");

        Assert.IsNull(row.Setting);
        Assert.AreEqual(RegSeverity.Warning, row.Severity);
        Assert.AreEqual(RegLocation.Machine, row.Extra.Single().Location);
    }

    [TestMethod]
    public void Format_PodlaTypu()
    {
        var m = Model();
        var recvPort = Row(m, "Client", "RecvPort").Setting!.Setting;

        Assert.AreEqual("#FF0000", InissSettingsModel.ColorText(0x0000FF));
        StringAssert.StartsWith(InissSettingsModel.Format(0, recvPort), "0 – ");
        Assert.AreEqual("5000", InissSettingsModel.Format(5000, recvPort));
        Assert.AreEqual("—", InissSettingsModel.Format((object?)null, recvPort));
    }

    [TestMethod]
    [DataRow("Environment", "Logging", (int)CellKind.Check)]
    [DataRow("Client", "RecvPort", (int)CellKind.Choice)]
    [DataRow("Grafikon", "MinStay [m]", (int)CellKind.Text)]
    [DataRow("PathNames", "LogPath", (int)CellKind.Text)]
    [DataRow("Environment", "State", (int)CellKind.ReadOnly)]
    public void Bunka_PodlaTypuAHodnot(string section, string name, int expected)
    {
        Assert.AreEqual((CellKind)expected, InissSettingsModel.KindOf(Row(Model(), section, name)));
    }

    [TestMethod]
    [DataRow("15", 15)]
    [DataRow("-1", -1)]
    [DataRow("0x0F", 15)]
    [DataRow("0 – rozhranie sa neotvorí", 0)]
    public void Parsovanie_Cislo(string text, int expected)
    {
        var recvPort = Row(Model(), "Client", "RecvPort").Setting!.Setting;

        Assert.IsTrue(InissSettingsModel.TryParse(text, recvPort, out var value));
        Assert.AreEqual(expected, value);
        Assert.IsFalse(InissSettingsModel.TryParse("abc", recvPort, out _));
    }

    [TestMethod]
    public void Parsovanie_FarbaSpatAjSystemova()
    {
        var color = FirstColor(Model());

        Assert.IsTrue(InissSettingsModel.TryParse("#FF8000", color, out var value));
        Assert.AreEqual("#FF8000", InissSettingsModel.EditText(value, color));
        Assert.IsTrue(InissSettingsModel.TryParse("", color, out var system));
        Assert.AreEqual(RegValues.SystemColor, system);
        Assert.AreEqual("", InissSettingsModel.EditText(system, color));
        Assert.IsFalse(InissSettingsModel.TryParse("#12345", color, out _));
    }

    [TestMethod]
    public void Hladanie_PodlaNazvuAjPopisu()
    {
        var m = Model();

        Assert.IsTrue(InissSettingsModel.Matches(Row(m, "Loging", "TableLogMode"), "tablelog"));
        Assert.IsTrue(InissSettingsModel.Matches(Row(m, "Loging", "TableLogMode"), "Loging"));
        Assert.IsFalse(InissSettingsModel.Matches(Row(m, "Loging", "TableLogMode"), "xyz-nic"));
    }
}
