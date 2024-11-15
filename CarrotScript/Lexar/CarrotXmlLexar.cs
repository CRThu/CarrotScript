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
                // check reader has read to end or not
                while (reader.Peek() != null)
                {
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
                            CloseTagLexar(reader, result);
                            //Flush(result, XML_TAG_END, "</...", reader.Position, reader.Position);
                        }
                        else if (sym2 == QUEST)
                        {
                            // ..< ? ...
                            reader.Read();
                            PiTagLexar(reader, result);
                            //Flush(result, XML_PI_TARGET, "<?...", reader.Position, reader.Position);
                        }
                        else
                        {
                            // ..< . ...
                            TagLexar(reader, result);
                            //Flush(result, XML_TAG_START, "<...", reader.Position, reader.Position);
                        }
                    }
                    else if (sym == SP || sym == TAB || sym == CR || sym == LF)
                    {
                        reader.Read();
                    }
                    else
                    {
                        // ...
                        Flush(result, UNKNOWN, c!.Value.ToString(), reader.Position, reader.Position);
                    }
                }
            }
            return result;
        }

        private void Flush(List<Token> tokens, TokenType tokenType, string content, CodePosition start, CodePosition end)
        {
            tokens.Add(new Token(tokenType, content, new TokenSpan(start, end)));
        }

        public void TagLexar(TokenReader reader, List<Token> tokens)
        {
            // enter method when char is  ..< ...
            var buffer = new StringBuilder();

            // MAYBE OFFSET+1
            CodePosition start = reader.Position;

            buffer.Append("<");

            // check reader has read to end or not
            while (reader.Peek() != null)
            {
                char? c = reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym != GT)
                {
                    // ..< . ...
                    buffer.Append(reader.Read());
                }
                else
                {
                    // <.. > ...
                    buffer.Append(reader.Read());
                    Flush(tokens, XML_TAG_START, buffer.ToString(), start, reader.Position);
                    break;
                }
            }
        }

        public void CloseTagLexar(TokenReader reader, List<Token> tokens)
        {
            // enter method when char is  .</ ...
            var buffer = new StringBuilder();

            // MAYBE OFFSET+2
            CodePosition start = reader.Position;

            buffer.Append("</");

            // check reader has read to end or not
            while (reader.Peek() != null)
            {
                char? c = reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym != GT)
                {
                    // .</ . ...
                    buffer.Append(reader.Read());
                }
                else
                {
                    // </. > ...
                    buffer.Append(reader.Read());
                    Flush(tokens, XML_TAG_END, buffer.ToString(), start, reader.Position);
                    break;
                }
            }
        }

        public void PiTagLexar(TokenReader reader, List<Token> tokens)
        {
            // enter method when char is  .<? ...
            var buffer = new StringBuilder();

            // MAYBE OFFSET+2
            CodePosition start = reader.Position;

            buffer.Append("<?");

            // check reader has read to end or not
            while (reader.Peek() != null)
            {
                char? c = reader.Peek();
                Symbol? sym = c.ToSymbol();

                if (sym != QUEST)
                {
                    // .<? . ...
                    buffer.Append(reader.Read());
                }
                else
                {
                    // <?. ? ...
                    reader.Read();
                    char? c2 = reader.Peek();
                    Symbol? sym2 = c2.ToSymbol();
                    if (sym2 != GT)
                    {
                        // <?. ? ...
                        buffer.Append(c);
                    }
                    else
                    {
                        // <?. ?> ...
                        buffer.Append(c);
                        buffer.Append(reader.Read());
                        Flush(tokens, XML_PI_TARGET, buffer.ToString(), start, reader.Position);
                        break;
                    }

                }
            }
        }
    }
}