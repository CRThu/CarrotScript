using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript
{
    public static class LangDef
    {
        public static string DIGITS =
            "0123456789."
        ;

        public static string STRINGS =
            ".;"
            ;

        public static string[] UNARY_OPERATORS = [
            "+", "-"
            ];

        public static string[] BINARY_OPERATORS = [
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

        /// <summary>
        /// Token类型
        /// </summary>
        public enum TokenType
        {
            NUM,
            STR,

            OPERATOR,
            KEYWORDS,
            IDENTIFY,
            DELIMITER
        }
    }
}
