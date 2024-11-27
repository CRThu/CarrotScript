﻿using System;
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
        IEnumerable<Token> Tokenize(IEnumerable<Token> inputTokens);
    }

    public class LexarPipeline
    {
        public string Code { get; set; } = "";
        //public List<StringCodeReader> Readers = new List<StringCodeReader>();
        public List<ILexar> Lexars { get; set; } = new List<ILexar>();
        public bool DebugInfo { get; set; }

        public void AddLexar(ILexar lexar)
        {
            Lexars.Add(lexar);
        }

        //public void AddReader(StringCodeReader reader)
        //{
        //    Readers.Add(reader);
        //}

        public IEnumerable<Token> Process()
        {
            List<Token> tokens = new List<Token>();
            tokens.Add(new Token(TokenType.ROOT, Code, new TokenSpan()));

            IEnumerable<Token> currTokens = tokens;

            int phase = 0;

            if (DebugInfo)
            {
                Console.WriteLine();
                Console.WriteLine($"--- PHASE {phase} LEX ---");
                foreach (var token in currTokens)
                    Console.WriteLine(token.ToString());
            }
            phase++;


            foreach (var lex in Lexars)
            {
                currTokens = lex.Tokenize(currTokens);

                if (DebugInfo)
                {
                    Console.WriteLine();
                    Console.WriteLine($"--- PHASE {phase} {lex} ---");
                    foreach (var token in currTokens)
                        Console.WriteLine(token.ToString());
                }
                phase++;
            }

            return currTokens;
        }
    }
}
