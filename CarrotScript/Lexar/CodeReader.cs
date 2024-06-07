using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Lexar
{
    public class CodeReader
    {
        /// <summary>
        /// 代码字符串
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 访问游标
        /// </summary>
        public int Cursor { get; set; }

        /// <summary>
        /// 当前游标所指向代码坐标
        /// </summary>
        public TokenPosition CurrentPosition
        {
            get => new(1, Cursor + 1);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public CodeReader(string code)
        {
            Code = code;
            Cursor = 0;
        }

        /// <summary>
        /// 游标返回指向代码头
        /// </summary>
        public void Restart()
        {
            Cursor = 0;
        }

        /// <summary>
        /// 前进游标
        /// </summary>
        /// <returns></returns>
        public void Advance(int i = 0)
        {
            Cursor += i;
        }

        /// <summary>
        /// 获取游标偏移所指字符串
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> GetNext()
        {
            return Code.AsSpan(Cursor);
        }

        /// <summary>
        /// 获取游标偏移所指字符串
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> GetNext(int length = 0)
        {
            return Code.AsSpan(Cursor, length);
        }

        public ReadOnlySpan<char> GetNextNum()
        {
            int numLength = 0;
            while (HasNext() && char.IsAsciiDigit(Code[Cursor + numLength]))
            {
                numLength++;
            }
            return Code.AsSpan(Cursor, numLength);
        }

        /// <summary>
        /// 是否到文件结尾
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool HasNext(int offset = 0)
        {
            return Cursor + offset <= Code.Length - 1;
        }
    }
}
