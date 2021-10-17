using System.Collections.Generic;
using TabTabCompiler.Components;

namespace TabTabCompiler.Tools
{
    public class TokenList
    {
        public int Position {get; set; }

        public List<Token> Tokens { get; }

        /// <summary>Initializes a new instance of the <see cref="TokenList" /> class.</summary>
        public TokenList(List<Token> tokens)
        {
            Position = 0;
            Tokens = tokens;
        }

        public Token GetToken()
        {
            if (Position == Tokens.Count)
                return null;

            var ret = Tokens[Position];
            Position++;
            return ret;
        }

        public Token Peek()
        {
            if (Position == Tokens.Count)
                return null;

            return Tokens[Position];
        }

        public Token PeekNext()
        {
            if (Position + 1 == Tokens.Count)
                return null;

            return Tokens[Position + 1];
        }
    }
}
