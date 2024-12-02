using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static CarrotScript.Lang.Def;
using System.Text.Json.Serialization;
using CarrotScript.Reader;

namespace CarrotScript.Parser
{
    public abstract class ASTNode
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NodeType Type { get; set; }

        public TokenSpan Span { get; set; }

    }

    /*
    public abstract class ASTBinaryNode : NodeBase<ASTNode>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NodeType Type { get; set; }

        public TokenSpan Span => GetSpan();

        public ASTBinaryNode() : base()
        {
            Type = NodeType.UNKNOWN;
        }

        public ASTBinaryNode(ASTNode val) : base(val)
        {
            Type = NodeType.UNKNOWN;
        }

        public ASTBinaryNode(ASTNode val, ASTNode? left = null, ASTNode? right = null) : base(val, left, right)
        {
            Type = NodeType.UNKNOWN;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private TokenSpan GetSpan()
        {
            CodePosition startPos = (Left == null) ? Value.Span.Start : Left.Value.Span.Start;
            CodePosition endPos = (Right == null) ? Value.Span.End : Right.Value.Span.End;
            return new TokenSpan(startPos, endPos);
        }
    }
    */

    public static class AstNodeDebugEx
    {
        public static string ToTree(this ASTNode node, int level = 0)
        {
            var indent = new string(' ', level * 2);
            var builder = new StringBuilder();

            builder.Append(indent);
            builder.Append($"[{node.Type}] ({node}) [{node.Span}]");
            builder.AppendLine();

            if (node.Type == NodeType.Program)
            {
                foreach (var child in ((ProgramNode)node).Children)
                {
                    builder.Append(child.ToTree(level + 1));
                }
            }
            else if (node.Type == NodeType.PrintStatement)
            {
                foreach (var child in ((PrintNode)node).Children)
                {
                    builder.Append(child.ToTree(level + 1));
                }
            }
            else if (node.Type == NodeType.UnaryExpression)
            {
                builder.Append(((UnaryOpNode)node).Right.ToTree(level + 1));
            }
            else if (node.Type == NodeType.BinaryExpression)
            {
                builder.Append(((BinaryOpNode)node).Left.ToTree(level + 1));
                builder.Append(((BinaryOpNode)node).Right.ToTree(level + 1));
            }

            return builder.ToString();
        }
    }

    public class ProgramNode : ASTNode
    {
        public List<StatementNode> Children { get; set; }

        public ProgramNode(IEnumerable<StatementNode> nodes)
        {
            Type = NodeType.Program;
            Children = new List<StatementNode>(nodes);
            Span = GetNestedSpan();
        }
        private TokenSpan GetNestedSpan()
        {
            CodePosition startPos = (Children == null || Children.Count == 0) ? default : Children.First().Span.Start;
            CodePosition endPos = (Children == null || Children.Count == 0) ? default : Children.Last().Span.End;
            return new TokenSpan(startPos, endPos);
        }
    }

    public abstract class StatementNode : ASTNode
    {
    }

    public class AssignNode : StatementNode
    {
        public string VariableName { get; set; }
        public ExpressionNode Value { get; set; }

        public AssignNode(string variableName, ExpressionNode value, TokenSpan span = default)
        {
            VariableName = variableName;
            Value = value;
            Span = span;
            Type = NodeType.AssignStatement;
        }
    }


    public class PrintNode : StatementNode
    {
        public List<ExpressionNode> Children { get; set; }

        public PrintNode(IEnumerable<ExpressionNode> val)
        {
            Type = NodeType.PrintStatement;
            Children = new List<ExpressionNode>(val);
            Span = GetNestedSpan();
        }

        private TokenSpan GetNestedSpan()
        {
            CodePosition startPos = (Children == null || Children.Count == 0) ? default : Children.First().Span.Start;
            CodePosition endPos = (Children == null || Children.Count == 0) ? default : Children.Last().Span.End;
            return new TokenSpan(startPos, endPos);
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
    }


    public class VariableNode : ExpressionNode
    {
        public string Identifier { get; set; }

        public VariableNode(string val, TokenSpan span = default)
        {
            Identifier = val;
            Type = NodeType.Identifier;
            Span = span;
        }

        public override string ToString()
        {
            return $"Identifier = {Identifier}";
        }
    }

    public class LiteralNode : ExpressionNode
    {
        public string Variable { get; set; }

        public LiteralNode(string val, TokenSpan span = default)
        {
            Variable = val;
            Type = NodeType.Literal;
            Span = span;
        }

        public override string ToString()
        {
            return $"Variable = {Variable}";
        }
    }

    public class NullNode : ExpressionNode
    {
        public NullNode(TokenSpan span) : base()
        {
            Type = NodeType.NULL;
            Span = span;
        }
    }

    public class UnaryOpNode : ExpressionNode
    {
        public string Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public UnaryOpNode(string op, ExpressionNode right, CodePosition opPos = default)
        {
            Type = NodeType.UnaryExpression;
            Operator = op;
            Right = right;

            // maybe wrong here
            Span = new(opPos, GetEndPosition());
        }

        public override string ToString()
        {
            return $"Operator = {Operator}";
        }

        private CodePosition GetEndPosition()
        {
            CodePosition endPos = (Right == null) ? default : Right.Span.End;
            return endPos;
        }
    }

    public class BinaryOpNode : ExpressionNode
    {
        public string Operator { get; set; }
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        public BinaryOpNode(string op, ExpressionNode left, ExpressionNode right)
        {
            Type = NodeType.BinaryExpression;
            Operator = op;
            Left = left;
            Right = right;

            // maybe wrong here
            Span = GetSpan();
        }

        public override string ToString()
        {
            return $"Operator = {Operator}";
        }

        private TokenSpan GetSpan()
        {
            CodePosition startPos = (Left == null) ? default : Left.Span.Start;
            CodePosition endPos = (Right == null) ? default : Right.Span.End;
            return new TokenSpan(startPos, endPos);
        }
    }
}
