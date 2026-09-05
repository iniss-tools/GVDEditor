using System.Runtime.InteropServices;
using System.Text;

namespace Iniss.Elis;

/// <summary>
///     Priama vrstva nad TT.dll z aplikacie Cestovne poriadky (CHAPS).
///     Vsetky exporty su <c>stdcall</c> s nedekorovanymi menami.
/// </summary>
/// <remarks>
///     TT.dll je 32-bitova, takze tento kod musi bezat v x86 procese.
///     Retazce vracia ako <c>char*</c> v kodovani CP1250 a casto do zdielaneho
///     statickeho buffera - vycitat treba hned po volani.
/// </remarks>
internal static class TTNative
{
    private const string Dll = "TT.dll";

    /// <summary>Predvoleny jazyk.</summary>
    public const int DefaultLang = 0;

    /// <summary><see cref="TTTrainRuns" /> - premavanie celeho vlaku, nie konkretnej stanice.</summary>
    public const int WholeTrain = -1;

    /// <summary>Cas, ktory TT.dll pouziva namiesto "ziadny cas".</summary>
    public const ushort NoTime = 0xFFFF;

    /// <summary>Velkost jedneho zaznamu trasy v bajtoch.</summary>
    public const int RouteItemSize = 16;

    /// <summary>Chybovy kod TT.dll pre datum mimo rozsahu platnosti.</summary>
    public const int ErrDateOutOfRange = 18;

    /// <summary>
    ///     Chybove kody, ktorymi loader hlasi, ze platený cestovny poriadok nebolo mozne
    ///     zaregistrovat (nespravne alebo chybajuce registracne cislo). Takyto poriadok
    ///     sa zahodi a <see cref="TTTTCount" /> ho nezapocita.
    /// </summary>
    private static readonly int[] RegistrationErrors = [27, 28, 29];

    /// <summary>Prikaz <see cref="TTService" /> na pridanie registracneho cisla do zoznamu na overenie.</summary>
    private const string SvcAddRegistration = "e271";

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool SetDllDirectory(string path);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr LoadLibrary(string path);

    [DllImport(Dll)]
    public static extern IntPtr TTVer();

    /// <summary>Nacita vsetky *.tt z priecinka. Cesta MUSI koncit spatnou lomkou.</summary>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern void TTInit(string path, IntPtr callback);

    [DllImport(Dll)]
    public static extern int TTTTCount();

    /// <summary>Vrati posledny chybovy kod a zaroven ho vynuluje.</summary>
    [DllImport(Dll)]
    public static extern int TTError();

    [DllImport(Dll)]
    public static extern int TTStCount(int tt);

    [DllImport(Dll)]
    public static extern int TTTrCount(int tt);

    [DllImport(Dll)]
    public static extern IntPtr TTStName(int tt, int st);

    /// <summary>Poradie vystupov je cislo / nazov / typ.</summary>
    [DllImport(Dll)]
    public static extern void TTTrainInfo(int tt, int lang, int tr,
        out IntPtr number, out IntPtr name, out IntPtr type, out uint flags);

    /// <summary>Vrati pocet zaznamov trasy; <paramref name="route" /> ukazuje na ZDIELANY globalny buffer.</summary>
    [DllImport(Dll)]
    public static extern int TTTrainRouteExt(int tt, int tr, out IntPtr route, int flags);

    [DllImport(Dll)]
    public static extern int TTTrainRuns(int tt, int tr, int day, int month, int year, int st, int flag);

    [DllImport(Dll)]
    public static extern int TTTrOwner(int tt, int tr);

    [DllImport(Dll)]
    public static extern int TTTrLine(int tt, int tr);

    [DllImport(Dll)]
    public static extern IntPtr TTOwnerDesc(int tt, int lang, int owner);

    [DllImport(Dll)]
    public static extern IntPtr TTLineDesc(int tt, int lang, int line);

    [DllImport(Dll)]
    public static extern int TTGetOwnersCount(int tt);

