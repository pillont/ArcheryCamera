using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CameraArcheryLib.Controller;
using System.Collections.Generic;

namespace CameraArcheryTest.Controller
{
    [TestClass]
    public class ListRecordControllerTest
    {
        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("test");

            // create test\\file1.avi
            using (StreamWriter w = File.AppendText("test\\file1.avi"))
            { w.Write(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()); }

            // create test\\file2.avi
            using (StreamWriter w = File.AppendText("test\\file2.avi"))
            { w.Write(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()); }
        }

        [TestCleanup]
        public void Clean()
        {
            Directory.Delete("test", true);
        }

        [TestMethod]
        public void TestGetList()
        {
            // get file
            var res = ListRecordController.GetList("test");

            Assert.IsTrue(res.Count == 2);

            //check first file
            Assert.IsTrue(res[0].IsEditing == false);
            Assert.IsTrue(res[0].Name == "file2.avi");
            Assert.IsTrue(res[0].Uri.EndsWith("test\\file2.avi"));

            //check second file
            Assert.IsTrue(res[1].IsEditing == false);
            Assert.IsTrue(res[1].Name == "file1.avi");
            Assert.IsTrue(res[1].Uri.EndsWith("test\\file1.avi"));
        }

        [TestMethod]
        public void TestGetVideoFileNames()
        {
            var res = new List<String>(ListRecordController.GetVideoFileNames("test"));

            Assert.IsTrue(res.Count == 2);

            //check first file
            Assert.IsTrue(res[0].EndsWith("test\\file1.avi"));

            //check second file
            Assert.IsTrue(res[1].EndsWith("test\\file2.avi"));
        }
    }
}
