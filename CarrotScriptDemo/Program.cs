using CarrotScript;
using CarrotScript.Lexar;
using CarrotScript.Parser;
using System.Collections.Generic;

namespace CarrotScriptDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code =
                "(1+2)*5\n";
            Console.WriteLine("code: ");
            Console.WriteLine(code);

            Lexar lexar = new(code);
            Console.WriteLine("lexar: ");
            Console.WriteLine(lexar);

            List<Token> tokens = lexar.Parse();
            Parser parser = new(tokens);
            var ast = parser.Parse();

            Console.WriteLine("ast: ");
            Console.WriteLine(ast);
        }
    }
}
