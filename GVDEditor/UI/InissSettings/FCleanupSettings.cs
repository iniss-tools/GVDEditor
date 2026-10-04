using System.Globalization;
using GVDEditor.Properties;
using ToolsCore.Iniss.Registry;
using ToolsCore.Tools;

namespace GVDEditor.UI.InissSettings;

/// <summary>
/// Vycistenie nastaveni INISSu v registri - pozostatky, nezname nazvy a sekcie, stare nazvy, hodnoty zleho typu
/// a kopie vo VirtualStore. Okno len vyberie polozky (<see cref="Selected" />); zapise ich okno Nastavenia INISSu
/// po zalohe.
/// </summary>
internal partial class FCleanupSettings : Form
{
    private readonly List<RegCleanupItem> _items;

    /// <summary>
    /// Vytvori okno so zoznamom poloziek.
    /// </summary>
    /// <param name="appName">vetva registra</param>
    /// <param name="items">co sa da vycistit</param>
    /// <param name="backupDir">priecinok, do ktoreho sa pred zapisom ulozi zaloha</param>
    public FCleanupSettings(string appName, List<RegCleanupItem> items, string backupDir)
    {
        _items = items;
        InitializeComponent();
        dgvItems.AutoGenerateColumns = false;
        this.ApplyThemeAndFonts();
        dgvItems.BackgroundColor = dgvItems.DefaultCellStyle.BackColor.IsEmpty ? SystemColors.Window : dgvItems.DefaultCellStyle.BackColor;
        lIntro.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissCleanup_Intro, appName);
        lBackup.Text = string.Format(CultureInfo.CurrentCulture, Resources.InissCleanup_Backup, backupDir);

        foreach (var item in items)
        {
            var index = dgvItems.Rows.Add(item.Recommended, item.Name is null ? item.Section : item.Section + "\\" + item.Name,
                InissSettingsModel.ShortLocationText(item.Location),
                item.Raw is null ? Resources.InissCleanup_WholeSection : InissSettingsModel.Format(item.Raw, null),
                Why(item), Action(item));
            dgvItems.Rows[index].Tag = item;
        }

        dgvItems.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (dgvItems.IsCurrentCellDirty) dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        dgvItems.CellValueChanged += (_, _) => UpdateButton();
        bClean.Click += (_, _) => DialogResult = DialogResult.OK;
        UpdateButton();
    }

    /// <summary>Oznacene polozky.</summary>
    public List<RegCleanupItem> Selected =>
        dgvItems.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[cDo.Index].Value is true).Select(r => (RegCleanupItem)r.Tag!).ToList();

    private void UpdateButton() => bClean.Enabled = Selected.Count > 0;

    private static string Why(RegCleanupItem item) => item.Kind switch
    {
        RegCleanupKind.Leftover => Resources.InissCleanup_Why_Leftover,
        RegCleanupKind.Unknown => Resources.InissCleanup_Why_Unknown,
        RegCleanupKind.OtherLanguageColor => Resources.InissCleanup_Why_OtherLanguageColor,
        RegCleanupKind.UnknownSection => Resources.InissCleanup_Why_UnknownSection,
        RegCleanupKind.LegacyGhost => Resources.InissCleanup_Why_LegacyGhost,
        RegCleanupKind.LegacyName => Resources.InissCleanup_Why_LegacyName,
        RegCleanupKind.WrongType => Resources.InissCleanup_Why_WrongType,
        _ => Resources.InissCleanup_Why_VirtualStoreCopy
    };

    private static string Action(RegCleanupItem item) => item.Kind switch
    {
        RegCleanupKind.UnknownSection => Resources.InissCleanup_Action_DeleteSection,
        RegCleanupKind.LegacyName when item.Replacement is not null => string.Format(CultureInfo.CurrentCulture, Resources.InissCleanup_Action_Rename, item.NewName),
        RegCleanupKind.WrongType when item.Replacement is not null => string.Format(CultureInfo.CurrentCulture, Resources.InissCleanup_Action_Convert,
            InissSettingsModel.Format(item.Replacement, null)),
        RegCleanupKind.WrongType => Resources.InissCleanup_Action_DeleteDefault,
        RegCleanupKind.VirtualStoreCopy => string.Format(CultureInfo.CurrentCulture, Resources.InissCleanup_Action_DeleteCopy,
            item.Reference is null ? "" : InissSettingsModel.Format(item.Reference, null)),
        _ => Resources.InissCleanup_Action_Delete
    };
}
