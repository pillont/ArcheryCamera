using CameraArchery.View;
using CameraArcheryLib;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CameraArchery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App ()
	    {
            SettingFactory.OnErrorFindSetting += SettingFactory_OnErrorFindSetting;
            LanguageController.OnErrorFindDictionary += LanguageController_OnErrorFindDictionary;
	    }

        void LanguageController_OnErrorFindDictionary(Exception e)
        {
            MessageBox.Show("Dictionnary not found, thanks to contact the IT service", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(5);
        }

        void SettingFactory_OnErrorFindSetting(Exception e)
        {
            // inform the user
            MessageBox.Show(LanguageController.Get("SettingErrorMessage"), LanguageController.Get("SettingError"), MessageBoxButton.OK, MessageBoxImage.Information);
            LogHelper.Write("Init to new  default setting");
        }
    }
}
