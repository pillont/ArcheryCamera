using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Utils;
using System.IO;

namespace CameraArcheryTest
{
    [TestClass]
    public class LogHelperTest
    {
        [TestInitialize]
        public void Init()
        {
            if (File.Exists(LogHelper.PathLogFile))
                File.Delete(LogHelper.PathLogFile);
        }

        /// <summary>
        /// function must create file if no exist
        /// function write in the file : contains the date, the time and the text
        /// </summary>
        [TestMethod]
        public void TestWrite()
        {
            LogHelper.Write("test");

            //create file if no exist
            Assert.IsTrue(File.Exists(LogHelper.PathLogFile));

            //check the line
            var lines = File.ReadAllLines(LogHelper.PathLogFile);
            Assert.IsTrue(lines.Length == 1);
            Assert.IsTrue(lines[0].Contains(DateTime.Now.ToLongDateString()));
            Assert.IsTrue(lines[0].Contains(DateTime.Now.ToLongTimeString()));
            Assert.IsTrue(lines[0].Contains("test"));



            LogHelper.Write("test2");

            //create file if no exist
            Assert.IsTrue(File.Exists(LogHelper.PathLogFile));

            //check the line
            lines = File.ReadAllLines(LogHelper.PathLogFile);
            Assert.IsTrue(lines.Length == 2);
            // first log not changed
            Assert.IsTrue(lines[0].Contains(DateTime.Now.ToLongDateString()));
            Assert.IsTrue(lines[0].Contains(DateTime.Now.ToLongTimeString()));
            Assert.IsTrue(lines[0].Contains("test"));
            // second log add
            Assert.IsTrue(lines[1].Contains(DateTime.Now.ToLongDateString()));
            Assert.IsTrue(lines[1].Contains(DateTime.Now.ToLongTimeString()));
            Assert.IsTrue(lines[1].Contains("test2"));

        }

        [TestMethod]
        public void TestError()
        {
            /*
             * without stack trace
             */
            LogHelper.Error(new Exception("test"));

            //create file if no exist
            Assert.IsTrue(File.Exists(LogHelper.PathLogFile));

            //check the line
            var lines = File.ReadAllLines(LogHelper.PathLogFile);
            Assert.IsTrue(lines.Length == 3);
            Assert.IsTrue(lines[0] == LogHelper.ErrorHeader);
            Assert.IsTrue(lines[1] == "test");
            Assert.IsTrue(lines[2] == "");

            /*
             * with stack trace
             */
            try
            {
                throw new Exception("test2");
            }
            // use try / catch to init the stackTrace
            catch (Exception e)
            {
                LogHelper.Error(e);

                //create file if no exist
                Assert.IsTrue(File.Exists(LogHelper.PathLogFile));

                //check the line
                lines = File.ReadAllLines(LogHelper.PathLogFile);
                Assert.IsTrue(lines.Length == 6);
                Assert.IsTrue(lines[0] == LogHelper.ErrorHeader);
                Assert.IsTrue(lines[1] == "test");
                Assert.IsTrue(lines[2] == "");
                Assert.IsTrue(lines[3] == LogHelper.ErrorHeader);
                Assert.IsTrue(lines[4] == "test2");
                Assert.IsTrue(lines[5] == e.StackTrace);
            }
        }
    }
}
