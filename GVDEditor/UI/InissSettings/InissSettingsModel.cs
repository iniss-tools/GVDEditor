using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Riadok tabulky hodnot: nastavenie z katalogu, alebo hodnota mimo katalogu (pozostatok, preklep, farba inej
/// jazykovej verzie), ktora sa da len zmazat.
/// </summary>
internal sealed class SettingRow
{
    private SettingRow(string section, string name, ResolvedSetting? setting, IReadOnlyList<RegLayerValue> extra, IReadOnlyList<RegDiagnostic> diagnostics)
    {
        Section = section;
        Name = name;
        Setting = setting;
        Extra = extra;
        Diagnostics = diagnostics;
    }

    /// <summary>Sekcia (konkretna, napr. Driver3).</summary>
    public string Section { get; }

    /// <summary>Nazov hodnoty.</summary>
    public string Name { get; }

    /// <summary>Vyhodnotene nastavenie; null pri hodnote mimo katalogu.</summary>
    public ResolvedSetting? Setting { get; }

    /// <summary>Vrstvy hodnoty mimo katalogu.</summary>
    public IReadOnlyList<RegLayerValue> Extra { get; }

    /// <summary>Zistenia k riadku.</summary>
    public IReadOnlyList<RegDiagnostic> Diagnostics { get; }

    /// <summary>Kluc zmeny.</summary>
    public string Key => InissSettingsModel.KeyOf(Section, Name);

    /// <summary>Najvyssia zavaznost zisteni alebo null.</summary>
    public RegSeverity? Severity => Diagnostics.Count == 0 ? null : Diagnostics.Max(d => d.Severity);

    /// <summary>Riadok nastavenia.</summary>
    public static SettingRow Of(ResolvedSetting setting) => new(setting.Section, setting.Name, setting, setting.Layers, setting.Diagnostics);

    /// <summary>Riadok hodnoty mimo katalogu.</summary>
    public static SettingRow OfExtra(ResolvedSection section, string name) =>
        new(section.Name, name, null, section.Extra.Where(l => string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase)).ToList(),
            section.Diagnostics.Where(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase)).ToList());
}

/// <summary>Ako sa hodnota upravuje v tabulke.</summary>
internal enum CellKind
{
    /// <summary>Neupravuje sa (binarne, mimo katalogu).</summary>
    ReadOnly,

    /// <summary>Zaskrtavacie pole (nula / nenula).</summary>
    Check,

    /// <summary>Zoznam vymenovanych hodnot.</summary>
    Choice,

    /// <summary>Text (cislo, text, farba #RRGGBB).</summary>
    Text
}

/// <summary>
/// Neulozena zmena jednej hodnoty.
/// </summary>
/// <param name="Row">riadok</param>
/// <param name="Value">nova hodnota (int, string); null = zmazat zo vsetkych vrstiev (obnovit predvolenu)</param>
/// <param name="Target">kam zapisat</param>
internal sealed record PendingChange(SettingRow Row, object? Value, RegWriteTarget Target)
{
    /// <summary>Zmena do katalogu domeny.</summary>
    public RegChange ToChange() => new(Row.Section, Row.Name, Row.Setting?.Setting.Type ?? RegValueType.String, Value, Target);
}

/// <summary>
/// Stav okna Nastavenia INISSu bez prvkov UI: nacitana konfiguracia, riadky a neulozene zmeny.
/// </summary>
internal sealed class InissSettingsModel
{
    private readonly Dictionary<string, PendingChange> _pending = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Vytvori model nad vyhodnotenou konfiguraciou.</summary>
    public InissSettingsModel(ResolvedConfig config, string? iniPath, bool canWriteMachine)
    {
        Config = config;
        IniPath = iniPath;
        CanWriteMachine = canWriteMachine;
    }

    /// <summary>Vyhodnotena konfiguracia.</summary>
    public ResolvedConfig Config { get; }

    /// <summary>Subor .INI vedla exe; null, ak exe nie je zname.</summary>
    public string? IniPath { get; }

    /// <summary>GVDEditor moze zapisovat do HKLM bez zvysenia prav.</summary>
    public bool CanWriteMachine { get; }

    /// <summary>Neulozene zmeny.</summary>
    public IReadOnlyCollection<PendingChange> Pending => _pending.Values;

    /// <summary>Kluc zmeny.</summary>
    public static string KeyOf(string section, string name) => section + "\\" + name;

    /// <summary>Neulozena zmena riadku alebo null.</summary>
    public PendingChange? PendingFor(SettingRow row) => _pending.GetValueOrDefault(row.Key);

