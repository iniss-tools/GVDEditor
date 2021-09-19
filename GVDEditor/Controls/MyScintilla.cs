using System;
using System.ComponentModel;
using System.Windows.Forms;
using ScintillaNET;

namespace GVDEditor.Controls
{
    /// <summary>
    ///     Custom Scintilla Control.
    /// </summary>
    public partial class MyScintilla : UserControl
    {
        /// <summary>
        ///     Custom Scintilla Control.
        /// </summary>
        public MyScintilla()
        {
            InitializeComponent();

            VScrollBarControl.Scroll += VScrollBarControlOnScroll;
            HScrollBarControl.Scroll += HScrollBarControlOnScroll;
        }

        private void VScrollBarControlOnScroll(object sender, ScrollEventArgs e)
        {
            scintilla.FirstVisibleLine = e.NewValue;
        }

        private void HScrollBarControlOnScroll(object sender, ScrollEventArgs e)
        {
            scintilla.XOffset = e.NewValue;
        }

        private void Scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            if (IsChange(e.Change, UpdateChange.VScroll))
            {
                VScrollBarControl.Minimum = 0;
                VScrollBarControl.Maximum = scintilla.Lines.Count;
                VScrollBarControl.LargeChange = scintilla.LinesOnScreen;
                VScrollBarControl.SmallChange = 1;
                VScrollBarControl.Value = scintilla.FirstVisibleLine;
            }

            if (IsChange(e.Change, UpdateChange.HScroll))
            {
                HScrollBarControl.Minimum = 0;
                HScrollBarControl.Maximum = scintilla.ScrollWidth;
                HScrollBarControl.LargeChange = scintilla.Width;
                HScrollBarControl.SmallChange = 10;
                HScrollBarControl.Value = scintilla.XOffset;
            }

            if (IsChange(e.Change, UpdateChange.Content))
            {
                ChangeScrollWidth();

                if (scintilla.Lines.Count < scintilla.LinesOnScreen)
                {
                    pVertical.Visible = false;
                }
                else
                {
                    pVertical.Visible = true;
                    VScrollBarControl.Minimum = 0;
                    VScrollBarControl.Maximum = scintilla.Lines.Count;
                    VScrollBarControl.LargeChange = scintilla.LinesOnScreen;
                    VScrollBarControl.SmallChange = 1;
                    VScrollBarControl.Value = scintilla.FirstVisibleLine;
                }

                if (scintilla.ScrollWidth < scintilla.Width)
                {
                    pHorizontal.Visible = false;
                }
                else
                {
                    pHorizontal.Visible = true;
                    HScrollBarControl.Minimum = 0;
                    HScrollBarControl.Maximum = scintilla.ScrollWidth;
                    HScrollBarControl.LargeChange = scintilla.Width;
                    HScrollBarControl.SmallChange = 10;
                    HScrollBarControl.Value = scintilla.XOffset;
                }
            }
        }

        private static bool IsChange(UpdateChange changes, UpdateChange expected)
        {
            return (changes & expected) != 0;
        }

        private int LargestLine()
        {
            int index = 0;
            int len = 0;
            foreach (var line in scintilla.Lines)
            {
                if (line.Length > len)
                {
                    index = line.Index;
                    len = line.Length;
                }
            }

            return index;
        }

        private void ChangeScrollWidth()
        {
            var index = LargestLine();
            if (scintilla.Lines[index].EndPosition - 2 >= 0)
            {
                int point = scintilla.PointXFromPosition(scintilla.Lines[index].EndPosition - 2);
                scintilla.ScrollWidth = point + 100;
            }
        }

        /// <summary>
        ///     Dokument bol vymeneny.
        /// </summary>
        public void SwitchedDocument()
        {
            Scintilla_UpdateUI(scintilla, new UpdateUIEventArgs(UpdateChange.Content));
        }

        /// <summary>
        ///     Gets the Scintilla control.
        /// </summary>
        [Browsable(true)]
        public Scintilla Scintilla => scintilla;

        /// <summary>Occurs when the user enters a text character.</summary>
        [Category("Notifications")]
        [Description("Occurs when the user types a character.")]
        public event EventHandler<CharAddedEventArgs> CharAdded
        {
            add => scintilla.CharAdded += value;
            remove => scintilla.CharAdded -= value;
        }

        /// <summary>
        /// Occurs when the control is about to display or print text and requires styling.
        /// </summary>
        /// <remarks>
        /// This event is only raised when <see cref="P:ScintillaNET.Scintilla.Lexer" /> is set to <see cref="F:ScintillaNET.Lexer.Container" />.
        /// The last position styled correctly can be determined by calling <see cref="M:ScintillaNET.Scintilla.GetEndStyled" />.
        /// </remarks>
        /// <seealso cref="M:ScintillaNET.Scintilla.GetEndStyled" />
        [Category("Notifications")]
        [Description("Occurs when the text needs styling.")]
        public event EventHandler<StyleNeededEventArgs> StyleNeeded
        {
            add => scintilla.StyleNeeded += value;
            remove => scintilla.StyleNeeded -= value;
        }

        /// <summary>
        /// Occurs when the control UI is updated as a result of changes to text (including styling),
        /// selection, and/or scroll positions.
        /// </summary>
        [Category("Notifications")]
        [Description("Occurs when the control UI is updated.")]
        public event EventHandler<UpdateUIEventArgs> UpdateUI
        {
            add => scintilla.UpdateUI += value;
            remove => scintilla.UpdateUI -= value;
        }

        /// <inheritdoc cref="TextChanged"/>
        public new event EventHandler TextChanged
        {
            add => scintilla.TextChanged += value;
            remove => scintilla.TextChanged -= value;
        }

        /// <inheritdoc cref="KeyPress"/>
        public new event KeyPressEventHandler KeyPress
        {
            add => scintilla.KeyPress += value;
            remove => scintilla.KeyPress -= value;
        }

        /// <inheritdoc cref="MouseDown"/>
        public new event MouseEventHandler MouseDown
        {
            add => scintilla.MouseDown += value;
            remove => scintilla.MouseDown -= value;
        }
    }
}