    /// <summary>Vytiahne jednu polozku ({ON}, {ONo}, ...) zo zaznamu vrateneho <see cref="TTOwnerDesc" />.</summary>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern IntPtr TTGetField(string tag, IntPtr desc);

    /// <summary>Zlozky <paramref name="from" /> a <paramref name="to" /> su v poradi den, mesiac, rok.</summary>
    [DllImport(Dll)]
    public static extern void TTGetValidityRange(int category, int subcat, [Out] int[] from, [Out] int[] to);

    /// <summary>Servisny prikaz kniznice; pouziva sa na vlozenie registracneho cisla pred nacitanim dat.</summary>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern int TTService(string command, string argument);

    /// <summary>Nastavi identifikaciu klienta, ktora vstupuje do overenia registracie. Segmenty oddeluje '|'.</summary>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern void TTRegisterClient(string client);

    private static readonly Encoding Cp1250 = Encoding.GetEncoding(1250);

    /// <summary>
    ///     Nastavi, odkial sa ma nacitat TT.dll, a rovno ju zavedie.
    /// </summary>
    /// <param name="appDirectory">Priecinok s TT.dll (instalacia Cestovnych poriadkov).</param>
    /// <exception cref="DirectoryNotFoundException">ak priecinok neexistuje</exception>
    /// <exception cref="FileNotFoundException">ak sa v nom TT.dll nenachadza</exception>
    /// <exception cref="DllNotFoundException">ak sa kniznicu nepodari zaviest</exception>
    public static void LoadFrom(string appDirectory)
    {
        if (!Directory.Exists(appDirectory))
            throw new DirectoryNotFoundException($"Priečinok s aplikáciou ELIS neexistuje: {appDirectory}");

        var dll = Path.Combine(appDirectory, Dll);
        if (!File.Exists(dll))
            throw new FileNotFoundException($"V priečinku {appDirectory} sa nenachádza {Dll}.", dll);

        SetDllDirectory(appDirectory);
        if (LoadLibrary(dll) == IntPtr.Zero)
            throw new DllNotFoundException($"Knižnicu {dll} sa nepodarilo zaviesť (chyba {Marshal.GetLastWin32Error()}).");
    }

    /// <summary>
    ///     Zaregistruje platený cestovny poriadok - vlozi registracne cislo (a volitelne
    ///     identifikaciu klienta) do kniznice. MUSI sa zavolat PRED <see cref="TTInit" />,
    ///     lebo overenie prebieha pocas nacitania kazdeho .tt.
    /// </summary>
    /// <param name="registrationNumber">Zakupene registracne cislo; ak je prazdne, nerobi sa nic.</param>
    /// <param name="client">Identifikacia klienta, ak ju dataset vyzaduje (inak <see langword="null" />).</param>
    public static void Register(string? registrationNumber, string? client)
    {
        if (!string.IsNullOrWhiteSpace(client))
            TTRegisterClient(client);

        if (!string.IsNullOrWhiteSpace(registrationNumber))
            TTService(SvcAddRegistration, registrationNumber.Trim());
    }

    /// <summary>Ci je <paramref name="error" /> jeden z chybovych kodov neuspesnej registracie.</summary>
    public static bool IsRegistrationError(int error) => Array.IndexOf(RegistrationErrors, error) >= 0;

    /// <summary>
    ///     Precita retazec, ktory vratila TT.dll, a dekoduje ho z CP1250.
    ///     Kopiruje okamzite, lebo kniznica vracia ukazovatele do zdielanych bufferov.
    /// </summary>
    public static string Str(IntPtr p)
    {
        if (p == IntPtr.Zero)
            return string.Empty;

        var length = 0;
        while (Marshal.ReadByte(p, length) != 0 && length < 4096)
            length++;

        var bytes = new byte[length];
        Marshal.Copy(p, bytes, 0, length);
        return Cp1250.GetString(bytes);
    }
}