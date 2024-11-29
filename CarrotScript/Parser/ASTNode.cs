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

        public TokenSpan Span { get; }

    }

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

        /*
        public ASTBinaryNode(ASTNode val, ASTNode? left = null, ASTNode? right = null) : base(val, left, right)
        {
            Type = NodeType.UNKNOWN;
        }
        */
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


    public static class AstNodeDebugEx
    {
        public static string ToTree(this ASTNode node, int level = 0)
        {
            var indent = new string(' ', level * 2);
            var builder = new StringBuilder();

            builder.Append(indent);
            builder.Append($"[{node.Type}]({node})[{node.Span}]");
            builder.AppendLine();

            if (node.Type == NodeType.Program)
            {
                foreach (var child in ((ProgramNode)node).Children)
                {
                    builder.AppendLine(child.ToTree(level + 1));
                }
            }
            else if (node.Type == NodeType.PrintStatement)
            {
                foreach (var child in ((PrintNode)node).Children)
                {
                    builder.AppendLine(child.ToTree(level + 1));
                }
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
        }
    }

    public abstract class StatementNode : ASTNode
    {
    }

    public class AssignNode : StatementNode
    {
        public string VariableName { get; set; }
        public ExpressionNode Value { get; set; }

        public AssignNode(string variableName, ExpressionNode value)
        {
            VariableName = variableName;
            Value = value;
            Type = NodeType.AssignStatement;
        }
    }


    public class PrintNode : StatementNode
    {
        public List<ExpressionNode> Children { get; set; }

        public PrintNode(ExpressionNode val)
        {
            Type = NodeType.PrintStatement;
            Children = [val];
        }

        public PrintNode(IEnumerable<ExpressionNode> val)
        {
            Type = NodeType.PrintStatement;
            Children = new List<ExpressionNode>(val);
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
    }


    public class VariableNode : ExpressionNode
    {
        public string Identifier { get; set; }

        public VariableNode(string val)
        {
            Identifier = val;
            Type = NodeType.Identifier;
        }
    }

    public class LiteralNode : ExpressionNode
    {
        public string Variable { get; set; }

        public LiteralNode(string val)
        {
            Variable = val;
            Type = NodeType.Literal;
        }

        public override string ToString()
        {
            return $"Variable = {Variable}";
        }
    }

    public class NullNode : ExpressionNode
    {
        public new TokenSpan Span { get; set; }

        public NullNode(TokenSpan span) : base()
        {
            Type = NodeType.NULL;
            Span = span;
        }
    }

    public class UnaryOpNode : ASTNode
    {
        public UnaryOpNode(ExpressionNode op, ExpressionNode right)
        {
            Type = NodeType.UnaryExpression;
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public BinaryOpNode(ExpressionNode op, ExpressionNode left, ExpressionNode right)
        {
            Type = NodeType.BinaryExpression;
        }
    }
}
