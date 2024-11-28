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

    public class ProgramNode : ASTNode
    {
        public List<StatementNode> Nodes { get; set; }

        public ProgramNode(IEnumerable<StatementNode> nodes)
        {
            Type = NodeType.Program;
            Nodes = new List<StatementNode>(nodes);
        }
    }

    public class ExpressionNode : ASTNode
    {
        public ExpressionNode(ASTNode val)
        {
            Type = NodeType.Expression;
        }
    }

    public class StatementNode : ASTNode
    {
        public StatementNode(ASTNode val)
        {
            Type = NodeType.Statement;
        }
    }

    public class PrintNode : StatementNode
    {
        public PrintNode(ExpressionNode val) : base(val)
        {
            Type = NodeType.PrintStatement;
        }
    }

    public class IdentifierNode : ASTNode
    {
        public IdentifierNode(ASTNode val)
        {
            Type = NodeType.Identifier;
        }
    }

    public class VariableDeclarationNode : ASTNode
    {
        public VariableDeclarationNode(ASTNode val)
        {
            Type = NodeType.VariableDeclaration;
        }
    }

    public class NullNode : ASTNode
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
        public UnaryOpNode(ASTNode op, ASTNode right)
        {
            Type = NodeType.UnaryExpression;
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public BinaryOpNode(ASTNode op, ASTNode left, ASTNode right)
        {
            Type = NodeType.BinaryExpression;
        }
    }
}
