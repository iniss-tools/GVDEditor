using AutocompleteMenuNS;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.TabTabEditor;

internal class FunctionItem : AutocompleteItem
{
    public FunctionItem(string fname, string text, string desc = "", FunReturnType returns = FunReturnType.Bool,
        params FunParameter[] parameters) : base(text)
    {
        ImageIndex = 0;

        var titleB = new StringBuilder();
        titleB.Append(fname);
        if (parameters.Length != 0)
        {
            titleB.Append('(');
            foreach (var par in parameters)
            {
                titleB.Append(par.Type.ToString() + ' ');
                titleB.Append(par.Name);
            }

            titleB.Append(')');
        }

        base.ToolTipTitle = $"(funkcia) {returns} {titleB}";
        base.MenuText = titleB.ToString();

        base.ToolTipText = desc;
        FunctionName = fname;
    }

    public string FunctionName { get; }

    public override CompareResult Compare(string fragmentText)
    {
        if (Text.StartsWith(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.VisibleAndSelected;
        if (Text.Contains(fragmentText.ToUpperInvariant(), StringComparison.Ordinal))
            return CompareResult.Visible;
        return CompareResult.Hidden;
    }
}
