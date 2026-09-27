using GVDEditor.XML;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor.Forms.Settings;

/// <summary>
///     Spolocne spravanie okien Lokalne a Globalne nastavenia: zapamatana velkost okna a farba chybovych textov.
/// </summary>
internal static class SettingsWindow
{
    /// <summary>
    ///     Nastavi oknu zapamatanu velkost; bez nej ostane velkost z navrhu.
    /// </summary>
    public static void ApplyPlacement(Form form, WindowPlacement? placement)
    {
        if (placement is null || placement.Width <= 0 || placement.Height <= 0)
            return;

        var scale = form.DeviceDpi / 96f;
        var area = Screen.FromControl(form).WorkingArea;
        var width = Math.Clamp((int)(placement.Width * scale), form.MinimumSize.Width, area.Width);
        var height = Math.Clamp((int)(placement.Height * scale), form.MinimumSize.Height, area.Height);
        form.Size = new Size(width, height);

        if (placement.Maximized)
            form.WindowState = FormWindowState.Maximized;
    }

    /// <summary>
    ///     Aktualna velkost okna v bodoch pri 96 DPI (pri maximalizovanom okne jeho normalna velkost).
    /// </summary>
    public static WindowPlacement CapturePlacement(Form form)
    {
        var bounds = form.WindowState == FormWindowState.Normal ? form.Bounds : form.RestoreBounds;
        var scale = form.DeviceDpi / 96f;
        return new WindowPlacement
        {
            Width = (int)Math.Round(bounds.Width / scale),
            Height = (int)Math.Round(bounds.Height / scale),
            Maximized = form.WindowState == FormWindowState.Maximized
        };
    }

    /// <summary>
    ///     Zapise konfiguraciu programu (napr. po zmene zapamatanej velkosti okna). Chyba zapisu sa len zaloguje -
    ///     velkost okna nestoji za prerusenie prace.
    /// </summary>
    public static void SaveConfig()
    {
        try
        {
            var configsDir = ToolsCore.AppPaths.ConfigDir;
            if (!Directory.Exists(configsDir))
                Directory.CreateDirectory(configsDir);

            XmlSerialization.WriteData(Utils.CombinePath(configsDir, ToolsCore.FileConsts.FILE_CONFIG)!, GlobData.Config);
        }
        catch (Exception e)
        {
            Log.Exception(e);
        }
    }

    /// <summary>
    ///     Farba textu chyby citatelna na pozadi prvku (svetla aj tmava tema).
    /// </summary>
    public static Color ProblemColor(Control control) =>
        control.BackColor.GetBrightness() < 0.5f ? Color.FromArgb(255, 128, 128) : Color.FromArgb(190, 30, 45);
}
