using GVDEditor.Properties;
using ToolsCore.Commands;

namespace GVDEditor.UI;

/// <summary>
/// Prikazy hlavneho okna s predvolenymi klavesovymi skratkami. Identifikator je nazov prvku skratky v config.xml -
/// nesmie sa menit, inak by sa stratili skratky nastavene pouzivatelom.
/// </summary>
internal static class GvdCommands
{
    public static CommandInfo New => new("NewGVD", Resources.Cmd_New, Shortcut.None);
    public static CommandInfo Open => new("OpenGVD", Resources.Cmd_Open, Shortcut.CtrlO);
    public static CommandInfo ImportGvd => new("ImportGVD", Resources.Cmd_ImportGvd, Shortcut.CtrlI);
    public static CommandInfo ImportData => new("ImportData", Resources.Cmd_ImportData, Shortcut.None);
    public static CommandInfo ImportElis => new("ImportELIS", Resources.Cmd_ImportElis, Shortcut.None);
    public static CommandInfo Save => new("Save", Resources.Cmd_Save, Shortcut.CtrlS);
    public static CommandInfo Analyze => new("Analyze", Resources.Cmd_Analyze, Shortcut.CtrlShiftA);

    public static CommandInfo AddTrain => new("AddTrain", Resources.Cmd_AddTrain, Shortcut.Ins);
    public static CommandInfo EditTrain => new("EditTrain", Resources.Cmd_EditTrain, Shortcut.ShiftIns);
    public static CommandInfo DeleteTrains => new("DeleteTrains", Resources.Cmd_DeleteTrains, Shortcut.Del);
    public static CommandInfo DuplicateTrain => new("DuplicateTrain", Resources.Cmd_DuplicateTrain, Shortcut.CtrlD);

    public static CommandInfo LocalSettings => new("LSettings", Resources.Cmd_LocalSettings, Shortcut.CtrlL);
    public static CommandInfo GlobalSettings => new("GSettings", Resources.Cmd_GlobalSettings, Shortcut.CtrlG);
    public static CommandInfo AppSettings => new("AppSettings", Resources.Cmd_AppSettings, Shortcut.CtrlP);

    public static CommandInfo GSGvds => new("GSGrafikony", Resources.Cmd_GSGvds, Shortcut.Ctrl0);
    public static CommandInfo GSLanguages => new("GSLangs", Resources.Cmd_GSLanguages, Shortcut.Ctrl1);
    public static CommandInfo GSDelays => new("GSMeskania", Resources.Cmd_GSDelays, Shortcut.Ctrl2);
    public static CommandInfo GSTrainTypes => new("GSTrainTypes", Resources.Cmd_GSTrainTypes, Shortcut.Ctrl3);
    public static CommandInfo GSAudio => new("GSAudio", Resources.Cmd_GSAudio, Shortcut.Ctrl4);

    public static CommandInfo LSGvd => new("LSGrafikon", Resources.Cmd_LSGvd, Shortcut.CtrlShiftG);
    public static CommandInfo LSLanguages => new("LSJazyky", Resources.Cmd_LSLanguages, Shortcut.CtrlShiftJ);
    public static CommandInfo LSStations => new("LSStanice", Resources.Cmd_LSStations, Shortcut.CtrlShiftS);
    public static CommandInfo LSOperators => new("LSDopravcovia", Resources.Cmd_LSOperators, Shortcut.CtrlShiftO);
    public static CommandInfo LSPlatforms => new("LSPlatforms", Resources.Cmd_LSPlatforms, Shortcut.CtrlShiftN);
    public static CommandInfo LSTracks => new("LSKolaje", Resources.Cmd_LSTracks, Shortcut.CtrlShiftK);
    public static CommandInfo LSPhysicalTables => new("LSTPhysicals", Resources.Cmd_LSPhysicalTables, Shortcut.CtrlShiftF);
    public static CommandInfo LSLogicalTables => new("LSTLogicals", Resources.Cmd_LSLogicalTables, Shortcut.CtrlShiftL);
    public static CommandInfo LSCatalogTables => new("LSTCatalogs", Resources.Cmd_LSCatalogTables, Shortcut.CtrlShiftC);
    public static CommandInfo LSTabTab => new("LSTabTab", Resources.Cmd_LSTabTab, Shortcut.CtrlShiftT);
    public static CommandInfo LSTableTexts => new("LSTTexts", Resources.Cmd_LSTableTexts, Shortcut.CtrlShiftE);
    public static CommandInfo LSTableFonts => new("LSTFonts", Resources.Cmd_LSTableFonts, Shortcut.CtrlShiftP);
    public static CommandInfo LSTabTabEditor => new("LSTabTabEditor", Resources.Cmd_LSTabTabEditor, Shortcut.CtrlT);
    public static CommandInfo StateDgm => new("StateDgm", Resources.Cmd_StateDgm, Shortcut.None);

    public static CommandInfo RunIniss => new("RunINISS", Resources.Cmd_RunIniss, Shortcut.F5);
    public static CommandInfo ShutdownIniss => new("ShutdownINISS", Resources.Cmd_ShutdownIniss, Shortcut.ShiftF5);
    public static CommandInfo KillIniss => new("KillINISS", Resources.Cmd_KillIniss, Shortcut.F10);
    public static CommandInfo RestartIniss => new("RestartINISS", Resources.Cmd_RestartIniss, Shortcut.CtrlShiftF5);
    public static CommandInfo InissStartupSettings => new("INISSStartupSettings", Resources.Cmd_InissStartupSettings, Shortcut.None);

    public static CommandInfo InfoApp => new("InfoApp", Resources.Cmd_InfoApp, Shortcut.F6);
    public static CommandInfo UpdateNotes => new("UpdateNotes", Resources.Cmd_UpdateNotes, Shortcut.None);
    public static CommandInfo DateLimit => new("DatObm", Resources.Cmd_DateLimit, Shortcut.F7);

    /// <summary>
    /// Vsetky prikazy v poradi, v akom ich ukazuju nastavenia skratiek.
    /// </summary>
    public static IReadOnlyList<CommandInfo> All =>
    [
        New, Open, ImportGvd, ImportData, ImportElis, Save, Analyze,
        AddTrain, EditTrain, DeleteTrains, DuplicateTrain,
        LocalSettings, GlobalSettings, AppSettings,
        GSGvds, GSLanguages, GSDelays, GSTrainTypes, GSAudio,
        LSGvd, LSLanguages, LSStations, LSOperators, LSPlatforms, LSTracks, LSPhysicalTables, LSLogicalTables,
        LSCatalogTables, LSTabTab, LSTableTexts, LSTableFonts, LSTabTabEditor, StateDgm,
        RunIniss, ShutdownIniss, KillIniss, RestartIniss, InissStartupSettings,
        InfoApp, UpdateNotes, DateLimit
    ];
}
