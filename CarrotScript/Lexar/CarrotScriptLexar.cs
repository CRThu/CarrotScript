using CarrotScript.Reader;
using System.Text;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.TokenType;
using static CarrotScript.Lang.Def.Symbol;

namespace CarrotScript.Lexar
{
    public class CarrotScriptLexar : ILexar
    {
        private TokenReader? Reader { get; set; }
        private List<Token> ResultTokens { get; set; }
        private StringBuilder Buffer { get; set; }
        private CodePosition Start { get; set; }
        private CodePosition End { get; set; }

        //private Stack<XmlLexarState> ContextStates { get; set; }
        //private XmlLexarState State => ContextStates.Peek();

        private void Flush(TokenType tokenType)
        {
            if (Buffer.Length != 0)
            {
                ResultTokens.Add(new Token(tokenType, Buffer.ToString(), new TokenSpan(Start, End)));
                Buffer.Clear();
            }
        }
        public IEnumerable<Token> Tokenize(IEnumerable<Token> inputTokens)
        {
            foreach (Token inputToken in inputTokens)
            {
                RootLexar(inputToken);
            }
            return ResultTokens;
        }

        public void RootLexar(Token token)
        {
            Reader = new TokenReader(token);
            // check reader has read to end or not
            while (Reader != null && Reader.CurrentChar != null)
            {
                switch (token.Type)
                {
                    case XML_SINGLE_TAG:
                        // <main/>
                        throw new NotImplementedException();
                        break;
                    case XML_OPEN_TAG:
                        // <main>
                        throw new NotImplementedException();
                        break;
                    case XML_CLOSE_TAG:
                        // </main>
                        throw new NotImplementedException();
                        break;
                    case XML_CONTENT:
                        // CONTENT;
                        ParseContent();
                        break;
                    case XML_PI_TARGET:
                        // <?def a=1?>
                        ParsePiTarget();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ParseContent()
        {
            // enter method after char  ..> ...
            if (Reader == null)
                return;

            Start = Reader.Position;
            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == LCUB)
                {
                    // Flush Text Before {
                    End = Reader.Position;
                    Flush(TEXT);

                    // ... { ...
                    Start = Reader.Position;
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    End = Reader.Position;
                    Flush(LBRACE);

                    // ... { ...
                    //var expr = Reader.ParseWhile(S => S != Symbol.RCUB);
                    TokenizeExpr();

                    // ... { ... } 
                    Start = Reader.Position;
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    End = Reader.Position;
                    Flush(RBRACE);

                    Start = Reader.Position;
                }
                else
                {
                    // ... .  ...
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                }
            }

            // Flush Text Before {
            End = Reader.Position;
            Flush(TEXT);
        }

        private void ParsePiTarget()
        {
            throw new NotImplementedException();
        }

        private void TokenizeExpr()
        {
            // check reader has read to end or not
            while (Reader != null && Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == RCUB)
                {
                    // { ... }
                    break;
                }
                else if (Reader.CurrentChar.Value.IsLangDefIdentifierStartChar())
                {
                    // { ... a
                    Start = Reader.Position;
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();

                    var identifierChars = Reader.ParseWhile(C => C.IsLangDefIdentifierChar());
                    Buffer.Append(identifierChars);
                    End = Reader.Position;
                    Flush(IDENTIFIER);
                }
                else if (Reader.CurrentChar.Value.IsLangDefNumberStartChar())
                {
                    // { ... 123
                    Start = Reader.Position;
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();

                    var numberChars = Reader.ParseWhile(C => C.IsLangDefNumberChar());
                    Buffer.Append(numberChars);
                    End = Reader.Position;
                    Flush(NUMBER);
                }
                else if(Reader.CurrentChar.Value.IsLangDefOperator())
                {

                }
                else
                {

                }
            }
        }
    }
}