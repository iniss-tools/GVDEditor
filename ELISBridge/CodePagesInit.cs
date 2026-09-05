using System.Runtime.CompilerServices;
using System.Text;

namespace Iniss.Elis;

/// <summary>
///     Registruje poskytovateľa legacy code-page kódovaní (Windows-1250, pozri <see cref="TTNative"/>).
///     Od .NET 5 už nie sú tieto kódovania súčasťou runtime a bez registrácie <see cref="Encoding.GetEncoding(int)"/>
///     vyhadzuje <see cref="NotSupportedException"/>. <see cref="ModuleInitializerAttribute"/> zaručuje spustenie
///     pred prvým použitím čohokoľvek z tejto zostavy (aj pred statickým konštruktorom <see cref="TTNative"/>).
/// </summary>
internal static class CodePagesInit
{
    [ModuleInitializer]
    internal static void Register() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
}
