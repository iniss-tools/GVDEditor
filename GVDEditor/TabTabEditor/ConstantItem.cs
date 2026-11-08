using AutocompleteMenuNS;
using GVDEditor.Properties;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.TabTabEditor;

internal class ConstantItem : AutocompleteItem
{
    public ConstantItem(string cname, string text, string desc = "") : base(text)
    {
        ImageIndex = 1;
        base.ToolTipTitle = Resources.TabTabAC_Constant + cname;
        base.MenuText = cname;
        base.ToolTipText = desc;
        ConstName = cname;
    }

    public string ConstName { get; }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.Visible;

        return CompareResult.Hidden;
    }
}
