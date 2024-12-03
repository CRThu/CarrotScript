using CarrotScript.Exception;
using CarrotScript.Reader;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{

    public class Lexar
    {
        /// <summary>
        /// 代码读取器
        /// </summary>
        public TokensReader Reader { get; set; }

        /// <summary>
        /// Token向量
        /// </summary>
        public List<Token> Tokens { get; set; }

        public XmlLexarState CurrentState { get; set; }

        public Dictionary<string, object> Context { get; set; }

        /// <summary>
        /// 是否为调试模式
        /// </summary>
        public bool DebugInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public Lexar(TokensReader codeReader, bool debugInfo = false)
        {
            Reader = codeReader;
            Tokens = new();
            //CurrentState = XmlState.XmlContent;
            Context = new();
            DebugInfo = debugInfo;
        }

        /// <summary>
        /// 主解析方法
        /// </summary>
        public List<Token> Tokenize()
        {
            if (DebugInfo)
                Console.WriteLine("Lexar.Parse():");
            /*
            while (CarrotXmlLexar.Tokenize(this))
                ;

            while (CarrotScriptLexar.Tokenize(this))
                ;
            */
            /*
            while (Cr.HasNext())
            {
                TokenType type;
                int len;

                if (DelimiterScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else if (OperatorScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else if (NumericScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else if (KeywordScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else if (IdentifierScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else if (StringScanner.TryScan(Cr, out type, out len))
                {
                    CreateToken(Cr, type, len);
                    continue;
                }
                else
                {
                    TokenPosition start = Cr.CurrentPosition;
                    TokenPosition end = Cr.CurrentPosition;

                    throw new InvalidSyntaxException("Lexar无法解析的语法",
                        new TokenSpan(ref start, ref end));
                }
            }

            // REMOVE EXCLUDE TOKENS
            int occur = Tokens.RemoveAll(t => EXCLUDE_TOKENS.Contains(t.Type));
            if (DebugInfo)
            {
                Console.WriteLine($"REMOVED {occur} TOKENS.");
                if (occur != 0)
                {
                    Console.WriteLine("NEW TOKENS:");
                    for (int i = 0; i < Tokens.Count; i++)
                        Console.WriteLine(Tokens[i].ToString());
                }
            }
            */
            return Tokens;
        }

        /*
        public void CreateToken(TokenType type, CodePosition start, CodePosition end)
        {
            ReadOnlySpan<char> s = Reader.GetSpan(start, end);

            Token token = new Token(type, s.ToString(), new TokenSpan(start, end));

            Tokens.Add(token);
            if (DebugInfo)
                Console.WriteLine(token);
        }
        */

    }
}
