using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Controller;
using System.Threading.Tasks;
using CameraArcheryLib.Factories;
using System.Threading;
using System.Windows;

namespace CameraArcheryTest
{
    [TestClass]
    public class LagLoadFeedBackControllerTest
    {
        LagLoadFeedBackController Controller;
        
        [TestInitialize]
        public void test()
        {
            Controller = new LagLoadFeedBackController();
        }

        /// <summary>
        /// wait the time in the setting to make a lag
        /// after, the visuals properties change
        /// </summary>
        [TestMethod]
        public void TestStartLoad()
        {
            var time = 5;
            SettingFactory.CurrentSetting.Time = time;

            //initial state
            Assert.IsFalse(Controller.IsLoad);
            Assert.IsTrue(Controller.Visibility == Visibility.Visible);
            Assert.IsTrue(Controller.Progress == 0);
            
            // start load
            new Task(() => Controller.StartLoad()).Start();
            Thread.Sleep((time+1) * 1000);

            // after load
            Assert.IsTrue(Controller.IsLoad);
            Assert.IsTrue(Controller.Visibility == Visibility.Collapsed);
            Assert.IsTrue(time - 1 <= Controller.Progress, "time = " + time + " progress = "+Controller.Progress);
        }
    }
}
