using System.Globalization;
using ExControls;
using GVDEditor.Domain.Entities;
using GVDEditor.Properties;
using GVDEditor.UI.Settings;
using ToolsCore.Iniss.Elen;
using ToolsCore.Tools;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Vyber pisma tabule podla vzhladu: rez, farba a efekty. Cislo pisma pre tabule ELEN z nich zlozi
/// <see cref="ElenFontCode.Compose" />; bity bez ovladaca (napr. 0x40) ponecha. Pri vyrobcovi, ktory cislo
/// nekoduje ako ELEN, sa cislo zadava rucne.
/// </summary>
public partial class TableFontPicker : UserControl
{
    private readonly ExRadioButton[] _faces;
    private readonly ExRadioButton[] _colors;
    private TableManufacturer? _manufacturer;
    private int _value = ElenFontCode.DefaultKeptBits | 0x10;
    private int _keptBits = ElenFontCode.DefaultKeptBits;
    private bool _manual;

    // nastavovanie ovladacov kodom nema spatne menit cislo
    private bool _updating;

    /// <summary>
    /// Vytvori vyber pisma (predvolene tenke pismo bez farby).
    /// </summary>
    public TableFontPicker()
    {
        InitializeComponent();
        _faces = [rbFace0, rbFace1, rbFace2, rbFace3];
        _colors = [rbColor0, rbColor1, rbColor2, rbColor3];
        Sync(true);
    }

    /// <summary>
    /// Pouzivatel zmenil cislo pisma.
    /// </summary>
    public event EventHandler? ValueChanged;

