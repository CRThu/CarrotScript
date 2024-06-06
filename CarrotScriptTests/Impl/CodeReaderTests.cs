using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarrotScript.Impl.Lexar;

namespace CarrotScript.Impl.Tests
{
    [TestClass()]
    public class CodeReaderTests
    {

        [TestMethod()]
        public void NextTest()
        {
            CodeReader cr = new("123\nHelloworld");
            Assert.AreEqual(cr.AdvanceNext(), '1');
            Assert.AreEqual(cr.AdvanceNext(), '2');
            Assert.AreEqual(cr.AdvanceNext(), '3');
            Assert.AreEqual(cr.Cursor, 3);
        }

        [TestMethod()]
        public void GetCharTest()
        {
            CodeReader cr = new("123\nHelloworld");
            cr.Cursor = 4;
            Assert.AreEqual(cr.GetChar(), 'H');
            cr.Cursor = 5;
            Assert.AreEqual(cr.GetChar(), 'e');
            cr.Cursor = 6;
            Assert.AreEqual(cr.GetChar(), 'l');
        }

        [TestMethod()]
        public void GetNextCharTest()
        {
            CodeReader cr = new("123\nHelloworld");
            cr.Cursor = 4;
            Assert.AreEqual( cr.GetNext(),'H');
            cr.Cursor = 5;
            Assert.AreEqual(cr.GetNext(3), 'o');
        }

        [TestMethod()]
        public void HasNextTest()
        {
            CodeReader cr = new("123\nHelloworld");
            cr.Cursor = 6;
            Assert.AreEqual(cr.HasNext(), true);
            cr.Cursor = 13;
            Assert.AreEqual(cr.HasNext(), true);
            cr.Cursor = 14;
            Assert.AreEqual(cr.HasNext(), false);
        }
    }
}