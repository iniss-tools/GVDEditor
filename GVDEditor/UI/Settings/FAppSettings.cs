using GVDEditor.Config;
using ExControls;
using ToolsCore.Tools;
using ToolsCore.Forms;
using ToolsCore.Iniss.Tools;
using ToolsCore.XML;

namespace GVDEditor.UI.Settings;

internal partial class FAppSettings : FAppSettingsBase
{
    private new GVDEditorConfig Config => (GVDEditorConfig)base.Config;
    private new Styles<GVDEditorStyle> Styles => (Styles<GVDEditorStyle>)base.Styles;

    protected override IList<CmdShortcut> DefaultShortcuts => ShortcutMap.DefaultRows(GvdCommands.All);
    
    protected override IList<DesktopColumn> DefaultColumns => new DesktopColumns().GetValues();

    private readonly EditorContext _ctx;

    public FAppSettings(EditorContext context)
        : base(context.Config, new Styles<GVDEditorStyle>(context.Session.Styles), context.UsingStyle, typeof(GVDEditorStyle))
    {
        _ctx = context;
        InitializeComponent();
        
        Shortcuts = new ExBindingList<CmdShortcut>(Config.Shortcuts.ToRows(GvdCommands.All));
        Columns = new ExBindingList<DesktopColumn>(Config.DesktopCols.GetValues());

        dgvShortcuts.DataSource = Shortcuts;
        dgvColumns.DataSource = Columns;

        cbDateLimitLanguage.BindEnum(new Dictionary<AppLanguage, string>
        {
            [AppLanguage.Slovak] = "Slovenčina",
            [AppLanguage.Czech] = "Čeština" 
        });
        cbDateLimitLanguage.SelectedValue = Config.DateLimitLocate;

        // skupiny stranky Vseobecne siahaju po pravy okraj ako skupina zo zakladu okna - kotvy z navrhu pocitaju
        // s inou sirkou panela, preto sa sirka nastavuje podla skutocneho panela
        pConcreteGeneral.ClientSizeChanged += (_, _) => FitGeneralGroups();
    }

    private void FitGeneralGroups()
    {
        foreach (var box in new Control[] { exGroupBox2, exGroupBox4, exGroupBox5 })
        {
            box.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            box.Width = Math.Max(box.MinimumSize.Width, pConcreteGeneral.ClientSize.Width - box.Left);
        }
    }

    /// <inheritdoc />
    protected override void OnLoad()
    {
        base.OnLoad();
        cboxTabTextAutoGenerate.Checked = Config.AutoTableText;
        nudPlayerWordPause.Value = Config.PlayerSoundsOffset;
        cboxInissEvaluation.Checked = Config.InissSettingsShowEvaluation;
        FitGeneralGroups();
    }

    /// <inheritdoc />
    protected override bool OnSaving()
    {
        Config.Shortcuts.SetFromRows(Shortcuts);
        Config.DesktopCols.SetValues(Columns);

        Config.DateLimitLocate = (AppLanguage)cbDateLimitLanguage.SelectedValue!;

        Config.AutoTableText = cboxTabTextAutoGenerate.Checked;
        Config.PlayerSoundsOffset = decimal.ToInt32(nudPlayerWordPause.Value);
        Config.InissSettingsShowEvaluation = cboxInissEvaluation.Checked;
        return base.OnSaving();
    }

    /// <inheritdoc />
    protected override void SaveData()
    {
        _ctx.Session.Config = Config;
        _ctx.Session.UsingStyle = (GVDEditorStyle)UsingStyle;
        _ctx.Session.Styles = Styles;
        
        var configsDir = ToolsCore.AppPaths.ConfigDir;
        if (!Directory.Exists(configsDir))
            Directory.CreateDirectory(configsDir);

        Styles<GVDEditorStyle>.WriteData(PathUtils.CombinePath(configsDir, ToolsCore.FileConsts.FileStyles)!, _ctx.Session.Styles);
        XmlSerialization.WriteData(PathUtils.CombinePath(configsDir, ToolsCore.FileConsts.FileConfig)!, _ctx.Config);
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
}