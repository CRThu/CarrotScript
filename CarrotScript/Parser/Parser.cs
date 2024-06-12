using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarrotScript.Exception;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Parser
{
    public class Parser
    {
        public Token? CurrentToken => GetToken();
        public int Index { get; set; }
        public List<Token> Tokens { get; set; }
        public bool DebugInfo { get; set; }

        public Parser(IEnumerable<Token> tokens, bool debugInfo = false)
        {
            Tokens = new List<Token>(tokens);
            DebugInfo = debugInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Token? GetToken(int offset = 0)
        {
            if (Index + offset >= 0 && Index + offset < Tokens.Count)
            {
                return Tokens[Index + offset];
            }
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Token? Advance()
        {
            Index++;
            return CurrentToken;
        }

        /// <summary>
        /// BNF GRAMMER<br/>
        /// ATOM        ::=     ( NUM | STR | IDENTIFER ) | ( "(" EXPR ")" )<br/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LexarNotSupportException"></exception>
        public ASTNode AtomProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            Token token = CurrentToken.Value;
            if (SINGLE_TYPE.Contains(token.Type))
            {
                Advance();
                return token.Type switch
                {
                    TokenType.NUMERIC => new NumericNode(token),
                    TokenType.STRING => new StringNode(token),
                    _ => throw new LexarNotSupportException(),
                };
            }

            if (token.Type == TokenType.LPAREN)
            {
                Advance();
                var node = ExprProc();
                Token? token2 = CurrentToken;
                if (token2 != null && token2.Value.Type != TokenType.RPAREN)
                    throw new LexarNotSupportException();
                Advance();
                return node;
            }

            throw new LexarNotSupportException();
        }

        public ASTNode FactorProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            Token token = CurrentToken.Value;
            if (UNARYOP_TYPE.Contains(token.Type))
            {
                Advance();
                ASTNode right = AtomProc();
                return new UnaryOpNode(token, right);
            }
            else
                return AtomProc();
        }


        public ASTNode BinaryOpProc(Func<ASTNode> func, TokenType[] ops)
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            var left = func.Invoke();
            Token? token = CurrentToken;
            if (token != null && ops.Contains(token.Value.Type))
            {
                Advance();
                var right = func.Invoke();

                return new BinaryOpNode(token.Value, left, right);
            }
            else
            {
                return left;
            }
        }


        public ASTNode TermProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            return BinaryOpProc(FactorProc, FACTOR_TYPE);

            throw new LexarNotSupportException();
        }

        public ASTNode CalcProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            return BinaryOpProc(TermProc, TERM_TYPE);

            throw new LexarNotSupportException();
        }

        public ASTNode ExprProc()
        {
            return CalcProc();
        }

        public ASTNode Parse()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            if (DebugInfo)
                Console.WriteLine("Parser.Parse():");

            var ast = ExprProc();

            if (CurrentToken != null)
                throw new LexarNotSupportException();

            if (DebugInfo)
                Console.WriteLine(ast);

            return ast;
        }
    }
}
