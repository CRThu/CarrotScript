using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
{
    public static class LangDef
    {
        public static string[] DIGITS = [
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "."
        ];

        public static string[] STRINGS = [
            "\"","\'"
        ];

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
        ];
    }
}
