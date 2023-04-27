using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace MP3_Unit_Testing
{
    [TestClass]
    public class UnitTest1
    {
        private const string Expected = "Hello World!";
        [TestMethod]
        public void TestMethod1()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                //MP3_EE_EA.MP3_functions.

                var result = sw.ToString().Trim();
                Assert.AreEqual(Expected, result);
            }
        }
    }
}
