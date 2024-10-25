using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript
{
    public struct TokenSpan
    {
        public TokenPosition Start { get; set; }
        public TokenPosition End { get; set; }

        public TokenSpan(TokenPosition start, TokenPosition end)
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
