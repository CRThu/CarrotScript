using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Parser
{
    public struct ASTNodePosition
    {
        public string? File { get; set; }
        public int StartLine { get; set; }
        public int StartCol { get; set; }
        public int EndLine { get; set; }
        public int EndCol { get; set; }

        public ASTNodePosition(string file, int startLine, int startCol, int endLine, int endCol)
        {
            File = file;
            StartLine = startLine;
            StartCol = startCol;
            EndLine = endLine;
            EndCol = endCol;
        }

        public ASTNodePosition(ref TokenPosition start, ref TokenPosition end)
        {
            File = start.File;
            StartLine = start.Line;
            StartCol = start.Col;
            EndLine = end.Line;
            EndCol = end.Col;
        }

        public override string ToString()
        {
            return $"{File}:{StartLine}:{StartCol}-{EndLine}:{EndCol}";
        }
    }
}