    /// <summary>
    /// Cislo pisma.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _keptBits = new ElenFontCode(value).KeptBits;
            Sync(true);
        }
    }

    /// <summary>
    /// Vyrobca tabule - urcuje, ci sa cislo sklada ako ELEN a kolko rozsirenych pisiem je k dispozicii.
    /// <see langword="null" /> = zoznam pisiem bez tabule (predpoklada sa ELEN).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TableManufacturer? Manufacturer
    {
        get => _manufacturer;
        set
        {
            _manufacturer = value;
            Sync(true);
        }
    }

    /// <summary>
    /// Subor pisiem ELEN pre nahlad; null = nahlad len pismom Windows bez moznosti vyberu suboru.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal ElenPreviewFonts? PreviewFonts
    {
        get => _previewFonts;
        set
        {
            _previewFonts = value;
            Sync(false);
        }
    }

    private ElenPreviewFonts? _previewFonts;

    // dialog vyberu suboru pisiem este nedobehol
    private bool _choosingFile;

    private bool IsElen => ElenFontCode.AppliesTo(_manufacturer);

    /// <summary>
    /// Sirka podla rodica (riadky volieb sa zalomia, nahlad sa roztiahne), vyska podla obsahu. Bez obmedzenia
    /// sirky (napr. dialog s automatickou velkostou) sa volby rozlozia do jedneho riadka.
    /// </summary>
    public override Size GetPreferredSize(Size proposedSize)
    {
        var width = proposedSize.Width is > 1 and < int.MaxValue / 2 ? proposedSize.Width : UnwrappedWidth();
        var height = tlp.GetPreferredSize(new Size(width, 0)).Height;
        return new Size(width, height + Padding.Vertical);
    }

    /// <summary>
    /// Sirka, pri ktorej sa ziadny riadok volieb nezalomi.
    /// </summary>
    private int UnwrappedWidth()
    {
        static int Row(Control label, Control row) =>
            label.PreferredSize.Width + label.Margin.Horizontal +
            // Visible je pred zobrazenim okna vzdy false - rata sa so vsetkymi volbami
            row.Controls.Cast<Control>().Sum(c => Math.Max(c.PreferredSize.Width, c.Width) + c.Margin.Horizontal);

        return new[] { Row(lFace, flpFace), Row(lColor, flpColor), Row(lEffects, flpEffects), tlp.GetPreferredSize(Size.Empty).Width }
            .Max() + tlp.Padding.Horizontal + 24;
    }

    private void Part_Changed(object? sender, EventArgs e)
    {
        if (_updating)
            return;

        // radio button hlasi aj odznacenie - zlozit az raz, podla aktualneho stavu
        if (sender is RadioButton { Checked: false })
            return;

        var face = Array.FindIndex(_faces, rb => rb.Checked);
        var ext = rbExt.Checked ? decimal.ToInt32(nudExt.Value) : 0;
        var code = ElenFontCode.Compose(_keptBits, Math.Max(0, face), Math.Max(0, Array.FindIndex(_colors, rb => rb.Checked)),
            cbBlink.Checked, cbTall.Checked, ext);
        // pri rozsirenom pisme ostava rez z povodneho cisla (tabula ho ignoruje)
        if (ext > 0 && face < 0)
            code = ElenFontCode.Compose(_keptBits, new ElenFontCode(_value).Face, code.Color, code.Blinks, code.TallDigits, ext);

        SetValue(code.Id, true);
    }

    private void nudId_ValueChanged(object? sender, EventArgs e)
    {
        if (_updating)
            return;

        var id = decimal.ToInt32(nudId.Value);
        _keptBits = new ElenFontCode(id).KeptBits;
        SetValue(id, false);
    }

    private void SetValue(int id, bool updateNumber)
    {
        if (id == _value)
        {
            Sync(updateNumber);
            return;
        }

        _value = id;
        Sync(updateNumber);
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    private void llFixBit_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        // rovnake cislo s bitom 0x40 - vzhlad ostane, tabula dostane platny bajt
        var fixedCode = new ElenFontCode(_value).WithKeptBit;
        _keptBits = fixedCode.KeptBits;
        SetValue(fixedCode.Id, true);
    }

    /// <summary>
    /// Nahlad pismom zo suboru pisiem ELEN, ak ho obsluha vybrala a pismo v nom je; popis s odkazmi na zmenu suboru.
    /// </summary>
    private void UpdatePreview(ElenFontCode code)
    {
        var file = _previewFonts?.File;
        led.TextEncoding = ElenText.EncodingFor(_manufacturer);
        led.Fonts = file;
        if (_previewFonts is null)
            return;

        var name = Path.GetFileName(_previewFonts.Path);
        var state = _previewFonts.Path.Length == 0 ? Resources.FontPicker_PreviewWindows
            : file is null ? string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_PreviewError, name)
            : !led.UsesFontFile ? string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_PreviewNoFont, name, code.FontIndex)
            : string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_PreviewFile, name);

        // odkazy za popisom: vyber (zmena) suboru a navrat k pismu Windows
        var choose = _previewFonts.Path.Length == 0 ? Resources.FontPicker_PreviewChoose : Resources.FontPicker_PreviewChange;
        var text = state + " " + choose;
        llPreview.Links.Clear();
        llPreview.Text = _previewFonts.Path.Length == 0 ? text : text + "  " + Resources.FontPicker_PreviewUseWindows;
        llPreview.Links.Add(state.Length + 1, choose.Length, PreviewLink.Choose);
        if (_previewFonts.Path.Length > 0)
            llPreview.Links.Add(text.Length + 2, Resources.FontPicker_PreviewUseWindows.Length, PreviewLink.Windows);
    }

    private enum PreviewLink
    {
        Choose,
        Windows
    }

    private async void llPreview_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        // dialog este konci (Windows po zruseni chvilu doupratuje) - druhy dialog by sa otvoril cez neho
        if (_previewFonts is null || _choosingFile)
            return;

        if (e.Link?.LinkData is PreviewLink.Windows)
        {
            _previewFonts.Set("");
            Sync(false);
            return;
        }

        using var dialog = new OpenFileDialog();
        dialog.Filter = Resources.FontPicker_PreviewFilter;
        var current = _previewFonts.Path;
        dialog.InitialDirectory = current.Length > 0 ? Path.GetDirectoryName(current) : _previewFonts.FontsDirectory;
        // dialog na vlastnom vlakne - po zruseni okno hned reaguje, hoci Windows dialog este doupratuje
        _choosingFile = true;
        DialogResult result;
        try
        {
            result = await dialog.ShowDialogAsync(FindForm() ?? (IWin32Window)this);
        }
        finally
        {
            _choosingFile = false;
        }

        if (result != DialogResult.OK || IsDisposed)
            return;

        try
        {
            ElenFontFile.Load(dialog.FileName);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or FormatException)
        {
            Serilog.Log.Warning(ex, "Subor pisiem ELEN {Path} pre nahlad sa nepodarilo nacitat", dialog.FileName);
            Utils.ShowError(string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_PreviewOpenFailed, dialog.FileName, ex.Message));
            return;
        }

        _previewFonts.Set(dialog.FileName);
        Sync(false);
    }

    private void llManual_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        _manual = !_manual;
        Sync(true);
        if (_manual)
            nudId.Focus();
    }

    /// <summary>
    /// Nastavi ovladace podla cisla pisma.
    /// </summary>
    private void Sync(bool updateNumber)
    {
        _updating = true;
        SuspendLayout();
        try
        {
            var code = new ElenFontCode(_value);
            var elen = IsElen;

            foreach (var control in new Control[] { lFace, flpFace, lColor, flpColor, lEffects, flpEffects, led, lResult })
                control.Visible = elen;
            llPreview.Visible = elen && _previewFonts is not null;

            var maxExt = ElenFontCode.MaxExtendedFont(_manufacturer);
            rbExt.Visible = nudExt.Visible = maxExt > 0 || code.ExtendedFont > 0;
            nudExt.Maximum = Math.Max(Math.Max(1, maxExt), code.ExtendedFont);
            nudExt.Enabled = code.ExtendedFont > 0;
            if (code.ExtendedFont > 0)
                nudExt.Value = code.ExtendedFont;

            rbExt.Checked = code.ExtendedFont > 0;
            for (var i = 0; i < _faces.Length; i++)
                _faces[i].Checked = code.ExtendedFont == 0 && code.Face == i;
            for (var i = 0; i < _colors.Length; i++)
                _colors[i].Checked = code.Color == i;
            cbBlink.Checked = code.Blinks;
            cbTall.Checked = code.TallDigits;

            led.FontId = _value;
            UpdatePreview(code);
            lResult.Text = string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Vysledok, _value,
                _value < 0 ? "" : $" (0x{_value:X})", code.Describe());

            llManual.Visible = elen;
            llManual.Text = _manual ? Resources.FontPicker_Skryt_cislo : Resources.FontPicker_Zadat_cislo;
            nudId.Visible = _manual || !elen;
            if (updateNumber)
                nudId.Value = Math.Clamp(_value, nudId.Minimum, nudId.Maximum);

            // cislo bez bitu 0x40 posle tabuli riadiaci znak (napr. 04 = koniec spravy ELEN) - tabula riadok odmietne
            var controlChar = elen && code.SendsControlChar;
            llFixBit.Visible = controlChar;
            if (controlChar)
                llFixBit.Text = string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Pridat_bit, code.WithKeptBit.Id);

            lNote.Text = NoteText(code, elen);
            lNote.Visible = lNote.Text.Length > 0;
            lNote.ForeColor = controlChar ? SettingsWindow.ProblemColor(lNote) : lResult.ForeColor;
        }
        finally
        {
            ResumeLayout(true);
            _updating = false;
        }
    }

    private string NoteText(ElenFontCode code, bool elen)
    {
        if (!elen)
            return string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_InyVyrobca, _manufacturer?.Name);

        var notes = new List<string>();
        if (code.SendsControlChar)
            notes.Add(string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Riadiaci_znak, _value & 0xFF));
        var extra = code.KeptBits & ~ElenFontCode.DefaultKeptBits;
        if (_value >= 0 && extra != 0)
            notes.Add(string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Ponechane_bity, extra));
        if (code.ExtendedFont > ElenFontCode.MaxExtendedFont(_manufacturer))
            notes.Add(string.Format(CultureInfo.CurrentCulture, Resources.FontPicker_Rozsirene_nepozna,
                _manufacturer?.Name, code.ExtendedFont));
        return string.Join(" ", notes);
    }
}
