using CarrotScript.Exception;
using CarrotScript.Lexar;
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
        public CodeReader Cr { get; set; }

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
            Cr = codeReader;
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

            Cr.Restart();
            while (Cr.HasNext())
            {
                Token? token;
                TokenPosition start, end;

                if (TryParseDelimiter(Cr, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (NumericScanner.Scan(Cr) != 0)
                {
                    int len = NumericScanner.Scan(Cr);
                    ReadOnlySpan<char> s = Cr.GetNextSpan(len);

                    start = Cr.CurrentPosition;
                    Cr.Advance(len);
                    end = Cr.CurrentPosition;

                    Tokens.Add(new Token(TokenType.NUMERIC, s.ToString(),
                        new TokenSpan(ref start, ref end)));
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseKeyword(Cr, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseOperator(Cr, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else if (TryParseString(Cr, out token))
                {
                    Tokens.Add((Token)token!);
                    if (DebugInfo)
                        Console.WriteLine(token);
                    continue;
                }
                else
                {
                    start = Cr.CurrentPosition;
                    end = Cr.CurrentPosition;

                    throw new InvalidSyntaxException("Lexar无法解析的语法",
                        new TokenSpan(ref start, ref end));
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
            if (TryMatchFixedToken(cr.GetNextSpan(), DELIMITERS, out var matchToken, out var matchType))
            {
                var start = cr.CurrentPosition;
                cr.Advance(matchToken!.Length);
                var end = cr.CurrentPosition;

                token = new Token(matchType!.Value, matchToken!,
                    new TokenSpan(ref start, ref end));
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
            if (TryMatchFixedToken(cr.GetNextSpan(), KEYWORDS, out var matchToken, out var matchType))
            {
                var start = cr.CurrentPosition;
                cr.Advance(matchToken!.Length);
                var end = cr.CurrentPosition;

                token = new Token(matchType!.Value, matchToken!,
                    new TokenSpan(ref start, ref end));
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
            if (TryMatchFixedToken(cr.GetNextSpan(), OPERATORS, out var matchToken, out var matchType))
            {
                var start = cr.CurrentPosition;
                cr.Advance(matchToken!.Length);
                var end = cr.CurrentPosition;

                token = new Token(matchType!.Value, matchToken!,
                    new TokenSpan(ref start, ref end));
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

            ReadOnlySpan<char> codeNext = cr.GetNextSpan();

            while (cr.HasNext(numLength)
                 && (char.IsAsciiLetterOrDigit(codeNext[numLength])
                /*|| STRINGS.Contains(codeNext[numLength])*/))
            {
                numLength++;
            }

            if (numLength != 0)
            {
                ReadOnlySpan<char> nums = cr.GetNextSpan(numLength);
                var matchToken = nums.ToString();

                var start = cr.CurrentPosition;
                cr.Advance(matchToken!.Length);
                var end = cr.CurrentPosition;

                token = new Token(TokenType.STRING, matchToken,
                    new TokenSpan(ref start, ref end));

                return true;
            }

            token = null;
            return false;
        }


    }
}
