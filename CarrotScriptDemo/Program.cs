using CarrotScript.Impl;
using System.Collections.Generic;

namespace CarrotScriptDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code =
                ">> COM88\n" +
                "Helloworld;A;B;C;\n" +
                "{i=0}\n" +
                "{i}";
            Lexar lexar = new(code);
            List<Token> tokens = lexar.Parse();
        }
    }
}
