using CarrotScript.Reader;
using System.Text;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.TokenType;
using static CarrotScript.Lang.Def.Symbol;
using CarrotScript.Exception;

namespace CarrotScript.Lexar
{
    public class CarrotScriptLexar : ILexar
    {
        private TokensReader? Reader { get; set; }
        private List<Token> ResultTokens { get; set; }

        private readonly StringBuilder _buffer;
        private CodePosition _start;
        private CodePosition _end;

        //private Stack<XmlLexarState> ContextStates { get; set; }
        //private XmlLexarState State => ContextStates.Peek();

        public CarrotScriptLexar()
        {
            _buffer = new();
            ResultTokens = new();
        }

        private void FlushCurrentToken(TokenType tokenType)
        {
            if (Reader != null && Reader.CurrentToken != null)
            {
                ResultTokens.Add(new Token(tokenType, Reader.CurrentToken.Value, Reader.CurrentToken.Span));
            }
        }

        private void Flush(TokenType tokenType)
        {
            Flush(tokenType, _buffer.ToString());
            _buffer.Clear();
        }

        private void Flush(TokenType tokenType, ReadOnlySpan<char> value)
        {
            if (value.Length != 0)
            {
                ResultTokens.Add(new Token(tokenType, value.ToString(), new TokenSpan(_start, _end)));
            }
        }

        public IEnumerable<Token> Tokenize(IEnumerable<Token> inputTokens)
        {
            Reader = new TokensReader(inputTokens);
            while (!Reader.IsAtEnd)
            {
                RootLexar();
                Reader.AdvanceToken();
            }
            return ResultTokens;
        }

        public void RootLexar()
        {
            // check reader has read to end or not
            while (Reader != null && Reader.CurrentChar != null)
            {
                switch (Reader.CurrentToken.Type)
                {
                    case XML_OPEN_TAG:
                        // <main>
                        // TODO
                        Reader.AdvanceToken();
                        break;
                    case XML_CLOSE_TAG:
                        // </main>
                        // TODO
                        Reader.AdvanceToken();
                        break;
                    case XML_CONTENT:
                        // CONTENT;
                        ParseContent();
                        break;
                    case XML_OPEN_PI_TARGET:
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

            _start = Reader.Position;
            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == LCUB)
                {
                    // Flush Text Before {
                    _end = Reader.Position;
                    Flush(TEXT);

                    // ... { ...
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    _end = Reader.Position;
                    Flush(LBRACE);

                    // ... { ...
                    //var expr = Reader.ParseWhile(S => S != Symbol.RCUB);
                    ParseExpression();

                    // ... { ... } 
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    _end = Reader.Position;
                    Flush(RBRACE);

                    _start = Reader.Position;
                }
                else
                {
                    // ... .  ...
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                }
            }

            // Flush Text Before {
            _end = Reader.Position;
            Flush(TEXT);
            // Add TEXT_END Symbol
            ResultTokens.Add(new Token(TEXT_END, "", new TokenSpan(Reader.Position, Reader.Position)));
        }

        private void ParsePiTarget()
        {
            // enter method after char  ..> ...
            if (Reader == null || Reader.CurrentToken == null)
                return;

            // <?target
            var target = Reader.CurrentToken.Value;
            switch (target)
            {
                case "carrotxml":
                    // TODO
                    ParseInfo();
                    break;
                case "def":
                    ParseAssignment();
                    break;
                default:
                    throw new InvalidSyntaxException(Reader.Position);
                    break;
            }

            Reader.AdvanceToken();

        }

        private void ParseInfo()
        {
            if (Reader == null || Reader.CurrentToken == null)
            {
                return;
            }

            // <?carrotxml
            Reader.AdvanceToken();

            while (Reader != null && Reader.CurrentToken != null
                    && (Reader.CurrentToken.Type == XML_ATTR_NAME
                        || Reader.CurrentToken.Type == XML_ATTR_VALUE))
            {
                Reader.AdvanceToken();
            }
        }

        private void ParseAssignment()
        {
            if (Reader == null || Reader.CurrentToken == null)
            {
                return;
            }

            // <?def
            FlushCurrentToken(ASSIGNMENT);
            Reader.AdvanceToken();

            while (Reader != null && Reader.CurrentToken != null)
            {
                // <?def name1
                // <?def name1=value1 name2
                if (Reader.CurrentToken.Type == XML_ATTR_NAME)
                {
                    FlushCurrentToken(IDENTIFIER);
                    Reader.AdvanceToken();
                }
                // <?def name1=value1
                // <?def name1=value1 name2=value2
                else if (Reader.CurrentToken.Type == XML_ATTR_VALUE)
                {
                    ParseExpression();
                    Reader.AdvanceToken();
                }
                else
                {
                    break;
                }
            }
        }

        private void ParseExpression()
        {
            // check reader has read to end or not
            while (Reader != null && Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == RCUB
                    || Reader.CurrentSymbol == RP)
                {
                    // { ... }
                    // { ... ( ... )
                    break;
                }
                else if (Reader.CurrentSymbol == SP)
                {
                    Reader.Advance();
                }
                else if (Reader.CurrentSymbol == LP)
                {
                    // { ... (
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    _end = Reader.Position;
                    Flush(LPAREN);

                    // { ... ( ...
                    ParseExpression();

                    // { ... ( ... )
                    _start = Reader.Position;
                    var sym = Reader.Expect(RP);
                    _buffer.Append(sym);
                    _end = Reader.Position;
                    Flush(RPAREN);
                }
                else if (Reader.CurrentChar.Value.IsLangDefIdentifierStartChar())
                {
                    // { ... a
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();

                    var identifierChars = Reader.ParseWhile(C => C.IsLangDefIdentifierChar());
                    _buffer.Append(identifierChars);
                    _end = Reader.Position;
                    Flush(IDENTIFIER);
                }
                else if (Reader.CurrentChar.Value.IsLangDefNumberStartChar())
                {
                    // { ... 123
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();

                    var numberChars = Reader.ParseWhile(C => C.IsLangDefNumberChar());
                    _buffer.Append(numberChars);
                    _end = Reader.Position;
                    Flush(NUMBER);
                }
                else if (Reader.CurrentChar.Value.IsLangDefOperatorChar())
                {
                    // { ... +
                    _start = Reader.Position;
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                    _end = Reader.Position;
                    Flush(OPERATOR);
                }
                else
                {
                    throw new InvalidSyntaxException(Reader.Position);
                }
            }
        }
    }
}