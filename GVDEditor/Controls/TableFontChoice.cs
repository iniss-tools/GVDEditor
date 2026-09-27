using System.Globalization;
using ExControls;
using GVDEditor.Entities;
using GVDEditor.Forms;
using GVDEditor.Properties;
using GVDEditor.Tools;

namespace GVDEditor.Controls;

/// <summary>
///     Vyber pisma tabule v rozbalovacom zozname: pomenovane pisma zo zoznamu Pisma, pripadne predvolene pismo
///     stlpca a polozka „Ine pismo…“, ktora otvori vyber podla vzhladu. Nahradza zadavanie cisla pisma.
/// </summary>
internal sealed class TableFontChoice
{
    private const int OtherId = int.MinValue;

    private readonly ExComboBox _combo;
    private readonly ToolTip _tip;
    private readonly bool _allowColumnDefault;
    private TableManufacturer? _manufacturer;
    private int _value;

    /// <summary>
    ///     Polozka zoznamu - cislo pisma a text pre obsluhu.
    /// </summary>
    private sealed record Item(int Id, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    ///     Pripoji vyber k rozbalovaciemu zoznamu.
    /// </summary>
    /// <param name="combo">rozbalovaci zoznam v okne</param>
    /// <param name="tip">popisok, do ktoreho sa zapise vyznam cisla</param>
    /// <param name="allowColumnDefault">ponuknut predvolene pismo stlpca (cislo -1)</param>
    public TableFontChoice(ExComboBox combo, ToolTip tip, bool allowColumnDefault)
    {
        _combo = combo;
        _tip = tip;
        _allowColumnDefault = allowColumnDefault;
        _value = allowColumnDefault ? -1 : 0;
        _combo.DropDownStyle = ComboBoxStyle.DropDownList;
        _combo.SelectionChangeCommitted += OnCommitted;
        Fill();
    }

    /// <summary>
    ///     Pouzivatel zmenil pismo.
    /// </summary>
    public event EventHandler? ValueChanged;

    /// <summary>
    ///     Cislo pisma.
    /// </summary>
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            Fill();
        }
    }

    /// <summary>
    ///     Vyrobca tabule - urcuje vyznam cisla a ponuku vo vybere podla vzhladu.
    /// </summary>
    public TableManufacturer? Manufacturer
    {
        get => _manufacturer;
        set
        {
            _manufacturer = value;
            UpdateTip();
        }
    }

    private void Fill()
    {
        var items = new List<Item>();
        if (_allowColumnDefault)
            items.Add(new Item(-1, Resources.FontChoice_Stlpec));
        items.AddRange(GlobData.TableFonts.Select(font =>
            new Item(font.FontID, string.Format(CultureInfo.CurrentCulture, Resources.FontChoice_Pismo, font.Name, font.FontID))));
        if (items.All(item => item.Id != _value))
            items.Add(new Item(_value, string.Format(CultureInfo.CurrentCulture, Resources.FontChoice_Vlastne, _value)));
        items.Add(new Item(OtherId, Resources.FontChoice_Ine));

        _combo.BeginUpdate();
        _combo.Items.Clear();
        _combo.Items.AddRange(items.ToArray<object>());
        _combo.SelectedItem = items.First(item => item.Id == _value);
        _combo.EndUpdate();
        UpdateTip();
    }

    private void OnCommitted(object? sender, EventArgs e)
    {
        if (_combo.SelectedItem is not Item item)
            return;

        if (item.Id != OtherId)
        {
            _value = item.Id;
            UpdateTip();
            ValueChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        // dialog az po zatvoreni rozbaleneho zoznamu
        _combo.BeginInvoke(PickOther);
    }

    private void PickOther()
    {
        var start = _value >= 0 ? _value : ElenFontCode.DefaultKeptBits | 0x10;
        using var picker = new FTableFontPicker(start, _manufacturer);
        if (picker.ShowDialog(_combo.FindForm()) != DialogResult.OK)
        {
            Fill();
            return;
        }

        if (picker.AddToList)
        {
            var code = new ElenFontCode(picker.Value);
            GlobData.TableFonts.Add(new TableFont
            {
                Name = code.SuggestedName(),
                FontID = picker.Value,
                Type = code.SuggestedType,
                IsProportional = code.SuggestedProportional,
                Width = code.SuggestedWidth,
                Size = 7,
                FileName = "",
                IsDia = true,
                IsLower = true,
                IsUpper = true,
                IsNumber = true
            });
        }

        _value = picker.Value;
        Fill();
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateTip()
    {
        string text;
        if (_value < 0)
            text = Resources.FontChoice_Stlpec;
        else if (ElenFontCode.AppliesTo(_manufacturer))
            text = string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_Popis, new ElenFontCode(_value).Describe());
        else
            text = string.Format(CultureInfo.CurrentCulture, Resources.ElenFont_InyVyrobca, _manufacturer!.Name);
        _tip.SetToolTip(_combo, text);
    }
}
