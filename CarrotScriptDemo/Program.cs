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
            Console.WriteLine($"CarrotScript {CarrotScript.Version.GetVersion()}");
            Console.WriteLine($"Command Line Runner");

            while (true)
            {
                Console.WriteLine($">>");
                List<string> s = new List<string>();
                string? input;

                while (!string.IsNullOrEmpty(input = Console.ReadLine()))
                {
                    s.Add(input);
                }
                string code = string.Concat(s);

                Runtime runtime = new Runtime();
                runtime.Emit(code);
            }
        }
    }
}
