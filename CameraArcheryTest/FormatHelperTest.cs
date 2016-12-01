using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Utils;

namespace CameraArcheryTest
{
    [TestClass]
    public class FormatHelperTest
    {
        /// <summary>
        /// test function to know if the string is a double or not
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]  
        public void TestIsTextNumeric()
        {
            // good format
            Assert.IsTrue(FormatHelper.IsNumericText("1"));
            Assert.IsTrue(FormatHelper.IsNumericText("2,3"));
            Assert.IsTrue(FormatHelper.IsNumericText("2,"));
            Assert.IsTrue(FormatHelper.IsNumericText(",45"));
            Assert.IsTrue(FormatHelper.IsNumericText(" 6789 "));
            Assert.IsTrue(FormatHelper.IsNumericText(" 67 89 "));
            Assert.IsTrue(FormatHelper.IsNumericText(" 67  "));

            // other format
            Assert.IsFalse(FormatHelper.IsNumericText("2,0,"));
            Assert.IsFalse(FormatHelper.IsNumericText("1,0,0"));
            Assert.IsFalse(FormatHelper.IsNumericText("1.0"));
            Assert.IsFalse(FormatHelper.IsNumericText("a01"));
            Assert.IsFalse(FormatHelper.IsNumericText("/01"));

            // must not null
            FormatHelper.IsNumericText(null);
        }

        /// <summary>
        /// test function to know if the string is a double or not
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIsTextNumericInteger()
        {
            // good format
            Assert.IsTrue(FormatHelper.IsTextNumericInteger("1"));
            Assert.IsTrue(FormatHelper.IsTextNumericInteger(" 6789 "));

            //decimal format 
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("2,"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("2,3"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("2,3,"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger(",45"));


            // other format
            Assert.IsFalse(FormatHelper.IsTextNumericInteger(" 67 89 "));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("1,0,0"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("1.0"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("a01"));
            Assert.IsFalse(FormatHelper.IsTextNumericInteger("/01"));

            // must not null
            FormatHelper.IsTextNumericInteger(null);
        }
    }
}
