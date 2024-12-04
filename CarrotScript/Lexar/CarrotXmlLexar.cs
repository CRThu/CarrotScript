using CarrotScript.Exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.TokenType;
using static CarrotScript.Lang.Def.Symbol;
using CarrotScript.Reader;
using CarrotScript.Lang;

namespace CarrotScript.Lexar
{
    public class CarrotXmlLexar : ILexar
    {
        private TokensReader? Reader { get; set; }
        private List<Token> ResultTokens { get; set; }

        private readonly StringBuilder _buffer;
        private CodePosition _start;
        private CodePosition _end;

        //private Stack<XmlLexarState> ContextStates { get; set; }
        //private XmlLexarState State => ContextStates.Peek();

        public CarrotXmlLexar()
        {
            _buffer = new();
            ResultTokens = new();
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

            _start = Reader.Position;
            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
                if (Reader.CurrentSymbol == CR
                    || Reader.CurrentSymbol == LF)
                {
                    // ... \n ...
                    Reader.Advance();
                    _end = Reader.Position;
                    Flush(XML_CONTENT);
                    break;
                }
                else if (Reader.CurrentSymbol == LT)
                {
                    // ... < ...
                    _end = Reader.Position;
                    Flush(XML_CONTENT);
                    break;
                }
                else
                {
                    // ... .  ...
                    _buffer.Append(Reader.CurrentChar);
                    Reader.Advance();
                }
            }
            _end = Reader.Position;
            Flush(XML_CONTENT);
        }

        public void OpeningTagLexar()
        {
            // enter method after char  ..< ...
            if (Reader == null)
                return;

            _start = Reader.Position;

            // ... < ...
            Reader.Expect(LT);

            // ... < name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            _buffer.Append(name);

            _end = Reader.Position;
            Flush(XML_OPEN_TAG);

            // ... < name \s...
            Reader.ParseWhile((S) => S == SP);

            // ... < name attrx='valx' ...
            AttributesLexar();

            if (Reader.CurrentSymbol == DIV)
            {
                _start = Reader.Position;

                // ... < name attrx='valx' / ...
                Reader.Expect(DIV);

                _end = Reader.Position;
                Flush(XML_CLOSE_TAG, name);
            }

            // ... < name attrx='valx' /> ...
            // ... < name attrx='valx' > ...
            Reader.Expect(GT);
        }

        public void ClosingTagLexar()
        {
            // enter method after char  .</ ...
            if (Reader == null)
                return;

            _start = Reader.Position;

            // ... </ . ...
            Reader.Expect(LT);
            Reader.Expect(DIV);

            // ... </ name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            _buffer.Append(name);

            // ... </ name > ...
            Reader.Expect(GT);

            _end = Reader.Position;
            Flush(XML_CLOSE_TAG);
        }

        public void PiTagLexar()
        {
            // enter method after char  .<? ...
            if (Reader == null)
                return;

            _start = Reader.Position;

            // ... <? . ...
            Reader.Expect(LT);
            Reader.Expect(QUEST);

            // ... <? name ...
            var name = Reader.ParseWhile(Def.IsLangDefNameChar);
            _buffer.Append(name);

            _end = Reader.Position;
            Flush(XML_OPEN_PI_TARGET);

            // ... < name \s...
            Reader.ParseWhile((S) => S == SP);

            // ... <? name attrx='valx' ...
            AttributesLexar();

            // ... <? name attrx='valx' ?>...
            _start = Reader.Position;

            Reader.Expect(QUEST);
            Reader.Expect(GT);

            _end = Reader.Position;
            Flush(XML_CLOSE_PI_TARGET, name);
        }

        public void AttributesLexar()
        {
            // enter method after char ... < name ...
            // enter method after char ... <? name ...
            if (Reader == null)
                return;

            // check reader has read to end or not
            while (Reader.CurrentChar != null)
            {
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
                _start = Reader.Position;
                var attrNameSpan = Reader.ParseWhile(Def.IsLangDefNameChar);
                _buffer.Append(attrNameSpan);
                _end = Reader.Position;
                Flush(XML_ATTR_NAME);

                // ... <  name attrx= ...
                // ... <? name attrx= ...
                Reader.Expect(EQ);

                // ... <  name attrx=' ...
                // ... <? name attrx=' ...
                //Reader.Expect(QUOT);
                Reader.ParseWhile((S) => S == QUOT);

                // ... <  name attrx='valx ...
                // ... <? name attrx='valx ...
                _start = Reader.Position;
                var attrValSpan = Reader.ParseWhile(Def.IsLangDefNameChar);
                _buffer.Append(attrValSpan);
                _end = Reader.Position;
                Flush(XML_ATTR_VALUE);

                // ... <  name attrx='valx' ...
                // ... <? name attrx='valx' ...
                //Reader.Expect(QUOT);
                Reader.ParseWhile((S) => S == QUOT);
            }

            return;
        }
    }
}