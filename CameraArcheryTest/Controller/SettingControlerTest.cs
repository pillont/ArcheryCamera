﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Utils;
using CameraArcheryLib.Models;

namespace CameraArcheryTest
{
    [TestClass]
    public class SettingControllerTest
    {
        /// <summary>
        /// test the save, save two values and test to deserialise the value to compare
        /// </summary>
        [TestMethod]
        public void TestSaveSetting()
        {
            // first value
            SettingController.SaveSetting(12, LanguageController.Languages.English, 6);

            var setting = SerializeHelper.Deserialization<Setting>(SettingFactory.FilePath);
            Assert.IsTrue(setting.Frame == 6);
            Assert.IsTrue(setting.Time == 12);
            Assert.IsTrue(setting.Language == LanguageController.Languages.English);

            // second value
            SettingController.SaveSetting(24, LanguageController.Languages.Français, 10);

            setting = SerializeHelper.Deserialization<Setting>(SettingFactory.FilePath);
            Assert.IsTrue(setting.Frame == 10);
            Assert.IsTrue(setting.Time == 24);
            Assert.IsTrue(setting.Language == LanguageController.Languages.Français);
        }
    }
}
