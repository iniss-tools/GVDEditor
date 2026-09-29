using ExControls;
using GVDEditor.Properties;
using ToolsCore.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor vzhladu tlacidla.
/// </summary>
internal sealed class SdDesignEditor : SdEditorBase
{
    private readonly ExTextBox _key = TextField();
    private readonly ExTextBox _bitmaps = TextField("6-7,8,9");
    private readonly ExCheckBox _def = Check(Resources.FStateDgm_DefPushBtn);
    private readonly ExTextBox _class = TextField();
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private StateDgmDesign? _d;

    public SdDesignEditor()
    {
        AddHeader(Resources.FStateDgm_Vzhlad);
        AddRow(Resources.FStateDgm_Kluc, _key);
        AddRow(Resources.FStateDgm_Bitmaps, _bitmaps);
        AddFull(_def);
        AddRow(Resources.FStateDgm_Trieda, _class);
        AddInfo(Resources.FStateDgm_BitmapsInfo);
        _errors.SetIconAlignment(_bitmaps, ErrorIconAlignment.MiddleLeft);

        _key.TextChanged += (_, _) => Set(d =>
        {
            d.Key = _key.Text.Trim();
            RenameReferences(d, d.Key);
        });
        _bitmaps.TextChanged += (_, _) =>
        {
            _errors.SetError(_bitmaps, StateDgmBitmaps.TryParse(_bitmaps.Text, out _) ? "" : Resources.FStateDgm_Bitmaps);
            Set(d => d.Bitmaps = _bitmaps.Text.Trim());
        };
        _def.CheckedChanged += (_, _) => Set(d => d.DefaultPushButton = _def.Checked);
        _class.TextChanged += (_, _) => Set(d => d.Class = _class.Text.Trim());
    }

    private void Set(Action<StateDgmDesign> a)
    {
        if (Loading || _d == null) return;
        a(_d);
        RaiseChanged();
    }

    public void Bind(StateDgmDiagram diagram, StateDgmDesign d)
    {
        Loading = true;
        try
        {
            _d = d;
            BindKey(diagram, d.Key);
            _key.Text = d.Key;
            _bitmaps.Text = d.Bitmaps;
            _def.Checked = d.DefaultPushButton;
            _class.Text = d.Class;
        }
        finally
        {
            Loading = false;
        }
    }
}
