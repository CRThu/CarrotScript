using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarrotScript.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl.Tests
{
    [TestClass()]
    public class LexarTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Lexar lexar = new("2**3\t+ 7.9E2+7*E6/-.32+(1+2*_x_)*-5");
            lexar.Parse();
            List<Token> expTokens = new() {
                new Token(TokenType.Value,      "2"),
                new Token(TokenType.Operator,   "**"),
                new Token(TokenType.Value,      "3"),
                new Token(TokenType.Operator,   "+"),
                new Token(TokenType.Value,      "7.9E2"),
                new Token(TokenType.Operator,   "+"),
                new Token(TokenType.Value,      "7"),
                new Token(TokenType.Operator,   "*"),
                new Token(TokenType.Variable,   "E6"),
                new Token(TokenType.Operator,   "/"),
                new Token(TokenType.Value,      "-.32"),
                new Token(TokenType.Operator,   "+"),
                new Token(TokenType.Operator,   "("),
                new Token(TokenType.Value,      "1"),
                new Token(TokenType.Operator,   "+"),
                new Token(TokenType.Value,      "2"),
                new Token(TokenType.Operator,   "*"),
                new Token(TokenType.Variable,   "_x_"),
                new Token(TokenType.Operator,   ")"),
                new Token(TokenType.Operator,   "*"),
                new Token(TokenType.Value,      "-5")
            };
            foreach (var (exp, lex) in expTokens.Zip(lexar.Tokens))
            {
                Assert.AreEqual(exp, lex);
            }
        }

        [TestMethod()]
        public void ParseOperatorTest()
        {
            Lexar lexar = new("+--*%**//");
            Assert.AreEqual("+", lexar.ParseOperator().Value);
            Assert.AreEqual("-", lexar.ParseOperator().Value);
            Assert.AreEqual("-", lexar.ParseOperator().Value);
            Assert.AreEqual("*", lexar.ParseOperator().Value);
            Assert.AreEqual("%", lexar.ParseOperator().Value);
            Assert.AreEqual("**", lexar.ParseOperator().Value);
            Assert.AreEqual("/", lexar.ParseOperator().Value);
            Assert.AreEqual("/", lexar.ParseOperator().Value);
        }

        [TestMethod()]
        public void ParseValueTest()
        {
            Lexar lexar = new("1 -1.233E-02 1E-3 .1 +.0E-3 -3.46e+02 6.0E2+3E+3");
            Assert.AreEqual("1", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual("-1.233E-02", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual("1E-3", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual(".1", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual("+.0E-3", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual("-3.46e+02", lexar.ParseValue().Value);
            lexar.cr.Next();
            Assert.AreEqual("6.0E2", lexar.ParseValue().Value);
            Assert.AreEqual("+3E+3", lexar.ParseValue().Value);
        }

        [TestMethod()]
        public void ParseVariableTest()
        {
            Lexar lexar = new("x E _x _ x7E_ c7");
            Assert.AreEqual("x", lexar.ParseVariable().Value);
            lexar.cr.Next();
            Assert.AreEqual("E", lexar.ParseVariable().Value);
            lexar.cr.Next();
            Assert.AreEqual("_x", lexar.ParseVariable().Value);
            lexar.cr.Next();
            Assert.AreEqual("_", lexar.ParseVariable().Value);
            lexar.cr.Next();
            Assert.AreEqual("x7E_", lexar.ParseVariable().Value);
            lexar.cr.Next();
            Assert.AreEqual("c7", lexar.ParseVariable().Value);
        }
    }
}