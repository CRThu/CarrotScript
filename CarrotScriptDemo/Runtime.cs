using CarrotScript;
using CarrotScript.Lexar;
using CarrotScript.Parser;
using CarrotScript.Reader;
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
                lexarPipeline.Process();

                //Lexar Lexar = new Lexar(codeReader, DebugInfo);
                //Tokens = Lexar.Parse();
                //Parser parser = new Parser(Tokens, DebugInfo);
                //Ast = parser.Parse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
