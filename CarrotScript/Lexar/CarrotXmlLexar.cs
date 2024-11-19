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
                char? c1 = Reader.Read();
                Symbol? sym1 = c1.ToSymbol();
                Symbol? sym2 = Reader.Peek().ToSymbol();

                if (sym1 == LT && sym2 == DIV)
                {
                    // ... </ ...
                    Append(c1);
                    Append(Reader.Read());
                    ClosingTagLexar();
                }
                else if (sym1 == LT && sym2 == QUEST)
                {
                    // ... <? ...
                    Append(c1);
                    Append(Reader.Read());
                    PiTagLexar();
                }
                else if (sym1 == LT)
                {
                    // ..< . ...
                    Append(c1);
                    OpeningTagLexar();
                }
                else if (sym1 == SP || sym1 == TAB || sym1 == CR || sym1 == LF)
                {
                    // ..< \s ...
                }
                else
                {
                    // ..< . ...
                    Append(c1);
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
                else if (sym == DIV && sym2 == GT)
                {
                    // <.. /> ...
                    Append(Reader.Read());
                    Flush(XML_SINGLE_TAG);
                    break;
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
                char? c1 = Reader.Read();
                Symbol? sym1 = c1.ToSymbol();
                Append(c1);

                if (sym1 == GT)
                {
                    // </. > ...
                    Flush(XML_CLOSE_TAG);
                    break;
                }
                else
                {
                    // .</ . ...
                }
            }
        }

        public void PiTagLexar()
        {
            // enter method after char  .<? ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                char? c1 = Reader.Read();
                Symbol? sym1 = c1.ToSymbol();
                Append(c1);
                char? c2 = Reader.Peek();
                Symbol? sym2 = c2.ToSymbol();

                if (sym1 == QUEST && sym2 == GT)
                {
                    // <?. ?> ...
                    Append(Reader.Read());
                    Flush(XML_PI_TARGET);
                    break;
                }
                else
                {
                    // .<? . ...
                }
            }
        }
    }
}