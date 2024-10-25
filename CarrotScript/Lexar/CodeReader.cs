using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Lexar
{
    public class CodeReader
    {
        /// <summary>
        /// 代码文件名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 代码字符串
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 游标位置
        /// </summary>
        public TokenPosition CurrentPosition { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeReader()
        {
            File = "<NULL>";
            Code = string.Empty;
            CurrentPosition = new TokenPosition(File, 0, 1, 1);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public CodeReader(string file, string code)
        {
            File = file;
            Code = code;
            CurrentPosition = new TokenPosition(File, 0, 1, 1);
        }

        /// <summary>
        /// 是否到文件结尾
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return CurrentPosition.Offset <= Code.Length - 1;
        }

        public char GetChar()
        {
            return Code[CurrentPosition.Offset];
        }

        public bool Advance()
        {
            string newFile = CurrentPosition.File!;
            int newOffset = CurrentPosition.Offset;
            int newLine = CurrentPosition.Line;
            int newCol = CurrentPosition.Col;

            newOffset += 1;

            char c = GetChar();

            if (c == '\n')
            {
                newLine += 1;
                newCol = 1;
            }
            else
            {
                newCol += 1;
            }

            CurrentPosition = new TokenPosition(newFile, newOffset, newLine, newCol);


            if (!HasNext())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取所指字符串
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> GetSpan(int start, int end)
        {
            return Code.AsSpan(start, end - start + 1);
        }

        /// <summary>
        /// 获取所指字符串
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> GetSpan(TokenPosition start, TokenPosition end)
        {
            return GetSpan(start.Offset, end.Offset);
        }
    }
}
