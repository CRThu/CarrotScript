using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript
{
    /// <summary>
    /// Token位置
    /// </summary>
    public struct TokenPosition
    {
        public string? File { get; set; }
        public int Line { get; set; }
        public int Col { get; set; }

        /// <param name="line">代码行</param>
        /// <param name="col">代码列</param>
        public TokenPosition(string file, int line, int col)
        {
            File = file;
            Line = line;
            Col = col;
        }

        public TokenPosition(TokenPosition currentPosition) : this()
        {
            File = currentPosition.File;
            Line = currentPosition.Line;
            Col = currentPosition.Col;
        }

        public override string ToString()
        {
            return $"{File}:{Line}:{Col}";
        }
    }
}
