using System;
using System.IO;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using static System.Environment;

namespace CameraArcheryLib.Factories
{
    /// <summary>
    /// factory of a setting
    /// </summary>
    public class SettingFactory
    {
        public const int SecondDefault = 5;
        public const LanguageController.Languages LanguageDefault = LanguageController.Languages.Français;

        public delegate void ErrorFindSettingDel(Exception e);

        /// <summary>
        /// Event during error to find file, not done if file not found
        /// </summary>
        public static event ErrorFindSettingDel OnErrorFindSetting;

        /// <summary>
        /// path of the setting path
        /// </summary>
        public static string FilePath => Environment.GetFolderPath(SpecialFolder.MyDocuments) + "/" + FileName;

        public const string FileName = @"setting.xml";

        /// <summary>
        /// default setting
        /// </summary>
        public static Setting DefaultSetting
        {
            get
            {
                if (defaultSetting == null)
                    defaultSetting = new Setting(SecondDefault, LanguageDefault) { VideoNumber = 0 };

                return defaultSetting;
            }
        }

        private static Setting defaultSetting;

        /// <summary>
        /// current setting
        /// </summary>
        public static Setting CurrentSetting
        {
            get
            {
                // get the last setting if exist
                if (current != null)
                    return current;

                // get values in the file is exist
                try
                {
                    current = SerializeHelper.Deserialization<Setting>(FilePath);
                }
                // if error during the read of the file
                // init to the default file and set new setting file
                catch (FileNotFoundException)
                {
                    InitSetting();
                    return current;
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                    InitSetting();

                    if (OnErrorFindSetting != null)
                        OnErrorFindSetting(e);
                }

                LogHelper.Write("Deserialization of setting : " + current);
                return current;
            }
        }

        private static Setting current;

        /// <summary>
        ///set default setting
        /// </summary>
        public static void InitSetting()
        {
            current = DefaultSetting;
            SerializeHelper.Serialization<Setting>(current, SettingFactory.FilePath);
        }

        public static void RefreshSettingValue()
        {
            current = null;
            defaultSetting = null;
        }
    }
}
