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
using CarrotScript.Lang;

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

        private void Flush(TokenType tokenType, IDictionary<string, string>? attributes = null)
        {
            if (Buffer.Length != 0)
            {
                ResultTokens.Add(new Token(tokenType, Buffer.ToString(), new TokenSpan(Start, End), attributes));
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
            while (Reader != null && Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == LT)
                {
                    if (Reader.NextSymbol == DIV)
                    {
                        // ... </ ...
                        ClosingTagLexar();
                    }
                    else if (Reader.NextSymbol == QUEST)
                    {
                        // ... <? ...
                        PiTagLexar();
                    }
                    else
                    {
                        // ..< . ...
                        OpeningTagLexar();
                    }
                }
                else if (Reader.CurrentSymbol == SP
                    || Reader.CurrentSymbol == TAB
                    || Reader.CurrentSymbol == CR
                    || Reader.CurrentSymbol == LF)
                {
                    // ..< \s ...
                    Reader.Advance();
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
            if (Reader == null)
                return;

            Start = Reader.Position;
            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == CR
                    || Reader.CurrentSymbol == LF)
                {
                    // ... \n ...
                    Reader.Advance();
                    End = Reader.Position;
                    Flush(XML_CONTENT);
                    break;
                }
                else if (Reader.CurrentSymbol == LT)
                {
                    // ... < ...
                    End = Reader.Position;
                    Flush(XML_CONTENT);
                    break;
                }
                else
                {
                    // ... .  ...
                    Buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                }
            }
            End = Reader.Position;
            Flush(XML_CONTENT);
        }

        public void OpeningTagLexar()
        {
            // enter method after char  ..< ...
            if (Reader == null)
                return;

            Start = Reader.Position;

            // ... < ...
            Reader.Expect(LT);

            // ... < name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            Buffer.Append(name);

            // ... < name \s...
            Reader.ParseWhile((S) => S == SP);

            // ... < name attrx='valx' ...
            var attrs = AttributesLexar();

            TokenType tokenType = XML_OPEN_TAG;
            if (Reader.CurrentSymbol == DIV)
            {
                // ... < name attrx='valx' / ...
                Reader.Expect(DIV);
                tokenType = XML_SINGLE_TAG;
            }

            // ... < name attrx='valx' /> ...
            // ... < name attrx='valx' > ...
            Reader.Expect(GT);
            End = Reader.Position;
            Flush(tokenType, attrs);
        }

        public void ClosingTagLexar()
        {
            // enter method after char  .</ ...
            if (Reader == null)
                return;

            Start = Reader.Position;

            // ... </ . ...
            Reader.Expect(LT);
            Reader.Expect(DIV);

            // ... </ name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            Buffer.Append(name);

            // ... </ name > ...
            Reader.Expect(GT);

            End = Reader.Position;
            Flush(XML_CLOSE_TAG);
        }

        public void PiTagLexar()
        {
            // enter method after char  .<? ...
            if (Reader == null)
                return;

            Start = Reader.Position;

            // ... <? . ...
            Reader.Expect(LT);
            Reader.Expect(QUEST);

            // ... <? name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            Buffer.Append(name);

            // ... < name \s...
            Reader.ParseWhile((S) => S == SP);

            // ... <? name attrx='valx' ...
            var attrs = AttributesLexar();

            // ... <? name attrx='valx' ?>...
            Reader.Expect(QUEST);
            Reader.Expect(GT);

            End = Reader.Position;
            Flush(XML_PI_TARGET, attrs);
        }

        public IDictionary<string, string> AttributesLexar()
        {
            Dictionary<string, string> attributes = new();

            // enter method after char ... < name ...
            // enter method after char ... <? name ...
            if (Reader == null)
                return attributes;

            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
                StringBuilder attrName = new(), attrValue = new();

                // ... < name \s...
                Reader.ParseWhile((S) => S == SP);

                if (Reader.CurrentSymbol == QUEST
                    || Reader.CurrentSymbol == DIV
                    || Reader.CurrentSymbol == GT)
                {
                    break;
                }

                // ... <  name attrx ...
                // ... <? name attrx ...
                var attrNameSpan = Reader.ParseWhile(Def.IsLangDefNameChar);
                attrName.Append(attrNameSpan);

                // ... <  name attrx= ...
                // ... <? name attrx= ...
                Reader.Expect(EQ);

                // ... <  name attrx=' ...
                // ... <? name attrx=' ...
                //Reader.Expect(QUOT);
                Reader.ParseWhile((S) => S == QUOT);

                // ... <  name attrx='valx ...
                // ... <? name attrx='valx ...
                var attrValSpan = Reader.ParseWhile(Def.IsLangDefNameChar);
                attrValue.Append(attrValSpan);

                // ... <  name attrx='valx' ...
                // ... <? name attrx='valx' ...
                //Reader.Expect(QUOT);
                Reader.ParseWhile((S) => S == QUOT);

                attributes.Add(attrName.ToString(), attrValue.ToString());
            }

            return attributes;
        }
    }
}