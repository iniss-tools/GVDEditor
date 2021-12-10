using Microsoft.Win32;

namespace GVDEditor.Tools;

internal static class AppRegistry
{
    public static string[] GetINISSRegisters()
    {
        var bit64 = Environment.Is64BitOperatingSystem;
        var key = Registry.LocalMachine.OpenSubKey(bit64 ? @"SOFTWARE\WOW6432Node\CHAPS" : @"SOFTWARE\CHAPS");
        var res = new List<string> { "" };
        if (key is not null) 
            res.AddRange(key.GetSubKeyNames());

        return res.ToArray();
    }
}