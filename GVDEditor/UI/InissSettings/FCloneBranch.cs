using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Klon konfiguracie INISSu - nazov novej vetvy registra a ci k nej zalozit konfiguraciu spustania s /Reg.
/// </summary>
internal partial class FCloneBranch : Form
{
    private readonly IReadOnlyCollection<string> _existing;

    /// <summary>
    /// Vytvori okno.
    /// </summary>
    /// <param name="appName">klonovana vetva</param>
    /// <param name="existing">existujuce vetvy (novy nazov sa s nimi nesmie zhodovat)</param>
    /// <param name="canCreateRunConfig">je z coho zalozit konfiguraciu spustania (program)</param>
    public FCloneBranch(string appName, IReadOnlyCollection<string> existing, bool canCreateRunConfig)
    {
        _existing = existing;
        InitializeComponent();
        this.ApplyThemeAndFonts();
        lIntro.MaximumSize = new Size(LogicalToDeviceUnits(520), 0);
        lIntro.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissClone_Intro, appName);
        cboxRunConfig.Visible = canCreateRunConfig;
        cboxRunConfig.Checked = canCreateRunConfig;
        tbName.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissClone_DefaultName, appName);
        tbName.TextChanged += (_, _) => Validate();
        bOK.Click += (_, _) =>
        {
            if (Validate()) DialogResult = DialogResult.OK;
        };
        Validate();
    }

    /// <summary>Nazov novej vetvy.</summary>
    public string NewName => tbName.Text.Trim();

    /// <summary>Zalozit konfiguraciu spustania s novou vetvou.</summary>
    public bool CreateRunConfig => cboxRunConfig.Visible && cboxRunConfig.Checked;

    private new bool Validate()
    {
        var name = NewName;
        var error = name.Length == 0 ? Resources.InissClone_NameEmpty
            : name.IndexOfAny(['\\', '/']) >= 0 ? Resources.InissClone_NameInvalid
            : _existing.Contains(name, StringComparer.OrdinalIgnoreCase) ? Resources.InissClone_NameExists
            : "";
        lError.Text = error;
        lError.ForeColor = GVDEditor.UI.Settings.SettingsWindow.ProblemColor(this);
        bOK.Enabled = error.Length == 0;
        return error.Length == 0;
    }
}
