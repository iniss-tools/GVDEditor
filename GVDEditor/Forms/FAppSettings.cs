using ExControls;
using GVDEditor.Properties;
using GVDEditor.Tools;
using GVDEditor.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToolsCore;
using ToolsCore.Tools;
using ToolsCore.XML;
using ColorSetting = ToolsCore.XML.ColorSetting;
using CommandShortcut = ToolsCore.XML.CommandShortcut;
using FormUtils = ToolsCore.Tools.FormUtils;
using ShortcutName = ToolsCore.XML.ShortcutName;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Dialog - Nastavenia programu
    /// </summary>
    internal partial class FAppSettings : Form
    {
        public const string GENERAL = "uObecne";
        public const string ENVIRONMENT = "uProstredie";
        public const string LOCALIZATION = "uLocalization";
        public const string FONTS_COLORS = "uFontsColors"; 
        public const string LOGGING = "uLogging";
        public const string PLAYING = "uPlaying";
        public const string DESKTOP = "uDesktop";
        public const string SHORTCUTS = "uShortcuts";
        public const string STARTUP = "uStartup";

        private readonly BindingList<DesktopColumn> Columns;

        private readonly List<int> fontSizes = new();
        private readonly bool initialization;
        private bool argsinit;

        [Localizable(true)] private readonly string lShortcutHelp1 = Resources.FAppSettings_lShortcutHelp1;

        [Localizable(true)] private readonly string lShortcutHelp2 = Resources.FAppSettings_lShortcutHelp2;

        private readonly List<string> SystemFonts = Utils.GetSystemFontNames();

        private BindingList<AppFont> AppFonts;

        private Color actualColorBackground, actualColorForeground;
        private Font actualColorSettingsFont;
        private bool actualFontBold;
        private float actualFontSize;
        private bool actualDarkScrollBar, actualDarkTitleBar, actualDefaultConStyle;

        private readonly BindingList<GVDEditorStyle> Styles;
        private int usedStyleID, actualstyle = -1;
        private BindingList<ColorCategory> ColorCategories;

        private ColorSetting previousColorSetting;
        private ColorCategory previousSelectedColorCategory;

        private int shortcutindex = -1;
        private BindingList<CommandShortcut> Shortcuts;

        private bool showInfoRestart;

        private readonly string selectTree;

        /// <inheritdoc />
        public FAppSettings(string opentree = null)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);

            treeMenu.SetTheme(WindowsTheme.Explorer);
            this.ApplyTheme();

            bColumnUp.Text += $@" {Utils.Arrow(Arrow.UP)}";
            bColumnDown.Text += $@" {Utils.Arrow(Arrow.DOWN)}";

            var config = GlobData.Config;

            initialization = true;

            cbDebugModeGUI.SelectedIndex = config.DebugModeGUI switch
            {
                Config.DebugMode.ONLY_MESSAGE => 0,
                Config.DebugMode.DETAILED_INFO => 1,
                Config.DebugMode.APP_CRASH => 2,
                _ => 0
            };

            cbAppLanguage.SelectedIndex = config.Language switch
            {
                Config.AppLanguage.SK => 0,
                Config.AppLanguage.CZ => 1,
                _ => 0
            };

            initialization = false;

            cbDateRemLocate.SelectedIndex = config.DateRemLocate switch
            {
                Config.AppLanguage.SK => 0,
                Config.AppLanguage.CZ => 1,
                _ => 0
            };

            switch (config.DesktopMenuMode)
            {
                case Config.DesktopMenu.TS_MS:
                    rbMSandTS.Checked = true;
                    break;
                case Config.DesktopMenu.TS_ONLY:
                    rbTS.Checked = true;
                    break;
                case Config.DesktopMenu.MS_ONLY:
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

            cbLoggingInfo.Checked = config.LoggingInfo;
            cbLoggingError.Checked = config.LoggingError;

            nudWordPause.Value = config.PlayerWordPause;

            cbArgRegister.DataSource = Tools.AppRegistry.GetINISSRegisters();
            cbStartINISSAdmin.Checked = config.StartupINISSConfig.RunAsAdmin;
            tbCmdArguments.Text = config.StartupINISSConfig.CmdArgs;
            FormatArgs(config.StartupINISSConfig.CmdArgs);

            AppFonts = new BindingList<AppFont>(config.Fonts.GetValues());
            dgvAppFonts.DataSource = AppFonts;

            Columns = new BindingList<DesktopColumn>(config.DesktopCols.GetOrderedValues());
            dgvDesktopColums.DataSource = Columns;

            Shortcuts = new BindingList<CommandShortcut>(config.Shortcuts.GetValues());
            dgvShortcuts.DataSource = Shortcuts;
            FindAndCheckDuplicateShortcuts();

            initialization = true;
            cbFont.DataSource = SystemFonts;

            for (var i = 6; i <= 24; i++) fontSizes.Add(i);

            cbFontSize.DataSource = fontSizes;

            Styles = new BindingList<GVDEditorStyle>(GlobData.Styles.StyleList);
            cbStyles.DataSource = Styles;
            usedStyleID = GlobData.Styles.UsingStyleID;
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

        private void InitializeAppColorSettings(GVDEditorStyle style)
        {
            if (style == null)
            {
                return;
            }

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
            var tesettingsControlsCP = new List<ColorSetting>();
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
            foreach (var t in tesettingsControls) tesettingsControlsCP.Add(ColorSetting.Copy(t));
            itemControls.Settings = tesettingsControlsCP;
            categories.Add(itemControls);

            ColorCategories = new BindingList<ColorCategory>(categories);
            cbColorSettings.DataSource = null;
            cbColorSettings.DataSource = ColorCategories;
        }

        private void SaveAppColorSettings(GVDEditorStyle style)
        {
            style.TabTabEditorScheme.Font = ColorCategories[0].Font;
            style.TrainTypeColumnScheme.Font = ColorCategories[1].Font;

            style.TabTabEditorScheme.Default = ColorCategories[0].Settings[0];
            style.TabTabEditorScheme.Function = ColorCategories[0].Settings[1];
            style.TabTabEditorScheme.Identifier = ColorCategories[0].Settings[2];
            style.TabTabEditorScheme.Number = ColorCategories[0].Settings[3];
            style.TabTabEditorScheme.String = ColorCategories[0].Settings[4];
            style.TabTabEditorScheme.Comment = ColorCategories[0].Settings[5];
            style.TabTabEditorScheme.Var = ColorCategories[0].Settings[6];
            style.TabTabEditorScheme.Event = ColorCategories[0].Settings[7];
            style.TabTabEditorScheme.OnNewLine = ColorCategories[0].Settings[8];
            style.TabTabEditorScheme.Operator = ColorCategories[0].Settings[9];
            style.TabTabEditorScheme.Constant = ColorCategories[0].Settings[10];
            style.TabTabEditorScheme.SelBraces = ColorCategories[0].Settings[11];
            style.TabTabEditorScheme.SelBraceBad = ColorCategories[0].Settings[12];

            style.TrainTypeColumnScheme.Os = ColorCategories[1].Settings[0];
            style.TrainTypeColumnScheme.R = ColorCategories[1].Settings[1];
            style.TrainTypeColumnScheme.X = ColorCategories[1].Settings[2];
            style.TrainTypeColumnScheme.Sl = ColorCategories[1].Settings[3];

            style.ControlsColorScheme.Panel = ColorCategories[2].Settings[0];
            style.ControlsColorScheme.Button = ColorCategories[2].Settings[1];
            style.ControlsColorScheme.Label = ColorCategories[2].Settings[2];
            style.ControlsColorScheme.Box = ColorCategories[2].Settings[3];
            style.ControlsColorScheme.Border = ColorCategories[2].Settings[4];
            style.ControlsColorScheme.Mark = ColorCategories[2].Settings[5];
            style.ControlsColorScheme.Highlight = ColorCategories[2].Settings[6];
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (Styles.Count != 0)
            {
                cbStyles.SelectedIndex = -1;
                cbStyles.SelectedIndex = 0;
            }

            var config = new GVDEditorConfig
            {
                DebugModeGUI = cbDebugModeGUI.SelectedIndex switch
                {
                    0 => Config.DebugMode.ONLY_MESSAGE,
                    1 => Config.DebugMode.DETAILED_INFO,
                    2 => Config.DebugMode.APP_CRASH,
                    _ => Config.DebugMode.ONLY_MESSAGE
                },
                Language = cbAppLanguage.SelectedIndex switch
                {
                    0 => Config.AppLanguage.SK,
                    1 => Config.AppLanguage.CZ,
                    _ => Config.AppLanguage.SK
                }
            };

            switch (cbDateRemLocate.SelectedIndex)
            {
                case 0:
                    config.DateRemLocate = Config.AppLanguage.SK;
                    DateLimit.Loc = DateLimit.Locale.SK;
                    break;
                case 1:
                    config.DateRemLocate = Config.AppLanguage.CZ;
                    DateLimit.Loc = DateLimit.Locale.CZ;
                    break;
                default:
                    config.DateRemLocate = Config.AppLanguage.SK;
                    DateLimit.Loc = DateLimit.Locale.SK;
                    break;
            }

            if (rbMSandTS.Checked)
                config.DesktopMenuMode = Config.DesktopMenu.TS_MS;
            else if (rbMS.Checked)
                config.DesktopMenuMode = Config.DesktopMenu.MS_ONLY;
            else if (rbTS.Checked) config.DesktopMenuMode = Config.DesktopMenu.TS_ONLY;

            config.MoreInstance = cbMoreInstance.Checked;
            config.ClassicGUI = cbClassicGUI.Checked;
            config.AutoTableText = cbAutoTableText.Checked;
            config.DisableVariantCheck = cbDisableVariantCheck.Checked;
            config.AutoVariant = cbAutoVariant.Checked;
            config.ShowRowsHeader = cbShowRowsHeader.Checked;
            config.ShowStateRow = cbShowStateRow.Checked;
            config.ShowDateTimeInStateRow = cboxDateTimeInStateRow.Checked;
            config.FitLastColumn = cbFitLastColumn.Checked;

            config.LoggingInfo = cbLoggingInfo.Checked;
            config.LoggingError = cbLoggingError.Checked;

            config.PlayerWordPause = decimal.ToInt32(nudWordPause.Value);

            config.StartupINISSConfig = new StartupINISS {RunAsAdmin = cbStartINISSAdmin.Checked, CmdArgs = tbCmdArguments.Text.Trim()};

            config.Fonts.Labels = AppFonts[0];
            config.Fonts.Buttons = AppFonts[1];
            config.Fonts.Menu = AppFonts[2];
            config.Fonts.ColsHeader = AppFonts[3];
            config.Fonts.TableCells = AppFonts[4];
            config.Fonts.StateRow = AppFonts[5];

            for (var i = 0; i < Columns.Count; i++) Columns[i].Order = i;

            config.DesktopCols.Number = Columns.First(x => x.Id == 0);
            config.DesktopCols.Type = Columns.First(x => x.Id == 1);
            config.DesktopCols.Name = Columns.First(x => x.Id == 2);
            config.DesktopCols.LinkaPrichod = Columns.First(x => x.Id == 3);
            config.DesktopCols.LinkaOdchod = Columns.First(x => x.Id == 4);
            config.DesktopCols.Routing = Columns.First(x => x.Id == 5);
            config.DesktopCols.Prichod = Columns.First(x => x.Id == 6);
            config.DesktopCols.Odchod = Columns.First(x => x.Id == 7);
            config.DesktopCols.VychodziaStanica = Columns.First(x => x.Id == 8);
            config.DesktopCols.KonecnaStanica = Columns.First(x => x.Id == 9);
            config.DesktopCols.DateLimit = Columns.First(x => x.Id == 10);
            config.DesktopCols.Track = Columns.First(x => x.Id == 11);
            config.DesktopCols.Operator = Columns.First(x => x.Id == 12);
            config.DesktopCols.OtherBtn = Columns.First(x => x.Id == 13);

            config.Shortcuts.NewGVD = Shortcuts[0];
            config.Shortcuts.OpenGVD = Shortcuts[1];
            config.Shortcuts.ImportData = Shortcuts[2];
            config.Shortcuts.ImportGVD = Shortcuts[3];
            config.Shortcuts.Save = Shortcuts[4];
            config.Shortcuts.Analyze = Shortcuts[5];

            config.Shortcuts.AddTrain = Shortcuts[6];
            config.Shortcuts.EditTrain = Shortcuts[7];
            config.Shortcuts.DeleteTrains = Shortcuts[8];
            config.Shortcuts.DuplicateTrain = Shortcuts[9];

            config.Shortcuts.LocalSettings = Shortcuts[10];
            config.Shortcuts.GlobalSettings = Shortcuts[11];
            config.Shortcuts.AppSettings = Shortcuts[12];

            config.Shortcuts.InfoApp = Shortcuts[13];
            config.Shortcuts.UpdateNotes = Shortcuts[14];
            config.Shortcuts.RunINISS = Shortcuts[15];
            config.Shortcuts.ShutdownINISS = Shortcuts[16];
            config.Shortcuts.KillINISS = Shortcuts[17];
            config.Shortcuts.RestartINISS = Shortcuts[18];

            config.Shortcuts.LSGrafikon = Shortcuts[19];
            config.Shortcuts.LSStanice = Shortcuts[20];
            config.Shortcuts.LSDopravcovia = Shortcuts[21];
            config.Shortcuts.LSPlatforms = Shortcuts[22];
            config.Shortcuts.LSKolaje = Shortcuts[23];
            config.Shortcuts.LSTPhysicals = Shortcuts[24];
            config.Shortcuts.LSTLogicals = Shortcuts[25];
            config.Shortcuts.LSTCatalogs = Shortcuts[26];
            config.Shortcuts.LSTabTab = Shortcuts[27];
            config.Shortcuts.LSTTexts = Shortcuts[28];
            config.Shortcuts.LSTFonts = Shortcuts[29];
            config.Shortcuts.LSTabTabEditor = Shortcuts[30];

            config.Shortcuts.GSGrafikony = Shortcuts[31];
            config.Shortcuts.GSLanguages = Shortcuts[32];
            config.Shortcuts.GSMeskania = Shortcuts[33];
            config.Shortcuts.GSTrainTypes = Shortcuts[34];
            config.Shortcuts.GSAudio = Shortcuts[35];

            GlobData.Config = config;
            GlobSettings.Fonts = config.Fonts;
            XMLSerialization.WriteData(Utils.CombinePath(Application.StartupPath, FileConsts.FILE_CONFIG), config);

            GlobData.Styles.StyleList = Styles.ToList();
            if (GlobData.Styles.UsingStyleID != usedStyleID)
            {
                GlobData.UsingStyle = Styles[usedStyleID];
                GlobSettings.UsingStyle = Styles[usedStyleID];
                GlobData.Styles.UsingStyleID = usedStyleID;
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
            if (cbColorSettings.SelectedIndex != -1)
            {
                if (previousSelectedColorCategory != null) previousSelectedColorCategory.Font = actualColorSettingsFont;

                var category = (ColorCategory) cbColorSettings.SelectedItem;

                actualColorSettingsFont = category.Font;

                listSettings.DataSource = null;
                listSettings.DataSource = category.Settings;

                if (!ColorCategories[cbColorSettings.SelectedIndex].DisableFontEdit)
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
        }

        private void bColorsUseDefault_Click(object sender, EventArgs e)
        {
            InitializeAppColorSettings(new GVDEditorStyle());
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

            var font = new Font(SystemFonts[e.Index], 10);
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

                if (!ColorCategories[cbColorSettings.SelectedIndex].DisableFontEdit)
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
                case GENERAL:
                    pObecne.Visible = true;
                    break;
                case ENVIRONMENT:
                    pEnvironment.Visible = true;
                    break;
                case LOCALIZATION:
                    pLocalization.Visible = true;
                    break;
                case FONTS_COLORS:
                    pFontsColors.Visible = true;
                    break;
                case LOGGING:
                    pLogging.Visible = true;
                    break;
                case PLAYING:
                    pPlaying.Visible = true;
                    break;
                case DESKTOP:
                    pDesktop.Visible = true;
                    break;
                case SHORTCUTS:
                    pShortcuts.Visible = true;
                    break;
                case STARTUP:
                    pStartup.Visible = true;
                    break;
                default:
                    pObecne.Visible = true;
                    break;
            }
        }

        private void FAppSettings_Load(object sender, EventArgs e)
        {
            treeMenu.SelectedNode = selectTree != null ? treeMenu.Nodes.Find(selectTree, true)[0] : treeMenu.Nodes[0];
            treeMenu.ExpandAll();

            for (var i = 0; i < dgvAppFonts.Rows.Count; i++)
            {
                var dgvcs = new DataGridViewCellStyle {Font = AppFonts[i].Font};
                dgvAppFonts.Rows[i].Cells[dgvAppFonts.Columns.Count - 1].Style = dgvcs;
            }
        }

        private void bAppFontDefault_Click(object sender, EventArgs e)
        {
            var fonts = GVDEditorConfig.SetAppFontsSettingsDefault();
            SettingsNaming.NameAppFontSetting(fonts);

            dgvAppFonts.DataSource = null;
            AppFonts = new BindingList<AppFont>(fonts.GetValues());
            dgvAppFonts.DataSource = AppFonts;
            for (var i = 0; i < dgvAppFonts.Rows.Count; i++)
            {
                var dgvcs = new DataGridViewCellStyle {Font = AppFonts[i].Font};
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
                appFontDialog.Font = AppFonts[index].Font;

                var result = appFontDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    AppFonts[index].Font = appFontDialog.Font;
                    SetAppFontExample(index, appFontDialog.Font);
                }
            }
        }

        private void bColumnUp_Click(object sender, EventArgs e)
        {
            if (dgvDesktopColums.CurrentRow != null)
            {
                var sel = dgvDesktopColums.SelectedRows[0].Index;
                var col = Columns[sel];

                if (sel - 1 >= 0)
                {
                    Columns.RemoveAt(sel);
                    Columns.Insert(sel - 1, col);
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
                var col = Columns[sel];

                if (sel + 1 < Columns.Count)
                {
                    Columns.RemoveAt(sel);
                    Columns.Insert(sel + 1, col);
                    dgvDesktopColums.ClearSelection();
                    dgvDesktopColums.Rows[sel + 1].Selected = true;
                }
            }
        }

        private void dgvDesktopColums_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Columns.Count) return;

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
            var shortcuts = GVDEditorConfig.SetShortcutsSettingsDefault();
            GVDEditorSettingsNaming.NameShortcutCommands(shortcuts);
            dgvShortcuts.DataSource = null;
            Shortcuts = new BindingList<CommandShortcut>(shortcuts.GetValues());
            dgvShortcuts.DataSource = Shortcuts;
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
            Shortcuts[index].Shortcut = new ShortcutName(Shortcut.None);
            Shortcuts.ResetBindings();
            FindAndCheckDuplicateShortcuts();
        }

        private void SetDefaultShortcut(int index)
        {
            var shortcuts = GVDEditorConfig.SetShortcutsSettingsDefault();
            GVDEditorSettingsNaming.NameShortcutCommands(shortcuts);
            Shortcuts[index].Shortcut = shortcuts.GetValues()[index].Shortcut;
            Shortcuts.ResetBindings();
            FindAndCheckDuplicateShortcuts();
        }

        private void FAppSettings_KeyDown(object sender, KeyEventArgs e)
        {
            var keys = e.KeyData;

            if (Utils.ValidateShortcut(keys))
            {
                var sc = (Shortcut) keys;
                Shortcuts[shortcutindex].Shortcut = new ShortcutName(sc);
                Shortcuts.ResetBindings();

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
            for (var i = 0; i < Shortcuts.Count; i++) dgvShortcuts.Rows[i].Cells[1].Style.ForeColor = dgvShortcuts.ForeColor;
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

            if (usedStyleID != index)
            {
                usedStyleID = index;
                cbStyles.Invalidate();

                if (!initialization)
                    showInfoRestart = true;

                Utils.ShowInfo(Resources.FAppSettings_Zmeny_sa_prejavia_po_reštarte_programu);
            }
        }

        private void bAddStyle_Click(object sender, EventArgs e)
        {
            var frename = new FRenameStyle(Styles.ToList(), Styles.Count - 1, "");
            var result = frename.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var style = new GVDEditorStyle {Name = frename.NewName};
                Styles.Add(style);
                cbStyles.SelectedIndex = Styles.Count - 1;
            }
        }

        private void bRenameStyle_Click(object sender, EventArgs e)
        {
            int index = cbStyles.SelectedIndex;
            if (index == -1)
                return;

            var frename = new FRenameStyle(Styles.ToList(), index, Styles[index].Name);
            var result = frename.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Styles[index].Name = frename.NewName;
                Styles.ResetBindings();
            }
        }

        private void bRemoveStyle_Click(object sender, EventArgs e)
        {
            int index = cbStyles.SelectedIndex;
            if (index == -1)
                return;

            Styles.RemoveAt(index);
            cbStyles.SelectedIndex = 0;
        }

        private void cbStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbStyles.SelectedIndex;
            if (index == -1)
                return;

            if (Styles[index].Name is Styles<GVDEditorStyle>.DEFAULT_STYLE_NAME or Styles<GVDEditorStyle>.DARK_STYLE_NAME)
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
                SaveAppColorSettings(Styles[actualstyle]);
                Styles[actualstyle].DarkScrollBar = actualDarkScrollBar;
                Styles[actualstyle].DarkTitleBar = actualDarkTitleBar;
                Styles[actualstyle].ControlsDefaultStyle = actualDefaultConStyle;
            }

            InitializeAppColorSettings(Styles[index]);
            cboxDarkScrollBars.Checked = Styles[index].DarkScrollBar;
            cboxDarkTitleBar.Checked = Styles[index].DarkTitleBar;
            cboxDefaultStyle.Checked = Styles[index].ControlsDefaultStyle;
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
                e.Graphics, Styles[e.Index].Name, e.Index == usedStyleID ? new Font(e.Font, FontStyle.Bold) : e.Font, e.Bounds.Location, e.ForeColor);
            e.DrawFocusRectangle();
        }

        private void FindAndCheckDuplicateShortcuts()
        {
            ResetColorShortcutDuplicates();
            for (var i = 0; i < Shortcuts.Count; i++)
            {
                var s1 = Shortcuts[i].Shortcut.ThisShortcut;
                for (var j = 0; j < Shortcuts.Count; j++)
                {
                    var s2 = Shortcuts[j].Shortcut.ThisShortcut;
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
}