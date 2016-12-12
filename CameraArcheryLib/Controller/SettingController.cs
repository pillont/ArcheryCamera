using CameraArcheryLib.Factories;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Windows;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    /// Controller of the setting view
    /// </summary>
    public static class SettingController
    {
        /// <summary>
        /// Function to save the new setting
        /// <para>get current setting</para>
        /// <para>try to save the new values</para>
        /// <para>catch exception to incorrect Input</para>
        /// <para>serialize the setting</para>
        /// </summary>
        /// <returns>bool to inform if the new values is saved</returns>
        public static void SaveSetting(int time, LanguageController.Languages language, int frameRate)
        {
            // get current setting
            Setting values = SettingFactory.CurrentSetting;

            LogHelper.Write("Current setting " + values);
            LogHelper.Write("New time " + time);
            LogHelper.Write("New Language " + language);
            
            //convert the new values
            values.Time = Convert.ToInt32(time);
            values.Language = language;
            values.Frame = frameRate;


            // save the setting
            SerializeHelper.Serialization<Setting>(values, SettingFactory.FilePath);
            LogHelper.Write("New setting" + values.ToString());
        }

        /// <summary>
        /// change the uri on the setting
        /// </summary>
        /// <param name="uri">new value of the uri -> must is an existing directory</param>
        public static void UpdateUri(Uri uri)
        {
            Contract.EnsuresOnThrow<ArgumentException>(Directory.Exists(uri.AbsolutePath), "folder not existing");

            Setting values = SettingFactory.CurrentSetting;

            values.VideoFolder = uri.OriginalString;
            SerializeHelper.Serialization<Setting>(values, SettingFactory.FilePath);
         }
    }
}   
