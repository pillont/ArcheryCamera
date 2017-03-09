using Microsoft.VisualStudio.TestTools.UnitTesting;
using CameraArcheryLib.Factories;
using System.IO;
using CameraArcheryLib;
using CameraArcheryLib.Utils;
using CameraArcheryLib.Models;

namespace CameraArcheryTest
{
    [TestClass]
    public class SerializeHelperTest
    {
        /// <summary>
        ///  test the serialization and deserialization
        ///  test without file and write on an existed file
        /// </summary>
        [TestMethod]
        public void TestSerialization()
        {
            // without file
            if (File.Exists(SettingFactory.FilePath))
                File.Delete(SettingFactory.FilePath);

            var setting = SettingFactory.DefaultSetting;
            setting.Language = LanguageController.Languages.Français;
            setting.Time = 156;

            SerializeHelper.Serialization<Setting>(setting, SettingFactory.FilePath);
            var res = SerializeHelper.Deserialization<Setting>(SettingFactory.FilePath);

            Assert.IsTrue(res == setting);

            // write on an existed file
            setting.Language = LanguageController.Languages.English;
            setting.Time = 185;

            SerializeHelper.Serialization<Setting>(setting, SettingFactory.FilePath);
            res = SerializeHelper.Deserialization<Setting>(SettingFactory.FilePath);

            Assert.IsTrue(res == setting);
        }
    }
}
