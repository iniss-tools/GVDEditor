using System.Windows.Forms;
using ToolsCore.Tools;

namespace GVDEditor.Forms
{
    /// <summary>
    ///     Okno oznamujuce pouzivatelovi nacitavanie dat
    /// </summary>
    public partial class FWait : Form
    {
        /// <summary>
        ///     Konstruktor
        /// </summary>
        public FWait(string text = null)
        {
            InitializeComponent();
            FormUtils.SetFormFont(this);
            this.ApplyTheme();

            if (text != null) lText.Text = text;
        }
    }
}