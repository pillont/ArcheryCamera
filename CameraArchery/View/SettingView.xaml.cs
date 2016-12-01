using CameraArcheryLib;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CameraArchery.View
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : Window
    {
        /// <summary>
        ///  bool to know if the setting change is save or not
        /// </summary>
        public bool IsChange { get; set; }

        /// <summary>
        /// ctor
        /// <para>init the language</para>
        /// <para>init the view</para>
        /// <para>add the possibility in the list of languages</para>
        /// <para>get the current setting and set in the view</para>
        /// </summary>
        public SettingView()
        {
            IsChange = false;
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            LogHelper.Write("------Open the setting view-----");

            InitializeComponent();

            // init icon 
            Uri iconUri = new Uri(@"../../Ressources/logoSetting.ico", UriKind.Relative);
           // this.Icon = BitmapFrame.Create(iconUri);


            LanguageComboBox.ItemsSource = Enum.GetValues(typeof(LanguageController.Languages)).Cast<LanguageController.Languages>();
            GetSetting();
        }

        /// <summary>
        /// get the current setting and set the values in the windows
        /// </summary>
        private void GetSetting()
        {
            var values = SettingFactory.CurrentSetting;
            LogHelper.Write("Current setting " + values);
            
            Spliter.Value = values.Time;
            LanguageComboBox.SelectedItem = values.Language;
            FrameInput.Text = values.Frame.ToString();
       }

        /// <summary>
        /// event to close the windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        ///  save the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingController.SaveSetting(
                                            Convert.ToInt32(Spliter.Value),
                                            (LanguageController.Languages)LanguageComboBox.SelectedItem, Int32.Parse(FrameInput.Text));
                IsChange = true;
                this.Close();
            }
            catch (Exception ee)
            {
                IsChange = false;
                LogHelper.Error(ee);
                MessageBox.Show("Error", "IncorrectInput", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        /// <summary>
        /// event when the windows is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            LogHelper.Write("-----Close setting view-----");
        }
    }
}
