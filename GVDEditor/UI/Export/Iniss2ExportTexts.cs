using System.Globalization;
using GVDEditor.Integration.Iniss2;
using GVDEditor.Properties;

namespace GVDEditor.UI.Export;

/// <summary>
/// Texty upozorneni exportu do INISS2 pre obsluhu.
/// </summary>
internal static class Iniss2ExportTexts
{
    /// <summary>
    /// Popis upozornenia.
    /// </summary>
    public static string Issue(Iniss2ExportIssue issue) => issue switch
    {
        Iniss2ExportIssue.StationWithoutNumber => Resources.Iniss2Export_Issue_StationWithoutNumber,
        Iniss2ExportIssue.RouteWithoutStation => Resources.Iniss2Export_Issue_RouteWithoutStation,
        Iniss2ExportIssue.BadValidity => Resources.Iniss2Export_Issue_BadValidity,
        Iniss2ExportIssue.PeriodTooLong => Resources.Iniss2Export_Issue_PeriodTooLong,
        Iniss2ExportIssue.LongDistanceFlag => Resources.Iniss2Export_Issue_LongDistanceFlag,
        Iniss2ExportIssue.VariantOverlap => Resources.Iniss2Export_Issue_VariantOverlap,
        Iniss2ExportIssue.SharedNumberTracks => Resources.Iniss2Export_Issue_SharedNumberTracks,
        Iniss2ExportIssue.CompositionWithoutTrain => Resources.Iniss2Export_Issue_CompositionWithoutTrain,
        Iniss2ExportIssue.CompositionOutsidePeriod => Resources.Iniss2Export_Issue_CompositionOutsidePeriod,
        Iniss2ExportIssue.NoticeOutsidePeriod => Resources.Iniss2Export_Issue_NoticeOutsidePeriod,
        Iniss2ExportIssue.LogTextMissing => Resources.Iniss2Export_Issue_LogTextMissing,
        Iniss2ExportIssue.LogTextFunctionMissing => Resources.Iniss2Export_Issue_LogTextFunctionMissing,
        Iniss2ExportIssue.TrainNotMerged => Resources.Iniss2Export_Issue_TrainNotMerged,
        Iniss2ExportIssue.StationDayOutsidePeriod => Resources.Iniss2Export_Issue_StationDayOutsidePeriod,
        Iniss2ExportIssue.StationTwice => Resources.Iniss2Export_Issue_StationTwice,
        Iniss2ExportIssue.TableUnsupported => Resources.Iniss2Export_Issue_TableUnsupported,
        Iniss2ExportIssue.TableViewUnknown => Resources.Iniss2Export_Issue_TableViewUnknown,
        Iniss2ExportIssue.ContentUnsupported => Resources.Iniss2Export_Issue_ContentUnsupported,
        Iniss2ExportIssue.ColumnOverlap => Resources.Iniss2Export_Issue_ColumnOverlap,
        Iniss2ExportIssue.TabTabNotConverted => Resources.Iniss2Export_Issue_TabTabNotConverted,
        Iniss2ExportIssue.TabTabMissing => Resources.Iniss2Export_Issue_TabTabMissing,
        Iniss2ExportIssue.TabTabGlobalDuplicate => Resources.Iniss2Export_Issue_TabTabGlobalDuplicate,
        Iniss2ExportIssue.TableTextConflict => Resources.Iniss2Export_Issue_TableTextConflict,
        Iniss2ExportIssue.TableWithoutLine => Resources.Iniss2Export_Issue_TableWithoutLine,
        Iniss2ExportIssue.LineUnsupported => Resources.Iniss2Export_Issue_LineUnsupported,
        Iniss2ExportIssue.FontBankMissing => Resources.Iniss2Export_Issue_FontBankMissing,
        Iniss2ExportIssue.StateDiagramMissing => Resources.Iniss2Export_Issue_StateDiagramMissing,
        Iniss2ExportIssue.StateDiagramUnreadable => Resources.Iniss2Export_Issue_StateDiagramUnreadable,
        Iniss2ExportIssue.StateExpressionNotConverted => Resources.Iniss2Export_Issue_StateExpressionNotConverted,
        Iniss2ExportIssue.StatePartNotConverted => Resources.Iniss2Export_Issue_StatePartNotConverted,
        Iniss2ExportIssue.StateReportConflict => Resources.Iniss2Export_Issue_StateReportConflict,
        Iniss2ExportIssue.StateReportMissing => Resources.Iniss2Export_Issue_StateReportMissing,
        Iniss2ExportIssue.LogTextFunctionAdded => Resources.Iniss2Export_Issue_LogTextFunctionAdded,
        Iniss2ExportIssue.ServerRunning => Resources.Iniss2Export_Issue_ServerRunning,
        Iniss2ExportIssue.GroupExists => Resources.Iniss2Export_Issue_GroupExists,
        _ => throw new ArgumentOutOfRangeException(nameof(issue), issue, null)
    };

    /// <summary>
    /// Hlasenie, preco sa export nedal vykonat: cast, ktorej sa tyka, a popis.
    /// </summary>
    public static string Error(Iniss2ExportException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return exception.Message.Length == 0
            ? Issue(exception.Issue)
            : string.Format(CultureInfo.CurrentCulture, Resources.Iniss2Export_IssueDetail, exception.Message, Issue(exception.Issue));
    }
}
