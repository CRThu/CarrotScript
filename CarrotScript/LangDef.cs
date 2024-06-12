using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.LangDef.TokenType;

namespace CarrotScript
{
    public static class LangDef
    {
        //public static string DIGITS =
        //    "0123456789."
        //;

        //public static string STRINGS =
        //    ".;"
        //    ;

        //public static string[] UNARY_OPERATORS = [
        //    "+", "-"
        //    ];

        //public static string[] BINARY_OPERATORS = [
        //    "+", "-", "*", "/", "%", "**",
        //    "==", "!=", ">", ">=", "<", "<=",
        //    "="
        //];

        //public static string[] DELIMITERS = [
        //    "\r", "\n", "\t", " ",
        //    "(", ")",
        //    "{", "}",
        //    "<<", ">>",
        //    ":",
        //    "#",
        //];

        //public static string[] KEYWORDS = [
        //    "def",
        //    "begin", "end",
        //    "if", "elif", "else",
        //    "for", "while",
        //    "true", "false",
        //];

        /// <summary>
        /// Token类型
        /// </summary>
        //public enum TokenType
        //{
        //    NUM,
        //    STR,

        //    OPERATOR,
        //    KEYWORDS,
        //    IDENTIFY,
        //    DELIMITER
        //}

        public readonly static FrozenDictionary<string, TokenType> L2TokenDict = new Dictionary<string, TokenType>()
        {
            { "**", POW },
        }.ToFrozenDictionary();

        public readonly static FrozenDictionary<string, TokenType> L1TokenDict = new Dictionary<string, TokenType>()
        {
            { "+", ADD },
            { "-", SUB },
            { "*", MUL },
            { "/", DIV },

            { "(", LPAREN },
            { ")", RPAREN },

            { " ", SPACE },
            { "\n", NEWLINE },

            { "if", IF },
            { "elif", ELIF },
            { "else", ELSE },

            { "for", FOR },
            { "while", WHILE },

        }.ToFrozenDictionary();

        public readonly static (FrozenDictionary<string, TokenType> dict, int len)[] LLTokenDict =
        [
            ( L2TokenDict!, 2 ),
            ( L1TokenDict!, 1 )
        ];

        public enum TokenType
        {
            UNKNOWN,

            NUMERIC,    // Const Numberic
            STRING,     // Const String

            ADD,        //  +
            SUB,        //  -
            MUL,        //  *
            DIV,        //  /
            POW,        //  **

            LPAREN,     //  (
            RPAREN,     //  )

            SPACE,      // <SPACE>
            NEWLINE,    // \n

            IF,         // if
            ELIF,       // elif
            ELSE,       // else

            FOR,        // for
            WHILE,      // while
        }

        public readonly static TokenType[] DELIMITERS = [
            SPACE,
            NEWLINE,

            LPAREN,
            RPAREN,
        ];

        public readonly static TokenType[] KEYWORDS = [
            IF,
            ELIF,
            ELSE,

            FOR,
            WHILE,
        ];


        public readonly static TokenType[] OPERATORS = [
            ADD,
            SUB,
            MUL,
            DIV,
            POW,
        ];


        public readonly static TokenType[] SINGLE_TYPE = [
            NUMERIC,
            STRING,
        ];

        public readonly static TokenType[] UNARYOP_TYPE = [
            ADD,
            SUB,
        ];

        public readonly static TokenType[] FACTOR_TYPE = [
            MUL,
            DIV,
        ];

        public readonly static TokenType[] TERM_TYPE = [
            ADD,
            SUB,
        ];

    }
}
