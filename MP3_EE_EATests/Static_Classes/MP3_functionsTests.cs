using Microsoft.VisualStudio.TestTools.UnitTesting;
using MP3_EE_EA.Models;
using MP3_EE_EA.Static_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3_EE_EA.Static_Classes.Tests
{
    [TestClass()]
    public class MP3_functionsTests
    {
        [TestMethod()]
        public void TimeToString_Test()
        {

            var result = MP3_functions.TimeToString(-2000);
            var result2 = MP3_functions.TimeToString(2000);
            var result3 = MP3_functions.TimeToString(4000);


            Assert.AreEqual("00:00",result);
            Assert.AreEqual("33:20",result2);
            Assert.AreEqual("01:06:40", result3);

        }

        [TestMethod()]
        public void Get_MP3_Folder_Test()
        {
            var folder = List_Helper.TryGetMp3Folder();

            Assert.IsNotNull(folder);


            string name = folder.Name;

            Assert.AreEqual("Mp3_Files", name);

        }





    }
}