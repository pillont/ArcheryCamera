using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

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

            LogHelper.Write("video directory change : " + selectedUri);
        }

        #endregion event

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

        #endregion databinding
    }
}
