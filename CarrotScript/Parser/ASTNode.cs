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
    public abstract class ASTNode : NodeBase<Token>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NodeType Type { get; set; }

        public TokenSpan Span => GetSpan();

        public ASTNode() : base()
        {
            Type = NodeType.UNKNOWN;
        }

        public ASTNode(Token token) : base(token)
        {
            Type = NodeType.UNKNOWN;
        }

        public ASTNode(Token token, ASTNode? left = null, ASTNode? right = null) : base(token, left, right)
        {
            Type = NodeType.UNKNOWN;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {
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
        public ProgramNode(Token token) : base(token)
        {
            Type = NodeType.Program;
        }
    }

    public class IdentifierNode : ASTNode
    {
        public IdentifierNode(Token token) : base(token)
        {
            Type = NodeType.Identifier;
        }
    }

    public class VariableDeclarationNode : ASTNode
    {
        public VariableDeclarationNode(Token token) : base(token)
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
        public UnaryOpNode(Token op, ASTNode right) : base(op, left: null, right: right)
        {
            Type = NodeType.UnaryExpression;
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public BinaryOpNode(Token op, ASTNode left, ASTNode right) : base(op, left: left, right: right)
        {
            Type = NodeType.BinaryExpression;
        }
    }
}
