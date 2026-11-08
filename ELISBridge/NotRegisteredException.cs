namespace Iniss.Elis;

/// <summary>
/// Vyhodena, ked platene data nie su zaregistrovane. Registracne cislo sa zadava
/// pri instalacii dat v programe Cestovne poriadky, nie tu.
/// </summary>
internal sealed class NotRegisteredException : Exception
{
    public NotRegisteredException(string message) : base(message) { }
}
