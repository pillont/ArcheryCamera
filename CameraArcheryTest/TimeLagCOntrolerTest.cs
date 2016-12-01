using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using System.Threading;

namespace CameraArcheryTest
{
    [TestClass]
    public class TimeLagControllerTest
    {
        TimeLagController timeController;
        
        [TestInitialize]
        public void init()
        {
            timeController = new TimeLagController();
        }

        /// <summary>
        /// test the event on new frame
        /// if the load in not done, the images are stocked
        /// if the load in done, the images ar stocked and the firsts is return 
        /// </summary>
        [TestMethod]
        public void TestOnNewFrame()
        {
            Bitmap first = new Bitmap(12, 15);
            Bitmap second = new Bitmap(24, 56);
            Bitmap third= new Bitmap(48, 69);

            // add an image
            var res = timeController.OnNewFrame(first);
            Assert.IsNull(res);

            // add other image
            res = timeController.OnNewFrame(second);
            Assert.IsNull(res);

            // change the lagLoadFeedBackController.IsLoad to true!
            var time = 5;
            SettingFactory.CurrentSetting.Time = time;
            timeController.lagLoadFeedBackController.StartLoad();
            Thread.Sleep((time + 1) * 1000);

            // get the first when add new image
            res = timeController.OnNewFrame(third);
            Assert.IsNotNull(res);
            Assert.IsTrue(res == first);
        }
    }
}
