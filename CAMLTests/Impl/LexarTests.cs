using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAML.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAML.Impl.Tests
{
    [TestClass()]
    public class LexarTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Lexar lexar = new("2**3\t+ 7.9E2+7*E6/-.32+(1+2*_x_)*-5");
            lexar.Parse();
            Assert.AreEqual("Token: {Value : 2}", lexar.Tokens[0].ToString());
            Assert.AreEqual("Token: {Operator : **}", lexar.Tokens[1].ToString());
            Assert.AreEqual("Token: {Value : 3}", lexar.Tokens[2].ToString());
            Assert.AreEqual("Token: {Operator : +}", lexar.Tokens[3].ToString());
            Assert.AreEqual("Token: {Value : 7.9E2}", lexar.Tokens[4].ToString());
            Assert.AreEqual("Token: {Operator : +}", lexar.Tokens[5].ToString());
            Assert.AreEqual("Token: {Value : 7}", lexar.Tokens[6].ToString());
            Assert.AreEqual("Token: {Operator : *}", lexar.Tokens[7].ToString());
            Assert.AreEqual("Token: {Variable : E6}", lexar.Tokens[8].ToString());
            Assert.AreEqual("Token: {Operator : /}", lexar.Tokens[9].ToString());
            Assert.AreEqual("Token: {Value : -.32}", lexar.Tokens[10].ToString());
            Assert.AreEqual("Token: {Operator : +}", lexar.Tokens[11].ToString());
            Assert.AreEqual("Token: {Operator : (}", lexar.Tokens[12].ToString());
            Assert.AreEqual("Token: {Value : 1}", lexar.Tokens[13].ToString());
            Assert.AreEqual("Token: {Operator : +}", lexar.Tokens[14].ToString());
            Assert.AreEqual("Token: {Value : 2}", lexar.Tokens[15].ToString());
            Assert.AreEqual("Token: {Operator : *}", lexar.Tokens[16].ToString());
            Assert.AreEqual("Token: {Variable : _x_}", lexar.Tokens[17].ToString());
            Assert.AreEqual("Token: {Operator : )}", lexar.Tokens[18].ToString());
            Assert.AreEqual("Token: {Operator : *}", lexar.Tokens[19].ToString());
            Assert.AreEqual("Token: {Value : -5}", lexar.Tokens[20].ToString());
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