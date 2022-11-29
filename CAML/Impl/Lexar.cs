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

                if (Info.OPERATORS.FindMatchHeader(nc) != null)
                {
                    Tokens.Add(ParseOperator());
                }
                else if (char.IsLetterOrDigit(nc) || nc.Equals('_'))
                {
                    Tokens.Add(ParseVariable());
                }
                else if (char.IsDigit(nc) || nc.Equals('-') || nc.Equals('.'))
                {
                    Tokens.Add(ParseValue());
                }
                else if (char.IsWhiteSpace(nc))
                    cr.Next();
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
            char op0 = cr.Next();
            if (cr.HasNext())
            {
                char op1 = cr.GetNextChar(0);

                // 判断是否为两位运算符
                for (int i = 0; i < Info.OPERATORS.Length; i++)
                {
                    string op = Info.OPERATORS[i];
                    if (op[0] == op0 && op[1] == op1)
                    {
                        cr.Next();
                        return new Token(op);
                    }
                }
            }

            // 判断是否为一位运算符
            for (int i = 0; i < Info.OPERATORS.Length; i++)
            {
                string op = Info.OPERATORS[i];
                if (op[0] == op0)
                {
                    return new Token(op);
                }
            }

            // 无法识别运算符
            throw new NotImplementedException($"无法识别的运算符:{op0}");
        }

        /// <summary>
        /// 解析常量
        /// </summary>
        /// <returns></returns>
        public Token ParseValue()
        {
            List<char> val = new(16);
            while (cr.HasNext())
            {
                char nc = cr.GetNextChar();
                if (char.IsDigit(nc) || nc.Equals('-') || nc.Equals('.') || nc.Equals('e') || nc.Equals('E'))
                    val.Add(cr.Next());
                else
                    break;
            }
            return new Token(TokenType.Variable, string.Concat(val));
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
