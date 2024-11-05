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

namespace CarrotScript.Lexar
{
    public class CarrotXmlScanner
    {
        public static bool Scan(Lexar lex)
        {
            char c = lex.Reader.GetChar();
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
                                    (TokenPosition)lex.Context["XmlContent.StartPosition"],
                                    (TokenPosition)lex.Context["XmlContent.EndPosition"]);
                                lex.Context.Remove("XmlContent.StartPosition");
                            }
                            lex.CurrentState = XmlState.XmlTagBegin;
                            break;
                        case SP:
                        case TAB:
                            lex.Context["XmlContent.EndPosition"] = lex.Reader.CurrentPosition;
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        case CR:
                        case LF:
                            if (lex.Context.ContainsKey("XmlContent.StartPosition"))
                            {
                                lex.CreateToken(XML_CONTENT,
                                    (TokenPosition)lex.Context["XmlContent.StartPosition"],
                                    (TokenPosition)lex.Context["XmlContent.EndPosition"]);
                                lex.Context.Remove("XmlContent.StartPosition");
                            }
                            else
                            {
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlContent.StartPosition"))
                            {
                                lex.Context["XmlContent.StartPosition"] = lex.Reader.CurrentPosition;
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            else
                            {
                                lex.Context["XmlContent.EndPosition"] = lex.Reader.CurrentPosition;
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
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.CurrentPosition;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
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
                                (TokenPosition)lex.Context["XmlTagName.StartPosition"],
                                (TokenPosition)lex.Context["XmlTagName.EndPosition"]);
                            lex.Context.Remove("XmlTagName.StartPosition");
                            lex.Context.Remove("IsEndtag");
                            lex.CurrentState = XmlState.XmlContent;
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.CurrentPosition;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
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
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.CurrentPosition;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            lex.CurrentState = XmlState.XmlPiTagName;
                            break;
                        case GT:
                            if (lex.Context.ContainsKey("XmlPiTagName.MaybeTagEnd"))
                            {
                                lex.CreateToken(XML_PI_TARGET,
                                    (TokenPosition)lex.Context["XmlTagName.StartPosition"],
                                    (TokenPosition)lex.Context["XmlTagName.EndPosition"]);
                                lex.Context.Remove("XmlTagName.StartPosition");
                                lex.Context.Remove("XmlPiTagName.MaybeTagEnd");
                                lex.CurrentState = XmlState.XmlContent;
                            }
                            else
                            {
                                if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                                {
                                    lex.Context["XmlTagName.StartPosition"] = lex.Reader.CurrentPosition;
                                    lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                                }
                                else
                                {
                                    lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                                }
                                lex.Context["XmlPiTagName.MaybeTagEnd"] = false;
                                lex.CurrentState = XmlState.XmlPiTagName;
                            }
                            break;
                        default:
                            if (!lex.Context.ContainsKey("XmlTagName.StartPosition"))
                            {
                                lex.Context["XmlTagName.StartPosition"] = lex.Reader.CurrentPosition;
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            else
                            {
                                lex.Context["XmlTagName.EndPosition"] = lex.Reader.CurrentPosition;
                            }
                            lex.Context["XmlPiTagName.MaybeTagEnd"] = false;
                            lex.CurrentState = XmlState.XmlPiTagName;
                            break;
                    }
                    break;
                default:
                    break;
            }

            if (!lex.Reader.Advance())
            {
                return false;
            }
            return true;
        }
    }
}