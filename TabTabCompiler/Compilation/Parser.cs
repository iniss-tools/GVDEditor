using TabTabCompiler.Components;
using TabTabCompiler.Components.AST;
using TabTabCompiler.Tools;
using Expression = TabTabCompiler.Components.AST.Expression;

namespace TabTabCompiler.Compilation
{
    public class Parser
    {
        private readonly TokenList tokens;

        public Block Block { get; set; }

        public Parser(TokenList tokens)
        {
            this.tokens = tokens;
            Block = ParseBlock();
        }

        private Block ParseBlock()
        {
            var tok = tokens.Peek();

            switch (tok)
            {
                case IdentifierToken:
                    var fun = Function.Parse(tok.Text);
                    if (fun != null)
                    {
                        return ParseConditionTypeBlock();
                    }
                    else
                    {
                        var trtype = TrType.Parse(tok.Text);
                        if (trtype != null)
                        {
                            return ParseSetFontsTypeBlock();
                        }

                        Compiler.Outputs.Add(new ErrorOutput(tok.Info, "Neocakavany token"));
                        return null;
                    }
                case OpenParenToken:
                    return ParseConditionTypeBlock();
                case StartFontSetToken:
                    return ParseSetFontsTypeBlock();
                case NumberLiteralToken:
                    return ParseConditionTypeBlock();
                case StringLiteralToken:
                    return ParseSetFontsTypeBlock();
                case ViewValuesToken:
                    return ParseReplaceTextTypeBlock();
                case EndOfTheFileToken:
                    return null;
                default:
                    Compiler.Outputs.Add(new ErrorOutput(tok.Info, "Neocakavany token"));
                    return null;
            }
        }

        private ConditionTypeBlock ParseConditionTypeBlock()
        {
            var running = true;
            var block = new ConditionTypeBlock();
            do
            {
                var stmt = new ConditionStmt
                {
                    Condition = ParseConditionExpr(null), 
                    Block = ParseConditionStatements()
                };

                block.Statements.Add(stmt);
                switch (tokens.Peek())
                {
                    case NewLineConditionToken:
                        break;
                    case EndOfTheFileToken:
                        running = false;
                        break;
                    default:
                        running = false;
                        Compiler.Outputs.Add(new ErrorOutput(tokens.Peek().Info,"neocakavany token"));
                        break;
                }
            } while (running);
            return block;
        }

        private SetFontsTypeBlock ParseSetFontsTypeBlock()
        {
            var block = new SetFontsTypeBlock();
            /*var running = true;
        
            do
            {
                break;
            } while (running);*/
            return block;
        }

        private ReplaceTextTypeBlock ParseReplaceTextTypeBlock()
        {
            var block = new ReplaceTextTypeBlock();
            return block;
        }

        private Expression ParseConditionExpr(Expression prev)
        {
            var token = tokens.Peek();
            switch (token)
            {
                case IdentifierToken:
                    if (prev is null or BinExpr)
                    {
                        var invocation = ParseInvocationExpr();
                        return ParseConditionExpr(invocation);
                    }
                    //error
                    return null;
                case NumberLiteralToken:
                    if (prev is null or BinExpr)
                    {
                        var nlx = ParseNumericLiteralExpr();
                        return ParseConditionExpr(nlx);
                    }
                    //error
                    return null;
                case OperatorToken:
                    if (prev is null or BinExpr)
                    {
                        //error
                    }
                    var opex = ParseBinExpr();
                    opex.Left = prev;
                    opex.Right = ParseConditionExpr(opex);
                    return opex;
                case OpenParenToken:
                    tokens.GetToken();
                    return ParseConditionExpr(prev);
                case CloseParenToken:
                    tokens.GetToken();
                    return prev;
                case CommaToken:
                    tokens.GetToken();
                    return prev;
                default:
                    Compiler.Outputs.Add(new ErrorOutput(token.Info, $"Neocakavany token {token}"));
                    return null;
            }
        }

        private InvocationExpr ParseInvocationExpr()
        {
            var ident = tokens.GetToken() as IdentifierToken;
            var expr = new InvocationExpr();
            var identex = new IdentifierNameExpr(ident);
            expr.IdentifierName = identex;
            var next = tokens.PeekNext();
            if (next is OpenParenToken a)
            {
                var argList = new ArgumentList();
                expr.ArgumentList = argList;
                argList.Start = a;
                tokens.Position += 2;
                var next2 = tokens.Peek();
                Argument arg;
                switch (next2)
                {
                    case CloseParenToken t:
                        argList.End = t;
                        break;
                    case IdentifierToken identifier:
                        arg = new Argument();
                        arg.Identifier = new IdentifierNameExpr(identifier);
                        argList.Arguments.Add(arg);
                        break;
                    case StringLiteralToken slt:
                        arg = new Argument();
                        arg.Identifier = new StringLiteralExpr(slt);
                        argList.Arguments.Add(arg);
                        break;
                    case NumberLiteralToken nlt:
                        arg = new Argument();
                        arg.Identifier = new NumberLiteralExpr(nlt);
                        argList.Arguments.Add(arg);
                        break;
                    default:
                        Compiler.Outputs.Add(new ErrorOutput(next2.Info, "Ocakavany identifikator, retazec, cislo alebo prava zatvorka ')'"));
                        break;
                }
            }

            return expr;
        }

        private BinExpr ParseBinExpr()
        {
            var token = tokens.GetToken() as OperatorToken;
            return new BinExpr(token);
        }

        private NumberLiteralExpr ParseNumericLiteralExpr()
        {
            var token = tokens.GetToken() as NumberLiteralToken;
            return new NumberLiteralExpr(token);
        }

        private void ParseSetTextBlock()
        {
            //bool running = true;
            //var tok = tokens.Peek();
            /*while (running)
            {
                switch (tok)
                {
                    
                }
            }*/
        }

        private ConditionStmtsBlock ParseConditionStatements()
        {
            return null;
        }
    }
}
