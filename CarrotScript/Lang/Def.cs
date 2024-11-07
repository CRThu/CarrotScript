using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def.TokenType;
using static CarrotScript.Lang.Def.Symbol;

namespace CarrotScript.Lang
{
    public static class Def
    {
        /*
        public readonly static char[] DIGITS = "0123456789".ToCharArray();
        public readonly static char[] LETTERS_AND_DIGITS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public readonly static char[] NUMERIC_SIGN = "+-".ToCharArray();
        public readonly static char[] NUMERIC_DIGITS = DIGITS;
        public readonly static char[] NUMERIC_DOTS = ".".ToCharArray();
        public readonly static char[] NUMERIC_EXPSYM = "eE".ToCharArray();

        public readonly static char[] DELIMITERS_SYM = "(){}:".ToCharArray();
        public readonly static char[] DELIMITERS_SPACE = " \r\t\n".ToCharArray();

        public readonly static char[] STRINGS_CHARS = [.. LETTERS_AND_DIGITS, .. "_.;[]".ToCharArray()];

        public readonly static char[] IDENTIFIER_CHARS = [.. LETTERS_AND_DIGITS, .. "_".ToCharArray()];
        public readonly static char[] IDENTIFIER_FIRST_CHAR_EXCLUDE = DIGITS;
        public readonly static char[] IDENTIFIER_FIRST_CHAR = IDENTIFIER_CHARS.Where(c => !IDENTIFIER_FIRST_CHAR_EXCLUDE.Contains(c)).ToArray();

        public readonly static string[] OPERATORS_SYM = [
            "+", "-", "*", "/", "%",
            "**",
            "==", "!=", ">", ">=", "<", "<=",
            "="
        ];

        public static string[] KEYWORDS_SYM = [
            "def",
            "begin", "end",
            "if", "elif", "else",
            "for", "while",
            "true", "false",
        ];

        //public static string[] TODO_DELIMITERS = [
        //    "<<", ">>",
        //    "#",
        //];
        */


        public readonly static FrozenDictionary<string, Symbol> SymbolDict = new Dictionary<string, Symbol>()
        {
            /*
            { "+", ADD },
            { "-", SUB },
            { "*", MUL },
            { "/", DIV },

            { "(", LPAREN },
            { ")", RPAREN },

            { " ", SPACE },

            { "**", POW },

            { "if", IF },
            { "elif", ELIF },
            { "else", ELSE },

            { "for", FOR },
            { "while", WHILE },
            */

            // CarrotXml
            {"<", LT },
            {">", GT },
            {"\"", QUOT },
            {"\'", QUOT },
            {"=", EQ },
            {"/", DIV },
            {" ", SP },
            {"\t", TAB },
            {"\r", CR },
            {"\n", LF },
            {"!", EXCL },
            {"?", QUEST },

        }.ToFrozenDictionary();

        public enum XmlState
        {
            XmlContent,
            XmlTagBegin,
            XmlTagName,
            XmlPiTagName,
            XmlAttrName,
            XmlAttrValue,
            XmlTagEnd
        }

        public enum ScriptState
        {
            Root
        }

        public enum TokenType
        {
            UNKNOWN,
            // ROOT
            ROOT,

            // CARROTXML
            XML_TAG_START,
            XML_ATTR_NAME,
            XML_ATTR_VALUE,
            XML_CONTENT,
            XML_TAG_END,
            XML_PI_TARGET,
            XML_PI_ATTR_NAME,
            XML_PI_ATTR_VALUE,

            /*
            NUMERIC,    // Const Numberic
            STRING,     // Const String
            IDENTIFIER, // Identifier

            ADD,        //  +
            SUB,        //  -
            MUL,        //  *
            DIV,        //  /
            POW,        //  **

            LPAREN,     //  (
            RPAREN,     //  )

            SPACE,      // <SPACE>

            IF,         // if
            ELIF,       // elif
            ELSE,       // else

            FOR,        // for
            WHILE,      // while
            */
        }

        public enum Symbol
        {
            // Default
            CHAR,       // Any Ascii            |   Ascii 

            // CarrotXml
            LT,         // '<'                  |   Less than
            GT,         // '>'                  |   Greater than
            QUOT,       // ''' | '"'            |   Quote
            EQ,         // '='                  |   Equals
            DIV,        // '/'                  |   Slash or Divide
            SP,         // ' '                  |   Space
            TAB,        // '\t'                 |   Horizontal Tab
            CR,         // '\r'                 |   Carriage Return
            LF,         // '\n'                 |   Line Feed
            EXCL,       // '!'                  |   Exclamation
            QUEST,      // '?'                  |   Question
        }

        /*
        public readonly static TokenType[] EXCLUDE_TOKENS = [
            SPACE
        ];

        public readonly static TokenType[] DELIMITERS = [
            SPACE,

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
        */
        public enum NodeType
        {
            UNKNOWN,

            NULL,           // NULL
            NUMERIC,        // Const Numberic
            STRING,         // Const String
            IDENTIFIER,     // Identifier

            UNARYOP,        // UNARYOP
            BINARYOP,       // BINARYOP
        }
    }
}
