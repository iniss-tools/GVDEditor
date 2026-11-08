using AutocompleteMenuNS;
using GVDEditor.Properties;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.TabTabEditor;

internal class EventItem : AutocompleteItem
{
    public EventItem(string ename, string text, string desc = "") : base(text)
    {
        ImageIndex = 2;
        base.ToolTipTitle = Resources.TabTabAC_Event + ename;
        base.MenuText = ename;
        base.ToolTipText = desc;
    }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        return CompareResult.Hidden;
    }
}
