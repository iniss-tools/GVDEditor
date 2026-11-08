// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GVDEditor.TabTabEditor;

internal readonly struct FunParameter
{
    public readonly FunParameterType Type;
    public readonly string Name;

    public FunParameter(FunParameterType type, string name)
    {
        Type = type;
        Name = name;
    }
}
