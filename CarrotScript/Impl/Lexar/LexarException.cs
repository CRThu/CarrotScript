using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl.Lexar
{
    public class LexarNotSupportException : LexarException
    {
        public LexarNotSupportException(TokenPosition pos)
        : base("LexarNotSuppportException", pos)
        {
        }
    }

    public class LexarException : Exception
    {
        public string Msg { get; set; }
        public TokenPosition Position { get; set; }

        public LexarException(string msg, TokenPosition pos)
        {
            Msg = msg;
            Position = pos;
        }

        public override string ToString()
        {
            return Msg + Environment.NewLine + base.ToString();
        }
    }
}
