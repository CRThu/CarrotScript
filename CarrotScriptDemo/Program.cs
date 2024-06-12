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
            Runtime runtime = new Runtime();

            Console.WriteLine($"CarrotScript {CarrotScript.Version.GetVersion()}");
            Console.WriteLine($"Command Line Runner");
            while (true)
            {
                Console.Write($">>");
                var l = Console.ReadLine();

                if (string.IsNullOrEmpty(l))
                    continue;

                runtime.Emit(l);
            }
        }
    }
}
