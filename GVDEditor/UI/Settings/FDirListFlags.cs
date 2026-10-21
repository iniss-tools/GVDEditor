using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Dialog - priznaky grafikonu v zozname grafikonov (4. stlpec DirList.TXT): spracovanie sprav o vlakoch z ILTIS
/// a prepinac, s ktorym je grafikon aktivny.
/// </summary>
internal partial class FDirListFlags : Form
{
    /// <summary>
    /// Vytvori okno s priznakmi grafikonu.
    /// </summary>
    /// <param name="grafikon">grafikon - do titulku okna</param>
    /// <param name="flags">povodne priznaky</param>
    public FDirListFlags(GVDDirectory grafikon, DirListFlags flags)
    {
        ArgumentNullException.ThrowIfNull(grafikon);

        InitializeComponent();
        this.ApplyThemeAndFonts();

        Text = string.Format(CultureInfo.CurrentCulture, Text, grafikon.PeriodFormatted);

        cbSwitch.Items.Add(Resources.DirListFlags_Always);
        for (var i = 1; i <= 9; i++)
            cbSwitch.Items.Add(string.Format(CultureInfo.CurrentCulture, Resources.DirListFlags_OnlyWithSwitch, i));

        cboxSpread.Checked = flags.Spread;
        cboxBothTracks.Checked = flags.DepartureSetsBothTracks;
        rbMissingIgnore.Checked = flags.MissingTrain == DirListMissingTrain.Ignore;
        rbMissingApplyToRoute.Checked = flags.MissingTrain == DirListMissingTrain.ApplyToRoute;
        rbMissingApplyToRouteWithoutCategori.Checked = flags.MissingTrain == DirListMissingTrain.ApplyToRouteWithoutCategori;
        cbSwitch.SelectedIndex = flags.Switch ?? 0;

        foreach (var box in new[] { cboxSpread, cboxBothTracks })
            box.CheckedChanged += (_, _) => UpdateCode();
        foreach (var radio in new[] { rbMissingIgnore, rbMissingApplyToRoute, rbMissingApplyToRouteWithoutCategori })
            radio.CheckedChanged += (_, _) => UpdateCode();
        cbSwitch.SelectedIndexChanged += (_, _) => UpdateCode();
        UpdateCode();
    }

    [AllowNull]
    public sealed override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>
    /// Priznaky nastavene v okne.
    /// </summary>
    public DirListFlags Flags => new(cboxSpread.Checked, cboxBothTracks.Checked,
        rbMissingApplyToRoute.Checked ? DirListMissingTrain.ApplyToRoute
        : rbMissingApplyToRouteWithoutCategori.Checked ? DirListMissingTrain.ApplyToRouteWithoutCategori
        : DirListMissingTrain.Ignore,
        cbSwitch.SelectedIndex > 0 ? cbSwitch.SelectedIndex : null);

    // zapis v subore, ako ho pozna specifikacia - pre toho, kto subor cita alebo upravuje rucne
    private void UpdateCode()
    {
        var code = Flags.ToString();
        lCode.Text = string.Format(CultureInfo.CurrentCulture, Resources.DirListFlags_Code, code.Length == 0 ? Resources.DirListFlags_CodeEmpty : code);
    }
}
