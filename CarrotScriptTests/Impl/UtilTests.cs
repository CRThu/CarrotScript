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
    public class UtilTests
    {
        [TestMethod()]
        public void FindMatchHeaderTest()
        {
            string[] a = new string[] { "bb", "cc", "a", "b" };
            Assert.AreEqual(Util.FindMatchHeader(a, 'a', isPrecise: false), "a");
            Assert.AreEqual(Util.FindMatchHeader(a, 'b', isPrecise: false), "bb");
            Assert.AreEqual(Util.FindMatchHeader(a, 'c', isPrecise: false), "cc");
            Assert.AreEqual(Util.FindMatchHeader(a, 'd', isPrecise: false), null);
            Assert.AreEqual(Util.FindMatchHeader(a, 'a', isPrecise: true), "a");
            Assert.AreEqual(Util.FindMatchHeader(a, 'b', isPrecise: true), "b");
            Assert.AreEqual(Util.FindMatchHeader(a, 'c', isPrecise: true), null);
            Assert.AreEqual(Util.FindMatchHeader(a, 'd', isPrecise: true), null);
        }

        [TestMethod()]
        public void FindMatchHeaderTest1()
        {
            string[] a = new string[] { "bb", "cc", "a", "b" };
            Assert.AreEqual(Util.FindMatch(a, "a", isPrecise: false), "a");
            Assert.AreEqual(Util.FindMatch(a, "b", isPrecise: false), "bb");
            Assert.AreEqual(Util.FindMatch(a, "c", isPrecise: false), "cc");
            Assert.AreEqual(Util.FindMatch(a, "d", isPrecise: false), null);
            Assert.AreEqual(Util.FindMatch(a, "a", isPrecise: true), "a");
            Assert.AreEqual(Util.FindMatch(a, "b", isPrecise: true), "b");
            Assert.AreEqual(Util.FindMatch(a, "c", isPrecise: true), null);
            Assert.AreEqual(Util.FindMatch(a, "d", isPrecise: true), null);
        }
    }
}