using ToolsCore.Iniss.Registry;

namespace GVDEditor.UI.InissSettings;

/// <summary>Zmena v detaile - ciel zapisu alebo obnovenie predvolenej.</summary>
/// <param name="Target">kam zapisat</param>
/// <param name="Reset">obnovit predvolenu (zmazat zo vsetkych vrstiev)</param>
internal sealed record SettingEdit(RegWriteTarget Target, bool Reset);
