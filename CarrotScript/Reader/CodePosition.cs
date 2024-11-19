﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Reader
{
    /// <summary>
    /// Token位置
    /// </summary>
    public struct CodePosition
    {
        public string? File { get; set; }
        public int Offset { get; set; }
        public int Line { get; set; }
        public int Col { get; set; }

        public CodePosition()
        {
            File = "<NULL>";
            Offset = 0;
            Line = 1;
            Col = 1;
        }

        /// <param name="line">代码行</param>
        /// <param name="col">代码列</param>
        public CodePosition(string file, int offset, int line, int col)
        {
            File = file;
            Offset = offset;
            Line = line;
            Col = col;
        }

        public CodePosition(CodePosition currentPosition) : this()
        {
            File = currentPosition.File;
            Offset = currentPosition.Offset;
            Line = currentPosition.Line;
            Col = currentPosition.Col;
        }

        public override string ToString()
        {
            return $"{File}:{Offset}({Line}:{Col})";
        }
    }
}
