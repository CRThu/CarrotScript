using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl.Parser
{
    public abstract class NodeBase : TreeNode<Token>
    {
        public string Type { get; set; }

        public NodeBase(Token token) : base(token)
        {
            Value = token;
            Type = "<empty>";
        }

        public NodeBase(Token token, Token? left = null, Token? right = null) : base(token, left, right)
        {
            Value = token;
            Type = "<empty>";
        }

        public override string ToString()
        {
            return $"{{ type: {Type}, value: {base.ToString()} }}";
        }
    }

    public class NumericNode : NodeBase
    {
        public NumericNode(Token token) : base(token)
        {
            Type = "Numberic";
        }
    }

    public class StringNode : NodeBase
    {
        public StringNode(Token token) : base(token)
        {
            Type = "String";
        }
    }


    public class UnaryNode : NodeBase
    {
        public UnaryNode(Token op, Token right) : base(op)
        {
            Type = "Unary";
        }

        public override string ToString()
        {
            return $"{{ type: {Type}, op:{Value} }}";
        }
    }
}
