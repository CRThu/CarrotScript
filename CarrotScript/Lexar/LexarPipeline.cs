using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarrotScript.Reader;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public interface ILexar
    {
        /// <summary>
        /// Tokenize
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        IEnumerable<Token> Tokenize(IEnumerable<Token> tokens);
    }

    public class LexarPipeline
    {
        public List<CodeReader> Readers = new List<CodeReader>();
        public List<ILexar> Lexars = new List<ILexar>();

        public void AddLexar(ILexar lexar)
        {
            Lexars.Add(lexar);
        }

        public void AddReader(CodeReader reader)
        {
            Readers.Add(reader);
        }

        public IEnumerable<Token> Process()
        {
            List<Token> tokens = new List<Token> { new Token(TokenType.ROOT, "", default) };
            IEnumerable<Token> currTokens = tokens;

            foreach (var lex in Lexars)
            {
                currTokens = lex.Tokenize(currTokens);
            }

            return currTokens;
        }
    }
}
