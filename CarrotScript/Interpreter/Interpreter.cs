using CarrotScript.Exception;
using CarrotScript.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def.NodeType;

namespace CarrotScript.Interpreter
{
    public class Interpreter
    {
        public RuntimeEnvironment Environment { get; set; }

        public Interpreter(RuntimeEnvironment environment)
        {
            Environment = environment;
        }

        public void Execute(ASTNode node)
        {
            if (node.Type == Program)
            {
                foreach (var statement in ((ProgramNode)node).Children)
                {
                    Execute(statement);
                }
            }
            else if (node.Type == AssignStatement)
            {
                var value = Evaluate(((AssignNode)node).Value);
                Environment.SetVariable(((AssignNode)node).VariableName, value);
            }
            else if (node.Type == PrintStatement)
            {
                foreach (var expression in ((PrintNode)node).Children)
                {
                    var result = Evaluate(expression);
                    Environment.Print(result);
                }
                Environment.Print("\r\n");
            }
            else
                throw new InvalidSyntaxException(node.Span);
        }

        private object Evaluate(ExpressionNode expression)
        {
            if (expression.Type == Literal)
            {
                return ((LiteralNode)expression).Variable;
            }
            else if (expression.Type == Identifier)
            {
                return Environment.GetVariable(((VariableNode)expression).Identifier);
            }
            else if (expression.Type == BinaryExpression)
            {
                var left = Convert.ToDouble(Evaluate(((BinaryOpNode)expression).Left));
                var right = Convert.ToDouble(Evaluate(((BinaryOpNode)expression).Right));
                return ((BinaryOpNode)expression).Operator switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    "/" => left / right,
                    _ => throw new InvalidSyntaxException(expression.Span)
                };
            }
            else
            {
                throw new InvalidSyntaxException(expression.Span);
            }
        }
    }
}
