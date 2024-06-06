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
            CodeReader cr = new("123\nHelloworld");\
            Assert.AreEqual(cr.Cursor, 3);
        }
    }
}