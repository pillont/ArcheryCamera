using CameraArcheryLib;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CameraArchery.VisualModel
{
    public class CustomMenuItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Func<string, bool> OnStopEditing { get; set; }

        public Uri Uri { get; private set; }

        public bool isExpanded;
        public bool IsExpanded 
        {
            get
            {
                return isExpanded;
            }
            set 
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public bool isSelected;
        public bool IsSelected 
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");

            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    try
                    {
                        var newUri = new Uri(Directory.GetParent(Uri.OriginalString).FullName + "\\" + value);


                        if (Directory.Exists(newUri.OriginalString))
                        {
                            MessageBox.Show(LanguageController.Get("fileExisting"), LanguageController.Get("fileExistingCaption"), MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        Directory.Move(Uri.OriginalString, newUri.OriginalString);
                        name = value;
                        Uri = newUri;
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                        return;
                    }
                }

            }
        }
        public string name;

        public ObservableCollection<CustomMenuItem> Items { get; set; }

        public CustomMenuItem(Uri uri)
        {
            Uri = uri;

            this.Items = new ObservableCollection<CustomMenuItem>();
            IsSelected = false;
            IsExpanded = false;

            // use field to not update the uri
            name = uri.OriginalString.Split('\\').Last();
            OnStopEditing = (arg) => true;
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
    }
}
