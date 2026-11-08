namespace GVDEditor.UI.EditTrain;

public class CalendarCellColumn : DataGridViewColumn
{
    public CalendarCellColumn()
    {
        base.CellTemplate = new CalendarCell();
    }
}
