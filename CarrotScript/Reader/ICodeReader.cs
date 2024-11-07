using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Reader
{
    public interface ICodeReader
    {
        /// <summary>
        /// 返回下一个字符但不前进游标
        /// </summary>
        /// <returns>字符,若不可用则返回</returns>
        public char? Peek();
        /// <summary>
        /// 返回下一个字符并前进游标
        /// </summary>
        /// <returns></returns>
        public char? Read();

    }
}