    /// <summary>Nastavi alebo zrusi zmenu (rovnaka hodnota ako ucinna a ciel podla zdroja = ziadna zmena).</summary>
    public void SetPending(SettingRow row, object? value, RegWriteTarget target, bool reset)
    {
        if (!reset && row.Setting is { } s && RegValues.AreEqual(value, s.Value, s.Setting.Type) && target == DefaultTarget(row))
        {
            _pending.Remove(row.Key);
            return;
        }

        _pending[row.Key] = new PendingChange(row, reset ? null : value, target);
    }

    /// <summary>Zrusi zmenu riadku.</summary>
    public void Revert(SettingRow row) => _pending.Remove(row.Key);

    /// <summary>Zrusi vsetky zmeny.</summary>
    public void RevertAll() => _pending.Clear();

    /// <summary>Predvoleny ciel zapisu: tam, kde hodnota prave plati (.INI), inak register.</summary>
    public static RegWriteTarget DefaultTarget(SettingRow row) =>
        row.Setting?.Source == RegSource.Ini ? RegWriteTarget.Ini : RegWriteTarget.Registry;

    /// <summary>Vsetky riadky sekcie (nastavenia a hodnoty mimo katalogu).</summary>
    public static IEnumerable<SettingRow> RowsOf(ResolvedSection section) =>
        section.Settings.Select(SettingRow.Of)
            .Concat(section.Extra.Select(e => e.Name).Distinct(StringComparer.OrdinalIgnoreCase).Select(n => SettingRow.OfExtra(section, n)));

    /// <summary>Vsetky riadky konfiguracie.</summary>
    public IEnumerable<SettingRow> AllRows() => Config.Sections.SelectMany(RowsOf);

    /// <summary>Text hodnoty nastavenia pre tabulku a detail.</summary>
    public static string Format(object? value, RegSetting? setting)
    {
        if (value is null) return "—";
        var type = setting?.Type ?? RegValueType.String;
        switch (value)
        {
            case byte[] bytes:
                return string.Format(CultureInfo.CurrentCulture, Resources.InissSettings_Bytes, bytes.Length);
            case string text:
                return text.Length == 0 ? Resources.InissSettings_Empty : text;
            case int n when type == RegValueType.Color:
                return n == RegValues.SystemColor ? Resources.InissSettings_SystemColor : ColorText(n);
            case int n when type == RegValueType.Bool:
                return n != 0 ? Resources.InissSettings_On : Resources.InissSettings_Off;
            case int n:
                var choice = setting?.Choices.FirstOrDefault(c => c.Number == n);
                return choice is null ? n.ToString(CultureInfo.CurrentCulture) : $"{n} – {choice.Text}";
            default:
                return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
        }
    }

    /// <summary>Text surovej hodnoty z registra (vrstva).</summary>
    public static string Format(RegRawValue raw, RegSetting? setting) => raw.Kind switch
    {
        RegRawKind.Dword => Format(raw.Number, setting),
        RegRawKind.String => Format(raw.Text, setting),
        RegRawKind.Binary => Format(raw.Bytes, setting),
        _ => $"{raw.OtherKind}: {raw.Text}"
    };

    /// <summary>Farba 0x00BBGGRR ako #RRGGBB.</summary>
    public static string ColorText(int colorRef) =>
        string.Create(CultureInfo.InvariantCulture, $"#{colorRef & 0xFF:X2}{(colorRef >> 8) & 0xFF:X2}{(colorRef >> 16) & 0xFF:X2}");

    /// <summary>Text zdroja hodnoty (pri <paramref name="short" /> korene registra skratene HKLM/HKCU - pre tabulku).</summary>
    public static string SourceText(RegSource source, bool @short = false) => source switch
    {
        RegSource.Ini => ShortLocationText(RegLocation.Ini),
        RegSource.User => @short ? ShortLocationText(RegLocation.User) : Resources.InissSettings_Source_User,
        RegSource.VirtualStore => Resources.InissSettings_Source_VirtualStore,
        RegSource.Machine => @short ? ShortLocationText(RegLocation.Machine) : Resources.InissSettings_Source_Machine,
        RegSource.ClassDefault => Resources.InissSettings_Source_ClassDefault,
        RegSource.Default => Resources.InissSettings_Source_Default,
        RegSource.UnknownDefault => Resources.InissSettings_Source_UnknownDefault,
        _ => Resources.InissSettings_Source_NotRead
    };

    /// <summary>Skratka miesta pre tabulku (HKLM, HKCU, VirtualStore, .INI).</summary>
    public static string ShortLocationText(RegLocation location) => location switch
    {
        RegLocation.Ini => Resources.InissSettings_Source_Ini,
        RegLocation.User => "HKCU",
        RegLocation.VirtualStore => Resources.InissSettings_Source_VirtualStore,
        _ => "HKLM"
    };

    /// <summary>Text miesta (vrstvy).</summary>
    public static string LocationText(RegLocation location) => location switch
    {
        RegLocation.Ini => Resources.InissSettings_Source_Ini,
        RegLocation.User => Resources.InissSettings_Source_User,
        RegLocation.VirtualStore => Resources.InissSettings_Source_VirtualStore,
        _ => Resources.InissSettings_Source_Machine
    };

