using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    public class Lexar
    {
        /// <summary>
        /// 代码读取器
        /// </summary>
        public CodeReader cr { get; set; }

        /// <summary>
        /// Token向量
        /// </summary>
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public Lexar(string code)
        {
            cr = new(code);
            Tokens = new();
        }

        /// <summary>
        /// 主解析方法
        /// </summary>
        public List<Token> Parse()
        {
            cr.Restart();
            while (cr.HasNext())
            {
                if (Util.TryFindMatch(LangDef.DELIMITERS, cr.GetNext(), out string? matchDelimiter))
                {
                    ParseDelimiter(matchDelimiter!);
                    continue;
                }
                else if (Util.TryFindMatch(LangDef.KEYWORDS, cr.GetNext(), out string? matchKeyword))
                {
                    ParseKeyword(matchKeyword!);
                    continue;
                }
                else if (Util.TryFindMatch(LangDef.OPERATORS, cr.GetNext(), out string? matchOperator))
                {
                    ParseOperator(matchOperator!);
                    continue;
                }
                else if (TryParseConst(cr, out Token? _))
                {
                    continue;
                }
                else if (TryParseVar(cr, out Token? _))
                {
                    continue;
                }
            }
            return Tokens;
        }


        public void ParseDelimiter(string matchDelimiter)
        {
            var token = new Token(TokenType.DELIMITER, matchDelimiter!, cr.CurrentPosition);
            Tokens.Add(token);
            Console.WriteLine(token);
            cr.Advance(matchDelimiter!.Length);
        }

        private void ParseKeyword(string matchKeyword)
        {
            var token = new Token(TokenType.KEYWORD, matchKeyword!, cr.CurrentPosition);
            Tokens.Add(token);
            Console.WriteLine(token);
            cr.Advance(matchKeyword!.Length);
        }


        private void ParseOperator(string matchOperator)
        {
            var token = new Token(TokenType.OPERATOR, matchOperator!, cr.CurrentPosition);
            Tokens.Add(token);
            Console.WriteLine(token);
            cr.Advance(matchOperator!.Length);
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

        /// <summary>
        /// 解析常量
        /// </summary>
        /// <returns></returns>
        public bool TryParseConst(CodeReader cr, out Token? token)
        {
            int numLength = 0;

            ReadOnlySpan<char> codeNext = cr.GetNext();

            while (cr.HasNext()
                && char.IsAsciiDigit(codeNext[numLength]))
            {
                numLength++;
            }

            if (numLength != 0)
            {
                ReadOnlySpan<char> nums = cr.GetNext( numLength);
                var matchConst = nums.ToString();
                token = new Token(TokenType.CONST, matchConst, this.cr.CurrentPosition);
                Tokens.Add((Token)token);
                Console.WriteLine(token);
                this.cr.Advance(matchConst!.Length);
                return true;
            }

            token = null;
            return false;
        }


        public bool TryParseVar(CodeReader cr, out Token? token)
        {
            int numLength = 0;

            ReadOnlySpan<char> codeNext = cr.GetNext();

            while (cr.HasNext()
                 && (char.IsAsciiLetterOrDigit(codeNext[numLength])
                || codeNext[numLength] == ';'
                || codeNext[numLength] == '_'))
            {
                numLength++;
            }

            if (numLength != 0)
            {
                ReadOnlySpan<char> nums = cr.GetNext(numLength);
                var matchConst = nums.ToString();
                token = new Token(TokenType.CONST, matchConst, this.cr.CurrentPosition);
                Tokens.Add((Token)token);
                Console.WriteLine(token);
                this.cr.Advance(matchConst!.Length);
                return true;
            }

            token = null;
            return false;
        }
    }
}
