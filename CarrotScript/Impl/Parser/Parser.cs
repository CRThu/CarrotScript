using CarrotScript.Impl.Lexar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Impl.LangDef;

namespace CarrotScript.Impl.Parser
{
    public class Parser
    {
        public Token? CurrentToken => GetToken();
        public int Index { get; set; }
        public List<Token> Tokens { get; set; }
        public ASTNode AST { get; set; }

        public Parser(IEnumerable<Token> tokens)
        {
            Tokens = new List<Token>(tokens);
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

        public ASTNode VarExprProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            Token token = CurrentToken.Value;
            if (token.Type == TokenType.NUM)
                return new NumericNode(token);
            else if (token.Type == TokenType.STR)
                return new StringNode(token);
            else
                throw new LexarNotSupportException();

            // TODO ( & ) parse
        }

        public ASTNode UnaryOpExprProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            Token token = CurrentToken.Value;
            if (token.Type == TokenType.OPERATOR)
            {
                Advance();
                ASTNode right = VarExprProc();

                //  UNARYOP VAREXPR
                return new UnaryNode(token, right);
            }

            // VAREXPR
            return VarExprProc();

            throw new LexarNotSupportException();
        }


        public ASTNode BinaryOpExprProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            var left = UnaryOpExprProc();
            Advance();
            var token = CurrentToken.Value;
            // TODO BINARYOP
            if (token.Type == TokenType.OPERATOR)
            {
                Advance();
                var right = UnaryOpExprProc();

                //   VAREXPR BINARYOP VAREXPR
                return new BinaryNode(token, left, right);
            }

            throw new LexarNotSupportException();
        }


        public ASTNode CalcExprProc()
        {
            if (CurrentToken == null)
                throw new LexarNotSupportException();

            return BinaryOpExprProc();

            throw new LexarNotSupportException();
        }

        public ASTNode Parse()
        {
            return CalcExprProc();
        }
    }
}
