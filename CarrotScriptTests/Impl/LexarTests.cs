using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            CarrotScript.Lexar.Lexar lexar = new("2**3\t+ 7.9E2+7*E6/-.32+(1+2*_x_)*-5");
            lexar.Parse();
        }
    }
}