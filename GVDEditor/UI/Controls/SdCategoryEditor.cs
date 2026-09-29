using System.Globalization;
using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor kategorie vlaku.
/// </summary>
internal sealed class SdCategoryEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
    private readonly ExTextBox _name = TextField();
    private readonly ExTextBox _comment = TextField();
    private readonly ExComboBox _icon;
    private StateDgmCategory? _c;

    public SdCategoryEditor(SdEditorContext context) : base(context)
    {
        AddHeader(Resources.FStateDgm_Kategoria);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Nazov, _name);
        AddRow(Resources.FStateDgm_Komentar, _comment);
        _icon = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat0, 0),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat1, 1),
            new SdEditorContext.Item(Resources.FStateDgm_IkonaKat2, 2));
        AddRow(Resources.FStateDgm_Ikona, _icon);
        AddInfo(Resources.FStateDgm_KatInfo);

        _key.TextChanged += (_, _) => Set(c => c.Key = _key.Text.Trim());
        _name.TextChanged += (_, _) => Set(c => c.Name = _name.Text.Trim());
        _comment.TextChanged += (_, _) => Set(c => c.Comment = _comment.Text.Trim());
        _icon.SelectedIndexChanged += (_, _) => Set(c => c.Icon = SdEditorContext.Value(_icon) as int? ?? 0);
    }

    private void Set(Action<StateDgmCategory> a)
    {
        if (Loading || _c == null) return;
        a(_c);
        RaiseChanged();
    }

    public void Bind(StateDgmCategory c)
    {
        Loading = true;
        try
        {
            _c = c;
            _key.Text = c.Key;
            _name.Text = c.Name;
            _comment.Text = c.Comment;
            if (c.Icon is >= 0 and <= 2) SdEditorContext.Select(_icon, c.Icon);
            else
            {
                _icon.Items.Add(new SdEditorContext.Item(c.Icon.ToString(CultureInfo.CurrentCulture), c.Icon));
                _icon.SelectedIndex = _icon.Items.Count - 1;
            }
        }
        finally
        {
            Loading = false;
        }
    }
}
