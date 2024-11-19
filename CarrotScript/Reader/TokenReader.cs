using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Reader
{
    public class TokenReader : ITokenReader
    {
        /// <summary>
        /// 代码文件名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 代码字符串
        /// </summary>
        public string Code { get; set; }

        public int Offset { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public int LastLine { get; set; }

        public int LastColumn { get; set; }

        /// <summary>
        /// 游标位置
        /// </summary>
        public CodePosition Position => new CodePosition(File, Offset, Line, Column);

        /// <summary>
        /// 上一个读取的字符位置
        /// </summary>
        public CodePosition LastPosition => new CodePosition(File, Offset, LastLine, LastColumn);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public TokenReader(Token token)
        {
            File = "<input>";
            Code = token.Value;
            Offset = token.Span.Start.Offset;
            Line = token.Span.Start.Line;
            Column = token.Span.Start.Col;
        }

        public char? Peek()
        {
            if (Offset <= Code.Length - 1)
                return Code[Position.Offset];
            else
                return null;
        }

        public char? Read()
        {
            LastLine = Line;
            LastColumn = Column;

            char? c = Peek();

            if (c == null)
            {
                return null;
            }
            else if (c == '\n')
            {
                Line += 1;
                Column = 1;
            }
            else
            {
                Column += 1;
            }

            Offset += 1;
            return c;
        }

        ///// <summary>
        ///// 获取所指字符串
        ///// </summary>
        ///// <returns></returns>
        //public ReadOnlySpan<char> GetSpan(int start, int end)
        //{
        //    return Code.AsSpan(start, end - start + 1);
        //}

        ///// <summary>
        ///// 获取所指字符串
        ///// </summary>
        ///// <returns></returns>
        //public ReadOnlySpan<char> GetSpan(CodePosition start, CodePosition end)
        //{
        //    return GetSpan(start.Offset, end.Offset);
        //}
    }
}
