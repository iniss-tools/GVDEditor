using System.Collections.Generic;
using ScintillaNET;

namespace GVDEditor.Tools
{
    /// <summary>
    ///     Zabezpečuje štýly formátovanie textu v prvku Scintilla vo FTabTab form
    /// </summary>
    public class TabTabLexer
    {
        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_COMMENT = 4;
        private const int STATE_VAR = 5;

        private readonly HashSet<string> constans;
        private readonly HashSet<string> events;
        private readonly HashSet<string> functions;
        private readonly HashSet<string> operators;

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="events"></param>
        /// <param name="constans"></param>
        /// <param name="operators"></param>
        public TabTabLexer(IEnumerable<string> functions, IEnumerable<string> events, IEnumerable<string> constans, IEnumerable<string> operators)
        {
            this.functions = new HashSet<string>(functions);
            this.events = new HashSet<string>(events);
            this.constans = new HashSet<string>(constans);
            this.operators = new HashSet<string>(operators);
        }

        /// <summary>
        ///     Nastavuje štýl pre prvok Scintilla RichTextBox v FTabTab
        /// </summary>
        /// <param name="scintilla"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            var length = 0;
            var state = STATE_UNKNOWN;

            // Start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
                var c = (char)scintilla.GetCharAt(startPos);

                REPROCESS:
                switch (state)
                {
                    case STATE_UNKNOWN:
                        if (c == '"')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, TabTabStyle.String);
                            state = STATE_STRING;
                        }
                        else if (c == '%')
                        {
                            scintilla.SetStyling(1, TabTabStyle.Var);
                            state = STATE_VAR;
                        }
                        else if (char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (char.IsLetter(c) || c == '#')
                        {
                            state = STATE_IDENTIFIER;
                            goto REPROCESS;
                        }
                        else if (c == ';')
                        {
                            // Start of "comment"
                            scintilla.SetStyling(1, TabTabStyle.Comment);
                            state = STATE_COMMENT;
                            goto REPROCESS;
                        }
                        else if (c == '\\')
                        {
                            scintilla.SetStyling(1, TabTabStyle.OnNewLine);
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, TabTabStyle.Default);
                        }

                        break;

                    case STATE_STRING:
                        if (c == '"')
                        {
                            length++;
                            scintilla.SetStyling(length, TabTabStyle.String);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }

                        break;

                    case STATE_VAR:
                        if (c == '%')
                        {
                            length++;
                            scintilla.SetStyling(length, TabTabStyle.Var);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }

                        break;

                    case STATE_NUMBER:
                        if (char.IsDigit(c) || c is >= 'a' and <= 'f' or >= 'A' and <= 'F' or 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, TabTabStyle.Number);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }

                        break;

                    case STATE_IDENTIFIER:
                        if (char.IsLetterOrDigit(c) || c is '#' or '_')
                        {
                            length++;
                        }
                        else
                        {
                            var style = TabTabStyle.Identifier;
                            var identifier = scintilla.GetTextRange(startPos - length, length);

                            if (events.Contains(identifier))
                                style = TabTabStyle.Event;
                            if (operators.Contains(identifier))
                                style = TabTabStyle.Operator;
                            if (constans.Contains(identifier.ToUpper()))
                                style = TabTabStyle.Constant;
                            if (functions.Contains(identifier.ToUpper()))
                                style = TabTabStyle.Function;

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }

                        break;

                    case STATE_COMMENT:
                        if (c == '\n' || startPos == endPos - 1)
                        {
                            scintilla.SetStyling(length, TabTabStyle.Comment);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }

                        break;
                }

                startPos++;
            }
        }
    }

    /// <summary>
    ///     Typy štýlov pre TabTab
    /// </summary>
    public static class TabTabStyle
    {
        /// <summary>
        ///     Bežný text
        /// </summary>
        public const int Default = 0;

        /// <summary>
        ///     Funkcia
        /// </summary>
        public const int Function = 1;

        /// <summary>
        ///     Identifikátor
        /// </summary>
        public const int Identifier = 2;

        /// <summary>
        ///     Číslo
        /// </summary>
        public const int Number = 3;

        /// <summary>
        ///     Reťazec
        /// </summary>
        public const int String = 4;

        /// <summary>
        ///     Komentár
        /// </summary>
        public const int Comment = 5;

        /// <summary>
        ///     Premenná
        /// </summary>
        public const int Var = 6;

        /// <summary>
        ///     Udalosť
        /// </summary>
        public const int Event = 7;

        /// <summary>
        ///     Pokračovanie do nového riadku
        /// </summary>
        public const int OnNewLine = 8;

        /// <summary>
        ///     Operátor
        /// </summary>
        public const int Operator = 9;

        /// <summary>
        ///     Konštanta
        /// </summary>
        public const int Constant = 10;
    }
}