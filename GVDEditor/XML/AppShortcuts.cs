using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using ToolsCore.XML;

namespace GVDEditor.XML
{
    /// <summary>
    ///     Obsahuje zoznam všetkých možných klávesových skratiek pre program.
    /// </summary>
    public class AppShortcuts
    {
        /// <summary>
        ///     Skratka pre otvorenie dialógu Importovanie dát.
        /// </summary>
        [XmlElement("ImportData")] 
        public CommandShortcut ImportData = new(new ShortcutName(Shortcut.None));

        /// <summary>
        ///     Skratka pre uloženie grafikonu.
        /// </summary>
        [XmlElement("Save")] 
        public CommandShortcut Save = new(new ShortcutName(Shortcut.CtrlS));

        /// <summary>
        ///     Skratka pre analyzu grafikonu.
        /// </summary>
        [XmlElement("Analyze")] 
        public CommandShortcut Analyze = new(new ShortcutName(Shortcut.CtrlShiftA));

        /// <summary>
        ///     Skratka pre import grafikonu.
        /// </summary>
        [XmlElement("AddTrain")]
        public CommandShortcut AddTrain = new(new ShortcutName(Shortcut.Ins));

        /// <summary>
        ///     Skratka pre duplikovanie vlaku.
        /// </summary>
        [XmlElement("DuplicateTrain")]
        public CommandShortcut DuplicateTrain = new(new ShortcutName(Shortcut.CtrlD));

        /// <summary>
        ///     Skratka pre uloženie grafikonu.
        /// </summary>
        [XmlElement("LSettings")]
        public CommandShortcut LocalSettings = new(new ShortcutName(Shortcut.CtrlL));

        /// <summary>
        ///     Skratka pre pridanie vlaku.
        /// </summary>
        [XmlElement("GSettings")] 
        public CommandShortcut GlobalSettings = new(new ShortcutName(Shortcut.CtrlG));

        /// <summary>
        ///     Skratka pre úpravu vlaku.
        /// </summary>
        [XmlElement("AppSettings")]
        public CommandShortcut AppSettings = new(new ShortcutName(Shortcut.CtrlP));

        /// <summary>
        ///     Skratka pre úpravu vlaku.
        /// </summary>
        [XmlElement("EditTrain")] 
        public CommandShortcut EditTrain = new(new ShortcutName(Shortcut.ShiftIns));

        /// <summary>
        ///     Skratka pre vymazanie vlaku.
        /// </summary>
        [XmlElement("DeleteTrains")] 
        public CommandShortcut DeleteTrains = new(new ShortcutName(Shortcut.Del));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
        /// </summary>
        [XmlElement("GSMeskania")] 
        public CommandShortcut GSMeskania = new(new ShortcutName(Shortcut.Ctrl2));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
        /// </summary>
        [XmlElement("GSTrainTypes")]
        public CommandShortcut GSTrainTypes = new(new ShortcutName(Shortcut.Ctrl3));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
        /// </summary>
        [XmlElement("GSAudio")] 
        public CommandShortcut GSAudio = new(new ShortcutName(Shortcut.Ctrl4));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTabTab")] 
        public CommandShortcut LSTabTab = new(new ShortcutName(Shortcut.CtrlShiftT));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTTexts")]
        public CommandShortcut LSTTexts = new(new ShortcutName(Shortcut.CtrlShiftE));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTFonts")] 
        public CommandShortcut LSTFonts = new(new ShortcutName(Shortcut.CtrlShiftP));

        /// <summary>
        ///     Skratka pre otvorenie editoru TabTab.
        /// </summary>
        [XmlElement("LSTabTabEditor")] 
        public CommandShortcut LSTabTabEditor = new(new ShortcutName(Shortcut.CtrlT));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
        /// </summary>
        [XmlElement("GSGrafikony")] 
        public CommandShortcut GSGrafikony = new(new ShortcutName(Shortcut.Ctrl0));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Globalnych nastaveni.
        /// </summary>
        [XmlElement("GSLangs")] 
        public CommandShortcut GSLanguages = new(new ShortcutName(Shortcut.Ctrl1));

        /// <summary>
        ///     Skratka pre otvorenie dialógu Nový grafikon.
        /// </summary>
        [XmlElement("NewGVD")]
        public CommandShortcut NewGVD = new(new ShortcutName(Shortcut.None));

