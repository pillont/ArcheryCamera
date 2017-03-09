using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Factories;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using System.IO;
using CameraArcheryLib.Models;

namespace CameraArcheryTest
{
    [TestClass]
    public class SettingFactoryTest
    {
        [TestInitialize]
        public void Init()
        {
            SettingFactory.RefreshSettingValue();
        }

        /// <summary>
        /// test the default setting
        /// </summary>
        [TestMethod]
        public void TestDefaultSetting()
        {
            var setting = SettingFactory.DefaultSetting;
            Assert.IsTrue(setting.Language == LanguageController.Languages.English);
            Assert.IsTrue(setting.Time == 5, "time = " + setting.Time);
            Assert.IsTrue(setting.VideoNumber == 0);
        }

        /// <summary>
        /// if no file of setting, return default
        /// if file, get value save
        /// if value save
        /// </summary>
        [TestMethod]
        public void TestCurrentSetting()
        {
            // no file return default
            if (File.Exists(SettingFactory.FilePath))
                File.Delete(SettingFactory.FilePath);
            Assert.IsTrue(SettingFactory.CurrentSetting == SettingFactory.DefaultSetting);

            //same value if value already load
            SettingController.SaveSetting(10, LanguageController.Languages.Français, 15);
            Assert.IsTrue(SettingFactory.CurrentSetting == SettingFactory.DefaultSetting);

            // refresh value and get value save
            SettingFactory.RefreshSettingValue();
            Assert.IsTrue(SettingFactory.CurrentSetting ==
                            new Setting(10, LanguageController.Languages.Français, 15));
        }

        [TestMethod]
        public void TestInitSetting()
        {
            SettingController.SaveSetting(10, LanguageController.Languages.Français, 10);
            SettingFactory.InitSetting();
            Assert.IsTrue(SettingFactory.CurrentSetting == SettingFactory.DefaultSetting);
        }
    }
}
