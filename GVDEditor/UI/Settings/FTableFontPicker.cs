using System.Globalization;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using ToolsCore.Tools;

namespace GVDEditor.UI.Settings;

/// <summary>
/// Dialog - vzhlad pisma, ktore nie je v zozname pisiem (stlpec katalogovej tabule, text vlaku na tabuli).
/// Pismo sa da rovno pridat do zoznamu pisiem.
/// </summary>
public partial class FTableFontPicker : Form
{
    /// <summary>
    /// Vytvori dialog s pismom <paramref name="value" /> pre tabule vyrobcu <paramref name="manufacturer" />.
    /// </summary>
    public FTableFontPicker(int value, TableManufacturer? manufacturer)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();

        picker.Manufacturer = manufacturer;
        picker.Value = value;
        // okno s automatickou velkostou by volby zalomilo - najmensia sirka je ta, pri ktorej sa nezalomia
        picker.MinimumSize = new Size(picker.GetPreferredSize(Size.Empty).Width, 0);
        UpdateExisting();
    }

    /// <summary>
    /// Zvolene cislo pisma.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Value => picker.Value;

    /// <summary>
    /// Pridat pismo do zoznamu pisiem (len ak toto cislo v zozname este nie je).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AddToList => cbAddToList.Enabled && cbAddToList.Checked;

    private void picker_ValueChanged(object? sender, EventArgs e) => UpdateExisting();

    private void UpdateExisting()
    {
        var existing = GlobData.TableFonts.FirstOrDefault(font => font.FontID == picker.Value);
        lExisting.Text = existing is null
            ? ""
            : string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Uz_v_zozname, existing.Name);
        cbAddToList.Enabled = existing is null && picker.Value >= 0;
    }
}
