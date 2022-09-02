using ExControls;
using ScintillaNET;
using ToolsCore.Tools;

namespace GVDEditor.Forms;

/// <summary>
///     Dialog vyhladavania textu v editore TabTab.
/// </summary>
public partial class FTabTabFindReplace : Form
{
    private readonly Scintilla _scintilla;
    private readonly Color _defaultStatusTextColor;
    private bool _replaceMode;
    private bool _lastFound;
    private int _lastFoundStartPos = -1, _lastFoundEndPos = -1;
    private readonly bool _showReplace;
    private int _backupCaretPos;

    private static readonly ExBindingList<string> FindHistory = new(), ReplaceHistory = new();

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<SearchingEventArgs> Searching;

    /// <summary>
    ///     Vytvori novy formular typu <see cref="FTabTabFindReplace"/>.
    /// </summary>
    public FTabTabFindReplace(Scintilla scintilla, bool showReplace)
    {
        InitializeComponent();
        this.ApplyThemeAndFonts();
        _defaultStatusTextColor = tsslStatus.ForeColor;

        _scintilla = scintilla;
        _scintilla.TextChanged += ScintillaOnTextChanged;
        ResetTarget();

        cbFind.DataSource = FindHistory;
        cbReplace.DataSource = ReplaceHistory;
        _showReplace = showReplace;
    }

    private void ScintillaOnTextChanged(object sender, EventArgs e) => ResetTarget();

    private void FTabTabFindReplace_Load(object sender, EventArgs e) => tabControl.SelectedIndex = _showReplace ? 1 : 0;

