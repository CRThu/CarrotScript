using CarrotScript;
using CarrotScript.Lexar;
using CarrotScript.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScriptDemo
{
    public class Runtime
    {
        public List<Token>? Tokens { get; set; }

        public ASTNode? Ast { get; set; }

        public bool DebugInfo { get; set; }

        public Runtime(bool debugInfo = true)
        {
            DebugInfo = debugInfo;
        }

        public void Emit(string c)
        {
            Lexar Lexar = new Lexar(c, DebugInfo);
            Tokens = Lexar.Parse();
            Parser parser = new Parser(Tokens, DebugInfo);
            Ast = parser.Parse();
        }
    }
}
