using CarrotScript.Exception;
using CarrotScript.Lexar;
using Microsoft.VisualBasic;
using System;
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
        public CodeReader CodeReader { get; set; }

        /// <summary>
        /// Token向量
        /// </summary>
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// 是否为调试模式
        /// </summary>
        public bool DebugInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public Lexar(CodeReader codeReader, bool debugInfo = false)
        {
            CodeReader = codeReader;
            Tokens = new();
            DebugInfo = debugInfo;
        }

        /// <summary>
        /// 主解析方法
        /// </summary>
        public List<Token> Parse()
        {
            if (DebugInfo)
                Console.WriteLine("Lexar.Parse():");

            CodeReader.Restart();
            while (CodeReader.HasNext())
            {
                Token? token;
                if (TryParseDelimiter(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseKeyword(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseOperator(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseString(CodeReader, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else
                {
                    throw new InvalidSyntaxException("Lexar无法解析的语法",
                        CodeReader.CurrentPosition);
                }
            }
            return Tokens;
        }


        /// <summary>
        /// 匹配不同长度固定表达Token字符串并转换为字符
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryMatchFixedToken(ReadOnlySpan<char> chars, TokenType[] searchTokens, out string? token, out TokenType? type)
        {
            token = null;
            type = null;

            for (int i = 0; i < LLTokenDict.Length; i++)
            {
                if (chars.Length < LLTokenDict[i].len)
                    continue;
                string currToken = chars.Slice(0, LLTokenDict[i].len).ToString();
                if (LLTokenDict[i].dict.TryGetValue(currToken, out TokenType currType)
                    && searchTokens.Contains(currType))
                {
                    token = currToken;
                    type = currType;
                    return true;
                }
            }
            return false;
        }

        public static bool TryParseDelimiter(CodeReader cr, out Token? token)
        {
            if (TryMatchFixedToken(cr.GetNext(), DELIMITERS, out var matchToken, out var matchType))
            {
                token = new Token(matchType!.Value, matchToken!, cr.CurrentPosition);
                cr.Advance(matchToken!.Length);
                return true;
            }
            else
            {
                token = null;
                return false;
            }
        }

        public static bool TryParseKeyword(CodeReader cr, out Token? token)
        {
            if (TryMatchFixedToken(cr.GetNext(), KEYWORDS, out var matchToken, out var matchType))
            {
                token = new Token(matchType!.Value, matchToken!, cr.CurrentPosition);
                cr.Advance(matchToken!.Length);
                return true;
            }
            else
            {
                token = null;
                return false;
            }
        }

        public static bool TryParseOperator(CodeReader cr, out Token? token)
        {
            if (TryMatchFixedToken(cr.GetNext(), OPERATORS, out var matchToken, out var matchType))
            {
                token = new Token(matchType!.Value, matchToken!, cr.CurrentPosition);
                cr.Advance(matchToken!.Length);
                return true;
            }
            else
            {
                token = null;
                return false;
            }
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
                /*|| STRINGS.Contains(codeNext[numLength])*/))
            {
                numLength++;
            }

            if (numLength != 0)
            {
                ReadOnlySpan<char> nums = cr.GetNext(numLength);
                var matchConst = nums.ToString();
                token = new Token(TokenType.STRING, matchConst, cr.CurrentPosition);
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
