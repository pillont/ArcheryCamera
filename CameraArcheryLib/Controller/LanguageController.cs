using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

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
        public const string FrUri = @"../Ressources/Strings/StringResources.fr.xaml";
        
        /// <summary>
        /// Uri of the default dictionnary
        /// </summary>
        public const string DefaultUri = @"../Ressources/Strings/StringResources.xaml";

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
                    // found the good dictionnary
                    if (SettingFactory.CurrentSetting.Language == Languages.Français)
                        dict.Source = new Uri(FrUri, UriKind.Relative);
                    else
                        dict.Source = new Uri(DefaultUri, UriKind.Relative);
                }
                catch(Exception e)
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
