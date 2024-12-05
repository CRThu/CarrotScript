using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Interpreter
{
    public class RuntimeEnvironment
    {
        private readonly Dictionary<string, object> _variables = new();
        private readonly TargetManager _targetManager;

        public RuntimeEnvironment(TargetManager targetManager)
        {
            _targetManager = targetManager;
        }

        public void SetVariable(string name, object value)
        {
            _variables[name] = value;
        }

        public object GetVariable(string name)
        {
            if (!_variables.ContainsKey(name))
            {
                _variables[name] = 0;
            }
            return _variables[name];
        }

        public void PrintSnapshot()
        {
            Console.WriteLine("--- RuntimeEnvironment ---");
            foreach (var key in _variables.Keys)
            {
                Console.WriteLine($"{key}={_variables[key]}");
            }
        }

        public void Retarget(string targetName)
        {
            _targetManager.SetTarget(targetName);
        }

        public void Write(ReadOnlySpan<char> content)
        {
            _targetManager.Current.Write(content);
        }

        public ReadOnlySpan<char> Read()
        {
            return _targetManager.Current.Read();
        }
    }
}
