using ToolsCore.Iniss.Registry;

namespace GVDEditor.DocScreenshots;

/// <summary>
/// Ukážková konfigurácia INISSu pre snímky okna Nastavenia INISSu - len v pamäti, register počítača sa nečíta
/// ani nemení. Dve linky (Elektročas na COM2, ELEN cez sieť), kópia vo VirtualStore a niekoľko pozostatkov na
/// ukážku čistenia.
/// </summary>
internal static class DemoIniss
{
    /// <summary>Vrstvy konfigurácie - podpis zhodný s <c>InissRegistry.LoadSource</c>.</summary>
    public static InissConfigSource Load(string appName, string? exePath, InissRunMode mode, IReadOnlyDictionary<int, RegTableInfo>? tables) => new()
    {
        AppName = appName,
        Version = new RegVersion(3, 39),
        RunMode = mode,
        Tables = tables,
        Machine = Machine(),
        VirtualStore = new RegBranch().Set("PathNames", "LogPath", RegRawValue.String(@"DATA\"))
    };

    /// <summary>Vetva HKLM.</summary>
    public static RegBranch Machine() => new RegBranch()
        .Set("Environment", "OutToTableDriver", RegRawValue.Dword(1))
        .Set("Environment", "Logging", RegRawValue.Dword(1))
        .Set("Environment", "BusTxt", RegRawValue.String(""))
        .Set("PathNames", "LogPath", RegRawValue.String(@"Logy\"))
        .Set("Loging", "TableLogMode", RegRawValue.Dword(15))
        .Set("Loging", "TableLogMaxSize", RegRawValue.String("5000000"))
        .Set("Driver", "TableClass", RegRawValue.Dword(5))
        .Set("Driver", "TablePort", RegRawValue.String("COM2"))
        .Set("Driver", "PollingInterval", RegRawValue.Dword(400))
        .Set("Driver0", "TableClass", RegRawValue.Dword(4))
        .Set("Driver0", "TablePort", RegRawValue.String("3=TCP://10.0.0.21:4001"))
        .Set("Tables", "BlackOut", RegRawValue.Dword(0))
        .Set("Tables", "DayLight0", RegRawValue.Dword(12));

    /// <summary>Súbor .reg inej stanice na ukážku porovnania - iné logovanie a jas.</summary>
    public static string WriteOtherRegFile(string dir, string appName)
    {
        var other = Machine()
            .Set("Loging", "TableLogMode", RegRawValue.Dword(7))
            .Set("Environment", "Logging", RegRawValue.Dword(0))
            .Set("Tables", "DayLight0", RegRawValue.Dword(8))
            .Set("Grafikon", "MinStay [m]", RegRawValue.Dword(2));
        var file = new RegFile();
        file.AddBranch(other, RegFile.MachineRoot, appName);
        var path = Path.Combine(dir, "Horne Mesto.reg");
        file.Save(path);
        return path;
    }
}
