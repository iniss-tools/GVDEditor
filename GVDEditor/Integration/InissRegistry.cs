using Microsoft.Win32;

namespace GVDEditor.Integration;

/// <summary>
///     Zapisy INISSu v registroch Windows.
/// </summary>
internal static class InissRegistry
{
    public static string[] GetINISSRegisters()
    {
        var bit64 = Environment.Is64BitOperatingSystem;
        using var key = Registry.LocalMachine.OpenSubKey(bit64 ? @"SOFTWARE\WOW6432Node\CHAPS" : @"SOFTWARE\CHAPS");
        var res = new List<string> { "" };
        if (key is not null) 
            res.AddRange(key.GetSubKeyNames());

        return res.ToArray();
    }
}