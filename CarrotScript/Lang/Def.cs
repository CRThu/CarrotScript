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
            {"{", LCUB },
            {"}", RCUB },
            { "(", LP },
            { ")", RP },

        }.ToFrozenDictionary();

        public enum XmlLexarState
        {
            Content,
            TagName,
            AttrName,
            AttrValue,
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
            XML_SINGLE_TAG,
            XML_OPEN_TAG,
            XML_CONTENT,
            XML_CLOSE_TAG,
            XML_PI_TARGET,

            // CARROTSCRIPT
            TEXT,       // output text like: helloworld
            IDENTIFIER, // identifier like: a, b
            NUMBER,     // numbers like: 123.456
            OPERATOR,   // op like: +, -
            ASSIGNMENT, // assignment like: a=1
            LPAREN,     //  (
            RPAREN,     //  )
            LBRACE,     // {
            RBRACE,     // }

            /*
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

            // CarrotScript
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
            LCUB,       // '{'                  |   Left Curly Brace
            RCUB,       // '}'                  |   Right Curly Brace
            LP,         // '('                  |   
            RP,         // ')'                  |   
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

            Program,        // 

            Identifier,     // like: a,b,c
            Literal,        // like: 123

            // Statement
            Statement,
            PrintStatement,      // 

            // Declaration
            VariableDeclaration,   // 

            // Expression
            Expression,
            UnaryExpression,
            BinaryExpression,

            // Function
            Function,       // 
        }

        public static Symbol ToSymbol(this char c)
        {
            bool hasSymbol = SymbolDict.TryGetValue(c!.ToString(), out Symbol tok);
            return hasSymbol ? tok : CHAR;
        }
        public static Symbol? ToSymbol(this char? c)
        {
            if (c == null)
            {
                return null;
            }
            else
            {
                bool hasSymbol = SymbolDict.TryGetValue(c!.ToString(), out Symbol tok);
                return hasSymbol ? tok : CHAR;
            }
        }

        public static bool InRange(this char? c, (int start, int end) range)
        {
            return c != null && c >= range.start && c <= range.end;
        }

        public static bool InRange(this char c, (char start, char end) range)
        {
            return c >= range.start && c <= range.end;
        }

        public static bool IsLangDefNameChar(this char c)
        {
            return c.InRange(('A', 'Z'))
                || c.InRange(('a', 'z'))
                || c.InRange(('0', '9'))
                || c == ':'
                || c == '.'
                || c == '_'
                || c == '-'
                || c == '+';
        }

        public static bool IsLangDefIdentifierStartChar(this char c)
        {
            return c.InRange(('A', 'Z'))
                || c.InRange(('a', 'z'))
                || c == '_';
        }

        public static bool IsLangDefIdentifierChar(this char c)
        {
            return c.InRange(('A', 'Z'))
                || c.InRange(('a', 'z'))
                || c.InRange(('0', '9'))
                || c == '_';
        }

        public static bool IsLangDefNumberStartChar(this char c)
        {
            return c.InRange(('0', '9'))
                || c == '.'
                || c == '+'
                || c == '-';
        }

        public static bool IsLangDefNumberChar(this char c)
        {
            return c.InRange(('0', '9'))
                || c == '.'
                || c == '+'
                || c == '-'
                || c == 'E';
        }

        public static bool IsLangDefOperatorChar(this char c)
        {
            return  c == '+'
                || c == '-'
                || c == '*'
                || c == '/';
        }
    }
}
