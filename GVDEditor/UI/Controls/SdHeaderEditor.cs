using System.Globalization;
using ExControls;
using GVDEditor.Properties;
using ToolsCore.Iniss.Expressions;
using ToolsCore.Iniss.StateDgm;

namespace GVDEditor.UI.Controls;

/// <summary>
/// Editor hlavicky diagramu - popis suboru a vyraz IndCat.
/// </summary>
internal sealed class SdHeaderEditor : SdEditorBase
{
    private readonly ExTextBox _comments;
    private readonly ExComboBox _indCatMode;
    private readonly ExTextBox _indCat;
    private readonly Label _info;
    private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private StateDgmDiagram? _d;

    public SdHeaderEditor(SdEditorContext context) : base(context)
    {
        AddHeader(Resources.FStateDgm_Diagram);
        _comments = new ExTextBox { Multiline = true, Height = 70, ScrollBars = ScrollBars.Vertical, Width = 300 };
        AddRow(Resources.FStateDgm_HlavickoveKomentare, _comments);
        _indCatMode = Combo(
            new SdEditorContext.Item(Resources.FStateDgm_IndCatPredvoleny, null),
            new SdEditorContext.Item("INDCAT6", "INDCAT6"),
            new SdEditorContext.Item("INDCAT8", "INDCAT8"),
            new SdEditorContext.Item(Resources.FStateDgm_IndCatVlastny, ""));
        AddRow(Resources.FStateDgm_IndCat, _indCatMode);
        _indCat = TextField();
        AddRow("", _indCat);
        _info = AddInfo("");
        _errors.SetIconAlignment(_indCat, ErrorIconAlignment.MiddleLeft);

        _comments.TextChanged += (_, _) =>
        {
            if (Loading || _d == null) return;
            _d.HeaderComments.Clear();
            _d.HeaderComments.AddRange(_comments.Lines.Where(l => l.Length > 0));
            RaiseChanged();
        };
        _indCatMode.SelectedIndexChanged += (_, _) =>
        {
            var v = SdEditorContext.Value(_indCatMode) as string;
            _indCat.Enabled = v == "";
            if (Loading || _d == null) return;
            if (v == "") _indCat.Text = _d.EffectiveIndCat;
            else
            {
                _d.IndCat = v;
                _indCat.Text = v ?? "";
                RaiseChanged();
            }

            ValidateInput();
        };
        _indCat.TextChanged += (_, _) =>
        {
            ValidateInput();
            if (Loading || _d == null || !_indCat.Enabled) return;
            _d.IndCat = _indCat.Text.Trim();
            RaiseChanged();
        };
    }

    public void Bind(StateDgmDiagram d)
    {
        Loading = true;
        try
        {
            _d = d;
            _comments.Lines = d.HeaderComments.ToArray();
            var ic = d.IndCat?.Trim();
            var mode = ic == null ? null : ic.Equals("INDCAT6", StringComparison.OrdinalIgnoreCase) ? "INDCAT6" : ic.Equals("INDCAT8", StringComparison.OrdinalIgnoreCase) ? "INDCAT8" : "";
            SdEditorContext.Select(_indCatMode, mode);
            _indCat.Text = ic ?? "";
            _indCat.Enabled = mode == "";
            _info.Text = string.Format(CultureInfo.CurrentCulture, Resources.FStateDgm_IndCatInfo, d.Categories.Count);
            ValidateInput();
        }
        finally
        {
            Loading = false;
        }
    }

    private void ValidateInput()
    {
        var r = _indCat.Enabled ? Context.Check(_indCat.Text, ExprContext.Condition, false) : null;
        _errors.SetError(_indCat, r?.Message ?? "");
    }
}
