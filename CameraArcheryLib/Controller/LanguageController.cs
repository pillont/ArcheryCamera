using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace CameraArcheryLib
{
    /// <summary>
    /// helper to use the String ressources
    /// </summary>
    public static class LanguageController
    {
        /// <summary>
        /// event if error during the find of the dictionnaries
        /// </summary>
        public static event Action<Exception> OnErrorFindDictionary;

        /// <summary>
        /// language possible
        /// </summary>
        public enum Languages { Français, English };

        /// <summary>
        /// Uri of the french dictionnary
        /// </summary>
        public const string FrUri = @"Ressources/Strings/StringResources.fr.xaml";

        /// <summary>
        /// Uri of the default dictionnary
        /// </summary>
        public const string DefaultUri = @"Ressources/Strings/StringResources.xaml";

        /// <summary>
        /// function to get the current dictionnary
        /// </summary>
        public static ResourceDictionary CurrentDictionnary
        {
            get
            {
                ResourceDictionary dict = new ResourceDictionary();

                try
                {
                    dict.Source = GetDictionaryPath();
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                    if (OnErrorFindDictionary != null)
                        OnErrorFindDictionary(e);
                    else
                        throw e;
                }
                return dict;
            }
        }

        public static Uri GetDictionaryPath()
        {
            Uri uri;
            var dir = Directory.GetCurrentDirectory();
            // found the good dictionnary
            if (SettingFactory.CurrentSetting.Language == Languages.Français)
                uri = new Uri(dir + @"/" + FrUri, UriKind.Absolute);
            else
                uri = new Uri(dir + @"/" + DefaultUri, UriKind.Absolute);

            LogHelper.Write(uri.OriginalString);
            return uri;
        }

        /// <summary>
        /// init a language in the collection of dictionnary
        /// </summary>
        /// <param name="collection"></param>
        public static void InitLanguage(Collection<ResourceDictionary> collection)
        {
            collection.Clear();
            collection.Add(CurrentDictionnary);
        }

        /// <summary>
        /// get the ressource associate to the key arg
        /// </summary>
        /// <param name="key">key of the wanted ressource</param>
        /// <returns>wanted ressource</returns>
        public static string Get(string key)
        {
            var st = CurrentDictionnary[key] as string;
            return st;
        }
    }
}
