﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CarrotScript.Parser
{
    public abstract class ASTNode : NodeBase<Token>
    {
        public string Type { get; set; }

        public ASTNode(Token token) : base(token)
        {
            Value = token;
            Type = "<empty>";
        }

        public ASTNode(Token token, ASTNode? left = null, ASTNode? right = null) : base(token, left, right)
        {
            Type = "<empty>";
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
    }

    public class NumericNode : ASTNode
    {
        public NumericNode(Token token) : base(token)
        {
            Type = "Numberic";
        }
    }

    public class StringNode : ASTNode
    {
        public StringNode(Token token) : base(token)
        {
            Type = "String";
        }
    }


    public class UnaryNode : ASTNode
    {
        public UnaryNode(Token op, ASTNode right) : base(op, left: null, right: right)
        {
            Type = "Unary";
        }
    }

    public class BinaryNode : ASTNode
    {
        public BinaryNode(Token op, ASTNode left, ASTNode right) : base(op, left: left, right: right)
        {
            Type = "Binary";
        }
    }
}
