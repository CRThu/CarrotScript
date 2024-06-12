using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Exception
{
    public class ExceptionBase : System.Exception
    {
        public string Msg { get; set; }
        public TokenPosition Position { get; set; }

        public ExceptionBase(string msg)
        {
            Msg = msg;
        }

        public ExceptionBase(string msg, TokenPosition pos)
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
