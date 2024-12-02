using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Interpreter
{
    public class RuntimeEnvironment
    {
        private readonly Dictionary<string, object> variables = new();

        public void SetVariable(string name, object value)
        {
            variables[name] = value;
        }

        public object GetVariable(string name)
        {
            if (!variables.ContainsKey(name))
            {
                variables[name] = 0;
            }
            return variables[name];
        }

        public void PrintSnapshot()
        {
            Console.WriteLine("--- RuntimeEnvironment ---");
            foreach (var key in variables.Keys)
            {
                Console.WriteLine($"{key}={variables[key]}");
            }
        }

        internal void Print(object result)
        {
            Console.Write(result.ToString());
        }
    }
}
