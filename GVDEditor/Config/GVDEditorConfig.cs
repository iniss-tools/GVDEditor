using System.Xml.Serialization;
using ToolsCore.XML;
using GVDEditor.UI;

namespace GVDEditor.Config;

/// <summary>
/// Konfiguracny subor pre program GVDEditor.
/// </summary>
[XmlRoot("CONFIG")]
public record GVDEditorConfig() : ConfigBase
{
    /// <summary>
    /// Nastavi jazyk generovania datumovych obmedzeni.
    /// </summary>
    [XmlElement("DateRemLocate"), DefaultValue(0)] 
    [Description("Nastavi jazyk generovania datumovych obmedzeni.")]
    public AppLanguage DateLimitLocate { get; set; } = AppLanguage.Slovak;

    /// <summary>
    /// Automaticky generovat texty do tabul pri ukladani do suborov.
    /// </summary>
    [XmlElement("AutoTableText"), DefaultValue(false)]
    public bool AutoTableText { get; set; }

    /// <summary>
    /// Nastavi cas v milisekundach medzi zvukmi pri prehravani.
    /// </summary>
    [XmlElement("PlayerSoundsOffset"), DefaultValue(0)]
    public int PlayerSoundsOffset { get; set; }

    /// <summary>
    /// Stlpce zobrazujuce sa v tabulke na pracovnej ploche programu.
    /// </summary>
    [XmlElement("DesktopCols")] 
    public DesktopColumns DesktopCols { get; set; } = new();

    /// <summary>
    /// Klávesové skratky pre akcie na pracovnej ploche programu.
    /// </summary>
    [XmlElement("Shortcuts")] 
    public ShortcutMap Shortcuts { get; set; } = new();

    /// <summary>
    /// konfiguracia spustania INISSu z tohto programu.
    /// </summary>
    [XmlElement("StartupINISSConfig")] 
    public StartupINISS StartupINISSConfig { get; set; } = new() { CmdArgs = "", RunAsAdmin = false };

    /// <summary>
    /// Velkost okna Lokalne nastavenia; <see langword="null" /> = predvolena z navrhu.
    /// </summary>
    [XmlElement("LocalSettingsWindow")]
    public WindowPlacement? LocalSettingsWindow { get; set; }

    /// <summary>
    /// Velkost okna Globalne nastavenia; <see langword="null" /> = predvolena z navrhu.
    /// </summary>
    [XmlElement("GlobalSettingsWindow")]
    public WindowPlacement? GlobalSettingsWindow { get; set; }

    /// <summary>
    /// Velkost okna uprava vlaku; <see langword="null" /> = predvolena z navrhu.
    /// </summary>
    [XmlElement("EditTrainWindow")]
    public WindowPlacement? EditTrainWindow { get; set; }

    /// <inheritdoc />
    public override string LinkAppSettingsGuide => GvdLinkConsts.LinkAppSettings;

    protected GVDEditorConfig(GVDEditorConfig original) : base(original)
    {
        DateLimitLocate = original.DateLimitLocate;
        AutoTableText = original.AutoTableText;
        PlayerSoundsOffset = original.PlayerSoundsOffset;
        DesktopCols = original.DesktopCols with { };
        Shortcuts = original.Shortcuts.Clone();
        StartupINISSConfig = original.StartupINISSConfig with { };
        LocalSettingsWindow = original.LocalSettingsWindow?.Clone();
        GlobalSettingsWindow = original.GlobalSettingsWindow?.Clone();
        EditTrainWindow = original.EditTrainWindow?.Clone();
    }
}