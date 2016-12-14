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
        /// <summary>
        /// event to dataBinding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// color of the text in the browse
        /// initial color = black
        /// </summary>
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
        private Brush foreground = Brushes.Black;

        /// <summary>
        /// selected uri of the browser
        /// </summary>
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
        private Uri selectedUri;

        /// <summary>
        /// file name of the selected file
        /// </summary>
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
        public string fileName;

        /// <summary>
        /// ctor
        /// </summary>
        public CustomBrowser()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region event
        /// <summary>
        /// show the dialog 
        /// <para>if no selected uri => init to the current directory</para>
        /// <para>open a FolderBrowserDialog</para>
        /// <para>change the value of the selectedUri</para>
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUri == null)
                SelectedUri = new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute);

            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = SettingFactory.CurrentSetting.VideoFolder;

            var result = dialog.ShowDialog();
            var newUri = new Uri(dialog.SelectedPath, UriKind.Absolute);

            SelectedUri = newUri;
        }
        #endregion

        #region databinding
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
        #endregion
        
    }
}
