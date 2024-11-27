using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Reader
{
    public class TokenReader : ITokenReader
    {
        public Token? Token { get; set; }

        /// <summary>
        /// 代码文件名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 代码字符串
        /// </summary>
        public string Code { get; set; }

        private int CodeFixedOffset { get; set; }

        public int Offset { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public char? CurrentChar => PeekNext(0);
        public Symbol? CurrentSymbol => CurrentChar.ToSymbol();
        public char? NextChar => PeekNext(1);
        public Symbol? NextSymbol => NextChar.ToSymbol();

        /// <summary>
        /// 游标位置
        /// </summary>
        public CodePosition Position => new CodePosition(File, Offset, Line, Column);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public TokenReader(Token token)
        {
            Token = token;
            File = "<input>";
            Code = token.Value;
            CodeFixedOffset = token.Span.Start.Offset;
            Offset = token.Span.Start.Offset;
            Line = token.Span.Start.Line;
            Column = token.Span.Start.Col;
        }

        public char? PeekNext(int offset = 1)
        {
            try
            {
                if (Position.Offset + offset - Token!.Span.Start.Offset < Code.Length)
                    return Code[Position.Offset - CodeFixedOffset + offset];
                else
                    return null;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public char? Advance()
        {
            if (CurrentChar == null)
            {
                return null;
            }
            else if (CurrentChar == '\n')
            {
                Line += 1;
                Column = 1;
            }
            else
            {
                Column += 1;
            }

            Offset += 1;
            char? c = CurrentChar;

            return c;
        }

        /// <summary>
        /// 获取所指字符串
        /// </summary>
        /// <returns></returns>
        private ReadOnlySpan<char> GetSpan(int start, int end)
        {
            try
            {
                return Code.AsSpan(start - CodeFixedOffset, end - start);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所指字符串
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> GetSpan(CodePosition start, CodePosition end)
        {
            return GetSpan(start.Offset, end.Offset);
        }
    }
}
