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
        /// 代码文件名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 代码字符串
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 访问游标
        /// </summary>
        public int Cursor { get; set; }

        private TokenPosition currentPosition;

        /// <summary>
        /// 当前游标所指向代码坐标
        /// </summary>
        public TokenPosition CurrentPosition => currentPosition;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeReader()
        {
            File = "<NULL>";
            Code = string.Empty;
            Restart();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public CodeReader(string file, string code)
        {
            File = file;
            Code = code;
            Restart();
        }

        /// <summary>
        /// 游标返回指向代码头
        /// </summary>
        public void Restart()
        {
            Cursor = 0;
            currentPosition = new TokenPosition(File, 1, 1);
        }

        /// <summary>
        /// 前进游标
        /// </summary>
        /// <returns></returns>
        public void Advance(int offset = 0)
        {
            UpdatePosition(offset);
            Cursor += offset;
        }

        public void UpdatePosition(int offset = 0)
        {
            ReadOnlySpan<char> chars = Code.AsSpan(Cursor, offset);
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\n')
                {
                    currentPosition.Line += 1;
                    currentPosition.Col = 1;
                }
                else
                {
                    currentPosition.Col += 1;
                }
            }
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
