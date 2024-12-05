using CarrotScript;
using CarrotScript.Interpreter;
using CarrotScript.Lexar;
using CarrotScript.Parser;
using CarrotScript.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public void Emit(string code)
        {
            try
            {
                //StringCodeReader codeReader = new(code);

                LexarPipeline lexarPipeline = new LexarPipeline();
                lexarPipeline.DebugInfo = true;
                lexarPipeline.AddLexar(new CarrotXmlLexar());
                lexarPipeline.AddLexar(new CarrotScriptLexar());
                //lexarPipeline.AddReader(codeReader);
                lexarPipeline.Code = code;
                Tokens = new List<Token>(lexarPipeline.Tokenize());

                Parser parser = new Parser(Tokens, DebugInfo);
                Ast = parser.Parse();

                Console.WriteLine($"");
                Console.WriteLine($"--- PARSER ---");
                Console.WriteLine(Ast.ToTree());

                var targetManager = new TargetManager();
                var env = new RuntimeEnvironment(targetManager);
                var interpreter = new Interpreter(env);

                Console.WriteLine($"");
                Console.WriteLine($"--- INTERPRETER ---");
                interpreter.Execute(Ast);

                Console.WriteLine($"");
                env.PrintSnapshot();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
