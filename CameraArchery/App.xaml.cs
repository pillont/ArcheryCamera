using CameraArcheryLib;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Windows;
using System.Windows.Threading;

namespace CameraArchery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SettingFactory.OnErrorFindSetting += SettingFactory_OnErrorFindSetting;
            LanguageController.OnErrorFindDictionary += LanguageController_OnErrorFindDictionary;

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "error");

            LogHelper.Error(e.Exception);
            throw e.Exception;
        }

        private void LanguageController_OnErrorFindDictionary(Exception e)
        {
            MessageBox.Show("Dictionnary not found, thanks to contact the IT service", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(5);
        }

        private void SettingFactory_OnErrorFindSetting(Exception e)
        {
            // inform the user
            MessageBox.Show(LanguageController.Get("SettingErrorMessage"), LanguageController.Get("SettingError"), MessageBoxButton.OK, MessageBoxImage.Information);
            LogHelper.Write("Init to new  default setting");
        }
    }
}
