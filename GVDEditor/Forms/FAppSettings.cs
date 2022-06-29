using ExControls;
using GVDEditor.Properties;
using GVDEditor.Tools;
using GVDEditor.XML;
using System.Globalization;
using ToolsCore;
using ToolsCore.Tools;
using ToolsCore.XML;
using ColorSetting = ToolsCore.XML.ColorSetting;
using CommandShortcut = ToolsCore.XML.CommandShortcut;
using FormUtils = ToolsCore.Tools.FormUtils;
using ShortcutName = ToolsCore.XML.ShortcutName;
// ReSharper disable MemberCanBePrivate.Global

namespace GVDEditor.Forms;

/// <summary>
///     Dialog - Nastavenia programu.
/// </summary>
public partial class FAppSettings : Form
{
    public const string General = "uObecne";
    public const string Environment = "uProstredie";
    public const string Localization = "uLocalization";
    public const string FontsColors = "uFontsColors"; 
    public const string Logging = "uLogging";
    public const string Playing = "uPlaying";
    public const string Desktop = "uDesktop";
    public const string Shortcuts = "uShortcuts";
    public const string Startup = "uStartup";

    private readonly BindingList<DesktopColumn> columns;

    private readonly List<int> fontSizes = new();
    private readonly bool initialization;
    private bool argsinit;

    [Localizable(true)] 
    private readonly string lShortcutHelp1 = Resources.FAppSettings_lShortcutHelp1;

    [Localizable(true)] 
    private readonly string lShortcutHelp2 = Resources.FAppSettings_lShortcutHelp2;

    private readonly List<string> systemFonts = Utils.GetSystemFontNames();

    private BindingList<AppFont> appFonts;

    private Color actualColorBackground, actualColorForeground;
    private Font actualColorSettingsFont;
    private bool actualFontBold;
    private float actualFontSize;
    private bool actualDarkScrollBar, actualDarkTitleBar, actualDefaultConStyle;

    private readonly BindingList<GVDEditorStyle> styles;
    private int usedStyleId, actualstyle = -1;
    private BindingList<ColorCategory> colorCategories;

    private ColorSetting previousColorSetting;
    private ColorCategory previousSelectedColorCategory;

    private int shortcutindex = -1;
    private BindingList<CommandShortcut> shortcuts;

    private bool showInfoRestart;

