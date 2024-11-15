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
        public IEnumerable<Token> Tokenize(IEnumerable<Token> tokens)
        {
            List<Token> result = new List<Token>();
            foreach (Token token in tokens)
            {
                TokenReader reader = new TokenReader(token);
                while (reader.Peek() != null)
                {
                    //result.Add(new Token(TokenType.UNKNOWN, reader.Read().ToString(), new TokenSpan(reader.Position, reader.Position)));
                    char? c = reader.Read();
                    Symbol? sym = c.ToSymbol();

                    if (sym == LT)
                    {
                        // ... < ...
                        Symbol? sym2 = reader.Peek().ToSymbol();
                        if (sym2 == DIV)
                        {
                            // ..< / ...
                            reader.Read();
                            Flush(result, XML_TAG_END, "</...", reader.Position, reader.Position);

                        }
                        else if (sym2 == QUEST)
                        {
                            // ..< ? ...
                            reader.Read();
                            Flush(result, XML_PI_TARGET, "<?...", reader.Position, reader.Position);
                        }
                        else
                        {
                            // ..< . ...
                            Flush(result, XML_TAG_START, "<...", reader.Position, reader.Position);
                        }
                    }
                    else
                    {
                        // ...
                        Flush(result, UNKNOWN, c!.Value.ToString(), reader.Position, reader.Position);
                    }
                }
            }
            /*
            char? c = lex.Reader.Peek();
            bool hasToken = SymbolDict.TryGetValue(c!.ToString(), out Symbol tok);
            if (!hasToken)
            {
                tok = CHAR;
            }
            switch (lex.CurrentState)
            {
                case XmlState.XmlContent:
                    switch (tok)
                    {
                        // ... < ...
                        case LT:
                            if (lex.Context.ContainsKey("XmlContent.StartPosition"))
                            {
                                lex.CreateToken(XML_CONTENT,
                                    (CodePosition)lex.Context["XmlContent.StartPosition"],
                                    (CodePosition)lex.Context["XmlContent.EndPosition"]);
                                lex.Context.Remove("XmlContent.StartPosition");
                            }
                            lex.CurrentState = XmlState.XmlTagBegin;
                            break;
                        case SP:
                        case TAB:
                            lex.Context["XmlContent.EndPosition"] = lex.Reader.Position;
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        case CR:
                        case LF:
                            if (lex.Context.ContainsKey("XmlContent.StartPosition"))
                            {
                                lex.CreateToken(XML_CONTENT,
                                    (CodePosition)lex.Context["XmlContent.StartPosition"],
                                    (CodePosition)lex.Context["XmlContent.EndPosition"]);
                                lex.Context.Remove("XmlContent.StartPosition");
                            }
                            else
                            {
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.Position;
                            }
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlContent.StartPosition"))
                            {
                                lex.Context["XmlContent.StartPosition"] = lex.Reader.Position;
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.Position;
                            }
                            else
                            {
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.Position;
                            }
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                    }
                    break;
                case XmlState.XmlTagBegin:
                    switch (tok)
                    {
                        // ..< / ...
                        case DIV:
                            lex.Context["IsEndTag"] = true;
                            lex.CurrentState = XmlState.XmlTagName;
                            break;
                        // ..< ? ...
                        case QUEST:
                            lex.Context["IsPiTag"] = true;
                            lex.CurrentState = XmlState.XmlPiTagName;
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.Position;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            lex.CurrentState = XmlState.XmlTagName;
                            break;
                    }
                    break;
                case XmlState.XmlTagName:
                    switch (tok)
                    {
                        // <.. > ...
                        case GT:
                            lex.CreateToken(
                                lex.Context.ContainsKey("IsEndTag")
                                    ? XML_TAG_END : XML_TAG_START,
                                (CodePosition)lex.Context["XmlTagName.StartPosition"],
                                (CodePosition)lex.Context["XmlTagName.EndPosition"]);
                            lex.Context.Remove("XmlTagName.StartPosition");
                            lex.Context.Remove("IsEndtag");
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.Position;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            lex.CurrentState = XmlState.XmlTagName;
                            break;
                    }
                    break;
                case XmlState.XmlPiTagName:
                    switch (tok)
                    {
                        // <?.. ? ...
                        case QUEST:
                            lex.Context["XmlPiTagName.MaybeTagEnd"] = true;
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.Position;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            lex.CurrentState = XmlState.XmlPiTagName;
                            break;
                        case GT:
                            if (lex.Context.ContainsKey("XmlPiTagName.MaybeTagEnd"))
                            {
                                lex.CreateToken(XML_PI_TARGET,
                                    (CodePosition)lex.Context["XmlTagName.StartPosition"],
                                    (CodePosition)lex.Context["XmlTagName.EndPosition"]);
                                lex.Context.Remove("XmlTagName.StartPosition");
                                lex.Context.Remove("XmlPiTagName.MaybeTagEnd");
                                lex.CurrentState = XmlState.XmlContent;
                            }
                            else
                            {
                                if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                                {
                                    lex.Context["XmlTagName.StartPosition"] = lex.Reader.Position;
                                    lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                                }
                                else
                                {
                                    lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                                }
                                lex.Context["XmlPiTagName.MaybeTagEnd"] = false;
                                lex.CurrentState = XmlState.XmlPiTagName;
                            }
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.Position;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.Position;
                            }
                            lex.Context["XmlPiTagName.MaybeTagEnd"] = false;
                            lex.CurrentState = XmlState.XmlPiTagName;
                            break;
                    }
                    break;
                default:
                    break;
            }
            */
            return result;
        }

        private void Flush(List<Token> tokens, TokenType tokenType, string content, CodePosition start, CodePosition end)
        {
            tokens.Add(new Token(tokenType, content, new TokenSpan(start, end)));
        }

        public void TagLexar(TokenReader reader, List<Token> tokens)
        {
            // TODO
        }
    }
}