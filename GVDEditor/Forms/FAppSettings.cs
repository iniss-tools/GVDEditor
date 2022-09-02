using ExControls;
using GVDEditor.XML;
using ToolsCore;
using ToolsCore.Forms;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor.Forms;

public partial class FAppSettings : FAppSettingsBase
{
    private new GVDEditorConfig Config => base.Config as GVDEditorConfig;
    private new Styles<GVDEditorStyle> Styles => base.Styles as Styles<GVDEditorStyle>;

    protected override IList<CmdShortcut> DefaultShortcuts => new AppShortcuts().GetValues();
    
    protected override IList<DesktopColumn> DefaultColumns => new DesktopColumns().GetValues();

    private bool _argsinit;

    public FAppSettings(GVDEditorConfig config, Styles<GVDEditorStyle> styles) 
        : base(config, new Styles<GVDEditorStyle>(styles), GlobData.UsingStyle, typeof(GVDEditorStyle))
    {
        InitializeComponent();
        
        Shortcuts = new ExBindingList<CmdShortcut>(Config.Shortcuts.GetValues());
        Columns = new ExBindingList<DesktopColumn>(Config.DesktopCols.GetValues());

        dgvShortcuts.DataSource = Shortcuts;
        dgvColumns.DataSource = Columns;

        cbDateLimitLanguage.BindEnum(new Dictionary<AppLanguage, string>
        {
            [AppLanguage.Slovak] = "Slovenčina",
            [AppLanguage.Czech] = "Čeština" 
        });
        cbDateLimitLanguage.SelectedValue = Config.DateLimitLocate;
    }

    /// <inheritdoc />
    protected override void OnLoad()
    {
        base.OnLoad();
        cboxAutoVariant.Checked = Config.AutoVariant;
        cboxTabTextAutoGenerate.Checked = Config.AutoTableText;
        cboxDontCheckTrainIndex.Checked = Config.DisableVariantCheck;
        cboxShowDateTimeInStateRow.Checked = Config.ShowDateTimeInStateRow;
        nudPlayerWordPause.Value = Config.PlayerWordPause;
        cboxRunAsAdmin.Checked = Config.StartupINISSConfig.RunAsAdmin;
        tbCmdArguments.Text = Config.StartupINISSConfig.CmdArgs;

        cbArgRegister.DataSource = Tools.AppRegistry.GetINISSRegisters();
        FormatArgs(Config.StartupINISSConfig.CmdArgs);
    }

    /// <inheritdoc />
    protected override bool OnSaving()
    {
        base.OnSaving();

        Config.Shortcuts.SetValues(Shortcuts);
        Config.DesktopCols.SetValues(Columns);

        Config.DateLimitLocate = (AppLanguage) cbDateLimitLanguage.SelectedValue;

        Config.AutoVariant = cboxAutoVariant.Checked;
        Config.AutoTableText = cboxTabTextAutoGenerate.Checked;
        Config.DisableVariantCheck = cboxDontCheckTrainIndex.Checked;
        Config.ShowDateTimeInStateRow = cboxShowDateTimeInStateRow.Checked;
        Config.PlayerWordPause = decimal.ToInt32(nudPlayerWordPause.Value);
        return true;
    }

    /// <inheritdoc />
    protected override void SaveData()
    {
        GlobData.Config = Config;
        GlobData.UsingStyle = UsingStyle as GVDEditorStyle;
        GlobSettings.UsingStyle = UsingStyle;
        GlobData.Styles = Styles;
        
        var configsDir = Utils.CombinePath(Application.StartupPath, ToolsCore.FileConsts.CONFIG_PATH);
        if (!Directory.Exists(configsDir))
            Directory.CreateDirectory(configsDir);

        Styles<GVDEditorStyle>.WriteData(Utils.CombinePath(configsDir, ToolsCore.FileConsts.FILE_STYLES), GlobData.Styles);
        XmlSerialization.WriteData(Utils.CombinePath(configsDir, ToolsCore.FileConsts.FILE_CONFIG), GlobData.Config);
    }

    /// <inheritdoc />
    protected override Style CreateStyleInstance(string name) => new GVDEditorStyle {Name = name};

    /// <inheritdoc />
    protected override Style OnResetStyle(bool darkMode) => Styles<GVDEditorStyle>.GetDefaultStyle(darkMode);

