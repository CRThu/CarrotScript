using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    public static class LangDef
    {
        public static string[] OPERATORS = new string[] {
            "+", "-", "*", "/", "%", "**",
            "==", "!=", ">", ">=", "<", "<=",
            "="
        };

        public static string[] DELIMITERS = new string[] {
            "\r", "\n", "\t", " ",
            "(", ")",
            "{", "}",
            "<<", ">>",
            ":",
            "#",
        };

        public static string[] KEYWORDS = new string[]
        {
            "def",
            "begin", "end",
            "if", "elif", "else",
            "for", "while",
        };
    }
}