        /// <summary>
        ///     Skratka pre otvorenie grafikonu.
        /// </summary>
        [XmlElement("OpenGVD")] 
        public CommandShortcut OpenGVD = new(new ShortcutName(Shortcut.CtrlO));

        /// <summary>
        ///     Skratka pre import grafikonu.
        /// </summary>
        [XmlElement("ImportGVD")]
        public CommandShortcut ImportGVD = new(new ShortcutName(Shortcut.CtrlI));

        /// <summary>
        ///     Skratka pre vymazanie vlaku.
        /// </summary>
        [XmlElement("InfoApp")] 
        public CommandShortcut InfoApp = new(new ShortcutName(Shortcut.F6));

        /// <summary>
        ///     Skratka pre duplikovanie vlaku.
        /// </summary>
        [XmlElement("UpdateNotes")] 
        public CommandShortcut UpdateNotes = new(new ShortcutName(Shortcut.None));

        /// <summary>
        ///     Skratka pre spustenie programu INISS.
        /// </summary>
        [XmlElement("RunINISS")] 
        public CommandShortcut RunINISS = new(new ShortcutName(Shortcut.F5));

        /// <summary>
        ///     Skratka pre ukoncenie programu INISS.
        /// </summary>
        [XmlElement("ShutdownINISS")]
        public CommandShortcut ShutdownINISS = new(new ShortcutName(Shortcut.ShiftF5));

        /// <summary>
        ///     Skratka pre nutene ukoncenie programu INISS.
        /// </summary>
        [XmlElement("KillINISS")] 
        public CommandShortcut KillINISS = new(new ShortcutName(Shortcut.F10));

        /// <summary>
        ///     Skratka pre restartovanie programu INISS.
        /// </summary>
        [XmlElement("RestartINISS")] 
        public CommandShortcut RestartINISS = new(new ShortcutName(Shortcut.CtrlShiftF5));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSGrafikon")] 
        public CommandShortcut LSGrafikon = new(new ShortcutName(Shortcut.CtrlShiftG));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSStanice")] 
        public CommandShortcut LSStanice = new(new ShortcutName(Shortcut.CtrlShiftS));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSDopravcovia")] 
        public CommandShortcut LSDopravcovia = new(new ShortcutName(Shortcut.CtrlShiftO));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSPlatforms")] 
        public CommandShortcut LSPlatforms = new(new ShortcutName(Shortcut.CtrlShiftN));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSKolaje")] 
        public CommandShortcut LSKolaje = new(new ShortcutName(Shortcut.CtrlShiftK));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTPhysicals")] 
        public CommandShortcut LSTPhysicals = new(new ShortcutName(Shortcut.CtrlShiftF));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTLogicals")] 
        public CommandShortcut LSTLogicals = new(new ShortcutName(Shortcut.CtrlShiftL));

        /// <summary>
        ///     Skratka pre otvorenie polozky z Lokalnych nastaveni.
        /// </summary>
        [XmlElement("LSTCatalogs")]
        public CommandShortcut LSTCatalogs = new(new ShortcutName(Shortcut.CtrlShiftC));

        /// <summary>
        ///     Skratka pre okno s generovaniim datumoveho obmedzenia.
        /// </summary>
        [XmlElement("DatObm")] 
        public CommandShortcut DatObm = new(new ShortcutName(Shortcut.F7));

        /// <summary>
        ///     Vráti zoznam všetkých možných klávesových skratiek pre program.
        /// </summary>
        /// <returns></returns>
        public List<CommandShortcut> GetValues()
        {
            return new List<CommandShortcut>
            {
                NewGVD, OpenGVD, ImportData, ImportGVD, Save, Analyze,
                AddTrain, EditTrain, DeleteTrains, DuplicateTrain,
                LocalSettings, GlobalSettings, AppSettings,
                InfoApp, UpdateNotes, 
                RunINISS, ShutdownINISS, KillINISS, RestartINISS,
                LSGrafikon, LSStanice, LSDopravcovia, LSPlatforms, LSKolaje, LSTPhysicals, LSTLogicals, LSTCatalogs, LSTabTab, LSTTexts, LSTFonts,
                LSTabTabEditor,
                GSGrafikony, GSLanguages, GSMeskania, GSTrainTypes, GSAudio,
                DatObm
            };
        }
    }
}