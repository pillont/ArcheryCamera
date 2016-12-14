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

        private Uri selectedUri;
        public Uri SelectedUri 
        { 
            get
            {
                return selectedUri;
            }
            set
            {
                selectedUri = value;
                OnPropertyChanged("SelectedUri");

                FileName = selectedUri.OriginalString.Split('\\').Last();
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

        public Uri Root { get; set; }
        private void ShowDialog()
        {
            if (Root == null)
                Root = new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute);
            
            if (SelectedUri == null)
                SelectedUri = new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute);

            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = SettingFactory.CurrentSetting.VideoFolder;

            var result = dialog.ShowDialog();
            var newUri = new Uri(dialog.SelectedPath, UriKind.Absolute);

            if(SettingFactory.CurrentSetting.VideoFolder != newUri.OriginalString)
                SettingController.UpdateUri(newUri);

            SelectedUri = newUri;
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
