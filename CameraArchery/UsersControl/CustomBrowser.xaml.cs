using CameraArchery.View;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class CustomBrowser : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Brush foreground;
        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
                OnPropertyChanged("Foreground");
            }
        }

        /// <summary>
        /// event to dataBinding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private string defaultFile;
        public string DefaultFile
        {
            get
            {
                return defaultFile;
            }
            set
            {
                defaultFile = value;
                SelectedUri = DefaultFile;
            }
        }


        private string selectedUri;
        public string SelectedUri 
        { 
            get
            {
                return selectedUri;
            }
            set
            {
                selectedUri = value;
                OnPropertyChanged("SelectedUri");
                FileName = selectedUri.Split('\\').Last();    
            }
        }

        public string fileName;
        public string FileName
        {
            get 
            { 
                return fileName;
            }
            set 
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }


        public CustomBrowser()
        {
            InitializeComponent();
            this.DataContext = this;
            Foreground = Brushes.Black;
        }

        private void ShowDialog()
        {
            var root = new Uri(@"" + LanguageController.Get("videoFolder"), UriKind.Relative);
            var initUri = new Uri(@"" + SettingFactory.CurrentSetting.VideoFolder, UriKind.Relative);
            
            var dialog = new CustomBrowserFolder(root, initUri);
            dialog.ShowDialog();

            var uriString = dialog.SelectedUri.OriginalString;
            if(SettingFactory.CurrentSetting.VideoFolder != uriString)
                SettingController.UpdateUri(uriString);

            SelectedUri = uriString;
        }
        
        /// <summary>
        /// function to dataBinding
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog();
        }

    }
}
