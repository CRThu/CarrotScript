using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Interpreter
{
    public interface ITarget : IDisposable
    {
        public string Name { get; }
        public void Write(ReadOnlySpan<char> data);
        public ReadOnlySpan<char> Read();
    }

    public class ConsoleTarget : ITarget
    {
        public string Name => "console";

        public void Write(ReadOnlySpan<char> data)
        {
            Console.WriteLine(data.ToString());
        }

        public ReadOnlySpan<char> Read()
        {
            return Console.ReadLine();
        }

        public void Dispose()
        {

        }
    }

    public class FileTarget : ITarget
    {
        public string Name { get; set; }

        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public FileTarget(string filepath)
        {
            Name = $"file:{filepath}";
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _reader = new StreamReader(fs);
            _writer = new StreamWriter(fs) { AutoFlush = true };
        }

        public void Write(ReadOnlySpan<char> data)
        {
            _writer.Write(data);
        }

        public ReadOnlySpan<char> Read()
        {
            return _reader.ReadLine();
        }

        public void Dispose()
        {
            _reader.Dispose();
            _writer.Dispose();
        }
    }
}
