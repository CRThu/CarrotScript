using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Reader
{
    public interface ITokenReader
    {
        public char? CurrentChar { get; }
        public Symbol? CurrentSymbol { get; }
        public char? NextChar { get; }
        public Symbol? NextSymbol { get; }

        /// <summary>
        /// 当前游标位置
        /// </summary>
        public CodePosition Position { get; }

        /// <summary>
        /// 返回下一个字符并前进游标
        /// </summary>
        /// <returns></returns>
        public char? Advance();

        public ReadOnlySpan<char> GetSpan(CodePosition start, CodePosition end);
    }
}
