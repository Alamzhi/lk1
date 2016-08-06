using System;
using System.Text;
using System.Collections.Generic;
using lk2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new int[] {-2, -1, 0, 3, 5, 6, 7, 9, 13, 14, 4};

            Class2.printPairSums(list, 4);
        }
    }
}
