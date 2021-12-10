using ScintillaNET;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog vyhladavania textu v editore TabTab.
/// </summary>
//TODO dorobit FTabTabFindReplace dialog
public partial class FTabTabFindReplace : Form
{
    private readonly Scintilla scintilla;
    private int startPos, endPos;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<SearchingEventArgs> Searching;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTabFindReplace"/>.
    /// </summary>
    public FTabTabFindReplace(Scintilla scintilla)
    {
        InitializeComponent();
        this.scintilla = scintilla;
        this.scintilla.TextChanged += ScintillaOnTextChanged;
        startPos = 0;
        endPos = scintilla.Text.Length - 1;
    }

    private void ScintillaOnTextChanged(object sender, EventArgs e)
    {
        startPos = 0;
        endPos = scintilla.Text.Length - 1;
    }

    private void bFind_Click(object sender, EventArgs e)
    {
        SetParams();
        int pos = scintilla.SearchInTarget(cbFind.Text);
        OnSearching(new SearchingEventArgs(pos));
        //startPos
    }

    private void bCount_Click(object sender, EventArgs e) => SetParams();

    private void SetParams()
    {
        if (cboxBackSearching.Checked)
        {
            //scintilla.SearchFlags |= SearchFlags.None; TODO
        }
        if (cboxSearchingWholeWord.Checked)
        {
            scintilla.SearchFlags |= SearchFlags.WholeWord;
        }
        if (cboxSearchingWholeWord.Checked)
        {
            scintilla.SearchFlags |= SearchFlags.MatchCase;
        }

        if (rbAdvancedSearching.Checked)
        {
            //scintilla.SearchFlags |= SearchFlags.; TODO
        }
        if (rbRegExSearching.Checked)
        {
            scintilla.SearchFlags |= SearchFlags.Regex;
        }
    }

    private void bFindClose_Click(object sender, EventArgs e) => Close();

    private void cboxTransparency_CheckedChanged(object sender, EventArgs e)
    {
        if (cboxTransparency.Checked)
        {
            barTransparency.Enabled = true;
        }
        else
        {
            barTransparency.Enabled = false;
            Opacity = 1;
        }
    }

    private void barTransparency_Scroll(object sender, EventArgs e)
    {
        if (rbTransAlways.Checked)
        {
            Opacity = barTransparency.Value / 100f;
        }
    }

    private void rbTransOnlyOnFocusLost_CheckedChanged(object sender, EventArgs e)
    {
        if (rbTransOnlyOnFocusLost.Checked)
        {
            Opacity = 1;
        }
    }

    private void FTabTabFindReplace_Leave(object sender, EventArgs e)
    {
        if (cboxTransparency.Checked && rbTransOnlyOnFocusLost.Checked)
        {
            Opacity = barTransparency.Value / 100f;
        }
    }

    private void rbRegExSearching_CheckedChanged(object sender, EventArgs e)
    {
        if (rbRegExSearching.Checked)
        {
            cboxBackSearching.Checked = false;
            cboxSearchingWholeWord.Checked = false;
            cboxBackSearching.Enabled = false;
            cboxSearchingWholeWord.Enabled = false;
        }
        else
        {
            cboxBackSearching.Enabled = true;
            cboxSearchingWholeWord.Enabled = true;
        }
    }

    private void OnSearching(SearchingEventArgs e) => Searching?.Invoke(this, e);
}

/// <summary>
///     Trida reprezentujuca data pri hladani v TabTab editore
/// </summary>
public class SearchingEventArgs : EventArgs
{
    /// <summary>
    ///     Pozicia zaciatku najdeneho vyrazu
    /// </summary>
    public int Position { get; }
    /// <summary>
    ///     Konstuktor
    /// </summary>
    /// <param name="start"></param>
    public SearchingEventArgs(int start)
    {
        Position = start;
    }
}