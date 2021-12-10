using GVDEditor.Properties;

namespace GVDEditor.XML;

internal static class GVDEditorSettingsNaming
{
    /// <summary>
    ///     Nazve nastavenia farieb podla aktualnej lokalizacie.
    /// </summary>
    public static void NameColorSettings(GVDEditorStyle style)
    {
        style.TabTabEditorScheme.Default.Name = Resources.SetTEDefault;
        style.TabTabEditorScheme.Default.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Function.Name = Resources.SetTEKeyword;
        style.TabTabEditorScheme.Function.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Identifier.Name = Resources.SetTEIdentifier;
        style.TabTabEditorScheme.Identifier.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Number.Name = Resources.SetTENumber;
        style.TabTabEditorScheme.Number.DisableBackColorEdit = true;
        style.TabTabEditorScheme.String.Name = Resources.SetTEString;
        style.TabTabEditorScheme.String.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Comment.Name = Resources.SetTEComment;
        style.TabTabEditorScheme.Comment.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Var.Name = Resources.SetTEVar;
        style.TabTabEditorScheme.Var.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Event.Name = Resources.SetTEEvent;
        style.TabTabEditorScheme.Event.DisableBackColorEdit = true;
        style.TabTabEditorScheme.OnNewLine.Name = Resources.SetTEOnNewLine;
        style.TabTabEditorScheme.OnNewLine.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Operator.Name = Resources.SetTEOperator;
        style.TabTabEditorScheme.Operator.DisableBackColorEdit = true;
        style.TabTabEditorScheme.Constant.Name = Resources.SetTEConstant;
        style.TabTabEditorScheme.Constant.DisableBackColorEdit = true;
        style.TabTabEditorScheme.SelBraces.Name = Resources.SetTEBraces;
        style.TabTabEditorScheme.SelBraceBad.Name = Resources.SetTEBraceBad;

        style.TrainTypeColumnScheme.Os.Name = Resources.SetDesktopTTOs;
        style.TrainTypeColumnScheme.R.Name = Resources.SetDesktopTTR;
        style.TrainTypeColumnScheme.X.Name = Resources.SetDesktopTTX;
        style.TrainTypeColumnScheme.Sl.Name = Resources.SetDesktopTTSl;

        style.ControlsColorScheme.Panel.Name = Resources.NameColorSettings_Panel;
        style.ControlsColorScheme.Button.Name = Resources.NameColorSettings_Button;
        style.ControlsColorScheme.Label.Name = Resources.NameColorSettings_Label;
        style.ControlsColorScheme.Box.Name = Resources.NameColorSettings_Box;
        style.ControlsColorScheme.Border.Name = Resources.NameColorSettings_Border;
        style.ControlsColorScheme.Border.DisableBackColorEdit = true;
        style.ControlsColorScheme.Mark.Name = Resources.NameColorSettings_Mark;
        style.ControlsColorScheme.Mark.DisableBackColorEdit = true;
        style.ControlsColorScheme.Highlight.Name = Resources.NameColorSettings_Highlight;
    }

    /// <summary>
    ///     Pomenuje názvy pre stĺpce tabulky na hlavnej obrazovke.
    /// </summary>
    /// <param name="columns"></param>
    public static void NameDesktopColsSetting(DesktopColumns columns)
    {
        columns.Number.Name = "Číslo";
        columns.Number.Id = 0;

        columns.Type.Name = "Typ";
        columns.Type.Id = 1;

        columns.Name.Name = "Názov";
        columns.Name.Id = 2;

        columns.LinkaPrichod.Name = "Linka - príchod";
        columns.LinkaPrichod.Id = 3;

        columns.LinkaOdchod.Name = "Linka - odchod";
        columns.LinkaOdchod.Id = 4;

        columns.Routing.Name = "Smerovanie";
        columns.Routing.Id = 5;

        columns.Prichod.Name = "Príchod";
        columns.Prichod.Id = 6;

        columns.Odchod.Name = "Odchod";
        columns.Odchod.Id = 7;

        columns.VychodziaStanica.Name = "Východzia stanica";
        columns.VychodziaStanica.Id = 8;

        columns.KonecnaStanica.Name = "Konečná stanica";
        columns.KonecnaStanica.Id = 9;

        columns.DateLimit.Name = "Dátumová obmedzenie";
        columns.DateLimit.Id = 10;

        columns.Track.Name = "Koľaj";
        columns.Track.Id = 11;

        columns.Operator.Name = "Dopravca";
        columns.Operator.Id = 12;

        columns.OtherBtn.Name = "Ostatné";
        columns.OtherBtn.Id = 13;
    }

