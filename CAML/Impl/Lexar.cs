using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAML.Impl
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
            cr.Clear();
            while (cr.HasNext() || cr.Equals(Info.FILE_END))
            {
                char nc = cr.GetNextChar();
                if (char.IsWhiteSpace(nc))
                    cr.Next();
                else if (Info.OPERATORS.FindMatchHeader(nc) != null)
                {
                    // 1*-0.12 1*-.12 1*+0.12 1*+.12

                    Tokens.Add(ParseOperator());

                    if (cr.HasNext() || cr.Equals(Info.FILE_END))
                    {
                        char nc1 = cr.GetNextChar();
                        if (nc1.Equals('-') || nc1.Equals('+'))
                            Tokens.Add(ParseValue());
                    }
                }
                else if (char.IsLetter(nc) || nc.Equals('_'))
                {
                    Tokens.Add(ParseVariable());
                }
                else if (char.IsDigit(nc) || nc.Equals('-') || nc.Equals('+') || nc.Equals('.'))
                {
                    Tokens.Add(ParseValue());
                }
            }
            return Tokens;
        }

        /// <summary>
        /// 解析运算符
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Token ParseOperator()
        {
            string op = cr.Next().ToString();
            if (cr.HasNext())
            {
                // 判断是否为两位运算符
                string? op2 = Util.FindMatchHeader(Info.OPERATORS, op + cr.GetNextChar(0).ToString(), isPrecise: true);
                if (op2 != null)
                {
                    cr.Next();
                    return new Token(op2);
                }
            }

            // 判断是否为一位运算符
            if (Util.FindMatchHeader(Info.OPERATORS, op, isPrecise: true) != null)
            {
                return new Token(op);
            }

            // 无法识别运算符
            throw new NotImplementedException($"无法识别的运算符: {op}");
        }

        /// <summary>
        /// 解析常量
        /// </summary>
        /// <returns></returns>
        public Token ParseValue()
        {
            // STATE:   0 1    2 3 4
            // NUMBER:  + 1.02 E + 03
            List<char> val = new(16);
            short state = 0;
            while (cr.HasNext())
            {
                char nc = cr.GetNextChar();

                switch (state)
                {
                    case 0:
                        // +/-
                        if (nc.Equals('-') || nc.Equals('+'))
                        {
                            val.Add(cr.Next());
                        }
                        state = 1;
                        break;
                    case 1:
                        // 1.02
                        if (char.IsDigit(nc) || nc.Equals('.'))
                        {
                            val.Add(cr.Next());
                        }
                        else
                        {
                            state = 2;
                        }
                        break;
                    case 2:
                        // e/E
                        if (nc.Equals('e') || nc.Equals('E'))
                        {
                            val.Add(cr.Next());
                            state = 3;
                        }
                        else
                        {
                            state = 5;
                        }
                        break;
                    case 3:
                        // +/-
                        if (nc.Equals('-') || nc.Equals('+'))
                        {
                            val.Add(cr.Next());
                        }
                        state = 4;
                        break;
                    case 4:
                        // 03
                        if (char.IsDigit(nc))
                        {
                            val.Add(cr.Next());
                        }
                        else
                        {
                            state = 5;
                        }
                        break;
                }
                if (state == 5)
                    break;
            }
            return new Token(TokenType.Value, string.Concat(val));
        }

        /// <summary>
        /// 解析变量
        /// </summary>
        /// <returns></returns>
        public Token ParseVariable()
        {
            List<char> variable = new(16);
            while (cr.HasNext())
            {
                char nc = cr.GetNextChar();
                if (char.IsLetterOrDigit(nc) || nc.Equals('_'))
                    variable.Add(cr.Next());
                else
                    break;
            }
            return new Token(TokenType.Variable, string.Concat(variable));
        }
    }
}
