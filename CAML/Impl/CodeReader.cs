using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAML.Impl
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
        public void Clear()
        {
            Cursor = 0;
        }

        /// <summary>
        /// 获取游标所指字符并向右移动游标
        /// </summary>
        /// <returns></returns>
        public char Next()
        {
            return Code[Cursor++];
        }


        /// <summary>
        /// 获取游标所指字符
        /// </summary>
        /// <returns></returns>
        public char GetChar()
        {
            return GetNextChar();
        }

        /// <summary>
        /// 获取游标偏移所指字符
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public char GetNextChar(int offset = 0)
        {
            int newCursor = Cursor + offset;
            if (newCursor > Code.Length - 1)
                return '\0';
            else
                return Code[newCursor];
        }

        /// <summary>
        /// 是否游标有字符
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool HasNext(int offset = 0)
        {
            int newCursor = Cursor + offset;
            if (newCursor > Code.Length - 1)
                return false;
            else
                return true;
        }
    }
}
