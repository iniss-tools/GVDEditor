using System.Collections;
using GVDEditor.Tools;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

public partial class FDatObm : Form
{
    public FDatObm()
    {
        InitializeComponent();
        FormUtils.SetFormFont(this);
        this.ApplyTheme();
    }

    private void bCopy_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(tbBitArray.Text);
    }

    private static string ToBitString(BitArray bits)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < bits.Count; i++)
        {
            var c = bits[i] ? '1' : '0';
            sb.Append(c);
        }

        return sb.ToString();
    }

    private void bGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            var limit = new DateLimit(dpStart.Value.Date, 
                dpEnd.Value.Date,
                cbSpecDays.Checked, 
                insertMarks: false, 
                monthRoman: cbMonthRoman.Checked,
                skipDateRangeCheck: cbSkipDateRangeCheck.Checked);

            var bits = limit.TextToBitArray(tbDatObm.Text);
            tbBitArray.Text = ToBitString(bits);
        }
        catch (Exception ex)
        {
            Utils.ShowError(ex.Message);
        }
    }
}