using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    public enum TokenType
    {
        /// <summary>
        /// 运算符
        /// </summary>
        Operator,

        /// <summary>
        /// 常量
        /// </summary>
        Value,

        /// <summary>
        /// 变量
        /// </summary>
        Variable
    }

    /// <summary>
    /// Token
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// Token类型
        /// </summary>
        public TokenType Type;

        /// <summary>
        /// Token值
        /// </summary>
        public string Value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public Token(TokenType tokenType, string value)
        {
            Type = tokenType;
            Value = value;
        }

        /// <summary>
        /// 常量构造函数
        /// </summary>
        /// <param name="num"></param>
        public Token(double num)
        {
            Type = TokenType.Value;
            Value = num.ToString("G15");
        }

        /// <summary>
        /// 运算符构造函数
        /// </summary>
        /// <param name="op"></param>
        public Token(string op)
        {
            Type = TokenType.Operator;
            Value = op;
        }

        public override string ToString()
        {
            return $"Token: {{{Type} : {Value}}}";
        }
    }
}
