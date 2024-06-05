using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    /// <summary>
    /// Token类型
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// 运算符
        /// </summary>
        OPERATOR,

        /// <summary>
        /// 常量
        /// </summary>
        CONST,

        /// <summary>
        /// 变量
        /// </summary>
        VARIABLE,

        /// <summary>
        /// 关键字
        /// </summary>
        KEYWORD,

        /// <summary>
        /// 分隔符
        /// </summary>
        DELIMITER
    }

    /// <summary>
    /// Token位置
    /// </summary>
    /// <param name="line">代码行</param>
    /// <param name="col">代码列</param>
    public readonly struct TokenPosition(int line, int col)
    {
        public int Line { get; init; } = line;
        public int Col { get; init; } = col;
    }

    /// <summary>
    /// Token实现
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
        /// 代码位置
        /// </summary>
        public TokenPosition Pos;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public Token(TokenType tokenType, string value, TokenPosition tokenPosition)
        {
            Type = tokenType;
            Value = value;
            Pos = tokenPosition;
        }

        /// <summary>
        /// 常量构造Token
        /// </summary>
        /// <param name="num"></param>
        public Token(double value, TokenPosition tokenPosition)
        : this(TokenType.CONST, value.ToString("G15"), tokenPosition)
        {
        }

        /// <summary>
        /// 运算符构造函数
        /// </summary>
        /// <param name="op"></param>
        public Token(string op, TokenPosition tokenPosition)
        : this(TokenType.OPERATOR, op, tokenPosition)
        {
        }

        public override readonly string ToString()
        {
            string readableValue = Value
                .Replace("\n", "<NEWLINE>")
                .Replace(" ", "<SPACE>");

            return $"Token: {{ {Type} : \t{readableValue} }}";
        }
    }
}
