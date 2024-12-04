using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarrotScript.Exception;
using CarrotScript.Lexar;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lang.Def.TokenType;

namespace CarrotScript.Parser
{
    public class Parser
    {
        public Token? CurrentToken => GetToken(position);
        private int position;
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
        public Token? GetToken(int pos = 0)
        {
            if (position >= 0 && position < Tokens.Count)
            {
                return Tokens[position];
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
            position++;
            return GetToken(position - 1);
        }

        public Token? Expect(TokenType type)
        {
            if (CurrentToken != null && CurrentToken.Type == type)
                return Advance();
            else
                throw new InvalidSyntaxException(CurrentToken!.Span);
        }

        public ASTNode Parse()
        {
            return ParseProgram();
        }

        public ProgramNode ParseProgram()
        {
            List<StatementNode> nodes = new List<StatementNode>();
            while (CurrentToken != null)
            {
                nodes.Add(ParseStatement());
            }
            return new ProgramNode(nodes);
        }

        public StatementNode ParseStatement()
        {
            if (CurrentToken == null)
            {
                return null;
            }

            switch (CurrentToken.Type)
            {
                case TEXT:
                case LBRACE:
                    return ParsePrintStatement();
                    break;
                case ASSIGNMENT:
                    return ParseAssignment();
                    break;
                default:
                    throw new InvalidSyntaxException(CurrentToken!.Span);
                    break;
            }
        }

        private PrintNode ParsePrintStatement()
        {
            if (CurrentToken == null)
            {
                return null;
            }
            List<ExpressionNode> nodes = new List<ExpressionNode>();
            while (CurrentToken != null)
            {
                if (CurrentToken.Type == TEXT_END)
                {
                    Expect(TEXT_END);  // TEXT_END
                    return new PrintNode(nodes);
                }

                switch (CurrentToken.Type)
                {
                    case LBRACE:
                        Expect(LBRACE);  // LBRACE
                        while (CurrentToken.Type != RBRACE)
                            nodes.Add(ParseExpression());
                        Expect(RBRACE);  // RBRACE
                        break;
                    case TEXT:
                        nodes.Add(ParseText());
                        break;
                    default:
                        throw new InvalidSyntaxException(CurrentToken!.Span);
                        break;
                }
            }

            throw new InvalidSyntaxException(CurrentToken!.Span);
        }

        private AssignNode ParseAssignment()
        {
            if (CurrentToken == null)
            {
                return null;
            }
            Expect(ASSIGNMENT);
            string name = CurrentToken.Value;
            Advance();
            ExpressionNode value = ParseExpression();
            return new AssignNode(name, value);
        }

        private ExpressionNode ParseExpression()
        {
            if (CurrentToken == null)
            {
                return null;
            }
            //expr = ParsePosNegExpr();
            var expr = ParseAddSubExpr();

            return expr;
        }

        /*
        private ExpressionNode ParsePosNegExpr()
        {
            var expr = ParseMulDivExpr();
            return expr;
        }
        */

        private ExpressionNode ParseMulDivExpr()
        {
            if (CurrentToken == null)
            {
                return null;
            }
            var node = ParseBasic();

            while (CurrentToken.Type == OPERATOR
                && (CurrentToken.Value == "*"
                    || CurrentToken.Value == "/"))
            {
                var op = CurrentToken.Value;
                Expect(OPERATOR);      // *|/
                var right = ParseBasic();
                node = new BinaryOpNode(op, node, right);
            }
            return node;
        }

        private ExpressionNode ParseAddSubExpr()
        {
            if (CurrentToken == null)
            {
                return null;
            }
            var node = ParseMulDivExpr();

            while (CurrentToken.Type == OPERATOR
                && (CurrentToken.Value == "+"
                    || CurrentToken.Value == "-"))
            {
                var op = CurrentToken.Value;
                Expect(OPERATOR);      // +|-
                var right = ParseMulDivExpr();
                node = new BinaryOpNode(op, node, right);
            }
            return node;
        }

        private ExpressionNode ParseBasic()
        {
            if (CurrentToken == null)
            {
                return null;
            }

            ExpressionNode expression = null;
            switch (CurrentToken.Type)
            {
                case IDENTIFIER:
                    expression = new VariableNode(CurrentToken.Value, CurrentToken.Span);
                    Advance();
                    break;
                case NUMBER:
                case TEXT:
                    expression = new LiteralNode(CurrentToken.Value, CurrentToken.Span);
                    Advance();
                    break;
                case LPAREN:
                    Expect(LPAREN);      // LPAREN
                    expression = ParseExpression();
                    Expect(RPAREN);      // RPAREN
                    break;
                default:
                    throw new InvalidSyntaxException(CurrentToken!.Span);
                    break;
            }

            return expression;
        }

        private ExpressionNode ParseText()
        {
            if (CurrentToken == null)
            {
                return null;
            }

            ExpressionNode expression = null;
            switch (CurrentToken.Type)
            {
                case TEXT:
                    expression = new LiteralNode(CurrentToken.Value, CurrentToken.Span);
                    break;
                default:
                    throw new InvalidSyntaxException(CurrentToken!.Span);
                    break;
            }

            Advance();
            return expression;
        }

        /*
        public ASTNode Factor()
        {
            //ParserTrace trace = new ParserTrace();
            //trace.Check(Advance());
            Token? token = CurrentToken;
            if (token != null)
            {
                if (token.Value.Type == TokenType.XML_TAG_START)
                {
                    Advance();
                    ASTNode node = new StringNode(token.Value);
                }
                else
                {
                    throw new InvalidSyntaxException(token.Value.Span);
                }
            }
        }
        /// <summary>
        /// BNF GRAMMER<br/>
        /// ATOM        ::=     ( NUM | STR | IDENTIFER ) | ( "(" EXPR ")" )<br/>
        /// </summary>
        /// <returns></returns>
        public ASTNode AtomProc()
        {
            if (CurrentToken == null)
                throw new InvalidSyntaxException("异常结尾", null);

            Token token = CurrentToken.Value;
            if (SINGLE_TYPE.Contains(token.Type))
            {
                Advance();
                return token.Type switch {
                    TokenType.NUMERIC => new NumericNode(token),
                    TokenType.STRING => new StringNode(token),
                    _ => throw new ParserNotSupportedException("Parser不支持的Token类型", token.Span),
                };
            }

            if (token.Type == TokenType.LPAREN)
            {
                Advance();
                var node = ExprProc();
                Token? token2 = CurrentToken;
                if (token2 != null && token2.Value.Type != TokenType.RPAREN)
                    throw new InvalidSyntaxException("Parser未找到LPAREN Token", token2.Value.Span);
                Advance();
                return node;
            }

            throw new ParserNotSupportedException("Parser不支持的表达式", token.Span);
        }

        public ASTNode FactorProc()
        {
            if (CurrentToken == null)
                throw new InvalidSyntaxException("异常结尾", null);

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
                throw new InvalidSyntaxException("异常结尾", null);

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
                throw new InvalidSyntaxException("异常结尾", null);

            return BinaryOpProc(FactorProc, FACTOR_TYPE);

            throw new ParserNotSupportedException("Parser不支持的表达式", CurrentToken.Value.Span);
        }

        public ASTNode CalcProc()
        {
            if (CurrentToken == null)
                throw new InvalidSyntaxException("异常结尾", null);

            return BinaryOpProc(TermProc, TERM_TYPE);

            throw new ParserNotSupportedException("Parser不支持的表达式", CurrentToken.Value.Span);
        }

        public ASTNode ExprProc()
        {
            return CalcProc();
        }

        public ASTNode Parse()
        {
            if (CurrentToken == null)
                throw new InvalidSyntaxException("异常结尾", null);

            if (DebugInfo)
                Console.WriteLine("Parser.Parse():");

            var ast = ExprProc();

            if (CurrentToken != null)
                throw new InvalidSyntaxException("末尾存在非法Token", CurrentToken.Value.Span);

            if (DebugInfo)
                Console.WriteLine(ast);

            return ast;
        }
        */
    }
}
