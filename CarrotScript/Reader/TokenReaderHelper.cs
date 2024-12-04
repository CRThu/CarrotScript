using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Reader
{
    public static class TokenReaderHelper
    {
        public static char? Expect(this ITokensReader reader, Symbol expected)
        {
            if (reader == null)
                throw new ArgumentNullException("Reader is null.");
            else if (reader.CurrentSymbol != expected)
                throw new InvalidSyntaxException(reader.Position);
            else
                return reader.Advance();
        }

        public static ReadOnlySpan<char> ParseWhile(this ITokensReader reader, Func<char, bool> condition)
        {
            if (reader == null)
                throw new ArgumentNullException("Reader is null.");
            else if (reader.CurrentChar == null)
            {
                return ReadOnlySpan<char>.Empty;
                //throw new ArgumentNullException("Condition is null.");
            }
            else
            {
                CodePosition start = reader.Position;
                while (reader.CurrentChar != null && condition(reader.CurrentChar.Value))
                {
                    reader.Advance();
                }
                return reader.CurrentToken!.GetSpan(start, reader.Position);
            }
        }
        public static ReadOnlySpan<char> ParseWhile(this ITokensReader reader, Func<Symbol, bool> condition)
        {
            if (reader == null)
                throw new ArgumentNullException("Reader is null.");
            else if (reader.CurrentSymbol == null)
            {
                return ReadOnlySpan<char>.Empty;
                //throw new ArgumentNullException("Condition is null.");
            }
            else
            {
                CodePosition start = reader.Position;
                while (reader.CurrentSymbol != null && condition(reader.CurrentSymbol.Value))
                {
                    reader.Advance();
                }
                return reader.CurrentToken!.GetSpan(start, reader.Position);
            }
        }
    }
}
