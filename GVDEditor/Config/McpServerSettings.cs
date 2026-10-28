using System.Security.Cryptography;
using System.Xml.Serialization;
using ToolsCore.Tools;

namespace GVDEditor.Config;

/// <summary>
/// Server MCP pre agentov (Claude Code, VS Code) - bezi v GVDEditore na tomto pocitaci a pristupuje sa k nemu klucom.
/// </summary>
public sealed class McpServerSettings
{
    /// <summary>Predvoleny port servera.</summary>
    public const int DefaultPort = 5490;

    /// <summary>Zaciatok kluca - odlisi ho od klucov simulatora (<c>ts_</c>).</summary>
    public const string KeyPrefix = "gvd_";

    /// <summary>Server sa spusti s GVDEditorom.</summary>
    [XmlAttribute("Enabled")]
    public bool Enabled { get; set; }

    /// <summary>Agent smie menit otvoreny grafikon (nastroje so zmenami); inak len cita.</summary>
    [XmlAttribute("AllowWrites")]
    public bool AllowWrites { get; set; }

    /// <summary>Port na 127.0.0.1.</summary>
    [XmlAttribute("Port")]
    public int Port { get; set; } = DefaultPort;

    /// <summary>Pristupovy kluc sifrovany pre aktualneho pouzivatela Windows (DPAPI).</summary>
    [XmlAttribute("Key")]
    public string ProtectedKey { get; set; } = "";

    /// <summary>Pristupovy kluc (<c>Authorization: Bearer</c>); prazdny = este nevytvoreny alebo ulozeny inym pouzivatelom.</summary>
    [XmlIgnore]
    public string Key
    {
        get => UserSecret.Unprotect(ProtectedKey, Entropy);
        set => ProtectedKey = UserSecret.Protect(value, Entropy);
    }

    /// <summary>Adresa servera MCP.</summary>
    [XmlIgnore]
    public string Address => $"http://127.0.0.1:{Port}/mcp";

    /// <summary>Kluc; ak ziadny nie je, vytvori novy.</summary>
    public string EnsureKey()
    {
        var key = Key;
        if (key.Length > 0)
            return key;
        Key = key = NewKey();
        return key;
    }

    /// <summary>Novy nahodny kluc: 32 bajtov v base64url za <see cref="KeyPrefix" />.</summary>
    public static string NewKey() =>
        KeyPrefix + Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    /// <summary>Kopia.</summary>
    public McpServerSettings Clone() => new() { Enabled = Enabled, AllowWrites = AllowWrites, Port = Port, ProtectedKey = ProtectedKey };

    /// <summary>Rovnake nastavenie - pri zmene sa server spusti znova (aj povolenie zmien - nastroje sa registruju pri starte).</summary>
    public bool SameAs(McpServerSettings other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Enabled == other.Enabled && AllowWrites == other.AllowWrites && Port == other.Port && Key == other.Key;
    }

    private static readonly byte[] Entropy = "GVDEditor.McpServer.Key"u8.ToArray();
}
