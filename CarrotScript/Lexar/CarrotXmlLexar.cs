using CarrotScript.Exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.TokenType;
using static CarrotScript.Lang.Def.Symbol;
using CarrotScript.Reader;

namespace CarrotScript.Lexar
{
    public class CarrotXmlLexar : ILexar
    {
        private TokenReader? Reader { get; set; }
        private List<Token> ResultTokens { get; set; }
        private StringBuilder Buffer { get; set; }
        private CodePosition Start { get; set; }
        private CodePosition End { get; set; }

        public CarrotXmlLexar()
        {
            Buffer = new();
            ResultTokens = new();
        }

        public IEnumerable<Token> Tokenize(IEnumerable<Token> inputTokens)
        {
            foreach (Token inputToken in inputTokens)
            {
                Reader = new TokenReader(inputToken);
                ContentLexar();
            }
            return ResultTokens;
        }

        private void Append(char? c)
        {
            if (Reader != null && c != null)
            {
                if (Buffer.Length == 0)
                {
                    Start = Reader.LastPosition;
                }
                End = Reader.LastPosition;
                Buffer.Append(c);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void Flush(TokenType tokenType)
        {
            ResultTokens.Add(new Token(tokenType, Buffer.ToString(), new TokenSpan(Start, End)));
            Buffer.Clear();
        }

        public void ContentLexar()
        {
            // enter method after char  ..> ...

            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                char? c = Reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym == LT)
                {
                    // ... < ...
                    char? c1 = Reader.Read();
                    Append(c1);

                    Symbol? sym2 = Reader.Peek().ToSymbol();
                    if (sym2 == DIV)
                    {
                        // ..< / ...
                        char? c2 = Reader.Read();
                        Append(c2);
                        ClosingTagLexar();
                    }
                    else if (sym2 == QUEST)
                    {
                        // ..< ? ...
                        char? c2 = Reader.Read();
                        Append(c2);
                        PiTagLexar();
                    }
                    else
                    {
                        // ..< . ...
                        OpeningTagLexar();
                    }
                }
                else if (sym == SP || sym == TAB || sym == CR || sym == LF)
                {
                    // ..< \s ...
                    Reader.Read();
                }
                else
                {
                    // ..< . ...
                    Flush(UNKNOWN);
                }
            }
        }

        public void OpeningTagLexar()
        {
            // enter method after char  ..< ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                char? c1 = Reader.Read();
                Symbol? sym = c1.ToSymbol();
                Append(c1);
                char? c2 = Reader.Peek();
                Symbol? sym2 = c2.ToSymbol();


                if (sym == GT)
                {
                    // <.. > ...
                    Flush(XML_OPEN_TAG);
                    break;
                }
                else if(sym == DIV && sym2 == GT)
                {
                    // <.. /> ...
                    Append(Reader.Read());
                    Flush(XML_SINGLE_TAG);
                }
                else
                {
                    // ..< . ...
                }
            }
        }

        public void ClosingTagLexar()
        {
            // enter method after char  .</ ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                char? c = Reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym != GT)
                {
                    // .</ . ...
                    Append(Reader.Read());
                }
                else
                {
                    // </. > ...
                    Append(Reader.Read());
                    Flush(XML_CLOSE_TAG);
                    break;
                }
            }
        }

        public void PiTagLexar()
        {
            // enter method after char  .<? ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                char? c = Reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym != QUEST)
                {
                    // .<? . ...
                    Append(Reader.Read());
                }
                else
                {
                    // <?. ? ...
                    char? c1 = Reader.Read();
                    Append(c1);

                    char? c2 = Reader.Peek();
                    Symbol? sym2 = c2.ToSymbol();
                    if (sym2 != GT)
                    {
                        // <?. ? ...
                    }
                    else
                    {
                        // <?. ?> ...
                        Append(Reader.Read());
                        Flush(XML_PI_TARGET);
                        break;
                    }

                }
            }
        }
    }
}