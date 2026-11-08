using ToolsCore.Iniss.Registry;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Neulozena zmena jednej hodnoty.
/// </summary>
/// <param name="Row">riadok</param>
/// <param name="Value">nova hodnota (int, string); null = zmazat zo vsetkych vrstiev (obnovit predvolenu)</param>
/// <param name="Target">kam zapisat</param>
internal sealed record PendingChange(SettingRow Row, object? Value, RegWriteTarget Target)
{
    /// <summary>Zmena do katalogu domeny.</summary>
    public RegChange ToChange() => new(Row.Section, Row.Name, Row.Setting?.Setting.Type ?? RegValueType.String, Value, Target);
}
