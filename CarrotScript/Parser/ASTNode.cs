using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static CarrotScript.Lang.Def;
using System.Text.Json.Serialization;

namespace CarrotScript.Parser
{
    public abstract class ASTNode : NodeBase<Token>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TokenType Type { get; set; }

        public ASTNodePosition Position => GetPosition();

        public ASTNode(Token token) : base(token)
        {
            Type = TokenType.UNKNOWN;
        }

        public ASTNode(Token token, ASTNode? left = null, ASTNode? right = null) : base(token, left, right)
        {
            Type = TokenType.UNKNOWN;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private ASTNodePosition GetPosition()
        {
            TokenPosition start = (Left == null) ? Value.Pos : Left.Value.Pos;
            TokenPosition end = (Right == null) ? Value.Pos : Right.Value.Pos;
            return new ASTNodePosition(ref start, ref end);
        }
    }

    public class NumericNode : ASTNode
    {
        public NumericNode(Token token) : base(token)
        {
            Type = TokenType.NUMERIC;
        }
    }

    public class StringNode : ASTNode
    {
        public StringNode(Token token) : base(token)
        {
            Type = TokenType.STRING;
        }
    }

    public class UnaryOpNode : ASTNode
    {
        public UnaryOpNode(Token op, ASTNode right) : base(op, left: null, right: right)
        {
            Type = op.Type;
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public BinaryOpNode(Token op, ASTNode left, ASTNode right) : base(op, left: left, right: right)
        {
            Type = op.Type;
        }
    }
}
