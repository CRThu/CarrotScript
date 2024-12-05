using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Interpreter
{
    public class TargetManager
    {
        public readonly Dictionary<string, ITarget> _targets = new();

        public ITarget Current { get; private set; }

        public TargetManager()
        {
            // default
            RegisterTarget(new ConsoleTarget());
            SetTarget("console");
        }

        public void RegisterTarget(ITarget target)
        {
            if (_targets.ContainsKey(target.Name))
                throw new InvalidOperationException();
            _targets.Add(target.Name, target);
        }

        public void SetTarget(string targetName)
        {
            if (_targets.TryGetValue(targetName, out var target))
            {
                Current = target;
            }
            else
                throw new InvalidOperationException();
        }

        public void Cleanup()
        {
            foreach (var target in _targets.Values)
            {
                target.Dispose();
            }
            _targets.Clear();
        }
    }
}
