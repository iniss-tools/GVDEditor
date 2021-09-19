using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML
{
    /// <summary>
    ///     Konfiguracny subor
    /// </summary>
    [XmlRoot("CONFIG")]
    public class GVDEditorConfig : Config
    {
        /// <summary>
        ///     Povolit pouzivanie automatickej spravy variant vlaku
        /// </summary>
        [XmlElement("AutoVariant")] [DefaultValue(false)]
        public bool AutoVariant;

        /// <summary>
        ///     Povoli/zakaze kontrolovanie spravnosti zadanej varianty vlaku
        /// </summary>
        [XmlElement("DisableVariantChk")] [DefaultValue(false)]
        public bool DisableVariantCheck;

        /// <summary>
        ///     Automaticky generovat texty do tabul pri ukladani do suborov
        /// </summary>
        [XmlElement("AutoTableText")] [DefaultValue(false)]
        public bool AutoTableText;

        /*/// <summary>
        /// Povoli/zakaze upravu datovych obmedzeni variant podla hlavnej varianty
        /// </summary>
        [XmlElement("EditVariantsDaterem")] public bool EditVariantsDaterem;*/

        /// <summary>
        ///     Ci sa ma zobrazovat datum v stavovom riadku na pracovnej ploche programu
        /// </summary>
        [XmlElement("ShowDateTimeInStateRow")] [DefaultValue(true)]
        public bool ShowDateTimeInStateRow = true;

        /// <summary>
        ///     Nastavi cas medzi zvukmi pri prehravani
        /// </summary>
        [XmlElement("PlayerWordPause")] [DefaultValue(0)]
        public int PlayerWordPause;

        /// <summary>
        ///     Stlpce zobrazujuce sa v tabulke na pracovnej ploche programu
        /// </summary>
        [XmlElement("DesktopCols")] public DesktopColumns DesktopCols = new();


        /// <summary>
        ///     Klávesové skratky pre akcie na pracovnej ploche programu
        /// </summary>
        [XmlElement("Shortcuts")] public AppShortcuts Shortcuts = new();


        /// <summary>
        ///     konfiguracia spustania INISSu z tohto programu
        /// </summary>
        [XmlElement("StartupINISSConfig")] public StartupINISS StartupINISSConfig = new() { CmdArgs = "", RunAsAdmin = false };

        private static DesktopColumns SetDesktopColumnsSettingsDefault()
        {
            var cols = new DesktopColumns
            {
                Number = new DesktopColumn { Order = 0, Visible = true, MinWidth = 60 },
                Type = new DesktopColumn { Order = 1, Visible = true, MinWidth = 40 },
                Name = new DesktopColumn { Order = 2, Visible = true, MinWidth = 100 },
                LinkaPrichod = new DesktopColumn { Order = 3, Visible = false, MinWidth = 50 },
                LinkaOdchod = new DesktopColumn { Order = 4, Visible = false, MinWidth = 50 },
                Routing = new DesktopColumn { Order = 5, Visible = true, MinWidth = 100 },
                Prichod = new DesktopColumn { Order = 6, Visible = true, MinWidth = 60 },
                Odchod = new DesktopColumn { Order = 7, Visible = true, MinWidth = 60 },
                VychodziaStanica = new DesktopColumn { Order = 8, Visible = true, MinWidth = 120 },
                KonecnaStanica = new DesktopColumn { Order = 9, Visible = true, MinWidth = 120 },
                DateLimit = new DesktopColumn { Order = 10, Visible = true, MinWidth = 300 },
                Track = new DesktopColumn { Order = 11, Visible = true, MinWidth = 100 },
                Operator = new DesktopColumn { Order = 12, Visible = true, MinWidth = 50 },
                OtherBtn = new DesktopColumn { Order = 13, Visible = true, MinWidth = 50 }
            };
            return cols;
        }

        /// <summary>
        ///     Vrati predvolene nastavenia pre klavesove skratky pracovnej plochy programu
        /// </summary>
        /// <returns></returns>
        public static AppShortcuts SetShortcutsSettingsDefault()
        {
            var shortcuts = new AppShortcuts
            {
                NewGVD = new CommandShortcut(new ShortcutName(Shortcut.None)),
                OpenGVD = new CommandShortcut(new ShortcutName(Shortcut.CtrlO)),
                ImportGVD = new CommandShortcut(new ShortcutName(Shortcut.CtrlI)),
                ImportData = new CommandShortcut(new ShortcutName(Shortcut.None)),
                Save = new CommandShortcut(new ShortcutName(Shortcut.CtrlS)),
                AddTrain = new CommandShortcut(new ShortcutName(Shortcut.Ins)),
                EditTrain = new CommandShortcut(new ShortcutName(Shortcut.ShiftIns)),
                DeleteTrains = new CommandShortcut(new ShortcutName(Shortcut.Del)),
                DuplicateTrain = new CommandShortcut(new ShortcutName(Shortcut.CtrlD)),
                LocalSettings = new CommandShortcut(new ShortcutName(Shortcut.CtrlL)),
                GlobalSettings = new CommandShortcut(new ShortcutName(Shortcut.CtrlG)),
                AppSettings = new CommandShortcut(new ShortcutName(Shortcut.CtrlP)),
                InfoApp = new CommandShortcut(new ShortcutName(Shortcut.F6)),
                UpdateNotes = new CommandShortcut(new ShortcutName(Shortcut.None)),
                RunINISS = new CommandShortcut(new ShortcutName(Shortcut.F5)),
                KillINISS = new CommandShortcut(new ShortcutName(Shortcut.ShiftF5))
            };

            return shortcuts;
        }

        /// <summary>
        ///     Vrati predvolene nastavenia pre pisma komponentov v GUI
        /// </summary>
        /// <returns></returns>
        public static ControlFonts SetAppFontsSettingsDefault()
        {
            var fonts = new ControlFonts
            {
                Labels = new AppFont { Font = SystemFonts.DefaultFont },
                Buttons = new AppFont { Font = SystemFonts.DefaultFont },
                ColsHeader = new AppFont { Font = SystemFonts.MenuFont },
                TableCells = new AppFont { Font = SystemFonts.DefaultFont },
                Menu = new AppFont { Font = SystemFonts.MenuFont },
                StateRow = new AppFont { Font = new Font(SystemFonts.MenuFont.FontFamily, 10, FontStyle.Bold) }
            };

            return fonts;
        }
    }
}