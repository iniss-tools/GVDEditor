using ToolsCore.Iniss.Registry;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Riadok tabulky hodnot: nastavenie z katalogu, alebo hodnota mimo katalogu (pozostatok, preklep, farba inej
/// jazykovej verzie), ktora sa da len zmazat.
/// </summary>
internal sealed class SettingRow
{
    private SettingRow(string section, string name, ResolvedSetting? setting, IReadOnlyList<RegLayerValue> extra, IReadOnlyList<RegDiagnostic> diagnostics)
    {
        Section = section;
        Name = name;
        Setting = setting;
        Extra = extra;
        Diagnostics = diagnostics;
    }

    /// <summary>Sekcia (konkretna, napr. Driver3).</summary>
    public string Section { get; }

    /// <summary>Nazov hodnoty.</summary>
    public string Name { get; }

    /// <summary>Vyhodnotene nastavenie; null pri hodnote mimo katalogu.</summary>
    public ResolvedSetting? Setting { get; }

    /// <summary>Vrstvy hodnoty mimo katalogu.</summary>
    public IReadOnlyList<RegLayerValue> Extra { get; }

    /// <summary>Zistenia k riadku.</summary>
    public IReadOnlyList<RegDiagnostic> Diagnostics { get; }

    /// <summary>Kluc zmeny.</summary>
    public string Key => InissSettingsModel.KeyOf(Section, Name);

    /// <summary>Najvyssia zavaznost zisteni alebo null.</summary>
    public RegSeverity? Severity => Diagnostics.Count == 0 ? null : Diagnostics.Max(d => d.Severity);

    /// <summary>Riadok nastavenia.</summary>
    public static SettingRow Of(ResolvedSetting setting) => new(setting.Section, setting.Name, setting, setting.Layers, setting.Diagnostics);

    /// <summary>Riadok hodnoty mimo katalogu.</summary>
    public static SettingRow OfExtra(ResolvedSection section, string name) =>
        new(section.Name, name, null, section.Extra.Where(l => string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase)).ToList(),
            section.Diagnostics.Where(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase)).ToList());
}
