using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarrotScript.Reader;

namespace CarrotScript
{
    public struct TokenSpan
    {
        public CodePosition Start { get; set; }
        public CodePosition End { get; set; }

        public TokenSpan(CodePosition start, CodePosition end)
        {
            Start = start;
            End = end;
        }

        public override readonly string ToString()
        {
            return $"{Start.File}:{Start.Line}:{Start.Col}-{End.Line}:{End.Col}";
        }
    }
}
