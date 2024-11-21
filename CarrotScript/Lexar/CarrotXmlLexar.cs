using CarrotScript.Exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.XmlLexarState;
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
        //private Stack<XmlLexarState> ContextStates { get; set; }
        //private XmlLexarState State => ContextStates.Peek();

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
                RootLexar();
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
            if (Buffer.Length != 0)
            {
                ResultTokens.Add(new Token(tokenType, Buffer.ToString(), new TokenSpan(Start, End)));
                Buffer.Clear();
            }
        }

        /*
        private void ChangeState(XmlLexarState state)
        {
            ContextStates.Push(state);
        }

        private void RestoreState()
        {
            if (ContextStates.Count > 1)
            {
                ContextStates.Pop();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        */

        public void RootLexar()
        {
            // enter method after char  ..> ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                Symbol? sym1 = Reader.Peek().ToSymbol();
                if (sym1 == LT)
                {
                    Append(Reader.Read());
                    Symbol? sym2 = Reader.Peek().ToSymbol();
                    if (sym2 == DIV)
                    {
                        // ... </ ...
                        Append(Reader.Read());
                        ClosingTagLexar();
                    }
                    else if (sym2 == QUEST)
                    {
                        // ... <? ...
                        Append(Reader.Read());
                        Append(Reader.Read());
                        PiTagLexar();
                    }
                    else
                    {
                        // ..< . ...
                        Append(Reader.Read());
                        OpeningTagLexar();
                    }
                }
                else if (sym1 == SP || sym1 == TAB || sym1 == CR || sym1 == LF)
                {
                    // ..< \s ...
                    Reader.Read();
                }
                else
                {
                    // ..< . ...
                    ContentLineLexar();
                }
            }
        }

        public void ContentLineLexar()
        {
            // enter method after char  ..> ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                Symbol? sym1 = Reader.Peek().ToSymbol();

                if (sym1 == CR || sym1 == LF)
                {
                    // ... \n ...
                    Reader.Read();
                    Flush(XML_CONTENT);
                    break;
                }
                else if (sym1 == GT)
                {
                    // ... < ...
                    break;
                }
                else
                {
                    // ... .  ...
                    Append(Reader.Read());
                }
            }
        }

        public void OpeningTagLexar()
        {
            // enter method after char  ..< ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                Symbol? sym1 = Reader.Peek().ToSymbol();

                if (sym1 == GT)
                {
                    // <.. > ...
                    Append(Reader.Read());
                    Flush(XML_OPEN_TAG);
                    break;
                }
                else if (sym1 == DIV)
                {
                    Append(Reader.Read());
                    Symbol? sym2 = Reader.Peek().ToSymbol();
                    if (sym2 == GT)
                    {
                        // <.. /> ...
                        Append(Reader.Read());
                        Flush(XML_SINGLE_TAG);
                        break;
                    }
                    else
                    {
                        Append(Reader.Read());
                    }
                }
                else if (sym1 == SP || sym1 == TAB)
                {
                    // <.. \s ...
                    Append(Reader.Read());
                }
                else if (sym1 == EQ)
                {
                    // <.. \s ... = ...
                    Append(Reader.Read());
                }
                else
                {
                    // ..< . ...
                    Append(Reader.Read());
                }
            }
        }

        public void ClosingTagLexar()
        {
            // enter method after char  .</ ...
            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                Symbol? sym1 = Reader.Peek().ToSymbol();
                Append(Reader.Read());

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
                Symbol? sym1 = Reader.Peek().ToSymbol();
                if (sym1 == QUEST)
                {
                    Append(Reader.Read());
                    Symbol? sym2 = Reader.Peek().ToSymbol();

                    if (sym2 == GT)
                    {
                        // <?. ?> ...
                        Append(Reader.Read());
                        Flush(XML_PI_TARGET);
                        break;
                    }
                    else
                    {

                    }
                }
                else
                {
                    // .<? . ...
                    Append(Reader.Read());
                }
            }
        }

        public IDictionary<string, string> AttributesLexar()
        {
            Dictionary<string, string> attrs = new();
            StringBuilder name = new(), value = new();

            // check reader has read to end or not
            while (Reader != null && Reader.Peek() != null)
            {
                Symbol? sym1 = Reader.Peek().ToSymbol();
                if (sym1 == QUEST || sym1 == DIV || sym1 == GT)
                {
                    break;
                }
                else if (sym1 == SP)
                {
                    Reader.Read();
                }
                else
                {

                }
            }

            return attrs;
        }
    }
}