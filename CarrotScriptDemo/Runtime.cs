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
        public Lexar Lexar { get; set; }

        public List<Token> Tokens { get; set; }

        public ASTNode? Ast { get; set; }

        public Runtime()
        {
        }

        public void Run()
        {
            Lexar = new Lexar();
            Tokens = Lexar.Parse();
            _ = Parser.TryParse(Tokens, out ASTNode? Ast);
            this.Ast = Ast;
        }
    }
}
