namespace GVDEditor.Mcp;

/// <summary>
/// Strucny opis jazyka TabTab a cisel pisiem pre agenta - podla specifikacie v iniss-tools-docs
/// (docs/iniss/formaty-suborov/local/tabtab.mdx, tkatalog.mdx, vyrazy.mdx). Pri zmene specifikacie upravit aj tu.
/// </summary>
internal static class TabTabReference
{
    public const string Text = """
        TabTab (file TabTab.TXT in the timetable folder) - recoding tables for boards.

        WHERE IT APPLIES
        - Named sections [NAME] hold rules. A column of a catalog board (TKatalog) refers to a section by TYPE_ITEMS_TAB1 (and TAB2 for times).
        - The column's DIVTYPE decides how the section is used:
          0 = not at all (no rule works, not even #SWITCH);
          1 = the whole text is looked up, only a found text is shown (not found = empty);
          2 = time "hh:mm": hours looked up in TAB1, minute tens and units in TAB2;
          3 = the whole text is looked up, a text not found passes unchanged;
          4 = character by character (a character not found becomes a space).
          With 2 and 4 a text starting with # is looked up as a whole.

        SIMPLE RULE   <text for the board> = <text from INISS>
        - If the column text equals the RIGHT side, the LEFT side goes to the board (opposite of what one would expect).
        - @ stands for the original column text, e.g. {@}=-{@} puts a dash before it.
        - {n} after a text is a font code: R{81}=R sends R in font 81. Elsewhere {n} writes the character with code n.
        - # inside a text separates parts the board shows alternately.
        - \ at line end continues the line; a line starting with ; is a comment.
        - Pairs like R{81}=R and R{81}=R{81} are usual: the second catches R that already has font 81.
        - A left side without a font keeps the font the text had (column FONT_IDX, TTexts.txt or the #SWITCH/#MERGE result).

        CONDITIONAL RULES   (event after =)
        - cond, "text", cond, "text", ... = #SWITCH   -> the text of the FIRST true condition. Condition 1 is always true (fallback at the end).
        - cond, "separator", "text", ... = #MERGE     -> appends the texts of ALL true conditions; the separator only between texts; empty texts
          are skipped; the font of the result is the font of the first part that has one. On boards that alternate parts, #MERGE joins
          alternating parts part by part (5 + min#MIN = 5min#5MIN); elsewhere it behaves like #MERGE2.
        - ... = #MERGE2   -> like #MERGE, texts are only concatenated (5 + min#MIN = 5min#MIN).
        - %name% in a text inserts the value of another column of the same board.
        - Fixed-key simple rules: <text> = #VYLUKA is shown when the train has a track lockout (flag Prizn_VylP for arrival columns,
          Prizn_VylO for departure); <text> = #ODKLON when the train is diverted; <text> = #POZODJ_<track> by the departure track
          (the track must exist in Pozice.TXT). They work only in columns with DIVTYPE other than 0.
          Example: Bus=#VYLUKA means "show Bus when there is a lockout", not "use section VYLUKA".

        ORDER OF COMPOSING A COLUMN TEXT (fixed)
        1 #VYLUKA, 2 #ODKLON, 3 #POZODJ_<departure track>, 4 #SWITCH, 5 #MERGE, 6 #MERGE2,
        7 replacement text of lockout or diversion set in the program (direction and destination columns), 8 text from TTexts.txt,
        9 the column's own value.
        The first source giving a text closes it - unless the text contains @: then it is kept and @ is filled by the next source, finally
        by the column's own value. The simple rules are applied to the finished text according to DIVTYPE.

        CONDITIONS - INISS expression language (same as StateDgm)
        - TYP(Typ_R) train type, PRIZNAK(Prizn_M) flag, STAV(Stav_Stoji) state, POZICE == Poz_K position (Poz_V originates, Poz_P passes,
          Poz_K terminates), OPERATOR("..."), KOLEJPRIJ("...") / KOLEJODJ("...") tracks, ZESMERU(id) / DOSMERU(id) / CILSTANICE(id) stations,
          ODKLON, VYLUKAZDE, ZPOZDENI / ZPOZDENIPRIJ delay in minutes, LINKAODJ("...") departure line. Names are case-insensitive.
        - Operators && || ! == != < > <= >=. A condition is true when non-zero.
        - ALL connectives have the SAME precedence and bind to the right: a && b || c means a && (b || c).
        - Typ_X must be a train type of TrTypes.txt (GVDEditor warns about others).

        ERRORS INISS LOGS
        - unknown magic item (unknown #NAME after =), uneven item count for #SWITCH, count not a multiple of 3 for #MERGE/#MERGE2.
        - An error in a condition skips that item when the board is drawn.

        FONT NUMBERS
        - ELEN boards (ELEN, ELENOLD, ELEN10, ELEN16, ELEN16Kam, ELEKON): bits 0-1 colour (0 none, 1 red, 2 green, 3 yellow), bit 2 (4)
          blinking, bit 3 (8) tall digits, bits 4-5 face (0 fixed 6 px, 1 thin, 2 bold, 3 digits only), bit 0x40 is usually set; with bit
          0x8000 (ELEN10/ELEN16) bits 8-11 select an extended font. E.g. 34369 = 0x8641 = extended font 6, red.
        - Other manufacturers use font numbers of the timetable font list (list_tables, fonts).
        - get_tabtab decodes the font numbers of a section for both readings.

        VERIFY, DO NOT GUESS
        - preview_board_text composes the real column texts for a train (optionally with lockout, diversion or delay) with every step of
          the order above - use it to confirm how a section behaves for a train type, line or state.
        """;
}
