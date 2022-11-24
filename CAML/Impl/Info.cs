using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAML.Impl
{
    public static class Info
    {
        public readonly static string TOKEN_END = " \r\n\t";
        public readonly static string FILE_END = "\0";

        public static string[] OPERATORS = new string[] { "+", "-", "*", "/", "%", "**" };

    }
}
