using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    public static class LangDef
    {
        public static string DIGITS = 
            "0123456789."
        ;

        public static string STRINGS =
            ".;"
            ;

        public static string[] OPERATORS = [
            "+", "-", "*", "/", "%", "**",
            "==", "!=", ">", ">=", "<", "<=",
            "="
        ];

        public static string[] DELIMITERS = [
            "\r", "\n", "\t", " ",
            "(", ")",
            "{", "}",
            "<<", ">>",
            ":",
            "#",
        ];

        public static string[] KEYWORDS = [
            "def",
            "begin", "end",
            "if", "elif", "else",
            "for", "while",
            "true", "false",
        ];
    }
}