    /// <summary>
    ///     Pomenuje názvy pre klávesové skratky programu.
    /// </summary>
    /// <param name="shortcuts">klavesove skratky programu</param>
    public static void NameShortcutCommands(AppShortcuts shortcuts)
    {
        shortcuts.NewGVD.Name = "Nový grafikon";
        shortcuts.OpenGVD.Name = "Otvoriť grafikon";
        shortcuts.ImportGVD.Name = "Importovať grafikon";
        shortcuts.ImportData.Name = "Importovať dáta";
        shortcuts.Save.Name = "Uložiť";
        shortcuts.Analyze.Name = "Analyzovať grafikon";

        shortcuts.AddTrain.Name = "Pridať vlak";
        shortcuts.EditTrain.Name = "Upraviť vlak";
        shortcuts.DeleteTrains.Name = "Odstrániť označené";
        shortcuts.DuplicateTrain.Name = "Duplikovať vlak";

        shortcuts.LocalSettings.Name = "Lokálne nastavenia";
        shortcuts.GlobalSettings.Name = "Globálne nastavenia";
        shortcuts.AppSettings.Name = "Nastavenia programu";

        shortcuts.InfoApp.Name = "Informácie o programe";
        shortcuts.UpdateNotes.Name = "Poznámky k aktualizáciám";

        shortcuts.RunINISS.Name = "Spustiť INISS";
        shortcuts.ShutdownINISS.Name = "Ukončiť INISS";
        shortcuts.KillINISS.Name = "Vynútene ukončiť INISS";
        shortcuts.RestartINISS.Name = "Reštartovať INISS";

        shortcuts.LSGrafikon.Name = "Lokálne nastavenia - Grafikon";
        shortcuts.LSStanice.Name = "Lokálne nastavenia - Stanice";
        shortcuts.LSDopravcovia.Name = "Lokálne nastavenia - Dopravcovia";
        shortcuts.LSPlatforms.Name = "Lokálne nastavenia - Nástupištia";
        shortcuts.LSKolaje.Name = "Lokálne nastavenia - Koľaje";
        shortcuts.LSTPhysicals.Name = "Lokálne nastavenia - Fyzické tabule";
        shortcuts.LSTLogicals.Name = "Lokálne nastavenia - Logické tabule";
        shortcuts.LSTCatalogs.Name = "Lokálne nastavenia - Katalóg tabúľ";
        shortcuts.LSTabTab.Name = "Lokálne nastavenia - TabTab";
        shortcuts.LSTTexts.Name = "Lokálne nastavenia - Texty do tabúľ";
        shortcuts.LSTFonts.Name = "Lokálne nastavenia - Písma do tabúľ";
        shortcuts.LSTabTabEditor.Name = "Lokálne nastavenia - Editor TabTab";

        shortcuts.GSGrafikony.Name = "Globálne nastavenia - Grafikony";
        shortcuts.GSLanguages.Name = "Globálne nastavenia - Jazyky";
        shortcuts.GSMeskania.Name = "Globálne nastavenia - Meškania";
        shortcuts.GSTrainTypes.Name = "Globálne nastavenia - Typy vlakov";
        shortcuts.GSAudio.Name = "Globálne nastavenia - Audio";

        shortcuts.DatObm.Name = "Generátor dátum. obm.";
    }
}