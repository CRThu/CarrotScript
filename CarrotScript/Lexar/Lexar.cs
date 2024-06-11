using CarrotScript.Lexar;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.LangDef;

namespace CarrotScript.Lexar
{
    public class Lexar
    {
        /// <summary>
        /// 代码读取器
        /// </summary>
        public CodeReader CodeReader { get; set; }

        /// <summary>
        /// Token向量
        /// </summary>
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public Lexar(string code = "")
        {
            CodeReader = new();
            CodeReader.Code = code;
            Tokens = new();
        }

        /// <summary>
        /// 主解析方法
        /// </summary>
        public List<Token> Parse()
        {
            CodeReader.Restart();
            while (CodeReader.HasNext())
            {
                Token? token;
                if (TryParseDelimiter(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    Console.WriteLine(token);
                    continue;
                }
                else if (TryParseKeyword(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    Console.WriteLine(token);
                    continue;
                }
                else if (TryParseOperator(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    Console.WriteLine(token);
                    continue;
                }
                else if (TryParseString(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    Console.WriteLine(token);
                    continue;
                }
                else
                {
                    throw new LexarNotSupportException(CodeReader.CurrentPosition);
                }
            }
            return Tokens;
        }

        private static bool TryParseDelimiter(CodeReader cr, out Token? token)
        {
            if (DELIMITERS.TryFindMatch(cr.GetNext(), out string? matchDelimiter))
            {
                token = new Token(TokenType.DELIMITER, matchDelimiter!, cr.CurrentPosition);
                cr.Advance(matchDelimiter!.Length);
                return true;
            }
            token = null;
            return false;
        }

        private static bool TryParseKeyword(CodeReader cr, out Token? token)
        {
            if (KEYWORDS.TryFindMatch(cr.GetNext(), out string? matchKeyword))
            {
                token = new Token(TokenType.KEYWORDS, matchKeyword!, cr.CurrentPosition);
                cr.Advance(matchKeyword!.Length);
                return true;
            }
            token = null;
            return false;
        }

        public static bool TryParseOperator(CodeReader cr, out Token? token)
        {
            if (BINARY_OPERATORS.TryFindMatch(cr.GetNext(), out string? matchOperator))
            {
                token = new Token(TokenType.OPERATOR, matchOperator!, cr.CurrentPosition);
                cr.Advance(matchOperator!.Length);
                return true;
            }
            token = null;
            return false;
        }
        /// <summary>
        /// 解析常量
        /// </summary>
        /// <returns></returns>
        public static bool TryParseString(CodeReader cr, out Token? token)
        {
            int numLength = 0;

            ReadOnlySpan<char> codeNext = cr.GetNext();

            while (cr.HasNext(numLength)
                 && (char.IsAsciiLetterOrDigit(codeNext[numLength])
                || STRINGS.Contains(codeNext[numLength])))
            {
                numLength++;
            }

            if (numLength != 0)
            {
                ReadOnlySpan<char> nums = cr.GetNext(numLength);
                var matchConst = nums.ToString();
                token = new Token(TokenType.STR, matchConst, cr.CurrentPosition);
                cr.Advance(matchConst!.Length);
                return true;
            }

            token = null;
            return false;
        }

        ///// <summary>
        ///// 解析常量
        ///// </summary>
        ///// <returns></returns>
        //public Token ParseValue()
        //{
        //    // STATE:   0 1    2 3 4
        //    // NUMBER:  + 1.02 E + 03
        //    List<char> val = new(16);
        //    short state = 0;
        //    while (cr.HasNext())
        //    {
        //        char nc = cr.GetNext();

        //        switch (state)
        //        {
        //            case 0:
        //                // +/-
        //                if (nc.Equals('-') || nc.Equals('+'))
        //                {
        //                    val.Add(cr.AdvanceNext());
        //                }
        //                state = 1;
        //                break;
        //            case 1:
        //                // 1.02
        //                if (char.IsDigit(nc) || nc.Equals('.'))
        //                {
        //                    val.Add(cr.AdvanceNext());
        //                }
        //                else
        //                {
        //                    state = 2;
        //                }
        //                break;
        //            case 2:
        //                // e/E
        //                if (nc.Equals('e') || nc.Equals('E'))
        //                {
        //                    val.Add(cr.AdvanceNext());
        //                    state = 3;
        //                }
        //                else
        //                {
        //                    state = 5;
        //                }
        //                break;
        //            case 3:
        //                // +/-
        //                if (nc.Equals('-') || nc.Equals('+'))
        //                {
        //                    val.Add(cr.AdvanceNext());
        //                }
        //                state = 4;
        //                break;
        //            case 4:
        //                // 03
        //                if (char.IsDigit(nc))
        //                {
        //                    val.Add(cr.AdvanceNext());
        //                }
        //                else
        //                {
        //                    state = 5;
        //                }
        //                break;
        //        }
        //        if (state == 5)
        //            break;
        //    }
        //    return new Token(TokenType.CONST, string.Concat(val));
        //}

    }
}
