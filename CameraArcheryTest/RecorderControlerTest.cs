using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Controller;
using System.IO;

namespace CameraArcheryTest
{
    [TestClass]
    public class RecorderControllerTest
    {
        RecorderController Controller;

        [TestInitialize]
        public void init()
        {
            Controller = new RecorderController();
        }

        [TestMethod]
        public void TestStartRecording()
        {
            //delete dir
            if (System.IO.Directory.Exists("video"))
                System.IO.Directory.Delete("video", true);
          
            Assert.IsFalse(System.IO.Directory.Exists("video"));
            SettingFactory.InitSetting();
            Assert.IsTrue(SettingFactory.CurrentSetting.VideoNumber == 0);

            //fct
            //Controller.StartRecording();
            //Controller.StopRecording();
            
            //new file
            Assert.IsTrue(File.Exists("video/0.avi"));

            //local setting
            Assert.IsTrue(SettingFactory.CurrentSetting.VideoNumber == 1, "number = " + SettingFactory.CurrentSetting.VideoNumber);
            //save setting
            SettingFactory.RefreshSettingValue();
            Assert.IsTrue(SettingFactory.CurrentSetting.VideoNumber == 1);
        }
    }
}
