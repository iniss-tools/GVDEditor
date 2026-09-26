using GVDEditor.Tools;
using ExControls;
using GVDEditor.XML;
using ToolsCore;
using ToolsCore.Forms;
using ToolsCore.Tools;
using ToolsCore.XML;

namespace GVDEditor.Forms;

public partial class FAppSettings : FAppSettingsBase
{
    private new GVDEditorConfig Config => (GVDEditorConfig)base.Config;
    private new Styles<GVDEditorStyle> Styles => (Styles<GVDEditorStyle>)base.Styles;

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
        nudPlayerWordPause.Value = Config.PlayerSoundsOffset;
        cboxRunAsAdmin.Checked = Config.StartupINISSConfig.RunAsAdmin;

        // zdroj registrov najprv - jeho vyber by inak prepisal nacitane argumenty
        _argsinit = true;
        cbArgRegister.DataSource = Tools.AppRegistry.GetINISSRegisters();
        cbArgRegister.SelectedIndex = -1;
        _argsinit = false;

        tbCmdArguments.Text = Config.StartupINISSConfig.CmdArgs;
        FormatArgs(Config.StartupINISSConfig.CmdArgs);
    }

    /// <inheritdoc />
    protected override bool OnSaving()
    {
        Config.Shortcuts.SetValues(Shortcuts);
        Config.DesktopCols.SetValues(Columns);

        Config.DateLimitLocate = (AppLanguage)cbDateLimitLanguage.SelectedValue!;

        Config.AutoVariant = cboxAutoVariant.Checked;
        Config.AutoTableText = cboxTabTextAutoGenerate.Checked;
        Config.DisableVariantCheck = cboxDontCheckTrainIndex.Checked;
        Config.PlayerSoundsOffset = decimal.ToInt32(nudPlayerWordPause.Value);
        Config.StartupINISSConfig = new StartupINISS
        {
            RunAsAdmin = cboxRunAsAdmin.Checked,
            CmdArgs = tbCmdArguments.Text.Trim()
        };
        return base.OnSaving();
    }

    /// <inheritdoc />
    protected override void SaveData()
    {
        GlobData.Config = Config;
        GlobData.UsingStyle = (GVDEditorStyle)UsingStyle;
        GlobSettings.UsingStyle = UsingStyle;
        GlobData.Styles = Styles;
        
        var configsDir = ToolsCore.AppPaths.ConfigDir;
        if (!Directory.Exists(configsDir))
            Directory.CreateDirectory(configsDir);

        Styles<GVDEditorStyle>.WriteData(Utils.CombinePath(configsDir, ToolsCore.FileConsts.FILE_STYLES)!, GlobData.Styles);
        XmlSerialization.WriteData(Utils.CombinePath(configsDir, ToolsCore.FileConsts.FILE_CONFIG)!, GlobData.Config);
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
        // v rucnom rezime su argumenty v textovom poli - zaskrtavacie polia ich nemenia
        if (_argsinit || cboxManualCmdArgs.Checked)
            return;

        var args = new List<string>();
        if (cboxArgMoreInstances.Checked) args.Add("/Multiuse");
        if (cboxArgMinimize.Checked) args.Add("/Minimize");
        if (cboxArgExportHlasTexts.Checked) args.Add("/ExportHlas");
        if (cboxArgAsClient.Checked) args.Add("/Remote");
        if (cboxArgExportTableTexts.Checked) args.Add("/Export");
        if (!string.IsNullOrWhiteSpace(cbArgRegister.Text))
            args.Add("/Reg:\"" + cbArgRegister.Text.Trim() + "\"");

        // parametre bez zaskrtavacieho pola (napr. /NoRestore, /1) zostanu zachovane
        string[] managed = ["Multiuse", "Minimize", "ExportHlas", "Remote", "Export"];
        foreach (var token in INISSArgs.Tokens(tbCmdArguments.Text))
        {
            var isManaged = managed.Any(m => INISSArgs.Has(token, m)) || INISSArgs.Registry(token) != null;
            if (!isManaged)
                args.Add(token.Contains(' ') ? "\"" + token + "\"" : token);
        }

        tbCmdArguments.Text = string.Join(" ", args);
    }

    private void FormatArgs(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        _argsinit = true;

        // parametre po jednom - /Export nesmie zaskrtnut aj /ExportHlas a na velkosti pismen INISSu nezalezi
        cboxArgMoreInstances.Checked = INISSArgs.Has(text, "Multiuse");
        cboxArgMinimize.Checked = INISSArgs.Has(text, "Minimize");
        cboxArgExportHlasTexts.Checked = INISSArgs.Has(text, "ExportHlas");
        cboxArgAsClient.Checked = INISSArgs.Has(text, "Remote");
        cboxArgExportTableTexts.Checked = INISSArgs.Has(text, "Export");
        cbArgRegister.Text = INISSArgs.Registry(text) ?? "";

        _argsinit = false;
    }
}