    private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        _replaceMode = tabControl.SelectedIndex == 1;
        if (!_replaceMode)
        {
            bFindOrReplace.Text = "Hľadať ďalší";
            bCountOrReplaceAll.Text = "Spočítať";
            lReplace.Visible = false;
            cbReplace.Visible = false;
            cboxSearchingCyclic.Enabled = true;
        }
        else
        {
            bFindOrReplace.Text = "Nahradiť";
            bCountOrReplaceAll.Text = "Nahradiť všetky";
            lReplace.Visible = true;
            cbReplace.Visible = true;
            cboxSearchingCyclic.Enabled = false;
        }
    }

    private void bFindOrReplace_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(cbFind.Text) || (_replaceMode && string.IsNullOrEmpty(cbReplace.Text)))
        {
            SetStatus("Neplatný vstup pre hľadanie", Color.Red);
            return;
        }

        if (!FindHistory.Contains(cbFind.Text)) 
            FindHistory.Add(cbFind.Text);
        if (_replaceMode && !ReplaceHistory.Contains(cbReplace.Text))
            ReplaceHistory.Add(cbReplace.Text);

        SearchReplace();
    }

    private void bCountOrReplaceAll_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(cbFind.Text) || (_replaceMode && string.IsNullOrEmpty(cbReplace.Text)))
        {
            SetStatus("Neplatný vstup pre hľadanie", Color.Red);
            return;
        }

        if (!FindHistory.Contains(cbFind.Text))
            FindHistory.Add(cbFind.Text);
        if (_replaceMode && !ReplaceHistory.Contains(cbReplace.Text))
            ReplaceHistory.Add(cbReplace.Text);

        if (!_replaceMode)
        {
            SetStatus($"{SearchReplace(true)} nájdení", Color.Green);
        }
        else
        {
            SearchReplace(false, true);
        }
    }

    private void bClose_Click(object sender, EventArgs e) => Close();

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
            Opacity = barTransparency.Value / 100f;
    }

    private void rbTransOnlyOnFocusLost_CheckedChanged(object sender, EventArgs e)
    {
        if (rbTransOnlyOnFocusLost.Checked)
            Opacity = 1;
    }

    private void FTabTabFindReplace_Deactivate(object sender, EventArgs e)
    {
        if (cboxTransparency.Checked && rbTransOnlyOnFocusLost.Checked)
            Opacity = barTransparency.Value / 100f;
    }

    private void FTabTabFindReplace_Activated(object sender, EventArgs e)
    {
        if (cboxTransparency.Checked && rbTransOnlyOnFocusLost.Checked)
            Opacity = 1;
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

    private void CboxBackSearching_CheckedChanged(object sender, EventArgs e) => ResetTarget();

    /// <summary>
    ///     Sluzi na hladanie/nahradzovanie textu. Ci sa ma hladat alebo nahradzovat zavisi od _replaceMode.
    /// </summary>
    /// <param name="countOnly">ci chceme len spocitat pocet najdeni</param>
    /// <param name="replaceAll">ci sa ma nahradit vsetko (iba ak _replaceMode = true)</param>
    /// <returns></returns>
    private int SearchReplace(bool countOnly = false, bool replaceAll = false)
    {
        //Nahradenie textu, ktory bol oznaceny po predoslom stlaceni tlacidla Nahradit
        if (_replaceMode && !replaceAll && _lastFound)
        {
            //Zaloha vyberu v predchadzajucej iteracii 
            //TargetStart a TargetEnd je pociatocny a konecny symbol od/do ktoreho sa bude hladat
            var targetStart = _scintilla.TargetStart;
            var targetEnd = _scintilla.TargetEnd;

            //Vyber pozicie, ktora bola v predoslom hladani oznacena ako najdena
            //TargetStart a TargetEnd su tu pociatocna a konecna pozicia v texte, ktora bude nahradena
            _scintilla.TargetStart = _lastFoundStartPos;
            _scintilla.TargetEnd = _lastFoundEndPos;

            //Nahradenie textu novym
            _scintilla.ReplaceTarget(cbReplace.Text);

            //Obnova vyberu
            _scintilla.TargetStart = targetStart;
            _scintilla.TargetEnd = targetEnd;
        }

        //Hladanie noveho ciela...
        SetStatus();
        SetParams();

        var reversed = cboxBackSearching.Checked;
        var cyclic = cboxSearchingCyclic.Checked;

        if (countOnly || replaceAll)
        {
            ResetTarget(false); //Vykonat pocitanie/nahradenie vsetkeho od zaciatku suboru
            cyclic = false; //Aby sme zabranili nekonecnemu cyklu
        }

        var found = 0; //Pocet najdeni (relevantne iba ak je je countOnly=true, inak vrati iba 0 alebo 1, lebo nejde cyklicky)

        if (SearchCycleIsDone(reversed))
        {
            SetStatus("Prišli ste na koniec dokumentu");
            
            if (!cyclic)
            {
                ResetTarget();
                _lastFound = false;
                return found;
            }
            ResetTarget(false);
        }

        //Zaloha aktualnej pozicie caretu (pouziva sa pri replaceAll)
        _backupCaretPos = _scintilla.CurrentPosition;

        do
        {
            if (_scintilla.SearchInTarget(cbFind.Text) == -1) //Medzi TargetStart a TargetEnd sa nic nenaslo
            {
                OnSearching(new SearchingEventArgs(cbFind.Text, -1, -1, null));
                if (!cyclic)
                {
                    ResetTarget();
                    SetStatus(replaceAll ? $"Všetkých {found} výskytov boli nahradených": "Prišli ste na koniec dokumentu");
                    _scintilla.CurrentPosition = _backupCaretPos;
                    _lastFound = false;
                    return found;
                }

                ResetTarget(false);
                continue;
            }

            OnSearching(new SearchingEventArgs(cbFind.Text, _scintilla.TargetStart, _scintilla.TargetEnd, _replaceMode ? cbReplace.Text : null));

            if (_replaceMode && replaceAll)
            {
                //Zaloha vyberu v predchadzajucej iteracii 
                //TargetStart a TargetEnd su tu pociatocna a konecna pozicia v texte, ktora bude nahradena
                var targetStart = _scintilla.TargetStart;
                _scintilla.CurrentPosition = targetStart;

                //Nahradenie textu novym (vrati dlzku nahradeneho textu)
                var len = _scintilla.ReplaceTarget(cbReplace.Text);

                //Obnova vyberu
                _scintilla.TargetStart = targetStart;
                _scintilla.TargetEnd = targetStart + len;
            }

            _lastFound = true;
            _lastFoundStartPos = _scintilla.TargetStart;
            _lastFoundEndPos = _scintilla.TargetEnd;

            if (countOnly || replaceAll)
            {
                //Nasli sme, priratam k vysledku
                found++;
            }

            if (!replaceAll) //Pri nahradzovani vsetkeho nepotrebujeme zvyraznovat vyber
            {
                _scintilla.SelectionStart = _scintilla.TargetStart;
                _scintilla.SelectionEnd = _scintilla.TargetEnd;
            }

            //Nastavenie noveho targetu
            if (reversed)
            {
                _scintilla.TargetEnd = 0;
            }
            else
            {
                _scintilla.TargetStart = _scintilla.TargetEnd + 1;
                _scintilla.TargetEnd = _scintilla.TextLength - 1;
            }

        } while ((replaceAll || countOnly) && !SearchCycleIsDone(reversed));

        if (!cyclic && SearchCycleIsDone(reversed))
        {
            SetStatus("Prišli ste na koniec dokumentu");
            ResetTarget(false);
        }

        return found;
    }

    private bool SearchCycleIsDone(bool reversed)
    {
        return reversed ? 
            _scintilla.TargetStart <= _scintilla.TargetEnd : 
            _scintilla.TargetStart >= _scintilla.TargetEnd;
    }

    private void SetParams()
    {
        _scintilla.SearchFlags = SearchFlags.None;
        if (cboxSearchingWholeWord.Checked) _scintilla.SearchFlags |= SearchFlags.WholeWord;
        if (cboxSearchingCaseSensitive.Checked) _scintilla.SearchFlags |= SearchFlags.MatchCase;
        if (rbRegExSearching.Checked) _scintilla.SearchFlags |= SearchFlags.Regex;
    }

    private void SetStatus(string text = null, Color textColor = default)
    {
        tsslStatus.Text = text ?? "Pripravený";
        tsslStatus.ForeColor = textColor == default ? _defaultStatusTextColor : textColor;
    }

    private void ResetTarget(bool toCurrentPos = true)
    {
        if (cboxBackSearching.Checked)
        {
            _scintilla.TargetStart = _scintilla.TextLength - 1;
            _scintilla.TargetEnd = toCurrentPos ? _scintilla.CurrentPosition : 0;
        }
        else
        {
            _scintilla.TargetStart = toCurrentPos ?  _scintilla.CurrentPosition : 0;
            _scintilla.TargetEnd = _scintilla.TextLength - 1;
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
    ///     Hladany text.
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     Nahradzany text.
    /// </summary>
    public string NewText { get; }

    /// <summary>
    ///     Pozicia zaciatku najdeneho vyrazu.
    /// </summary>
    public int StartPosition { get; }

    /// <summary>
    ///     Pozicia konca najdeneho vyrazu.
    /// </summary>
    public int EndPosition { get; }

    public int Length => EndPosition - StartPosition;

    public bool Replacing => !string.IsNullOrEmpty(NewText);

    /// <summary>
    ///     Konstuktor
    /// </summary>
    /// <param name="startPosition"></param>
    public SearchingEventArgs(string text, int startPosition, int endPosition, string newText)
    {
        Text = text;
        StartPosition = startPosition;
        EndPosition = endPosition;
        NewText = newText;
    }
}