    private readonly string selectTree;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FAppSettings"/>.
    /// </summary>
    public FAppSettings(string opentree = null)
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);

        treeMenu.SetTheme(WindowsTheme.Explorer);
        this.ApplyTheme();

        bColumnUp.Text += $@" {Utils.Arrow(Arrow.Up)}";
        bColumnDown.Text += $@" {Utils.Arrow(Arrow.Down)}";

        var config = GlobData.Config;

        initialization = true;

        cbDebugModeGUI.SelectedIndex = config.DebugModeGUI switch
        {
            Config.DebugMode.OnlyMessage => 0,
            Config.DebugMode.DetailedInfo => 1,
            Config.DebugMode.AppCrash => 2,
            _ => 0
        };

        cbAppLanguage.SelectedIndex = config.Language switch
        {
            Config.AppLanguage.Slovak => 0,
            Config.AppLanguage.Czech => 1,
            _ => 0
        };

        initialization = false;

        cbDateRemLocate.SelectedIndex = config.DateRemLocate switch
        {
            Config.AppLanguage.Slovak => 0,
            Config.AppLanguage.Czech => 1,
            _ => 0
        };

        switch (config.DesktopMenuMode)
        {
            case Config.DesktopMenu.MsTs:
                rbMSandTS.Checked = true;
                break;
            case Config.DesktopMenu.TsOnly:
                rbTS.Checked = true;
                break;
            case Config.DesktopMenu.MsOnly:
                rbMS.Checked = true;
                break;
        }

        cbMoreInstance.Checked = config.MoreInstance;
        cbClassicGUI.Checked = config.ClassicGUI;
        cbAutoTableText.Checked = config.AutoTableText;
        cbDisableVariantCheck.Checked = config.DisableVariantCheck;
        cbAutoVariant.Checked = config.AutoVariant;
        cbShowStateRow.Checked = config.ShowStateRow;
        cboxDateTimeInStateRow.Checked = config.ShowDateTimeInStateRow;
        cbShowRowsHeader.Checked = config.ShowRowsHeader;
        cbFitLastColumn.Checked = config.FitLastColumn;
        cbCheckUpdate.Checked = config.CheckUpdate;

        cbLoggingInfo.Checked = config.LoggingInfo;
        cbLoggingError.Checked = config.LoggingError;

        nudWordPause.Value = config.PlayerWordPause;

        cbArgRegister.DataSource = Tools.AppRegistry.GetINISSRegisters();
        cbStartINISSAdmin.Checked = config.StartupINISSConfig.RunAsAdmin;
        tbCmdArguments.Text = config.StartupINISSConfig.CmdArgs;
        FormatArgs(config.StartupINISSConfig.CmdArgs);

        appFonts = new BindingList<AppFont>(config.Fonts.GetValues());
        dgvAppFonts.DataSource = appFonts;

        columns = new BindingList<DesktopColumn>(config.DesktopCols.GetOrderedValues());
        dgvDesktopColums.DataSource = columns;

        shortcuts = new BindingList<CommandShortcut>(config.Shortcuts.GetValues());
        dgvShortcuts.DataSource = shortcuts;
        FindAndCheckDuplicateShortcuts();

        initialization = true;
        cbFont.DataSource = systemFonts;

        for (var i = 6; i <= 24; i++) fontSizes.Add(i);

        cbFontSize.DataSource = fontSizes;

        styles = new BindingList<GVDEditorStyle>(GlobData.Styles.StyleList);
        cbStyles.DataSource = styles;
        usedStyleId = GlobData.Styles.UsingStyleID;
        cbStyles.Invalidate();
        cbStyles.SelectedIndex = GlobData.Styles.UsingStyleID;

        initialization = false;

        selectTree = opentree;
    }

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
            return cp;
        }
    }

    //TODO prerobit tuto metodu
    private void InitializeAppColorSettings(GVDEditorStyle style)
    {
        if (style == null)
            return;

        GVDEditorSettingsNaming.NameColorSettings(style);

        var categories = new List<ColorCategory>();

        var itemTETabTab = new ColorCategory
        {
            Name = "Textový editor TabTab",
            Font = style.TabTabEditorScheme.Font
        };

        var itemDeskopTT = new ColorCategory
        {
            Name = "Pracovná plocha - Typ vlaku",
            Font = style.TrainTypeColumnScheme.Font
        };

        var itemControls = new ColorCategory
        {
            Name = "Ovládacie prvky",
            DisableFontEdit = true
        };

        var tesettingsTETabTab = new List<ColorSetting>();
        var tesettingsTETabTabCP = new List<ColorSetting>();
        tesettingsTETabTab.AddRange(new[]
        {
            style.TabTabEditorScheme.Default,
            style.TabTabEditorScheme.Function,
            style.TabTabEditorScheme.Identifier,
            style.TabTabEditorScheme.Number,
            style.TabTabEditorScheme.String,
            style.TabTabEditorScheme.Comment,
            style.TabTabEditorScheme.Var,
            style.TabTabEditorScheme.Event,
            style.TabTabEditorScheme.OnNewLine,
            style.TabTabEditorScheme.Operator,
            style.TabTabEditorScheme.Constant,
            style.TabTabEditorScheme.SelBraces,
            style.TabTabEditorScheme.SelBraceBad
        });
        foreach (var t in tesettingsTETabTab) tesettingsTETabTabCP.Add(ColorSetting.Copy(t));
        itemTETabTab.Settings = tesettingsTETabTabCP;
        categories.Add(itemTETabTab);

        var tesettingsDesktopTT = new List<ColorSetting>();
        var tesettingsDesktopTTCP = new List<ColorSetting>();
        tesettingsDesktopTT.AddRange(new[]
        {
            style.TrainTypeColumnScheme.Os,
            style.TrainTypeColumnScheme.R,
            style.TrainTypeColumnScheme.X,
            style.TrainTypeColumnScheme.Sl
        });
        foreach (var t in tesettingsDesktopTT) tesettingsDesktopTTCP.Add(ColorSetting.Copy(t));
        itemDeskopTT.Settings = tesettingsDesktopTTCP;
        categories.Add(itemDeskopTT);

        var tesettingsControls = new List<ColorSetting>();
        tesettingsControls.AddRange(new[]
        {
            style.ControlsColorScheme.Panel,
            style.ControlsColorScheme.Button,
            style.ControlsColorScheme.Label,
            style.ControlsColorScheme.Box,
            style.ControlsColorScheme.Border,
            style.ControlsColorScheme.Mark,
            style.ControlsColorScheme.Highlight
        });

        var tesettingsControlsCP = tesettingsControls.Select(ColorSetting.Copy).ToList();
        itemControls.Settings = tesettingsControlsCP;
        categories.Add(itemControls);

        colorCategories = new BindingList<ColorCategory>(categories);
        cbColorSettings.DataSource = null;
        cbColorSettings.DataSource = colorCategories;
    }

    private void SaveAppColorSettings(GVDEditorStyle style)
    {
        style.TabTabEditorScheme.Font = colorCategories[0].Font;
        style.TrainTypeColumnScheme.Font = colorCategories[1].Font;

        style.TabTabEditorScheme.Default = colorCategories[0].Settings[0];
        style.TabTabEditorScheme.Function = colorCategories[0].Settings[1];
        style.TabTabEditorScheme.Identifier = colorCategories[0].Settings[2];
        style.TabTabEditorScheme.Number = colorCategories[0].Settings[3];
        style.TabTabEditorScheme.String = colorCategories[0].Settings[4];
        style.TabTabEditorScheme.Comment = colorCategories[0].Settings[5];
        style.TabTabEditorScheme.Var = colorCategories[0].Settings[6];
        style.TabTabEditorScheme.Event = colorCategories[0].Settings[7];
        style.TabTabEditorScheme.OnNewLine = colorCategories[0].Settings[8];
        style.TabTabEditorScheme.Operator = colorCategories[0].Settings[9];
        style.TabTabEditorScheme.Constant = colorCategories[0].Settings[10];
        style.TabTabEditorScheme.SelBraces = colorCategories[0].Settings[11];
        style.TabTabEditorScheme.SelBraceBad = colorCategories[0].Settings[12];

        style.TrainTypeColumnScheme.Os = colorCategories[1].Settings[0];
        style.TrainTypeColumnScheme.R = colorCategories[1].Settings[1];
        style.TrainTypeColumnScheme.X = colorCategories[1].Settings[2];
        style.TrainTypeColumnScheme.Sl = colorCategories[1].Settings[3];

        style.ControlsColorScheme.Panel = colorCategories[2].Settings[0];
        style.ControlsColorScheme.Button = colorCategories[2].Settings[1];
        style.ControlsColorScheme.Label = colorCategories[2].Settings[2];
        style.ControlsColorScheme.Box = colorCategories[2].Settings[3];
        style.ControlsColorScheme.Border = colorCategories[2].Settings[4];
        style.ControlsColorScheme.Mark = colorCategories[2].Settings[5];
        style.ControlsColorScheme.Highlight = colorCategories[2].Settings[6];
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        if (styles.Count != 0)
        {
            cbStyles.SelectedIndex = -1;
            cbStyles.SelectedIndex = 0;
        }

        var config = new GVDEditorConfig
        {
            DebugModeGUI = cbDebugModeGUI.SelectedIndex switch
            {
                0 => Config.DebugMode.OnlyMessage,
                1 => Config.DebugMode.DetailedInfo,
                2 => Config.DebugMode.AppCrash,
                _ => Config.DebugMode.OnlyMessage
            },
            Language = cbAppLanguage.SelectedIndex switch
            {
                0 => Config.AppLanguage.Slovak,
                1 => Config.AppLanguage.Czech,
                _ => Config.AppLanguage.Slovak
            }
        };

        switch (cbDateRemLocate.SelectedIndex)
        {
            case 0:
                config.DateRemLocate = Config.AppLanguage.Slovak;
                DateLimit.Loc = DateLimit.Locale.SK;
                break;
            case 1:
                config.DateRemLocate = Config.AppLanguage.Czech;
                DateLimit.Loc = DateLimit.Locale.CZ;
                break;
            default:
                config.DateRemLocate = Config.AppLanguage.Slovak;
                DateLimit.Loc = DateLimit.Locale.SK;
                break;
        }

        if (rbMSandTS.Checked)
            config.DesktopMenuMode = Config.DesktopMenu.MsTs;
        else if (rbMS.Checked)
            config.DesktopMenuMode = Config.DesktopMenu.MsOnly;
        else if (rbTS.Checked) config.DesktopMenuMode = Config.DesktopMenu.TsOnly;

        config.MoreInstance = cbMoreInstance.Checked;
        config.ClassicGUI = cbClassicGUI.Checked;
        config.AutoTableText = cbAutoTableText.Checked;
        config.DisableVariantCheck = cbDisableVariantCheck.Checked;
        config.AutoVariant = cbAutoVariant.Checked;
        config.ShowRowsHeader = cbShowRowsHeader.Checked;
        config.ShowStateRow = cbShowStateRow.Checked;
        config.ShowDateTimeInStateRow = cboxDateTimeInStateRow.Checked;
        config.FitLastColumn = cbFitLastColumn.Checked;
        config.CheckUpdate = cbCheckUpdate.Checked;

        config.LoggingInfo = cbLoggingInfo.Checked;
        config.LoggingError = cbLoggingError.Checked;

        config.PlayerWordPause = decimal.ToInt32(nudWordPause.Value);

        config.StartupINISSConfig = new StartupINISS {RunAsAdmin = cbStartINISSAdmin.Checked, CmdArgs = tbCmdArguments.Text.Trim()};

        config.Fonts.Labels = appFonts[0];
        config.Fonts.Buttons = appFonts[1];
        config.Fonts.Menu = appFonts[2];
        config.Fonts.ColsHeader = appFonts[3];
        config.Fonts.TableCells = appFonts[4];
        config.Fonts.StateRow = appFonts[5];

        for (var i = 0; i < columns.Count; i++) columns[i].Order = i;

        config.DesktopCols.Number = columns.First(x => x.Id == 0);
        config.DesktopCols.Type = columns.First(x => x.Id == 1);
        config.DesktopCols.Name = columns.First(x => x.Id == 2);
        config.DesktopCols.LinkaPrichod = columns.First(x => x.Id == 3);
        config.DesktopCols.LinkaOdchod = columns.First(x => x.Id == 4);
        config.DesktopCols.Routing = columns.First(x => x.Id == 5);
        config.DesktopCols.Prichod = columns.First(x => x.Id == 6);
        config.DesktopCols.Odchod = columns.First(x => x.Id == 7);
        config.DesktopCols.VychodziaStanica = columns.First(x => x.Id == 8);
        config.DesktopCols.KonecnaStanica = columns.First(x => x.Id == 9);
        config.DesktopCols.DateLimit = columns.First(x => x.Id == 10);
        config.DesktopCols.Track = columns.First(x => x.Id == 11);
        config.DesktopCols.Operator = columns.First(x => x.Id == 12);
        config.DesktopCols.OtherBtn = columns.First(x => x.Id == 13);

        config.Shortcuts.NewGVD = shortcuts[0];
        config.Shortcuts.OpenGVD = shortcuts[1];
        config.Shortcuts.ImportData = shortcuts[2];
        config.Shortcuts.ImportGVD = shortcuts[3];
        config.Shortcuts.Save = shortcuts[4];
        config.Shortcuts.Analyze = shortcuts[5];

        config.Shortcuts.AddTrain = shortcuts[6];
        config.Shortcuts.EditTrain = shortcuts[7];
        config.Shortcuts.DeleteTrains = shortcuts[8];
        config.Shortcuts.DuplicateTrain = shortcuts[9];

        config.Shortcuts.LocalSettings = shortcuts[10];
        config.Shortcuts.GlobalSettings = shortcuts[11];
        config.Shortcuts.AppSettings = shortcuts[12];

        config.Shortcuts.InfoApp = shortcuts[13];
        config.Shortcuts.UpdateNotes = shortcuts[14];
        config.Shortcuts.RunINISS = shortcuts[15];
        config.Shortcuts.ShutdownINISS = shortcuts[16];
        config.Shortcuts.KillINISS = shortcuts[17];
        config.Shortcuts.RestartINISS = shortcuts[18];

        config.Shortcuts.LSGrafikon = shortcuts[19];
        config.Shortcuts.LSStanice = shortcuts[20];
        config.Shortcuts.LSDopravcovia = shortcuts[21];
        config.Shortcuts.LSPlatforms = shortcuts[22];
        config.Shortcuts.LSKolaje = shortcuts[23];
        config.Shortcuts.LSTPhysicals = shortcuts[24];
        config.Shortcuts.LSTLogicals = shortcuts[25];
        config.Shortcuts.LSTCatalogs = shortcuts[26];
        config.Shortcuts.LSTabTab = shortcuts[27];
        config.Shortcuts.LSTTexts = shortcuts[28];
        config.Shortcuts.LSTFonts = shortcuts[29];
        config.Shortcuts.LSTabTabEditor = shortcuts[30];

        config.Shortcuts.GSGrafikony = shortcuts[31];
        config.Shortcuts.GSLanguages = shortcuts[32];
        config.Shortcuts.GSMeskania = shortcuts[33];
        config.Shortcuts.GSTrainTypes = shortcuts[34];
        config.Shortcuts.GSAudio = shortcuts[35];

        config.Shortcuts.DatObm = shortcuts[36];

        GlobData.Config = config;
        GlobSettings.Fonts = config.Fonts;
        XMLSerialization.WriteData(Utils.CombinePath(Application.StartupPath, FileConsts.FILE_CONFIG), config);

        GlobData.Styles.StyleList = styles.ToList();
        if (GlobData.Styles.UsingStyleID != usedStyleId)
        {
            GlobData.UsingStyle = styles[usedStyleId];
            GlobSettings.UsingStyle = styles[usedStyleId];
            GlobData.Styles.UsingStyleID = usedStyleId;
        }

        Styles<GVDEditorStyle>.WriteData(Utils.CombinePath(Application.StartupPath, FileConsts.FILE_STYLES), GlobData.Styles);

        if (showInfoRestart)
        {
            var result = 
                Utils.ShowQuestion(Resources.FAppSettings_bSave_Click_Niektoré_zmeny_sa_prejavia_až_po_reštarte_programu);

            if (result == DialogResult.Yes) 
                Utils.RestartApp();
        }

        DialogResult = DialogResult.OK;
    }

    private void bStorno_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void cbAppLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!initialization)
            showInfoRestart = true;
    }

    private void cbDebugModeGUI_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!initialization)
            if (cbDebugModeGUI.SelectedIndex == 2)
            {
                var result = 
                    Utils.ShowWarning(Resources.FAppSettings_cbDebugModeGUI_SelectedIndexChanged_Not_For_All_Users, MessageBoxButtons.YesNo);

                if (result == DialogResult.No) cbDebugModeGUI.SelectedIndex = 0;
            }
    }

    private void cbColorSettings_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbColorSettings.SelectedIndex == -1) 
            return;

        if (previousSelectedColorCategory != null) previousSelectedColorCategory.Font = actualColorSettingsFont;

        var category = (ColorCategory) cbColorSettings.SelectedItem;

        actualColorSettingsFont = category.Font;
        if (!category.DisableFontEdit) 
            actualFontSize = category.Font.Size;

        listSettings.DataSource = null;
        listSettings.DataSource = category.Settings;

        if (!category.DisableFontEdit)
        {
            cbFont.Enabled = true;
            cbFontSize.Enabled = true;
            cbBold.Enabled = true;
            cbFontSize.Text = category.Font.Size.ToString(CultureInfo.InvariantCulture);
            cbFont.SelectedItem = category.Font.Name;
        }
        else
        {
            cbFontSize.Text = "";
            cbFont.Enabled = false;
            cbFontSize.Enabled = false;
            cbBold.Enabled = false;
        }

        previousSelectedColorCategory = category;
    }

    private void bColorsUseDefault_Click(object sender, EventArgs e)
    {
        var style = new GVDEditorStyle();
        if (cbStyles.SelectedItem is Style { Name: StyleNames.DARK })
        {
            style.ControlsColorScheme = Style.SetDefaultDarkControlsScheme();
            style.TabTabEditorScheme = GVDEditorStyle.SetTabTabEditorSchemeDarkDefault();
        }

        InitializeAppColorSettings(style);
    }

    private void cbFont_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbFont.SelectedIndex != -1 && actualFontSize > 0)
        {
            actualColorSettingsFont = new Font(cbFont.SelectedItem as string, actualFontSize, FontStyle.Regular,
                GraphicsUnit.Point);

            labelExample.Font = new Font(actualColorSettingsFont.FontFamily, actualFontSize,
                actualColorSettingsFont.Style, GraphicsUnit.Point);
        }
    }

    private void cbFont_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1)
            return;

        e.DrawBackground();

        var font = new Font(systemFonts[e.Index], 10);
        var text = (string) cbFont.Items[e.Index];

        TextRenderer.DrawText(e.Graphics,text, Utils.IsFontMonospaced(e.Graphics, font) ? new Font(e.Font, FontStyle.Bold) : e.Font, e.Bounds.Location,e.ForeColor);

        e.DrawFocusRectangle();
    }

    private void cbFontSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!initialization && cbFontSize.SelectedIndex != -1)
            if (float.TryParse(cbFontSize.SelectedItem as string, out var num))
            {
                actualColorSettingsFont = new Font(actualColorSettingsFont.Name, num, FontStyle.Regular,
                    GraphicsUnit.Point);
                actualFontSize = num;

                labelExample.Font = actualFontBold
                    ? new Font(actualColorSettingsFont.FontFamily, num, FontStyle.Bold, GraphicsUnit.Point)
                    : new Font(actualColorSettingsFont.FontFamily, num, FontStyle.Regular, GraphicsUnit.Point);
            }
    }

    private void cbFontSize_TextChanged(object sender, EventArgs e)
    {
        if (initialization) return;

        if (float.TryParse(cbFontSize.Text, out var num))
        {
            actualColorSettingsFont = new Font(actualColorSettingsFont.Name, num);
            actualFontSize = num;

            labelExample.Font = actualFontBold
                ? new Font(actualColorSettingsFont.FontFamily, num, FontStyle.Bold, GraphicsUnit.Point)
                : new Font(actualColorSettingsFont.FontFamily, num, FontStyle.Regular, GraphicsUnit.Point);
        }
    }

    private void cbFontSize_Validating(object sender, CancelEventArgs e)
    {
        var isnum = float.TryParse(cbFontSize.Text, out var num);
        if (!isnum || num > 26) e.Cancel = true;
    }

    private void listSettings_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listSettings.SelectedIndex != -1)
        {
            if (previousColorSetting != null)
            {
                previousColorSetting.Bold = actualFontBold;
                previousColorSetting.ForeColor = actualColorForeground;
                previousColorSetting.BackColor = actualColorBackground;
            }

            var setting = (ColorSetting) listSettings.SelectedItem;

            if (setting.DisableBackColorEdit)
            {
                pbBackgroundColor.BackColor = BackColor;
                pbBackgroundColor.Enabled = false;
                bSetBackgroundColor.Enabled = false;
            }
            else
            {
                pbBackgroundColor.Enabled = true;
                bSetBackgroundColor.Enabled = true;
                pbBackgroundColor.BackColor = setting.BackColor;
            }
                
            pbForegroundColor.BackColor = setting.ForeColor;
            cbBold.Checked = setting.Bold;

            if (!colorCategories[cbColorSettings.SelectedIndex].DisableFontEdit)
            {
                labelExample.Font = setting.Bold
                    ? new Font(actualColorSettingsFont, FontStyle.Bold)
                    : new Font(actualColorSettingsFont, FontStyle.Regular);
            }
            labelExample.ForeColor = setting.ForeColor;
            labelExample.BackColor = setting.DisableBackColorEdit ? BackColor : setting.BackColor;

            actualFontBold = setting.Bold;
            actualColorForeground = setting.ForeColor;
            actualColorBackground = setting.BackColor;

            previousColorSetting = setting;
        }
    }

    private void bSetBackgroundColor_Click(object sender, EventArgs e)
    {
        colorDialog.Color = actualColorBackground;
        var result = colorDialog.ShowDialog();
        if (result == DialogResult.OK)
        {
            pbBackgroundColor.BackColor = colorDialog.Color;
            labelExample.BackColor = colorDialog.Color;
            actualColorBackground = colorDialog.Color;
        }
    }

    private void bSetForegroundColor_Click(object sender, EventArgs e)
    {
        colorDialog.Color = actualColorForeground;
        var result = colorDialog.ShowDialog();
        if (result == DialogResult.OK)
        {
            pbForegroundColor.BackColor = colorDialog.Color;
            labelExample.ForeColor = colorDialog.Color;
            actualColorForeground = colorDialog.Color;
        }
    }

    private void cbBold_CheckedChanged(object sender, EventArgs e)
    {
        actualFontBold = cbBold.Checked;

        labelExample.Font = cbBold.Checked
            ? new Font(actualColorSettingsFont, FontStyle.Bold)
            : new Font(actualColorSettingsFont, FontStyle.Regular);
    }

    private void treeMenu_AfterSelect(object sender, TreeViewEventArgs e)
    {
        HideAllPanels();

        switch (e.Node.Name)
        {
            case General:
                pObecne.Visible = true;
                break;
            case Environment:
                pEnvironment.Visible = true;
                break;
            case Localization:
                pLocalization.Visible = true;
                break;
            case FontsColors:
                pFontsColors.Visible = true;
                break;
            case Logging:
                pLogging.Visible = true;
                break;
            case Playing:
                pPlaying.Visible = true;
                break;
            case Desktop:
                pDesktop.Visible = true;
                break;
            case Shortcuts:
                pShortcuts.Visible = true;
                break;
            case Startup:
                pStartup.Visible = true;
                break;
            default:
                pObecne.Visible = true;
                break;
        }
    }

    private void FAppSettings_Load(object sender, EventArgs e)
    {
        imagesMenu.Images.Clear();
        imagesMenu.Images.Add(Resources.colors);
        imagesMenu.Images.Add(Resources.debugging);
        imagesMenu.Images.Add(Resources.desktop);
        imagesMenu.Images.Add(Resources.environment);
        imagesMenu.Images.Add(Resources.general);
        imagesMenu.Images.Add(Resources.iniss_settings);
        imagesMenu.Images.Add(Resources.localization);
        imagesMenu.Images.Add(Resources.shortcut);
        imagesMenu.Images.Add(Resources.start);

        treeMenu.Nodes[0].ImageIndex = treeMenu.Nodes[0].SelectedImageIndex = 4;
        treeMenu.Nodes[1].ImageIndex = treeMenu.Nodes[1].SelectedImageIndex = 3;
        treeMenu.Nodes[1].Nodes[0].ImageIndex = treeMenu.Nodes[1].Nodes[0].SelectedImageIndex = 6;
        treeMenu.Nodes[1].Nodes[1].ImageIndex = treeMenu.Nodes[1].Nodes[1].SelectedImageIndex = 0;
        treeMenu.Nodes[1].Nodes[2].ImageIndex = treeMenu.Nodes[1].Nodes[2].SelectedImageIndex = 2;
        treeMenu.Nodes[1].Nodes[3].ImageIndex = treeMenu.Nodes[1].Nodes[3].SelectedImageIndex = 7;
        treeMenu.Nodes[2].ImageIndex = treeMenu.Nodes[2].SelectedImageIndex = 1;
        treeMenu.Nodes[3].ImageIndex = treeMenu.Nodes[3].SelectedImageIndex = 5;
        treeMenu.Nodes[4].ImageIndex = treeMenu.Nodes[4].SelectedImageIndex = 8;

        treeMenu.SelectedNode = selectTree != null ? treeMenu.Nodes.Find(selectTree, true)[0] : treeMenu.Nodes[0];
        treeMenu.ExpandAll();

        for (var i = 0; i < dgvAppFonts.Rows.Count; i++)
        {
            var dgvcs = new DataGridViewCellStyle {Font = appFonts[i].Font};
            dgvAppFonts.Rows[i].Cells[dgvAppFonts.Columns.Count - 1].Style = dgvcs;
        }
    }

    private void bAppFontDefault_Click(object sender, EventArgs e)
    {
        var fonts = GVDEditorConfig.GetDefaultAppFontsSettings();
        SettingsNaming.NameAppFontSetting(fonts);

        dgvAppFonts.DataSource = null;
        appFonts = new BindingList<AppFont>(fonts.GetValues());
        dgvAppFonts.DataSource = appFonts;
        for (var i = 0; i < dgvAppFonts.Rows.Count; i++)
        {
            var dgvcs = new DataGridViewCellStyle {Font = appFonts[i].Font};
            dgvAppFonts.Rows[i].Cells[dgvAppFonts.Columns.Count - 1].Style = dgvcs;
        }

        dgvAppFonts.Refresh();
    }

    private void HideAllPanels()
    {
        pObecne.Visible = false;
        pEnvironment.Visible = false;
        pLocalization.Visible = false;
        pFontsColors.Visible = false;
        pLogging.Visible = false;
        pPlaying.Visible = false;
        pDesktop.Visible = false;
        pShortcuts.Visible = false;
        pStartup.Visible = false;

        KeyPreview = false;
        lShortcutHelp.Visible = false;
        shortcutindex = -1;
    }

    private void dgvAppFonts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        var index = dgvAppFonts.CurrentCell.RowIndex;
        if (index != -1)
        {
            appFontDialog.Font = appFonts[index].Font;

            var result = appFontDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                appFonts[index].Font = appFontDialog.Font;
                SetAppFontExample(index, appFontDialog.Font);
            }
        }
    }

    private void bColumnUp_Click(object sender, EventArgs e)
    {
        if (dgvDesktopColums.CurrentRow != null)
        {
            var sel = dgvDesktopColums.SelectedRows[0].Index;
            var col = columns[sel];

            if (sel - 1 >= 0)
            {
                columns.RemoveAt(sel);
                columns.Insert(sel - 1, col);
                dgvDesktopColums.ClearSelection();
                dgvDesktopColums.Rows[sel - 1].Selected = true;
            }
        }
    }

    private void bColumnDown_Click(object sender, EventArgs e)
    {
        if (dgvDesktopColums.CurrentRow != null)
        {
            var sel = dgvDesktopColums.SelectedRows[0].Index;
            var col = columns[sel];

            if (sel + 1 < columns.Count)
            {
                columns.RemoveAt(sel);
                columns.Insert(sel + 1, col);
                dgvDesktopColums.ClearSelection();
                dgvDesktopColums.Rows[sel + 1].Selected = true;
            }
        }
    }

    private void dgvDesktopColums_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= columns.Count) return;

        if (dgvDesktopColums.Columns["MinWidth"] != null &&
            dgvDesktopColums.Columns["MinWidth"].Index == e.ColumnIndex)
            if (int.TryParse(e.FormattedValue.ToString(), out var num) && num <= 2)
                e.Cancel = true;
    }

    private void dgvDesktopColums_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        Utils.ShowError(Resources.FAppSettings_Bolo_zadané_neplatné_číslo);
    }

    private void bAllShortcutsSetDefault_Click(object sender, EventArgs e)
    {
        var shortcuts = GVDEditorConfig.GetDefaultShortcutsSettings();
        GVDEditorSettingsNaming.NameShortcutCommands(shortcuts);
        dgvShortcuts.DataSource = null;
        this.shortcuts = new BindingList<CommandShortcut>(shortcuts.GetValues());
        dgvShortcuts.DataSource = this.shortcuts;
    }

    private void dgvShortcuts_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == dgvShortcuts.Columns["AssignShortcut"]?.Index)
            DoAssignShortcut(e.RowIndex);
        else if (e.ColumnIndex == dgvShortcuts.Columns["RemoveShortcut"]?.Index)
            DoRemoveShortcut(e.RowIndex);
        else if (e.ColumnIndex == dgvShortcuts.Columns["Default"]?.Index) SetDefaultShortcut(e.RowIndex);
    }

    private void DoAssignShortcut(int index)
    {
        KeyPreview = true;
        lShortcutHelp.Visible = true;
        shortcutindex = index;
        lShortcutHelp.Text = lShortcutHelp1;
    }

    private void DoRemoveShortcut(int index)
    {
        shortcuts[index].Shortcut = new ShortcutName(Shortcut.None);
        shortcuts.ResetBindings();
        FindAndCheckDuplicateShortcuts();
    }

    private void SetDefaultShortcut(int index)
    {
        var shortcuts = GVDEditorConfig.GetDefaultShortcutsSettings();
        GVDEditorSettingsNaming.NameShortcutCommands(shortcuts);
        this.shortcuts[index].Shortcut = shortcuts.GetValues()[index].Shortcut;
        this.shortcuts.ResetBindings();
        FindAndCheckDuplicateShortcuts();
    }

    private void FAppSettings_KeyDown(object sender, KeyEventArgs e)
    {
        var keys = e.KeyData;

        if (Utils.ValidateShortcut(keys))
        {
            var sc = (Shortcut) keys;
            shortcuts[shortcutindex].Shortcut = new ShortcutName(sc);
            shortcuts.ResetBindings();

            FindAndCheckDuplicateShortcuts();

            KeyPreview = false;
            lShortcutHelp.Visible = false;
            shortcutindex = -1;
            lShortcutHelp.Text = lShortcutHelp1;
        }
        else
        {
            lShortcutHelp.Text = lShortcutHelp2;
        }

        e.Handled = true;
    }

    private void SetAppFontExample(int index, Font font)
    {
        dgvAppFonts.Rows[index].Cells[dgvAppFonts.Columns.Count - 1].Style.Font = font;
    }

    private void ResetColorShortcutDuplicates()
    {
        for (var i = 0; i < shortcuts.Count; i++) dgvShortcuts.Rows[i].Cells[1].Style.ForeColor = dgvShortcuts.ForeColor;
    }

    private void FAppSettings_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        Process.Start(LinkConsts.LINK_APP_SETTINGS);
    }

    private void cbClassicGUI_CheckedChanged(object sender, EventArgs e)
    {
        if (!initialization)
            showInfoRestart = true;
    }

    private void bUseStyle_Click(object sender, EventArgs e)
    {
        int index = cbStyles.SelectedIndex;
        if (index == -1)
            return;

        if (usedStyleId != index)
        {
            usedStyleId = index;
            cbStyles.Invalidate();

            if (!initialization)
                showInfoRestart = true;

            Utils.ShowInfo(Resources.FAppSettings_Zmeny_sa_prejavia_po_reštarte_programu);
        }
    }

    private void bAddStyle_Click(object sender, EventArgs e)
    {
        var frename = new FRenameStyle(styles.ToList(), styles.Count - 1, "");
        var result = frename.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            var style = new GVDEditorStyle {Name = frename.NewName};
            styles.Add(style);
            cbStyles.SelectedIndex = styles.Count - 1;
        }
    }

    private void bRenameStyle_Click(object sender, EventArgs e)
    {
        int index = cbStyles.SelectedIndex;
        if (index == -1)
            return;

        var frename = new FRenameStyle(styles.ToList(), index, styles[index].Name);
        var result = frename.ShowDialog(this);
        if (result == DialogResult.OK)
        {
            styles[index].Name = frename.NewName;
            styles.ResetBindings();
        }
    }

    private void bRemoveStyle_Click(object sender, EventArgs e)
    {
        int index = cbStyles.SelectedIndex;
        if (index == -1)
            return;

        styles.RemoveAt(index);
        cbStyles.SelectedIndex = 0;
    }

    private void cbStyles_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = cbStyles.SelectedIndex;
        if (index == -1)
            return;

        if (styles[index].Name is StyleNames.LIGHT or StyleNames.DARK)
        {
            bRemoveStyle.Enabled = false;
            bRenameStyle.Enabled = false;
        }
        else
        {
            bRemoveStyle.Enabled = true;
            bRenameStyle.Enabled = true;
        }

        if (actualstyle != -1)
        {
            SaveAppColorSettings(styles[actualstyle]);
            styles[actualstyle].DarkScrollBar = actualDarkScrollBar;
            styles[actualstyle].DarkTitleBar = actualDarkTitleBar;
            styles[actualstyle].ControlsDefaultStyle = actualDefaultConStyle;
        }

        InitializeAppColorSettings(styles[index]);
        cboxDarkScrollBars.Checked = styles[index].DarkScrollBar;
        cboxDarkTitleBar.Checked = styles[index].DarkTitleBar;
        cboxDefaultStyle.Checked = styles[index].ControlsDefaultStyle;
        actualstyle = index;
    }

    private void cboxDefaultStyle_CheckedChanged(object sender, EventArgs e)
    {
        actualDefaultConStyle = cboxDefaultStyle.Checked;
    }

    private void cboxDarkScrollBars_CheckedChanged(object sender, EventArgs e)
    {
        actualDarkScrollBar = cboxDarkScrollBars.Checked;
    }

    private void cboxDarkTitleBar_CheckedChanged(object sender, EventArgs e)
    {
        actualDarkTitleBar = cboxDarkTitleBar.Checked;
    }

    private void cbStyles_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1) return;
        e.DrawBackground();
        TextRenderer.DrawText(
            e.Graphics, styles[e.Index].Name, e.Index == usedStyleId ? new Font(e.Font, FontStyle.Bold) : e.Font, e.Bounds.Location, e.ForeColor);
        e.DrawFocusRectangle();
    }

    private void FindAndCheckDuplicateShortcuts()
    {
        ResetColorShortcutDuplicates();
        for (var i = 0; i < shortcuts.Count; i++)
        {
            var s1 = shortcuts[i].Shortcut.ThisShortcut;
            for (var j = 0; j < shortcuts.Count; j++)
            {
                var s2 = shortcuts[j].Shortcut.ThisShortcut;
                if (i != j && s1 == s2 && (s1 != Shortcut.None || s2 != Shortcut.None))
                {
                    dgvShortcuts.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                    dgvShortcuts.Rows[j].Cells[1].Style.ForeColor = Color.Red;
                }
            }
        }
    }

    private void cboxManualCmdArgs_CheckedChanged(object sender, EventArgs e)
    {
        if (cboxManualCmdArgs.Checked)
        {
            groupArguments.Enabled = false;
            tbCmdArguments.ReadOnly = false;
        }
        else
        {
            tbCmdArguments.ReadOnly = true;
            groupArguments.Enabled = true;
        }
    }

    private void cboxArgMoreInstances_CheckedChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cboxArgMinimize_CheckedChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cboxArgExportHlasTexts_CheckedChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cboxArgAsClient_CheckedChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cboxArgExportTableTexts_CheckedChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cbArgRegister_TextUpdate(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void cbArgRegister_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArgsUpdate();
    }

    private void ArgsUpdate()
    {
        if (argsinit)
        {
            return;
        }

        tbCmdArguments.Text = "";
        var str = new StringBuilder();

        if (cboxArgMoreInstances.Checked)
        {
            str.Append("/multiuse ");
        }

        if (cboxArgMinimize.Checked)
        {
            str.Append("/Minimize ");
        }

        if (cboxArgExportHlasTexts.Checked)
        {
            str.Append("/ExportHlas ");
        }

        if (cboxArgAsClient.Checked)
        {
            str.Append("/remote ");
        }

        if (cboxArgExportTableTexts.Checked)
        {
            str.Append("/Export ");
        }

        if (!string.IsNullOrEmpty(cbArgRegister.Text))
        {
            str.Append("/reg:\"" + cbArgRegister.Text + "\" ");
        }
        else if (cbArgRegister.SelectedIndex != -1 && !string.IsNullOrEmpty(cbArgRegister.SelectedText))
        {
            str.Append("/reg:\"" + cbArgRegister.SelectedText + "\" ");
        }

        tbCmdArguments.Text = str.ToString();
    }

    private void FormatArgs(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        argsinit = true;

        if (text.Contains("/multiuse"))
        {
            cboxArgMoreInstances.Checked = true;
        }

        if (text.Contains("/Minimize"))
        {
            cboxArgMinimize.Checked = true;
        }

        if (text.Contains("/ExportHlas"))
        {
            cboxArgExportHlasTexts.Checked = true;
        }

        if (text.Contains("/remote"))
        {
            cboxArgAsClient.Checked = true;
        }

        if (text.Contains("/Export"))
        {
            cboxArgExportTableTexts.Checked = true;
        }

        if (text.Contains("/reg:"))
        {
            int start = text.IndexOf("/reg:\"", StringComparison.Ordinal) + 6;
            if (start == -1)
            {
                start = text.IndexOf("/reg:", StringComparison.Ordinal) + 5;
            }
            int end = text.IndexOf("\"", start, StringComparison.Ordinal);
            if (end == -1)
            {
                end = text.IndexOf(" ", start, StringComparison.Ordinal);
            }

            end -= start;
            string t = text.Substring(start, end);

            t = t.Replace("\"", "");
            cbArgRegister.Text = t.Trim();
        }

        argsinit = false;
    }
}