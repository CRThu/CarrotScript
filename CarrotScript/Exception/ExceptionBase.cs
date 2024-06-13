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
        public TokenSpan? Position { get; set; }

        public ExceptionBase(string msg)
        {
            Msg = msg;
        }

        public ExceptionBase(string msg, TokenSpan? pos)
        {
            Msg = msg;
            Position = pos;
        }

        public override string ToString()
        {
            return Msg
                + Environment.NewLine
                + ((Position != null) ? Position!.Value : "Position = <NULL>")
                + Environment.NewLine
                + base.ToString();
        }
    }
}