    /// <inheritdoc />
    protected override void FillTreeViewStyles(object selectedStyle, ExTreeView tv)
    {
        if (selectedStyle is not GVDEditorStyle style)
            return;
        
        base.FillTreeViewStyles(selectedStyle, tv);
        
        var tabtab = CreateParentNode(style.TabTabEditorScheme, nameof(style.TabTabEditorScheme));
        CreateNode(style.TabTabEditorScheme.Number, nameof(style.TabTabEditorScheme.Number), tabtab);
        CreateNode(style.TabTabEditorScheme.String, nameof(style.TabTabEditorScheme.String), tabtab);
        CreateNode(style.TabTabEditorScheme.Comment, nameof(style.TabTabEditorScheme.Comment), tabtab);
        CreateNode(style.TabTabEditorScheme.OnNewLine, nameof(style.TabTabEditorScheme.OnNewLine), tabtab);
        CreateNode(style.TabTabEditorScheme.Operator, nameof(style.TabTabEditorScheme.Operator), tabtab);
        CreateNode(style.TabTabEditorScheme.Constant, nameof(style.TabTabEditorScheme.Constant), tabtab);
        CreateNode(style.TabTabEditorScheme.Default, nameof(style.TabTabEditorScheme.Default), tabtab);
        CreateNode(style.TabTabEditorScheme.Var, nameof(style.TabTabEditorScheme.Var), tabtab);
        CreateNode(style.TabTabEditorScheme.Event, nameof(style.TabTabEditorScheme.Event), tabtab);
        CreateNode(style.TabTabEditorScheme.Function, nameof(style.TabTabEditorScheme.Function), tabtab);
        CreateNode(style.TabTabEditorScheme.Identifier, nameof(style.TabTabEditorScheme.Identifier), tabtab);
        CreateNode(style.TabTabEditorScheme.SelBraces, nameof(style.TabTabEditorScheme.SelBraces), tabtab);
        CreateNode(style.TabTabEditorScheme.SelBraceBad, nameof(style.TabTabEditorScheme.SelBraceBad), tabtab);
        tv.Nodes.Add(tabtab);

        var traintype = CreateParentNode(style.TrainTypeColumnScheme, nameof(style.TrainTypeColumnScheme));
        CreateNode(style.TrainTypeColumnScheme.Os, nameof(style.TrainTypeColumnScheme.Os), traintype);
        CreateNode(style.TrainTypeColumnScheme.R, nameof(style.TrainTypeColumnScheme.R), traintype);
        CreateNode(style.TrainTypeColumnScheme.Sl, nameof(style.TrainTypeColumnScheme.Sl), traintype);
        CreateNode(style.TrainTypeColumnScheme.X, nameof(style.TrainTypeColumnScheme.X), traintype);
        tv.Nodes.Add(traintype);
    }

    private void CboxManualCmdArgs_CheckedChanged(object sender, EventArgs e)
    {
        groupArguments.Enabled = !cboxManualCmdArgs.Checked;
        tbCmdArguments.ReadOnly = !cboxManualCmdArgs.Checked;
    }

    private void ArgsUpdate(object sender, EventArgs e)
    {
        if (_argsinit)
            return;

        var str = new StringBuilder();

        if (cboxArgMoreInstances.Checked) str.Append("/multiuse ");
        if (cboxArgMinimize.Checked) str.Append("/Minimize ");
        if (cboxArgExportHlasTexts.Checked) str.Append("/ExportHlas ");
        if (cboxArgAsClient.Checked) str.Append("/remote ");
        if (cboxArgExportTableTexts.Checked) str.Append("/Export ");

        if (!string.IsNullOrEmpty(cbArgRegister.Text))
            str.Append("/reg:\"" + cbArgRegister.Text + "\" ");
        else if (cbArgRegister.SelectedIndex != -1 && !string.IsNullOrEmpty(cbArgRegister.SelectedText))
            str.Append("/reg:\"" + cbArgRegister.SelectedText + "\" ");

        tbCmdArguments.Text = str.ToString();
    }

    private void FormatArgs(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        _argsinit = true;

        if (text.Contains("/multiuse")) cboxArgMoreInstances.Checked = true;
        if (text.Contains("/Minimize")) cboxArgMinimize.Checked = true;
        if (text.Contains("/ExportHlas")) cboxArgExportHlasTexts.Checked = true;
        if (text.Contains("/remote")) cboxArgAsClient.Checked = true;
        if (text.Contains("/Export")) cboxArgExportTableTexts.Checked = true;

        if (text.Contains("/reg:"))
        {
            var start = text.IndexOf("/reg:\"", StringComparison.Ordinal) + 6;
            if (start == -1) 
                start = text.IndexOf("/reg:", StringComparison.Ordinal) + 5;
            var end = text.IndexOf("\"", start, StringComparison.Ordinal);
            if (end == -1) 
                end = text.IndexOf(" ", start, StringComparison.Ordinal);

            end -= start;
            var t = text.Substring(start, end);

            t = t.Replace("\"", "");
            cbArgRegister.Text = t.Trim();
        }

        _argsinit = false;
    }
}