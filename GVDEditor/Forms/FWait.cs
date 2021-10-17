using System.Drawing;
using System.Windows.Forms;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Okno oznamujuce pouzivatelovi nacitavanie dat.
    /// </summary>
    public partial class FWait : Form
    {
        /// <summary>
        ///     Vytvori novy formular typu <see cref="FWait"/>.
        /// </summary>
        public FWait(string text = null)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            if (text != null) 
                lText.Text = text;
        }

        private void FWait_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(GlobData.UsingStyle.ControlsColorScheme.Border.ForeColor);
            var rec = e.ClipRectangle;
            rec.Inflate(-1,-1);
            e.Graphics.DrawRectangle(pen, rec);
        }
    }
}