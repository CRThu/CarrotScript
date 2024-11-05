using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Parser
{
    public class ParserTrace
    {
        public ASTNode? Node { get; set; }
        public ExceptionBase? Error { get; set; }

        public ParserTrace()
        {
            
        }

        public ParserTrace SetError(ExceptionBase error)
        {
            Error = error;
            return this;
        }

        public ASTNode Check(ASTNode node)
        {
            Node = node;
            return Node;
        }

        public ParserTrace Check(ParserTrace trace)
        {
            if (trace.Error != null)
            {
                Error = trace.Error;
            }
            return this;
        }
    }
}
