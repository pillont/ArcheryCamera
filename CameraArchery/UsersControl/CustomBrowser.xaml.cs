using CameraArchery.View;
using CameraArcheryLib;
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
                fileName = DefaultFile;
            }
        }


        private string fileName;
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
            new CustomBrowserFolder(new Uri(@""+LanguageController.Get("videoFolder"), UriKind.Relative)).ShowDialog();
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
