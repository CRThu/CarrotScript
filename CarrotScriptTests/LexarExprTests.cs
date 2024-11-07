using CarrotScript.Lexar;
using CarrotScript.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScriptTests
{
    [TestClass()]
    public class LexarExprTests
    {
        [TestMethod()]
        public void ParseExprStringTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "ABC");
            Lexar lexar = new(codeReader);
            var tokens = lexar.Parse();

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual("ABC", tokens[0].Value);
            Assert.AreEqual(TokenType.STRING, tokens[0].Type);
        }

        [TestMethod()]
        public void ParseExprNumIntPosTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "123");
            Lexar lexar = new(codeReader);
            var tokens = lexar.Parse();

            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual("123", tokens[0].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[0].Type);
        }

        [TestMethod()]
        public void ParseExprNumIntNegTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "-123");
            Lexar lexar = new(codeReader);
            var tokens = lexar.Parse();

            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual("-", tokens[0].Value);
            Assert.AreEqual(TokenType.SUB, tokens[0].Type);
            Assert.AreEqual("123", tokens[1].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[1].Type);
        }

        [TestMethod()]
        public void ParseExprAddTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "1+3");
            Lexar lexar = new(codeReader);
            var tokens = lexar.Parse();

            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual("1", tokens[0].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[0].Type);
            Assert.AreEqual("+", tokens[1].Value);
            Assert.AreEqual(TokenType.ADD, tokens[1].Type);
            Assert.AreEqual("3", tokens[2].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[2].Type);
        }

        [TestMethod()]
        public void ParseExprMulTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "1*3");
            Lexar lexar = new(codeReader);
            var tokens = lexar.Parse();

            Assert.AreEqual(3, tokens.Count);
            Assert.AreEqual("1", tokens[0].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[0].Type);
            Assert.AreEqual("*", tokens[1].Value);
            Assert.AreEqual(TokenType.MUL, tokens[1].Type);
            Assert.AreEqual("3", tokens[2].Value);
            Assert.AreEqual(TokenType.NUMERIC, tokens[2].Type);
        }

        [TestMethod()]
        public void ParseExprComplexTest()
        {
            CodeReader codeReader = new CodeReader("<FILE>", "2**3\t+ 7.9E2+7*E6/-.32+(1+2*_x_)*-5");
            Lexar lexar = new(codeReader);
            lexar.Parse();
        }
    }
}