    /// <summary>Text zavaznosti.</summary>
    public static string SeverityText(RegSeverity? severity) => severity switch
    {
        RegSeverity.Error => Resources.InissSettings_Severity_Error,
        RegSeverity.Warning => Resources.InissSettings_Severity_Warning,
        RegSeverity.Info => Resources.InissSettings_Severity_Info,
        _ => ""
    };

    /// <summary>Nazov skupiny sekcii.</summary>
    public static string GroupText(RegGroup group) => group switch
    {
        RegGroup.Boards => Resources.InissSettings_Group_Boards,
        RegGroup.Sound => Resources.InissSettings_Group_Sound,
        RegGroup.Timetable => Resources.InissSettings_Group_Timetable,
        RegGroup.Operator => Resources.InissSettings_Group_Operator,
        RegGroup.Logs => Resources.InissSettings_Group_Logs,
        RegGroup.Interfaces => Resources.InissSettings_Group_Interfaces,
        RegGroup.Files => Resources.InissSettings_Group_Files,
        RegGroup.Installation => Resources.InissSettings_Group_Installation,
        RegGroup.Appearance => Resources.InissSettings_Group_Appearance,
        _ => Resources.InissSettings_Group_Debug
    };

    /// <summary>Ako sa hodnota riadku upravuje v tabulke.</summary>
    public static CellKind KindOf(SettingRow row)
    {
        if (row.Setting is not { } s || s.Setting.Type == RegValueType.Binary) return CellKind.ReadOnly;
        if (s.Setting.Type == RegValueType.Bool) return CellKind.Check;
        // zoznam so znamymi hodnotami; zapisat sa da aj ine cislo (vymenovanie v dokumentacii nie je vzdy uplne)
        if (s.Setting.Type == RegValueType.Dword && ExactChoices(s.Setting).Any()) return CellKind.Choice;
        return CellKind.Text;
    }

    /// <summary>Polozky zoznamu hodnot (rovnaky text ako <see cref="Format(object?, RegSetting?)" />).</summary>
    public static IEnumerable<string> ChoiceItems(RegSetting setting) => ExactChoices(setting).Select(c => Format(c.Number, setting));

    /// <summary>Hodnoty, ktore su jedno cislo (nie rozsah, bit "+4" ani slovny popis).</summary>
    private static IEnumerable<RegChoice> ExactChoices(RegSetting setting) =>
        setting.Choices.Where(c => c.Value.Length > 0 && (char.IsAsciiDigit(c.Value[0]) || c.Value[0] == '-') && c.Value.Skip(1).All(char.IsAsciiDigit));

    /// <summary>Text hodnoty na upravu v bunke (cislo desiatkovo, farba #RRGGBB alebo prazdna pre systemovu).</summary>
    public static string EditText(object? value, RegSetting setting) => value switch
    {
        int n when setting.Type == RegValueType.Color => n == RegValues.SystemColor ? "" : ColorText(n),
        int n => n.ToString(CultureInfo.InvariantCulture),
        string t => t,
        _ => ""
    };

    /// <summary>
    /// Hodnota z textu bunky podla typu: cislo desiatkovo alebo 0x… sestnastkovo, farba #RRGGBB (prazdna = systemova),
    /// pri zozname cislo na zaciatku polozky; text tak, ako je.
    /// </summary>
    public static bool TryParse(string? text, RegSetting setting, out object? value)
    {
        value = null;
        var t = (text ?? "").Trim();
        switch (setting.Type)
        {
            case RegValueType.String:
                value = text ?? "";
                return true;
            case RegValueType.Color:
                if (t.Length == 0 || string.Equals(t, Resources.InissSettings_SystemColor, StringComparison.OrdinalIgnoreCase))
                {
                    value = RegValues.SystemColor;
                    return true;
                }

                var hex = t.TrimStart('#');
                if (hex.Length != 6 || !int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb)) return false;
                value = ((rgb & 0xFF) << 16) | (rgb & 0xFF00) | ((rgb >> 16) & 0xFF);
                return true;
            default:
                var number = t.Split('–')[0].Trim();
                if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    && uint.TryParse(number.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var h))
                {
                    value = unchecked((int)h);
                    return true;
                }

                if (!int.TryParse(number, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var n)) return false;
                value = n;
                return true;
        }
    }

    /// <summary>Hodnota riadku vyhovuje hladanemu textu (nazov, sekcia alebo popis).</summary>
    public static bool Matches(SettingRow row, string search) =>
        row.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase)
        || row.Section.Contains(search, StringComparison.CurrentCultureIgnoreCase)
        || (row.Setting?.Setting.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase) ?? false);
}
